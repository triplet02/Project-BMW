using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [SerializeField]
    GameObject mainCamera;
    [SerializeField]
    Vector3 backgroundDistance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mainCameraPosition = mainCamera.GetComponent<MainCameraController>().GetCameraPosition();
        this.transform.position = mainCameraPosition + backgroundDistance;
    }
}
