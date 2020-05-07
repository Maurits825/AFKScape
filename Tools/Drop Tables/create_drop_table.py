import pandas as pd
import requests
import re
from osrsbox import items_api
import numpy as np
import click
import json


class ItemDrop:
    def __init__(self, name, amount_min, amount_max, actual_chance, base_chance):
        self.name = name
        self.amount_min = amount_min
        self.amount_max = amount_max
        self.actual_chance = actual_chance
        self.base_chance = base_chance


def format_table_header(raw, start, end):
    start_i = raw.index(start)+1
    if end:
        end_i = raw.index(end)+1
    else:
        end_i = -1
    return raw[start_i:end_i]


def get_rarity(raw):
    if raw == "Always":
        return 1, 1
    else:
        raw_no_comma = raw.replace(',', '')
        chances = re.findall(r'([0-9]*)/([0-9]*)', raw_no_comma)
        return chances[0][0], chances[0][1]


def print_parsed_data(drops):
    for drop in drops:
        print(drop.name + ", " + str(drop.amount_min) + "-" + str(drop.amount_max) +
              ", " + str(drop.actual_chance) + "/" + str(drop.base_chance))


def get_data_frames(url, start="Drops", end="Tertiary"):
    header = {
        "User-Agent": "Mozilla/5.0"
    }

    req = requests.get(url, headers=header)

    table_pattern = re.compile(r'class="mw-headline" id="(.*?)"')
    table_headers_raw = table_pattern.findall(req.text)
    if start == "all":
        table_headers = table_headers_raw
    else:
        table_headers = format_table_header(table_headers_raw, start, end)

    return pd.read_html(req.text), table_headers


def process_data_frame(frame, item_drop_list):
    for row in frame.itertuples():
        item_name = row.Item

        quantity = str(row.Quantity).replace(',', '')
        amounts = re.findall(r'([0-9]+)', str(quantity))
        if len(amounts) == 1:
            min_amount = amounts[0]
            max_amount = amounts[0]
        elif len(amounts) == 2:
            min_amount = amounts[0]
            max_amount = amounts[1]
        elif item_name == "Nothing":
            item_name = "Coins"
            min_amount = 0
            max_amount = 0
        else:
            a = 5

        chance, base = get_rarity(row.Rarity)
        item_drop_list.append(ItemDrop(item_name, min_amount, max_amount, chance, base))


def check_table_header(header, exclude):
    for header_exclude in exclude:
        if header == header_exclude:
            return False
    return True


def get_drop_table(url, exclude):
    dfs, table_headers = get_data_frames(url)

    default_exclude = ["Rare_drop_table", "Tertiary", "Unique_drop_table", "Unique_drop_table",
                       "Mutagens", "100%", "100%_drops"]
    all_exclude = default_exclude + exclude
    item_drop_list = []
    header_ind = 0
    for data_frame in dfs:
        try:
            data_frame.iloc[0].loc['Item']
        except KeyError:
            pass
        else:
            if check_table_header(table_headers[header_ind], all_exclude):
                process_data_frame(data_frame, item_drop_list)
            header_ind = header_ind + 1

    return item_drop_list


def get_single_table(url, table_id, start_table):
    dfs, table_headers = get_data_frames(url, start_table, None)
    item_drop_list = []
    header_ind = 0
    for data_frame in dfs:
        try:
            data_frame.iloc[0].loc['Item']
        except KeyError:
            pass
        else:
            if table_headers[header_ind] == table_id:
                process_data_frame(data_frame, item_drop_list)
            header_ind = header_ind + 1

    return item_drop_list


def create_json(drops):
    all_db_items = items_api.load()

    with open(r"./stacked_items_link.json") as json_file:
        stacked_dict = json.load(json_file)

    actual_chance = []
    base_chances = []
    ids = []

    json_data = dict()
    json_data["name"] = "general"
    #json_data["indexMapping"] = []

    for drop in drops:
        actual_chance.append(drop.actual_chance)
        base_chances.append(drop.base_chance)
        ids.append(all_db_items.lookup_by_item_name(drop.name).id)

    actual_chance_arr = np.array(actual_chance).astype(np.int)
    base_chance_arr = np.array(base_chances).astype(np.int)

    lcm = np.lcm.reduce(base_chance_arr)
    base_chance = int(lcm)
    json_data["baseChance"] = base_chance
    scaled_chances = (actual_chance_arr * (lcm / base_chance_arr))

    json_data["basicLoots"] = []
    index_mapping = 0
    for i, drop in enumerate(drops):
        index_mapping = index_mapping + scaled_chances[i]

        basic_loot = dict()
        try:
            basic_loot["id"] = stacked_dict[str(ids[i])]
        except KeyError:
            basic_loot["id"] = ids[i]
        basic_loot["weight"] = scaled_chances[i]
        basic_loot["amountMin"] = drop.amount_min
        basic_loot["amountMax"] = drop.amount_max
        json_data["basicLoots"].append(basic_loot)

        #json_data["indexMapping"].append(index_mapping)

    out_file_name = r"./temp.json"
    with open(out_file_name, "w", newline="\n") as out_file:
        json.dump(json_data, out_file, indent=4)


@click.command()
@click.option('--name', '-b', help='Name of wiki page')
@click.option('--exclude', '-e', help='Exclude table', multiple=True)
@click.option('--table_type', default="boss", help='Boss drop table or single table')
@click.option('--table_name', '-t', help='Table name of single table')
@click.option('--start_table', '-s', default="all", help='Start table (exclusive)')
def json_from_boss(name, exclude, table_type, table_name, start_table):
    url = r"https://oldschool.runescape.wiki/w/" + name
    if table_type == "boss":
        drop_list = get_drop_table(url, list(exclude))
    elif table_type == "single":
        drop_list = get_single_table(url, table_name, start_table)
    else:
        raise ValueError
    print_parsed_data(drop_list)
    create_json(drop_list)


if __name__ == '__main__':
    json_from_boss()


#drop_list = get_drop_table(r"https://oldschool.runescape.wiki/w/Zulrah")
#print_parsed_data(drop_list)
#create_json(drop_list)
