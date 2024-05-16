using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameController : MonoBehaviour
{

    [Header("Game Panel")]
    public TMPro.TMP_Text scoreTxt;
    private int currScore;
    public Button pauseBtn;
    public TMPro.TMP_Text coinTxt;

    [Header("Pause")]
    public GameObject pausePanel;
    public Button continueBtn;
    public Button exitgameBtn;
    public GameObject musicBtn;
    public GameObject soundBtn;
    public TMPro.TMP_Text pauseScoreTxt;
    [SerializeField]
    private Sprite musicOn;
    [SerializeField]
    private Sprite musicOff;
    [SerializeField]
    private Sprite soundOn, soundOff;

    [Header("GameOver")]
    public GameObject gameOverPanel;
    public Button restartBtn;
    public Button goHomeBtn;
    public TMPro.TMP_Text gameOverScoreTxt, gameOverBestScoreTxt;
    public TMPro.TMP_Text keyCountTxt, coinsCountTxt, moneyCountTxt;
    public AudioSource winSound;

    [Header("Editor")]
    public GameObject mainCamera;
    public int curLocNumber;

    void Start()
    {
        currScore = 0;
        BtnSettings();
        GetScore(0);
        StartSettings();
        UpdateBtnSprite();
        Time.timeScale = 1f;
    }

    //текущий рекорд на локации
    private int CurrentScoreOnLoc
    {
        get
        {
            if (PlayerPrefs.HasKey($"CurrentScoreOnLoc{curLocNumber}"))
            {
                return PlayerPrefs.GetInt($"CurrentScoreOnLoc{curLocNumber}");
            }
            else
            {
                PlayerPrefs.SetInt($"CurrentScoreOnLoc{curLocNumber}", 0);
                return PlayerPrefs.GetInt($"CurrentScoreOnLoc{curLocNumber}");
            }
        }

        set
        {
            PlayerPrefs.SetInt($"CurrentScoreOnLoc{curLocNumber}", value);
        }
    }

    //настройка кнопок
    private void BtnSettings()
    {
        pauseBtn.onClick.AddListener(PauseOn);
        restartBtn.onClick.AddListener(Restart);
        continueBtn.onClick.AddListener(PauseOff);
        exitgameBtn.onClick.AddListener(GoHome);
        musicBtn.GetComponent<Button>().onClick.AddListener(MusicBtnClick);
        soundBtn.GetComponent<Button>().onClick.AddListener(SoundBtnClick);
        goHomeBtn.onClick.AddListener(GoHome);
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

    private bool Music
    {
        get
        {
            if (PlayerPrefs.HasKey("music"))
            {
                if (PlayerPrefs.GetInt("music") == 1)
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
                PlayerPrefs.SetInt("music", 1);
                return true;
            }
        }
        set
        {
            if (value)
            {
                PlayerPrefs.SetInt("music", 1);
            }
            else
            {
                PlayerPrefs.SetInt("music", 0);
            }
        }
    }

    private bool Sound
    {
        get
        {
            if (PlayerPrefs.HasKey("sound"))
            {
                if (PlayerPrefs.GetInt("sound") == 1)
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
                PlayerPrefs.SetInt("sound", 1);
                return true;
            }
        }
        set
        {
            if (value)
            {
                PlayerPrefs.SetInt("sound", 1);
            }
            else
            {
                PlayerPrefs.SetInt("sound", 0);
            }
        }
    }

    private void FixedUpdate()
    {
        coinTxt.text = $"{Coins}";
    }

    //начальные настройки
    private void StartSettings()
    {
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    //включаем панель конца игры
    public void GameOverMenuOn(int curCoins)
    {
        gameOverScoreTxt.text = $"Score: {currScore}";
        gameOverBestScoreTxt.text = $"Best Score: {CurrentScoreOnLoc}";

        int curKey = currScore / 10;
        keyCountTxt.text = $"x{curKey}";
        KeyReward += curKey;

        int curMoney = currScore * 5;
        moneyCountTxt.text = $"x{curMoney}";
        Money += curMoney;

        coinsCountTxt.text = $"x{curCoins}";

        if(Sound) winSound.Play();

        gameOverPanel.SetActive(true);
        pausePanel.SetActive(false);
}

    //включаем паузу
    public void PauseOn()
    {
        gameOverPanel.SetActive(false);
        pausePanel.SetActive(true);
        pauseScoreTxt.text = $"your current score: {currScore}";
        Time.timeScale = 0f;
    }

    //выключаем паузу
    public void PauseOff()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }

    //обновляем спрайыт кнопок
    private void UpdateBtnSprite()
    {
        if (Music)
        {
            musicBtn.GetComponent<Image>().sprite = musicOn;
        }
        else
        {
            musicBtn.GetComponent<Image>().sprite = musicOff;
        }

        if (Sound)
        {
            soundBtn.GetComponent<Image>().sprite = soundOn;
        }
        else
        {
            soundBtn.GetComponent<Image>().sprite = soundOff;
        }
    }

    //кнопка музыки
    public void MusicBtnClick()
    {
        if (Music)
        {
            Music = false;
        }
        else
        {
            Music = true;
        }
        UpdateBtnSprite();
    }

    //кнопка звуков
    public void SoundBtnClick()
    {
        if (Sound)
        {
            Sound = false;
        }
        else
        {
            Sound = true;
        }
        UpdateBtnSprite();
    }

    //рестарт
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //домой
    public void GoHome()
    {
        Time.timeScale = 1f;
        StartCoroutine(openScene("main menu"));
    }

    //получаем рекорд
    public void GetScore(int score)
    {
        currScore = score;
        scoreTxt.text = $"{currScore}";
        if (currScore > CurrentScoreOnLoc)
        {
            CurrentScoreOnLoc = currScore;
        }
    }

    //открываем сцену после задержки для перехода
    IEnumerator openScene(string sceneName)
    {
        float fadeTime = mainCamera.GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(sceneName);
    }
}
