using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ManagedBass;
using System.Diagnostics;

namespace Taipu
{
    public class Game1 : Game
    {

        public Game1()
        {
            Debug.WriteLine("Welcome to Taipu <3");
            Debug.WriteLine("Version b01m");
            Global.graphicsDeviceManager = new GraphicsDeviceManager(this);
            Global.contentManager = Content;
            Global.contentManager.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Bass.Init();
            Global.graphicsDevice = GraphicsDevice;
            base.Initialize();
            Debug.WriteLine(ExtContent.gameFolder);
            
        }

        protected override void LoadContent()
        {
            Global.spriteBatch = new SpriteBatch(GraphicsDevice);
            SceneManager.LoadScene(new EditorMode());

        }

        protected override void Update(GameTime gameTime)
        {
            Global.gameTime = gameTime;
            SceneManager.currentScene.Update();
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            Global.spriteBatch.Begin();
            SceneManager.currentScene.Draw();
            Global.spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
