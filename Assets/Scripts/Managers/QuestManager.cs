using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{

    [Header("Button Settings")]
    public Button homeBtn;

    [Header("Editor")]
    public GameObject questPanel;
    public GameObject questHideLoc;
    public GameObject[] questLoc;
    public GameObject claimPanel;
    public Button claimPanelBackBtn;
    public TMPro.TMP_Text subTitleLocTxt;

    public MyMainMenu mainMenu;

    void Start()
    {
        //buttons
        homeBtn.onClick.AddListener(CloseQuestPanel);
        claimPanelBackBtn.onClick.AddListener(CloseRewardPanel);

        UpdateCurrentView();
        CloseRewardPanel();
    }

    //текущая локация
    private int CurrentLoc
    {
        get
        {
            if (PlayerPrefs.HasKey("currentLocation"))
            {
                return PlayerPrefs.GetInt("currentLocation");
            }
            else
            {
                return 0;
            }
        }
        set
        {
            PlayerPrefs.SetInt("currentLocation", value);
        }

    }

    //обновляем отображение панели квестов
    public void UpdateCurrentView()
    {
        for (int i = 0; i < questLoc.Length; i++)
        {
            if (i == CurrentLoc)
            {
                UpdateItemsPositions(questLoc[i]);
                questLoc[i].SetActive(true);
                subTitleLocTxt.text = $"Location {i + 1}";
            }
            else
            {
                questLoc[i].SetActive(false);
            }
        }
        if (CurrentLoc > questLoc.Length)
        {
            questLoc[0].SetActive(true);
        }
    }
    
    private void UpdateItemsPositions(GameObject loc)
    {
        for (int i = 0; i < loc.transform.childCount; i++) 
        {
            GameObject currChild = loc.transform.GetChild(i).gameObject;
            //если собрана то двигем вниз
            if (currChild.GetComponent<QuestItemController>().GetIsItemClaimed())
            {
                currChild.transform.SetParent(null);
                currChild.transform.SetParent(loc.transform);
            }
        }
    }

    //закрываем панель наград
    private void CloseRewardPanel()
    {
        claimPanel.SetActive(false);
        mainMenu.UpdateCoinTxt();
    }

    //закрываем панель достижений
    private void CloseQuestPanel()
    {
        questPanel.SetActive(false);
    }
}
