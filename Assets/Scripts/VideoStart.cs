using UnityEngine;
using UnityEngine.Video;

public class VideoStart : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    public Canvas videoCanvas; // Reference to the Canvas

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoEnd; // Subscribe to the event
            videoPlayer.Play();
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        // Hide the canvas when the video finishes playing
        if (videoCanvas != null)
        {
            videoCanvas.gameObject.SetActive(false);
        }
    }
}
