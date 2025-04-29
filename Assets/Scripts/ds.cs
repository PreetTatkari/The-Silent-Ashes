using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertSoundManager : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 10.0f;  // Range within which the sound will trigger
    public AudioClip[] alertSounds = new AudioClip[5];  // Array to hold the alert sounds
    public float soundCooldown = 5.0f;  // Cooldown between sound triggers

    private AudioSource audioSource;
    private float nextSoundTime = 0f;  // Time at which the next sound can be played
    private bool isPlayerInRange = false;  // Track if the player is in range

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        bool playerInRange = IsPlayerInRangeAndVisible();

        if (playerInRange && Time.time >= nextSoundTime)
        {
            if (!isPlayerInRange)
            {
                PlayRandomSound();
                nextSoundTime = Time.time + soundCooldown;
                isPlayerInRange = true;
            }
        }
        else if (!playerInRange && isPlayerInRange)
        {
            StopSound();
            isPlayerInRange = false;
        }
    }

    private bool IsPlayerInRangeAndVisible()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer <= detectionRange)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, directionToPlayer.normalized, out hit, detectionRange))
            {
                if (hit.collider.transform == player)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void PlayRandomSound()
    {
        if (alertSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, alertSounds.Length);
            AudioClip selectedSound = alertSounds[randomIndex];

            if (selectedSound != null)
            {
                audioSource.clip = selectedSound;
                audioSource.loop = true;  // Set to true if you want the sound to loop
                audioSource.Play();
            }
        }
    }

    private void StopSound()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
