using MonoGame.Extended.BitmapFonts;
using System;
using Microsoft.Xna.Framework;

namespace Taipu.Scenes
{
    public class Disclaimer : Scene
    {
        public UI.Label header;
        public UI.Label underText;
        public BitmapFont font;
        public Color color = Color.White;
        public UI.KeyHeart heart;
        public UI.KeyWarning warning;
        public double time = 0;
        public double alpha = 0;
        public int state = 0;
        public Disclaimer()
        {
            font = SkinLoader.getFont("fonts/main/main.fnt");
            header = new UI.Label(new Vector2(640,400),"Thank You",font);
            header.localScale = new Vector2(0.25f);
            header.centerOrig = true;
            underText = new UI.Label(new Vector2(640, 460), "Taipu's still running only\n thanks to your support.", font);
            underText.color = Color.Gray;
            underText.centerOrig = true;
            underText.localScale = new Vector2(0.125f);
            heart = new();
            heart.localPosition = new Vector2(640, 300);
            heart.localScale = new Vector2(0.25f);
            color.A = (byte)0f;
            warning = new();
            warning.localPosition = heart.localPosition;
            warning.localScale = heart.localScale;
            warning.color.A = color.A;
        }
        public void Update()
        {
            time += Global.deltaTime;
            
            if ((time>0 && time < 1) || (time>4 && time < 5))
            {
                alpha += 1000.0*Global.deltaTime;
            }
            if ((time>3 && time < 4) || (time > 8 && time < 9))
            {
                alpha -= 1000.0 * Global.deltaTime;
            }
            if (KeyboardMan.JustPressed(Microsoft.Xna.Framework.Input.Keys.Enter))
            {
                alpha = 0;
                color.A = 0;
                time = 20;
                
            }
            warning.Update(Global.gameTime);
            heart.Update(Global.gameTime);
            alpha = Math.Clamp(alpha, 0, 255);
            color.A = (byte)alpha;
            header.Update(Global.gameTime);
            header.color.A = color.A;
            underText.Update(Global.gameTime);
            underText.color.A = color.A;
            if (time < 4)
            {
                
                heart.color.A = color.A;
            }
            if (time>4)
            {
                header.text = "This is an Alpha build";
                underText.text = "Don't expect ANYTHING to work well.";
                warning.color.A = color.A;
            }
            
            if (time>9)
            {
                SceneManager.LoadScene(new Scenes.MainMenu.TestMainMenu());
            }
        }
        public void Draw()
        {
            header.Draw(Global.spriteBatch);
            underText.Draw(Global.spriteBatch);
            heart.Draw(Global.spriteBatch);
            warning.Draw(Global.spriteBatch);
        }
    }
}
