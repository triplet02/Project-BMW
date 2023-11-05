using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SceneController : MonoBehaviour
{
    Dictionary<string, int> stageDict = new Dictionary<string, int>()
    {
        {"Stage1", 1 },
        {"Stage2", 2 },
        {"Stage3", 3 },
    };

    Dictionary<string, int> characterDict = new Dictionary<string, int>()
    {
        {"CatButton", 1 },
        {"MouseButton", 2 },
        {"DogButton", 3 },
    };

    bool isPaused = false;
    GameObject AudioManager; 

    private void Awake()
    {
        AudioManager = GameObject.Find("AudioManager");
        DontDestroyOnLoad(AudioManager);
    }

    // Start is called before the first frame update
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        Debug.Log(currentScene.name.ToString());
        AudioManager.GetComponent<AudioManager>().PlayBGM();
        SetScreenDirection();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toStageSelectionScene()
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
        Debug.Log(CharacterInfo.characterNumber.ToString());
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

    public void toOpeningScene()
    {
        SceneManager.LoadScene("Opening");
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

    public void toGameClearScene()
    {
        SceneManager.LoadScene("GameClear");
    }

    public void SetScreenDirection()
    {
        Scene CurrentScene = SceneManager.GetActiveScene();
        if (CurrentScene.name.Contains("TopView"))
        {
            Screen.orientation = ScreenOrientation.Portrait;
        }
        else
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }
    }
}
