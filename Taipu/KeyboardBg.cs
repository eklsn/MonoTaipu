using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Taipu
{
    public class KeyboardBg
    {
        public Texture2D outlineTex;
        public Dictionary<char,Sprite> keyArr = new();
        public BitmapFont font;
        public char[][] kbrdLayout = new char[][]
        {
            new[] { '1','2','3','4','5','6','7','8','9','0' },
            new[] { 'Q','W','E','R','T','Y','U','I','O','P' },
            new[] { 'A','S','D','F','G','H','J','K','L' },
            new[] { 'Z','X','C','V','B','N','M' }
        };
        public int[] rowOffset = { 0, 0, 30, 85 };
        public Vector2 spacing = new Vector2(100);
        public Vector2 scale = new Vector2(0.175f);
        public Vector2 position = new Vector2(175,240);
        public KeyboardBg() {
            outlineTex = SkinLoader.getTexture("keysq_atlas.png");
            font = SkinLoader.getFont("fonts/main/main.fnt");
            Construct();
        }
        public void Construct()
        {
            keyArr.Clear();
            for (int i = 0; i < kbrdLayout.Length; i++)
            {
                Vector2 tempPos;
                tempPos.X = position.X + (float)rowOffset[i];
                tempPos.Y = position.Y+spacing.Y * i;
                for (int j = 0; j < kbrdLayout[i].Length; j++)
                {
                    tempPos.X = position.X+rowOffset[i] + j * spacing.X;
                    AtlasedSprite tempSprite = new(outlineTex,new Rectangle(512,0,512,512), scale);
                    tempSprite.origin = tempSprite.centerOrigin;
                    tempSprite.position = tempPos;
                    tempSprite.scale = scale;
                    keyArr.Add(kbrdLayout[i][j], tempSprite);
                }
            }
        }
        public Vector2 getPosition(char key)
        {
            return keyArr[key].position;
        }
        public void Update() { }
        public void Draw() { 
            foreach(var key in keyArr)
            {
                key.Value.Draw();
                float uniformScale = (float)this.scale.X;
                float textScale = 1.65f * uniformScale;
                Vector2 size = font.MeasureString(key.Key.ToString());
                Vector2 baseOffset = new Vector2(8f, -12f);
                Vector2 scalableShift = baseOffset * uniformScale;
                Vector2 drawPosition = key.Value.position - scalableShift;
                Vector2 origin = new Vector2(size.X / 2f, size.Y / 2f);

                Global.spriteBatch.DrawString(
                    font,
                    key.Key.ToString(),
                    drawPosition,
                    Color.Gray,
                    0f,
                    origin,
                    textScale,
                    SpriteEffects.None,
                    0f
                );
            }
        }
    }
}
