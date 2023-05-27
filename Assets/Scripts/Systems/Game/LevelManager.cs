using System;
using System.Collections;
using System.Collections.Generic;
using mianjoto.Scene;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    #region Singleton Handler
    public static LevelManager Instance { get; private set; }
    void Awake() => Instance = this;
    #endregion

    #region Level System
    public Transform PlayerSpawnPoint { get; private set; }
    public const byte MAIN_MENU_LEVEL = 100;
    public const byte LAST_LEVEL = 10;
    public const byte GAME_COMPLETE_LEVEL = 200;
    public const string PLAYER_SPAWN_POINT_TAG = "SpawnPoint";
    #endregion

    void OnEnable()
    {
        SceneManager.sceneLoaded += InitializeLevel;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= InitializeLevel;
    }
    
    void InitializeLevel(Scene scene, LoadSceneMode mode)
    {
        if (!SceneLoader.IsInLevelScene())
            return;

        PlayerSpawnPoint = GetPlayerSpawnPoint();
    }

    public Transform GetPlayerSpawnPoint()
    {
        return GameObject.FindGameObjectWithTag(PLAYER_SPAWN_POINT_TAG)?.transform;
    }

    public byte GetNumberOfEnemiesInLevel()
    {
        return ((byte)FindObjectsOfType<BaseEnemyStateMachine>().Length);
    }    
}
