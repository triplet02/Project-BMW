using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScriptLibrary : MonoBehaviour
{
    [SerializeField]
    GameObject AlertOnPrefab;
    [SerializeField]
    GameObject AlertIdlePrefab;
    [SerializeField]
    GameObject AlertOffPrefab;
    [SerializeField]
    float AlertOnPlayTime;
    [SerializeField]
    float AlertOffPlayTime;
    [SerializeField]
    Transform Canvas;
    
    GameObject AlertOnInstance;
    GameObject AlertIdleInstance;
    GameObject AlertOffInstance;

    new Vector3 AlertPosition;

    // Start is called before the first frame update
    void Start()
    {
        AlertPosition = AlertOnPrefab.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit01()
    {
        StartCoroutine(AlertOnCoroutine());
    }

    public void Hit02()
    {
        StartCoroutine(AlertOffCoroutine());
    }

    IEnumerator AlertOnCoroutine()
    {
        AlertOnInstance = Instantiate(AlertOnPrefab);
        AlertOnInstance.transform.SetParent(Canvas);
        AlertOnInstance.transform.position = AlertPosition;
        AlertOnInstance.SetActive(true);
        yield return new WaitForSeconds(AlertOnPlayTime);
        Destroy(AlertOnInstance);
        AlertIdleInstance = Instantiate(AlertIdlePrefab);
        AlertIdleInstance.transform.SetParent(Canvas);
        AlertIdleInstance.transform.position = AlertPosition;
        AlertIdleInstance.SetActive(true);
    }
    IEnumerator AlertOffCoroutine()
    {
        Destroy(AlertIdleInstance);
        AlertOffInstance = Instantiate(AlertOffPrefab);
        AlertOffInstance.transform.SetParent(Canvas);
        AlertOffInstance.transform.position = AlertPosition;
        AlertOffInstance.SetActive(true);
        yield return new WaitForSeconds(AlertOffPlayTime);
        Destroy(AlertOffInstance);
    }
}
