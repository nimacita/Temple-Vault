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

    //обновляем отображение в меню
    public void UpdateMenuView()
    {
        currentLocation = CurrentLoc;
        DefineCurrentLocation();
        UpdateCoinTxt();
    }

    //настройкка кнопок
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

    //настройки отображения экранов в начале
    private void StartViewSettings()
    {
        MenuView.SetActive(true);
        QuestsView.SetActive(false);
        SettingsView.SetActive(false);
        BonusView.SetActive(false);
        ShopView.SetActive(false);

        UpdateMenuView();
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
            if (value <= AllOpenlocationNumber)
            {
                PlayerPrefs.SetInt("currentLocation", value);
            }
        }

    }

    //открыта ли локация по индексам, локации от 0 до последней
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
            //если первая локация
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

    //устанавливаем локацию на открытую
    private void SetOpenLocation(int locInd)
    {
        PlayerPrefs.SetInt($"IsOpenLocation{locInd}", 1);
    }

    //определяем открытую локацию
    public void DefineCurrentLocation()
    {
        int curloc = currentLocation;

        //последняя локация
        if (curloc < AllOpenlocationNumber)
        {
            openLocatinView.SetActive(true);
            lockedLocationView.SetActive(false);
            startBtn.interactable = false;
            //определяем локацию
            for (int i = 0; i < locationsBgs.Length; i++)
            {
                if (i == curloc)
                {
                    openLocationIcon[i].SetActive(true);
                    //если локацич открыта
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
                        //цена за локацию
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

    //Проверяем активность стрелочек переключения локаций
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

    //переключени. на след локацию
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

    //переключение на прошлую локацию
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

    //обновляем коин текст
    public void UpdateCoinTxt()
    {
        coinLabel.text = "" + Coins;
        moneyLabel.text = "" + Money;
    }

    //включаем экран ежедневной награды
    private void QuestsViewOn()
    {
        questManager.UpdateCurrentView();
        QuestsView.SetActive(true);
    }

    //включаем экран настроек
    private void SettingsViewOn()
    {
        SettingsView.SetActive(true);
    }

    //запуск игровых сцен
    private void start()
    {
        //запуск игры если лоакция открыта
        if (IsOpenLocation(currentLocation)) 
        {
            StartCoroutine(openScene($"GameScene{CurrentLoc}"));
        }
        else
        {
            if (locationPrices[currentLocation] <= Money)
            {
                //покупаем локацию
                Money -= locationPrices[currentLocation];
                //покупаем
                SetOpenLocation(currentLocation);
                //обнволяем отоюражение
                DefineCurrentLocation();
                UpdateCoinTxt();
                CurrentLoc = currentLocation;
            }
        }
    }

    //включаем мини игру
    private void miniGame()
    {
        StartCoroutine(openScene("MiniGame"));
    }

    //включаем сцену бонусов
    private void bonusMenu()
    {
        bonusMenuManager.UpdateKeyCount();
        ViewOpen(BonusView);
    }

    //включаем сцену магазина
    private void shop()
    {
        shopManager.UpdateCoinTxt();
        ViewOpen(ShopView);
    }

    //открываем нужный экран
    private void ViewOpen(GameObject view)
    {
        view.SetActive(true);
        MenuView.SetActive(false);
    }

    //открываем сцену после задержки для перехода
    IEnumerator openScene(string sceneName,int addSec = 0)
    {
        yield return new WaitForSeconds(addSec);
        float fadeTime = mainCamera.GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(sceneName);
    }

}
