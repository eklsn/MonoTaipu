using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;
using System.Collections.Generic;

namespace Taipu
{
    public class EditorMode : Scene
    {
        List<KeyObject> keys;
        JukeboxSynced jbox;
        BitmapFont font;
        KeyboardBg keyboard;
        bool paused;
        public double time => jbox.streamPosition;
        public void Load()
        {
            jbox = new();

            font = SkinLoader.getFont("fonts/main/main.fnt");
            jbox.LoadStream("music.mp3");
            jbox.Start(true);
            keyboard = new();
            keys = new();
        }
        public void Update()
        {
            if (jbox.state == ManagedBass.PlaybackState.Playing)
            {
                paused = false;
            }
            else
            {
                paused = true;
            }
            foreach (KeyObject key in keys)
            {
                key?.Update();
            }

            if (MouseMan.RightJustPressed())
            {
                foreach (KeyObject key in keys)
                {
                    if (key != null)
                    {
                        if (key.visible && key.collRect.Contains(MouseMan.mousePos))
                        {
                            keys.Remove(key);
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
            if (KeyboardMan.Down(Keys.LeftShift)&&KeyboardMan.JustPressed(Keys.Delete))
            {
                keys.Clear();
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
                        KeyObject tempKey = new(keyboard.getPosition(keypressed), keyboard.scale);
                        tempKey.keyText = keypressed.ToString();
                        tempKey.spawnStamp = time - tempKey.preRingTime - tempKey.ringTime;
                        keys.Add(tempKey);
                    }
                }

            }

        }
        
        public void Draw()
        {
            keyboard.Draw();
            foreach (KeyObject key in keys)
            {
                key?.Draw();
            }
            Global.spriteBatch.DrawString(
                    font,
                    time.ToString()+"\n"+paused.ToString(),
                    Vector2.Zero,
                    Color.White,
                    0f,
                    Vector2.Zero,
                    0.25f,
                    SpriteEffects.None,
                    0f
                );
        }

    } }
