using System;
using System.Collections;
using System.Collections.Generic;
using mianjoto.Scene;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton Handler
    public static GameManager Instance { get; private set; }
    void Awake() => Instance = this;
    #endregion

    #region Player Statistics
    public GameObject Player { get; private set;}
    const string PLAYER_TAG = "Player";
    public byte NumberOfLivesRemaining { get; private set; }
    const byte MAX_NUMBER_OF_LIVES = 3;
    public byte NumberOfEnemiesRemaining { get; private set; }
    public byte NumberOfEnemyKills { get; private set; }
    #endregion

    #region Level System
    public static Action OnLevelComplete;
    public byte CurrentLevel { get; private set; }
    #endregion

    void OnEnable()
    {
        SceneManager.sceneLoaded += InitializeLevel;
        BaseEnemyStateMachine.OnEnemyDeath += UpdateEnemyCount;
        PlayerManager.OnPlayerDeath += HandlePlayerDeath;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= InitializeLevel;
        BaseEnemyStateMachine.OnEnemyDeath -= UpdateEnemyCount;
        PlayerManager.OnPlayerDeath -= HandlePlayerDeath;
    }

    private void InitializeLevel(Scene scene, LoadSceneMode mode)
    {
        if (!scene.name.Contains("Level"))
            return;
        
        if (CurrentLevel == 0 || CurrentLevel > LevelManager.LAST_LEVEL)
            CurrentLevel = SceneLoader.GetCurrentLevelNumber();
        
        if (Player == null)
            Player = GameObject.FindGameObjectWithTag(PLAYER_TAG);

        if (NumberOfLivesRemaining == 0)
            NumberOfLivesRemaining = MAX_NUMBER_OF_LIVES;

        NumberOfEnemiesRemaining = LevelManager.Instance.GetNumberOfEnemiesInLevel();
        Player.transform.position = LevelManager.Instance.PlayerSpawnPoint.position;
    }

    void UpdateEnemyCount()
    {
        NumberOfEnemiesRemaining--;
        NumberOfEnemyKills++;
        if (NumberOfEnemiesRemaining == 0)
            ProceedLevel();
    }

    void ProceedLevel()
    {
        OnLevelComplete?.Invoke();

        byte nextLevel = (byte)(CurrentLevel + 1);

        if (CurrentLevel >= LevelManager.LAST_LEVEL)
            SceneLoader.Instance.LoadGameComplete();
        else
            SceneLoader.Instance.LoadLevel(nextLevel);

        CurrentLevel = nextLevel;
    }

    void HandlePlayerDeath()
    {
        // Just for testing :)
        NumberOfLivesRemaining--;
        Debug.Log("Player died :(");
        Debug.Log("Number of lives remaining: " + NumberOfLivesRemaining);

        if (NumberOfLivesRemaining == 0)
            SceneLoader.Instance.LoadGameOver();
        else
            SceneLoader.Instance.LoadLevel(CurrentLevel);
    }
}
