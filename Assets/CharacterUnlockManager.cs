using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUnlockManager : MonoBehaviour
{
    public GameObject[] lockCharacter;
    public GameObject[] unlockCharacter;
    enum CharacterAchive { UnlockMouse, UnlockDog }
    CharacterAchive[] characterAchives;

    // Start is called before the first frame update
    void Start()
    {
        UnlockCharacter();
    }

    void UnlockCharacter()
    {
        for (int i = 0; i < lockCharacter.Length; i++)
        {
            string achiveName = characterAchives[i].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;
            lockCharacter[i].SetActive(!isUnlock);
            unlockCharacter[i].SetActive(isUnlock);
        }
    }
}
