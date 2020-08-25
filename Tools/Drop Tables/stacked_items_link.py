import json
from osrsbox import items_api


def create_dict():
    all_db_items = items_api.load()

    stacked_items_link = dict()
    stack_count = dict()
    meta_link = dict()

    for item in all_db_items:
        if item.linked_id_item is not None and item.stacked is not None:
            linked_id = item.linked_id_item

            if linked_id not in meta_link:
                if linked_id > item.id:
                    meta_link[linked_id] = item.id
                else:
                    meta_link[linked_id] = linked_id
            correct_linked_id = meta_link[linked_id]
            if correct_linked_id in stack_count:
                if item.stacked > stack_count[correct_linked_id]:
                    stacked_items_link[correct_linked_id] = item.id
                    stack_count[correct_linked_id] = item.stacked
            else:
                stacked_items_link[correct_linked_id] = item.id
                stack_count[correct_linked_id] = item.stacked

    stacked_items_link[617] = 1004  # coins
    stacked_items_link[4561] = 10483  # purple sweets

    return stacked_items_link


def create_json():
    stacked_items = create_dict()
    out_file_name = r"./stacked_items_link.json"
    with open(out_file_name, "w", newline="\n") as out_file:
        json.dump(stacked_items, out_file, indent=4)


create_json()

