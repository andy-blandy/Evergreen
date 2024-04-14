using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    private ShopCamera shopCamera;
    public GameObject trigger;

    public ShopItem rerollItem;
    public List<ShopItem> allShopItems;
    public int totalNumOfItems;
    private List<ShopItem> shopItems;

    public List<ItemAnimation> flowerModels;

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

        shopItems.Add(rerollItem);
        foreach(ItemAnimation flower in flowerModels)
        {
            flower.flowerBaseAnimator.SetTrigger("GrowFlower");
        }
    }

    public void Reroll()
    {
        foreach(ItemAnimation flower in flowerModels)
        {
            flower.flowerPetalsAnimator.SetTrigger("Close");
            flower.flowerBaseAnimator.SetTrigger("RemoveFlower");
        }
        GetRandomItems();
    }

    public void EnterShop()
    {
        StartCoroutine(SetInShop(true));

        shopCamera.ActivateCameras();
        trigger.SetActive(false);
        curItemIndex = 0;

        Player.instance.FreezePlayer(true);

        ShopUI.instance.OpenDialogue();
        ShopUI.instance.SetText(shopItems[curItemIndex]);
    }

    public void ExitShop()
    {
        if (!inShop)
        {
            return;
        }

        StartCoroutine(SetInShop(false));

        shopCamera.DeactivateCameras();
        trigger.SetActive(true);

        Player.instance.FreezePlayer(false);

        ShopUI.instance.CloseDialogue();
    }

    IEnumerator SetInShop(bool value)
    {
        yield return new WaitForFixedUpdate();
        inShop = value;
        Player.instance.inShop = value;
    }

    void NextItem()
    {
        curItemIndex++;
        if (curItemIndex == totalNumOfItems + 1)
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
            curItemIndex = totalNumOfItems;
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

        curItem.Use();
        Player.instance.SetXP(Player.instance.xp - curItem.cost);

        if (curItemIndex < flowerModels.Count)
        {
            flowerModels[curItemIndex].flowerPetalsAnimator.SetTrigger("Bloom");
        }
    }
}
