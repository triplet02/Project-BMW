using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopViewGameplay : MonoBehaviour
{
    public void Start()
    {
        Debug.Log(SideViewGameplay1.sideViewGameplay1.skillValue.ToString());
        Debug.Log(SideViewGameplay1.sideViewGameplay1.playerHealth.ToString());
    }
    
}
