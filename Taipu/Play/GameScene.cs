using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Collisions.Layers;
using MonoGame.Framework;
using NativeFileDialogSharp;
using System;
using System.IO;


namespace Taipu.Play
{
    public class GameScene : Scene
    {
        TaipuMap level;
        string mapPath;
        MapLoader loader;
        public Sprite background;
        public Texture2D bgTex;
        public double time => music.streamPosition+ConfigManager.playerConfig.manualLatency;
        public bool paused;
        public JukeboxSynced music;
        public KeyboardBg keyboard;
        public KeyObject[] renderKeys;
        public GameScene(string mapPath)
        {
            this.mapPath = mapPath;
            loader = new();
            music = new();
            while (mapPath == null)
            {
                var openResult = Dialog.FileOpen("taipu");
                if (openResult.IsOk)
                {
                    this.mapPath = openResult.Path;
                    break;
                }
            }
            
            level = loader.Load(this.mapPath);
            LoadAudio();
            LoadBackground();
            keyboard = new();
            music.Start(true);

        }
        public void LoadAudio()
        {
            if (File.Exists(Path.Combine(loader.mapFolder, level.audioFile)))
            {
                music.Stop();
                music.LoadStream(Path.Combine(loader.mapFolder, level.audioFile));
            }
        }
        public void LoadBackground()
        {
            if (File.Exists(Path.Combine(loader.mapFolder, level.imageBg)))
            {
                bgTex = ExtContent.getTexture(Path.Combine(loader.mapFolder, level.imageBg));
                background = new(bgTex, Vector2.Zero);
                background.color.A = (byte)90f;
                background.origin = background.centerOrigin;
                background.position = MatrixUpscaler.virtualResolution / 2f;
                background.scale = new Vector2(MatrixUpscaler.virtualResolution.X / background.size.X);
            }
        }

        public void Update()
        {
            keyboard.Update();
            if (KeyboardMan.Down(Keys.LeftShift) && KeyboardMan.Down(Keys.Escape))
            {
                music.Stop();
                SceneManager.LoadScene(new Scenes.MainMenu.TestMainMenu());
            }
        }
        public void Draw()
        {
            background.Draw();
            keyboard.Draw();
        }

    }
}

