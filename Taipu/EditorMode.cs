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
        JukeboxSynced jbox;
        BitmapFont font;
        KeyboardBg keyboard;
        public TaipuLevel level;
        bool paused;
        Timeline timel = new();
        UI.Textbox textbox = new(new(532,512),new(500,64));
        public double time => jbox.streamPosition;
        public void Load()
        {
            jbox = new();
            level = new();
            textbox.bgColor = Color.Black;
            textbox.bgColor.A = 250;
            if (File.Exists("level.taipu")) {
                string json = File.ReadAllText("level.taipu");
                var options = new JsonSerializerOptions { IncludeFields = true };
                level = (TaipuLevel)JsonSerializer.Deserialize(json, typeof(TaipuLevel), options);
            }
            
            font = SkinLoader.getFont("fonts/main/main.fnt");
            jbox.LoadStream("D:/Taipu/test.mp3");
            jbox.Start(true);
            keyboard = new();
            renderKeys = new();
        }
        public void Update()
        {
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
                var options = new JsonSerializerOptions { WriteIndented = true, IncludeFields = true };
                string json = JsonSerializer.Serialize(level, typeof(TaipuLevel), options);
                File.WriteAllText("level.taipu", json);
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
                        String[] arrtemp = [time.ToString(), keypressed.ToString()];
                        int insPos = 0;
                        while (insPos < level.keys.Count && double.Parse(level.keys[insPos][0])<time)
                        {
                            insPos++;
                        }
                        level.keys.Insert(insPos, arrtemp);

                    }
                }

            }

        }

        public void Draw()
        {
            keyboard.Draw();
            foreach (KeyObject key in renderKeys)
            {
                if (key != null)
                {
                    if (key.visible)
                    {
                        key.Draw();
                    }
                }
                
            }
            Global.spriteBatch.DrawString(
                        font,
                        time.ToString() + "\n" + paused.ToString(),
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
