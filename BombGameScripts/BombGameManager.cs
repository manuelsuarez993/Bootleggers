using BasicSurvivalKit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombGameManager : MonoBehaviour
{
    public static BombGameManager instance;
    //--------------
    // Timer variables
    public float timer;
    public float maxTime;
    //--------------

    //--------------
    // UI text and image
    [SerializeField] private Text clockTxt;
    [SerializeField] private Image endImgUI;
    [SerializeField] private Text currentLives;
    [SerializeField] private TimeEvent timeEvent;
    //--------------
   
    //--------------
    //Bomb variables
    [SerializeField] int max = 4;
    [SerializeField] private List<Image> uiImg;
    [SerializeField] private List<Sprite> imagenes;
    [SerializeField] private List<Sprite> alreadyUsedImg;
    [SerializeField] private List<int> order; 
    [SerializeField] private List<int> currentOrder;
    //--------------

    //--------------
    //Images shown after completing level, 0 is VICTORY 1 is FAILURE
    [SerializeField] private Sprite[] endimages;
    //--------------

    //--------------
    // Game states
    [SerializeField] private GameState currentGameState;
    //--------------
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        timer = maxTime;
        InitializeBomb();
    }

    void Update()
    {
        Cursor.visible = false;
        if (TheGameManager.instance != null) { currentLives.text = "Lives: " + TheGameManager.instance.currentLifes; }
        
        GameStateController();
        CheckOrder();
    }
  

    void Clock()
    {
        timer -= Time.deltaTime;
        clockTxt.text = ":"+Convert.ToInt32(timer).ToString();
        if(timer < 0)
        {
            currentGameState = GameState.lose;
        }
    }
    public void AddToOrder(int or)
    {
        currentOrder.Add(or);
    }
    void InitializeBomb()
    {
        for (int i = 0; i < uiImg.Count; i++)
        {
            Sprite sprite = null;
            alreadyUsedImg.Add(sprite);
        }

        for (int i = 0; i < uiImg.Count; i++)
        {
            int rand = UnityEngine.Random.Range(0, imagenes.Count);

            while (alreadyUsedImg.Contains(imagenes[rand]))
            {
                rand = UnityEngine.Random.Range(0, imagenes.Count);
            }

            uiImg[i].sprite = imagenes[rand]; 
            alreadyUsedImg[i] = imagenes[rand];
            order.Add(rand);

        }
    }
    void CheckOrder()
    {
        if(currentOrder.Count > 0 )
        {
            print("Entro al if de current order > 0");
            if (currentOrder.Count < order.Count)
            {
                print("Entro al if de currentOrder.Count < order.Count");
                for (int i = 0; i < currentOrder.Count; i++)
                {
                    print("Entro al for de count menor a order count");
                    print("Dentro del For: " + i);
                    if (currentOrder[i] != order[i])
                    {
                        print("Entro al if de si el current order es diferente");
                        currentGameState = GameState.lose;
                    }
                }
            }
            else
            {
                for (int i = 0; i < currentOrder.Count - 1; i++)
                {
                    if (currentOrder[i] != order[i])
                    {
                        currentGameState = GameState.lose;
                    }
                }
                currentGameState = GameState.win;

            }
        }
    }

    public void SetState(int i)
    {
        currentGameState = (GameState)i;
    }

    public void GameStateController()
    {
        switch(currentGameState) 
        {
            case GameState.win:
                {
                    timer = 0;
                    endImgUI.enabled = true;
                    endImgUI.sprite = endimages[0];
                    timeEvent.Play();
                    break;
                }
            case GameState.lose:
                {
                    timer = 0;
                    Time.timeScale = 0;
                    endImgUI.enabled = true;
                    endImgUI.sprite = endimages[1];
                    if (TheGameManager.instance) { TheGameManager.instance.LoseLife(); };
                    break;
                }
            case GameState.playing:
                {
                    Clock();
                    Time.timeScale = 1;
                    endImgUI.enabled = false;
                    break;
                }
            case GameState.restart:
                {
                    Time.timeScale = 1;
                    timer = 0;
                    currentGameState = GameState.playing;
                    break;
                }
            case GameState.paused:
                {
                    Time.timeScale = 0;
                    break;
                }
            case GameState.starting:
                {
                    endImgUI.enabled = false;
                    clockTxt.text = "";
                    break;
                }
            default: break;
        }
        
    }
}
