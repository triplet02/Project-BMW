using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class OpeningController : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    public GameObject skipButton;
    private bool isSkipButtonVisible = false;

    // Start is called before the first frame update
    void Start()
    {
        if (skipButton != null)
        {
            skipButton.SetActive(false);
        }

        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))
        {
            ShowSkipButton();
        }
    }

    public void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene("Project BMW");
    }

    public void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoEnd;
        }
    }

    private void ShowSkipButton()
    {
        if (!isSkipButtonVisible && skipButton != null)
        {
            skipButton.SetActive(true); // 버튼 활성화
            isSkipButtonVisible = true;
        }
    }

    public void ToProjectBMWScene()
    {
        SceneManager.LoadScene("Project BMW");
    }
}
