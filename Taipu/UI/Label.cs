using Microsoft.Xna.Framework;
using FontStashSharp;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using System;

namespace Taipu.UI
{
    public class Label : Element
    {
        public String text;
        public Color color = Color.White;
        public bool centerOrig = false;
        public Vector2 textScale = Vector2.One;
        public BitmapFont font;
        public bool initialized = false;
        public Label(Vector2 position, String text, BitmapFont font)
        {
            this.text = text;
            localPosition = position;
            this.font = font;
        }
        protected override void OnUpdate(GameTime gameTime)
        {
            size = font.MeasureString(text);
            if (parent != null)
            {
                absolutePosition = parent.absolutePosition + localPosition;
            }
            if (centerOrig)
            {
                origin = centerOrigin;
            }
            initialized = true;
        }
        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            if (initialized)
            {
                Global.spriteBatch.DrawString(
                        font,
                        text,
                        absolutePosition,
                        color,
                        0f,
                        origin,
                        absoluteScale * textScale,
                        SpriteEffects.None,
                        0f
                    );
            }
        }
    }
}
