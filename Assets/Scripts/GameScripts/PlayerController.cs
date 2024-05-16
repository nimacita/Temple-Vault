using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Player Settings")]
    private float leftJumpForce;
    [SerializeField]
    private float maxRightJumpForce = 1.5f;
    [SerializeField]
    private Vector2 jumpRightDirectionVector = new Vector2(120f, 320f);
    [SerializeField]
    private Vector2 jumpLeftDirectionVector = new Vector2(90f, 280f);
    [SerializeField]
    private bool isOnColumn;
    private float deadYOffset;

    private float pressBtnTime;
    private float maxPressBtnTime;
    private bool btnDown;
    private Rigidbody2D rb;
    private float raycastDistance;

    private bool isRotate;
    private Quaternion playerSkinTargetRot;
    [SerializeField]
    private float rotateSpeed;

    private int currentRecord;
    private int minCameraMoveRecord;

    [Header("Skins Settings")]
    public Sprite[] skins;

    [Header("Sound Settings")]
    public AudioSource jumpSound;
    public AudioSource pullingSound;
    public AudioSource stepOnPlateSound;
    public AudioSource collectGemSound;

    [Header("Editor Settings")]
    public GameObject playerSkinObj;
    public GameObject mainCamera;
    private CameraControlller cameraControlller;
    public PlatformSpawner platformSpawner;
    private GameObject currentPlatform;
    public TrajectoryController trajectoryController;
    public ToothAreaController toothAreaController;
    public GameController gameController;
    private int curCoins;


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

    //текущий скин персонажа
    private int CurrentCharacterSkinInd
    {
        get
        {
            if (PlayerPrefs.HasKey($"CurrentCharacterInd"))
            {
                return PlayerPrefs.GetInt($"CurrentCharacterInd");
            }
            else
            {
                PlayerPrefs.SetInt($"CurrentCharacterInd", 0);
                return PlayerPrefs.GetInt($"CurrentCharacterInd");
            }
        }

        set
        {
            PlayerPrefs.SetInt($"CurrentCharacterInd", value);
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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        trajectoryController.HideTrajectory();
        isOnColumn = true;
        cameraControlller = mainCamera.GetComponent<CameraControlller>();
        deadYOffset = 7f;

        DefineCurrentSkin();

        //Player Start Settings
        //jumpLeftDirectionVector = new Vector2(90f, 280f);
        leftJumpForce = 1f;

        //jumpRightDirectionVector = new Vector2(120f, 320f);
        //maxRightJumpForce = 1.5f;

        raycastDistance = 0.5f;

        //score
        currentRecord = 0;
        minCameraMoveRecord = 1;
        curCoins = 0;

        //Player Skin Rotate
        isRotate = false;

        //Btn Start Settings
        maxPressBtnTime = 2f;
        btnDown = false;
    }

    //определяем текущий скин персонажа
    private void DefineCurrentSkin()
    {
        playerSkinObj.GetComponent<SpriteRenderer>().sprite = skins[CurrentCharacterSkinInd];
    }
    
    void Update()
    {
        PlusPressedTime();
        IsGameOver();
        RotatePlayer();
        RayHit();
    }

    //конец ли игры
    private void IsGameOver()
    {
        if (transform.position.y < mainCamera.transform.position.y - deadYOffset)
        {
            GameOver();
        }
    }

    //конец игры
    private void GameOver()
    {
        gameObject.SetActive(false);
        toothAreaController.SetIsMove(false);
        gameController.GameOverMenuOn(curCoins);
    }

    //прыжок игрока
    private void Jump(float jumpForce, Vector2 jumpVector)
    {
        if (isOnColumn) 
        {
            PlayCurrentSound(jumpSound);
            rb.AddForce(jumpVector * jumpForce);
            GoRotate();
        }
    }

    //запускаем поворот
    private void GoRotate()
    {
        Vector3 rot = playerSkinObj.transform.rotation.eulerAngles;
        rot = new Vector3(rot.x, rot.y, rot.z - 90f);
        playerSkinTargetRot = Quaternion.Euler(rot);
        isRotate = true;
    }

    //перевернуть скин игрока
    private void RotatePlayer()
    {
        if (isRotate)
        {
            if (playerSkinObj.transform.rotation.eulerAngles.z != playerSkinTargetRot.eulerAngles.z)
            {
                playerSkinObj.transform.rotation = Quaternion.RotateTowards(playerSkinObj.transform.rotation, playerSkinTargetRot, rotateSpeed * Time.deltaTime);
            }
            else
            {
                isRotate = false;
            }
        }
    }

    //нажатие левой кнопки
    public void LeftBtnDown()
    {
        //рисуем траекторию
        if (isOnColumn) 
        {
            trajectoryController.ShowTrajectory(transform.position, jumpLeftDirectionVector * leftJumpForce);
        }
    }

    //отжатие левой кнопки
    public void LeftBtnUp()
    {
        Jump(leftJumpForce, jumpLeftDirectionVector);
        trajectoryController.HideTrajectory();
    }

    //зажатие правой кнопки
    public void RightBtnDown()
    {
        btnDown = true;
        pressBtnTime = 1f;
        PlayCurrentSound(pullingSound);
    }

    //отпускание кнопки прыжка
    public void RightBtnUp()
    {
        btnDown = false;
        Jump(CalculatedJumpForce(), jumpRightDirectionVector);
        trajectoryController.HideTrajectory();
        pressBtnTime = 1f;
    }

    //считаем силу прыжка
    private float CalculatedJumpForce()
    {
        float jf = 0f;
        jf = pressBtnTime * (maxRightJumpForce / maxPressBtnTime);
        if (jf > maxRightJumpForce)
        {
            jf = maxRightJumpForce;
        }
        return jf;
    }

    //накопление силы прыжка 
    private void PlusPressedTime()
    {
        if (btnDown && isOnColumn)
        {
            if (pressBtnTime < maxPressBtnTime)
            {
                pressBtnTime += Time.deltaTime;
            }
            else
            {
                pressBtnTime = 1f;
            }
            //рисуем траекторию
            trajectoryController.ShowTrajectory(transform.position, jumpRightDirectionVector * CalculatedJumpForce());
        }
    }

    //собрали гем
    public void GemCollected()
    {
        //прописываем прибавление валюты
        Coins++;
        curCoins++;
        PlayCurrentSound(collectGemSound);
    }

    //играме выбранный звук
    private void PlayCurrentSound(AudioSource currSound)
    {
        if (Sound)
        {
            currSound.Play(); 
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Column")
        {
            PlayCurrentSound(stepOnPlateSound);
            if (currentPlatform!=null && currentPlatform != collision.gameObject)
            {
                currentPlatform.GetComponent<PlatformController>().MoveDownPlatform();
            }
            currentPlatform = collision.gameObject;
            currentRecord = currentPlatform.GetComponent<PlatformController>().GetPlatformRecord();
            gameController.GetScore(currentRecord);
            //двигаем камеру к калоне
            if (currentRecord >= minCameraMoveRecord)
            {
                cameraControlller.MoveTheCameraToClolumn(currentPlatform);
                //запускаем движение зубов
                toothAreaController.SetIsMove(true);
            }
            //спавним новую платфомру
            platformSpawner.SpawnPlatform(currentRecord);
        }
    }

    //для првоекри на земле ли
    private void RayHit()
    {
        int layerMask = LayerMask.GetMask("Ground");
        var rend = GetComponent<BoxCollider2D>();
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, Vector2.down, raycastDistance, layerMask);
        RaycastHit2D rightHit = Physics2D.Raycast(new Vector2(rend.bounds.max.x, rend.bounds.center.y), Vector2.down, raycastDistance, layerMask);
        RaycastHit2D leftHit = Physics2D.Raycast(new Vector2(rend.bounds.min.x, rend.bounds.center.y), Vector2.down, raycastDistance, layerMask);
        if (hit.collider != null || rightHit.collider != null || leftHit.collider != null)
        {
            isOnColumn = true;
        }
        else
        {
            isOnColumn = false;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "TothArea")
        {
            GameOver();
        }
        if (collision.gameObject.tag == "Gem")
        {
            GemCollected();
            collision.gameObject.SetActive(false);
        }
    }



}
