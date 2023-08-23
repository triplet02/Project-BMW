using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopviewPlayerController : MonoBehaviour
{
    [SerializeField]
    private float dragDistance = 50.0f;
    private Vector3 touchStart;
    private Vector3 touchEnd;
    bool isLeftObstacle = false;
    bool isRightObstacle = false;
    private RaycastHit hit;
    private int layerMask;

    [SerializeField] GameObject player;
    [SerializeField] CapsuleCollider capsuleCollider;

    private Movement movement;

    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.isMobilePlatform)
        {
            OnMobilePlatform();
        }
        else
        {
            OnPCPlatform();
        }
        // detect obstacle
        if (Physics.Raycast(transform.position, Vector3.left, out hit, 2.0f, LayerMask.GetMask("Obstacle")))
        {
            isLeftObstacle = true;
            Debug.Log("Hit obstacle " + hit.collider.gameObject.name);
            Debug.DrawRay(transform.position, Vector3.left* hit.distance, Color.red);
        }
        else isLeftObstacle = false;
        if (Physics.Raycast(transform.position, Vector3.right, out hit, 2.0f, LayerMask.GetMask("Obstacle")))
        {
            isRightObstacle = true;
            Debug.Log("Hit obstacle " + hit.collider.gameObject.name);
            Debug.DrawRay(transform.position, Vector3.right * hit.distance, Color.red);
        }
        else isRightObstacle = false;
    }

    private void OnMobilePlatform()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            touchStart = touch.position;
        }
        else if (touch.phase == TouchPhase.Moved)
        {
            touchEnd = touch.position;
            OnDragXY();
        }
    }

    private void OnPCPlatform()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            touchEnd = Input.mousePosition;
            OnDragXY();
        }
    }

    private void OnDragXY()
    {
        if (Mathf.Abs(touchEnd.x - touchStart.x) >= dragDistance)
        {
            bool isHeadingLeft = touchEnd.x - touchStart.x > 0 ? true : false;
            if (movement.isOnGround) {
                if (isHeadingLeft && isLeftObstacle) return;
                if (!isHeadingLeft && isRightObstacle) return;
            }
            movement.MoveToX((int)Mathf.Sign(touchEnd.x - touchStart.x));
            return;
        }
    }
}
