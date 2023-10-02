using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Area : MonoBehaviour
{
    [SerializeField]
    private float destroyDistance = 60;
    [SerializeField]
    private float speed = 5;
    public UnityEvent onAreaDestroyed;
    
    private void Update()
    {
        if (SideViewGameplay1.sideViewGameplay1.currentView == "side"){
            transform.position += Vector3.left * Time.deltaTime * speed;    //speed 
            if(gameObject.transform.position.x < -destroyDistance) {
                onAreaDestroyed.Invoke();
                Destroy(gameObject);
            }
        }
        else {
            transform.position += Vector3.back * Time.deltaTime * speed; //speed
            if(gameObject.transform.position.z < -destroyDistance) {
                Destroy(gameObject);
                onAreaDestroyed.Invoke();
            }
        }
    }
}
