using UnityEngine;
using UnityEngine.Video;

public class GameEnd : MonoBehaviour
{
    public VideoPlayer videoPlayer;  // Reference to the VideoPlayer component
    public BOX1 box1;                // Reference to the BOX1 component
    public int itemsToDestroy = 5;   // Number of items to trigger the game end

    private int itemsDestroyed = 0;  // Counter for the number of items destroyed

    private void Start()
    {
        if (box1 != null)
        {
            box1.ItemDestroyed += OnItemDestroyed;
        }
    }

    private void OnDestroy()
    {
        if (box1 != null)
        {
            box1.ItemDestroyed -= OnItemDestroyed;
        }
    }

    private void OnItemDestroyed()
    {
        itemsDestroyed++;
        if (itemsDestroyed >= itemsToDestroy)
        {
            PlayEndGameVideo();
        }
    }

    private void PlayEndGameVideo()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Play();
        }
    }
}
