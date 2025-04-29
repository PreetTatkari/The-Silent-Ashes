using UnityEngine;
using UnityEngine.UI;

public class Key : MonoBehaviour
{
    public Door door;
    public Canvas keyCanvas;

    void Start()
    {
        if (keyCanvas != null)
        {
            keyCanvas.enabled = false; // Hide the canvas initially
        }
    }

    void OnMouseDown()
    {
        if (door != null)
        {
            door.Open();
            if (keyCanvas != null)
            {
                Destroy(keyCanvas.gameObject); // Remove the canvas when the key is clicked
            }
            Destroy(gameObject); // Remove the key after it's used
        }
    }

    void OnMouseEnter()
    {
        if (keyCanvas != null)
        {
            keyCanvas.enabled = true; // Show the canvas when aiming at the key
        }
    }

    void OnMouseExit()
    {
        if (keyCanvas != null)
        {
            keyCanvas.enabled = false; // Hide the canvas when not aiming at the key
        }
    }
}
