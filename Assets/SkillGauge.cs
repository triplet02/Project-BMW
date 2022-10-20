using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Required when Using UI elements.

public class SkillGauge : MonoBehaviour
{
    public Slider skillGauge;

    void Start(){
        skillGauge = GetComponent<Slider>();
    }
    void Update(){

    }
    // public void setValue(int value){
    //     skillGauge.value = value;
    // }

}