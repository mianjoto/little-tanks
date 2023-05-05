namespace mianjoto.Scene
{
    public interface ISceneLoader
    {
        void LoadSceneWithLoadingScreen(string sceneName);
        void LoadSceneImmediately(string sceneName);
        void LoadLevel(int levelNumber);
        void LoadLevelImmediately(int levelNumber);
        void LoadMainMenu();
        void LoadGameOver();
        void LoadGameComplete();
    }
}