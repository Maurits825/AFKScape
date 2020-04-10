import json

item_data = dict()
info_dict = dict()
info_data = {"name": "Abyssal whip", "id": 0}
info_dict["info"] = info_data

item_data["itemList"] = [info_dict, info_dict]

out_file_name = r"..\..\AFKScape\Assets\Resources\JSON\Test\item.json"
with open(out_file_name, "w", newline="\n") as out_file:
    json.dump(item_data, out_file, indent=4)
