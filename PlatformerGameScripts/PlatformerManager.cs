using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlatformerManager : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject[] Levels;
    [SerializeField] private GameObject[] End;
    [SerializeField] private Transform[] LevelStart;

    [SerializeField] private AudioSource audioS;
    [SerializeField] private AudioClip[] clips;


    public GameState currentGameState;

    [SerializeField] private int currentLevel;
    [SerializeField] private int currentlives;

    private Vector2 startingPosition;
    void Start()
    {
        //Start with first level active
        ActivateLevel(0);
        startingPosition = LevelStart[0].position;
    }
    void Update()
    {
        gameStateController();
    }

    void ActivateLevel(int i)
    {
        for(int j = 0; j < Levels.Length; j++)
        {
            if(j == i)
            {
                Levels[j].SetActive(true); 

            }
            else
            {
                Levels[j].SetActive(false);
            }
        }
        
    }
    public void setState(int i)
    {
        currentGameState = (GameState)i;
    }

    public void PlayerRevive()
    {
        Player.transform.position = LevelStart[currentLevel].position;
        Player.SetActive(true);
        setState(2);
    }
    public void setNextLevel()
    {
        if(currentLevel != Levels.Length-1)
        {
            currentLevel++;
            ActivateLevel(currentLevel);
            Player.transform.position = LevelStart[currentLevel].position;
            audioS.clip = clips[currentLevel];
            audioS.Play();
            
        }
        else
        {
            setState(0);
        }
    }

    public void waitForTime()
    {
        float wait = 1;
        while (wait > 0)
        {
            wait -= Time.deltaTime;
        }
    }
    public void gameStateController()
    {
        switch(currentGameState)
        {
            case GameState.win:
                {
                    waitForTime();
                    if (TheGameManager.instance) { TheGameManager.instance.FinishLevel();};
                    break;
                }
            case GameState.lose:
                {
                    Player.SetActive(false);
                    PlayerRevive();
                    if (TheGameManager.instance) { TheGameManager.instance.LoseLife(); };
                    break;
                }
            case GameState.playing:
                {
                    Player.SetActive(true);
                    Time.timeScale = 1;
                    break;
                }
            case GameState.starting:
                {
                    Player.SetActive(false);
                    break;
                }

        }
    }
}
