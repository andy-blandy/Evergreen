using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    public GameObject ShopDialogue;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI costText;

    public static ShopUI instance;

    void Awake()
    {
        instance = this;
    }

    public void OpenDialogue()
    {
        ShopDialogue.SetActive(true);
    }

    public void CloseDialogue()
    {
        ShopDialogue.SetActive(false);
    }

    public void SetText(ShopItem item)
    {
        nameText.text = item.name;
        descriptionText.text = item.description;
        costText.text = item.cost.ToString();
    }
}
