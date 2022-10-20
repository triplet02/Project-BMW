using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject map;

    // ���� ���� ��ġ ����
    Vector3 initialPlayerPosition = new Vector3(-5.0f, 0.01f, 0.0f);
    Vector3 gameStartPlayerPosition = new Vector3(0.0f, 0.01f, 0.0f);
    Vector3 velocity = Vector3.zero;
    [SerializeField] float standByTime = 1.3f;

    // ���� ��Ʈ��
    [SerializeField] float jumpForce = 5.0f;
    [SerializeField] int maxJumpCount = 0;
    int jumpCount = 0;
    bool isDoubleJump = false;

    //�����̵� ��Ʈ��
    [SerializeField] float slideTime = 1.5f;
    bool isSlide = false;

    // ���� ���� Raycast
    float distance = 0.0f;
    bool isGround = true;

    // Physics �� �浹 ����
    new Rigidbody rigidbody;
    CapsuleCollider capsuleCollider;
    [SerializeField] LayerMask layerMask = 0;
    [SerializeField] public Slider skillGauge;

    // Map ��Ʈ��
    Rigidbody mapRigidbody;

    // �ִϸ��̼�
    Animator animator = null;

    // UI
    Text velocityMonitor;
    Text debug;

    int beer = 0;
    int coin = 0;

    // Start is called before the first frame update
    private void Start()
    {
        player.transform.position = initialPlayerPosition;

        rigidbody = player.GetComponent<Rigidbody>();
        mapRigidbody = map.GetComponent<Rigidbody>();
        capsuleCollider = player.GetComponent<CapsuleCollider>();
        distance = capsuleCollider.bounds.extents.y + 0.05f;
        animator = player.GetComponentInChildren<Animator>();
        
        skillGauge = GameObject.Find("SkillGauge").GetComponent<Slider>();
        velocityMonitor = GameObject.Find("Velocity Monitor").GetComponent<Text>();
        debug = GameObject.Find("Game UI").GetComponent<Text>();

        mapRigidbody.velocity = new Vector3(-3.0f, 0, 0);
    }

    // Update is called once per frame
    private void Update()
    {
        // ���� ���� ��ġ�� ĳ���� ��ġ �̵�
        //player.transform.position = Vector3.SmoothDamp(gameObject.transform.position, gameStartPlayerPosition, ref velocity, standByTime);

        //�� �̵�
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
            "y_axis_coord : " + player.transform.position.y.ToString() + "\n\n" +
            "Beer : " + beer.ToString() + "\n" +
            "Coin : " + coin.ToString(); 
    }

    public void TryJump()
    {
        //if (Input.GetKeyDown(KeyCode.Space)){}

        if (jumpCount < maxJumpCount)
        {
            if (jumpCount == 0)
            {
                animator.SetBool("isJump", true);
                isGround = false;
                rigidbody.velocity = Vector3.up * jumpForce;
            }
            else
            {
                animator.SetBool("isDoubleJump", true);
                isDoubleJump = true;
                rigidbody.velocity = Vector3.up * jumpForce;
            }
            jumpCount++;
        }
    }

    public void TrySlide()
    {
        Debug.Log("�����̵� Ŭ��");
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
            //�������� �� ���� ī�޶� �����
            isGround = Physics.Raycast(centerPosition, Vector3.down, distance, layerMask);

            if (isGround)
            {

                // ���� Ƚ�� �ʱ�ȭ
                jumpCount = 0;
                
                //���� ī�޶� ���� ����
                isDoubleJump = false;

                //�ִϸ��̼� ���� ����
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Beer"))
        {
            beer++;
            skillGauge.value += beer;
        }

        if (other.tag.Equals("Coin"))
        {
            coin++;
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
