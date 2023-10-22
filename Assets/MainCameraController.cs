using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float jumpCameraHeight;
    Vector3 cameraDistance = new Vector3(3.1f, 1.7f, -5.0f);
    Vector3 gameStartPlayerPosition = new Vector3(0.0f, 0.0f, 0.0f);

    int jumpCount = 0;
    bool isGround = true;
    float yVelocity = 0.0f;
    Vector3 jumpCameraPosition;

    Vector3 vel;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = gameStartPlayerPosition + cameraDistance;
    }

    // Update is called once per frame
    void Update()
    {
        jumpCount = player.GetComponent<PlayerController>().GetJumpCount();
        isGround = player.GetComponent<PlayerController>().GetIsGround();
        yVelocity = player.GetComponent<PlayerController>().Get_y_Velocity();

        float playerXPosition = player.transform.position.x;
        float playerYPosition = player.transform.position.y;
        float playerZPosition = player.transform.position.z;

        jumpCameraPosition = new Vector3(playerXPosition, playerYPosition < jumpCameraHeight ? playerYPosition : jumpCameraHeight, playerZPosition);

        if (jumpCount > 0)
        {
            transform.position = Vector3.SmoothDamp(transform.position, jumpCameraPosition + cameraDistance, ref vel, 0.1f);
            
            //transform.position = Vector3.SmoothDamp(transform.position, jumpCameraPosition + cameraDistance, ref vel, 0.1f);
        }

        if(yVelocity < -0.2f)//&& !isGround
        {
            transform.position = jumpCameraPosition + cameraDistance;
        }

        if (isGround && playerYPosition < 2.0f)
        {
            transform.position = player.transform.position + cameraDistance;
        }
    }
    public Vector3 GetCameraPosition()
    {
        return this.transform.position;
    }
}
