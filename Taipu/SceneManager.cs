using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taipu
{
    public static class SceneManager
    {
        public static Scene currentScene;
        
        public static void LoadScene(Scene scene)
        {
            scene.Load();
            currentScene = scene;
        }
    }
}
