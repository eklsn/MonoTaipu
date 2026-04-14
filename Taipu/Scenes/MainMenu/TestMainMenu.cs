using MonoGame.Extended.BitmapFonts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
        public void Load()
        {
            font = SkinLoader.getFont("fonts/main/main.fnt");
            welcomeLabel = new UI.Label(MatrixUpscaler.virtualResolution / 2 - new Vector2(0,100), "Welcome to TAIPU",font);
            welcomeLabel.centerOrig = true;
            welcomeLabel.textScale = Vector2.One / 2;
            playBtn = new(SkinLoader.getTexture("playbtn.png"), Vector2.One/3, MatrixUpscaler.virtualResolution / 2 + new Vector2(-200, 100));
            editBtn = new(SkinLoader.getTexture("editmode.png"), Vector2.One / 3, MatrixUpscaler.virtualResolution / 2 + new Vector2(200, 100));
        }
        public void Update()
        {
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
            welcomeLabel.Draw(Global.spriteBatch);
            playBtn.Draw(Global.spriteBatch);
            editBtn.Draw(Global.spriteBatch);
        }
    }
}
