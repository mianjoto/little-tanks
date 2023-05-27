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

        void Start()
        {
            loadingScreen = loadingScreenObject.GetComponent<LoadingScreen>();
            loadingScreenCamera = loadingScreenObject.GetComponentInChildren<Camera>();
            loadingScreenAnimator = loadingScreenObject.GetComponentInChildren<Animator>();
        }

        [SerializeField] GameObject loadingScreenObject;
        LoadingScreen loadingScreen;
        Camera loadingScreenCamera;
        Animator loadingScreenAnimator;

        Camera currentMainCamera;

        const string LEVEL_SCENE_PREFIX = "Level";

        public IEnumerator LoadWithLoadingScreen(string sceneName)
        {
            currentMainCamera = Camera.main;

            yield return StartCoroutine(ShowLoadingScreen());

            // Load new scene before unloading current
            var nextSceneOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            yield return nextSceneOperation;

            var currentSceneOperation = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

            // Give time for tooltip to display
            yield return new WaitForSeconds(3f);

            yield return StartCoroutine(HideLoadingScreen());
        }

        public static bool IsInLevelScene()
        {
            string sceneName = SceneManager.GetActiveScene().name;
            return sceneName.Contains(LEVEL_SCENE_PREFIX);
        }

        public static byte GetCurrentLevelNumber()
        {
            string sceneName = SceneManager.GetActiveScene().name;
            if (sceneName.Contains(LEVEL_SCENE_PREFIX))
            {
                string levelNumber = sceneName.Substring(LEVEL_SCENE_PREFIX.Length);
                return byte.Parse(levelNumber);
            }
            else
            {
                Debug.LogError("Scene name does not contain level prefix");
                return 0;
            }
        }

        public static byte GetNextLevelNumber()
        {
            byte currentLevel = GetCurrentLevelNumber();
            if (currentLevel == 10)
                return 200;
            else
                return (byte)(currentLevel + 1);
        }

        IEnumerator ShowLoadingScreen()
        {
            loadingScreenObject.gameObject.SetActive(true);

            currentMainCamera.tag = "Untagged";
            loadingScreenCamera.tag = "MainCamera";

            yield return new WaitForSeconds(1);
        }

        IEnumerator HideLoadingScreen()
        {
            loadingScreenAnimator.SetTrigger("SlideDown");
            yield return new WaitForSeconds(1f);

            currentMainCamera = Camera.main;
            currentMainCamera.tag = "MainCamera";

            loadingScreenCamera.tag = "Untagged";

            loadingScreenObject.SetActive(false);
        }

        public void LoadLevel(byte levelNumber)
        {
            StartCoroutine(LoadWithLoadingScreen(LEVEL_SCENE_PREFIX + levelNumber));
            loadingScreen.UpdateLoadingScreen(levelNumber);
        }

        public void LoadLevel(string sceneName)
        {
            loadingScreen.UpdateLoadingScreen(GetNextLevelNumber());
            StartCoroutine(LoadWithLoadingScreen(sceneName));
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
    }
}
