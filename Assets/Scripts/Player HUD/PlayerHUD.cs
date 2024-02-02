using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    public TextMeshProUGUI healthText;

    public static PlayerHUD instance;
    void Awake()
    {
        instance = this;
    }

    public void UpdateHealth(int health)
    {
        healthText.text = health.ToString();
    }
}
