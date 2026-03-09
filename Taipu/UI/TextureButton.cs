using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taipu.UI
{
    public class TextureButton : Element
    {
        public Texture2D btnTex;
        public Sprite btnSpr;
        public Vector2 defScale;
        public Vector2 curScale;
        public float clickFactor = 0.75f;
        public bool pressed = false;
        public bool prevPressed = false;
        public bool prevToggled = false;
        public bool overMouseDown = false;

        public TextureButton(Texture2D btnTex, Vector2 scale, Vector2 position)  
        {
            this.btnTex = btnTex;
            this.defScale = scale;
            this.localPosition = position;
            btnSpr = new(this.btnTex,this.localPosition);
            btnSpr.origin = btnSpr.centerOrigin;
            btnSpr.scale = curScale*absoluteScale;
            btnSpr.position = absolutePosition;
        }

        protected override void OnUpdate(GameTime gametime)
        {
            prevPressed = pressed;
            if (MouseMan.LeftJustPressed() && btnSpr.rect.Contains(MouseMan.mousePos))
            {
                overMouseDown = true;
            }
            if (overMouseDown && MouseMan.LeftReleased())
            {
                overMouseDown = false;
            }
            if (overMouseDown && btnSpr.rect.Contains(MouseMan.mousePos))
            {
                pressed = true;
                
            }
            else
            {
                pressed = false;
            }
            if (overMouseDown)
            {
                curScale = defScale * clickFactor;
            }
            else
            {
                curScale = defScale;
            }
            btnSpr.scale = Vector2.Lerp(btnSpr.scale,curScale*absoluteScale,0.4f);
            btnSpr.position = absolutePosition;
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
