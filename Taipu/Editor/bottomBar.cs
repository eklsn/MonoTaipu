using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using System;

namespace Taipu.Editor
{
    public class bottomBar : UI.Element
    {
        public UI.Slider timeSlider = new(new(175, 650), new(950, 16));
        public UI.ToggleScale pauseBtn = new(SkinLoader.getTexture("pausebtn.png"), SkinLoader.getTexture("playbtn.png"), new Vector2(0.15f), new Vector2(1198, 657));
        BitmapFont font;
        public String timerText = "";
        public bottomBar()
        {
            AddChild(timeSlider);
            AddChild(pauseBtn);
            font = SkinLoader.getFont("fonts/main/main.fnt");
        }
        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            Global.spriteBatch.DrawString(
                        font,
                        timerText,
                        (new Vector2(29, 642)+absolutePosition)*absoluteScale,
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
