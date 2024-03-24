using System;
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

    [SerializeField] float platformDetectRange;
    bool isLeftPlatform;
    bool isRightPlatform;

    bool isLeftAheadPlatform;
    bool isRightAheadPlatform;

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

        Debug.DrawRay(new Vector3(transform.position.x - 2.0f, 0f, platformDetectRange), Vector3.up * 2.0f, Color.blue);
        Debug.DrawRay(new Vector3(transform.position.x + 2.0f, 0f, platformDetectRange), Vector3.up * 2.0f, Color.blue);


        if (Physics.Raycast(new Vector3(transform.position.x - 2.0f, -2.0f, 0f), Vector3.up, out hit, 3.0f, LayerMask.GetMask("Platform")))
        {
            isLeftPlatform = true;
            Debug.DrawRay(new Vector3(transform.position.x - 0.7f, 0f, 0f), Vector3.up * 2.0f, Color.red);
            //Debug.Log("Trailer is On Right!");
        }
        else
        {
            isLeftPlatform = false;
        }

        if (Physics.Raycast(new Vector3(transform.position.x - 2.0f, -2.0f, platformDetectRange), Vector3.up, out hit, 3.0f, LayerMask.GetMask("Platform")))
        {
            isLeftAheadPlatform = true;
            Debug.DrawRay(new Vector3(transform.position.x - 0.7f, 0f, 0f), Vector3.up * 2.0f, Color.red);
            //Debug.Log("Trailer is On Right!");
        }
        else
        {
            isLeftAheadPlatform = false;
        }

        if (Physics.Raycast(new Vector3(transform.position.x + 2.0f, -2.0f, 0f), Vector3.up, out hit, 3.0f, LayerMask.GetMask("Platform")))
        {
            isRightPlatform = true;
            Debug.DrawRay(new Vector3(transform.position.x + 0.7f, 0f, 0f), Vector3.up * 2.0f, Color.red);
            //Debug.Log("Trailer is On Right!");
        }
        else
        {
            isRightPlatform = false;
        }

        if (Physics.Raycast(new Vector3(transform.position.x + 2.0f, -2.0f, platformDetectRange), Vector3.up, out hit, 3.0f, LayerMask.GetMask("Platform")))
        {
            isRightAheadPlatform = true;
            Debug.DrawRay(new Vector3(transform.position.x + 0.7f, 0f, 0f), Vector3.up * 2.0f, Color.red);
            //Debug.Log("Trailer is On Right!");
        }
        else
        {
            isRightAheadPlatform = false;
        }

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
            bool isHeadingLeft = touchEnd.x - touchStart.x < 0;
            bool isHeadingRight = touchEnd.x - touchStart.x > 0;

            if(transform.position.y < 3.0f)
            {
                if (isHeadingLeft && (isLeftPlatform || isLeftAheadPlatform))
                {
                    return;
                }
                if (isHeadingRight && (isRightPlatform || isRightAheadPlatform))
                {
                    return;
                }
            }
            
            movement.MoveToX((int)Mathf.Sign(touchEnd.x - touchStart.x));
            return;
        }
    }
}
