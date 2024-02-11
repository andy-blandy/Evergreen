using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    private ShopCamera shopCamera;
    public GameObject trigger;

    public List<ShopItem> allShopItems;
    public int totalNumOfItems;
    private List<ShopItem> shopItems;

    private bool inShop;

    public int curItemIndex;

    public static Shop instance;
    void Awake()
    {
        instance = this;

        shopCamera = GetComponent<ShopCamera>();
    }

    void Start()
    {
        GetRandomItems();
    }

    void Update()
    {
        if (inShop)
        {
            ShopUpdate();
        }
    }

    void ShopUpdate()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            NextItem();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            PrevItem();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            PurchaseItem();
        }
    }

    void GetRandomItems()
    {
        shopItems = new List<ShopItem>();

        for (int i = 0; i < totalNumOfItems; i++)
        {
            List<int> choices = new List<int>();
            for (int c = 0; c < allShopItems.Count; c++)
            {
                choices.Add(c);
            }

            int randomChoice = choices[Random.Range(0, allShopItems.Count)];
            choices.Remove(randomChoice);
            shopItems.Add(allShopItems[randomChoice]);
        }
    }

    public void EnterShop()
    {
        shopCamera.Enter();
        trigger.SetActive(false);
        curItemIndex = 0;
        inShop = true;

        ShopUI.instance.OpenDialogue();
        ShopUI.instance.SetText(shopItems[curItemIndex]);
    }

    public void ExitShop()
    {
        shopCamera.Exit();
        trigger.SetActive(true);
        inShop = false;

        ShopUI.instance.CloseDialogue();
    }

    void NextItem()
    {
        curItemIndex++;
        if (curItemIndex == totalNumOfItems)
        {
            curItemIndex = 0;
        }

        shopCamera.SwitchCamera(curItemIndex);
        ShopUI.instance.SetText(shopItems[curItemIndex]);
    }

    void PrevItem()
    {
        curItemIndex--;
        if (curItemIndex < 0)
        {
            curItemIndex = totalNumOfItems - 1;
        }

        shopCamera.SwitchCamera(curItemIndex);
        ShopUI.instance.SetText(shopItems[curItemIndex]);
    }

    void PurchaseItem()
    {
        ShopItem curItem = shopItems[curItemIndex];

        if (Player.instance.xp < curItem.cost)
        {
            return;
        }

        Player.instance.xp -= curItem.cost;

        StatManager.instance.UpgradeStat(curItem.stat, curItem.statChange);
    }
}
