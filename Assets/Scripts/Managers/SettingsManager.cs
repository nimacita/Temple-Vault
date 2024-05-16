using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{

    [Header("Buttons")]
    public GameObject musicBtn;
    public GameObject soundBtn;
    public Button backBtn;
    public Button privacyBtn;
    public Button rateBtn;

    [Header("Sprite Settings")]
    [SerializeField]
    private Sprite musicOn;
    [SerializeField]
    private Sprite musicOff;
    [SerializeField]
    private Sprite soundOn, soundOff;

    [Header("Links")]
    public string PrivacyLink;
    public string RateUsLink;

    //public StartupLoadingController loadingController;

    void Start()
    {
        ButtonSettings();
        UpdateBtnSprite();
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

    //��������� ������� ������
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

    //��������� ������
    private void ButtonSettings()
    {
        musicBtn.GetComponent<Button>().onClick.AddListener(MusicBtnClick);
        soundBtn.GetComponent<Button>().onClick.AddListener(SoundBtnClick);
        backBtn.onClick.AddListener(BackToMenu);
        privacyBtn.onClick.AddListener(PrivacyBtnClick);
        rateBtn.onClick.AddListener(RateBtnClick);
    }

    //������ ������
    public void MusicBtnClick()
    {
        if(Music)
        {
            Music = false;
        }
        else
        {
            Music = true;
        }
        UpdateBtnSprite();
    }

    //������ ������
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

    //������ ������
    public void RateBtnClick()
    {
        Application.OpenURL(RateUsLink);
    }

    //������ ������
    public void PrivacyBtnClick()
    {
        Application.OpenURL(PrivacyLink);
    }

    //����������� � ����
    public void BackToMenu()
    {
        gameObject.SetActive(false);
    }
}
