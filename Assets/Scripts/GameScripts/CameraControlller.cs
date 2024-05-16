using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlller : MonoBehaviour
{

    [Header("Settings")]
    public float cameraSpeed;
    private Vector3 cameraOffset;
    private Vector3 currentCameraPos;
    [SerializeField]
    private bool canMove;
    [SerializeField]
    private GameObject currentColumn;
    private Transform currentColumnTr;
    

    void Start()
    {
        currentCameraPos = transform.position;
        canMove = false;

        cameraOffset = new Vector3(1.65f, 2.5f, 0f);
    }

    
    void Update()
    {
        CameraMove();
    }

    //двигаем камеру к игроку
    private void CameraMove()
    {
        if (canMove && !currentColumn.GetComponent<PlatformController>().IsMove())
        {
            float step = cameraSpeed * Time.deltaTime;
            Vector3 targetPos = new Vector3(currentColumnTr.position.x + cameraOffset.x, currentColumnTr.position.y + cameraOffset.y, currentCameraPos.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
        }
    }

    //можем ли двигать
    public void MoveTheCameraToClolumn(GameObject column)
    {
        currentColumn = column;
        currentColumnTr = column.transform;
        canMove = true;
    }

}
