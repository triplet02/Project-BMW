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

    public IEnumerator GaugeReduce(float skillTime)
    {
        //Debug.Log("GuageReduce Coroutine!!");
        while (slider.value > 0)
        {
            //Debug.Log("GaugeReduce Working... / left : " + slider.value.ToString());
            slider.value -= (100 / (skillTime));
            yield return new WaitForSeconds(1f);
        }
        //Debug.Log("GaugeReduce Coroutine End!!");
    }
}