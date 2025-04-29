using UnityEngine;
using UnityEngine.UI;

public class HoverImageDisplay : MonoBehaviour
{
    public GameObject imageCanvas; // Reference to the canvas with the image

    private void Start()
    {
        // Ensure the image is hidden at the start
        imageCanvas.SetActive(false);
    }

    private void OnMouseEnter()
    {
        // Show the image when the mouse enters the object's collider
        imageCanvas.SetActive(true);
    }

    private void OnMouseExit()
    {
        // Hide the image when the mouse exits the object's collider
        imageCanvas.SetActive(false);
    }
}
