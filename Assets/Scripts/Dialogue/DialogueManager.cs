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
    public int positionInDialogue;

    // DELETE THIS
    string testText = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla ac ornare nisi, id laoreet mauris. Duis viverra placerat dolor sed aliquet. Ut vitae pharetra tortor, id sodales sapien. Integer fringilla laoreet lacus";

    private YieldInstruction letterYield;
    private YieldInstruction fastLetterYield;

    private string invisibleTag = "<alpha=#00>";

    void Awake()
    {
        letterYield = new WaitForSeconds(readingSpeed);
        fastLetterYield = new WaitForSeconds(fastReadingSpeed);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            dialogueUI.SetActive(true);
            StartCoroutine(TypeDialogue(testText));
        }
    }


    IEnumerator TypeDialogue(string textToType)
    {
        okIndicator.SetActive(false);

        textBox.text = "<alpha=#00>" + textToType;
        int i = 0;

        while (i < textToType.Length)
        {
            yield return letterYield;

            i++;
            string newText = textToType.Insert(i, invisibleTag);
            textBox.text = newText;
        }

        okIndicator.SetActive(true);
    }
}
