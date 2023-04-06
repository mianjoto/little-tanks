using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace mianjoto.Scene
{
    public class SceneLoader : MonoBehaviour, ISceneLoader
    {
        #region Singleton Handler
        public static SceneLoader Instance { get; private set; }
        void Awake() => Instance = this;
        #endregion

        const string LEVEL_SCENE_PREFIX = "Level";

        public void LoadScene(int levelNumber)
        {
            SceneManager.LoadSceneAsync(LEVEL_SCENE_PREFIX + levelNumber);
        }

        public void LoadMainMenu()
        {
            SceneManager.LoadScene(Scenes.MainMenu.ToString());
        }

        public void LoadGameOver()
        {
            SceneManager.LoadScene(Scenes.GameOver.ToString());
        }

        public void LoadGameComplete()
        {
            SceneManager.LoadScene(Scenes.GameComplete.ToString());
        }

        public void LoadNextLevel(int currentLevelNumber)
        {
            LoadNextLevel(currentLevelNumber + 1);
        }

        public void LoadScene(Scenes scene)
        {
            if (SceneManager.GetActiveScene().name != scene.ToString())
                SceneManager.LoadScene(scene.ToString());
        }
    }
}
