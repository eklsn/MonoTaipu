using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Taipu.Editor.Tabs
{
    public class Audio
    {
        Editor.EditorScene root = null;
        public UI.TextureButton tempoUpBtn = new(SkinLoader.getTexture("arrowupbtn.png"),new Vector2(0.175f),new Vector2(792,256));
        public UI.TextureButton tempoDownBtn = new(SkinLoader.getTexture("arrowdownbtn.png"), new Vector2(0.175f), new Vector2(490, 256));
        public UI.TextureButton offsetUpBtn = new(SkinLoader.getTexture("arrowupbtn.png"), new Vector2(0.175f), new Vector2(792, 512));
        public UI.TextureButton offsetDownBtn = new(SkinLoader.getTexture("arrowdownbtn.png"), new Vector2(0.175f), new Vector2(490, 512));
        public UI.Label BPMLabel;
        public UI.Label OffsetLabel;
        public UI.Label BPMStatusLabel;
        public UI.Label OffsetStatusLabel;
        public BitmapFont font;
        public Audio(Editor.EditorScene root) {
            this.root = root;
            font = SkinLoader.getFont("fonts/main/main.fnt");
            BPMLabel = new(new Vector2(640, 160), "BPM", font);
            BPMLabel.centerOrig = true;
            BPMLabel.localScale = Vector2.One / 4f;
            OffsetLabel = new(new Vector2(640, 430), "Offset", font);
            OffsetLabel.centerOrig = true;
            OffsetLabel.localScale = Vector2.One / 4f;
            BPMStatusLabel = new(new Vector2(640, 256), "120.0", font);
            BPMStatusLabel.centerOrig = true;
            BPMStatusLabel.localScale = Vector2.One / 4f;
            OffsetStatusLabel = new(new Vector2(640, 512), "0", font);
            OffsetStatusLabel.centerOrig = true;
            OffsetStatusLabel.localScale = Vector2.One / 4f;
        }
        public void Update(GameTime gameTime)
        {
            tempoUpBtn.Update(Global.gameTime);
            tempoDownBtn.Update(Global.gameTime);
            offsetUpBtn.Update(Global.gameTime);
            offsetDownBtn.Update(Global.gameTime);
            BPMLabel.Update(Global.gameTime);
            OffsetLabel.Update(gameTime);
            BPMStatusLabel.Update(gameTime);
            OffsetStatusLabel.Update(gameTime);
            if (tempoUpBtn.JustPressed())
            {
                Debug.WriteLine("Yup");
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            tempoUpBtn.Draw(Global.spriteBatch);
            tempoDownBtn.Draw(Global.spriteBatch);
            offsetUpBtn.Draw(Global.spriteBatch);
            offsetDownBtn.Draw(Global.spriteBatch);
            BPMLabel.Draw(spriteBatch);
            OffsetLabel.Draw(spriteBatch);
            BPMStatusLabel.Draw(spriteBatch);
            OffsetStatusLabel.Draw(spriteBatch);
        }
    }
}
