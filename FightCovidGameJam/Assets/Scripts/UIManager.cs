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

    //Stats
    public RawImage mask_ui;

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

    public void RealizeActionHospital(GameObject patient, int action, uint time, int health, int positivism)
    {
        double decimal_time = (float)time / 60.0;
        GameManager.instance.action_manager.DoActionHospital(patient, action, decimal_time, health, positivism);

        CloseTask();
    }


    public void Update()
    {
        if (Input.GetKey(KeyCode.F3))
            GameManager.instance.int_stats["Positivism"] -= 1;
        if (Input.GetKey(KeyCode.F4))
            GameManager.instance.int_stats["Positivism"] += 1;
        if (Input.GetKey(KeyCode.F5))
            GameManager.instance.int_stats["Health"] -= 1;
        if (Input.GetKey(KeyCode.F6))
            GameManager.instance.int_stats["Health"] += 1;
        if (Input.GetKey(KeyCode.F7))
            GameManager.instance.boolean_stats["Mask"] = true;
        if (Input.GetKey(KeyCode.F8))
            GameManager.instance.boolean_stats["Mask"] = false;

        mask_ui.color = GameManager.instance.boolean_stats["Mask"] ? Color.white : Color.gray;
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

        foreach (var item in selection.bool_requirements)
        {
            if (GameManager.instance.boolean_stats[item.stat] != item.value)
            {
                selection_go.GetComponent<Image>().color = Color.gray;
                selection_go.GetComponent<Button>().interactable = false;

                RawImage img = selection_go.GetComponentInChildren<RawImage>();
                img.texture = emojis[item.stat];
                img.enabled = true;
                break;
            }
        }
        foreach (var item in selection.int_requirements)
        {
            if (GameManager.instance.int_stats[item.stat] < item.value)
            {
                selection_go.GetComponent<Image>().color = Color.gray;
                selection_go.GetComponent<Button>().interactable = false;

                RawImage img = selection_go.GetComponentInChildren<RawImage>();
                img.texture = emojis[item.stat];
                img.enabled = true;
                break;
            }
        }

        foreach (var item in selection.int_effects)
        {
            GameObject eff_obj = Instantiate(Resources.Load<GameObject>("UI/Effect_Image"), selection_go.transform.GetChild(2));
            float scale = 1;

            if (item.stat.Equals("Health"))
            {
                eff_obj.GetComponent<RawImage>().texture = item.value > 0 ? Resources.Load<Texture>("UI/Health_Positive"): Resources.Load<Texture>("UI/Health");
                
                if (Math.Abs(item.value) < 2.0)
                    scale = 0.5f;
            }
            else
            {
                eff_obj.GetComponent<RawImage>().texture = item.value > 0 ? Resources.Load<Texture>("UI/green_face") : Resources.Load<Texture>("UI/Positivism");
                if (Math.Abs(item.value) < 2.0)
                    scale = 0.5f;
                else if (Math.Abs(item.value) >= 4)
                    scale = 1.5f;
            }

            eff_obj.transform.localScale = new Vector3(scale, scale, 1);
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
        GameManager.instance.ui_opened = false;
        GameManager.instance.dialogue_manager.EndDialogue();
        foreach (Transform child in selection_list.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
