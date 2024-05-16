using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IsFirstGameManager : MonoBehaviour
{

    [SerializeField]
    private int pageCount;
    [SerializeField]
    private int currPage;
    [SerializeField]
    private TMPro.TMP_Text pageTxt;
    [SerializeField]
    [Space(5), Tooltip("���������� ��������� ������ ��������������� ���������� �������")]
    private string[] pageTips;
    [SerializeField]
    private GameObject[] pageImgs;
    [SerializeField]
    private TMPro.TMP_Text pageNumberTxt;
    [SerializeField]
    private GameObject firstGameView;
    [SerializeField]
    private Button nextBtn;
    public GameObject titleTxt, subTitleTxt;

    void Start()
    {
        currPage = 0;
        nextBtn.onClick.AddListener(NextBtnClick);
        UpdateIsFirstGame();
    }

    //����������� �������� ����� ������ ����
    private bool isFirstgame
    {
        get
        {
            if (PlayerPrefs.HasKey("isFirstGame"))
            {
                if (PlayerPrefs.GetInt("isFirstGame") == 1)
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
                PlayerPrefs.SetInt("isFirstGame", 1);
                return true;
            }
        }

        set
        {
            if (value)
            {
                PlayerPrefs.SetInt("isFirstGame", 1);
            }
            else
            {
                PlayerPrefs.SetInt("isFirstGame", 0);
            }
        }
    }

    //������� ���� ������ ��� ����� ��������� ������
    private void UpdateIsFirstGame()
    {
        if (isFirstgame)
        {
            //�������� ����� ������ ����
            firstGameView.SetActive(true);
            pageTxt.text = pageTips[currPage];
            pageNumberTxt.text = $"{currPage + 1}/{pageCount}";
            //�������� ������ �����������
            for (int i = 0; i < pageImgs.Length; i++)
            {
                if (i == currPage)
                {
                    pageImgs[i].SetActive(true);
                }
                else
                {
                    pageImgs[i].SetActive(false);
                }
            }
            if (currPage == 0)
            {
                titleTxt.SetActive(true);
                subTitleTxt.SetActive(true);
            }
            else
            {
                titleTxt.SetActive(false);
                subTitleTxt.SetActive(false);
            }
        }
        else
        {
            //��������� ����� ������ ����
            firstGameView.SetActive(false);
        }

    }

    //������� �� ���� ������
    public void NextBtnClick()
    {
        if (currPage < pageCount - 1)
        {
            //����������� ��������
            currPage++;
        }
        else
        {
            //��������� ���
            isFirstgame = false;
        }
        UpdateIsFirstGame();
    }

}
