using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipShopItemController : MonoBehaviour
{

    [Header("Item Settings")]
    [SerializeField]
    private string itemName;

    [Header("Coin Item")]
    [SerializeField]
    private int coinPrice;
    [SerializeField]
    private int requiredScore;

    [Space]
    [Header("Equip Reward")]
    [SerializeField]
    private Sprite currentEquipSprite;


    [Space]
    [Header("Editor")]
    [SerializeField]
    private ShopManager shopManager;
    [SerializeField]
    private TMPro.TMP_Text itemNameTxt;
    [SerializeField]
    private GameObject itemIcon;
    [SerializeField]
    private GameObject lockedIcon;
    [SerializeField]
    private TMPro.TMP_Text lockedLvlTxt;
    [SerializeField]
    private GameObject shopBtn;
    [SerializeField]
    private GameObject coinTxt;
    [SerializeField]
    private GameObject equiped;

    void Start()
    {
        itemNameTxt.text = itemName;
        itemIcon.GetComponent<Image>().sprite = currentEquipSprite;
        coinTxt.SetActive(false);
        coinTxt.GetComponent<TMPro.TMP_Text>().text = $"{coinPrice}";
        gameObject.GetComponent<Button>().interactable = true;

        UpdateItemView();
    }

    private void FixedUpdate()
    {
        UpdateItemView();
    }

    //����������� �������� �������
    private int CurrentOpenScore
    {
        get
        {
            if (PlayerPrefs.HasKey("currentOpenScore"))
            {
                return PlayerPrefs.GetInt("currentOpenScore");
            }
            else
            {
                PlayerPrefs.SetInt("currentOpenScore", 0);
                return 0;
            }
        }
        set
        {
            PlayerPrefs.SetInt("currentOpenScore", value);
        }
    }

    //����������� �������� ��������� �������
    private int BonusCount
    {
        get
        {
            if (PlayerPrefs.HasKey("BonusCount"))
            {
                return PlayerPrefs.GetInt("BonusCount");
            }
            else
            {
                PlayerPrefs.SetInt("BonusCount", 0);
                return 0;
            }
        }
        set
        {
            PlayerPrefs.SetInt("BonusCount", value);
        }
    }

    //���������� ��� ������
    private void UpdateItemView()
    {
        //�������� ���� �������
        if (requiredScore <= CurrentOpenScore)
        {
            //������� ������
            lockedIcon.SetActive(false);
            itemIcon.SetActive(true);
            coinTxt.SetActive(true);
            equiped.SetActive(false);
        }
        else
        {
            //�������
            gameObject.GetComponent<Button>().interactable = false;
            itemIcon.SetActive(false);
            lockedLvlTxt.text = $"Needed {requiredScore}";
            lockedIcon.SetActive(true);
        }

    }

    //����� �� ������, ������� ������ ������
    private void CanClaim()
    {
        //���� �� ������� - ��������
        if (PlayerPrefs.GetInt("Coins") < coinPrice)
        {
            //�� ����� ������
            shopManager.PurchaseViewEnable(false);
        }
        else
        {
            //����� ������
            //��������
            BonusCount += 1;
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - coinPrice);
            shopManager.PurchaseViewEnable(true);
        }
    }

    //������� �� ������ �������
    public void ShopItemBtnClick()
    {
        CanClaim();
    }
}
