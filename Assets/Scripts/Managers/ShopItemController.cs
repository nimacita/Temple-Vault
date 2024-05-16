using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ShopItemController;

public class ShopItemController : MonoBehaviour
{

    [Header("Item Settings")]
    [SerializeField]
    private string itemName;

    [Header("Coin Item")]
    [SerializeField]
    private int coinPrice;

    //для покупки скина персонажа
    [Space]
    [Header("Character Skin Reward")]
    [SerializeField]
    private Sprite currentCharacterSkin;
    [SerializeField]
    [Space(5), Tooltip("индекс персонажа скина для покупки начиная ")]
    [Range(0, 3)]
    private int playerSkinSelectedInd;

    [Space]
    [Header("Editor")]
    [SerializeField]
    private ShopManager shopManager;
    [SerializeField]
    private TMPro.TMP_Text itemNameTxt;
    [SerializeField]
    private GameObject itemIcon;
    [SerializeField]
    private GameObject shopBtn;
    [SerializeField]
    private GameObject coinTxt;
    [SerializeField]
    private GameObject equiped;

    void Start()
    {
        itemNameTxt.text = itemName;
        itemIcon.GetComponent<Image>().sprite = currentCharacterSkin;
        coinTxt.SetActive(false);
        coinTxt.GetComponent<TMPro.TMP_Text>().text = $"{coinPrice}";
        gameObject.GetComponent<Button>().interactable = true;
        gameObject.GetComponent<Button>().onClick.AddListener(ShopItemBtnClick);

        UpdateItemView();
    }

    private void FixedUpdate()
    {
        UpdateItemView();
    }

    //сохраненное значение куплен ли товар
    private bool IsShopItemPurchased
    {
        get
        {
            if (PlayerPrefs.HasKey($"IsShopItemPurchased{itemName}"))
            {
                if (PlayerPrefs.GetInt($"IsShopItemPurchased{itemName}") == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (playerSkinSelectedInd == 0) 
                {
                    PlayerPrefs.SetInt($"IsShopItemPurchased{itemName}", 1);
                    return true;
                }
                else
                {
                    PlayerPrefs.SetInt($"IsShopItemPurchased{itemName}", 0);
                    return false;
                }
            }
        }

        set
        {
            if (value)
            {
                PlayerPrefs.SetInt($"IsShopItemPurchased{itemName}", 1);
            }
            else
            {
                PlayerPrefs.SetInt($"IsShopItemPurchased{itemName}", 0);
            }
        }
    }

    //текущий скин персонажа
    private int CurrentCharacterSkinInd
    {
        get
        {
            if (PlayerPrefs.HasKey($"CurrentCharacterInd"))
            {
                return PlayerPrefs.GetInt($"CurrentCharacterInd");
            }
            else
            {
                PlayerPrefs.SetInt($"CurrentCharacterInd", 0);
                return PlayerPrefs.GetInt($"CurrentCharacterInd");
            }
        }

        set
        {
            PlayerPrefs.SetInt($"CurrentCharacterInd", value);
        }

    }

    //определяем вид кнопки
    private void UpdateItemView()
    {
        itemIcon.SetActive(true);

        //проверить куплено ли
        if (!IsShopItemPurchased)
        {
            //если не куплен
            coinTxt.SetActive(true);
            equiped.SetActive(false);
        }
        else
        {
            //если куплен
            coinTxt.SetActive(false);
            equiped.SetActive(IsEquiped());
        }

    }

    //проверяем если скин экипирован
    private bool IsEquiped()
    {
        bool isEquiped = false;
        //скин персонажа
        //проверяем значение имени спрайта с сохраненным значением
        if (playerSkinSelectedInd == CurrentCharacterSkinInd)
        {
            isEquiped = true;
        }
        return isEquiped;
    }

    //можем ли купить, выводим нужные экраны
    private void CanClaim()
    {
        if (!IsShopItemPurchased) {
            //если не куплено - покупаем
            if (PlayerPrefs.GetInt("Coins") < coinPrice)
            {
                //не можем купить
                shopManager.PurchaseViewEnable(false);
            }
            else
            {
                //можем купить
                PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - coinPrice);
                shopManager.ItepPurchaseEnable(currentCharacterSkin, itemName);
                IsShopItemPurchased = true;
            }
        }
        else
        {
            //иначе по нажатию - экипируем
            EquipedSelectProduct();
        }
    }

    //экипируем или снимаем
    private void EquipedSelectProduct()
    {
        //скина персонажа
        if (playerSkinSelectedInd != CurrentCharacterSkinInd)
        {
            //экипируем 
            CurrentCharacterSkinInd = playerSkinSelectedInd;
        }
        else
        {
            //снимаем 
            CurrentCharacterSkinInd = 0;
        }
    }

    //нажатие на кнопку покупки
    public void ShopItemBtnClick()
    {
        CanClaim();
    }
    
}
