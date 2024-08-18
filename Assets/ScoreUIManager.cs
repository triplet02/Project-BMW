using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreUIManager : MonoBehaviour
{
    public GameObject[] stars;

    TextMeshProUGUI score;
    int playerScore;

    // Start is called before the first frame update
    void Start()
    {
        score = GameObject.Find("Label_Score").GetComponent<TextMeshProUGUI>();
        score.text = PlayerPrefs.GetInt("Score").ToString();

        if (SceneManager.GetActiveScene().name.Contains("Clear")){
            switch (PlayerPrefs.GetInt("Stage"))
            {
                case 1:
                    if (PlayerPrefs.GetInt("Score") >= 1000)
                    {
                        stars[0].GetComponent<Image>().color = Color.white;
                    }
                    if (PlayerPrefs.GetInt("Score") >= 2000)
                    {
                        stars[1].GetComponent<Image>().color = Color.white;
                    }
                    if (PlayerPrefs.GetInt("Score") >= 3000)
                    {
                        stars[2].GetComponent<Image>().color = Color.white;
                    }
                    break;
                case 2:
                    if (PlayerPrefs.GetInt("Score") >= 1000)
                    {
                        stars[0].GetComponent<Image>().color = Color.white;
                    }
                    if (PlayerPrefs.GetInt("Score") >= 2000)
                    {
                        stars[1].GetComponent<Image>().color = Color.white;
                    }
                    if (PlayerPrefs.GetInt("Score") >= 3000)
                    {
                        stars[2].GetComponent<Image>().color = Color.white;
                    }
                    break;
                case 3:
                    if (PlayerPrefs.GetInt("Score") >= 1000)
                    {
                        stars[0].GetComponent<Image>().color = Color.white;
                    }
                    if (PlayerPrefs.GetInt("Score") >= 2000)
                    {
                        stars[1].GetComponent<Image>().color = Color.white;
                    }
                    if (PlayerPrefs.GetInt("Score") >= 3000)
                    {
                        stars[2].GetComponent<Image>().color = Color.white;
                    }
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
