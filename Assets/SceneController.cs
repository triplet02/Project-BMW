using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SceneController : MonoBehaviour
{
    Dictionary<string, int> stageDict = new Dictionary<string, int>()
    {
        {"Stage1Button", 1 },
        {"Stage2Button", 2 },
        {"Stage3Button", 3 },
    };

    Dictionary<string, int> characterDict = new Dictionary<string, int>()
    {
        {"Character1Button", 1 },
        {"Character2Button", 2 },
        {"Character3Button", 3 },
    };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toGameplayScene()
    {
        SceneManager.LoadScene("Stage Selection");
    }

    public void toGameoverScene()
    {
        SceneManager.LoadScene("Gameover");
    }

    public void moveStage()
    {
        GameObject clickedObject = EventSystem.current.currentSelectedGameObject;
        StageInfo.stageNumber = stageDict[clickedObject.name];
        Debug.Log(StageInfo.stageNumber.ToString());
        SceneManager.LoadScene("Character Selection");
    }

    public void toSideViewGameplayScene()
    {
        GameObject clickedObject = EventSystem.current.currentSelectedGameObject;
        CharacterInfo.characterNumber = characterDict[clickedObject.name];
        SceneManager.LoadScene("SideView Gameplay " + StageInfo.stageNumber.ToString());
    }
}
