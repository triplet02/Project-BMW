using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageUnlockManager : MonoBehaviour
{
    public GameObject[] stageImages;
    public GameObject[] stageLockLines;
    public GameObject[] stageNames;
    string[] stageAchives = { "UnlockAfternoon", "UnlockNight" };

    // Start is called before the first frame update
    void Start()
    {
        UnlockStage();
    }

    void UnlockStage()
    {
        for (int i = 0; i < stageImages.Length; i++)
        {
            string achiveName = stageAchives[i].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;
            stageImages[i].SetActive(isUnlock);
            stageLockLines[i].SetActive(!isUnlock);
            stageNames[i].SetActive(isUnlock);
        }
    }
}
