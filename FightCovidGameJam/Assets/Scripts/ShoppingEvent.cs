using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShoppingEvent : MonoBehaviour
{
    public GameObject shopping_selection_prefab;
    public GameObject shopping_button_prefab;
    public GameObject shopping_list;

    // Start is called before the first frame update
    void Start()
    {
        TextAsset json_file = Resources.Load("Shopping_Event") as TextAsset;
        JSONObject json = new JSONObject(json_file.text);

        JSONObject options = json.GetField("options");
        foreach (JSONObject j_option in options.list)
        {
            SituationsManager.SelectionChoice selection = JsonUtility.FromJson<SituationsManager.SelectionChoice>(j_option.ToString());

            j_option.GetField("stat_requirement", delegate (JSONObject stat_requirement)
            {
                if (stat_requirement.GetField("value").IsBool)
                {
                    selection.bool_requirement = JsonUtility.FromJson<GameManager.Stat<bool>>(stat_requirement.ToString());
                    selection.int_requirement = new GameManager.Stat<int>("none", 0);
                }
                else
                {
                    selection.int_requirement = JsonUtility.FromJson<GameManager.Stat<int>>(stat_requirement.ToString());
                    selection.bool_requirement = new GameManager.Stat<bool>("none", false);
                }
            }, delegate (string name)
            {
                selection.bool_requirement = new GameManager.Stat<bool>("0", false);
                selection.int_requirement = new GameManager.Stat<int>("0", 0);
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

    private void CreateShoppingSelection(SituationsManager.SelectionChoice selection)
    {
        GameObject selection_go = Instantiate(shopping_selection_prefab);
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

        selection_go.transform.SetParent(shopping_list.transform, false);
    }
    private void OnSelection(SituationsManager.SelectionChoice selection, GameObject selection_go)
    {
        //selection_go.
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
