using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EndingController : MonoBehaviour
{
    [SerializeField]
    Sprite[] endingImages;
    [SerializeField]
    SpriteRenderer endingSprite;
    [SerializeField]
    GameObject skipButton;
    [SerializeField]
    GameObject endButton;


    [Header("Zoomout Main Camera")]
    [SerializeField] GameObject mainCamera;
    [SerializeField] float zoomOutSpeed = 5f; // 줌아웃 속도
    [SerializeField] float maxDistance = -1200f;

    private bool isSkipButtonVisible = false;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < endingImages.Length; i++)
        {
            if(i + 1 == CharacterInfo.characterNumber)
            {
                endingSprite.sprite = endingImages[i];
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))
        {
            ShowSkipButton();
        }

        if (mainCamera.transform.position.z <= -1198)
        {
            endButton.SetActive(true);
        }

        if (mainCamera != null)
        {
            // 카메라를 천천히 뒤로 이동
            Vector3 targetPosition = mainCamera.transform.position + Vector3.back * zoomOutSpeed * Time.deltaTime;
            if (targetPosition.z > maxDistance)
            {
                mainCamera.transform.position = targetPosition;
            }
        }
    }

    private void ShowSkipButton()
    {
        if (!isSkipButtonVisible && skipButton != null && mainCamera.transform.position.z > -1198)
        {
            skipButton.SetActive(true);
            isSkipButtonVisible = true;
        }
    }

    public void SkipEnding()
    {
        skipButton.SetActive(false);
        isSkipButtonVisible = false;
        mainCamera.transform.position = new Vector3(1480, 680, -1200);
        endButton.SetActive(true);
    }
}
