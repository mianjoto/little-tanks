namespace mianjoto.Scene
{
    public interface ISceneLoader
    {
        void LoadLevel(string sceneName);
        void LoadLevel(byte levelNumber);
        void LoadMainMenu();
        void LoadGameOver();
        void LoadGameComplete();
    }
}