using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    [SerializeField]
    private float destroyDistance = 45;
    [SerializeField]
    private float speed = 5;
    
    private void Update()
    {
        transform.position += Vector3.left * Time.deltaTime * speed;
        if(gameObject.transform.position.x < -30) {
            Destroy(gameObject);
        }
    }
}
