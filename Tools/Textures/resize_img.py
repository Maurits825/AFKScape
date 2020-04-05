from PIL import Image
import glob

image_glob = glob.glob(r"./*icon.png")

for img in image_glob:
    im = Image.open(img)

    width, height = im.size

    new_size = (width*10, height*10)
    im1 = im.resize(new_size, Image.NEAREST)

    im1.save("./large_icon" + img[:-4] + "_large.png")
