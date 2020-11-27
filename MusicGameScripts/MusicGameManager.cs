using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MusicGameManager : MonoBehaviour
{

    [SerializeField] private List<Level> levels;
    [SerializeField] private AudioSource mainMusic;
    [SerializeField] private GameObject transitionImage;
    [SerializeField] private float transitionSpeed;
    //Nivel de juego
    [SerializeField] private int currentlevel;


    //Score que se muestra en la pantalla
    [SerializeField] private Text score;
    [SerializeField] private Text failscore;
    [SerializeField] private Image scorefill;
    [SerializeField] private Image failfill;

    //Estado del juego
    public GameState currentGameState;



    [SerializeField] private int currentScore;
    [SerializeField] private List<int> scoreLevel;
    [SerializeField] private float totalTime;
    [SerializeField] private float currentTime;

    //Fails y cantidad e fails total que se pueden tolerar
    [SerializeField] private int totalfail;
    [SerializeField] private int currentfail;
    //Evento cuando se cambia el score
    public scoreChange scoreChanged;
    [Serializable]
    public class scoreChange : UnityEvent<int> { }

    [Serializable]
    internal class Level
    {
        public float scoreMax;
        public GameObject levelObject;
        public AudioClip mainMusic;
    }


    private void Start()
    {
        currentlevel = 0;
        Cursor.visible = false;
        currentGameState = GameState.playing;
        failscore.text = "Fails: " + currentfail + "/" + totalfail;
    }

    private void Update()
    {
        SetUI();
        changeLevel();
        gameStateController();
        checkState();
    }


    void changeLevel()
    {
        if (currentScore > levels[currentlevel].scoreMax )
        {
            if (levels.Count > currentlevel + 1)
            {
                currentScore = 0;
                currentfail = 0;
                currentlevel += 1;
                setLevel(currentlevel);
            }
            else
            {
                currentGameState = GameState.win;
            }
        }
        
    }

    void setLevel(int CurrentLevel)
    {
        transitionImage.GetComponent<Animator>().Play("cosoanimation");
        print("CurrentLevel" + currentlevel);
        mainMusic.Play();
        StartCoroutine(transition());
    }

    //Cheque status, usado por tema de pausa.
    void checkState()
    {
        if(currentScore < scoreLevel[2] && currentfail < totalfail && currentGameState != GameState.paused)
        {
            setState(2);
        }
        else if(currentScore < scoreLevel[2] && currentfail >= totalfail && currentGameState != GameState.paused)
        {
            setState(1);
        }
        else if(currentScore >= scoreLevel[2] && currentfail <= totalfail && currentGameState != GameState.paused)
        {
            setState(0);
        }
    }
    //Seteo el score
    public void setScore(int i)
    {
        scoreChanged.Invoke(i);
        currentScore += i;
        
    }

    public void SetUI()
    {
        score.text = "Score: " + currentScore;
        scorefill.fillAmount = currentScore / levels[currentlevel].scoreMax;
        failscore.text = "Fails: " + currentfail + "/" + totalfail;
        failfill.fillAmount = (currentfail / totalfail * 1f);
    }
    void addFail()
    {
        if(currentfail < totalfail )
        {
            currentfail++; 
        }
    }

    //Seteo el estado del juego
    void setState(int i)
    {
        currentGameState = (GameState)i;
    }

    void waitForTime()
    {
        float wait = 1;
        while (wait > 0)
        {
            wait -= Time.deltaTime;
        }
    }

    //State machine
    void gameStateController()
    {
        switch (currentGameState)
        {
            case GameState.win:
                {
                    TheGameManager.instance.FinishLevel();
                    
                    //Time.timeScale = 0;
                    break;
                }
            case GameState.paused:
                {
                    Time.timeScale = 0;
                    break;
                }
            case GameState.lose:
                {
                    //Time.timeScale = 0; 
                    if (TheGameManager.instance != null) { TheGameManager.instance.LoseLife(); }
                    break;
                }
            case GameState.restart:
                {
                    break;
                }
            case GameState.playing:
                {
                    /*foreach (SpawnObjMus s in spa)
                    {
                        s.gameObject.SetActive(true);
                    };*/
                    Time.timeScale = 1;
                    break;
                }
            case GameState.starting:
                {
                    break;
                }
        }
    }

    IEnumerator transition()
    {
        mainMusic.clip = levels[currentlevel].mainMusic;
        mainMusic.Play();
        new WaitForSeconds(transitionSpeed);
        for (int i = 0; i < levels.Count; i++)
        {

            if (!(levels[i] == levels[currentlevel]))
            {
                levels[i].levelObject.SetActive(false);
            }
            else
            {
                levels[i].levelObject.SetActive(true);
            }
        }
        yield return null;
    }
        
}
