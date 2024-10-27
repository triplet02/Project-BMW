using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ViewConvertingManager : MonoBehaviour
{
    [SerializeField] float viewConvertingTime;

    // Start is called before the first frame update
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name.Equals("SideView to TopView"))
        {
            StartCoroutine(SideViewToTopView());
        }
        else
        {
            StartCoroutine(TopViewToSideView());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SideViewToTopView()
    {
        yield return new WaitForSeconds(viewConvertingTime);
        SceneManager.LoadScene("TopView Gameplay " + StageInfo.stageNumber.ToString());
    }

    IEnumerator TopViewToSideView()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        switch (currentSceneName)
        {
            case "TopView Gameplay 1" :
                SideViewGameplay1.sideViewGameplay1.currentMapIdx = 12;
                break;
            case "TopView Gameplay 2":
                SideViewGameplay1.sideViewGameplay1.currentMapIdx = 12;
                break;
            case "TopView Gameplay 3":
                SideViewGameplay1.sideViewGameplay1.currentMapIdx = 12;
                break;
        }
        yield return new WaitForSeconds(viewConvertingTime);
        SceneManager.LoadScene("SideView Gameplay " + StageInfo.stageNumber.ToString());
    }
}
