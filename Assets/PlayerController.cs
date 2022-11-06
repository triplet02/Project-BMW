using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject mainCamera;

    // Game Scene Initializing
    Vector3 initialPlayerPosition = new Vector3(-5.0f, 0.01f, 0.0f);
    Vector3 gameStartPlayerPosition = new Vector3(0.0f, 0.01f, 0.0f);
    Vector3 velocity = Vector3.zero;
    [SerializeField] float standByTime = 1.3f;

    // Jump Control
    [SerializeField] float jumpForce = 5.0f;
    [SerializeField] int maxJumpCount = 0;
    int jumpCount = 0;
    bool isDoubleJump = false;

    // Slide Control
    [SerializeField] float slideTime = 1.5f;
    bool isSlide = false;

    // Raycast for Ground Check
    float distance = 0.0f;
    bool isOnGround = true;
    bool isOnObstacle = false;

    // Physics and Collision
    new Rigidbody rigidbody;
    CapsuleCollider capsuleCollider;
    [SerializeField] LayerMask groundLayerMask = 0;
    [SerializeField] LayerMask obstacleLayerMask = 0;
    [SerializeField] Slider skillGauge;

    // Animation Control
    Animator animator = null;

    // UI, Debugging
    Text debuggingUI;

    // Player Status
    int maxHealth = 3;
    int playerHealth;
    int beer = 0;
    int coin = 0;

    // Start is called before the first frame update
    private void Start()
    {
        player.transform.position = initialPlayerPosition;

        rigidbody = player.GetComponent<Rigidbody>();
        capsuleCollider = player.GetComponent<CapsuleCollider>();
        distance = capsuleCollider.bounds.extents.y + 0.05f;
        animator = player.GetComponentInChildren<Animator>();
        
        skillGauge = GameObject.Find("SkillGauge").GetComponent<Slider>();
        debuggingUI = GameObject.Find("Game UI").GetComponent<Text>();

        playerHealth = maxHealth;
    }

    // Update is called once per frame
    private void Update()
    {
        CheckGround();
        if (isSlide)
        {
            StartCoroutine(Slide());
        }

        debuggingUI.text = /*"isJump : " + animator.GetBool("isJump").ToString() + "\n" +
            "isDoubleJump : " + animator.GetBool("isDoubleJump").ToString() + "\n" +
            "isSlide : " + animator.GetBool("isSlide").ToString() + "\n" +
            "isGround : " + isGround.ToString() + "\n" +
            "y_axis_coord : " + player.transform.position.y.ToString() + "\n\n" +*/
            "Beer : " + beer.ToString() + "\n" +
            "Coin : " + coin.ToString() + "\n" +
            "Health : " + playerHealth.ToString() + "\n" +
            "isOnGround : " + isOnGround.ToString() + "\n" +
            "isOnObstacle : " + isOnObstacle.ToString(); 
    }

    public void TryJump()
    {
        //if (Input.GetKeyDown(KeyCode.Space)){}

        if (jumpCount < maxJumpCount)
        {
            if (jumpCount == 0)
            {
                animator.SetBool("isJump", true);
                isOnGround = false;
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
        if (!isSlide)
        {
            isSlide = true;
            StartCoroutine(Slide());
        }
    }

    public void TrySkill()
    {
        if (skillGauge.value == 100)
        {
            Debug.Log("Skill~~~~~~~");
            skillGauge.value = 0;
        }
    }
     
    private void CheckGround()
    {
        Vector3 centerPosition = GetComponent<CapsuleCollider>().bounds.center;

        if (rigidbody.velocity.y < -0.0f)
        {
            isOnGround = Physics.Raycast(centerPosition, Vector3.down, distance, groundLayerMask);
            isOnObstacle = Physics.Raycast(centerPosition, Vector3.down, distance, obstacleLayerMask);

            if (isOnGround || isOnObstacle)
            {
                jumpCount = 0;
                
                isDoubleJump = false;

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
        if (collision.collider.gameObject.CompareTag("Obstacle"))
        {
            BoxCollider boxCollider = collision.collider.gameObject.GetComponent<BoxCollider>();

            float playerYPosition = player.transform.position.y;
            float obstacleTopYPosition = boxCollider.bounds.center.y + boxCollider.bounds.extents.y - 0.03f;

            Debug.Log(playerYPosition.ToString() + " / " + obstacleTopYPosition.ToString());

            if (playerYPosition < obstacleTopYPosition)
            {
                Debug.Log("collision");
                //GetComponent<SceneController>().toGameoverScene();
                playerHealth--;
                rigidbody.velocity = new Vector3(-1.2f, 0.5f, 0) * jumpForce;
            }
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
        return (isOnGround || isOnObstacle);
    }

    public bool GetIsDoubleJump()
    {
        return isDoubleJump;
    }

    public float Get_y_Velocity()
    {
        return rigidbody.velocity.y;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public int GetPlayerHealth()
    {
        return playerHealth;
    }
}
