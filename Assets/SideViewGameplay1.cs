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
    public int playerHealth;

    private void Awake()
    {
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
        skillValue = 60;
        playerHealth = 3;
    }
    
    private void Update()
    {

    }
}
