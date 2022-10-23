using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SkillGauge : MonoBehaviour
{
    Image skillGaugeFill;
    private Slider slider;
    
    void Start(){
        skillGaugeFill = GameObject.Find("Fill").GetComponent<Image>();
        slider = gameObject.GetComponent<Slider>();
    }
    void Update(){
        if(slider.value == 100){
            skillGaugeFill.color = Color.red;
        }
        else{
            skillGaugeFill.color = Color.green;
        }
    }
}