using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.UI;
using System;
using System.IO;


namespace Taipu.Editor.Tabs
{
    public class MetaEditor
    {
        Editor.EditorScene root;
        Desktop desktop;
        BitmapFont font;
        Vector2 myraScale = Vector2.One * 2;

        TextBox songName, songAuthor, mapAuthor, keyAppear, preRing, ring,hitTimeframe, keyDisappear, minusHPIdle,minusHPMiss;
        UI.Label songNameLabel, songAuthorLabel, mapAuthorLabel, audioImportLabel, bgImageImportLabel, keyAppearLabel, preRingLabel, ringLabel, hitTimeframeLabel, keyDisappearLabel, mHPIdleLabel, mHPMissLabel;
        UI.NinePatchButton audioImportButton, bgImageImportButton;

        public MetaEditor(Editor.EditorScene root)
        {
            this.root = root;
            font = SkinLoader.getFont("fonts/main/main.fnt");

            desktop = new Desktop { HasExternalTextInput = true, Scale = myraScale };
            Global.game.Window.TextInput += (s, a) => desktop.OnChar(a.Character);

            songNameLabel = CreateLabel(new Vector2(100, 95), "Song name");
            songName = CreateTextBox(78, 98, "Song name here...");

            songAuthorLabel = CreateLabel(songNameLabel.localPosition+new Vector2(0, 82), "Song author's name");
            songAuthor = CreateTextBox(songName.Left, songName.Top+58, "Song author name here...");

            mapAuthorLabel = CreateLabel(songAuthorLabel.localPosition + new Vector2(0, 82), "Mapper's name");
            mapAuthor = CreateTextBox(songAuthor.Left, songAuthor.Top + 58, "Mapper's name here...");

            audioImportLabel = CreateLabel(mapAuthorLabel.localPosition + new Vector2(0, 82), "Audio file");
            audioImportButton = CreateButton(new Vector2(305, 415), "Import audio file...");

            bgImageImportLabel = CreateLabel(new Vector2(100, 475), "Background Image file");
            bgImageImportButton = CreateButton(new Vector2(305, 550), "Import BG image...");

            keyAppearLabel = CreateLabel(songNameLabel.localPosition+new Vector2(500,0), "Key Appear time");
            keyAppear = CreateTextBox(songName.Left + 372,songName.Top, "Key Appear time...");

            preRingLabel = CreateLabel(keyAppearLabel.localPosition + new Vector2(0, 82), "Pre Ring time");
            preRing = CreateTextBox(keyAppear.Left, keyAppear.Top + 58, "Pre Ring time...");

            ringLabel = CreateLabel(preRingLabel.localPosition + new Vector2(0, 82), "Ring time");
            ring = CreateTextBox(preRing.Left, preRing.Top + 60, "Ring time...");

            hitTimeframeLabel = CreateLabel(ringLabel.localPosition + new Vector2(0, 82), "Hit timeframe");
            hitTimeframe = CreateTextBox(ring.Left, ring.Top + 60, "Hit timeframe...");

            keyDisappearLabel = CreateLabel(hitTimeframeLabel.localPosition + new Vector2(0, 82), "Disappear Time");
            keyDisappear = CreateTextBox(hitTimeframe.Left, hitTimeframe.Top + 60, "Disappear Time...");

            mHPIdleLabel = CreateLabel(keyDisappearLabel.localPosition + new Vector2(0, 76), "Minus HP Idle");
            minusHPIdle = CreateTextBox(keyDisappear.Left, keyDisappear.Top + 60, "Minus HP Idle...");

            mHPMissLabel = CreateLabel(mHPIdleLabel.localPosition + new Vector2(0, 76), "Minus HP Miss");
            minusHPMiss = CreateTextBox(minusHPIdle.Left, minusHPIdle.Top + 60, "Minus HP Miss...");
        }

        public void Update(GameTime gameTime)
        {
            audioImportButton.Update(gameTime);
            bgImageImportButton.Update(gameTime);
            audioImportButton.text = root.level.audioFile;
            bgImageImportButton.text = root.level.imageBg;

            bool isActive = root.currentTab == Editor.EditorScene.EditorTabs.MetaEditor;
            desktop.FocusedKeyboardWidget = isActive ? desktop.FocusedKeyboardWidget : null;

            SyncTextBox(songName, ref root.level.songName);
            SyncTextBox(songAuthor, ref root.level.songAuthor);
            SyncTextBox(mapAuthor, ref root.level.mapAuthor);
            SyncTextBox(keyAppear, ref root.level.appearTime);
            SyncTextBox(preRing, ref root.level.preRingTime);
            SyncTextBox(ring, ref root.level.ringTime);
            SyncTextBox(hitTimeframe, ref root.level.hitTimeframe);
            SyncTextBox(keyDisappear, ref root.level.disappearTime);
            SyncTextBox(minusHPIdle, ref root.level.minusHPIdle);
            SyncTextBox(minusHPMiss, ref root.level.minusHPMiss);

            HandleImport(audioImportButton, "mp3,wav,wave,ogg,flac",
                path => { root.level.audioFile = Path.GetFileName(path); root.LoadAudio(); });

            HandleImport(bgImageImportButton, "png,jpg,jpeg,gif,bmp",
                path => { root.level.imageBg = Path.GetFileName(path); root.LoadBackground(); });
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            desktop.Render();
            foreach (var label in new[] { songNameLabel, songAuthorLabel, mapAuthorLabel, audioImportLabel, bgImageImportLabel, keyAppearLabel, preRingLabel,ringLabel,hitTimeframeLabel,keyDisappearLabel,mHPIdleLabel,mHPMissLabel })
                label.Draw(spriteBatch);

            audioImportButton.Draw(spriteBatch);
            bgImageImportButton.Draw(spriteBatch);
        }

        private TextBox CreateTextBox(int x, int y, string placeholder)
        {
            var tb = new TextBox
            {
                Left = x,
                Top = y,
                Width = 300,
                Height = 30,
                Text = placeholder,
                Background = new SolidBrush(new Color(30, 30, 30)),
                TextColor = Color.White
            };
            desktop.Widgets.Add(tb);
            return tb;
        }

        private UI.Label CreateLabel(Vector2 pos, string text)
        {
            return new UI.Label(pos, text, font) { localScale = new Vector2(0.15f) };
        }

        private UI.NinePatchButton CreateButton(Vector2 pos, string text)
        {
            return new UI.NinePatchButton(SkinLoader.getTexture("9patchbtn.png"), font, 25, text, new Vector2(500, 100), pos)
            {
                localScale = Vector2.One / 1.25f,
                textScale = new Vector2(0.25f)
            };
        }

        private void SyncTextBox(TextBox tb, ref string modelValue)
        {
            tb.Enabled = root.currentTab == Editor.EditorScene.EditorTabs.MetaEditor;

            if (tb.IsKeyboardFocused)
                modelValue = tb.Text;
            else
                tb.Text = modelValue;
        }
        private void SyncTextBox(TextBox tb, ref double modelValue)
        {
            tb.Enabled = root.currentTab == Editor.EditorScene.EditorTabs.MetaEditor;

            if (tb.IsKeyboardFocused)
            {
                if (double.TryParse(tb.Text, out double result))
                {
                    modelValue = result;
                }
            }
            else
            {
                tb.Text = modelValue.ToString();
            }
        }

        private void HandleImport(UI.NinePatchButton btn, string filter, Action<string> onSuccess)
        {
            if (!btn.JustToggled()) return;

            var result = NativeFileDialogSharp.Dialog.FileOpen(filter);
            if (result.IsOk)
            {
                btn.pressed = false;
                string destPath = Path.Combine(Path.GetDirectoryName(root.mapPath), Path.GetFileName(result.Path));

                if (result.Path != destPath)
                {
                    try { File.Copy(result.Path, destPath, true); }
                    catch { }
                }
                onSuccess(destPath);
            }
        }
    }
}