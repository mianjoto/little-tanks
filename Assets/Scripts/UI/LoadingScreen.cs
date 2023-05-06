using System.Collections;
using UnityEngine;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] LevelsData levelsData;
    [SerializeField] TMP_Text levelTooltip;
    [SerializeField] TMP_Text levelNumber;
    string[] levelNames;

    public void UpdateLoadingScreen(byte levelNumber)
    {
        if (levelNames == null)
            levelNames = levelsData.LevelNames.ToArray();

        levelTooltip.text = levelNames[levelNumber-1];
        this.levelNumber.text = levelNumber.ToString();
    }

}
