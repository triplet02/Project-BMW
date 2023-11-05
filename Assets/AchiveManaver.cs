using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchiveManaver : MonoBehaviour
{
    public GameObject[] lockCharacter;
    public GameObject[] unlockCharacter;

    enum Achive { UnlockMouse, UnlockDog }
    Achive[] achives;

    // Start is called before the first frame update
    void Awake()
    {
        achives = (Achive[])Enum.GetValues(typeof(Achive));

        if (!PlayerPrefs.HasKey("Data"))
        {
            InitAchives();
        }
    }

    void InitAchives()
    {
        PlayerPrefs.SetInt("Data", 1);

        foreach(Achive achive in achives)
        {
            PlayerPrefs.SetInt(achive.ToString(), 1);
        }
    }

    void Start()
    {
        UnlockCharacter();
    }

    void UnlockCharacter()
    {
        for (int i = 0; i < lockCharacter.Length; i++)
        {
            string achiveName = achives[i].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;
            lockCharacter[i].SetActive(!isUnlock);
            unlockCharacter[i].SetActive(isUnlock);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
