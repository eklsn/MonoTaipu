using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

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
        public Vector2 scaledOrigin => origin * scale;
        public Vector2 scaledSize => size * scale;
        public Color color = Color.White;
        public bool visible = true;
        public Rectangle rect => new Rectangle((int)(position.X - scaledOrigin.X), (int)(position.Y - scaledOrigin.Y), (int)scaledSize.X, (int)scaledSize.Y);
        public Sprite(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
            this.size.X = texture.Width;
            this.size.Y = texture.Height;
        }
        public void Draw()
        {
            if (visible)
            {
                Global.spriteBatch.Draw(texture, rect, color);
            }
        }
    }
}
