using UnityEngine;
using UnityEngine.Video;

public class GameOverVideoPlayer : MonoBehaviour
{
    public Transform player; // Reference to the player transform
    public Transform ghost;  // Reference to the ghost transform
    public GameObject videoPlayerCutscene; // Reference to the VideoPlayerCutscene GameObject
    public float detectionRange = 5f; // Range within which the video will play
    public FirstPersonMovement playerControlScript; // Reference to the player's control script

    private VideoPlayer videoPlayer;
    private bool isVideoPlaying = false;
    private Collider playerCollider; // Reference to the player's collider
    private Vector3 playerPosition; // To store the player's position

    void Start()
    {
        if (videoPlayerCutscene != null)
        {
            videoPlayer = videoPlayerCutscene.GetComponent<VideoPlayer>();
            videoPlayerCutscene.SetActive(false); 
            videoPlayer.loopPointReached += OnVideoFinished;
        }

        if (player != null)
        {
            playerCollider = player.GetComponent<Collider>();
        }
    }

    void Update()
    {
        if (player != null && ghost != null && videoPlayer != null)
        {
            float distance = Vector3.Distance(player.position, ghost.position);

            if (distance <= detectionRange && !isVideoPlaying)
            {
                PlayGameOverVideo();
            }
        }
    }

    void PlayGameOverVideo()
    {
        if (!videoPlayer.isPlaying)
        {
            playerPosition = player.position; // Store the player's position

            videoPlayerCutscene.SetActive(true);
            videoPlayer.Play();
            isVideoPlaying = true;

            if (playerControlScript != null)
            {
                playerControlScript.enabled = false; 
            }

            if (playerCollider != null)
            {
                playerCollider.enabled = false; // Disable the player's collider
            }
        }
    }

    void StopGameOverVideo()
    {
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
        }

        videoPlayerCutscene.SetActive(false);
        isVideoPlaying = false;

        if (playerControlScript != null)
        {
            playerControlScript.enabled = true; 
        }

        if (playerCollider != null)
        {
            playerCollider.enabled = true; // Enable the player's collider
        }

        player.position = playerPosition; // Reset the player's position
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        StopGameOverVideo();
    }
}
