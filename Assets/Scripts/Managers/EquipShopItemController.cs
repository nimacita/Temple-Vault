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

    //сохраненное значения рекорда
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

    //сохраненное значение купленных бонусов
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

    //определяем вид кнопки
    private void UpdateItemView()
    {
        //проверка если открыта
        if (requiredScore <= CurrentOpenScore)
        {
            //открыта всегда
            lockedIcon.SetActive(false);
            itemIcon.SetActive(true);
            coinTxt.SetActive(true);
            equiped.SetActive(false);
        }
        else
        {
            //закрыта
            gameObject.GetComponent<Button>().interactable = false;
            itemIcon.SetActive(false);
            lockedLvlTxt.text = $"Needed {requiredScore}";
            lockedIcon.SetActive(true);
        }

    }

    //можем ли купить, выводим нужные экраны
    private void CanClaim()
    {
        //если не куплено - покупаем
        if (PlayerPrefs.GetInt("Coins") < coinPrice)
        {
            //не можем купить
            shopManager.PurchaseViewEnable(false);
        }
        else
        {
            //можем купить
            //покупаем
            BonusCount += 1;
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - coinPrice);
            shopManager.PurchaseViewEnable(true);
        }
    }

    //нажатие на кнопку покупки
    public void ShopItemBtnClick()
    {
        CanClaim();
    }
}
