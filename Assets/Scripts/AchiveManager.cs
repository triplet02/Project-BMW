using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchiveManaver : MonoBehaviour
{
    public GameObject[] lockCharacter;
    public GameObject[] unlockCharacter;

    enum CharacterAchive { UnlockMouse, UnlockDog }
    CharacterAchive[] characterAchives;

    enum StageAchive { UnlockAfternoon, UnlockNight }
    StageAchive[] stageAchives;

    // Start is called before the first frame update
    void Awake()
    {
        characterAchives = (CharacterAchive[])Enum.GetValues(typeof(CharacterAchive));
        stageAchives = (StageAchive[])Enum.GetValues(typeof(StageAchive));

        if (!PlayerPrefs.HasKey("Data"))
        {
            InitAchives();
        }
    }

    void InitAchives()
    {
        PlayerPrefs.SetInt("Data", 1);
        PlayerPrefs.SetInt("Score", 0);
        PlayerPrefs.SetInt("Stage", 0);
        PlayerPrefs.SetInt("Character", 0);
        PlayerPrefs.SetInt("Health", 0);
        PlayerPrefs.SetInt("SkillGauge", 0);
        PlayerPrefs.SetInt("CriticalVFXPlayed", 0);

        foreach (CharacterAchive characterAchive in characterAchives)
        {
            PlayerPrefs.SetInt(characterAchive.ToString(), 0);
        }

        foreach (StageAchive stageAchive in stageAchives)
        {
            PlayerPrefs.SetInt(stageAchive.ToString(), 0);
        }
    }
}
