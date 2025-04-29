using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialogues : MonoBehaviour
{
    [SerializeField]
    private GameObject dialogueCanvas;

    [SerializeField]
    private TMP_Text speakerText;

    [SerializeField]
    private TMP_Text dialogueText;

    [SerializeField]
    private Image portraitImage;

    [SerializeField]
    private string[] speaker;

    [SerializeField]
    [TextArea]
    private string[] dialogueWords;

    [SerializeField]
    private Sprite[] portrait;

    [SerializeField]
    private AudioClip[] dialogueAudioClips;

    private bool dialogueActivated;
    private int step;
    private bool isTalking;

    void Update()
    {
        if (dialogueActivated && Input.GetButtonDown("Interact") && !isTalking)
        {
            StartCoroutine(AdvanceDialogue());
        }
    }

    private IEnumerator AdvanceDialogue()
    {
        isTalking = true;

        if (step >= speaker.Length)
        {
            dialogueCanvas.SetActive(false);
            step = 0;
        }
        else
        {
            dialogueCanvas.SetActive(true);
            speakerText.text = speaker[step];
            dialogueText.text = dialogueWords[step];
            portraitImage.sprite = portrait[step];

            if (step < dialogueAudioClips.Length && dialogueAudioClips[step] != null)
            {
                AudioSource audioSource = GetComponent<AudioSource>();
                audioSource.clip = dialogueAudioClips[step];
                audioSource.Play();
            }
            step += 1;

            // Wait a bit before allowing the next interaction
            yield return new WaitForSeconds(0.2f);
        }

        isTalking = false;
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
