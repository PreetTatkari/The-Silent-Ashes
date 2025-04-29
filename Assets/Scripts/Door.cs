using UnityEngine;

public class Door : MonoBehaviour
{
    public float openAngle = 90f;
    public float openSpeed = 2f;
    public AudioClip openSound;  // Reference to the audio clip to be played when the door opens
    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;
    private MeshRenderer meshRenderer;
    private AudioSource audioSource;

    void Start()
    {
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(transform.eulerAngles + Vector3.up * openAngle);
        meshRenderer = GetComponent<MeshRenderer>();

        // Add AudioSource component if not already present
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // Ensure the audio does not play on awake
        audioSource.clip = openSound;
    }

    void Update()
    {
        if (isOpen)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, openRotation, Time.deltaTime * openSpeed);
        }
    }

    public void Open()
    {
        isOpen = true;
        if (meshRenderer != null)
        {
            meshRenderer.enabled = false;
        }

        // Play the open sound
        if (audioSource != null && openSound != null)
        {
            audioSource.Play();
        }
    }
}