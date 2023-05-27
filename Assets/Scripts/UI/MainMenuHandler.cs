using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    public static void LoadGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public static void QuitGame()
    {
        Application.Quit();
    }
}
