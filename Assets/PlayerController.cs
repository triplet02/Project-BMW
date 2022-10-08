using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject map;

    // 게임 시작 위치 지정
    Vector3 initialPlayerPosition = new Vector3(-5.0f, 0.01f, 0.0f);
    Vector3 gameStartPlayerPosition = new Vector3(0.0f, 0.01f, 0.0f);
    Vector3 velocity = Vector3.zero;
    [SerializeField] float standByTime = 1.3f;

    // 점프 컨트롤
    [SerializeField] float jumpForce = 5.0f;
    [SerializeField] int maxJumpCount = 0;
    int jumpCount = 0;
    bool isDoubleJump = false;

    //슬라이드 컨트롤
    [SerializeField] float slideTime = 1.5f;
    bool isSlide = false;

    // 착지 감지 Raycast
    float distance = 0.0f;
    bool isGround = true;

    // Physics 및 충돌 감지
    new Rigidbody rigidbody;
    CapsuleCollider capsuleCollider;
    [SerializeField] LayerMask layerMask = 0;

    // Map 컨트롤
    Rigidbody mapRigidbody;

    // 애니메이션
    Animator animator = null;

    // UI
    Text velocityMonitor;
    Text debug;

    // Start is called before the first frame update
    private void Start()
    {
        player.transform.position = initialPlayerPosition;

        rigidbody = player.GetComponent<Rigidbody>();
        mapRigidbody = map.GetComponent<Rigidbody>();
        capsuleCollider = player.GetComponent<CapsuleCollider>();
        distance = capsuleCollider.bounds.extents.y + 0.05f;
        animator = player.GetComponentInChildren<Animator>();

        velocityMonitor = GameObject.Find("Velocity Monitor").GetComponent<Text>();
        debug = GameObject.Find("Game UI").GetComponent<Text>();

        mapRigidbody.velocity = new Vector3(-3.0f, 0, 0);
    }

    // Update is called once per frame
    private void Update()
    {
        // 게임 시작 위치로 캐릭터 위치 이동
        player.transform.position = Vector3.SmoothDamp(gameObject.transform.position, gameStartPlayerPosition, ref velocity, standByTime);

        //맵 이동
        //map.transform.Translate(-0.006f, 0, 0);
        

        CheckGround();
        if (isSlide)
        {
            StartCoroutine(Slide());
        }

        velocityMonitor.text = rigidbody.velocity.ToString();
        debug.text = "isJump : " + animator.GetBool("isJump").ToString() + "\n" +
            "isDoubleJump : " + animator.GetBool("isDoubleJump").ToString() + "\n" +
            "isSlide : " + animator.GetBool("isSlide").ToString() + "\n" +
            "isGround : " + isGround.ToString() + "\n" +
            "y_axis_coord : " + player.transform.position.y.ToString();
    }

    public void TryJump()
    {
        //if (Input.GetKeyDown(KeyCode.Space)){}

        //현재 있는 y축 좌표에 따라 점프 높이가 달라짐(??? 원인 모르겠음)
        //y축 좌표로 점프할 때 부여하는 velocity에 보정해 주는 방식으로 일관성 있게 구현
        float y_axis_coord = player.transform.position.y;
        float adjustedJumpForce = jumpForce + y_axis_coord;

        if(adjustedJumpForce < 0)
        {
            adjustedJumpForce = 1.0f;
        }

        if (jumpCount < maxJumpCount)
        {
            if (jumpCount == 0)
            {
                animator.SetBool("isJump", true);
                isGround = false;
                rigidbody.velocity = Vector3.up * adjustedJumpForce;
            }
            else
            {
                animator.SetBool("isDoubleJump", true);
                isDoubleJump = true;
                rigidbody.velocity = Vector3.up * adjustedJumpForce;
            }
            jumpCount++;
        }
    }

    public void TrySlide()
    {
        Debug.Log("슬라이드 클릭");
        if (!isSlide)
        {
            isSlide = true;
            StartCoroutine(Slide());
        }
    }
     
    private void CheckGround()
    {
        Vector3 centerPosition = GetComponent<CapsuleCollider>().bounds.center;

        if (rigidbody.velocity.y < -0.0f)
        {
            //착지감지 및 메인 카메라 제어용
            isGround = Physics.Raycast(centerPosition, Vector3.down, distance, layerMask);

            if (isGround)
            {

                // 점프 횟수 초기화
                jumpCount = 0;
                
                //메인 카메라 제어 변수
                isDoubleJump = false;

                //애니메이션 제어 변수
                animator.SetBool("isJump", false);
                animator.SetBool("isDoubleJump", false);

            }
        }
    }

    IEnumerator Slide()
    {
        animator.SetBool("isSlide", true);
        capsuleCollider.height = 0.8f;
        capsuleCollider.center = new Vector3(0.0f, 0.4f, -0.2f);
        yield return new WaitForSeconds(slideTime);
        animator.SetBool("isSlide", false);
        capsuleCollider.height = 1.5f;
        capsuleCollider.center = new Vector3(0.0f, 0.75f, 0.0f);
        isSlide = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("StartBlock"))
        {
            return;
        }

        if(collision.collider.gameObject.CompareTag("Ground"))
        {
            if(player.transform.position.y <= collision.collider.gameObject.transform.position.y)
            {
                GetComponent<SceneController>().toGameoverScene();
            }
        }

        if (collision.collider.gameObject.CompareTag("SlideObstacle"))
        {
            GetComponent<SceneController>().toGameoverScene();
        }
    }

    public int GetJumpCount()
    {
        return jumpCount;
    }

    public bool GetIsGround()
    {
        return isGround;
    }

    public bool GetIsDoubleJump()
    {
        return isDoubleJump;
    }

    public float Get_y_Velocity()
    {
        return rigidbody.velocity.y;
    }
}
