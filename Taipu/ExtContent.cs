using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Taipu
{
    public static class ExtContent
    {
        public static string gameFolder = AppDomain.CurrentDomain.BaseDirectory;

        public static Dictionary<String, Texture2D> texcache = new();
        public static Dictionary<String, BitmapFont> fontcache = new();
        public static Texture2D getTextureAbs(String path)
        {
            if (!texcache.ContainsKey(path)){
                using Stream stream = File.OpenRead(path);
                texcache[path] = Texture2D.FromStream(Global.graphicsDevice, stream);
            }
            return texcache[path];
        }
        public static BitmapFont getFont(String path)
        {
            if (!fontcache.ContainsKey(path))
            {
                fontcache[path] = BitmapFont.FromFile(Global.graphicsDevice, path);
            }
            return fontcache[path];
        }
        public static Texture2D getTexture(String path)
        {
            return getTextureAbs(getAbsPath(path));
        }
        public static void texFreeAbs(String path)
        {
            texcache.Remove(path);
        }
        public static void texFree(String path)
        {
            texFreeAbs(getAbsPath(path));
        }
        public static String getAbsPath(String path)
        {
            return Path.Combine(gameFolder,path);
        }
    }
}
