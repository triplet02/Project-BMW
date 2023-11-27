using System.Collections;
using System.Collections.Generic;
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

        if(PlayerPrefs.GetInt("Score") >= 100)
        {
            stars[0].GetComponent<Image>().color = Color.white;
        }
        if (PlayerPrefs.GetInt("Score") >= 200)
        {
            stars[0].GetComponent<Image>().color = Color.white;
        }
        if (PlayerPrefs.GetInt("Score") >= 300)
        {
            stars[0].GetComponent<Image>().color = Color.white;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
