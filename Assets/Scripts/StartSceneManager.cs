using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class StartSceneManager : MonoBehaviour
{
    public KamlaAI kamlaAI;  // Reference to the KamlaAI script
    public VideoPlayer startSceneVideo;  // VideoPlayer for the start scene
    public GameObject startSceneUI;  // UI elements for the start scene

    private void Start()
    {
        // Ensure Kamla is disabled at the start
        kamlaAI.enabled = false;

        // Play the start scene video if available
        if (startSceneVideo != null)
        {
            startSceneVideo.Play();
            // Wait until the video is finished before enabling Kamla
            StartCoroutine(WaitForVideoToEnd(startSceneVideo));
        }
        else
        {
            // If no video, enable Kamla immediately
            EnableKamla();
        }
    }

    private IEnumerator WaitForVideoToEnd(VideoPlayer videoPlayer)
    {
        // Wait until the video has finished playing
        while (videoPlayer.isPlaying)
        {
            yield return null;
        }

        // Enable Kamla after the video ends
        EnableKamla();
    }

    private void EnableKamla()
    {
        // Hide the start scene UI if available
        if (startSceneUI != null)
        {
            startSceneUI.SetActive(false);
        }

        // Enable Kamla
        kamlaAI.enabled = true;
    }
}
