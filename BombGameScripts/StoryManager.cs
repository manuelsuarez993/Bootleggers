using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class StoryManager : MonoBehaviour
{
    public static StoryManager thisStory;

    public Mail[] mailList;
    public MailManager mailManager;
    public Transform popup;

    void Start()
    {
        TriggerStory(0);
    }

    void Update()
    {
 
    }

    public void TriggerStory(int i)
    {
        Instantiate(mailList[i].notification, popup);
        mailManager.LoadMail(mailList[i]);
    }

}
