using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class UIManager : MonoBehaviour
{
    //Task
    public GameObject verticalTask;
    public GameObject buttonTask;
    public GameObject titleTask;
    public bool isTaskMenuOpen = false;

    //Dialogue objects
    public Text time_text;

    public void SetTimeText(float time)
    {
        double decimal_part = (time - System.Math.Truncate(time));
        decimal_part = (decimal_part * 0.6) * 100;
        int intpart = (int)time;
        time_text.text = intpart.ToString("0#") + ":" + decimal_part.ToString("#00");
    }

    public void Start()
    {
        
    }

    public void RealizeAction(uint time, int health, int positivism, int mask)
    {
        GameManager.instance.AdvanceTime(time);
        GameManager.instance.AddPositivism(positivism);
        GameManager.instance.AddHealth(health);
        GameManager.instance.mask += mask;
    }

    public void CloseTask()
    {
        isTaskMenuOpen = false;
        verticalTask.SetActive(false);

        var taskCount = verticalTask.transform.childCount;
        for(int i = 0; i < taskCount; ++i)
        {
            GameObject.Destroy(verticalTask.transform.GetChild(i).gameObject);
        }
    }
}
