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

        [SerializeField] GameObject loadingScreen;
        Camera loadingScreenCamera;
        Animator loadingScreenAnimator;

        Camera currentMainCamera;

        const string LEVEL_SCENE_PREFIX = "Level";

        List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
        public IEnumerator LoadWithLoadingScreen(string sceneName)
        {
            Debug.Log("in loading script");
            currentMainCamera = Camera.main;

            yield return StartCoroutine(ShowLoadingScreen());

            // Get scene references
            var nextSceneOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            // nextSceneOperation.allowSceneActivation = false;
            yield return nextSceneOperation;

            var currentSceneOperation = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            // Load and unload scenes

            scenesLoading.Add(currentSceneOperation);
            scenesLoading.Add(nextSceneOperation);

            // yield return StartCoroutine(GetSceneLoadProgress());
            
            yield return new WaitForSeconds(3f);
            // Show new scene
            yield return StartCoroutine(HideLoadingScreen());
            nextSceneOperation.allowSceneActivation = true;

        }

        IEnumerator GetSceneLoadProgress()
        {
            foreach (AsyncOperation sceneOperation in scenesLoading)
            {
                if (sceneOperation == null) continue;

                while (!sceneOperation.isDone)
                {
                    yield return null;
                }
            }

            scenesLoading.Clear();
            yield return null;
        }

        public void LoadSceneWithLoadingScreen(Scenes scene)
        {
            LoadWithLoadingScreen(scene.ToString());
        }

        public void LoadLevel(int levelNumber)
        {
            StartCoroutine(LoadWithLoadingScreen(LEVEL_SCENE_PREFIX + levelNumber.ToString()));
        }

        public void LoadNextLevel()
        {
            LoadWithLoadingScreen(LEVEL_SCENE_PREFIX + LevelManager.Instance.NextLevel.ToString());
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

        IEnumerator ShowLoadingScreen()
        {
            loadingScreen.gameObject.SetActive(true);

            if (loadingScreenAnimator == null)
                loadingScreenAnimator = loadingScreen.GetComponentInChildren<Animator>();

            if (loadingScreenCamera == null)
                loadingScreenCamera = loadingScreen.GetComponentInChildren<Camera>();

            currentMainCamera.tag = "Untagged";
            loadingScreenCamera.tag = "MainCamera";

            yield return new WaitForSeconds(1);
        }

        IEnumerator HideLoadingScreen()
        {
            if (loadingScreenAnimator != null)
            {
                loadingScreenAnimator.SetTrigger("SlideDown");
                yield return new WaitForSeconds(1f);
            }

            currentMainCamera = Camera.main;
            currentMainCamera.tag = "MainCamera";

            if (loadingScreenCamera != null)
                loadingScreenCamera.tag = "Untagged";

            loadingScreen.gameObject.SetActive(false);

        }
    }
}
