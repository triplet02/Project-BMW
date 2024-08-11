using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public static class CharacterInfo
{
    public static int characterNumber = 0;
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject mainCamera;

    // Game Scene Initializing
    Vector3 initialPlayerPosition = new Vector3(-5.0f, 0.1f, 0.0f);
    Vector3 gameStartPlayerPosition = new Vector3(2.0f, 0.01f, 0.0f);
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

    bool isCharacterImmuned = false;

    Vector3 standColliderCenter;
    float standColliderHeight;
    Vector3 slideColliderCenter;
    float slideColliderHeight;


    // Animation Control
    Animator animator = null;

    // UI, Debugging
    Text debuggingUI;
    TextMeshProUGUI score;
    [SerializeField] int scorePerCoin = 100;
    bool criticalVFXPlayed;

    // Alert Particle System
    [SerializeField] GameObject warning;
    [SerializeField] GameObject AlertOnPrefab;
    [SerializeField] GameObject AlertIdlePrefab;
    [SerializeField] GameObject AlertOffPrefab;
    [SerializeField] float AlertOnPlayTime;
    [SerializeField] float AlertOffPlayTime;
    [SerializeField] Transform Canvas;

    GameObject AlertOnInstance;
    GameObject AlertIdleInstance;
    GameObject AlertOffInstance;

    Vector3 AlertPosition;

    // Player Status
    int maxHealth = 3;
    int playerHealth;
    int additionalHealth;
    bool isPlayerImmuned = false;
    new Vector3 centerPosition;
    new Vector3 playerPosition;

    //Skills
    [SerializeField] float MouseBuffTime = 6.0f;
    [SerializeField] GameObject[] MouseCompanions;
    [SerializeField] float DogImmuneTime = 3.0f;
    bool mouseSkillActivate = false;
    int mouseDummy = 0;
    int setMouseDummy = 2;

    // Start is called before the first frame update
    private void Start()
    {
        //player.transform.position = gameStartPlayerPosition;

        rigidbody = player.GetComponent<Rigidbody>();
        capsuleCollider = player.GetComponent<CapsuleCollider>();
        standColliderCenter = capsuleCollider.center;
        standColliderHeight = capsuleCollider.height;
        distance = capsuleCollider.bounds.extents.y + 0.05f;
        animator = player.GetComponentInChildren<Animator>();

        score = GameObject.Find("Text_Score").GetComponent<TextMeshProUGUI>();
        //debuggingUI = GameObject.Find("Game UI").GetComponent<Text>();
        playerHealth = SideViewGameplay1.sideViewGameplay1.playerHealth;
        skillGauge = GameObject.Find("Skill_Gauge").GetComponentInParent<Slider>();
        skillGauge.value = SideViewGameplay1.sideViewGameplay1.skillValue;

        // Alert Particle System
        AlertPosition = AlertOnPrefab.transform.position;

        // Moved from Topview Scene
        if (PlayerPrefs.GetInt("CriticalVFXPlayed") == 1)
        {
            criticalVFXPlayed = true;
        }
        else
        {
            criticalVFXPlayed = false;
        }

        switch (playerHealth)
        {
            case 1:
                warning.SetActive(false);
                AlertIdleInstance = Instantiate(AlertIdlePrefab);
                AlertIdleInstance.transform.SetParent(Canvas);
                AlertIdleInstance.transform.position = AlertPosition;
                AlertIdleInstance.SetActive(true);
                break;
            case 2:
                warning.SetActive(true);
                break;
            case 3:
                warning.SetActive(false);
                break;
        }
    }

    // Update is called once per frame
    private void Update()
    {


        playerPosition = player.transform.position;
        centerPosition = GetComponent<CapsuleCollider>().bounds.center;
        Debug.DrawRay(centerPosition, Vector3.down * distance, Color.red);

        CheckGround();
        UpdateHealth();
        UpdateScore();

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        {
            animator.SetBool("isHit", false);
        }

        /*
        if (isSlide)
        {
            StartCoroutine(Slide());
        }
        */
        

        /*
        debuggingUI.text =
            "Stage : " + StageInfo.stageNumber + "\n" +
            "Coin : " + SideViewGameplay1.sideViewGameplay1.coin.ToString() + "\n" +
            "Health : " + SideViewGameplay1.sideViewGameplay1.playerHealth.ToString() + "\n" +
            "[@@@] y-vel : " + rigidbody.velocity.y.ToString() + "\n" +
            "[@@@] isSlide : " + isSlide.ToString() + "\n" +
            "[@@@] jumpcount : " + jumpCount.ToString() + "\n" +
            "isOnGround : " + isOnGround.ToString() + "\n" +
            "isOnObstacle : " + isOnObstacle.ToString() + "\n" +
            "collider set to trigger : " + capsuleCollider.isTrigger.ToString() + "\n" +
            "Character Number : " + CharacterInfo.characterNumber.ToString();
        */
    }

    public void UpdateHealth()
    {
        switch (playerHealth)
        {
            case 1:
                warning.SetActive(false);
                if (!criticalVFXPlayed)
                {
                    StartCoroutine(AlertOnCoroutine());
                };
                break;
            case 2:
                if (criticalVFXPlayed)
                {
                    PlayerPrefs.SetInt("CriticalVFXPlayed", 0);
                    criticalVFXPlayed = false;
                    StartCoroutine(AlertOffCoroutine());
                }
                warning.SetActive(true);
                break;
            case 3:
                if (warning.activeSelf)
                {
                    warning.SetActive(false);
                }
                break;
            default:
                if (criticalVFXPlayed)
                {
                    PlayerPrefs.SetInt("CriticalVFXPlayed", 0);
                    criticalVFXPlayed = false;
                    StartCoroutine(AlertOffCoroutine());
                }
                if (warning.activeSelf)
                {
                    warning.SetActive(false);
                }
                break;
        }
    }

    public void UpdateScore()
    {
        score.text = (SideViewGameplay1.sideViewGameplay1.coin * scorePerCoin).ToString();
    }

    public void TryJump()
    {
        //if (Input.GetKeyDown(KeyCode.Space)){}

        if (jumpCount < maxJumpCount)
        {
            if (jumpCount == 0)
            {
                //Debug.Log("jump");
                animator.SetBool("isJump", true);
                isOnGround = false;
                animator.SetBool("isOnGround", false);
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
    /*
    public void TrySlide()
    {
        if (!isSlide)
        {
            isSlide = true;
            StartCoroutine(Slide());
        }
    }
    */

    public void TrySkill()
    {
        if (skillGauge.value == 100)
        {
            /*
            //Skill Demo(Cat Skill)
            if(playerHealth < maxHealth)
            {
                playerHealth++;
            }
            */

            switch (CharacterInfo.characterNumber)
            {
                case 1:
                    CaffeineDash();
                    break;
                case 2:
                    StartCoroutine(UnitedWeStand());
                    break;
                case 3:
                    StartCoroutine(NoGutsNoGlory());
                    break;
            }
        }
    }

    public void CaffeineDash()
    {
        if (playerHealth < maxHealth)
        {
            Debug.Log("[Caffeine Dash!]");
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Cat);
            playerHealth++;
            SideViewGameplay1.sideViewGameplay1.playerHealth++;

            skillGauge.value = 0;
        }
        else
        {
            Debug.Log("�̹� �ִ� �Ÿ��Դϴ�.");
        }
    }

    IEnumerator UnitedWeStand()
    {
        Debug.Log("[United We Stand!]");
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Mouse);

        mouseDummy = setMouseDummy;

        mouseSkillActivate = true;
        foreach (GameObject mouseCompanion in MouseCompanions){
            mouseCompanion.SetActive(true);
        }

        StartCoroutine(skillGauge.GetComponentInChildren<SkillGauge>().GaugeReduce(MouseBuffTime));

        yield return new WaitForSeconds(MouseBuffTime);

        mouseDummy = 0;

        foreach (GameObject mouseCompanion in MouseCompanions)
        {
            mouseCompanion.SetActive(false);
        }

    }

    IEnumerator NoGutsNoGlory()
    {
        Debug.Log("[No Guts No Glory!]");
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Dog);
        isPlayerImmuned = true;
        StartCoroutine(skillGauge.GetComponentInChildren<SkillGauge>().GaugeReduce(DogImmuneTime));
        yield return new WaitForSeconds(DogImmuneTime);
        isPlayerImmuned = false;

    }

    private void CheckGround()
    {
        Vector3 centerPosition = GetComponent<CapsuleCollider>().bounds.center;

        if (rigidbody.velocity.y < -0.1f)
        {
            //Debug.Log("[@@@]Ground Checking");
            isOnGround = Physics.Raycast(centerPosition, Vector3.down, distance, groundLayerMask);
            //Debug.Log("[@@@] isOnGround = " + isOnGround.ToString());
            isOnObstacle = Physics.Raycast(centerPosition, Vector3.down, distance, obstacleLayerMask);
            //Debug.Log("[@@@]isOnGround = " + isOnGround.ToString());

            if (isOnGround || isOnObstacle)
            {
                jumpCount = 0;

                isDoubleJump = false;

                animator.SetBool("isOnGround", true);
                animator.SetBool("isFalling", false);
                animator.SetBool("isJump", false);
                animator.SetBool("isDoubleJump", false);

            }
            else
            {
                animator.SetBool("isFalling", true);
            }
        }
    }

    /*IEnumerator Slide()
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
    }*/

    public void StartSlide()
    {
        isSlide = true;
        animator.SetBool("isStartSlide", true);
        animator.SetBool("isSliding", true);
        capsuleCollider.height = 0.8f;
        capsuleCollider.center = new Vector3(0.0f, 0.4f, -0.2f);
        rigidbody.useGravity = false;
    }

    public void EndSlide()
    {
        //Debug.Log("[@@@] isSlide : " + isSlide.ToString());
        //Debug.Log("[@@@] EndSlide");
        animator.SetBool("isStartSlide", false);
        animator.SetBool("isSliding", false);
        rigidbody.useGravity = true;
        capsuleCollider.height = standColliderHeight;
        capsuleCollider.center = standColliderCenter;
        isSlide = false;
        isOnGround = true;
        //Debug.Log("[@@@] isSlide : " + isSlide.ToString());
    }

    IEnumerator AfterCollisionImmune()
    {
        Debug.Log("immune subroutine started");
        isCharacterImmuned = true;
        Debug.Log("isTrigger setted True");

        //animator.SetBool("isHit", false);
        yield return new WaitForSeconds(immuneTime);
        isCharacterImmuned = false;
        Debug.Log("isTrigger setted False");
    }
    IEnumerator AlertOnCoroutine()
    {
        PlayerPrefs.SetInt("CriticalVFXPlayed", 1);
        criticalVFXPlayed = true;
        AlertOnInstance = Instantiate(AlertOnPrefab);
        AlertOnInstance.transform.SetParent(Canvas);
        AlertOnInstance.transform.position = AlertPosition;
        AlertOnInstance.SetActive(true);
        yield return new WaitForSeconds(AlertOnPlayTime);
        Destroy(AlertOnInstance);
        AlertIdleInstance = Instantiate(AlertIdlePrefab);
        AlertIdleInstance.transform.SetParent(Canvas);
        AlertIdleInstance.transform.position = AlertPosition;
        AlertIdleInstance.SetActive(true);
    }
    IEnumerator AlertOffCoroutine()
    {
        Destroy(AlertIdleInstance);
        AlertOffInstance = Instantiate(AlertOffPrefab);
        AlertOffInstance.transform.SetParent(Canvas);
        AlertOffInstance.transform.position = AlertPosition;
        AlertOffInstance.SetActive(true);
        yield return new WaitForSeconds(AlertOffPlayTime);
        Destroy(AlertOffInstance);
    }

    private void OnCollisionExit(Collision collision)
    {
        animator.SetBool("isHit", false);
        capsuleCollider.isTrigger = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Obstacle"))
        {
            Debug.Log("isHit anim go false");
            animator.SetBool("isHit", false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Ground") &&
            collision.collider.gameObject.transform.position.y > centerPosition.y)
        {
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Collision);
            animator.SetBool("isHit", true);
            CameraShaker.Invoke();

            if (playerHealth > 1)
            {
                if (!isPlayerImmuned)
                {
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.Collision);
                    playerHealth--;
                    SideViewGameplay1.sideViewGameplay1.playerHealth--;
                    StartCoroutine(AfterCollisionImmune());
                    animator.SetBool("isHit", false);
                }
                else
                {
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.Immune);
                }
                Debug.Log("isHit anim go false");
                animator.SetBool("isHit", false);
            }
            else
            {
                // stop and show left(or moved) distance to user?
                // ...
                animator.SetBool("isDead", true);
                player.GetComponent<SceneController>().toGameoverScene();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (player.tag.Equals("Dummy"))
        {
            return;
        }

        if (other.tag.Equals("Obstacle"))
        {
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Collision);
            Destroy(other.gameObject, 0.2f);
            animator.SetBool("isHit", true);
            CameraShaker.Invoke();
            if (playerHealth > 1)
            {
                if (!isPlayerImmuned)
                {
                    /*
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.Collision);
                    playerHealth--;
                    SideViewGameplay1.sideViewGameplay1.playerHealth--;
                    */
                    if (mouseSkillActivate)
                    {
                        if (mouseDummy > 0)
                        {
                            mouseDummy -= 1;

                            foreach (GameObject mouseCompanion in MouseCompanions)
                            {
                                if (mouseCompanion.activeSelf)
                                {
                                    mouseCompanion.SetActive(false);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            AudioManager.instance.PlaySfx(AudioManager.Sfx.Collision);
                            playerHealth--;
                            SideViewGameplay1.sideViewGameplay1.playerHealth--;
                        }
                    }
                    else
                    {
                        AudioManager.instance.PlaySfx(AudioManager.Sfx.Collision);
                        playerHealth--;
                        SideViewGameplay1.sideViewGameplay1.playerHealth--;
                    }
                    StartCoroutine(AfterCollisionImmune());
                }
                else
                {
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.Immune);
                }
            }
            else
            {
                // stop and show left(or moved) distance to user?
                // ...
                animator.SetBool("isDead", true);
                player.GetComponent<SceneController>().toGameoverScene();
            }
        }
        if (other.tag.Equals("Beer"))
        {
            if (mouseSkillActivate)
            {
                return;
            }
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Drink);
            SideViewGameplay1.sideViewGameplay1.skillValue += 100;

            if(SideViewGameplay1.sideViewGameplay1.skillValue > 100)
            {
                SideViewGameplay1.sideViewGameplay1.skillValue = 100;
            }

            skillGauge.value = SideViewGameplay1.sideViewGameplay1.skillValue;
        }
        if (other.tag.Equals("Coin"))
        {
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Coin);
            SideViewGameplay1.sideViewGameplay1.coin += 1;
            PlayerPrefs.SetInt("Score", SideViewGameplay1.sideViewGameplay1.coin * scorePerCoin);
        }
        if (other.tag.Equals("Portal") && playerPosition.y < 2.8)
        {
            Debug.Log("From Side To Top View");
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Portal);
            SideViewGameplay1.sideViewGameplay1.currentView = "top";
            //player.GetComponent<SceneController>().toTopViewScene();
            SceneManager.LoadScene("SideView to TopView");
        }
        if (other.tag.Equals("Portal1") && playerPosition.y < 2.8)
        {
            Debug.Log("From Top To Side View");
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Portal);
            SideViewGameplay1.sideViewGameplay1.currentView = "side";
            //player.GetComponent<SceneController>().toSideViewScene();
            SceneManager.LoadScene("TopView to SideView");
            switch (SceneManager.GetActiveScene().name)
            {
                case "TopView Gameplay 1":
                    SideViewGameplay1.sideViewGameplay1.currentMapIdx = 4;
                    break;
                case "TopView Gameplay 2":
                    SideViewGameplay1.sideViewGameplay1.currentMapIdx = 4;
                    break;
                case "TopView Gameplay 3":
                    SideViewGameplay1.sideViewGameplay1.currentMapIdx = 4;
                    break;
            }
            
        }

        if (other.tag.Equals("Goal"))
        {
            Debug.Log("Stage Clear");
            SideViewGameplay1.sideViewGameplay1.currentView = "side";
            player.GetComponent<SceneController>().toGameClearScene();
        }
    }

    private void ModifyPosition(float x, float y, float z)
    {
        Vector3 currentPosition = player.transform.position;
        Vector3 newPosition = currentPosition + new Vector3(x, y, z);

        player.transform.position = newPosition;
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

    public void SetPlayerHealth(int health)
    {
        playerHealth = health;
        return;
    }

    public float GetImmuneTime()
    {
        return immuneTime;
    }
}
