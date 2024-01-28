using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue", order = 1)]
public class Dialogue : ScriptableObject
{
    [TextArea(15, 20)]
    public string text;

    public Dialogue(string text)
    {
        this.text = text;
    }
}
