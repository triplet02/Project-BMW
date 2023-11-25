using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnlockManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("Stage") == 1)
        {
            PlayerPrefs.SetInt("UnlockAfternoon", 1);
            PlayerPrefs.SetInt("UnlockMouse", 1);
        }
        else if (PlayerPrefs.GetInt("Stage") == 2)
        {
            PlayerPrefs.SetInt("UnlockNight", 1);
            PlayerPrefs.SetInt("UnlockDog", 1);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
