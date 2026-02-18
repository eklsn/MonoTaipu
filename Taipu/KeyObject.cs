using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Tweening;
using System;

namespace Taipu
{
    public class KeyObject
    {
        public AtlasedSprite KeyMain;
        public Vector2 mainScale = Vector2.One;
        public Color mainColor = Color.White;

        public AtlasedSprite Outline;
        public Vector2 outlineScale = Vector2.One;
        public Color outlineColor = Color.White;

        public AtlasedSprite Blur;
        public Vector2 blurScale = Vector2.One;
        public Vector2 fontScale = Vector2.One;

        public Color blurColor = Color.White;
        public Color fontColor = Color.Black;

        public AtlasedSprite HitRank;
        public Color hitColor = Color.White;
        public Vector2 hitScale = Vector2.One;

        public Vector2 textScale = Vector2.One;
        public Vector2 textMeas = Vector2.One;
        public Vector2 textOrigin = Vector2.Zero;
        public Vector2 textPosition = Vector2.Zero;

        public Texture2D atlasTex;

        public double appearTime = 0;
        public double preRingTime = 0;
        public double ringTime = 0;
        public double hitTimeframe = 0;
        public double disappearTime = 0;

        public double keyTime = 0;  

        public double hitStamp = -1;
        public double missStamp = -1;
        public double spawnStamp = 1;

        public bool missed = false;

        public Vector2 position = Vector2.Zero;
        public Vector2 scale = Vector2.One;
        public Color color = Color.White;
        public bool visible = true;

        public string keyText = "A";
        public string measuredText = "";
        public string hitRank = "";

        public String[] keyLink = null;

        public Rectangle collRect = new();

        public BitmapFont font;
        public KeyObject(Vector2 pos,Vector2 scale)
        {
            visible = false;
            position = pos;
            this.scale = scale;
            atlasTex = SkinLoader.getTexture("keysq_atlas.png");
            font = SkinLoader.getFont("fonts/main/main.fnt");
            KeyMain = new(atlasTex, new Rectangle(0, 0, 512, 512), position);
            KeyMain.origin = KeyMain.centerOrigin;

            Outline = new(atlasTex, new Rectangle(512,0,512,512), position);
            Outline.origin = Outline.centerOrigin;
            
            Blur = new(atlasTex, new Rectangle(1024,0, 750, 750), position);
            Blur.origin = Blur.centerOrigin;

            HitRank = new(atlasTex, new Rectangle(1774, 0, 750, 512), position);
            HitRank.origin = HitRank.centerOrigin;
            collRect = new((int)(position.X-(256*scale.X)), (int)(position.Y - (256 * scale.Y)), (int)(512 * scale.X), (int)(512 * scale.Y));
        }
        public void Update()
        {
            if (appearTime > preRingTime) {
                appearTime = preRingTime;
            }
            if (SceneManager.currentScene is EditorMode game)
            {
                keyTime = game.time - spawnStamp;
                appearTime = game.level.appearTime;
                preRingTime = game.level.preRingTime;
                ringTime = game.level.ringTime;
                hitTimeframe = game.level.hitTimeframe;
                disappearTime = game.level.disappearTime;
                if (game is EditorMode)
                {
                    hitStamp = preRingTime + ringTime;
                }
            }
            if ((keyTime < preRingTime + ringTime + hitTimeframe + disappearTime) && (keyTime > 0)){
                visible = true;
                if (keyTime <= preRingTime + ringTime + hitTimeframe)
                {
                    mainColor = Color.White;
                    fontColor = Color.Black;
                   
                    blurColor.A = (byte)Math.Clamp((float)(keyTime / appearTime) * 255f, 0f, 105f);
                    float tempBlurScale = EasingFunctions.CubicOut((Math.Clamp((float)(keyTime / appearTime), 0, 1f)));
                    blurScale = new Vector2(3 - tempBlurScale * 2);
                    hitColor.A = (byte)0f;
                    mainColor.A = (byte)Math.Clamp((float)(keyTime / appearTime) * 600f, 0f, 255f);

                    float tempMainScale = EasingFunctions.QuadraticOut((Math.Clamp((float)(keyTime / appearTime), 0, 1f)));
                    mainScale = new Vector2(tempMainScale);

                    outlineColor.A = (byte)Math.Clamp((float)((keyTime - preRingTime) / ringTime) * 800f, 0f, 255f);

                    float tempOutlineScale = Math.Clamp((float)(keyTime - preRingTime) / (float)ringTime, 0, 1);
                    outlineScale = new Vector2(3) - new Vector2(2 * tempOutlineScale);

                    fontColor.A = (byte)Math.Clamp((float)(keyTime / appearTime) * 255f, 0f, 255f);
                    float tempFontScale = EasingFunctions.CubicOut((Math.Clamp((float)(keyTime / appearTime), 0, 1f)));
                    fontScale = (Vector2.One*1.65f)*new Vector2(3 - tempFontScale * 2);

                }
                if (hitStamp != -1 && keyTime>hitStamp)
                {

                    fontColor.A = (byte)0f;
                    //curFont = font_outline;
                    double tempPostHit = keyTime - hitStamp;
                    blurColor.A = (byte)(105f - Math.Clamp((float)(tempPostHit / disappearTime) * 255f, 0f, 105f));
                    float tempBlurScale = EasingFunctions.CubicOut((Math.Clamp((float)(tempPostHit / disappearTime), 0, 1f)));
                    blurScale = new Vector2(tempBlurScale * 2);

                    mainColor.A = (byte)(255f - Math.Clamp((float)(tempPostHit / (disappearTime / 2)) * 255f, 0f, 255f));

                    float tempMainScale = EasingFunctions.QuadraticOut((Math.Clamp((float)(tempPostHit / disappearTime), 0, 1f)));
                    mainScale = new Vector2(1f + (tempMainScale * 1f));

                    outlineColor.A = (byte)(255f - Math.Clamp((float)((tempPostHit) / disappearTime) * 600f, 0f, 255f));

                    float tempOutlineScale = Math.Clamp((float)(tempPostHit) / (float)disappearTime, 0, 1);
                    outlineScale = new Vector2(1 + 4 * tempOutlineScale);

                    hitColor.A = (byte)Math.Clamp((float)(tempPostHit / (disappearTime / 2)) * 400f, 0f, 255f);
                    float tempHitScale = EasingFunctions.CubicOut((Math.Clamp((float)(tempPostHit / (disappearTime / 2)), 0, 1f)));
                    hitScale = new Vector2(0.5f + (tempHitScale * 1.25f));
                    if (tempPostHit > disappearTime / 2)
                    {
                        float tempPostDis = (float)tempPostHit - (float)(disappearTime / 2);
                        hitColor.A = (byte)(255f - Math.Clamp((float)((tempPostDis / (disappearTime / 2)) * 400f), 0f, 255f));
                    }
                }
                if (hitStamp == -1 && keyTime > preRingTime + ringTime + hitTimeframe && missStamp == -1)
                {
                    missStamp = preRingTime + ringTime + hitTimeframe;
                    missed = true;
                }
                if (missStamp != -1 && keyTime>missStamp)
                {
                    disappearTime = 1;
                    float tempPostMiss = (float)(keyTime - missStamp);
                    fontColor = Color.White;
                    //keyText = "MISS";
                    //fontScale = Vector2.One / 1.5f;
                    outlineColor.A = (byte)0f;
                    blurColor.A = (byte)0f;
                    mainColor.G = 0;
                    mainColor.B = 0;
                    mainColor.A = (byte)(255f - Math.Clamp((float)(tempPostMiss / (disappearTime / 2)) * 255f, 0f, 255f));
                    fontColor.A = mainColor.A;
                }
            }
            else
            {
                visible = false;
            }


            KeyMain.scale = scale * mainScale;
            KeyMain.color = color * mainColor;

            Outline.scale = (scale * outlineScale) + new Vector2(0.001f);
            Outline.color = color * outlineColor;

            Blur.scale = (scale * blurScale);
            Blur.color = color * blurColor;
            HitRank.color = color * hitColor;
            HitRank.scale = scale * hitScale;
            textScale = new(this.fontScale.X * this.scale.X);
            if (keyText !=measuredText) {
                textMeas = font.MeasureString(keyText);
                measuredText = keyText;
            }
            float uniformScale = (float)this.scale.X;
            Vector2 baseOffset = new(8f, -12f);
            Vector2 scalableShift = baseOffset * uniformScale;
            textPosition = position - scalableShift;
            textOrigin = new(textMeas.X / 2f, textMeas.Y / 2f);
        }
        public void Draw()
        {
            if (visible)
            {
                Blur.Draw();
                KeyMain.Draw();
                Outline.Draw();
                HitRank.Draw();
                

                Global.spriteBatch.DrawString(
                    font,
                    keyText,
                    textPosition, 
                    fontColor,
                    0f,
                    textOrigin,
                    textScale,
                    SpriteEffects.None,
                    0f
                );
            }
        }
    }
}
