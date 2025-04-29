using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public string requiredKeyTag; // The tag of the key that opens this door
    public Animator anim; // Animator component for door
    public AudioClip doorSound; // AudioClip for door opening/closing sound
    private AudioSource audioSource; // AudioSource component for playing the sound

    private bool playerInRange = false; // To track if player is in range
    private KeyPickup playerKeyPickup; // Reference to player's KeyPickup script

    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        playerKeyPickup = GameObject.FindGameObjectWithTag("Player").GetComponent<KeyPickup>();
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (playerKeyPickup.HasKey(requiredKeyTag))
            {
                OpenDoor();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void OpenDoor()
    {
        anim.SetBool("DoorOpen", true);
        anim.SetBool("DoorClose", false);
        audioSource.clip = doorSound;
        audioSource.Play();
    }

    private void CloseDoor()
    {
        anim.SetBool("DoorOpen", false);
        anim.SetBool("DoorClose", true);
    }
}
