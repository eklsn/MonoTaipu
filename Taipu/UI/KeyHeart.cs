using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Taipu.UI
{
    public class KeyHeart : Element
    {
        public Sprite keyBg;
        public Sprite heart;
        public Color color = Color.White;
        public KeyHeart()
        {
            keyBg = new(SkinLoader.getTexture("keysq_main.png"), Vector2.Zero);
            keyBg.origin = keyBg.centerOrigin;
            heart = new(SkinLoader.getTexture("thankyou_heart.png"), Vector2.Zero);
            heart.origin = heart.centerOrigin;
        }
        protected override void OnUpdate(GameTime gameTime)
        {
            keyBg.scale = absoluteScale;
            heart.scale = absoluteScale;
            heart.position = absolutePosition;
            keyBg.position = absolutePosition;
            keyBg.color = color;
            heart.color = color;
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            keyBg.Draw();
            heart.Draw();
        }
    }
}
