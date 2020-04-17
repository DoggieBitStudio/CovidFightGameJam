﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class UIManager : MonoBehaviour
{
    GameManager gameManager;
    //Task
    public GameObject verticalTask;
    public GameObject buttonTask;
    public GameObject closeButton;
    public bool isTaskMenuOpen = false;

    //Dialogue objects
    public GameObject time;

    //Dialogue texts
    Text time_text;

    public void SetTimeText(float time)
    {
        time_text.text = time.ToString();
    }

    public void RealizeAction(uint time, int health, int positivism, int mask)
    {
        gameManager.AdvanceTime(time);
        gameManager.AddPositivism(positivism);
        gameManager.AddHealth(health);
        gameManager.mask += mask;
    }

    public void CloseTask()
    {
        isTaskMenuOpen = false;

        var taskCount = verticalTask.transform.childCount;
        for(int i = 0; i < taskCount; ++i)
        {
            GameObject.Destroy(verticalTask.transform.GetChild(i).gameObject);
        }
    }
}
