import os, shutil

currentPath = os.path.dirname(os.path.abspath(__file__))

folderSource = [
    "assets",
    "Shader",

]
BUILD_LOCATION = "C:/Users/c1419/source/repos/Minecraft Clone with Open GL/Minecraft Clone with Open GL/bin/Release/netcoreapp3.1";

def copytree(src, dst):
    for item in os.listdir(src):
        s = os.path.join(src, item)
        d = os.path.join(dst, item)
        if os.path.isdir(s):
            shutil.copytree(s, d, False, None)
        else:
            shutil.copy2(s, d)

for folder in folderSource:
    print("copying folder from :", currentPath+"/"+folder)
    print("to :", BUILD_LOCATION+"/"+folder)
    os.remove(BUILD_LOCATION  + "/"+folder) 
    copytree(currentPath + "/" + folder,BUILD_LOCATION  + "/"+folder)

