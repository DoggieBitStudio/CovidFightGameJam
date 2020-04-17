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

    void Start()
    {
        time_text = time.GetComponent<Text>();
    }

    public void SetTimeText(float time)
    {
        time_text.text = time.ToString("0#.00");
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
