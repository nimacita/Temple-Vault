using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BonusBtnController : MonoBehaviour
{

    [SerializeField]
    [Header("Bonus Settings")]
    private int bonusPrice;
    public enum BonusReward
    {
        CoinReward = 1,
        MysteryCoinReward = 2
    }
    [Space]
    [Header("Set Bonus Reward")]
    [SerializeField]
    private BonusReward bonusReward;
    [SerializeField]
    private Sprite bonusSprite;

    [Space]
    [SerializeField]
    [Header("To Coin Reward")]
    private int coinBonus;

    [Space]
    [SerializeField]
    [Header("To Mystery Reward")]
    private int maxCoinReward;
    [SerializeField]
    private int minCoinReward;

    [Space]
    [SerializeField]
    [Header("To timer")]
    private int hoursToWait;
    private bool canBuy;
    private bool isWait;
    private bool isUnlock;
    private DateTime currentTime;

    [Header("Debug")]
    [SerializeField]
    private int addHours = 0;


    [Space]
    [SerializeField]
    [Header("To Editor")]
    private GameObject unlockBonusBtn;
    [SerializeField]
    private GameObject bonusImg;
    [SerializeField]
    private Sprite unlockBtnSprite, cantUnlockBtnSprite, waitBtnSprite, openBtnSprite;
    [SerializeField]
    private GameObject timeBtnTxt;
    [SerializeField]
    private GameObject mysterySpoiler, coinSpoiler;
    [SerializeField]
    private TMPro.TMP_Text coinSpoilerTxt;
    [SerializeField]
    private TMPro.TMP_Text bonusPricetxt;
    [SerializeField]
    private BonusMenuManager bonusMenuManager;



    void Start()
    {
        unlockBonusBtn.GetComponent<Button>().onClick.AddListener(BonusBtnClick);
        UpdateBonusView();
    }

    private void Update()
    {
        currentTime = DateTime.Now.AddHours(addHours);
        UpdateBonusView();

        isWait = IsWaitBonus();
        isUnlock = IsUnlockClick;
        canBuy = CanUnlock();
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

    //время когда открыли бонус
    private DateTime UnlockBtnTime
    {
        get
        {
            if (!PlayerPrefs.HasKey($"LastGame{bonusPrice}"))
            {
                DateTime dateTime = new DateTime();
                return dateTime;
            }
            else
            {
                return DateTime.Parse(PlayerPrefs.GetString($"LastGame{bonusPrice}"));
            }
        }
        set
        {
            PlayerPrefs.SetString($"LastGame{bonusPrice}", value.ToString());
        }
    }

    //разблокировали ли бонус
    private bool IsUnlockClick
    {
        get
        {
            if (PlayerPrefs.HasKey($"IsUnlockClick{bonusPrice}"))
            {
                if (PlayerPrefs.GetInt($"IsUnlockClick{bonusPrice}") == 1)
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
                PlayerPrefs.SetInt($"IsUnlockClick{bonusPrice}", 0);
                return false;
            }
        }
        set
        {
            if (value)
            {
                PlayerPrefs.SetInt($"IsUnlockClick{bonusPrice}", 1);
            }
            else
            {
                PlayerPrefs.SetInt($"IsUnlockClick{bonusPrice}", 0);
            }
        }
    }

    //можем ли купить
    private bool CanUnlock()
    {
        if (bonusPrice <= KeyReward)
        {
            return true;
        }
        return false;
    }

    //обновляем отображение кнопки
    private void UpdateBonusView()
    {
        bonusImg.GetComponent<Image>().sprite = bonusSprite;
        bonusPricetxt.text = $"{KeyReward}/{bonusPrice}";
        timeBtnTxt.SetActive(false);
        unlockBonusBtn.GetComponent<Image>().sprite = cantUnlockBtnSprite;
        unlockBonusBtn.GetComponent<Button>().interactable = false;

        //устанавливаем спрайты на кнопку
        if (IsWaitBonus())
        {
            unlockBonusBtn.GetComponent<Image>().sprite = waitBtnSprite;
            timeBtnTxt.SetActive(true);
            timeBtnTxt.GetComponent<TMPro.TMP_Text>().text = TimerTxt();
            unlockBonusBtn.GetComponent<Button>().interactable = false;
        }
        else
        {
            //если можем разблокировать
            if (CanUnlock())
            {
               unlockBonusBtn.GetComponent<Image>().sprite = unlockBtnSprite;
               unlockBonusBtn.GetComponent<Button>().interactable = true;
               timeBtnTxt.SetActive(true);
               timeBtnTxt.GetComponent<TMPro.TMP_Text>().text = $"UNLOCK {hoursToWait} hrs";
            }
            else
            {
                unlockBonusBtn.GetComponent<Image>().sprite = cantUnlockBtnSprite;
                unlockBonusBtn.GetComponent<Button>().interactable = false;
                timeBtnTxt.SetActive(true);
                timeBtnTxt.GetComponent<TMPro.TMP_Text>().text = $"UNLOCK {hoursToWait} hrs";
            }

            //если можем открыть
            if (IsUnlockClick)
            {
                unlockBonusBtn.GetComponent<Image>().sprite = openBtnSprite;
                unlockBonusBtn.GetComponent<Button>().interactable = true;
                timeBtnTxt.SetActive(false);
            }
        }

        coinSpoiler.SetActive(false);
        mysterySpoiler.SetActive(false);

        if (bonusReward == BonusReward.CoinReward)
        {
            coinSpoiler.SetActive(true);
            coinSpoilerTxt.text = $"X{coinBonus}";
        }
        else
        {
            mysterySpoiler.SetActive(true);
        }
    }

    //обнволяем таймер ожидания
    private string TimerTxt()
    {
        TimeSpan sub = new TimeSpan(hoursToWait, 0, 0).Subtract(currentTime.Subtract(UnlockBtnTime));
        string txt = $"{sub.Hours:D2}:{sub.Minutes:D2}";
        return txt;
    }

    //получение денежного бонуса
    private void ClaimCoinBonus(int addCoin)
    {
        bonusMenuManager.OpenCoinCLaimPanel(bonusSprite, addCoin);
        Coins += addCoin;
    }

    //собираем разные бонусы
    private void ClaimBonuses()
    {
        if (bonusReward == BonusReward.CoinReward)
        {
            ClaimCoinBonus(coinBonus);

        }
        if (bonusReward == BonusReward.MysteryCoinReward)
        {
            ClaimCoinBonus(Random.Range(minCoinReward, maxCoinReward + 1));
        }
        UpdateBonusView();
    }

    //ждем ли бонус
    private bool IsWaitBonus()
    {
        if (IsUnlockClick) 
        {
            //Debug.Log(currentTime.Subtract(UnlockBtnTime).Hours);
            if (currentTime.Subtract(UnlockBtnTime).Hours >= hoursToWait)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }
    }

    //нажатие на кнопку
    public void BonusBtnClick()
    {
        //если не ждем
        if (!IsWaitBonus())
        {
            //если можем открыть
            if (IsUnlockClick)
            {
                ClaimBonuses();
                IsUnlockClick = false;
                return;
            }

            //если можем разблокировать
            if (CanUnlock())
            {
                KeyReward -= bonusPrice;
                UnlockBtnTime = currentTime;
                IsUnlockClick = true;
                return;
            }

        }
    }

}
