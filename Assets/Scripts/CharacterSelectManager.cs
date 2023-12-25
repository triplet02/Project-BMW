using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectManager : MonoBehaviour
{
    public GameObject[] characterGroup;
    public GameObject[] jumpButtons;
    public GameObject[] slideButtons;
    public GameObject[] skillGauges;

    // Start is called before the first frame update
    void Start()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        for(int i = 0; i < characterGroup.Length; i++)
        {
            if(CharacterInfo.characterNumber == i + 1)
            {
                characterGroup[i].SetActive(true);
                skillGauges[i].SetActive(true);
                if (currentSceneName.Contains("SideView")){
                    jumpButtons[i].SetActive(true);
                    slideButtons[i].SetActive(true);
                }
            }
            else
            {
                characterGroup[i].SetActive(false);
                skillGauges[i].SetActive(false);
                if (currentSceneName.Contains("SideView"))
                {
                    jumpButtons[i].SetActive(false);
                    slideButtons[i].SetActive(false);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
