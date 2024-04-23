using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI xpText;

    [Header("Dashes")]
    public Transform dashRoot;
    public float dashRechargeSpeed;

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

    public void AnimateDashUse(int dashIndex, float timeBeforeRecharge, float rechargeTime)
    {
        Slider currentSlider = dashRoot.GetChild(dashIndex).GetChild(0).GetComponent<Slider>();
        currentSlider.value = 0;
        StartCoroutine(RechargeDash(currentSlider, timeBeforeRecharge, rechargeTime));
    }

    IEnumerator RechargeDash(Slider sliderToRecharge, float timeBeforeRecharge, float rechargeTime)
    {
        sliderToRecharge.transform.parent.SetAsLastSibling();
        yield return new WaitForSeconds(timeBeforeRecharge);

        // Animate the dash recharging
        float rechargeStep = rechargeTime / 10;
        while (sliderToRecharge.value < 1)
        {
            sliderToRecharge.value = sliderToRecharge.value + 0.1f;
            yield return new WaitForSeconds(rechargeStep);
        }
    }
}
