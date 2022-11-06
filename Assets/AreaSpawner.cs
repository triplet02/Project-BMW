using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] areaPrefabs;
    private float xDistance = 15;
    [System.NonSerialized]
    private int currentIdx = 3;

    private void Start()
    {
        InitializeArea();
    }

//TODO: Need a branch - vertial / horizontal spawner
    public void InitializeArea()
    {
        GameObject clone = null;
        for (int i = 0; i < 3; i++)
        {
            clone = Instantiate(areaPrefabs[i]);
            clone.transform.position = new Vector3(i * xDistance, 0, 0);
        }
    }

    public void onAreaDestroyed()
    {
        Debug.Log("onAreadestroyed => " + currentIdx.ToString());
        GameObject clone = null;
        if (currentIdx < areaPrefabs.Length)
        {
            clone = Instantiate(areaPrefabs[currentIdx]);
            clone.transform.position = new Vector3(xDistance, 0, 0);
        }
        currentIdx += 1;
    }
}