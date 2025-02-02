using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class ViewChangeController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    Scene currentScene;
    // Start is called before the first frame update
    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("View Convert Video Ended");
        if (currentScene.name.Equals("SideView to TopView"))
        {
            Debug.Log("Side to Top : " + StageInfo.stageNumber.ToString());
            SceneManager.LoadScene("TopView Gameplay " + StageInfo.stageNumber.ToString());
        }
        else
        {
            Debug.Log("Top to Side : " + StageInfo.stageNumber.ToString());
            string currentSceneName = SceneManager.GetActiveScene().name;

            switch (currentSceneName)
            {
                case "TopView Gameplay 1":
                    SideViewGameplay1.sideViewGameplay1.currentMapIdx = 12;
                    break;
                case "TopView Gameplay 2":
                    SideViewGameplay1.sideViewGameplay1.currentMapIdx = 12;
                    break;
                case "TopView Gameplay 3":
                    SideViewGameplay1.sideViewGameplay1.currentMapIdx = 12;
                    break;
            }
            SceneManager.LoadScene("SideView Gameplay " + StageInfo.stageNumber.ToString());
        }
    }

    public void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoEnd;
        }
    }
}
