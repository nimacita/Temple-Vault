using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestItemController : MonoBehaviour
{

    public enum ItemReward
    {
        CoinReward = 1,
        KeyReward = 2
    }
    [Header("Item Settings")]
    [SerializeField]
    private ItemReward itemReward;
    [SerializeField]
    private int itemRewardCount;
    [SerializeField]
    private int neededScore;
    [SerializeField]
    [Range(0,2)]
    private int selectedLocInd;
    [SerializeField]
    private Sprite itemSprite;

    [Header("Editor")]
    public TMPro.TMP_Text itemName;
    public TMPro.TMP_Text itemCount;
    public GameObject itemIcon;
    public TMPro.TMP_Text itemRewardTxt;
    public GameObject itemCoinReward;
    public GameObject itemKeyReward;
    public GameObject itemClaimBtn;
    public Sprite claimBtnActiveSprite, claimBtnInactiveSprite;
    public GameObject itemClaimedImg;

    public GameObject claimPanel;
    public TMPro.TMP_Text claimPanelRewardTxt;
    public GameObject claimPaneRewardImg;

    public QuestManager questManager;

    void Start()
    {
        itemClaimBtn.GetComponent<Button>().onClick.AddListener(itemBtnClicked);
        UpdateItemView();
    }

    //разблокировали ли бонус
    private bool IsItemClaimed
    {
        get
        {
            if (PlayerPrefs.HasKey($"IsItemClaimed{neededScore}:{selectedLocInd}"))
            {
                if (PlayerPrefs.GetInt($"IsItemClaimed{neededScore}:{selectedLocInd}") == 1)
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
                PlayerPrefs.SetInt($"IsItemClaimed{neededScore}:{selectedLocInd}", 0);
                return false;
            }
        }
        set
        {
            if (value)
            {
                PlayerPrefs.SetInt($"IsItemClaimed{neededScore}:{selectedLocInd}", 1);
            }
            else
            {
                PlayerPrefs.SetInt($"IsItemClaimed{neededScore}:{selectedLocInd}", 0);
            }
        }
    }


    //текущий рекорд на локации
    private int CurrentScoreOnLoc
    {
        get
        {
            if (PlayerPrefs.HasKey($"CurrentScoreOnLoc{selectedLocInd}"))
            {
                return PlayerPrefs.GetInt($"CurrentScoreOnLoc{selectedLocInd}");
            }
            else
            {
                PlayerPrefs.SetInt($"CurrentScoreOnLoc{selectedLocInd}",0);
                return PlayerPrefs.GetInt($"CurrentScoreOnLoc{selectedLocInd}");
            }
        }

        set
        {
            PlayerPrefs.SetInt($"CurrentScoreOnLoc{selectedLocInd}", value);
        }
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

    //обновляем отображение кнопки
    private void UpdateItemView()
    {
        itemName.text = $"Collect {neededScore} items #{selectedLocInd + 1}";
        itemCount.text = $"{CurrentScoreOnLoc}/{neededScore}";
        itemIcon.GetComponent<Image>().sprite = itemSprite;
        itemRewardTxt.text = $"x{itemRewardCount}";

        if (itemReward == ItemReward.CoinReward)
        {
            itemCoinReward.SetActive(true);
            itemKeyReward.SetActive(false);
        }
        else
        {
            itemCoinReward.SetActive(false);
            itemKeyReward.SetActive(true);
        }
        if (IsItemClaimed)
        {
            itemClaimBtn.SetActive(false);
            itemClaimedImg.SetActive(true);
        }
        else
        {
            itemClaimBtn.SetActive(true);
            if (CurrentScoreOnLoc >= neededScore)
            {
                itemClaimBtn.GetComponent<Image>().sprite = claimBtnActiveSprite;
                itemClaimBtn.GetComponent<Button>().interactable = true;
            }
            else
            {
                itemClaimBtn.GetComponent<Image>().sprite = claimBtnInactiveSprite;
                itemClaimBtn.GetComponent<Button>().interactable = false;
            }
            itemClaimedImg.SetActive(false);
        }

    }

    //нажатие на кнопку айтема
    private void itemBtnClicked()
    {
        if (CurrentScoreOnLoc >= neededScore)
        {
            //включаем панель
            claimPanel.SetActive(true);
            claimPanelRewardTxt.text = $"X{itemRewardCount}";
            //выдаем награду
            if (itemReward == ItemReward.CoinReward)
            {
                Coins += itemRewardCount;
                claimPaneRewardImg.GetComponent<Image>().sprite = itemCoinReward.GetComponent<Image>().sprite;
            }
            else
            {
    ;           KeyReward += itemRewardCount;
                claimPaneRewardImg.GetComponent<Image>().sprite = itemKeyReward.GetComponent<Image>().sprite;
            }
            IsItemClaimed = true;
            UpdateItemView();
            questManager.UpdateCurrentView();
        }
    }

    public bool GetIsItemClaimed()
    {
        return IsItemClaimed;
    }
}
