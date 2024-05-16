using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToothAreaController : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField]
    private float toothWallSpeed;
    private Vector3 toothWallMoveVector;
    private bool isMove;

    [Header("Editor")]
    public GameObject player;

    void Start()
    {
        isMove = false;
    }

    
    void Update()
    {
        Move();
    }

    public void Move()
    {
        if (isMove)
        {
            toothWallMoveVector = player.transform.position - transform.position;
            transform.Translate(toothWallMoveVector * toothWallSpeed * Time.deltaTime);
        }
    }

    //устанавливаем значение движения
    public void SetIsMove(bool value)
    {
        isMove = value;
    }
}
