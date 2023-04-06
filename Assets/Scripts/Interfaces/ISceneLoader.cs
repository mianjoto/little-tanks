namespace mianjoto.Scene
{
    public interface ISceneLoader
    {
        void LoadNextLevel(int currentLevelNumber);
        void LoadScene(Scenes scene);
        void LoadMainMenu();
        void LoadGameOver();
        void LoadGameComplete();
    }
}