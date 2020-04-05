from PIL import Image
import glob

image_glob = glob.glob(r"./large_icon/*large.png")

for img in image_glob:
    background = Image.open(r"./button_background.png")
    image = Image.open(img)

    background_width, background_height = background.size
    image_width, image_height = image.size

    coor_x = 50
    coor_y = int(round((background_height/2) - (image_height/2)))

    background.paste(image, (coor_x, coor_y), image.convert('RGBA'))

    background.save("./buttons/" + img[12:-4] + "_button.png")
