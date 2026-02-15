using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.Swift;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Taipu
{
    public static class WindowManager
    {
        public static GraphicsDevice graphicsDevice => Global.graphicsDevice;
        public static GraphicsDeviceManager graphicsDeviceManager => Global.graphicsDeviceManager;
        public static DisplayMode displayMode => GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
        public static GameWindow window => Global.window;
        public static bool fullscreen => graphicsDeviceManager.IsFullScreen;
        public static bool fullscreenBorderless = false;
        public static Vector2 currentResolution => new Vector2(graphicsDeviceManager.PreferredBackBufferWidth, graphicsDeviceManager.PreferredBackBufferHeight);
        public static void SetFullscreen(bool fs)
        {
            if (fullscreen != fs)
            {
                graphicsDeviceManager.ToggleFullScreen();
            }
        }
        public static void ToggleFullscreen()
        {
            graphicsDeviceManager.ToggleFullScreen();
        }
        public static void ToggleFullscreenBorderless()
        {
            SetFullscreenBorderless(!fullscreenBorderless);
        }
        public static void SetFullscreenBorderless(bool fs)
        {
            if (fullscreenBorderless != fs)
            {
                if (fs)
                {
                    graphicsDeviceManager.PreferredBackBufferWidth = displayMode.Width;
                    graphicsDeviceManager.PreferredBackBufferHeight = displayMode.Height;
                }
                fullscreenBorderless = fs;
                window.IsBorderless = fs;
                graphicsDeviceManager.ApplyChanges();
            }
        }
        public static void SetResolution(int width, int height)
        {
            graphicsDeviceManager.PreferredBackBufferWidth = width;
            graphicsDeviceManager.PreferredBackBufferHeight = height;
            graphicsDeviceManager.ApplyChanges();
        }
    }
}
