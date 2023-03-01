using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StageInfo
{
    public static int stageNumber = 0;
}

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
        Debug.Log(areaPrefabs.Length.ToString());
        Debug.Log(areaPrefabs[areaPrefabs.Length-1].name);
        foreach (var areaSet in areaPrefabs)
        {
            Debug.Log(areaSet.name);
        }
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
        // Debug.Log("onAreadestroyed => " + currentIdx.ToString());
        GameObject clone = null;
        if (currentIdx < areaPrefabs.Length)
        {
            Debug.Log(areaPrefabs[currentIdx].name);
            clone = Instantiate(areaPrefabs[currentIdx]);
            clone.transform.position = new Vector3(xDistance, 0, 0);
            currentIdx += 1;
        }
        Debug.Log("[map index] " + currentIdx.ToString());
    }
}