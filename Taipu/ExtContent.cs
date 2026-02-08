using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Taipu
{
    public static class ExtContent
    {
        public static string gameFolder = AppDomain.CurrentDomain.BaseDirectory;

        public static Dictionary<String, Texture2D> texcache = new();
        public static Texture2D getTextureAbs(String path)
        {
            Texture2D tex;
            if (!texcache.ContainsKey(path)){
                using Stream stream = File.OpenRead(path);
                tex = Texture2D.FromStream(Global.graphicsDevice, stream);
                texcache[path] = tex;
            }
            else
            {
                tex = texcache[path];
            }
            return tex;
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
