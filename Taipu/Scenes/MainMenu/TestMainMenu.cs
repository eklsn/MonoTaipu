using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Screens.Transitions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Taipu.Scenes.MainMenu
{
    public class TestMainMenu : Scene
    {
        public UI.Label welcomeLabel;
        public UI.TextureButton playBtn;
        public UI.TextureButton editBtn;
        public BitmapFont font;
        public VFX.BgScroll bg;
        public Color fade = Color.Black;
        public void Load()
        {
            
            font = SkinLoader.getFont("fonts/main/main.fnt");
            welcomeLabel = new UI.Label(MatrixUpscaler.virtualResolution / 2 - new Vector2(0,100), "Welcome to TAIPU",font);
            welcomeLabel.centerOrig = true;
            welcomeLabel.textScale = Vector2.One / 2;
            playBtn = new(SkinLoader.getTexture("playbtn.png"), Vector2.One/4, MatrixUpscaler.virtualResolution / 2 + new Vector2(-200, 100));
            editBtn = new(SkinLoader.getTexture("editmode.png"), Vector2.One / 4, MatrixUpscaler.virtualResolution / 2 + new Vector2(200, 100));
            bg = new(SkinLoader.getTexture("stroke_10px_gray.png"), new Vector2(0,-300), Vector2.One / 4, new Vector2(144, 144), 50, 50);
            bg.speed = 16f;
            bg.color.A = (byte)90f;
        }
        public void Update()
        {
            if (fade.A > (byte)0)
            {
                double fadetemp;
                fadetemp = fade.A-(900*Global.deltaTime);
                fade.A = (byte)Math.Clamp(fadetemp,0,255);
                
            }
            
            bg.position = new Vector2(-300, -300) + ((MouseMan.mousePos / 80) * -1);
            bg.Update();
            welcomeLabel.Update(Global.gameTime);
            playBtn.Update(Global.gameTime);
            editBtn.Update(Global.gameTime); 
            if (editBtn.JustReleased())
            {
                SceneManager.LoadScene(new Editor.EditorScene(null));
            }
            if (playBtn.JustReleased())
            {
                SceneManager.LoadScene(new Play.GameScene(null));
            }
            

        }
        public void Draw()
        {
            bg.Draw(Global.spriteBatch);
            welcomeLabel.Draw(Global.spriteBatch);
            playBtn.Draw(Global.spriteBatch);
            editBtn.Draw(Global.spriteBatch);
            Global.spriteBatch.FillRectangle(new RectangleF(0, 0, MatrixUpscaler.virtualResolution.X, MatrixUpscaler.virtualResolution.Y), fade);
        }
    }
}
