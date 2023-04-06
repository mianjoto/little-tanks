namespace mianjoto.Scene
{
    public interface ISceneLoader
    {
        void LoadScene(Scenes scene);
        void LoadLevel(int levelNumber);
        void LoadMainMenu();
        void LoadGameOver();
        void LoadGameComplete();
    }
}