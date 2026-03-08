using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taipu.UI
{
    public class ToggleScale : Element
    {
        public Texture2D offTex;
        public Texture2D onTex;
        public Sprite btnSpr;
        public Vector2 defScale;
        public Vector2 curScale;
        public float clickFactor = 0.75f;
        public bool toggled = false;
        public bool prevToggled = false;
        public bool overMouseDown = false;

        public ToggleScale(Texture2D offTex, Texture2D onTex, Vector2 scale, Vector2 position)  
        {
            this.offTex = offTex;
            this.onTex = onTex;
            this.defScale = scale;
            this.localPosition = position;
            btnSpr = new(this.offTex,this.localPosition);
            btnSpr.origin = btnSpr.centerOrigin;
        }

        protected override void OnUpdate(GameTime gametime)
        {
            prevToggled = toggled;
            if (MouseMan.LeftDown() && btnSpr.rect.Contains(MouseMan.mousePos))
            {
                overMouseDown = true;
            }
            if (overMouseDown && MouseMan.LeftReleased())
            {
                overMouseDown = false;
            }
            if (MouseMan.LeftJustReleased() && btnSpr.rect.Contains(MouseMan.mousePos))
            {
                toggled = !toggled;
                
            }
            if (toggled)
            {
                btnSpr.texture = onTex;
            }
            else
            {
                btnSpr.texture = offTex;
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
            return (toggled != prevToggled);
        }
        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            btnSpr.Draw();
        }
        public void SetToggle(bool toggle)
        {
            toggled = toggle;
            prevToggled = toggle;
        }
    }
}
