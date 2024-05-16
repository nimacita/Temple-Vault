using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameBtnController : MonoBehaviour
{

    [SerializeField]
    private int currI, currJ;
    [SerializeField]
    private bool isFree;
    [SerializeField]
    private int value;

    [Header("Editor")]
    [SerializeField]
    private GameObject btnImg;

    void Start()
    {
        btnImg.SetActive(false);  
    }
    
    //������������� ���������� ������
    public void SetCurrentCord(int i, int j)
    {
        currI = i;
        currJ = j;
    }

    //������������� �������� ����������� ������
    public void SetIsFree(bool isf)
    {
        isFree = isf;
    }

    public void setValue(int val)
    {
        value = val;
    }
}
