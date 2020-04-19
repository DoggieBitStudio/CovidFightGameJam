using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ShoppingEvent : MonoBehaviour
{
    public GameObject shopping_selection_prefab;
    public GameObject shopping_list;

    public Dictionary<Toggle, SituationsManager.SelectionChoice> toggles;
    public Button close_button;
    public Button accept_button;

    // Start is called before the first frame update
    void Awake()
    {
        toggles = new Dictionary<Toggle, SituationsManager.SelectionChoice>();
    }

    private void CreateShoppingSelection(SituationsManager.SelectionChoice selection)
    {
        GameObject selection_go = Instantiate(shopping_selection_prefab);
        selection_go.GetComponentInChildren<Text>().text = selection.text;

        bool booleans_check = true;
        bool ints_check = true;

        foreach (var item in selection.bool_requirements)
        {
            if (GameManager.instance.boolean_stats[item.stat] != item.value)
            {
                booleans_check = false;
                break;
            }
        }
        foreach (var item in selection.int_requirements)
        {
            if (GameManager.instance.int_stats[item.stat] < item.value)
            {
                ints_check = false;
                break;
            }
        }

        if(!booleans_check || !ints_check)
        {
            selection_go.GetComponent<Toggle>().interactable = false;
        }

        selection_go.transform.SetParent(shopping_list.transform, false);
        selection_go.transform.DOPunchScale(new Vector3(0.1f, 0.5f, 0.0f), 1.0f, 3);
        selection_go.GetComponent<Toggle>().onValueChanged.AddListener(delegate { OnSelection(); });
        toggles.Add(selection_go.GetComponent<Toggle>(), selection);
    }
    private void OnSelection()
    {
        bool activate = false;

        foreach (var item in toggles)
        {
            if (!item.Key.isOn)
                activate = true;
        }

        accept_button.interactable = activate;
    }

    public void Accept()
    {
        float time_increment = 0;

        foreach (var item in toggles)
        {
            if (!item.Key.isOn)
            {
                time_increment += item.Value.time_investment;
                foreach (var ints in item.Value.int_effects)
                {
                    GameManager.instance.int_stats[ints.stat] += ints.value;
                }

                foreach (var bools in item.Value.bool_effects)
                {
                    GameManager.instance.boolean_stats[bools.stat] = bools.value;
                }
            }
        }

        GameManager.instance.situations_manager.current_situation.duration += time_increment;
        GameManager.instance.situations_manager.OnStepFinish();
        GameManager.instance.ui_opened = false;

        gameObject.SetActive(false);
    }

    public void Close()
    {
        GameManager.instance.situations_manager.current_situation.duration = 0;
        GameManager.instance.situations_manager.OnSituationEnd();

        gameObject.SetActive(false);
        GameManager.instance.ui_opened = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
       
    }

    internal void LoadEvent(JSONObject json)
    {
        GameManager.instance.ui_opened = true;
        close_button.interactable = GameManager.instance.boolean_stats["Shop"];
        accept_button.interactable = false;

        foreach (Transform child in shopping_list.transform)
        {
            Destroy(child.gameObject);
        }

        JSONObject options = json.GetField("options");
        foreach (JSONObject j_option in options.list)
        {
            SituationsManager.SelectionChoice selection = JsonUtility.FromJson<SituationsManager.SelectionChoice>(j_option.ToString());

            j_option.GetField("stat_requirement", delegate (JSONObject stat_requirement)
            {
                foreach (JSONObject j_req in stat_requirement.list)
                {
                    if (j_req.GetField("value").IsBool)
                        selection.bool_requirements.Add(JsonUtility.FromJson<GameManager.Stat<bool>>(j_req.ToString()));
                    else
                        selection.int_requirements.Add(JsonUtility.FromJson<GameManager.Stat<int>>(j_req.ToString()));
                }
            }, delegate (string name)
            {
            });

            selection.int_effects = new List<GameManager.Stat<int>>();
            selection.bool_effects = new List<GameManager.Stat<bool>>();

            j_option.GetField("effect", delegate (JSONObject effects)
            {
                foreach (JSONObject j_eff in effects.list)
                {
                    if (j_eff.GetField("value").IsBool)
                        selection.bool_effects.Add(JsonUtility.FromJson<GameManager.Stat<bool>>(j_eff.ToString()));
                    else
                        selection.int_effects.Add(JsonUtility.FromJson<GameManager.Stat<int>>(j_eff.ToString()));
                }
            }, delegate (string name)
            {
                Debug.Log("No effects found on");
            });

            CreateShoppingSelection(selection);
        }
    }
}
