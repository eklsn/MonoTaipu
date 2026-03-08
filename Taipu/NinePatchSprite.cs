using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;

namespace Taipu
{
    public class NinePatchSprite : Sprite
    {
        public Thickness borderThickness;

        public NinePatchSprite(Texture2D texture, Vector2 position, Thickness borderThickness)
            : base(texture, position)
        {
            this.borderThickness = borderThickness;
        }

        public override void Draw()
        {
            if (!visible) return;

            Rectangle[] sources = CalculateSources(texture.Width, texture.Height, borderThickness);

            Thickness scaledBorder = new Thickness(
                (int)(borderThickness.Left * scale.X),
                (int)(borderThickness.Top * scale.Y),
                (int)(borderThickness.Right * scale.X),
                (int)(borderThickness.Bottom * scale.Y)
            );

            RectangleF[] destinations = CalculateDestinations(position, scaledSize, scaledOrigin, scaledBorder);

            for (int i = 0; i < 9; i++)
            {
                if (i == 4 && (scaledSize.X <= scaledBorder.Left + scaledBorder.Right ||
                               scaledSize.Y <= scaledBorder.Top + scaledBorder.Bottom))
                {
                    continue;
                }

                Global.spriteBatch.Draw(
                    texture,
                    destinationRectangle: (Rectangle)destinations[i],
                    sourceRectangle: sources[i],
                    color: color,
                    rotation: 0f,
                    origin: Vector2.Zero,
                    effects: SpriteEffects.None,
                    layerDepth: 0f
                );
            }
        }

        private Rectangle[] CalculateSources(int texWidth, int texHeight, Thickness border)
        {
            Rectangle[] patches = new Rectangle[9];

            float x = border.Left;
            float y = border.Top;
            float w = texWidth - border.Left - border.Right;
            float h = texHeight - border.Top - border.Bottom;

            patches[0] = new Rectangle(0, 0, (int)border.Left, (int)border.Top);
            patches[1] = new Rectangle((int)x, 0, (int)w, (int)border.Top);
            patches[2] = new Rectangle((int)(x + w), 0, (int)border.Right, (int)border.Top);

            patches[3] = new Rectangle(0, (int)y, (int)border.Left, (int)h);
            patches[4] = new Rectangle((int)x + 1, (int)y + 1, (int)w - 2, (int)h - 2);
            patches[5] = new Rectangle((int)(x + w), (int)y, (int)border.Right, (int)h);

            patches[6] = new Rectangle(0, (int)(y + h), (int)border.Left, (int)border.Bottom);
            patches[7] = new Rectangle((int)x, (int)(y + h), (int)w, (int)border.Bottom);
            patches[8] = new Rectangle((int)(x + w), (int)(y + h), (int)border.Right, (int)border.Bottom);

            return patches;
        }

        private RectangleF[] CalculateDestinations(Vector2 pos, Vector2 size, Vector2 origin, Thickness border)
        {
            RectangleF[] patches = new RectangleF[9];

            float drawX = (float)Math.Floor(pos.X - origin.X);
            float drawY = (float)Math.Floor(pos.Y - origin.Y);

            float leftB = (float)Math.Floor((double)border.Left);
            float topB = (float)Math.Floor((double)border.Top);
            float rightB = (float)Math.Floor((double)border.Right);
            float bottomB = (float)Math.Floor((double)border.Bottom);

            float x1 = drawX + leftB;
            float x2 = drawX + size.X - rightB;

            float y1 = drawY + topB;
            float y2 = drawY + size.Y - bottomB;

            float centerW = x2 - x1;
            float centerH = y2 - y1;

            patches[0] = new RectangleF(drawX, drawY, leftB, topB);
            patches[1] = new RectangleF(x1, drawY, centerW, topB);
            patches[2] = new RectangleF(x2, drawY, rightB, topB);

            patches[3] = new RectangleF(drawX, y1, leftB, centerH);
            patches[4] = new RectangleF(x1, y1, centerW, centerH);
            patches[5] = new RectangleF(x2, y1, rightB, centerH);

            patches[6] = new RectangleF(drawX, y2, leftB, bottomB);
            patches[7] = new RectangleF(x1, y2, centerW, bottomB);
            patches[8] = new RectangleF(x2, y2, rightB, bottomB);

            return patches;
        }
    }
}