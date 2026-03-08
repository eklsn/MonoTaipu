

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NativeFileDialogSharp;
using System.IO;

namespace Taipu.Editor.Tabs
{
    public class MetaEditor
    {
        Editor.EditorScene root = null;
        public Desktop desktop;
        TextBox songName;
        UI.Label songNameLabel;
        TextBox songAuthor;
        UI.Label songAuthorLabel;
        TextBox mapAuthor;
        UI.Label mapAuthorLabel;
        Vector2 myraScale = Vector2.One * 2;
        BitmapFont font;
        UI.NinePatchButton audioImportButton;
        UI.Label audioImportLabel;
        UI.NinePatchButton bgImageImportButton;
        UI.Label bgImageImportLabel;
        public MetaEditor(Editor.EditorScene root) {
            
            
            this.root = root;
            desktop = new Desktop();
            desktop.HasExternalTextInput = true;
            Global.game.Window.TextInput += (s, a) =>
            {
                desktop.OnChar(a.Character);
            };
            font = SkinLoader.getFont("fonts/main/main.fnt");
            
            songNameLabel = new(new Vector2(100, 95), "Song name",font);
            songNameLabel.localScale = new Vector2(0.15f);

            songName = new TextBox
            {
                Left = 150 / (int)myraScale.X,
                Top = 200 / (int)myraScale.Y,
                Width = 300,
                Height = 30,
                Text = "Song name here...",
                Background = new SolidBrush(new Color(30, 30, 30)),
                TextColor = Color.White
            };
            desktop.Widgets.Add(songName);

            songAuthorLabel = new(songNameLabel.localPosition+new Vector2(0, 95), "Song author's name", font);
            songAuthorLabel.localScale = new Vector2(0.15f);

            songAuthor = new TextBox
            {
                Left = songName.Left,
                Top = songName.Top+70,
                Width = 300,
                Height = 30,
                Text = "Song author name here...",
                Background = new SolidBrush(new Color(30, 30, 30)),
                TextColor = Color.White
            };
            
            desktop.Widgets.Add(songAuthor);

            mapAuthorLabel = new(songAuthorLabel.localPosition + new Vector2(0, 95), "Mapper's name", font);
            mapAuthorLabel.localScale = new Vector2(0.15f);
            
            mapAuthor = new TextBox
            {
                Left = songAuthor.Left,
                Top = songAuthor.Top + 70,
                Width = 300,
                Height = 30,
                Text = "Mapper's name here...",
                Background = new SolidBrush(new Color(30, 30, 30)),
                TextColor = Color.White
            };
            desktop.Widgets.Add(mapAuthor);
            audioImportLabel = new(mapAuthorLabel.localPosition + new Vector2(0, 105), "Audio file", font);
            audioImportLabel.localScale = new Vector2(0.15f);
            audioImportButton = new(SkinLoader.getTexture("9patchbtn.png"), font, 25, "Import audio file...", new Vector2(500, 100), songAuthorLabel.localPosition + new Vector2(205, 275));
            audioImportButton.localScale = Vector2.One/1.25f;
            audioImportButton.textScale = new Vector2(0.25f);

            bgImageImportLabel = new(audioImportLabel.localPosition + new Vector2(0, 125), "Background Image file", font);
            bgImageImportLabel.localScale = new Vector2(0.15f);
            bgImageImportButton = new(SkinLoader.getTexture("9patchbtn.png"), font, 25, "Import BG image...", new Vector2(500, 100), audioImportButton.localPosition + new Vector2(0, 125));
            bgImageImportButton.localScale = Vector2.One / 1.25f;
            bgImageImportButton.textScale = new Vector2(0.25f);
            desktop.Scale = myraScale;
        }
        public void Update(GameTime gameTime)
        {
            audioImportButton.Update(gameTime);
            bgImageImportButton.Update(gameTime);
            audioImportButton.text = root.level.audioFile;
            bgImageImportButton.text = root.level.imageBg;
            if (root.currentTab != Taipu.Editor.EditorScene.EditorTabs.MetaEditor)
            {
                desktop.FocusedKeyboardWidget = null;
                songName.Enabled = false;
                songAuthor.Enabled = false;
                mapAuthor.Enabled = false;
            }
            else
            {
                songName.Enabled = true;
                songAuthor.Enabled = true;
                mapAuthor.Enabled = true;
            }
            if (!songName.IsKeyboardFocused)
            {
                songName.Text = root.level.songName;
            }
            else
            {
                root.level.songName = songName.Text;
            }
            if (!songAuthor.IsKeyboardFocused)
            {
                songAuthor.Text = root.level.songAuthor;
            }
            else
            {
                root.level.songAuthor = songAuthor.Text;
            }
            if (!mapAuthor.IsKeyboardFocused)
            {
                mapAuthor.Text = root.level.mapAuthor;
            }
            else
            {
                root.level.mapAuthor = mapAuthor.Text;
            }
            if (audioImportButton.JustToggled())
            { 
                var openResult = NativeFileDialogSharp.Dialog.FileOpen("mp3,wav,wave,ogg,flac");
                if (openResult.IsOk)
                {
                    audioImportButton.pressed = false;
                    String newFilePath = Path.Combine(Path.GetDirectoryName(root.mapPath), Path.GetFileName(openResult.Path));
                    if (openResult.Path != newFilePath) {
                        try
                        {
                            File.Copy(openResult.Path, newFilePath, true);
                            
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("Error copying audio: " + ex.Message);
                        }
                    }
                    root.level.audioFile = Path.GetFileName(openResult.Path);
                    root.LoadAudio();
                }

            }
            if (bgImageImportButton.JustToggled())
            {
                var openResult = NativeFileDialogSharp.Dialog.FileOpen("png,jpg,jpeg,gif,bmp");
                if (openResult.IsOk)
                {
                    audioImportButton.pressed = false;
                    String newFilePath = Path.Combine(Path.GetDirectoryName(root.mapPath), Path.GetFileName(openResult.Path));
                    if (openResult.Path != newFilePath)
                    {
                        try
                        {
                            File.Copy(openResult.Path, newFilePath, true);

                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("Error copying image: " + ex.Message);
                        }
                    }
                    root.level.imageBg = Path.GetFileName(openResult.Path);
                    root.LoadBackground();
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            
            desktop.Render();
            songNameLabel.Draw(spriteBatch);
            songAuthorLabel.Draw(spriteBatch);
            mapAuthorLabel.Draw(spriteBatch);
            audioImportLabel.Draw(spriteBatch);
            audioImportButton.Draw(spriteBatch);
            bgImageImportButton.Draw(spriteBatch);
            bgImageImportLabel.Draw(spriteBatch);
        }
    }
}
