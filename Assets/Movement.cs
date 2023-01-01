using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    /*
    [SerializeField]
    private GameController gameController;
    */

    // x 축 이동
    private float moveXWidth = 2.0f;
    private float moveTimeX = 0.1f;
    private bool isXMove = false;
    //  y축 이동
    private float originY = 0.55f;
    private float gravity = -9.81f;
    private float moveTimeY = 0.3f;
    private bool isJump = false;
    // z축 이동
    [SerializeField]
    private float moveSpeed = 2.0f;

    private float rotateSpeed = 300.0f;

    private float limitY = -1.0f;

    private Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //if (gameController.IsGameStart == false) return;

        // z축 이동
        //transform.position += Vector3.forward * moveSpeed * Time.deltaTime;

        // 오브젝트회전 (x축)
        //transform.Rotate(Vector3.right * rotateSpeed * Time.deltaTime);

        // 낭떠러지 사망
        if (transform.position.y < limitY)
        {
            Debug.Log("게임 오버");
        }
    }

    public void MoveToX(int x)
    {
        if (isXMove == true) return;
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

    private IEnumerator OnMoveToX(int direction)
    {
        float current = 0;
        float percent = 0;
        float start = transform.position.x;
        float end = transform.position.x + direction * moveXWidth;

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
}
