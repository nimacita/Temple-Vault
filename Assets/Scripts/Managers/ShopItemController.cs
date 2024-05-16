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

    //��� ������� ����� ���������
    [Space]
    [Header("Character Skin Reward")]
    [SerializeField]
    private Sprite currentCharacterSkin;
    [SerializeField]
    [Space(5), Tooltip("������ ��������� ����� ��� ������� ������� ")]
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

    //����������� �������� ������ �� �����
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

    //������� ���� ���������
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

    //���������� ��� ������
    private void UpdateItemView()
    {
        itemIcon.SetActive(true);

        //��������� ������� ��
        if (!IsShopItemPurchased)
        {
            //���� �� ������
            coinTxt.SetActive(true);
            equiped.SetActive(false);
        }
        else
        {
            //���� ������
            coinTxt.SetActive(false);
            equiped.SetActive(IsEquiped());
        }

    }

    //��������� ���� ���� ����������
    private bool IsEquiped()
    {
        bool isEquiped = false;
        //���� ���������
        //��������� �������� ����� ������� � ����������� ���������
        if (playerSkinSelectedInd == CurrentCharacterSkinInd)
        {
            isEquiped = true;
        }
        return isEquiped;
    }

    //����� �� ������, ������� ������ ������
    private void CanClaim()
    {
        if (!IsShopItemPurchased) {
            //���� �� ������� - ��������
            if (PlayerPrefs.GetInt("Coins") < coinPrice)
            {
                //�� ����� ������
                shopManager.PurchaseViewEnable(false);
            }
            else
            {
                //����� ������
                PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - coinPrice);
                shopManager.ItepPurchaseEnable(currentCharacterSkin, itemName);
                IsShopItemPurchased = true;
            }
        }
        else
        {
            //����� �� ������� - ���������
            EquipedSelectProduct();
        }
    }

    //��������� ��� �������
    private void EquipedSelectProduct()
    {
        //����� ���������
        if (playerSkinSelectedInd != CurrentCharacterSkinInd)
        {
            //��������� 
            CurrentCharacterSkinInd = playerSkinSelectedInd;
        }
        else
        {
            //������� 
            CurrentCharacterSkinInd = 0;
        }
    }

    //������� �� ������ �������
    public void ShopItemBtnClick()
    {
        CanClaim();
    }
    
}
