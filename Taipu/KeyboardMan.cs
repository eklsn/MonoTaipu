using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taipu
{
    public static class KeyboardMan
    {
        public static KeyboardState currentKeyboard;
        public static KeyboardState previousKeyboard;

        public static void Update()
        {
            previousKeyboard = currentKeyboard;
            currentKeyboard = Keyboard.GetState();
        }
        public static bool JustPressed(Keys key)
        {
            if (currentKeyboard.IsKeyDown(key) && previousKeyboard.IsKeyUp(key))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool Down(Keys key)
        {
            if (currentKeyboard.IsKeyDown(key))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool Up(Keys key)
        {
            if (currentKeyboard.IsKeyUp(key))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
