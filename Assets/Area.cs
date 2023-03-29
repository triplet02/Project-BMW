using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Area : MonoBehaviour
{
    [SerializeField]
    private float destroyDistance = 45;
    [SerializeField]
    private float speed = 5;
    public UnityEvent onAreaDestroyed;
    
    private void Update()
    {
        if (SideViewGameplay1.sideViewGameplay1.currentView == "side"){
            transform.position += Vector3.left * Time.deltaTime * 5;    //speed = 5
            if(gameObject.transform.position.x < -30) {
                Destroy(gameObject);
                onAreaDestroyed.Invoke();
            }
        }
        else {
            transform.position += Vector3.back * Time.deltaTime * 5; //speed = 2
            if(gameObject.transform.position.z < -30) {
                Destroy(gameObject);
                onAreaDestroyed.Invoke();
            }
        }
    }
}
