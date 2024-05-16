using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgMusicManager : MonoBehaviour
{

    [SerializeField]
    private AudioSource audioSource;

    private static BgMusicManager _instance;

    void Awake()
    {
        if (!_instance)
            _instance = this;
        else
            Destroy(this.gameObject);


        DontDestroyOnLoad(this.gameObject);
    }

    public void Play()
    {
        audioSource.Play();
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

    void Update()
    {
        if (!Music)
        {
            audioSource.mute = true;
        }
        else
        {
            audioSource.mute = false;
        }
    }
}
