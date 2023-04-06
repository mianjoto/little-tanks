using System;
using System.Collections;
using System.Collections.Generic;
using mianjoto.Scene;
using UnityEngine;

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
    #endregion

    void OnEnable()
    {
        BaseEnemyStateMachine.OnEnemyDeath += UpdateEnemyCount;
        TankManager.OnPlayerDeath += HandlePlayerDeath;
    }

    void OnDisable()
    {
        BaseEnemyStateMachine.OnEnemyDeath -= UpdateEnemyCount;
        TankManager.OnPlayerDeath -= HandlePlayerDeath;
    }

    void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        if (Player == null)
            Player = GameObject.FindGameObjectWithTag(PLAYER_TAG);
        Debug.Log("GameManager: Player=" + Player);

        if (NumberOfLivesRemaining == 0)
            NumberOfLivesRemaining = MAX_NUMBER_OF_LIVES;
        Debug.Log("GameManager: NumberOfLivesRemaining=" + NumberOfLivesRemaining);

        NumberOfEnemiesRemaining = LevelManager.Instance.GetNumberOfEnemiesInLevel();
        Player.transform.position = LevelManager.Instance.PlayerSpawnPoint.position;
        Debug.Log("GameManager: Loaded in Level" + LevelManager.Instance.CurrentLevel);
    }

    void UpdateEnemyCount()
    {
        NumberOfEnemiesRemaining--;
        NumberOfEnemyKills++;
        if (NumberOfEnemiesRemaining == 0)
            OnLevelComplete?.Invoke();
    }

    void HandlePlayerDeath()
    {
        // Just for testing :)
        NumberOfLivesRemaining--;

        if (NumberOfLivesRemaining == 0)
            SceneLoader.Instance.LoadGameOver();
        else
            SceneLoader.Instance.LoadScene(LevelManager.Instance.CurrentLevel);
    }
}
