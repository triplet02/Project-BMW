using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField]
    private GameObject coinEffectPrefab;
    /*
    private float rotateSpeed;

    private void Awake()
    {
        rotateSpeed = Random.Range(0, 360);
    }

    private void Update()
    {
        transform.Rotate(Vector3.right * rotateSpeed * Time.deltaTime);
    }
    */

    private void OnTriggerEnter(Collider other)
    {
        GameObject clone = Instantiate(coinEffectPrefab);
        clone.transform.position = transform.position;

        Destroy(gameObject);
    }
}
