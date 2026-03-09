using Microsoft.Xna.Framework.Input;

namespace Taipu
{
    public static class KeyboardMan
    {
        public static KeyboardState currentKeyboard;
        public static KeyboardState previousKeyboard;

        public static void Update()
        {
            previousKeyboard = currentKeyboard;
            if (Global.game.IsActive)
            {
                currentKeyboard = Keyboard.GetState();
            }
            else
            {
                currentKeyboard = new();
            }
        }
        public static bool JustPressed(Keys key)
        {
            return (currentKeyboard.IsKeyDown(key) && previousKeyboard.IsKeyUp(key));
        }
        public static bool Down(Keys key)
        {
            return (currentKeyboard.IsKeyDown(key));
        }
        public static bool Up(Keys key)
        {
            return (currentKeyboard.IsKeyUp(key));
        }
    }
}
