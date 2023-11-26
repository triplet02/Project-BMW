using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SideViewGameplay1 : MonoBehaviour
{
    public static SideViewGameplay1 sideViewGameplay1;
    public GameObject gameUI;
    public Text gameUIText;
    public int skillValue;
    public int maxHealth;
    public int playerHealth;
    public int coin;
    public string currentView;  //side, top
    public int currentMapIdx;  //sideview area index

    private void Awake()
    {
        skillValue = 100;
        maxHealth = 3;
        playerHealth = maxHealth;
        coin = 0;
        currentView = "side";
        currentMapIdx = 0;
        if (sideViewGameplay1 == null)
        {
            sideViewGameplay1 = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {

    }

    private void Update()
    {

    }
}
