using UnityEngine;
using TMPro;

public class Letter : MonoBehaviour
{
    public string letterContent; // Content of the letter

    private bool isPlayerNearby; // Flag to track if player is nearby

    void Start()
    {
        isPlayerNearby = false;
    }

    void Update()
    {
        // Check for player interaction if nearby
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            InteractWithPlayer();
        }
    }

    // Method to interact with the player
    private void InteractWithPlayer()
    {
        // Example action: Display the letter content
        Debug.Log("Letter content: " + letterContent);

        // Optionally, trigger any specific actions related to the letter content
        // For example, you can display the content on a UI canvas.
        // ExampleUIManager.DisplayLetterContent(letterContent);

        // You can extend this method to perform additional actions as needed.
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the trigger area
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            Debug.Log("Player entered letter trigger area.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the player exits the trigger area
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            Debug.Log("Player exited letter trigger area.");
        }
    }
}
