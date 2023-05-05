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
        const string LOADING_SCENE_NAME = "Loading";

        public IEnumerator LoadSceneWithLoadingScreen(string sceneName)
        {
            string oldSceneName = SceneManager.GetActiveScene().name;
            if (SceneManager.GetActiveScene().name == sceneName)
                yield return null;

            var loadingScene = SceneManager.LoadSceneAsync(LOADING_SCENE_NAME, LoadSceneMode.Additive);
            Camera.main.gameObject.SetActive(false);

            do
            {
                yield return null;
            } while (!loadingScene.isDone);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(LOADING_SCENE_NAME));
            Animator loadingAnimator = GameObject.FindGameObjectWithTag("Loading").GetComponent<Animator>();

            var scene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync(oldSceneName);
            scene.allowSceneActivation = false;
            do
            {
                Debug.Log("loading progress:" + scene.progress);
                yield return null;
            } while (scene.progress < 0.9f);

            Debug.Log("loading animator:" + loadingAnimator);
            loadingAnimator.SetTrigger("SlideDown");
            yield return new WaitForSeconds(5f);

            scene.allowSceneActivation = true;

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
            SceneManager.UnloadSceneAsync(LOADING_SCENE_NAME);
        }

        public void LoadSceneWithLoadingScreen(Scenes scene)
        {
            LoadSceneWithLoadingScreen(scene.ToString());
        }

        public void LoadLevel(int levelNumber)
        {
            StartCoroutine(LoadSceneWithLoadingScreen(LEVEL_SCENE_PREFIX + levelNumber.ToString()));
        }

        public void LoadNextLevel()
        {
            LoadSceneWithLoadingScreen(LEVEL_SCENE_PREFIX + LevelManager.Instance.NextLevel.ToString());
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

        void ISceneLoader.LoadSceneWithLoadingScreen(string sceneName)
        {
            throw new NotImplementedException();
        }

        public void LoadSceneImmediately(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void LoadLevelImmediately(int levelNumber)
        {
            SceneManager.LoadScene(LEVEL_SCENE_PREFIX + levelNumber.ToString());
        }
    }
}
