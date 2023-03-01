using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public static class CharacterInfo
{
    public static int characterNumber = 0;
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject mainCamera;

    // Game Scene Initializing
    Vector3 initialPlayerPosition = new Vector3(-5.0f, 0.01f, 0.0f);
    Vector3 gameStartPlayerPosition = new Vector3(0.0f, 0.01f, 0.0f);
    Vector3 velocity = Vector3.zero;
    [SerializeField] float standByTime = 1.3f;

    [Header("Jump Control")]
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
    [SerializeField] float immuneTime = 1.0f;

    // Animation Control
    Animator animator = null;

    // UI, Debugging
    Text debuggingUI;

    // Player Status
    int maxHealth = 3;
    int playerHealth;
    int beer = 0;
    int coin = 0;

    // Chaser(Health(Distance) Decrease) System
    [SerializeField] int healthDecrease;

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
            "Stage : " + StageInfo.stageNumber + "\n" +
            "Beer : " + beer.ToString() + "\n" +
            "Coin : " + coin.ToString() + "\n" +
            "Health : " + playerHealth.ToString() + "\n" +
            "isOnGround : " + isOnGround.ToString() + "\n" +
            "isOnObstacle : " + isOnObstacle.ToString() +
            "collider set to trigger : " + capsuleCollider.isTrigger.ToString();
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

            //Skill Demo(Cat Skill)
            if(playerHealth < maxHealth)
            {
                playerHealth++;
            }

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
        rigidbody.useGravity = false;
        yield return new WaitForSeconds(slideTime);
        animator.SetBool("isSlide", false);
        rigidbody.useGravity = true;
        capsuleCollider.height = 1.5f;
        capsuleCollider.center = new Vector3(0.0f, 0.75f, 0.0f);
        isSlide = false;
    }

    IEnumerator AfterCollisionImmune()
    {
        Debug.Log("immune subroutine started");
        capsuleCollider.isTrigger = true;
        Debug.Log("isTrigger setted True");
        yield return new WaitForSeconds(immuneTime);
        capsuleCollider.isTrigger = false;
        Debug.Log("isTrigger setted False");
    }

    private void OnCollisionEnter(Collision collision)
    {        
        if (collision.collider.gameObject.CompareTag("Obstacle"))
        {
            BoxCollider boxCollider = collision.collider.gameObject.GetComponent<BoxCollider>();
            boxCollider.isTrigger = true;
            Destroy(collision.gameObject, 0.2f);

            float playerYPosition = player.transform.position.y;
            float obstacleTopYPosition = boxCollider.bounds.center.y + boxCollider.bounds.extents.y - 0.03f;

            Debug.Log(playerYPosition.ToString() + " / " + obstacleTopYPosition.ToString());

            if (playerYPosition < obstacleTopYPosition)
            {
                Debug.Log("collision");
                //GetComponent<SceneController>().toGameoverScene();
                if(playerHealth > 1)
                {
                    playerHealth--;
                    //rigidbody.velocity = new Vector3(-1.2f, 0.5f, 0) * jumpForce;
                    StartCoroutine(AfterCollisionImmune());
                }
                else
                {
                    // stop and show left(or moved) distance to user?
                    // ...

                    player.GetComponent<SceneController>().toGameoverScene();
                }
                
            }
        }

        if (collision.collider.gameObject.CompareTag("Deadzone"))
        {
            Debug.Log("DeadZone Detected!");
            playerHealth = 0;
            player.GetComponent<SceneController>().toGameoverScene();
        }

        if(collision.collider.gameObject.CompareTag("Ground") && !isOnGround)
        {
            Debug.Log("Mid-air Collision!");

            if (playerHealth > 1)
            {
                playerHealth--;
                //rigidbody.velocity = new Vector3(-1.2f, 0.5f, 0) * jumpForce;
                StartCoroutine(AfterCollisionImmune());
            }
            else
            {
                // stop and show left(or moved) distance to user?
                // ...

                player.GetComponent<SceneController>().toGameoverScene();
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        capsuleCollider.isTrigger = false;
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
