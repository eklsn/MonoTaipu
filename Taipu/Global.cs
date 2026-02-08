using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taipu
{
    public static class Global
    {
        public static SpriteBatch spriteBatch;
        public static GraphicsDeviceManager graphicsDeviceManager;
        public static GraphicsDevice graphicsDevice;
        public static ContentManager contentManager;
        public static GameTime gameTime;
        public static double deltaTime => gameTime.ElapsedGameTime.TotalSeconds;
    }
}
