using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI xpText;

    public static PlayerHUD instance;
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateXP(Player.instance.xp);
        UpdateHealth(Player.instance.playerHealth.health);
    }

    public void UpdateXP(int xp)
    {
        xpText.text = xp.ToString();
    }

    public void UpdateHealth(int health)
    {
        healthText.text = health.ToString();
    }
}
