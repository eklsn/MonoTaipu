namespace Taipu
{
    public static class SceneManager
    {
        public static Scene currentScene;
        
        public static void LoadScene(Scene scene)
        {
            scene.Load();
            currentScene = null;
            currentScene = scene;
        }
    }
}
