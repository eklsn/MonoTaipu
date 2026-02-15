using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taipu
{
    public static class MouseMan
    {
        public static MouseState currentMouse;
        public static MouseState previousMouse;
        public static Vector2 mousePosVirtual;
        public static Vector2 mousePos;
        //MouseSta
        public static void Update()
        {
            previousMouse = currentMouse;
            currentMouse = Mouse.GetState();
            mousePosVirtual = new Vector2(currentMouse.X, currentMouse.Y);
            mousePos = Vector2.Transform(mousePosVirtual, Matrix.Invert(MatrixUpscaler.transformationMatrix));
        }
        public static bool RightJustPressed()
        {
            if (currentMouse.RightButton == ButtonState.Pressed && previousMouse.RightButton == ButtonState.Released)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool RightDown()
        {
            if (currentMouse.RightButton == ButtonState.Pressed)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool LeftDown()
        {
            if (currentMouse.LeftButton == ButtonState.Pressed)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool MiddleDown()
        {
            if (currentMouse.MiddleButton == ButtonState.Pressed)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool RightReleased()
        {
            if (currentMouse.RightButton == ButtonState.Released)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool LeftReleased()
        {
            if (currentMouse.LeftButton == ButtonState.Released)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool MiddleReleased()
        {
            if (currentMouse.MiddleButton == ButtonState.Released)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool LeftJustPressed()
        {
            if (currentMouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Released)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool MiddleJustPressed()
        {
            if (currentMouse.MiddleButton == ButtonState.Pressed && previousMouse.MiddleButton == ButtonState.Released)
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
