using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopGemsItemController : MonoBehaviour
{
    [Header("Item Settings")]
    [SerializeField]
    private int moneyPrice;
    [SerializeField]
    private int gemsRewardValue;
    [SerializeField]
    private Sprite itemSprite;

    [Space]
    [Header("Editor")]
    [SerializeField]
    private ShopManager shopManager;
    [SerializeField]
    private TMPro.TMP_Text itemRewardTxt;
    [SerializeField]
    private GameObject itemIcon;
    [SerializeField]
    private GameObject moneyTxt;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(ShopItemBtnClick);
        itemRewardTxt.text = $"{gemsRewardValue} gems";
        itemIcon.GetComponent<Image>().sprite = itemSprite;
        moneyTxt.GetComponent<TMPro.TMP_Text>().text = $"{moneyPrice}";
        gameObject.GetComponent<Button>().interactable = true;
    }

    //сохраненное значение монет
    private int Coins
    {
        get
        {
            if (PlayerPrefs.HasKey("Coins"))
            {
                return PlayerPrefs.GetInt("Coins");
            }
            else
            {
                PlayerPrefs.SetInt("Coins", 0);
                return 0;
            }
        }
        set
        {
            PlayerPrefs.SetInt("Coins", value);
        }
    }

    //сохраненное значение монет
    private int Money
    {
        get
        {
            if (PlayerPrefs.HasKey("Money"))
            {
                return PlayerPrefs.GetInt("Money");
            }
            else
            {
                PlayerPrefs.SetInt("Money", 0);
                return 0;
            }
        }
        set
        {
            PlayerPrefs.SetInt("Money", value);
        }
    }

    //можем ли купить, выводим нужные экраны
    private void CanClaim()
    {
        if (Money < moneyPrice)
        {
            //не можем купить
            shopManager.PurchaseViewEnable(false);
        }
        else
        {
            //можем купить
            Money -= moneyPrice;
            //покупаем
            Coins += gemsRewardValue;
            shopManager.PurchaseViewEnable(true);
        }
    }

    //нажатие на кнопку покупки
    public void ShopItemBtnClick()
    {
        CanClaim();
    }
}
