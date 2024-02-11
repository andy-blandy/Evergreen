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

    void OnEnable()
    {
        PlayerInput.OnInteract += PurchaseItem;
        PlayerInput.OnBack += ExitShop;
    }

    void OnDisable()
    {
        PlayerInput.OnInteract -= PurchaseItem;
        PlayerInput.OnBack -= ExitShop;
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
    }

    /*
     * Fills the shop with random items from the allShopItems list
     */
    void GetRandomItems()
    {
        List<int> choices = new List<int>();
        for (int c = 0; c < allShopItems.Count; c++)
        {
            choices.Add(c);
        }

        shopItems = new List<ShopItem>();
        for (int i = 0; i < totalNumOfItems; i++)
        {
            int randomChoice = choices[Random.Range(0, choices.Count)];
            choices.Remove(randomChoice);
            shopItems.Add(allShopItems[randomChoice]);
        }
    }

    public void EnterShop()
    {
        StartCoroutine(SetInShop(true));

        shopCamera.Enter();
        trigger.SetActive(false);
        curItemIndex = 0;

        Player.instance.FreezePlayer(true);

        ShopUI.instance.OpenDialogue();
        ShopUI.instance.SetText(shopItems[curItemIndex]);
    }

    public void ExitShop()
    {
        StartCoroutine(SetInShop(false));

        shopCamera.Exit();
        trigger.SetActive(true);

        Player.instance.FreezePlayer(false);

        ShopUI.instance.CloseDialogue();
    }

    IEnumerator SetInShop(bool value)
    {
        yield return new WaitForFixedUpdate();
        inShop = value;
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

        if (!inShop ||
            Player.instance.xp < curItem.cost)
        {
            return;
        }

        if (curItem.stat == StatManager.StatType.Health &&
            Player.instance.playerHealth.health >= Player.instance.playerHealth.maxHealth)
        {
            return;
        }

        Player.instance.SetXP(Player.instance.xp - curItem.cost);
        StatManager.instance.UpgradeStat(curItem.stat, curItem.statChange);
    }
}
