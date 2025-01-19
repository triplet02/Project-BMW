using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    [SerializeField]
    public GameObject[] characterImageList;
    public GameObject[] characterButtonList;

    [SerializeField] float viewConvertingTime;

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
        PlayerPrefs.SetInt("Stage", 0);
        SceneManager.LoadScene("Stage Selection");
    }

    public void toGameoverScene()
    {
        SceneManager.LoadScene("Gameover");
    }

    public void moveStage()
    {
        bool readyToMove = false;
        GameObject clickedObject = EventSystem.current.currentSelectedGameObject;
        int stageNum = stageDict[clickedObject.name];
        if (stageNum == 1)
        {
            StageInfo.stageNumber = stageNum;
            readyToMove = true;
        }
        else
        {
            if (stageNum == 2 && PlayerPrefs.GetInt("UnlockAfternoon") == 1)
            {
                StageInfo.stageNumber = stageNum;
                readyToMove = true;
            }
            if (stageNum == 3 && PlayerPrefs.GetInt("UnlockNight") == 1)
            {
                StageInfo.stageNumber = stageNum;
                readyToMove = true;
            }
        }
        Debug.Log(StageInfo.stageNumber.ToString());
        if (readyToMove)
        {
            SceneManager.LoadScene("Character Selection");
        }

    }

    public void toSideViewGameplayScene()
    {
        if(CharacterInfo.characterNumber != 0)
        {
            PlayerPrefs.SetInt("Stage", 0);
            PlayerPrefs.SetInt("CriticalVFXPlayed", 0);
            SideViewGameplay1.sideViewGameplay1.skillValue = 0;
            SideViewGameplay1.sideViewGameplay1.maxHealth = 3;
            SideViewGameplay1.sideViewGameplay1.playerHealth = SideViewGameplay1.sideViewGameplay1.maxHealth;
            SideViewGameplay1.sideViewGameplay1.coin = 0;
            SideViewGameplay1.sideViewGameplay1.currentView = "side";
            SideViewGameplay1.sideViewGameplay1.currentMapIdx = 0;
            SceneManager.LoadScene("SideView Gameplay " + StageInfo.stageNumber.ToString());
        }
    }

    public void characterSelection()
    {
        GameObject clickedObject = EventSystem.current.currentSelectedGameObject;
        CharacterInfo.characterNumber = characterDict[clickedObject.name];
        Debug.Log(CharacterInfo.characterNumber.ToString());

        for(int i=0; i<characterImageList.Length; i++)
        {
            if(i == CharacterInfo.characterNumber - 1)
            {
                characterImageList[i].SetActive(true);
                characterButtonList[i].GetComponentsInChildren<Image>()[1].color = new Color(0,0,0,255/255f);
            }
            else
            {
                characterImageList[i].SetActive(false);
                characterButtonList[i].GetComponentsInChildren<Image>()[1].color = new Color(0, 0, 0, 60 / 255f);
            }
        }
    }

    public void gamePause()
    {
        if (isPaused)
        {
            Time.timeScale = 1;
            isPaused = false;
        }
        else
        {
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
        switch (SceneManager.GetActiveScene().name)
        {
            case "SideView Gameplay 1":
                SceneManager.LoadScene("TopView Gameplay 1");
                break;
            case "SideView Gameplay 2":
                SceneManager.LoadScene("TopView Gameplay 2");
                break;
            case "SideView Gameplay 3":
                SceneManager.LoadScene("TopView Gameplay 3");
                break;
        }
    }

    public void toSideViewScene()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "TopView Gameplay 1":
                SceneManager.LoadScene("SideView Gameplay 1");
                break;
            case "TopView Gameplay 2":
                SceneManager.LoadScene("SideView Gameplay 2");
                break;
            case "TopView Gameplay 3":
                SceneManager.LoadScene("SideView Gameplay 3");
                break;
        }
    }

    public void toCharacterSelectionScene()
    {
        SceneManager.LoadScene("Character Selection");
    }

    public void toLobby()
    {
        PlayerPrefs.SetInt("StageNum", 0);
        SceneManager.LoadScene("Lobby");
    }

    public void toTutorial()
    {
        //SceneManager.LoadScene("Tutorial");

    }

    public void toGameClearScene()
    {
        PlayerPrefs.SetInt("Stage", StageInfo.stageNumber);
        SceneManager.LoadScene("GameClear");
    }

    public void toSideViewToTopViewScene()
    {
        SceneManager.LoadScene("SideView to TopView");
    }

    public void toTopViewToSideViewScene()
    {
        SceneManager.LoadScene("TopView to SideView");
    }

    public void toEndingScene()
    {
        SceneManager.LoadScene("Ending");
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

    public void QuitGame()
    {
        Application.Quit();
    }

    public void _ToTestScene()
    {
        SceneManager.LoadScene("TestScene");
    }
}
