using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTest : MonoBehaviour
{
    [SerializeField] GameObject map;
    [SerializeField] float mapMoveSpeed;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        map.transform.position -= Vector3.forward * mapMoveSpeed;
    }
}
