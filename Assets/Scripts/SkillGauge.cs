using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SkillGauge : MonoBehaviour
{
    Image skillGaugeFill;
    private Slider slider;

    void Start()
    {
        skillGaugeFill = GameObject.Find("Skill_Gauge").GetComponent<Image>();
        slider = GameObject.Find("Skill_Gauge").GetComponentInParent<Slider>();
        Debug.Log("[slider] : " + slider.ToString());
        slider.value = SideViewGameplay1.sideViewGameplay1.skillValue;
    }
    void Update()
    {
        if (slider.value == 100)
        {
            skillGaugeFill.color = Color.green;
        }
        else
        {
            skillGaugeFill.color = Color.red;
        }
    }
}