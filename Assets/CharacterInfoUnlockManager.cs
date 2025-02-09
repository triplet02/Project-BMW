using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoUnlockManager : MonoBehaviour
{

    public Button[] characterButton;
    public GameObject[] lockIcon;
    string[] characterAchives = { "UnlockMouse", "UnlockDog" };

    // Start is called before the first frame update
    void Start()
    {
        UnlockCharacter();
    }

    // Update is called once per frame
    void UnlockCharacter()
    {
        for (int i = 0; i < characterButton.Length; i++)
        {
            string achiveName = characterAchives[i].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;
            if (isUnlock)
            {
                characterButton[i].interactable = true;
                lockIcon[i].SetActive(false);
            }
        }
    }
}
