using ManagedBass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input.InputListeners;
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
            Global.window = Window;
            base.Initialize();
            Debug.WriteLine(ExtContent.gameFolder);
            Global.graphicsDeviceManager.PreferMultiSampling = true;
            Global.graphicsDevice.PresentationParameters.MultiSampleCount = 8;


        }

        protected override void LoadContent()
        {
            Global.spriteBatch = new SpriteBatch(GraphicsDevice);
            SceneManager.LoadScene(new EditorMode());
            var displayMode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
            MatrixUpscaler.SetVRes(1280, 720);
            WindowManager.SetResolution(1920, 1080);
            //WindowManager.SetFullscreenBorderless(true);
            MatrixUpscaler.Update(Global.graphicsDevice.Viewport);

        }

        protected override void Update(GameTime gameTime)
        {
            Global.gameTime = gameTime;
            MouseMan.Update();
            KeyboardMan.Update();
            base.Update(gameTime);
            SceneManager.currentScene.Update();
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            Global.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.LinearClamp, transformMatrix: MatrixUpscaler.transformationMatrix);
            SceneManager.currentScene.Draw();
            Global.spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
