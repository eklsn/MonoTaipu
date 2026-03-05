using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;


namespace Taipu
{
    public class EditorMode : Scene
    {
        List<KeyObject> renderKeys;
        Sprite background;
        Texture2D bgTex;
        JukeboxSynced jbox;
        BitmapFont font;
        KeyboardBg keyboard;
        public TaipuMap level;
        bool paused;
        String mapPath;
        UI.Textbox textbox = new(new(532,512),new(500,64));
        UI.Slider timeSlider = new(new(100, 650), new(1000, 16));
        public double time => jbox.streamPosition;
        public void Load()
        {
            jbox = new();
            level = new();
            textbox.bgColor = Color.Black;
            textbox.bgColor.A = 250;
            mapPath = "level.taipu";
            MapLoader loader = new();
            font = SkinLoader.getFont("fonts/main/main.fnt");
            if (File.Exists(mapPath))
            {
                level = loader.Load(mapPath);
                
                if (File.Exists(Path.Combine(loader.mapFolder, level.imageBg)))
                {
                    bgTex = ExtContent.getTexture(Path.Combine(loader.mapFolder, level.imageBg));
                    background = new(bgTex, Vector2.Zero);
                    background.color.A = (byte)90f;
                    background.origin = background.centerOrigin;
                    background.position = MatrixUpscaler.virtualResolution / 2f;
                    background.scale = new Vector2(MatrixUpscaler.virtualResolution.X / background.size.X);
                }
                if (File.Exists(Path.Combine(loader.mapFolder, level.audioFile)))
                {
                    jbox.LoadStream(Path.Combine(loader.mapFolder, level.audioFile));
                    jbox.Start(true);
                }
            }
            keyboard = new();
            keyboard.editor = this;
            renderKeys = new();
        }
        public void Update()
        {
            if (timeSlider.upperRange!=jbox.streamLength)
            {
                timeSlider.upperRange = jbox.streamLength;
            }
            timeSlider.bottomRange = 0;
            if (!timeSlider.dragging)
            {
                timeSlider.value = time;
            }
            else
            {
                jbox.Seek(timeSlider.value);
            }
            timeSlider.Update();
            keyboard.Update();
            textbox.Update();
            foreach (String[] key in level.keys) {
                if (time < double.Parse(key[0])-level.preRingTime-level.ringTime)
                {
                    break;
                }
                if ((time > double.Parse(key[0]) - level.preRingTime - level.ringTime) && (time < double.Parse(key[0])+level.hitTimeframe+level.disappearTime))
                {
                    bool foundFlag = false;
                    foreach (KeyObject renderKey in renderKeys)
                    {
                        if (renderKey.keyLink == key)
                        {
                            foundFlag = true;
                            break;
                        }
                    }
                    if (!foundFlag)
                    {
                        KeyObject tempKey = new(keyboard.getPosition(char.Parse(key[1])), keyboard.scale);
                        tempKey.keyLink = key;
                        tempKey.keyText = key[1].ToString();
                        tempKey.spawnStamp = double.Parse(key[0]) - level.preRingTime - level.ringTime;
                        renderKeys.Add(tempKey);
                    }
                }
            }
            for (int i = 0; i<=renderKeys.Count-1;i++)
            {
                KeyObject key = renderKeys[i];
                if (!level.keys.Contains(key.keyLink))
                {
                    renderKeys.RemoveAt(i);
                    return;
                }
                if (((key.keyTime > level.preRingTime + level.ringTime + level.hitTimeframe + level.disappearTime) || (key.keyTime < 0))&&!key.visible)
                {
                    renderKeys.RemoveAt(i);
                }
                else
                {
                    key?.Update();
                }
            }
            if (jbox.state == ManagedBass.PlaybackState.Playing)
            {
                paused = false;
            }
            else
            {
                paused = true;
            }
            

            if (MouseMan.RightJustPressed())
            {
                foreach (KeyObject key in renderKeys)
                {
                    if (key != null)
                    {
                        if (key.visible && key.collRect.Contains(MouseMan.mousePos))
                        {
                            level.keys.Remove(key.keyLink);
                            break;

                        }
                    }
                }
            }

            if (KeyboardMan.JustPressed(Keys.Space))
            {
                if (jbox.state == ManagedBass.PlaybackState.Playing)
                {
                    jbox.Stop();
                }
                else
                {
                    jbox.Start(false);
                }
            }

            if (KeyboardMan.JustPressed(Keys.F10))
            {
                var options = new JsonSerializerOptions { IncludeFields = true };
                string json = JsonSerializer.Serialize(level, typeof(TaipuMap), options);
                File.Copy(mapPath, mapPath + ".bak");
                File.WriteAllText(mapPath, json);
            }

            if (KeyboardMan.JustPressed(Keys.Home))
            {
                jbox.Start(true);
            }
            if (KeyboardMan.Down(Keys.Left))
            {
                jbox.Seek(jbox.streamPosition - 0.05);
            }
            if (KeyboardMan.Down(Keys.Right))
            {
                jbox.Seek(jbox.streamPosition + 0.05);
            }
            if (KeyboardMan.Down(Keys.LeftShift) && KeyboardMan.JustPressed(Keys.Delete))
            {
                level.keys.Clear();
                renderKeys.Clear();
            }


            foreach (Keys key in KeyboardMan.currentKeyboard.GetPressedKeys())
            {
                char keypressed = '\0';
                if (KeyboardMan.JustPressed(key))
                {
                    if (key >= Keys.A && key <= Keys.Z)
                    {
                        keypressed = (char)key;
                    }
                    if (key >= Keys.D0 && key <= Keys.D9)
                    {
                        keypressed = (char)key;
                    }
                    if (keypressed != '\0')
                    {
                        CreateKey(keypressed);

                    }
                }

            }

        }

        public void CreateKey(char keypressed)
        {
            String[] arrtemp = [Math.Round(time, 3).ToString(), keypressed.ToString()];
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
            keyboard.Draw();
            timeSlider.Draw();
            foreach (KeyObject key in renderKeys)
            {
                if (key != null)
                {
                    if (key.visible)
                    {
                        key.Blur.Draw();
                    }
                }
                
            }
            foreach (KeyObject key in renderKeys)
            {
                if (key != null)
                {
                    if (key.visible)
                    {
                        key.KeyMain.Draw();
                        key.DrawText();
                    }
                }

            }
            foreach (KeyObject key in renderKeys)
            {
                if (key != null)
                {
                    if (key.visible)
                    {
                        key.Outline.Draw();
                    }
                }

            }
            foreach (KeyObject key in renderKeys)
            {
                if (key != null)
                {
                    if (key.visible)
                    {
                        key.HitRank.Draw();
                    }
                }

            }
            Global.spriteBatch.DrawString(
                        font,
                        Math.Round(time,3).ToString() + "\n" + paused.ToString(),
                        Vector2.Zero,
                        Color.White,
                        0f,
                        Vector2.Zero,
                        0.25f,
                        SpriteEffects.None,
                        0f
                    );
            //timel.Draw(level.keys);
            textbox.Draw();
        }
    }
}
