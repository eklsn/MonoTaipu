using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Taipu
{
    public static class MatrixUpscaler
    {
        public static Matrix transformationMatrix;
        public static Matrix inverseTransformationMatrix;
        public static Vector2 virtualResolution;
        public static void SetVRes(int virtualWidth, int virtualHeight)
        {
            virtualResolution = new Vector2(virtualWidth, virtualHeight);
        }
        public static void Update(Viewport viewport)
        {
            float scaleX = viewport.Width / virtualResolution.X;
            float scaleY = viewport.Height / virtualResolution.Y;
            float scale = Math.Min(scaleX, scaleY);
            transformationMatrix = Matrix.CreateTranslation(-virtualResolution.X / 2, -virtualResolution.Y / 2, 0) * Matrix.CreateScale(scale, scale, 1.0f) * Matrix.CreateTranslation(viewport.Width / 2, viewport.Height / 2, 0);
            inverseTransformationMatrix = Matrix.Invert(transformationMatrix);
        }
        public static Vector2 ToVirtual(Vector2 screenPosition)
        {
            return Vector2.Transform(screenPosition, inverseTransformationMatrix);
        }
    }
}