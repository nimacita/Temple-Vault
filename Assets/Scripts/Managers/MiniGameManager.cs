using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MiniGameManager : MonoBehaviour
{

    [Header("Setings")]
    [SerializeField]
    private int btnCount;
    [SerializeField]
    private int maxMoveCount;
    private int currentMoveCount;
    [SerializeField]
    private int startSpawnItems;
    [SerializeField]
    private Sprite[] btnSpriteToValue;
    private int maxBtnValue;

    private int currentCoins;

    [Header("Sound Settings")]
    public AudioSource winSound;
    public AudioSource popSound;
    public AudioSource moveSound;

    [Space]
    [Header("Editor")]
    [SerializeField]
    private GameObject mainCamera;
    [SerializeField]
    private GameObject gameField;
    [SerializeField]
    private TMPro.TMP_Text moveCountTxt;
    [SerializeField]
    private GameObject rewardView;
    [SerializeField]
    private TMPro.TMP_Text rewardCoinsValueTxt;
    [SerializeField]
    private GameObject comeBackView;
    [SerializeField]
    private TMPro.TMP_Text comeBackTimerTxt;
    public Button comeBackHomeBtn;
    public Button homeBtn;
    public Button rewardHomeBtn;
    private bool isCanMove;


    [Header("Debug")]
    [SerializeField]
    int length;
    private bool canPlay;
    private DateTime currentTime;
    private bool isTimer;
    [SerializeField]
    private int addDays;


    private GameObject[,] buttons;
    private GameObject[,] buttonsImgs;
    private int[,] buttonsValues;
    private Animation[,] buttonsAnim;
    private bool[,] isFree;

    void Start()
    {
        InitializationBtns();

        homeBtn.onClick.AddListener(Home);
        rewardHomeBtn.onClick.AddListener(Home);
        comeBackHomeBtn.onClick.AddListener(Home);

        isCanMove = true;

        canPlay = true;
        currentMoveCount = 0;
        moveCountTxt.text = $"{currentMoveCount}/{maxMoveCount} move";

        maxBtnValue = 256;

        StartCoroutine(StartSpawn());
    }

    //начальный спавн
    private IEnumerator StartSpawn()
    {
        yield return new WaitForSeconds(0.20f);
        for (int i = 0; i < startSpawnItems; i++)
        {
            RandomItemSpawn();
        }
    }

    //можем ли дальше играть
    private void IsWin()
    {
        //провряем выйграли ли
        if (currentMoveCount >= maxMoveCount || !IsCanMove())
        {
            canPlay = false;
            //если выйграли
            if (!isTimer)
            {
                currentCoins = CalculateCoins();
                rewardView.SetActive(true);
                rewardCoinsValueTxt.text = $"X{currentCoins}";
                PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + currentCoins);
                LastGame = currentTime;
                PlayCurrSoun(winSound);
            }
        }
    }

    private void Update()
    {
        currentTime = DateTime.Now.AddDays(addDays);
        IsStartComeBackView();
    }

    //последняя игра
    public DateTime LastGame
    {
        get
        {
            DateTime dateTime = new DateTime();
            if (!PlayerPrefs.HasKey("LastGame"))
            {
                return dateTime;
            }
            else
            {
                return DateTime.Parse(PlayerPrefs.GetString("LastGame"));
            }
        }
        set
        {
            PlayerPrefs.SetString("LastGame", value.ToString());
        }
    }

    //запускаем ли экран ожидания или игру
    private void IsStartComeBackView()
    {
        if (currentTime.Subtract(LastGame).Days >= 1)
        {
            canPlay = true;
            isTimer = false;
            comeBackView.SetActive(false);
        }
        else
        {
            if (canPlay)
            {
                isTimer = true;
                comeBackView.SetActive(true);
            }
        }
        UpdateTimeToNextrewardTxt();
    }

    //обнволяем таймер ожидания
    private void UpdateTimeToNextrewardTxt()
    {
        TimeSpan sub = new TimeSpan(24, 0, 0).Subtract(currentTime.Subtract(LastGame));

        string txt = $"{sub.Hours:D2}:{sub.Minutes:D2}:{sub.Seconds:D2}";
        comeBackTimerTxt.text = $"{txt}";
    }

    //обновляем количество ходов
    private void UpdateCurrentMoveCount()
    {
        currentMoveCount++;
        moveCountTxt.text = $"{currentMoveCount}/{maxMoveCount} move";
        //каждый шаг проверяем можем ли дальше играть
        IsWin();
    }

    //определяем все кнопки
    private void InitializationBtns()
    {
        length = (int)Mathf.Sqrt(btnCount);
        buttons = new GameObject[length, length];
        buttonsAnim = new Animation[length, length];
        buttonsImgs = new GameObject[length, length];
        buttonsValues = new int[length, length];
        isFree = new bool[length, length];

        int btnIndex = 0;
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < length; j++)
            {
                buttons[i, j] = gameField.transform.GetChild(btnIndex).gameObject;
                buttons[i, j].GetComponent<MiniGameBtnController>().SetCurrentCord(i, j);
                buttonsAnim[i, j] = buttons[i, j].GetComponent<Animation>();
                buttonsImgs[i, j] = buttons[i, j].transform.GetChild(0).gameObject;
                buttonsImgs[i, j].GetComponent<Image>().sprite = null;
                buttonsValues[i, j] = 0;

                isFree[i, j] = true;
                buttons[i, j].GetComponent<MiniGameBtnController>().SetIsFree(isFree[i, j]);
                btnIndex++;
            }
        }
    }

    //спавним предмет
    private void RandomItemSpawn()
    {
        int randi = UnityEngine.Random.Range(0, length);
        int randj = UnityEngine.Random.Range(0, length);

        //если свободен спавним предмет
        if (isFree[randi, randj])
        {
            //спавним предмет
            buttonsValues[randi, randj] = RandBtnValue();
            buttonsImgs[randi, randj].GetComponent<Image>().sprite = CurrentSpriteToValue(buttonsValues[randi, randj]);
            buttonsImgs[randi, randj].SetActive(true);
            StartCoroutine(SpawnActiveItemWithAnim(buttonsImgs[randi, randj], buttonsAnim[randi, randj], randi, randj));
            isFree[randi, randj] = false;

            //устанавливаем значения в скрипт предмеьа
            SetItemsScriptsValues(randi, randj);
        }
        else if (isAllFree())
        {
            RandomItemSpawn();
        }
    }

    //ходим на лево (асуждаю)
    private void StepToLeft()
    {
        //проверяем все items
        for (int i = 0; i < length; i++)
        {
            for (int j = 1; j < length; j++)
            {
                //если не свободна
                if (!isFree[i,j]) 
                {
                    int newI = -1, newJ = -1;
                    int newValue = 0;
                    bool isNewValue = false;
                    //движение влево по j
                    for (int jInd = j - 1; jInd >= 0; jInd--)
                    {
                        if (isFree[i, jInd])
                        {
                            //если свободно запоминаем координаты чтоб перепрыгнуть
                            newI = i;
                            newJ = jInd;
                            newValue = buttonsValues[i, j];
                        }
                        else
                        {
                            //если значение такое же как и у изначальной кнопки, то запоминаем и меняем
                            if (buttonsValues[i, jInd] == buttonsValues[i, j] && buttonsValues[i, j] != 0 && buttonsValues[i, j] != maxBtnValue)
                            {
                                newI = i;
                                newJ = jInd;
                                newValue = buttonsValues[i, jInd] + buttonsValues[i, j];
                                isNewValue = true;
                            }
                            break;
                        }
                    }
                    //перпрыгиваем на новый индекс если нашли
                    if (newI != -1 && newJ != -1)
                    {
                        GoToNewPosition(i, j, newI, newJ, newValue, "left", isNewValue);
                        StepToLeft();
                    }
                }
            }
        }
    }

    //ходим на право
    private void StepToRight()
    {
        //проверяем все items
        for (int i = 0; i < length; i++)
        {
            for (int j = length - 2; j >= 0; j--)
            {
                //если не свободна
                if (!isFree[i, j])
                {
                    int newI = -1, newJ = -1;
                    int newValue = 0;
                    bool isNewValue = false;
                    //движение влево по j
                    for (int jInd = j + 1; jInd < length; jInd++)
                    {
                        if (isFree[i, jInd])
                        {
                            //если свободно запоминаем координаты чтоб перепрыгнуть
                            newI = i;
                            newJ = jInd;
                            newValue = buttonsValues[i, j];
                        }
                        else
                        {
                            //если значение такое же как и у изначальной кнопки, то запоминаем и меняем
                            if (buttonsValues[i, jInd] == buttonsValues[i, j] && buttonsValues[i, j] != 0 && buttonsValues[i, j] != maxBtnValue)
                            {
                                newI = i;
                                newJ = jInd;
                                newValue = buttonsValues[i, jInd] + buttonsValues[i, j];
                                isNewValue = true;
                            }
                            break;
                        }
                    }
                    //перпрыгиваем на новый индекс если нашли
                    if (newI != -1 && newJ != -1) 
                    {
                        GoToNewPosition(i, j, newI, newJ, newValue, "right", isNewValue);
                        StepToRight();
                    }
                }
            }
        }
    }

    //ходим вверх
    private void StepToUp()
    {
        //проверяем все items
        for (int i = 1; i < length; i++)
        {
            for (int j = 0; j < length; j++)
            {
                //если не свободна
                if (!isFree[i, j])
                {
                    int newI = -1, newJ = -1;
                    int newValue = 0;
                    bool isNewValaue = false;
                    //движение влево по j
                    for (int iInd = i - 1; iInd >= 0; iInd--)
                    {
                        if (isFree[iInd, j])
                        {
                            //если свободно запоминаем координаты чтоб перепрыгнуть
                            newI = iInd;
                            newJ = j;
                            newValue = buttonsValues[i, j];
                        }
                        else
                        {
                            //если значение такое же как и у изначальной кнопки, то запоминаем и меняем
                            if (buttonsValues[iInd, j] == buttonsValues[i, j] && buttonsValues[i, j] != 0 && buttonsValues[i, j] != maxBtnValue)
                            {
                                newI = iInd;
                                newJ = j;
                                newValue = buttonsValues[iInd, j] + buttonsValues[i, j];
                                isNewValaue = true;
                            }
                            break;
                        }
                    }
                    //перпрыгиваем на новый индекс если нашли
                    if (newI != -1 && newJ != -1)
                    {
                        GoToNewPosition(i, j, newI, newJ, newValue, "up", isNewValaue);
                        StepToUp();
                    }
                }
            }
        }
    }

    //ходим вверх
    private void StepToDown()
    {
        //проверяем все items
        for (int i = length - 2; i >=0; i--)
        {
            for (int j = 0; j < length; j++)
            {
                //если не свободна
                if (!isFree[i, j])
                {
                    bool isNewValue = false;
                    int newI = -1, newJ = -1;
                    int newValue = 0;
                    //движение вниз по i
                    for (int iInd = i + 1; iInd < length; iInd++)
                    {
                        if (isFree[iInd, j])
                        {
                            //если свободно запоминаем координаты чтоб перепрыгнуть
                            newI = iInd;
                            newJ = j;
                            newValue = buttonsValues[i, j];
                        }
                        else
                        {
                            //если значение такое же как и у изначальной кнопки, то запоминаем и меняем
                            if (buttonsValues[iInd, j] == buttonsValues[i, j] && buttonsValues[i, j] != 0 && buttonsValues[i, j] != maxBtnValue)
                            {
                                newI = iInd;
                                newJ = j;
                                newValue = buttonsValues[iInd, j] + buttonsValues[i, j];
                                isNewValue = true;
                            }
                            break;
                        }
                    }
                    //перпрыгиваем на новый индекс если нашли
                    if (newI != -1 && newJ != -1)
                    {
                        GoToNewPosition(i, j, newI, newJ, newValue, "down", isNewValue);
                        StepToDown();
                    }
                }
            }
        }
    }

    //перепрыгиваем на новый индекс и если нужно меняем значение и спрайт
    private void GoToNewPosition(int i, int j, int newI, int newJ, int curValue, string direction, bool isNewValue)
    {
        //удаляем прошлые значения
        DeleteItem(i, j, false);
        //ставим новые
        isFree[newI, newJ] = false;
        buttonsValues[newI, newJ] = curValue;
        //устанавливаем картинку
        buttonsImgs[newI, newJ].GetComponent<Image>().sprite = CurrentSpriteToValue(curValue);
        buttonsImgs[newI, newJ].SetActive(true);

        if (isNewValue)
        {
            PlayCurrSoun(popSound);
        }

        StartCoroutine(ActiveDirectionItemAnimation(buttonsImgs[newI, newJ], buttonsAnim[newI, newJ], newI, newJ, direction));
        SetItemsScriptsValues(newI, newJ);
    }

    //особождаем эллемент 
    private void DeleteItem(int i, int j, bool isAnim)
    {
        if (!isFree[i, j])
        {
            if (isAnim)
            {
                StartCoroutine(DeleteActiveItemWithAnim(buttonsImgs[i, j], buttonsAnim[i, j], i, j));
            }
            else
            {
                buttonsImgs[i, j].SetActive(false);
                buttonsImgs[i, j].GetComponent<Image>().sprite = null;
                buttonsValues[i, j] = 0;
                isFree[i, j] = true;
                buttons[i, j].GetComponent<MiniGameBtnController>().SetIsFree(isFree[i, j]);
            }
            SetItemsScriptsValues(i, j);
        }
    }

    //определяем сторону слайда
    public void DefineSlideDirection(Vector3 startSlideVector, Vector3 endSlideVector)
    {
        if (isCanMove) {
            if (Math.Abs(endSlideVector.x - startSlideVector.x) > Math.Abs(endSlideVector.y - startSlideVector.y))
            {
                //по оси X
                if (endSlideVector.x < startSlideVector.x)
                {
                    //налево
                    StepToLeft();
                }
                else
                {
                    //направо
                    StepToRight();
                }
                PlayCurrSoun(moveSound);
                //спавним новый эллемент
                RandomItemSpawn();
                //прибавляем ход
                UpdateCurrentMoveCount();
            }
            else if (Math.Abs(endSlideVector.x - startSlideVector.x) < Math.Abs(endSlideVector.y - startSlideVector.y))
            {
                //по оси Y
                if (endSlideVector.y < startSlideVector.y)
                {
                    //вниз
                    StepToDown();
                }
                else
                {
                    //вверх
                    StepToUp();
                }
                PlayCurrSoun(moveSound);
                //спавним новый эллемент
                RandomItemSpawn();
                //прибавляем ход
                UpdateCurrentMoveCount();
            }
        }
    }

    //выключение объекта после анимации
    IEnumerator DeleteActiveItemWithAnim(GameObject item, Animation itemAnim, int i, int j)
    {
        buttonsValues[i, j] = 0;
        itemAnim.Play("MiniGameBtnDeleteAnim");
        yield return new WaitForSeconds(0.15f);
        isFree[i, j] = true;
        buttons[i, j].GetComponent<MiniGameBtnController>().SetIsFree(isFree[i, j]);
        item.SetActive(false);
        item.GetComponent<Image>().sprite = null;
    }

    //включение объекта после анимации
    IEnumerator SpawnActiveItemWithAnim(GameObject item, Animation itemAnim, int i, int j)
    {
        isCanMove = false;
        itemAnim.Play("MiniGameBtnSpawnAnim");
        isFree[i, j] = false;
        buttons[i, j].GetComponent<MiniGameBtnController>().SetIsFree(isFree[i, j]);
        yield return new WaitForSeconds(0.15f);
        isCanMove = true;
    }

    //анимация по направлению
    IEnumerator ActiveDirectionItemAnimation(GameObject item, Animation itemAnim, int i, int j,string direction)
    {
        isCanMove = false;
        switch (direction)
        {
            case "right":
                itemAnim.Play("MiniGameBtnRightAnim");
                break;
            case "left":
                itemAnim.Play("MiniGameBtnLeftAnim");
                break;
            case "up":
                itemAnim.Play("MiniGameBtnUpAnim");
                break;
            case "down":
                itemAnim.Play("MiniGameBtnDownAnim");
                break;
        }

        isFree[i, j] = false;
        buttons[i, j].GetComponent<MiniGameBtnController>().SetIsFree(isFree[i, j]);
        yield return new WaitForSeconds(0.15f);
        isCanMove = true;
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

    //играем выбранную музыку
    private void PlayCurrSoun(AudioSource audio)
    {
        if (Sound)
        {
            audio.Play();
        }
    }

    //рандомный номер при спавне (либо 2 либо 4)
    private int RandBtnValue()
    {
        int v = Random.Range(0, 3);
        if (v == 0)
        {
            return 2;
        }
        else
        {
            return 4;
        }
    }

    //устанавливаем спрайт
    private Sprite CurrentSpriteToValue(int val)
    {
        Sprite spr = null;
        switch (val) 
        { 
            case 2:
                spr = btnSpriteToValue[0];
                break;
            case 4:
                spr = btnSpriteToValue[1];
                break;
            case 8:
                spr = btnSpriteToValue[2];
                break;
            case 16:
                spr = btnSpriteToValue[3];
                break;
            case 32:
                spr = btnSpriteToValue[4];
                break;
            case 64:
                spr = btnSpriteToValue[5];
                break;
            case 128:
                spr = btnSpriteToValue[6];
                break;
            case 256:
                spr = btnSpriteToValue[7];
                break;
            default:
                spr = btnSpriteToValue[0];
                break;
        }

        return spr;
    }

    //есть ли свободное место на карте
    private bool isAllFree()
    {
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < length; j++)
            {
                if (isFree[i, j])
                {
                    return true;
                }
            }
        }
        return false;
    }

    //проверка есть ли куда ходить
    private bool IsCanMove()
    {
        //проверяем все большие кнопки
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < length; j++)
            {
                //если это активный предмет, проверяем есть ли у него ходы
                if (!isFree[i, j])
                {
                    if (IsFreeColumn(i, i+1, j) || IsFreeColumn(i, i-1, j) ||
                        IsFreeRow(j, j+1, i) || IsFreeRow(j, j-1, i))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    //свободна ли строка движения
    private bool IsFreeRow(int currJ, int selectedJ, int indI)
    {
        if (selectedJ < 0 || selectedJ >= length)
        {
            return false;
        }
        if (!isFree[indI, selectedJ] && buttonsValues[indI, selectedJ] != buttonsValues[indI, currJ])
        {
            return false;
        }
        return true;
    }

    //свободна ли колонка движения
    private bool IsFreeColumn(int currI, int selectedI, int indJ)
    {
        if (selectedI < 0 || selectedI >= length)
        {
            return false;
        }
        if (!isFree[selectedI, indJ] && buttonsValues[selectedI, indJ] != buttonsValues[currI, indJ])
        {
            return false;
        }
        return true;
    }

    //считаем моенты в конце
    private int CalculateCoins()
    {
        int coins = 0;
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < length; j++)
            {

                switch (buttonsValues[i, j])
                {
                    case 2:
                        coins += 1;
                        break;
                    case 4:
                        coins += 2;
                        break;
                    case 8:
                        coins += 3;
                        break;
                    case 16:
                        coins += 4;
                        break;
                    case 32:
                        coins += 5;
                        break;
                    case 64:
                        coins += 6;
                        break;
                    case 128:
                        coins += 10;
                        break;
                    case 256:
                        coins += 12;
                        break;
                }
            }
        }
        return coins;
    }

    //устанавливаем значения в скрипт на айтемы
    private void SetItemsScriptsValues(int i, int j)
    {
        buttons[i,j].GetComponent<MiniGameBtnController>().setValue(buttonsValues[i, j]);
        buttons[i,j].GetComponent<MiniGameBtnController>().SetIsFree(isFree[i, j]);
    }

    //выход в меню
    public void Home()
    {
        StartCoroutine(openScene("main menu"));
    }

    //открываем сцену после задержки для перехода
    IEnumerator openScene(string sceneName)
    {
        float fadeTime = mainCamera.GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(sceneName);
    }
}
