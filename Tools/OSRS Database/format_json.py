import json
from osrsbox import items_api

#sed cmds for ref: sed 's/\(.*\)/item_dict[\"\1\"] = item\.\1/'

json_data = dict()
json_data["itemList"] = []
all_db_items = items_api.load()
total = 50000
count = 0
for item in all_db_items:
    item_dict = dict()

    item_dict["id"] = item.id
    item_dict["name"] = item.name
    item_dict["incomplete"] = item.incomplete
    item_dict["members"] = item.members
    item_dict["tradeable"] = item.tradeable
    item_dict["tradeable_on_ge"] = item.tradeable_on_ge
    item_dict["stackable"] = item.stackable
    item_dict["noted"] = item.noted
    item_dict["noteable"] = item.noteable
    item_dict["linked_id_item"] = item.linked_id_item
    item_dict["linked_id_noted"] = item.linked_id_noted
    item_dict["linked_id_placeholder"] = item.linked_id_placeholder
    item_dict["placeholder"] = item.placeholder
    item_dict["equipable"] = item.equipable
    item_dict["equipable_by_player"] = item.equipable_by_player
    item_dict["equipable_weapon"] = item.equipable_weapon
    item_dict["cost"] = item.cost
    item_dict["lowalch"] = item.lowalch
    item_dict["highalch"] = item.highalch
    item_dict["weight"] = item.weight
    item_dict["buy_limit"] = item.buy_limit
    item_dict["quest_item"] = item.quest_item
    item_dict["release_date"] = item.release_date
    item_dict["duplicate"] = item.duplicate
    item_dict["examine"] = item.examine
    item_dict["wiki_name"] = item.wiki_name
    item_dict["wiki_url"] = item.wiki_url

    if item.equipment:
        equipment = dict()
        equipment["attack_stab"] = item.equipment.attack_stab
        equipment["attack_slash"] = item.equipment.attack_slash
        equipment["attack_crush"] = item.equipment.attack_crush
        equipment["attack_magic"] = item.equipment.attack_magic
        equipment["attack_ranged"] = item.equipment.attack_ranged
        equipment["defence_stab"] = item.equipment.defence_stab
        equipment["defence_slash"] = item.equipment.defence_slash
        equipment["defence_crush"] = item.equipment.defence_crush
        equipment["defence_magic"] = item.equipment.defence_magic
        equipment["defence_ranged"] = item.equipment.defence_ranged
        equipment["melee_strength"] = item.equipment.melee_strength
        equipment["ranged_strength"] = item.equipment.ranged_strength
        equipment["magic_damage"] = item.equipment.magic_damage
        equipment["prayer"] = item.equipment.prayer
        equipment["slot"] = item.equipment.slot
        item_dict["equipment"] = equipment
        item_dict["requirements"] = item.equipment.requirements #TODO

    if item.weapon:
        weapon = dict()
        weapon["attack_speed"] = item.weapon.attack_speed
        weapon["weapon_type"] = item.weapon.weapon_type
        weapon["stances"] = item.weapon.stances
        item_dict["weapon"] = weapon

    json_data["itemList"].append(item_dict)
    count = count + 1
    if count > total:
        break

out_file_name = r"..\..\AFKScape\Assets\Resources\JSON\Items.json"
with open(out_file_name, "w", newline="\n") as out_file:
    json.dump(json_data, out_file, indent=4)

