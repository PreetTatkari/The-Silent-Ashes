using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PD : MonoBehaviour
{
    public Camera playerCamera;
    public List<GameObject> crosshairs; // List of crosshairs for different items
    public Transform itemHolder;
    public float maxPickupDistance = 5f;
    public LayerMask pickupLayer; // Layer to detect items for pickup

    private GameObject heldItem;

    // Dialogue related fields
    [SerializeField]
    private GameObject dialogueCanvas;

    [SerializeField]
    private TMP_Text dialogueText;

    [SerializeField]
    [TextArea]
    private string[] dialogueWords;

    [SerializeField]
    private AudioClip[] dialogueAudioClips;

    private bool dialogueActivated;
    private int step;
    private bool isTalking;

    void Start()
    {
        foreach (GameObject crosshair in crosshairs)
        {
            crosshair.SetActive(false); // Hide all crosshairs initially
        }

        dialogueCanvas.SetActive(false); // Hide dialogue canvas initially
    }

    void Update()
    {
        if (heldItem == null)
        {
            CheckForPickup();
        }
        else
        {
            HideAllCrosshairs(); // Hide crosshairs if holding an item

            // Drop item if holding and press Q
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Drop();
            }
        }

        if (dialogueActivated && Input.GetButtonDown("Interact") && !isTalking)
        {
            StartCoroutine(AdvanceDialogue());
        }
    }

    void CheckForPickup()
    {
        // Raycast from camera to detect items
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, maxPickupDistance, pickupLayer))
        {
            int crosshairIndex = GetCrosshairIndex(hit.transform.tag);
            if (crosshairIndex != -1 && heldItem == null)
            {
                ShowCrosshair(crosshairIndex); // Show the appropriate crosshair if pointing at an item

                if (Input.GetKeyDown(KeyCode.E))
                {
                    Pickup(hit.transform.gameObject, crosshairIndex);
                }
            }
            else
            {
                HideAllCrosshairs(); // Hide crosshairs if not pointing at an item
            }
        }
        else
        {
            HideAllCrosshairs(); // Hide crosshairs if no item is detected
        }
    }

    private void Pickup(GameObject item, int crosshairIndex)
    {
        if (heldItem != null)
        {
            return; // Can't pick up another item if already holding one
        }

        // Disable item's physics interactions temporarily
        Rigidbody itemRigidbody = item.GetComponent<Rigidbody>();
        if (itemRigidbody != null)
        {
            itemRigidbody.isKinematic = true;
            itemRigidbody.detectCollisions = false;
        }

        // Set item's parent to itemHolder
        item.transform.SetParent(itemHolder);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

        heldItem = item;

        // Start dialogue based on the crosshair index
        StartDialogue(crosshairIndex);
    }

    private void Drop()
    {
        if (heldItem == null)
        {
            return;
        }

        // Stop dialogue when dropping the item
        StopDialogue();

        Rigidbody heldItemRigidbody = heldItem.GetComponent<Rigidbody>();
        if (heldItemRigidbody != null)
        {
            // Enable item's physics interactions again
            heldItemRigidbody.isKinematic = false;
            heldItemRigidbody.detectCollisions = true;
        }

        // Release item
        heldItem.transform.SetParent(null); // Unparent the item

        heldItem = null;
    }

    private int GetCrosshairIndex(string tag)
    {
        // Determine which crosshair to show based on the item tag
        switch (tag)
        {
            case "Item":
                return 0;
            case "Item2":
                return 1;
            case "Item3":
                return 2;
            case "Item4":
                return 3;
            case "Item5":
                return 4;
            default:
                return -1;
        }
    }

    private void ShowCrosshair(int index)
    {
        for (int i = 0; i < crosshairs.Count; i++)
        {
            crosshairs[i].SetActive(i == index);
        }
    }

    private void HideAllCrosshairs()
    {
        foreach (GameObject crosshair in crosshairs)
        {
            crosshair.SetActive(false);
        }
    }

    private void StartDialogue(int index)
    {
        if (index < dialogueWords.Length && index < dialogueAudioClips.Length)
        {
            step = index; // Set the dialogue step to the index of the picked item
            dialogueCanvas.SetActive(true);
            StartCoroutine(AdvanceDialogue());
        }
    }

    private void StopDialogue()
    {
        StopAllCoroutines();
        dialogueCanvas.SetActive(false);
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        isTalking = false;
    }

private IEnumerator AdvanceDialogue()
{
    isTalking = true;

    if (step >= dialogueWords.Length)
    {
        dialogueCanvas.SetActive(false);
        step = 0;
    }
    else
    {
        dialogueCanvas.SetActive(true);
        dialogueText.text = dialogueWords[step];

        if (step < dialogueAudioClips.Length && dialogueAudioClips[step] != null)
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.clip = dialogueAudioClips[step];
            audioSource.Play();

            // Determine duration based on index
            float duration = GetDialogueDuration(step);
            yield return new WaitForSeconds(duration);

            // Clear dialogue text after the specified duration
            dialogueText.text = "";
        }
        step += 1;

        // Wait a bit before allowing the next interaction
        yield return new WaitForSeconds(0.2f);
    }

    isTalking = false;
}

private float GetDialogueDuration(int index)
{
    switch (index)
    {
        case 0:
            return 15f; // Element 0 lasts up to 15 seconds
        case 1:
            return 6f;  // Element 1 lasts up to 6 seconds
        case 2:
            return 10f; // Element 2 lasts up to 10 seconds
        case 3:
            return 4f;  // Element 3 lasts up to 4 seconds
        case 4:
            return 5f;  // Element 4 lasts up to 5 seconds
        default:
            return 0f;
    }
}


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            dialogueActivated = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            dialogueActivated = false;
            dialogueCanvas.SetActive(false);
            step = 0;
        }
    }
}

