using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BonusMenuManager : MonoBehaviour
{

    [SerializeField]
    private GameObject bonusView;
    [SerializeField]
    private GameObject menuView;
    [SerializeField]
    private TMPro.TMP_Text keyText;
    [SerializeField]
    private GameObject bonusClaimPanel;
    public GameObject coinClaimPanel, bonusClaimImg;
    public TMPro.TMP_Text coinClaimTxt;
    public MyMainMenu myMainMenu;

    [Header("Button Settings")]
    public Button homeBtn;
    public Button closeClaimPanelBtn;


    void Start()
    {
        bonusClaimPanel.SetActive(false);
        ButtonSettings();
        UpdateKeyCount();
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

    //настройка нопок
    private void ButtonSettings()
    {
        homeBtn.onClick.AddListener(MainMenu);
        closeClaimPanelBtn.onClick.AddListener(CloseBonusClaimPanel);
    }

    private void Update()
    {
        UpdateKeyCount();
    }

    //открываем панель сбора бонуса и изменяем
    public void OpenCoinCLaimPanel(Sprite bonusImg, int coinBonus)
    {
        bonusClaimPanel.SetActive(true);
        coinClaimPanel.SetActive(true);
        bonusClaimImg.GetComponent<Image>().sprite = bonusImg;
        coinClaimTxt.text = $"x{coinBonus}";
    }

    public void UpdateKeyCount()
    {
        keyText.text = $"{KeyReward}";
    }

    public void CloseBonusClaimPanel()
    {
        bonusClaimPanel.SetActive(false );
        UpdateKeyCount();
    }

    //выход в меню
    public void MainMenu()
    {
        myMainMenu.UpdateCoinTxt();
        menuView.SetActive(true);
        bonusView.SetActive(false);
    }
}
