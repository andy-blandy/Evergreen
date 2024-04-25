using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public List<Dialogue> dialogueTree;

    [Header("Player Detection")]
    public GameObject dialogueIndicator;
    public bool inTrigger;

    public GameObject dialogueCamera;

    void Awake()
    {
        dialogueIndicator.SetActive(false);
    }

    void Update()
    {
        if (inTrigger && !DialogueManager.instance.inDialogue)
        {
            ListenForPress();
        }

        if (dialogueCamera != null && dialogueCamera.activeSelf != inTrigger)
        {
            dialogueCamera.SetActive(inTrigger);
        }
    }

    void ListenForPress()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DialogueManager.instance.StartDialogue(dialogueTree);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            inTrigger = true;


            // Display UI indicating the player can press a button to start the dialogue
            dialogueIndicator.SetActive(true);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            inTrigger = false;

            // Hide UI indicating the player can press a button to start the dialogue
            dialogueIndicator.SetActive(false);
        }
    }
}
