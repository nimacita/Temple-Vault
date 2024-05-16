using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MyMainMenu : MonoBehaviour
{

    [Space]
    [Header("Main settings")]
    [SerializeField]
    TMPro.TMP_Text coinLabel;
    [SerializeField]
    TMPro.TMP_Text moneyLabel;
    [SerializeField]
    private GameObject MenuView;
    [SerializeField]
    private GameObject QuestsView;
    [SerializeField]
    private GameObject SettingsView;
    [SerializeField]
    private GameObject BonusView;
    [SerializeField]
    private GameObject ShopView;

    [Space]
    [Header("Buttons Settings")]
    public Button startBtn;
    public Button shopBtn;
    public Button rewardBtn;
    public Button settingsBtn;
    public Button bonusBtn;
    public Button miniGameBtn;
    public Button addCoinsBtn, addMoneyBtn;
    public Button nextArrowBtn, pastArrowBtn;


    [Space]
    [Header("Location Settings")]
    [SerializeField]
    private int AllOpenlocationNumber;
    [SerializeField]
    private int currentLocation;
    [SerializeField]
    private GameObject nextLocationArrow;
    [SerializeField]
    private GameObject pastLocationArrow;
    [SerializeField]
    private GameObject bgPanels;
    [SerializeField]
    private GameObject[] locationsBgs;
    [SerializeField]
    private GameObject[] openLocationIcon;
    [SerializeField]
    private GameObject[] lockedLocationIcon;
    [SerializeField]
    private GameObject[] locationPricesObj;
    [SerializeField]
    private int[] locationPrices;
    [SerializeField]
    private GameObject openLocatinView, lockedLocationView;
    [SerializeField]
    private TMPro.TMP_Text lockedLocationTxt;

    [Space]
    [SerializeField]
    private GameObject mainCamera;
    [SerializeField]
    private ShopManager shopManager;
    [SerializeField]
    private BonusMenuManager bonusMenuManager;
    [SerializeField]
    private QuestManager questManager;



    void Start()
    {
        currentLocation = CurrentLoc;
        ButtonSettings();
        DefineCurrentLocation();
        UpdateCoinTxt();
        StartViewSettings();
    }

    //��������� ����������� � ����
    public void UpdateMenuView()
    {
        currentLocation = CurrentLoc;
        DefineCurrentLocation();
        UpdateCoinTxt();
    }

    //���������� ������
    private void ButtonSettings()
    {
        startBtn.onClick.AddListener(start);
        shopBtn.onClick.AddListener(shop);
        rewardBtn.onClick.AddListener(QuestsViewOn);
        settingsBtn.onClick.AddListener(SettingsViewOn);
        bonusBtn.onClick.AddListener(bonusMenu);
        miniGameBtn.onClick.AddListener(miniGame);
        addCoinsBtn.onClick.AddListener(shop);
        addMoneyBtn.onClick.AddListener(shop);
        nextArrowBtn.onClick.AddListener(NextLocation);
        pastArrowBtn.onClick.AddListener(PastLocation);
    }

    //��������� ����������� ������� � ������
    private void StartViewSettings()
    {
        MenuView.SetActive(true);
        QuestsView.SetActive(false);
        SettingsView.SetActive(false);
        BonusView.SetActive(false);
        ShopView.SetActive(false);

        UpdateMenuView();
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

    //������� �������
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
            if (value <= AllOpenlocationNumber)
            {
                PlayerPrefs.SetInt("currentLocation", value);
            }
        }

    }

    //������� �� ������� �� ��������, ������� �� 0 �� ���������
    private bool IsOpenLocation(int locInd)
    {
        if (PlayerPrefs.HasKey($"IsOpenLocation{locInd}"))
        {
            if (PlayerPrefs.GetInt($"IsOpenLocation{locInd}") == 1)
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
            //���� ������ �������
            if (locInd == 0)
            {
                PlayerPrefs.SetInt($"IsOpenLocation{locInd}", 1);
                return true;
            }
            else
            {
                PlayerPrefs.SetInt($"IsOpenLocation{locInd}", 0);
                return false;
            }
        }
    }

    //������������� ������� �� ��������
    private void SetOpenLocation(int locInd)
    {
        PlayerPrefs.SetInt($"IsOpenLocation{locInd}", 1);
    }

    //���������� �������� �������
    public void DefineCurrentLocation()
    {
        int curloc = currentLocation;

        //��������� �������
        if (curloc < AllOpenlocationNumber)
        {
            openLocatinView.SetActive(true);
            lockedLocationView.SetActive(false);
            startBtn.interactable = false;
            //���������� �������
            for (int i = 0; i < locationsBgs.Length; i++)
            {
                if (i == curloc)
                {
                    openLocationIcon[i].SetActive(true);
                    //���� ������� �������
                    if (IsOpenLocation(i))
                    {
                        startBtn.interactable = true;
                        lockedLocationIcon[i].SetActive(false);
                        locationPricesObj[i].SetActive(false);
                        locationsBgs[i].SetActive(true);
                        for (int j = 0; j < locationsBgs.Length; j++)
                        {
                            if (j != i)
                            {
                                locationsBgs[j].SetActive(false);
                            }
                        }
                    }
                    else
                    {
                        lockedLocationIcon[i].SetActive(true);
                        //���� �� �������
                        locationPricesObj[i].GetComponent<TMPro.TMP_Text>().text = $"{Money}/{locationPrices[i]}";
                        locationPricesObj[i].SetActive(true);

                        if (locationPrices[i] > Money)
                        {
                            startBtn.interactable = false;
                        }
                        else
                        {
                            startBtn.interactable = true;
                        }
                    }
                }
                else
                {
                    openLocationIcon[i].SetActive(false);
                }
            }
        }
        else
        {
            openLocatinView.SetActive(false);
            lockedLocationView.SetActive(true);
            startBtn.interactable = false;
        }

        DefineLocationArrow();
    }

    //��������� ���������� ��������� ������������ �������
    private void DefineLocationArrow()
    {
        int curloc = currentLocation;

        if (curloc <= 0)
        {
            pastLocationArrow.SetActive(false);
        }
        else
        {
            pastLocationArrow.SetActive(true);
        }

        if (curloc >= AllOpenlocationNumber)
        {
            nextLocationArrow.SetActive(false);
        }
        else
        {
            nextLocationArrow.SetActive(true);
        }
    }

    //�����������. �� ���� �������
    private void NextLocation()
    {
        currentLocation += 1;
        if (IsOpenLocation(currentLocation))
        {
            CurrentLoc = currentLocation;
        }

        //go to change location
        DefineCurrentLocation();
    }

    //������������ �� ������� �������
    private void PastLocation()
    {
        currentLocation -= 1;
        if (IsOpenLocation(currentLocation))
        {
            CurrentLoc = currentLocation;
        }

        //go to change location
        DefineCurrentLocation();
    }

    //��������� ���� �����
    public void UpdateCoinTxt()
    {
        coinLabel.text = "" + Coins;
        moneyLabel.text = "" + Money;
    }

    //�������� ����� ���������� �������
    private void QuestsViewOn()
    {
        questManager.UpdateCurrentView();
        QuestsView.SetActive(true);
    }

    //�������� ����� ��������
    private void SettingsViewOn()
    {
        SettingsView.SetActive(true);
    }

    //������ ������� ����
    private void start()
    {
        //������ ���� ���� ������� �������
        if (IsOpenLocation(currentLocation)) 
        {
            StartCoroutine(openScene($"GameScene{CurrentLoc}"));
        }
        else
        {
            if (locationPrices[currentLocation] <= Money)
            {
                //�������� �������
                Money -= locationPrices[currentLocation];
                //��������
                SetOpenLocation(currentLocation);
                //��������� �����������
                DefineCurrentLocation();
                UpdateCoinTxt();
                CurrentLoc = currentLocation;
            }
        }
    }

    //�������� ���� ����
    private void miniGame()
    {
        StartCoroutine(openScene("MiniGame"));
    }

    //�������� ����� �������
    private void bonusMenu()
    {
        bonusMenuManager.UpdateKeyCount();
        ViewOpen(BonusView);
    }

    //�������� ����� ��������
    private void shop()
    {
        shopManager.UpdateCoinTxt();
        ViewOpen(ShopView);
    }

    //��������� ������ �����
    private void ViewOpen(GameObject view)
    {
        view.SetActive(true);
        MenuView.SetActive(false);
    }

    //��������� ����� ����� �������� ��� ��������
    IEnumerator openScene(string sceneName,int addSec = 0)
    {
        yield return new WaitForSeconds(addSec);
        float fadeTime = mainCamera.GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(sceneName);
    }

}
