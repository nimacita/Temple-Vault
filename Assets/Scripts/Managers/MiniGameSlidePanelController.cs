using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MiniGameSlidePanelController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    [Header("Editor")]
    [SerializeField]
    private Vector3 startMoveVecotor;
    [SerializeField]
    private Vector3 endMoveVecotor;
    public MiniGameManager gameManager;

    void Start()
    {
        startMoveVecotor = Vector3.zero;
        endMoveVecotor = Vector3.zero;
    }

    //отслеживаем нажатие 
    public void OnPointerDown(PointerEventData eventData)
    {
        startMoveVecotor = eventData.pointerCurrentRaycast.worldPosition;
    }

    //отслеживаем отпускания нажатия 
    public void OnPointerUp(PointerEventData eventData)
    {
        endMoveVecotor = eventData.pointerCurrentRaycast.worldPosition;
        gameManager.DefineSlideDirection(startMoveVecotor,endMoveVecotor);

        startMoveVecotor = Vector3.zero;
        endMoveVecotor = Vector3.zero;
    }

}
