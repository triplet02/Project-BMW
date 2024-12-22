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
    bool isRunning = true;

    // Physics and Collision
    new Rigidbody rigidbody;
    CapsuleCollider capsuleCollider;
    [SerializeField] LayerMask groundLayerMask = 0;
    [SerializeField] LayerMask obstacleLayerMask = 0;
    [SerializeField] Slider skillGauge;
    [SerializeField] float immuneTime = 1.0f;
    [SerializeField] float fallingGravity = 0.0f;
    Vector3 fallingForceBias;
    Vector3 resetGravity = new Vector3(0.0f, -9.81f, 0.0f);

    Vector3 standColliderCenter;
    float standColliderHeight;
    Vector3 slideColliderCenter;
    float slideColliderHeight;


    // Animation Control
    Animator animator = null;
    bool pastIsRunning;

    // UI, Debugging
    Text debuggingUI;
    TextMeshProUGUI score;
    [SerializeField] int scorePerCoin = 100;
    bool criticalVFXPlayed;
    GameObject characterSelectManager;
    GameObject jumpButton, SlideButton;

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
    [SerializeField] Vector3[] mouseCompanionPositionBias;

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

        fallingForceBias = new Vector3(0.0f, -fallingGravity, 0.0f);

        score = GameObject.Find("Text_Score").GetComponent<TextMeshProUGUI>();
        //debuggingUI = GameObject.Find("Game UI").GetComponent<Text>();
        playerHealth = SideViewGameplay1.sideViewGameplay1.playerHealth;
        skillGauge = GameObject.Find("Skill_Gauge").GetComponentInParent<Slider>();
        skillGauge.value = SideViewGameplay1.sideViewGameplay1.skillValue;

        // Alert Particle System
        AlertPosition = AlertOnPrefab.transform.position;

        //UI
        characterSelectManager = GameObject.Find("CharacterSelectManager");
        jumpButton = characterSelectManager.GetComponent<CharacterSelectManager>().GetCurrentActiveJumpButton();
        SlideButton = characterSelectManager.GetComponent<CharacterSelectManager>().GetCurrentActiveSlideButton();

        Debug.Log("[JumpButton] : " + jumpButton.ToString());
        Debug.Log("[SlideButton] : " + SlideButton.ToString());


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
        if(jumpButton == null && SceneManager.GetActiveScene().name.Contains("SideView"))
        {
            jumpButton = characterSelectManager.GetComponent<CharacterSelectManager>().GetCurrentActiveJumpButton();
        }
        if(SlideButton == null && SceneManager.GetActiveScene().name.Contains("SideView"))
        {
            SlideButton = characterSelectManager.GetComponent<CharacterSelectManager>().GetCurrentActiveSlideButton();
        }
        playerPosition = player.transform.position;
        centerPosition = GetComponent<CapsuleCollider>().bounds.center;
        Debug.DrawRay(centerPosition, Vector3.down * distance, Color.red);

        pastIsRunning = isRunning;
        CheckGround();
        if(pastIsRunning != isRunning && isRunning)
        {
            animator.SetBool("isJustLanded", true);
            MouseCompanionAnimatorSetter("isJustLanded", true);
        }
        else
        {
            animator.SetBool("isJustLanded", false);
            MouseCompanionAnimatorSetter("isJustLanded", false);
        }

        UpdateHealth();
        UpdateScore();

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        {
            animator.SetBool("isHit", false);
            MouseCompanionAnimatorSetter("isHit", false);
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
                MouseCompanionAnimatorSetter("isJump", true);
                isRunning = false;
                animator.SetBool("isOnGround", false);
                MouseCompanionAnimatorSetter("isOnGround", false);
                rigidbody.velocity = Vector3.up * jumpForce;
            }
            else
            {
                Physics.gravity = resetGravity;
                animator.SetBool("isDoubleJump", true);
                MouseCompanionAnimatorSetter("isDoubleJump", true);
                isDoubleJump = true;
                rigidbody.velocity = Vector3.up * jumpForce;
            }
            jumpCount++;
            if(jumpCount >= maxJumpCount)
            {
                jumpButton.GetComponent<Button>().interactable = false;
            }
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
            Debug.Log("Health is Already Full.");
        }
    }

    IEnumerator UnitedWeStand()
    {
        Debug.Log("[United We Stand!]");
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Mouse);

        mouseDummy = setMouseDummy;

        mouseSkillActivate = true;
        foreach (GameObject mouseCompanion in MouseCompanions){
            Vector3 positionBias = mouseCompanion.GetComponent<MouseCompanionController>().GetPositionBias();
            Debug.Log("[dummy position log] player : " + player.transform.position.ToString() + " / bias : " + positionBias.ToString() + " / final : " + (player.transform.position - positionBias).ToString());
            mouseCompanion.transform.position = player.transform.position - positionBias;
            mouseCompanion.SetActive(true);
        }

        StartCoroutine(skillGauge.GetComponentInChildren<SkillGauge>().GaugeReduce(MouseBuffTime));

        yield return new WaitForSeconds(MouseBuffTime);

        mouseSkillActivate = false;
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
        if (SceneManager.GetActiveScene().name.Contains("TopView"))
        {
            return;
        }
        Vector3 centerPosition = GetComponent<CapsuleCollider>().bounds.center;
        if(player.transform.position.y >= 0.1)
        {
            SlideButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            SlideButton.GetComponent<Button>().interactable = true;
        }

        isOnGround = Physics.Raycast(centerPosition, Vector3.down, distance, groundLayerMask);
        isOnObstacle = Physics.Raycast(centerPosition, Vector3.down, distance, obstacleLayerMask);

        if(isOnGround || isOnObstacle)
        {
            animator.SetBool("isOnGround", true);
            MouseCompanionAnimatorSetter("isOnGround", true);
            isRunning = true;
        }
        else
        {
            animator.SetBool("isOnGround", false);
            MouseCompanionAnimatorSetter("isOnGround", false);
            isRunning = false;
        }

        if (rigidbody.velocity.y < -0.1f)
        {
            if (isOnGround || isOnObstacle)
            {
                Physics.gravity = resetGravity;

                jumpCount = 0;
                isDoubleJump = false;
                jumpButton.GetComponent<Button>().interactable = true;

                animator.SetBool("isOnGround", true);
                animator.SetBool("isFalling", false);
                animator.SetBool("isJump", false);
                animator.SetBool("isDoubleJump", false);
                MouseCompanionAnimatorSetter("isOnGround", true);
                MouseCompanionAnimatorSetter("isFalling", false);
                MouseCompanionAnimatorSetter("isJump", false);
                MouseCompanionAnimatorSetter("isDoubleJump", false);

            }
            else
            {
                Physics.gravity = fallingForceBias;
                animator.SetBool("isFalling", true);
                MouseCompanionAnimatorSetter("isFalling", true);
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
        if (isRunning)
        {
            isSlide = true;
            animator.SetBool("isStartSlide", true);
            MouseCompanionAnimatorSetter("isStartSlide", true);
            animator.SetBool("isSliding", true);
            MouseCompanionAnimatorSetter("isSliding", true);
            capsuleCollider.height = 0.8f;
            capsuleCollider.center = new Vector3(0.0f, 0.4f, -0.2f);
            rigidbody.useGravity = false;
        }
    }

    public void EndSlide()
    {
        //Debug.Log("[@@@] isSlide : " + isSlide.ToString());
        //Debug.Log("[@@@] EndSlide");
        animator.SetBool("isStartSlide", false);
        MouseCompanionAnimatorSetter("isStartSlide", false);
        animator.SetBool("isSliding", false);
        MouseCompanionAnimatorSetter("isSliding", false);
        rigidbody.useGravity = true;
        capsuleCollider.height = standColliderHeight;
        capsuleCollider.center = standColliderCenter;
        isSlide = false;
        isRunning = true;
        //Debug.Log("[@@@] isSlide : " + isSlide.ToString());
    }

    IEnumerator AfterCollisionImmune()
    {
        Debug.Log("immune subroutine started");
        isPlayerImmuned = true;
        Debug.Log("isTrigger setted True");

        //animator.SetBool("isHit", false);
        yield return new WaitForSeconds(immuneTime);
        isPlayerImmuned = false;
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
        MouseCompanionAnimatorSetter("isHit", false);
        capsuleCollider.isTrigger = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Obstacle"))
        {
            Debug.Log("isHit anim go false");
            animator.SetBool("isHit", false);
            MouseCompanionAnimatorSetter("isHit", false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("[Collision(not trigger)] : " + collision.collider.gameObject.tag.ToString());
        if (collision.collider.gameObject.CompareTag("Ground") &&
            collision.collider.gameObject.transform.position.y > centerPosition.y)
        {
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Collision);
            animator.SetBool("isHit", true);
            MouseCompanionAnimatorSetter("isHit", true);
            CameraShaker.Invoke();

            if (!isPlayerImmuned)
            {
                if (playerHealth > 1)
                {
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.Collision);
                    playerHealth--;
                    SideViewGameplay1.sideViewGameplay1.playerHealth--;
                    StartCoroutine(AfterCollisionImmune());
                    animator.SetBool("isHit", false);
                    MouseCompanionAnimatorSetter("isHit", false);
                }
                else
                {
                    // stop and show left(or moved) distance to user?
                    // ...
                    animator.SetBool("isDead", true);
                    MouseCompanionAnimatorSetter("isDead", true);
                    player.GetComponent<SceneController>().toGameoverScene();
                }
            }
            else
            {
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Immune);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (player.tag.Equals("Dummy"))
        {
            return;
        }
        if (other.tag.Equals("Truck"))
        {
            animator.SetBool("isDead", true);
            MouseCompanionAnimatorSetter("isDead", true);
            player.GetComponent<SceneController>().toGameoverScene();
        }
        if (other.tag.Equals("Obstacle"))
        {
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Collision);

            if (!other.gameObject.name.Contains("Crane"))
            {
                Destroy(other.gameObject, 0.2f);
            }
            animator.SetBool("isHit", true);
            MouseCompanionAnimatorSetter("isHit", true);
            CameraShaker.Invoke();
            if (!isPlayerImmuned)
            {
                if (playerHealth > 0)
                {
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
                        if(mouseDummy == 0)
                        {
                            mouseSkillActivate = false;
                        }
                    }
                    else
                    {
                        StartCoroutine(AfterCollisionImmune());
                        AudioManager.instance.PlaySfx(AudioManager.Sfx.Collision);
                        playerHealth--;
                        SideViewGameplay1.sideViewGameplay1.playerHealth--;
                    }
                }
                if(playerHealth <= 0)
                {
                    // stop and show left(or moved) distance to user?
                    // ...
                    animator.SetBool("isDead", true);
                    MouseCompanionAnimatorSetter("isDead", true);
                    player.GetComponent<SceneController>().toGameoverScene();
                }
            }
            else
            {
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Immune);
            }
        }
        if (other.tag.Equals("Deadzone"))
        {
            player.GetComponent<SceneController>().toGameoverScene();
            playerHealth = 0;
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
            SideViewGameplay1.sideViewGameplay1.currentMapIdx = 0;
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
                    SideViewGameplay1.sideViewGameplay1.currentMapIdx = 12;
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

    public void MouseCompanionAnimatorSetter(string status, bool value)
    {
        if(MouseCompanions.Length > 0)
        {
            foreach(GameObject mouseCompanion in MouseCompanions)
            {
                Animator mouseCompanionAnimator = mouseCompanion.GetComponent<Animator>();
                mouseCompanionAnimator.SetBool(status, value);
            }
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

    public bool GetIsRunning()
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
