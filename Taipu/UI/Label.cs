using MonoGame.Extended;
using Microsoft.Xna.Framework;
using FontStashSharp;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taipu.UI
{
    public class Label : Element
    {
        public String text;
        public Color color = Color.White;
        public bool centerOrig = false;
        public BitmapFont font;
        public Label(Vector2 position, String text, BitmapFont font)
        {
            this.text = text;
            localPosition = position;
            this.font = font;
        }
        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            
            size = font.MeasureString(text);
            if (centerOrig)
            {
                origin = centerOrigin;
            }
            Update(Global.gameTime);
            Global.spriteBatch.DrawString(
                    font,
                    text,
                    absolutePosition,
                    color,
                    0f,
                    origin,
                    absoluteScale,
                    SpriteEffects.None,
                    0f
                );
        }
    }
}
