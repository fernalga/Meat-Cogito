using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoScenes : MonoBehaviour
{
    public VideoPlayer videoPlayer;           // VideoPlayer on 'Intro'
    public GameObject introObject;            // The 'Intro' GameObject
    public GameObject afterVideoObject;       // The 'AfterVideoScreen' GameObject

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("Video ended – swapping UI");

        videoPlayer.Stop();
        videoPlayer.enabled = false;

        // Disable intro (video screen) and enable the new screen
        introObject.SetActive(false);
        afterVideoObject.SetActive(true);
    }
    
    public void PlayVideo()
    {
        introObject.SetActive(true);
        afterVideoObject.SetActive(false);

        videoPlayer.enabled = true;
        videoPlayer.Play();

        Debug.Log("Video started!");
    }
}
