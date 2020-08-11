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
    item_dict["tradeableOnGe"] = item.tradeable_on_ge
    item_dict["stackable"] = item.stackable
    item_dict["noted"] = item.noted
    item_dict["noteable"] = item.noteable
    item_dict["linkedIdItem"] = item.linked_id_item
    item_dict["linkedIdNoted"] = item.linked_id_noted
    item_dict["linkedIdPlaceholder"] = item.linked_id_placeholder
    item_dict["placeholder"] = item.placeholder
    item_dict["equipable"] = item.equipable
    item_dict["equipableByPlayer"] = item.equipable_by_player
    item_dict["equipableWeapon"] = item.equipable_weapon
    item_dict["cost"] = item.cost
    item_dict["lowalch"] = item.lowalch
    item_dict["highalch"] = item.highalch
    item_dict["weight"] = item.weight
    item_dict["buyLimit"] = item.buy_limit
    item_dict["questItem"] = item.quest_item
    item_dict["duplicate"] = item.duplicate
    item_dict["examine"] = item.examine

    if item.equipment:
        equipment = dict()
        equipment["attackStab"] = item.equipment.attack_stab
        equipment["attackSlash"] = item.equipment.attack_slash
        equipment["attackCrush"] = item.equipment.attack_crush
        equipment["attackMagic"] = item.equipment.attack_magic
        equipment["attackRanged"] = item.equipment.attack_ranged
        equipment["defenceStab"] = item.equipment.defence_stab
        equipment["defenceSlash"] = item.equipment.defence_slash
        equipment["defenceCrush"] = item.equipment.defence_crush
        equipment["defenceMagic"] = item.equipment.defence_magic
        equipment["defenceRanged"] = item.equipment.defence_ranged
        equipment["meleeStrength"] = item.equipment.melee_strength
        equipment["rangedStrength"] = item.equipment.ranged_strength
        equipment["magicDamage"] = item.equipment.magic_damage
        equipment["prayer"] = item.equipment.prayer
        equipment["slot"] = item.equipment.slot

        if item.equipment.slot == "head":
            equipment["slot"] = 0
        elif item.equipment.slot == "cape":
            equipment["slot"] = 1
        elif item.equipment.slot == "neck":
            equipment["slot"] = 2
        elif item.equipment.slot == "ammo":
            equipment["slot"] = 3
        elif item.equipment.slot == "weapon":
            equipment["slot"] = 4
        elif item.equipment.slot == "body":
            equipment["slot"] = 5
        elif item.equipment.slot == "shield":
            equipment["slot"] = 6
        elif item.equipment.slot == "legs":
            equipment["slot"] = 7
        elif item.equipment.slot == "hands":
            equipment["slot"] = 8
        elif item.equipment.slot == "feet":
            equipment["slot"] = 9
        elif item.equipment.slot == "ring":
            equipment["slot"] = 10
        elif item.equipment.slot == "2h":
            equipment["slot"] = 11

        item_dict["equipment"] = equipment
        item_dict["requirements"] = item.equipment.requirements #TODO

    if item.weapon:
        weapon = dict()
        weapon["attackSpeed"] = item.weapon.attack_speed
        weapon["weaponType"] = item.weapon.weapon_type
        weapon["stances"] = item.weapon.stances
        item_dict["weapon"] = weapon

    json_data["itemList"].append(item_dict)
    count = count + 1
    if count > total:
        break

out_file_name = r"..\..\AFKScape\Assets\Resources\JSON\Items.json"
with open(out_file_name, "w", newline="\n") as out_file:
    json.dump(json_data, out_file, indent=4)

