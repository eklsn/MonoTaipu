using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Taipu.Editor
{
    public class beatsnapBar : UI.Element
    {
        public UI.Slider divSlider = new(new(955, 125), new(285, 16));
        BitmapFont font;
        public int beatSnap;
        public beatsnapBar()
        {
            AddChild(divSlider);
            font = SkinLoader.getFont("fonts/main/main.fnt");
        }
        protected override void OnUpdate(GameTime gameTime)
        {
            divSlider.valueLinear = Math.Round(divSlider.valueLinear * 4) / 4;
            beatSnap = (int)Math.Pow(2, divSlider.valueLinear * 4);
        }
        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            Global.spriteBatch.DrawString(
                        font,
                        "Beat Snapping: 1/"+beatSnap.ToString(),
                        (new Vector2(950, 72)+absolutePosition)*absoluteScale,
                        Color.White,
                        0f,
                        Vector2.Zero,
                        0.17f*absoluteScale,
                        SpriteEffects.None,
                        0f
                    );
        }
    }
}
