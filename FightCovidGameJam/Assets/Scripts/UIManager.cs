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

    internal void CreateSelection(SituationsManager.SelectionChoice selection)
    {
        GameObject selection_go = Instantiate(selection_prefab);
        selection_go.GetComponentInChildren<Text>().text = selection.text;

        if (GameManager.instance.boolean_stats.ContainsKey(selection.bool_requirement.Item1) 
            && GameManager.instance.boolean_stats[selection.bool_requirement.Item1] != selection.bool_requirement.Item2)
        {
            selection_go.GetComponent<Image>().color = Color.gray;
            selection_go.GetComponent<Button>().interactable = false;
        }
        else if (GameManager.instance.int_stats.ContainsKey(selection.int_requirement.Item1) 
            && GameManager.instance.int_stats[selection.int_requirement.Item1] < selection.int_requirement.Item2)
        {
            selection_go.GetComponent<Image>().color = Color.gray;
            selection_go.GetComponent<Button>().interactable = false;
        }
        selection_go.GetComponent<Button>().onClick.AddListener(delegate { OnSelection(selection); });
    }

    private void OnSelection(SituationsManager.SelectionChoice selection)
    {
        foreach (var item in selection.int_effects)
        {
            GameManager.instance.int_stats[item.Item1] += item.Item2;
        }

        foreach (var item in selection.bool_effects)
        {
            GameManager.instance.boolean_stats[item.Item1] = item.Item2;
        }

        //GameManager.instance.situations_manager.OnStepFinish(); end dialogue does it for now
        GameManager.instance.dialogue_manager.EndDialogue();
    }
}
