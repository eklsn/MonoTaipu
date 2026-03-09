using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taipu.Editor.Tabs
{
    public class Export
    {
        Editor.EditorScene root = null;
        //Editor.EditorScene.EditorTabs me = Editor.EditorScene.EditorTabs.Export;
        public Export(Editor.EditorScene root) { this.root = root; }
        public void Update(GameTime gameTime) { }
        public void Draw(SpriteBatch spriteBatch) { }
    }
}
