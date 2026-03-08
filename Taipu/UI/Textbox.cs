using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using System;
using System.Diagnostics;

namespace Taipu.UI
{
    public class Textbox
    {
        public Vector2 size;
        public Vector2 position;
        public float textScale = 0.25f;
        public float textStartMargin = 5f;
        public Color bgColor = Color.Black;
        public Color outlineColorUnselected = Color.Gray;
        public Color outlineColorSelected = Color.White;
        public Color curOutlineColor;
        public float borderThickness = 2f;
        public RectangleF rect => new RectangleF(position, size);
        public bool selected = false;
        public BitmapFont font;
        public string text = "Hello World! This is a test of my textbox system";

        public string dispText;
        public int dispLeft = 0;
        public int dispRight = 0;

        public int textCursPos = -1;
        public Vector2 graphCursPos;
        public bool textScrolled = false;
        public Vector2 measureStr;
        public Textbox(Vector2 position,Vector2 size)
        {
            
            this.size = size;
            this.position = position;
            font = SkinLoader.getFont("fonts/main/main.fnt");
            curOutlineColor = outlineColorUnselected;
            dispText = text;
        }

        public void Update(GameTime gameTime)
        {
            if (textCursPos == -1)
            {
                textCursPos = text.Length;
                dispRight = textCursPos;
                while (sizeCheck())
                {
                    dispLeft+= 1;
                    dispText = text.Substring(dispLeft, textCursPos-dispLeft);
                    textScrolled = true;
                }
            }
            if (MouseMan.LeftJustPressed())
            {
                if (rect.Contains(MouseMan.lastClickPos))
                {
                    selected = true;
                    curOutlineColor = outlineColorSelected;
                }
                else
                {
                    selected = false;
                    curOutlineColor = outlineColorUnselected;
                }
            }
            if (selected)
            {
                if ((KeyboardMan.Down(Keys.Left)) && textCursPos > 0)
                {
                    textCursPos -= 1;
                    if (textCursPos < dispLeft)
                    {
                        dispLeft -= 1;
                        dispRight -= 1;
                        textScrolled = true;
                        while (!sizeCheck())
                        {
                            dispRight += 1;
                        }
                    }


                }
                if (KeyboardMan.Down(Keys.Right) && textCursPos < text.Length)
                {
                    textCursPos += 1;
                    while (textCursPos >= dispRight && dispRight < text.Length)
                    {
                        dispLeft += 1;
                        dispRight += 1;
                    }

                }
            }
            dispText = text.Substring(dispLeft, dispRight-dispLeft);
            while (sizeCheck())
            {
                textScrolled = true;
                dispRight -= 1;
            }
            graphCursPos.X = (font.MeasureString(dispText.Substring(0, Math.Clamp(textCursPos-dispLeft,0,dispText.Length))).Width * textScale) + textStartMargin;
        }
        public bool sizeCheck()
        {
            dispRight = Math.Clamp(dispRight, 0, text.Length);
            dispLeft = Math.Clamp(dispLeft, 0, text.Length);
            dispText = text.Substring(dispLeft, dispRight - dispLeft);
            measureStr = font.MeasureString(dispText);
            return (textStartMargin + (measureStr.X*textScale) > size.X-textStartMargin);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Global.spriteBatch.FillRectangle(rect, bgColor);
            Global.spriteBatch.DrawRectangle(position, size, curOutlineColor, thickness: borderThickness);
            Global.spriteBatch.DrawString(
                    font,
                    dispText,
                    new(textStartMargin + position.X, position.Y + size.Y / 2f),
                    Color.White,
                    0f,
                    new(0, measureStr.Y / 2f),
                    textScale,
                    SpriteEffects.None,
                    0f
                );
            if (selected)
            {
                Global.spriteBatch.DrawLine(position.X + graphCursPos.X, position.Y + size.Y / 2f - (measureStr.Y * textScale / 2f), position.X + graphCursPos.X, position.Y + size.Y / 2f + (measureStr.Y * textScale / 2f), Color.White, thickness: 2f);
            }
        }
    }
}
