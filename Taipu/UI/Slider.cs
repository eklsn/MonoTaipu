using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Tweening;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taipu.UI
{
    public class Slider
    {
        public Vector2 size;
        public Vector2 position;
        public Color sliderBgColor = Color.Gray;
        public Color sliderOutColor = Color.Transparent;
        public Texture2D sliderPointTex;
        public Sprite sliderPoint;
        public float pointScaleDrag = 0.095f;
        public float pointScaleUndrag = 0.075f;
        public float pointScaleCurrent = 0f;
        public bool dragging = false;
        public double bottomRange = 0;
        public double upperRange = 1;
        public double value = 0;
        public double prevValue = 0;
        public double valueLinear = 0;
        public double prevValueLinear = 0;

        public Slider(Vector2 position, Vector2 size)
        {
            this.position = position;
            this.size = size;
            sliderPointTex = SkinLoader.getTexture("keysq_main.png");
            sliderPoint = new(sliderPointTex,new Vector2(position.X, position.Y+size.Y / 2f));
            sliderPoint.origin = sliderPoint.centerOrigin;
            pointScaleCurrent = pointScaleUndrag;
            sliderPoint.scale = new Vector2(pointScaleCurrent);
        }
        public void Update()
        {
            if ((valueLinear != prevValueLinear) && (value==prevValue))
            {
                value = bottomRange + (valueLinear * (upperRange - bottomRange));
            }
            if ((value != prevValue) && (valueLinear == prevValueLinear))
            {
                valueLinear = (value - bottomRange) / upperRange-bottomRange;
            }
                if (MouseMan.LeftDown() && !dragging)
            {
                if (sliderPoint.rect.Contains(MouseMan.mousePos) || new RectangleF(position, size).Contains(MouseMan.mousePos))
                {
                    dragging = true;
                    pointScaleCurrent = pointScaleDrag;
                }
            }
            if(MouseMan.LeftReleased() && dragging)
            {
                dragging = false;
                pointScaleCurrent = pointScaleUndrag;
            }
            if(dragging)
            {
                
                valueLinear = Math.Clamp(((MouseMan.mousePos.X - position.X) / size.X), 0, 1);
                value = bottomRange + (valueLinear * (upperRange-bottomRange));
            }
            sliderPoint.scale = Vector2.Lerp(sliderPoint.scale,new Vector2(pointScaleCurrent),0.2f);
            sliderPoint.position.X = float.Lerp(sliderPoint.position.X, (float)(position.X+(valueLinear * size.X)), 0.4f);
            
            prevValue = value;
            prevValueLinear = valueLinear;
        }
        public void Draw()
        {
            Global.spriteBatch.FillRectangle(new RectangleF(position, size), sliderBgColor);
            Global.spriteBatch.DrawRectangle(new RectangleF(position, size), sliderOutColor);
            sliderPoint.Draw();
        }
    }
}
