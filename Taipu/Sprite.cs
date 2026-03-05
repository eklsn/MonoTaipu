using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Taipu
{
    public class Sprite
    {
        public Texture2D texture;
        public Vector2 position;
        public Vector2 size;
        public Vector2 scale = Vector2.One;
        public Vector2 origin = Vector2.Zero;
        public Vector2 centerOrigin => size / 2f;
        public Vector2 scaledOrigin {
            get
            {
                if (origin == centerOrigin) {
                    return scaledSize / 2f;
                }
                else
                {
                    return origin * scale;
                }
            }
        }
        public Vector2 scaledSize => size * scale;
        public Color color = Color.White;
        public bool visible = true;
        public RectangleF rect => new RectangleF(position-scaledOrigin,scaledSize);
        public Sprite(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
            this.size.X = texture.Width;
            this.size.Y = texture.Height;
        }
        public virtual void Draw()
        {
            if (visible)
            {
                Global.spriteBatch.Draw(
                    texture: texture,
                    position: position,
                    sourceRectangle: null,
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
