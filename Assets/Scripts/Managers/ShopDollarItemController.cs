using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopDollarItemController : MonoBehaviour
{
    [Header("Item Settings")]
    [SerializeField]
    private string itemName;
    [SerializeField]
    private Sprite itemImage;

    [Header("Dollar Item")]
    [SerializeField]
    private float dollarPrice;
    [SerializeField]
    private int itemCoinReward;
    [SerializeField]
    private int itemKeyReward;

    [Space]
    [Header("Editor")]
    [SerializeField]
    private TMPro.TMP_Text itemNameTxt;
    [SerializeField]
    private GameObject itemIcon;
    [SerializeField]
    private GameObject itemRewardTxt;
    [SerializeField]
    private GameObject itemKeyRewardTxt;
    [SerializeField]
    private GameObject shopBtn;
    [SerializeField]
    private GameObject dollarTxt;
    [SerializeField]
    private ShopManager shopManager;

    void Start()
    {

        UpdateItemView();
    }

    //сохраненное значение ключей
    private int KeyReward
    {
        get
        {
            if (PlayerPrefs.HasKey("key"))
            {
                return PlayerPrefs.GetInt("key");
            }
            else
            {
                PlayerPrefs.SetInt("key", 0);
                return 0;
            }
        }
        set
        {
            PlayerPrefs.SetInt("key", value);
        }
    }

    //определяем вид кнопки
    private void UpdateItemView()
    {
        itemNameTxt.text = itemName;

        dollarTxt.SetActive(false);
        itemRewardTxt.SetActive(false);
        itemKeyRewardTxt.SetActive(false);
        itemIcon.GetComponent<Image>().sprite = itemImage;

        //устанавливаем отоброжение для покупки за доллары
        shopBtn.GetComponent<Button>().interactable = true;
        dollarTxt.GetComponent<TMPro.TMP_Text>().text = $"{dollarPrice.ToString("0.00")}$";
        dollarTxt.SetActive(true);
        itemIcon.SetActive(true);
        if (itemCoinReward > 0)
        {
            itemRewardTxt.GetComponent<TMPro.TMP_Text>().text = $"x{itemCoinReward}";
            itemRewardTxt.SetActive(true);
        }
        if (itemKeyReward > 0)
        {
            itemKeyRewardTxt.GetComponent<TMPro.TMP_Text>().text = $"x{itemKeyReward}";
            itemKeyRewardTxt.SetActive(true);
        }
        
    }


    //метод совершенной покупки
    public void PurchaseComplete()
    {
        //прибавляем монетки и ключи
        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + itemCoinReward);
        KeyReward += itemKeyReward;
        //вызываем вью успешной покупки
        shopManager.PurchaseViewEnable(true);
    }
}
