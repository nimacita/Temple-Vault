using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{

    [Header("Game Settings")]
    [SerializeField]
    private float minPlatformXDist;
    [SerializeField]
    private float maxPlatformXDist;
    [SerializeField]
    [Tooltip("��� � ������� ������ ��������")]
    private float platformYStep;
    [Tooltip("������������ ���������� ����� �����")]
    public int maxYStep;
    [Tooltip("���� ��������� � ���������, ��� ������� �������� ��� ������ ����")]
    public int crystalPlatformChance;
    [Tooltip("���� ��������� ������, ��� ������� �������� ��� ������ ����")]
    public int farPlatformChance;

    [Header("Platform Settings")]
    public GameObject platformPref;
    private Vector3 currentPlatformPos;
    [SerializeField]
    private List<GameObject> platformsPool;
    private int currentMaxRecord;

    [Header("Editor Settings")]
    public GameObject startPlatform;

    void Start()
    {
        currentPlatformPos = startPlatform.transform.position;
        platformsPool = new List<GameObject>();
        currentMaxRecord = 1;
        StartSpawnPool();

        StartSpawn(8);
    }

    //������� ������ ���������� ��������� � ������
    private void StartSpawn(int spawnCount)
    {
        for (int i = 0; i < spawnCount; i++) 
        {
            SpawnPlatform();
        }
    }

    //������� ������� ��� ����
    private void StartSpawnPool()
    {
        for (int i = 0; i < 10; i++) 
        {
            GameObject poolPlatform = Instantiate(platformPref, currentPlatformPos, Quaternion.identity);
            poolPlatform.SetActive(false);
            platformsPool.Add(poolPlatform);
        }
    }

    //�������� ����� ��������� �������
    private GameObject WhichPlatformSpawn()
    {
        for (int i = 0; i < platformsPool.Count; i++) 
        {
            if (!platformsPool[i].activeSelf)
            {
                return platformsPool[i];
            }
        }
        GameObject currentPlatform = Instantiate(platformPref, currentPlatformPos, Quaternion.identity);
        platformsPool.Add(currentPlatform);
        return currentPlatform;
    }

    //������� �������� ���������
    public void SpawnPlatform(int playerRecord = 0)
    {
        Vector3 nextPlatformPos = new Vector3(currentPlatformPos.x + RandXPos(), currentPlatformPos.y + RandYPos(), currentPlatformPos.z);
        GameObject pref = WhichPlatformSpawn();
        pref.GetComponent<PlatformController>().SetPlatformRecord(currentMaxRecord);
        pref.GetComponent<PlatformController>().setIsMove(false);
        currentMaxRecord++;
        pref.transform.position = nextPlatformPos;
        pref.SetActive(true);
        IsGemSpawn(pref);
        currentPlatformPos = nextPlatformPos;

        CheckToDeletePlatform(playerRecord);
    }

    //������� �� ���
    private void IsGemSpawn(GameObject pref)
    {
        int r = Random.Range(0, crystalPlatformChance);
        if (r == 0)
        {
            pref.GetComponent<PlatformController>().GemActive(true);
        }
        else
        {
            pref.GetComponent<PlatformController>().GemActive(false);
        }
    }

    //��������� ��������� � ������� �� ������� ����������
    private void CheckToDeletePlatform(int playerRecord)
    {
        for (int i = 0; i < platformsPool.Count; i++) 
        {
            if (platformsPool[i].activeSelf && platformsPool[i].GetComponent<PlatformController>().GetPlatformRecord() < playerRecord - 3)
            {
                platformsPool[i].GetComponent<PlatformController>().DisablePlatform();
                SpawnPlatform();
            }
        }
    }
    
    //��������� �������� �� Y
    private float RandYPos()
    {
        //���� ��������� ��������� �� ������ ������
        int randMinusLvl = Random.Range(0, 10);
        int minYStep;
        if (randMinusLvl == 0)
        {
            minYStep = -1;
        }
        else if(randMinusLvl == 1)
        {
            minYStep = 0;
        }
        else
        {
            minYStep = 1;
        }
        float yPos = minYStep * platformYStep;
        return yPos;
    }

    //��������� �������� �� X
    private float RandXPos()
    {
        //���� ���������� �������� �� ������� �����������
        int randMaxX = Random.Range(0, farPlatformChance);
        float maxX = minPlatformXDist;
        if (randMaxX == 0)
        {
            maxX = maxPlatformXDist;
        }
        float xPos = Random.Range(minPlatformXDist, maxX);
        return xPos;
    }

}
