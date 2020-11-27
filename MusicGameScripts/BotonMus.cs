using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BotonMus : MonoBehaviour
{
    
    [SerializeField] private TriggerEnterBtnMus[] trigger;
    [SerializeField] private string button;
    [SerializeField] private KeyCode key;
    [SerializeField] private MusicGameManager ms;
    [SerializeField] private int score = 0;
    [SerializeField] private AudioClip[] clips = new AudioClip[4];
    [SerializeField] private AudioSource _as;

    [SerializeField] private UnityEvent destroyEvent;




    void Start()
    {
        _as = GetComponent<AudioSource>();
        trigger = GetComponentsInChildren<TriggerEnterBtnMus>();
    }

    
    void Update()
    {
        if(Input.GetKeyDown(key))
        {
            triggerEnter();
            destroyMus();
            ms.setScore(score);
            score = 0;
            
        }
    }
    void triggerEnter()
    {
        {
            foreach (TriggerEnterBtnMus t in trigger)
            {
                if (t.inside)
                {
                    //as.clip = clips[UnityEngine.Random.Range(0, 3)];
                    //_as.Play();
                    //t.GetComponent<Animator>().Play("AnimationReceptor");
                    score += 1;
                }
            }
            destroyEvent.Invoke();
        }
    }

    void destroyMus()
    {
        foreach (TriggerEnterBtnMus t in trigger)
        {
            Destroy(t.currentObjMus);
        }
    }
}
