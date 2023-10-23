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
    public GameObject[] areaPrefabs;
    private float xDistance = 30;
    [System.NonSerialized]
    private static int mapIdx = 0;
    //private static int currentIdx = 0;
    
    private void Start()
    {
        Debug.Log("[ Start() ] mapIdx ===> " + mapIdx.ToString());
        InitializeArea();
        Debug.Log(SideViewGameplay1.sideViewGameplay1.currentView);
        Debug.Log(areaPrefabs.Length.ToString());
        //Debug.Log(areaPrefabs[areaPrefabs.Length-1].name);
        foreach (var areaSet in areaPrefabs)
        {
            Debug.Log(areaSet.name);
        }
    }

    public void InitializeArea()
    {
        GameObject clone = null;
        mapIdx = 0;
        if (SideViewGameplay1.sideViewGameplay1.currentView == "side") {
            mapIdx = SideViewGameplay1.sideViewGameplay1.currentMapIdx;
        }
        for (int i = 0; i < 3; i++)
        {
            Debug.Log("[ Init() ] mapIdx ===> " + mapIdx.ToString());
            clone = Instantiate(areaPrefabs[mapIdx]);
            if (SideViewGameplay1.sideViewGameplay1.currentView == "side"){
                Debug.Log(areaPrefabs[mapIdx].ToString());
                clone.transform.position = new Vector3(i * xDistance, 0, 0);
            }
            else {
                clone.transform.position = new Vector3(0, 0, i * xDistance);
            }
            mapIdx += 1;
            //currentIdx += 1;
            if (SideViewGameplay1.sideViewGameplay1.currentView == "side")
            {
                SideViewGameplay1.sideViewGameplay1.currentMapIdx += 1;
            }
            //SideViewGameplay1.sideViewGameplay1.currentMapIdx += 1;
        }
    }

    public void onAreaDestroyed()
    {
        Debug.Log("[ onDest() ] mapIdx ===> " + mapIdx.ToString());
        foreach (var areaSet in areaPrefabs)
        {
            Debug.Log("// " + areaSet.name);
        }
        //Debug.Log("onAreadestroyed => " + currentIdx.ToString());
        GameObject clone = null;
        if (mapIdx < areaPrefabs.Length)
        {
            //Debug.Log("new area::" + areaPrefabs[currentIdx].name);
            //Debug.Log(areaPrefabs[currentIdx].ToString());
            //clone = Instantiate(areaPrefabs[currentIdx]);
            clone = Instantiate(areaPrefabs[mapIdx]);
            if (SideViewGameplay1.sideViewGameplay1.currentView == "side"){
                clone.transform.position = new Vector3(xDistance, 0, 0);
                //SideViewGameplay1.sideViewGameplay1.currentMapIdx = currentIdx;
                SideViewGameplay1.sideViewGameplay1.currentMapIdx = mapIdx;
            }
            else {
                clone.transform.position = new Vector3(0, 0, xDistance);
            }
            //currentIdx += 1;
            mapIdx += 1;
        }
        // Debug.Log("[map index] " + currentIdx.ToString());
    }

    public GameObject[] GetAreaPrefabs()
    {
        return areaPrefabs;
    }
}