using System;
using System.Collections;
using System.Collections.Generic;
using mianjoto.Scene;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    #region Singleton Handler
    public static LevelManager Instance { get; private set; }
    void Awake() => Instance = this;
    #endregion

    #region Level System
    public Transform PlayerSpawnPoint { get; private set; }
    public byte CurrentLevel { get; private set; }
    public byte NextLevel { get; private set; }
    public const byte MAIN_MENU_LEVEL = 100;
    public const byte LAST_LEVEL = 10;
    public const byte GAME_COMPLETE_LEVEL = 200;
    public const string PLAYER_SPAWN_POINT_TAG = "SpawnPoint";
    #endregion

    #region GameManager Subscription
    void OnEnable()
    {
        GameManager.OnLevelComplete += ProceedLevel;
    }
    void OnDisable()
    {
        GameManager.OnLevelComplete -= ProceedLevel;
    }
    #endregion
    
    void Start()
    {
        if (CurrentLevel == 0)
            CurrentLevel = 1;
        
        NextLevel = GetNextLevel();
        PlayerSpawnPoint = GetPlayerSpawnPoint();
    }

    public byte GetNextLevel()
    {
        if (CurrentLevel == LAST_LEVEL)
            return GAME_COMPLETE_LEVEL;
        else
            return (byte)(CurrentLevel + 1);
    } 

    public Transform GetPlayerSpawnPoint()
    {
        return GameObject.FindGameObjectWithTag(PLAYER_SPAWN_POINT_TAG).transform;
    }

    public byte GetNumberOfEnemiesInLevel()
    {
        return ((byte)FindObjectsOfType<BaseEnemyStateMachine>().Length);
    }


    void ProceedLevel()
    {
        CurrentLevel = NextLevel;
        NextLevel = GetNextLevel();

        if (NextLevel == GAME_COMPLETE_LEVEL)
            SceneLoader.Instance.LoadGameComplete();
        else
            Debug.Log("Proceeding to Level " + CurrentLevel);
        
        SceneLoader.Instance.LoadLevel(CurrentLevel);
    }
    
}
