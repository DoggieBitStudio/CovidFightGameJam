using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class UIManager : MonoBehaviour
{
    //Task
    public GameObject verticalTask;
    public GameObject buttonTask;
    public GameObject titleTask;
    public bool isTaskMenuOpen = false;

    //Dialogue objects
    public Text time_text;

    public GameObject selection_prefab;
    public GameObject selection_list;

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
    public void RealizeAction(int action, uint time, int health, int positivism)
    {
        GameManager.instance.AdvanceTime(time);

        GameManager.instance.AddPositivism(positivism);
        GameManager.instance.AddHealth(health);

        GameManager.instance.action_manager.DoAction(action);

        CloseTask();
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

    internal void CreateSelection(SituationsManager.SelectionChoice selection)
    {
        GameObject selection_go = Instantiate(selection_prefab);
        selection_go.GetComponentInChildren<Text>().text = selection.text;

        if (GameManager.instance.boolean_stats.ContainsKey(selection.bool_requirement.stat) 
            && GameManager.instance.boolean_stats[selection.bool_requirement.stat] != selection.bool_requirement.value)
        {
            selection_go.GetComponent<Image>().color = Color.gray;
            selection_go.GetComponent<Button>().interactable = false;
        }
        else if (GameManager.instance.int_stats.ContainsKey(selection.int_requirement.stat) 
            && GameManager.instance.int_stats[selection.int_requirement.stat] < selection.int_requirement.value)
        {
            selection_go.GetComponent<Image>().color = Color.gray;
            selection_go.GetComponent<Button>().interactable = false;
        }
        selection_go.GetComponent<Button>().onClick.AddListener(delegate { OnSelection(selection); });

        selection_go.transform.SetParent(selection_list.transform, false);
    }

    private void OnSelection(SituationsManager.SelectionChoice selection)
    {
        foreach (var item in selection.int_effects)
        {
            GameManager.instance.int_stats[item.stat] += item.value;
        }

        foreach (var item in selection.bool_effects)
        {
            GameManager.instance.boolean_stats[item.stat] = item.value;
        }

        //GameManager.instance.situations_manager.OnStepFinish(); end dialogue does it for now
        GameManager.instance.dialogue_manager.EndDialogue();

        foreach (Transform child in selection_list.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
