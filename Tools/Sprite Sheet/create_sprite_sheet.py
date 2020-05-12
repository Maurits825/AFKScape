import numpy as np
import glob
import matplotlib.pyplot as plt
import matplotlib.image
import math
import os
from natsort import natsorted

icons_path = r"C:\Users\Maurits\Downloads\icons\items-icons\*.png"
icon_glob = glob.glob(icons_path)
icon_glob = natsorted(icon_glob, key=lambda y: y.lower())

width = 36
height = 32
total_icons = int(os.path.basename(icon_glob[-1])[:-4])
max_size = 2048
sprite_sheet_cols = math.floor(max_size/width)
sprite_sheet_rows = math.floor(max_size/height)
icons_per_sheet = sprite_sheet_cols * sprite_sheet_rows
total_sheets = math.floor(total_icons/icons_per_sheet) + 1
sprite_sheet_cols_pixels = sprite_sheet_cols * width
sprite_sheet_rows_pixels = sprite_sheet_rows * height
final_images = []
row = 0
col = 0

for i in range(total_sheets):
    final_images.append(np.ones((2048, 2048, 4)))

sheet_index = 0
for idx, img in enumerate(icon_glob):
    icon_np = plt.imread(img)
    h, w, _ = icon_np.shape  # one icon is not 36x32

    name = os.path.basename(img)
    item_id = int(name[:-4])

    if np.sum(icon_np) != 0:  # some icons are completely blank
        sheet_index = math.floor(item_id / icons_per_sheet)
        row, col = (math.floor(item_id / sprite_sheet_cols)) % sprite_sheet_rows, item_id % sprite_sheet_cols
        row_idx, col_idx = row*height, col*width

        final_images[sheet_index][row_idx:row_idx+h, col_idx:col_idx+w, :] = icon_np

    if (idx % 1000) == 0:
        print(idx)


for idx, sheet in enumerate(final_images):
    matplotlib.image.imsave("spritesheet_" + str(idx+1) + ".png", sheet)
