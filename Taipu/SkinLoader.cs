using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using System;
using System.IO;
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
        public static BitmapFont getFont(String path)
        {
            return ExtContent.getFont(Path.Combine(skinFolder, path));
        }
        public static String getAbsPath(String path)
        {
            return Path.Combine(skinFolderAbs, path);
        }
        public static String getRelPath(String path)
        {
            return Path.Combine(skinFolder, path);
        }
    }
}
