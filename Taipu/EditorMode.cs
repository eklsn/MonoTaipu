using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Taipu
{
    public class EditorMode : Scene
    {
        KeyManager keyMan;
        KeyObject key1;
        JukeboxSynced jbox;
        public void Load()
        {
            keyMan = new();
            jbox = new();
            key1 = new(new Vector2(386),Vector2.One/2f);
            jbox.LoadStream("D:/Taipu/test.mp3");
            jbox.Play();
        }
        public void Update()
        {
            key1.Update();
            Debug.WriteLine(jbox.streamPosition);
        }
        public void Draw()
        {
            key1.Draw();
        }
    }
}
