using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Taipu
{
    public static class MouseMan
    {
        public static MouseState currentMouse;
        public static MouseState previousMouse;
        public static Vector2 mousePosVirtual;
        public static Vector2 mousePos;
        public static Vector2 prevMousePosVirtual;
        public static Vector2 prevMousePos;
        public static Vector2 lastClickPos;
        public static void Update()
        {
            previousMouse = currentMouse;
            currentMouse = Mouse.GetState();
            mousePosVirtual = new Vector2(currentMouse.X, currentMouse.Y);
            mousePos = Vector2.Transform(mousePosVirtual, Matrix.Invert(MatrixUpscaler.transformationMatrix));
            
        }
        public static bool RightJustPressed()
        {
            return (currentMouse.RightButton == ButtonState.Pressed && previousMouse.RightButton == ButtonState.Released);
        }
        public static bool RightDown()
        {
            return (currentMouse.RightButton == ButtonState.Pressed);
        }
        public static bool LeftDown()
        {
            return (currentMouse.LeftButton == ButtonState.Pressed);
        }
        public static bool MiddleDown()
        {
            return (currentMouse.MiddleButton == ButtonState.Pressed);
        }
        public static bool RightReleased()
        {
            return (currentMouse.RightButton == ButtonState.Released);
        }
        public static bool LeftReleased()
        {
            return (currentMouse.LeftButton == ButtonState.Released);
        }
        public static bool MiddleReleased()
        {
            return (currentMouse.MiddleButton == ButtonState.Released);
        }
        public static bool LeftJustPressed()
        {
            lastClickPos = mousePos;
            return (currentMouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Released);
        }
        public static bool MiddleJustPressed()
        {
            return (currentMouse.MiddleButton == ButtonState.Pressed && previousMouse.MiddleButton == ButtonState.Released);
        }
    }
}
