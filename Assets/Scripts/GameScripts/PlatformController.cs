using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{

    [Header("Platform Settings")]
    public GameObject gem;
    [SerializeField]
    private int platformRecord;
    [SerializeField]
    private float yOffset;
    [SerializeField]
    private float platformSpeed;
    private bool canMove;
    private Vector3 startPlatformPos;
    private Vector3 deadoffsetVector;
    public AudioSource moveSound;
    //private bool isActive;

    void Start()
    {
        canMove = false;
        //isActive = gameObject.activeSelf;
        PlatformPosSettings();
        GemActive(false);
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

    //��������� ��������� ������� ���������
    private void PlatformPosSettings()
    {
        startPlatformPos = transform.position;
        deadoffsetVector = new Vector3(0f, startPlatformPos.y - 7f, 0f);
    }

    private void Update()
    {
        Move();
        IsActive();
    }

    //�������
    private void Move()
    {
        if (canMove)
        {
            float step = platformSpeed * Time.deltaTime;
            Vector3 targetPos = new Vector3(transform.position.x, transform.position.y - yOffset, transform.position.z);
            if (Vector3.Distance(transform.position, targetPos) > 0.001f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
            }
            else
            {
                canMove = false;
            }
        }
    }

    //������� �� ���������
    private void IsActive()
    {
        if (transform.position.y <= deadoffsetVector.y)
        {
            DisablePlatform();
        }
    }

    //������� �� ��������� ����
    public void MoveDownPlatform()
    {
        canMove = true;
        if(Sound) moveSound.Play();
        PlatformPosSettings();
    }

    //���� ��������� ��
    public bool IsMove()
    {
        return canMove;
    }

    public void setIsMove(bool value)
    {
        canMove = value;
    }

    //��������� ���������
    public void DisablePlatform()
    {
        //isActive = false;
        gameObject.SetActive(false);
    }

    //���������� ����
    public void GemActive(bool valuse)
    {
        gem.SetActive(valuse);
    }

    //������������� ������
    public void SetPlatformRecord(int rec)
    {
        platformRecord = rec;
    }

    //���� ������
    public int GetPlatformRecord()
    {
        return platformRecord;
    }
    
}
