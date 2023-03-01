using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleController : MonoBehaviour
{
    [SerializeField]
    float approachDistance;
        [SerializeField]
    float triggerDistance;
    [SerializeField]
    float threshold = 50.0f;

    GameObject player;
    float distance;
    bool approached = false;
    bool triggered = false;

    //Temp
    [SerializeField]
    Material[] mat = new Material[2];

    float triggeredPosition = 1.75f;
    Vector3 tirggeredScale = new Vector3(1.0f, 3.0f, 1.0f);

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<MeshRenderer>().material = mat[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            distance = Vector3.Distance(player.transform.position, this.transform.position);
            //Debug.Log(distance.ToString());

            if(distance <= triggerDistance && triggered){
                triggerTentacle();
            }

            if(distance <= approachDistance && !approached)
            {
                approached = true;
                checkTentacle();
            }
        }
        else
        {
            player = GameObject.FindWithTag("Player");
        }
    }

    void checkTentacle()
    {
        float check = Random.Range(0.0f, 100.0f);
        Debug.Log("[Tentacle Check] : " + check.ToString());
        if(check <= threshold){
            triggered = true;
            Debug.Log("tentacle triggered!");
            
        }
        else{
            Debug.Log("tentacle NOT triggered!");
        }
    }

    void triggerTentacle()
    {
        this.GetComponent<MeshRenderer>().material = mat[1];
        this.GetComponent<BoxCollider>().center = new Vector3(0.0f, 0.5f, 0.0f);
        this.GetComponent<BoxCollider>().size = new Vector3(1.0f, 2.0f, 1.0f);

        Vector3 currentPosition = this.transform.position;
        this.transform.position = new Vector3(currentPosition.x, triggeredPosition, currentPosition.z);
        this.transform.localScale = tirggeredScale;
    }
}
