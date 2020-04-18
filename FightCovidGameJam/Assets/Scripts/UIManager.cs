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

    Dictionary<string, Texture> emojis;

    public void SetTimeText(float time)
    {
        double decimal_part = (time - System.Math.Truncate(time));
        decimal_part = (decimal_part * 0.6) * 100;
        int intpart = (int)time;
        time_text.text = intpart.ToString("0#") + ":" + decimal_part.ToString("#00");
    }

    public void Start()
    {
        emojis = new Dictionary<string, Texture>();

        foreach (var item in GameManager.instance.int_stats)
        {
            emojis.Add(item.Key, Resources.Load<Texture>("UI/" + item.Key));
        }
        foreach (var item in GameManager.instance.boolean_stats)
        {
            emojis.Add(item.Key, Resources.Load<Texture>("UI/" + item.Key));
        }
    }
    public void RealizeAction(int action, uint time, int health, int positivism)
    {
        double decimal_time = (float)time / 60.0;
        GameManager.instance.action_manager.DoAction(action, decimal_time, health, positivism);

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

            RawImage img = selection_go.GetComponentInChildren<RawImage>();
            Debug.Log(selection.bool_requirement.stat);
            img.texture = emojis[selection.bool_requirement.stat];
            img.enabled = true;
        }
        else if (GameManager.instance.int_stats.ContainsKey(selection.int_requirement.stat) 
            && GameManager.instance.int_stats[selection.int_requirement.stat] < selection.int_requirement.value)
        {
            selection_go.GetComponent<Image>().color = Color.gray;
            selection_go.GetComponent<Button>().interactable = false;

            RawImage img = selection_go.GetComponentInChildren<RawImage>();
            img.texture = emojis[selection.int_requirement.stat];
            img.enabled = true;
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

        GameManager.instance.situations_manager.current_situation.duration += selection.time_investment;
        if (selection.sound != null)
        {
            GameManager.instance.audio_source.PlayOneShot(Resources.Load<AudioClip>("Sounds/" + selection.sound));
            Debug.Log("Playing audio " + selection.sound);
        }


        //GameManager.instance.situations_manager.OnStepFinish(); end dialogue does it for now
        GameManager.instance.situations_manager.current_situation.current_step.next_step = selection.next_step;
        GameManager.instance.dialogue_manager.EndDialogue();
        GameManager.instance.ui_opened = false;
        foreach (Transform child in selection_list.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
