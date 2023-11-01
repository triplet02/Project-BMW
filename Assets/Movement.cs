using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    /*
    [SerializeField]
    private GameController gameController;
    */

    // x-axis movement
    private float moveXWidth = 2.0f;
    private float moveTimeX = 0.1f;
    private bool isXMove = false;
    // y-axis movement
    private float originY = 0.55f;
    private float gravity = -9.81f;
    private float moveTimeY = 0.3f;
    private bool isJump = false;
    // z-axis movement
    [SerializeField]
    private float moveSpeed = 2.0f;

    private float rotateSpeed = 300.0f;

    private float limitY = -1.0f;

    private Rigidbody rigidbody;

    [Header("Jump Control")]
    [SerializeField] float jumpForce = 5.0f;
    [SerializeField] int maxJumpCount = 0;
    int jumpCount = 0;
    bool isDoubleJump = false;
    public bool isOnGround = true;
    [SerializeField] LayerMask groundLayerMask = 0;
    float distance = 0.0f;

    [Header("Animation Control")]
    Animator animator = null;
    


    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        //if (gameController.IsGameStart == false) return;

        // z-axis movement
        //transform.position += Vector3.forward * moveSpeed * Time.deltaTime;

        // object rotation x-axis
        //transform.Rotate(Vector3.right * rotateSpeed * Time.deltaTime);

        // cliff death
        if (transform.position.y < limitY)
        {
            Debug.Log("GAME OVER");
        }
    }

    public void MoveToX(int x)
    {
        if (isXMove == true) return;
        // if (isOnGround) return;
        // console.log()
        if (x > 0 && transform.position.x < moveXWidth)
        {
            StartCoroutine(OnMoveToX(x));
        }
        else if (x < 0 && transform.position.x > -moveXWidth)
        {
            StartCoroutine(OnMoveToX(x));
        }
    }

    public void MoveToY()
    {
        if (isJump == true) return;

        StartCoroutine(OnMoveToY());
    }

    private float findClosetValue(float val) {
        float[] arr = {-2, 0, 2};
        float dest = val;
        float minDiff = 10000;
        for (int i=0; i<arr.Length; i++)
        {
            float diff = Mathf.Abs(val - arr[i]);
            if(diff < minDiff)
            {
                minDiff = diff;
                dest = arr[i];
            }
        }
        return dest;
    }

    private IEnumerator OnMoveToX(int direction)
    {
        float current = 0;
        float percent = 0;
        float start = transform.position.x;
        float end = transform.position.x + direction * moveXWidth;
        end = findClosetValue(end);

        isXMove = true;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / moveTimeX;

            float x = Mathf.Lerp(start, end, percent);
            transform.position = new Vector3(x, transform.position.y, transform.position.z);

            yield return null;
        }
        isXMove = false;
    }

    private IEnumerator OnMoveToY()
    {
        float current = 0;
        float percent = 0;
        float v0 = -gravity;

        isJump = true;
        rigidbody.useGravity = false;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / moveTimeY;

            float y = originY + (v0 * percent) + (gravity * percent * percent);
            transform.position = new Vector3(transform.position.x, y, transform.position.z);

            yield return null;
        }
        isJump = false;
        rigidbody.useGravity = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("JumpingBoard"))
        {
            Debug.Log("Enter JumpingBoard");
            TryJump();
        }
    }

    public void TryJump()
    {   
        animator.SetBool("isJump", true);
        isOnGround = false;
        GetComponent<Rigidbody>().velocity = Vector3.up * jumpForce;
        animator.SetBool("isJump", false);
        Debug.Log(GetComponent<Rigidbody>().velocity.ToString());
    }

}
