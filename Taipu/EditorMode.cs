using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Input.InputListeners;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taipu
{
    public class EditorMode : Scene
    {
        KeyManager keyMan;
        KeyObject key1;
        KeyObject[] keys;
        JukeboxSynced jbox;
        Metronome metronome;
        BitmapFont font;
        KeyboardBg keyboard;
        bool paused;
        int maxIndex = -1;
        List<int> freeIndexes = new();
        public double time => jbox.streamPosition;
        public void Load()
        {
            keyMan = new();
            jbox = new();

            font = SkinLoader.getFont("fonts/main/main.fnt");
            jbox.LoadStream("music.mp3");
            jbox.Start(true);
            keyboard = new();
            keys = new KeyObject[0];
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
                if (key != null)
                {
                    key.Update();
                }
            }

            if (MouseMan.RightJustPressed())
            {
                foreach (KeyObject key in keys)
                {
                    if (key != null)
                    {
                        if (key.visible && key.collRect.Contains(MouseMan.mousePos))
                        {
                            removeIndex(key.myIndex);
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
                freeIndexes.Clear();
                maxIndex = -1;
                Array.Clear(keys);
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
                        int index = getFreeIndex();
                        Debug.WriteLine(index.ToString());
                        tempKey.myIndex = index;
                        keys[index] = tempKey;
                    }
                }

            }

        }
        
        public int getFreeIndex()
        {
            if (freeIndexes.Count >= 1)
            {
                freeIndexes = freeIndexes.Distinct().ToList();
                int index = freeIndexes[0];
                freeIndexes.RemoveAt(0);
                return index;
            }
            else
            {
                maxIndex++;
                Array.Resize(ref keys, maxIndex + 1);
                return maxIndex;
            }
        }
        public void removeIndex(int index)
        {
            keys[index] = null;
            freeIndexes.Add(index);
        }
        public void Draw()
        {
            keyboard.Draw();
            foreach (KeyObject key in keys)
            {
                if (key != null)
                {
                    key.Draw();
                }
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
