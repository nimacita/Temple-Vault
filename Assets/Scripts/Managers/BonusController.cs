using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusController : MonoBehaviour
{

    [Header("Bonus Settings")]
    [SerializeField]
    private float bonusDuration;
    [SerializeField]
    [Tooltip ("��� ������ �������� ��� ������� ����������")]
    [Range(1, 3)]
    private int bonusSlowSpeed; 

    [Space]
    [Header("Editor")]
    [SerializeField]
    private GameObject bonusBtn;
    [SerializeField]
    private GameObject bonusTimerPanel;
    [SerializeField]
    private TMPro.TMP_Text bonusTimerTxt;
    [SerializeField]
    private bool isActive;
    private float timer;
    private bool gameEnd;

    void Start()
    {
        isActive = false;
        gameEnd = false;
        timer = 0;

        //����������� ���������
        IsBonusActive = false;
        BonusSlowSpeed = 0;

        IsBonusBtn();
        bonusTimerPanel.SetActive(false);
    }

    //������������ �� ������
    private void IsBonusBtn()
    {
        if (BonusCount <=0 && !isActive)
        {
            bonusBtn.SetActive(false);
            bonusTimerPanel.SetActive(false);
        }
        else
        {
            bonusBtn.SetActive(true);
        }
    }


    //������� �� ������ ������
    public void BonusBtnClick()
    {
        isActive = true;
        timer = bonusDuration;
        BonusCount -= 1;
    }

    void Update()
    {
        if (isActive)
        {
            UpdateBonusTimer();
        }
        IsBonusBtn();
    }

    //��������� ������ ���� ������ ������
    private void UpdateBonusTimer()
    {
        if (timer > 0)
        {
            if (!gameEnd) {
                timer -= Time.deltaTime;
                IsBonusActive = true;
                bonusTimerTxt.text = $"00:{timer.ToString("00")}";
                bonusTimerPanel.SetActive(true);
                BonusSlowSpeed = bonusSlowSpeed;
            }
        }
        else
        {
            timer = 0;
            bonusTimerPanel.SetActive(false);
            IsBonusActive = false;
            BonusSlowSpeed = 0;

            isActive = false;
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

    //����������� �������� ���������� ������
    private bool IsBonusActive
    {
        get
        {
            if (PlayerPrefs.HasKey("IsBonusACtive"))
            {
                if (PlayerPrefs.GetInt("IsBonusACtive") == 0)
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
                PlayerPrefs.SetInt("IsBonusACtive", 0);
                return false;
            }
        }

        set
        {
            if(value)
            {
                PlayerPrefs.SetInt("IsBonusACtive", 1);
            }
            else
            {
                PlayerPrefs.SetInt("IsBonusACtive", 0);
            }
        }
    }

    //����������� �������� ���������� ������
    private int BonusSlowSpeed
    {
        get
        {
            if (PlayerPrefs.HasKey("BonusSlowSpeed"))
            {
                return PlayerPrefs.GetInt("BonusSlowSpeed");
            }
            else
            {
                PlayerPrefs.SetInt("BonusSlowSpeed", 0);
                return 0;
            }
        }
        set
        {
            PlayerPrefs.SetInt("BonusSlowSpeed", value);
        }
    }

    public void SetGameEnd(bool value)
    {
        gameEnd = value;
    }
}
