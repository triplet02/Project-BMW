using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] areaPrefabs;
    private float xDistance = 15;


    private void Awake()
    {
        SpawnArea();
    }

    public void SpawnArea()
    {
        GameObject clone = null;
        for (int i = 0; i < 5; i++)
        {
            clone = Instantiate(areaPrefabs[i]);
            clone.transform.position = new Vector3( i * xDistance, 0, 0);
        }
    }
}

