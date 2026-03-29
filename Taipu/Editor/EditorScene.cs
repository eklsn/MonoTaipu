using ManagedBass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;
using NativeFileDialogSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taipu.Editor
{
    public class EditorScene : Scene
    {
        public enum EditorTabs
        {
            None,
            MetaEditor = 1,
            Main = 2,
            Audio = 3,
            Export = 4,
        }
        public EditorTabs currentTab = EditorTabs.None;
        public Editor.Tabs.Main mainTab;
        public Editor.Tabs.MetaEditor metaTab;
        public Editor.Tabs.Export exportTab;
        public Editor.Tabs.Audio audioTab;

        public JukeboxSynced music;
        public TaipuMap level;
        public bool paused;
        public String mapPath;
        public MapLoader loader;
        public double time => music.streamPosition;

        public Sprite background;
        public Texture2D bgTex;
        public bool beatSnapping = false;
        public int beatSnapDivisor = 1;
        public EditorScene(String mapPath)
        {
            while (true)
            {
                var openResult = Dialog.FileOpen("taipu");
                if (openResult.IsOk)
                {
                    mapPath = openResult.Path;
                    break;
                }
            }

            mainTab = new(this);
            metaTab = new(this);
            exportTab = new(this);
            audioTab = new(this);
            this.mapPath = mapPath;
            loader = new();
            music = new();
            level = loader.Load(mapPath);
            LoadAudio();
            LoadBackground();
            
            while (level == null) { }

            currentTab = EditorTabs.Main;
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
            metaTab.Update(Global.gameTime);
            if (KeyboardMan.JustPressed(Keys.F1))
            {
                currentTab = EditorTabs.MetaEditor;
            }
            if (KeyboardMan.JustPressed(Keys.F2))
            {
                currentTab = EditorTabs.Main;
            }
            if (KeyboardMan.JustPressed(Keys.F3))
            {
                currentTab = EditorTabs.Audio;
            }
            switch (currentTab)
            {
                
                case EditorTabs.Main:
                    mainTab.Update(Global.gameTime); return;
                case EditorTabs.Audio:
                    audioTab.Update(Global.gameTime); return;
                case EditorTabs.Export:
                    exportTab.Update(Global.gameTime); return;
                case EditorTabs.MetaEditor:
                    metaTab.Update(Global.gameTime); return;
            }
            
        }
        public double CreateKeyTime(double time)
        {
            if (beatSnapping)
            {
                return Audio.BeatSnap.toDivisor(time, level.bpm, beatSnapDivisor);
            }
            else
            {
                return time;
            }
        }
        public void CreateKey(char keypressed)
        {
            String[] arrtemp = [Math.Round(CreateKeyTime(time), 3).ToString(), keypressed.ToString()];
            int insPos = 0;
            while (insPos < level.keys.Count && double.Parse(level.keys[insPos][0]) < time)
            {
                insPos++;
            }
            level.keys.Insert(insPos, arrtemp);
        }
        public void Draw()
        {
            background?.Draw();
            switch (currentTab) {
                case EditorTabs.Main:
                    mainTab.Draw(Global.spriteBatch); return;
                case EditorTabs.Audio:
                    audioTab.Draw(Global.spriteBatch); return;
                case EditorTabs.Export:
                    exportTab.Draw(Global.spriteBatch); return;
                case EditorTabs.MetaEditor:
                    metaTab.Draw(Global.spriteBatch); return;
            }
        }
    }
}
