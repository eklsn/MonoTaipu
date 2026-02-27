using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using System;
using System.IO;
namespace Taipu
{
    public class MapLoader
    {
        public string mapsRootFolder = "maps";
        public string curMap = "test";
        public string mapFolder
        {
            get
            {
                return Path.Combine(mapsRootFolder, curMap);
            }
        }
        public string mapFolderAbs
        {
            get
            {
                return Path.Combine(ExtContent.gameFolder, mapFolder);
            }
        }

        public Texture2D getTexture(String path)
        {
            return ExtContent.getTexture(Path.Combine(mapFolder, path));
        }
        public BitmapFont getFont(String path)
        {
            return ExtContent.getFont(Path.Combine(mapFolder, path));
        }
        public String getAbsPath(String path)
        {
            return Path.Combine(mapFolderAbs, path);
        }
        public String getRelPath(String path)
        {
            return Path.Combine(mapFolder, path);
        }
    }
}
