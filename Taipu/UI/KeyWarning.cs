using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Taipu.UI
{
    public class KeyWarning : Element
    {
        public Sprite keyBg;
        public Sprite warning;
        public Color color = Color.White;
        public KeyWarning()
        {
            keyBg = new(SkinLoader.getTexture("keysq_main.png"), Vector2.Zero);
            keyBg.origin = keyBg.centerOrigin;
            warning = new(SkinLoader.getTexture("keywarning.png"), Vector2.Zero);
            warning.origin = warning.centerOrigin;
        }
        protected override void OnUpdate(GameTime gameTime)
        {
            keyBg.scale = absoluteScale;
            warning.scale = absoluteScale;
            warning.position = absolutePosition;
            keyBg.position = absolutePosition;
            keyBg.color = color;
            warning.color = color;
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            keyBg.Draw();
            warning.Draw();
        }
    }
}
