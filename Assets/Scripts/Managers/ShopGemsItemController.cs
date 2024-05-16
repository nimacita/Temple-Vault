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

    //����������� �������� �����
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

    //����������� �������� �����
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

    //����� �� ������, ������� ������ ������
    private void CanClaim()
    {
        if (Money < moneyPrice)
        {
            //�� ����� ������
            shopManager.PurchaseViewEnable(false);
        }
        else
        {
            //����� ������
            Money -= moneyPrice;
            //��������
            Coins += gemsRewardValue;
            shopManager.PurchaseViewEnable(true);
        }
    }

    //������� �� ������ �������
    public void ShopItemBtnClick()
    {
        CanClaim();
    }
}
