using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended; // Required for Thickness

namespace Taipu.UI
{
    public class NinePatchButton : Element
    {
        public Texture2D tex;
        public NinePatchSprite btnSpr;
        public Vector2 defScale;
        public Vector2 curScale;
        public float clickFactor = 0.75f;
        public bool pressed = false;
        public bool prevToggled = false;
        public bool overMouseDown = false;
        public String text;
        public Label label;
        public Vector2 textScale;
        public BitmapFont font;

        public NinePatchButton(Texture2D ninepatchspr, BitmapFont font, int thickness, String text, Vector2 size, Vector2 position)
        {
            this.tex = ninepatchspr;
            this.size = size;
            this.defScale = Vector2.One;
            this.localPosition = position;
            btnSpr = new NinePatchSprite(this.tex, this.localPosition, new Thickness(thickness));
            btnSpr.size = size;
            btnSpr.origin = btnSpr.centerOrigin;
            label = new Label(Vector2.Zero, text, font);
            label.origin = new Vector2(font.MeasureString(text).Width / 2f, font.MeasureString(text).Height / 2f);
            label.color = Color.Black;
            textScale = new Vector2(2f);
            AddChild(label);
        }

        protected override void OnUpdate(GameTime gametime)
        {
            label.text = text;
            pressed = false;
            btnSpr.position = absolutePosition;
            label.centerOrig = true;
            if (label.scaledSize.X+25 > scaledSize.X)
            {
                textScale -= new Vector2(0.01f);
            }
            if (MouseMan.LeftJustPressed() && btnSpr.rect.Contains(MouseMan.mousePos))
            {
                overMouseDown = true;
            }
            if (MouseMan.LeftJustReleased() && overMouseDown && btnSpr.rect.Contains(MouseMan.mousePos))
            {
                pressed = true;
            }
            if (overMouseDown && MouseMan.LeftReleased())
            {
                overMouseDown = false;
            }
            

            if (overMouseDown)
            {
                curScale = defScale * clickFactor;
            }
            else
            {
                curScale = defScale;
            }

            btnSpr.scale = Vector2.Lerp(btnSpr.scale, curScale * absoluteScale, 32f * (float)Global.deltaTime);
            label.localScale = btnSpr.scale*textScale;

        }

        public bool JustToggled()
        {
            if (pressed)
            {
                pressed = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            btnSpr.Draw();

        }
    }
}