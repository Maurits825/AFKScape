import pandas as pd
import requests
import re
from osrsbox import items_api
import numpy as np


class ItemDrop:
    def __init__(self, name, amount_min, amount_max, actual_chance, base_chance):
        self.name = name
        self.amount_min = amount_min
        self.amount_max = amount_max
        self.actual_chance = actual_chance
        self.base_chance = base_chance


def format_table_header(raw):
    start = raw.index("Drops")
    end = raw.index("Tertiary")
    return raw[start+1:end+1]


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


def get_drop_table(url):
    header = {
      "User-Agent": "Mozilla/5.0"
    }

    req = requests.get(url, headers=header)

    table_pattern = re.compile(r'class="mw-headline" id="(.*?)"')
    table_headers_raw = table_pattern.findall(req.text)
    table_headers = format_table_header(table_headers_raw)

    dfs = pd.read_html(req.text)

    item_drop_list = []
    header_ind = 0
    for data_frame in dfs:
        try:
            data_frame.iloc[0].loc['Item']
        except KeyError:
            pass
        else:
            if table_headers[header_ind] != "Rare_drop_table" and table_headers[header_ind] != "Tertiary":
                for row in data_frame.itertuples():
                    item_name = row.Item

                    amounts = re.findall(r'([0-9]+)', str(row.Quantity))
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

                    chance, base = get_rarity(row.Rarity)
                    item_drop_list.append(ItemDrop(item_name, min_amount, max_amount, chance, base))
            header_ind = header_ind + 1

    return item_drop_list


def create_json(drops):
    all_db_items = items_api.load()

    actual_chance = []
    base_chances = []
    ids = []
    for drop in drops:
        actual_chance.append(drop.actual_chance)
        base_chances.append(drop.base_chance)
        ids.append(all_db_items.lookup_by_item_name(drop.name))

    actual_chance_arr = np.array(actual_chance).astype(np.int)
    base_chance_arr = np.array(base_chances).astype(np.int)

    lcm = np.lcm.reduce(base_chance_arr)
    scaled_chances = actual_chance_arr * (lcm / base_chance_arr)
    a=5


drop_list = get_drop_table(r"https://oldschool.runescape.wiki/w/Zulrah")
#print_parsed_data(drop_list)
create_json(drop_list)
#print(item_drop_list)
