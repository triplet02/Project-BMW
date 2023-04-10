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
        {"CatButton", 1 },
        {"Dog2Button", 2 },
        {"MouseButton", 3 },
    };

    bool isPaused = false;

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
        SceneManager.LoadScene("SideView Gameplay " + StageInfo.stageNumber.ToString());
    }

    public void characterSelection()
    {
        GameObject clickedObject = EventSystem.current.currentSelectedGameObject;
        CharacterInfo.characterNumber = characterDict[clickedObject.name];
    }

    public void gamePause()
    {
        if(isPaused){
            Time.timeScale = 1;
            isPaused = false;
        }
        else{
            Time.timeScale = 0;
            isPaused = true;
        }
    }

    public void toTopViewScene()
    {
        SceneManager.LoadScene("TopView Gameplay");
    }

    public void toSideViewScene()
    {
        SceneManager.LoadScene("SideView Gameplay 1");
    }

    public void toCharacterSelectionScene()
    {
        SceneManager.LoadScene("Character Selection");
    }

    public void toLobby()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void toCharacterSettingsScene()
    {
        SceneManager.LoadScene("Character Settings");
    }
}
