import pandas as pd
import requests
import re
from osrsbox import items_api
import numpy as np
import json

nothing_added = False


class ItemDrop:
    def __init__(self, name, amount_min, amount_max, actual_chance, base_chance):
        self.name = name
        self.amount_min = amount_min
        self.amount_max = amount_max
        self.actual_chance = actual_chance
        self.base_chance = base_chance


def format_table_header(raw):
    start = raw.index("Runes_and_ammunition")
    end = raw.index("Mega-rare_drop_table")
    return raw[start:end+1]


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


def get_data_frames(url):
    header = {
        "User-Agent": "Mozilla/5.0"
    }

    req = requests.get(url, headers=header)

    table_pattern = re.compile(r'class="mw-headline" id="(.*?)"')
    table_headers_raw = table_pattern.findall(req.text)
    table_headers = format_table_header(table_headers_raw)

    return pd.read_html(req.text), table_headers


def process_data_frame(frame, item_drop_list, header, ind):
    for row in frame.itertuples():
        item_name = row.Item
        if item_name == "Mega-rare drop table":
            continue

        amounts = re.findall(r'([0-9]+)', str(row.Quantity))
        if len(amounts) == 1:
            min_amount = amounts[0]
            max_amount = amounts[0]
        elif len(amounts) == 2:
            min_amount = amounts[0]
            if amounts[1] == '0':
                max_amount = amounts[0]
            else:
                max_amount = amounts[1]
        elif item_name == "Nothing":
            min_amount = 0
            max_amount = 0

        global nothing_added
        chance, base = get_rarity(row.Rarity)
        if header[ind] == 'Gem_drop_table':
            chance = int(chance) * 20
            base = int(base) * 128

        if item_name == "Rune spear":
            chance = 124
            base = 16384
        elif item_name == "Shield left half":
            chance = 62
            base = 16384
        elif item_name == "Dragon spear":
            chance = 45
            base = 16384
        elif item_name == "Nothing":
            if nothing_added:
                continue
            else:
                nothing_added = True
                item_name = "Coins"
                chance = 2972
                base = 16384

        item_drop_list.append(ItemDrop(item_name, min_amount, max_amount, chance, base))


def get_drop_table(url):
    dfs, table_headers = get_data_frames(url)

    item_drop_list = []
    header_ind = 0
    for data_frame in dfs:
        try:
            data_frame.iloc[0].loc['Item']
        except KeyError:
            pass
        else:
            if table_headers[header_ind] != "Subtables":
                process_data_frame(data_frame, item_drop_list, table_headers, header_ind)
            header_ind = header_ind + 1

    return item_drop_list


def get_single_table(url, table_id):
    dfs, table_headers = get_data_frames(url)
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

    actual_chance = []
    base_chances = []
    ids = []

    json_data = dict()
    json_data["name"] = "Rare drop Table"
    json_data["indexMapping"] = []

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
        basic_loot["id"] = ids[i]
        basic_loot["amountMin"] = drop.amount_min
        basic_loot["amountMax"] = drop.amount_max
        json_data["basicLoots"].append(basic_loot)

        json_data["indexMapping"].append(index_mapping)

    out_file_name = r"./rare_drop_table.json"
    with open(out_file_name, "w", newline="\n") as out_file:
        json.dump(json_data, out_file, indent=4)


drop_list = get_drop_table(r"https://oldschool.runescape.wiki/w/Rare_drop_table")
print_parsed_data(drop_list)
create_json(drop_list)
