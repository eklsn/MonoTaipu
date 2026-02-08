using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Taipu
{
    public class KeyObject
    {
        public Sprite KeyMain;
        public Vector2 mainScale = Vector2.One;
        public Color mainColor = Color.White;
        public Sprite Outline;
        public Vector2 outlineScale = Vector2.One;
        public Color outlineColor = Color.White;
        public float outlineAlpha = 1f;
        public Texture2D mainbgTex;
        public Texture2D outlineTex;

        public double preRingTime = 0.5;
        public double ringTime = 0.5;
        public double hitTimeframe = 0.3;
        public double keyTime = 0;

        public int keyType = 0;
        public int myIndex = -1;
        public double hitStamp = -1;
        public double spawnStamp = -1;

        public bool missed = false;
        public bool globalTime = false;
        public bool editor = false;

        public Vector2 position = Vector2.Zero;
        public Vector2 scale = Vector2.One;
        public Color color = Color.White;
        public bool visible = true;

        public string keyText = "";
        public string hitRank = "";

        public KeyObject(Vector2 pos,Vector2 scale)
        {
            position = pos;
            this.scale = scale;
            mainbgTex = SkinLoader.getTexture("keysq_main.png");
            outlineTex = SkinLoader.getTexture("stroke_10px_gray.png");
            KeyMain = new(mainbgTex, position);
            
            KeyMain.origin = KeyMain.centerOrigin;
            Outline = new(outlineTex, position);
            
            Outline.origin = Outline.centerOrigin;
        }
        public void Update()
        {
            KeyMain.scale = scale*mainScale;
            Outline.scale = (scale*outlineScale)+new Vector2(0.01f);
            KeyMain.color = color*mainColor;
            Outline.color = color*outlineColor;
        }
        public void Draw()
        {
            if (visible)
            {
                KeyMain.Draw();
                Outline.Draw();
            }
        }
    }
}
