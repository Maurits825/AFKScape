import numpy as np
import glob
import matplotlib.pyplot as plt
import matplotlib.image
import math
import os
from natsort import natsorted

icons_path = r"./icons/*.png"
icon_glob = glob.glob(icons_path)
icon_glob = natsorted(icon_glob, key=lambda y: y.lower())

width = 36
height = 32
total_icons = int(os.path.basename(icon_glob[-1])[:-4])
NUM_COLS = 160
sprite_sheet_cols = NUM_COLS * width
sprite_sheet_rows = (int(math.floor(total_icons/NUM_COLS)) + 1) * height
final_image_np = np.zeros((sprite_sheet_rows, sprite_sheet_cols, 4))
row = 0
col = 0

for idx, img in enumerate(icon_glob):
    icon_np = plt.imread(img)
    h, w, _ = icon_np.shape  # one icon is not 36x32

    name = os.path.basename(img)
    item_id = int(name[:-4])
    row, col = math.floor(item_id/NUM_COLS), item_id % NUM_COLS
    row_idx = row*height
    col_idx = col*width
    final_image_np[row_idx:row_idx+h, col_idx:col_idx+w, :] = icon_np

    if (idx % 1000) == 0:
        print(idx)


#imgplot = plt.imshow(final_image_np)
#plt.show()
matplotlib.image.imsave("spritesheet.png", final_image_np)
