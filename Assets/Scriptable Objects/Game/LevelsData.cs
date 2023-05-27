using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelsData", menuName = "Scriptable Objects/Game/Levels Data")]
public class LevelsData : ScriptableObject
{
    // Not serialized, but public for access in other scripts
    [HideInInspector] public Dictionary<string, byte> LevelInformation = new Dictionary<string, byte>();
    
    [SerializeField] public List<string> LevelNames = new List<string>();

    void OnEnable()
    {
        for (byte i = 0; i < LevelNames.Count; i++)
        {
            LevelInformation.Add(LevelNames[i], i);
        }
    }

    public byte GetLevelNumber(string levelName)
    {
        return LevelInformation[levelName];
    }

    public string GetLevelName(byte levelNumber)
    {
        foreach (var level in LevelInformation)
        {
            if (level.Value == levelNumber)
                return level.Key;
        }
        return null;
    }
}
