using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taipu
{
    public static class SkinLoader
    {
        public static string skinsRootFolder = "skins";
        public static string curSkin = "Default";
        public static string skinFolder
        {
            get
            {
                return Path.Combine(skinsRootFolder, curSkin);
            }
        }
        public static string skinFolderAbs
        {
            get
            {
                return Path.Combine(ExtContent.gameFolder, skinFolder);
            }
        }

        public static Texture2D getTexture(String path)
        {
            return ExtContent.getTexture(Path.Combine(skinFolder, path));
        }
        public static String getAbsPath(String path)
        {
            return Path.Combine(skinFolderAbs, path);
        }
    }
}
