using BasicSurvivalKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SnapAttackGameManager : MonoBehaviour
{
    //UI variables
    [SerializeField] private Text scoreText;
    [SerializeField] private Text missedText;
    [SerializeField] private Text currentLifes;

    [SerializeField] private TimeEvent timeEvent;

    private GameState currentGameState;
    //Spawns
    [SerializeField] private int spawnTotal;
    private int currentSpawns;
    //Missed
    private int currentmissed;
    [SerializeField] private int missedTotal;
    //Sets score
    private int score;
    [SerializeField] int totalScore;
    //Timer
    [SerializeField] private float timer;
    [SerializeField] private float inBetweenTime;

    [SerializeField] private SpawnPhotoObj _currentSpawner;

    void Start()
    {
        SetScore(0);
    }

    void Update()
    {
        CheckState();
        gameStateControler();
        if (TheGameManager.instance) { currentLifes.text = "Lives: " + TheGameManager.instance.currentLifes; }

    }

    public void Spawn()
    {
        if (timer <= inBetweenTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            int rand = UnityEngine.Random.Range(0, _currentSpawner.PhotoObj.Length);
            if (_currentSpawner.PhotoObj[rand].CompareTag("PhotoObj"))
            {
                currentSpawns++;
            }
            _currentSpawner.randomizeLocation(rand);
            timer = 0;
        }
    }
    public void SetScore(int i)
    {
        score += i;
        scoreText.text = "Score " + score + "/ Mini: " + totalScore;
    }
    public void Missed()
    {
        if(currentmissed < missedTotal)
        {
            currentmissed++;
        } 
        missedText.text = "Missed: " + currentmissed + "/" + missedTotal;
    }
    void CheckState()
    {
        if (currentGameState != GameState.paused && currentGameState != GameState.starting)
        {
            if (currentmissed > missedTotal)
            {
                SetState(1);
            }
            if (currentSpawns >= spawnTotal)
            {
                if(score > totalScore)
                {
                    SetState(0);
                }
                else
                {
                    SetState(2);
                }
                
            }

            if (currentmissed < missedTotal && currentSpawns < spawnTotal)
            {
                SetState(2);
            }
        }

    }
    void SetState(int i)
    {
        currentGameState = (GameState)i;
    }
    void gameStateControler()
    {
        switch(currentGameState)
        {
            case GameState.playing:
                {
                    Spawn();
                    Time.timeScale = 1;
                    Cursor.visible = false;
                    break;
                }
            case GameState.win:
                {
                    timeEvent.Play();
                    Cursor.visible = true;
                    break;
                }
            case GameState.lose:
                {
                    Cursor.visible = true;
                    TheGameManager.instance.LoseLife();
                    break;
                }
            case GameState.starting:
                {
                    break;
                }
        }
    }


}
