using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Taipu
{
    public class AtlasedSprite : Sprite
    {
        public Rectangle sourceRect { get; private set; }
        public AtlasedSprite(Texture2D atlas, Rectangle sourceRect, Vector2 position) : base(atlas, position)
        {
            this.sourceRect = sourceRect;
            this.size = new Vector2(sourceRect.Width, sourceRect.Height);
        }
        public override void Draw()
        {
            if (visible)
            {
                Global.spriteBatch.Draw(
                    texture: texture,
                    position: position,
                    sourceRectangle: sourceRect,
                    color: color,
                    rotation: 0f,
                    origin: origin,
                    scale: scale,
                    effects: SpriteEffects.None,
                    layerDepth: 0f
                );
            }
        }
    }
}