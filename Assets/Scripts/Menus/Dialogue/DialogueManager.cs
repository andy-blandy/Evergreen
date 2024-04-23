/*
 * Written by Andrew
 * @andy_blandy
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("UI Objects")]
    public GameObject dialogueUI;
    public Image dialogueBackground;
    public TextMeshProUGUI textBox;
    public GameObject okIndicator;

    [Header("Text Speed")]
    public float readingSpeed;
    public float fastReadingSpeed;

    [Header("Dialogue Logic")]
    public int positionInTree;
    public bool inDialogue;
    public bool isTyping;
    public bool speedUp;
    public bool dialogueCooling;
    public List<Dialogue> currentDialogueTree;
    private Coroutine typeDialogueCoroutine;

    // Corotuine Logic
    private YieldInstruction cooldownDialogue;
    private YieldInstruction letterYield;
    private YieldInstruction fastLetterYield;

    private string invisibleTag = "<alpha=#00>";
    private int wordLimit = 218;

    // Singleton
    public static DialogueManager instance;

    void Awake()
    {
        instance = this;

        // This instantiates the pauses between each letter typed
        cooldownDialogue = new WaitForSeconds(1);
        letterYield = new WaitForSeconds(readingSpeed);
        fastLetterYield = new WaitForSeconds(fastReadingSpeed);
    }

    void Update()
    {
        if (inDialogue)
        {
            DialogueUpdate();
        }
    }
    
    void DialogueUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ReadDialogueTree();
        }
    }

    public void StartDialogue(List<Dialogue> dialogueTree)
    {
        if (dialogueCooling)
        {
            return;
        }

        Player.instance.FreezePlayer(true);

        currentDialogueTree = dialogueTree;
        dialogueUI.SetActive(true);
        inDialogue = true;
        positionInTree = 0;
        ReadDialogueTree();
    }

    void ReadDialogueTree()
    {
        /*
         * Couple return statements below.
         * First makes sure there is actually a dialogue tree.
         * Second skips the typing coroutine and shows the full text.
         * Last stops the dialogue if the end of the current dialogue tree has been reached.
         */
        if (currentDialogueTree == null)
        {
            Debug.LogAssertion("DialogueTree is null!");
            EndDialogue();
            return;
        }
        if (typeDialogueCoroutine != null && isTyping)
        {
            StopCoroutine(typeDialogueCoroutine);

            okIndicator.SetActive(true);
            isTyping = false;

            textBox.text = currentDialogueTree[positionInTree - 1].text;
            return;
        }
        if (positionInTree >= currentDialogueTree.Count)
        {
            EndDialogue();
            return;
        }

        /*
         * If a message is being typed, we stop the typing coroutine and display the full message.
         * Otherwise, we begin typing the next message from the dialogue tree.
         */
        string currentText = currentDialogueTree[positionInTree].text;
        if (currentText.Length > wordLimit)
        {
            Debug.LogAssertion("The text in '" + currentDialogueTree[positionInTree].name + "' is too long! Try to keep each dialogue to under " + wordLimit + " characters.");
            SplitText(currentText, positionInTree + 1);

            currentText = currentText.Remove(wordLimit);
        }

        // Speed up the typing if needed
        if (currentText.Length > wordLimit * 0.5)
        {
            speedUp = true;
        } else
        {
            speedUp = false;
        }

        typeDialogueCoroutine = StartCoroutine(TypeDialogue(currentText));
        positionInTree++;
    }

    void SplitText(string textToSplit, int pos)
    {
        string newText = textToSplit.Remove(0, wordLimit - 1);
        Dialogue newDialogue;
        if (newText.Length < wordLimit)
        {
            newDialogue = new Dialogue(newText);
        } else
        {
            newDialogue = new Dialogue(newText.Remove(wordLimit));
        }
        currentDialogueTree.Insert(pos, newDialogue);

        if (newText.Length > wordLimit)
        {
            SplitText(newText, pos + 1);
        }
    }

    void EndDialogue()
    {
        if (isTyping)
        {
            StopCoroutine(typeDialogueCoroutine);
            isTyping = false;
        }

        dialogueUI.SetActive(false);
        inDialogue = false;
        StartCoroutine(DialogueCooldown());

        Player.instance.FreezePlayer(false);
    }

    IEnumerator TypeDialogue(string textToType)
    {
        okIndicator.SetActive(false);
        isTyping = true;

        /*
         * This coroutine makes the text appear by changing the alpha value of each letter
         * The invisibleTag is an aplha tag set to 0 (<alpha=#00>) that is placed in front of all the letters we 
         * don't want to see yet.
         */
        textBox.text = invisibleTag + textToType;
        for (int i = 0; i <= textToType.Length; i++)
        {
            if (speedUp)
            {
                yield return fastLetterYield;
            }
            else
            {
                yield return letterYield;
            }

            string newText = textToType.Insert(i, invisibleTag);
            textBox.text = newText;
        }

        okIndicator.SetActive(true);
        isTyping = false;
    }

    /*
     * Adds a pause between each dialogue tree to prevent an infinite loop of dialogue
     */
    IEnumerator DialogueCooldown()
    {
        dialogueCooling = true;
        yield return cooldownDialogue;
        dialogueCooling = false;
    }
}
