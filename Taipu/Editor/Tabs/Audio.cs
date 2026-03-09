using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
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
        public UI.Element AudioUi = new();
        public UI.Label BPMLabel;
        public UI.Label OffsetLabel;
        public UI.Label BPMStatusLabel;
        public UI.Label OffsetStatusLabel;
        public double tempoModifier = 1;
        public double offsetModifier = 1;
        public BitmapFont font;
        public double changeTimer = 0;
        public double changeMargin = 0.1;
        public bool change = false;
        public Audio(Editor.EditorScene root) {
            this.root = root;
            font = SkinLoader.getFont("fonts/main/main.fnt");
            BPMLabel = new(new Vector2(640, 160), "BPM", font);
            BPMLabel.centerOrig = true;
            BPMLabel.textScale = Vector2.One / 4f;
            OffsetLabel = new(new Vector2(640, 430), "Offset", font);
            OffsetLabel.centerOrig = true;
            OffsetLabel.textScale = Vector2.One / 4f;
            BPMStatusLabel = new(new Vector2(640, 256), "120.0", font);
            BPMStatusLabel.centerOrig = true;
            BPMStatusLabel.textScale = Vector2.One / 4f;
            OffsetStatusLabel = new(new Vector2(640, 512), "0", font);
            OffsetStatusLabel.centerOrig = true;
            OffsetStatusLabel.textScale = Vector2.One / 4f;
            AudioUi.AddChildren([BPMLabel,OffsetLabel,BPMStatusLabel,OffsetStatusLabel,tempoUpBtn,tempoDownBtn,offsetUpBtn,offsetDownBtn]);
            
        }
        public void Update(GameTime gameTime)
        {
            changeTimer += Global.deltaTime;
            BPMStatusLabel.text = root.level.bpm.ToString("0.00");
            OffsetStatusLabel.text = root.level.beatOffset.ToString();
            if (KeyboardMan.Down(Keys.LeftShift) && KeyboardMan.Down(Keys.LeftControl))
            {
                changeMargin = 0.01;
                tempoModifier = 1;
                offsetModifier = 10;
            }
            else if (KeyboardMan.Down(Keys.LeftShift))
            {
                changeMargin = 0.05;
                tempoModifier = 1;
                offsetModifier = 10;
            }
            else if (KeyboardMan.Down(Keys.LeftControl))
            {
                changeMargin = 0.1;
                tempoModifier = 0.01;
                offsetModifier = 1;
            }
            else {
                changeMargin = 0.1;
                tempoModifier = 1;
                offsetModifier = 10;
            }
            if (offsetDownBtn.JustPressed() || offsetUpBtn.JustPressed() || tempoDownBtn.JustPressed() || tempoUpBtn.JustPressed())
            {
                changeTimer = changeMargin;
            }
            if (changeTimer >= changeMargin)
            {
                double tempBpm = root.level.bpm;
                double tempOffset = root.level.beatOffset;
                if (offsetDownBtn.Down())
                {
                    tempOffset -= offsetModifier;
                }
                if (offsetUpBtn.Down())
                {
                    tempOffset += offsetModifier;
                }
                if (tempoDownBtn.Down())
                {
                    tempBpm -= tempoModifier;
                }
                if (tempoUpBtn.Down())
                {
                    tempBpm += tempoModifier;
                }
                root.level.bpm = Math.Clamp(Math.Round(tempBpm, 2),0,512);
                root.level.beatOffset = Math.Round(tempOffset);
                changeTimer = 0;
            }
            AudioUi.Update(gameTime);
            
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            AudioUi.Draw(spriteBatch);
        }
    }
}
