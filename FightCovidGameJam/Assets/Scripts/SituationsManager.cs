using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SituationsManager : MonoBehaviour
{
    GameObject shopping_event;

    List<Situation> completed_situations;
    List<Situation> day_situations;

    public Situation current_situation;
    int completed_today = 0;

    [System.Serializable]
    public struct SelectionChoice
    {
        public string text;
        public float time_investment;

        public GameManager.Stat<int> int_requirement;
        public GameManager.Stat<bool> bool_requirement;

        public List<GameManager.Stat<int>> int_effects;
        public List<GameManager.Stat<bool>> bool_effects;
    }

    // Start is called before the first frame update
    void Start()
    {
        day_situations = new List<Situation>();

        string day = GameManager.instance.current_character == CHARACTER.CARMEN ? GameManager.instance.carmen_day.ToString() : GameManager.instance.julian_day.ToString();
        string character = GameManager.instance.current_character == CHARACTER.CARMEN ? "Carmen" : "Julian";
        
        LoadSituations("Day_"+day+"_"+character);

        shopping_event = GameObject.Find("ShoppingPanel");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CompleteAction(string identifier)
    {
        //current_situation.CompleteAction(identifier);
    }

    public void OnStepFinish()
    {
        current_situation.current_step++;
        if(current_situation.current_step < current_situation.sequence.Count())
            StartStep();
        else
            OnSituationEnd();
    }

    public void OnStepFinish(int v)
    {
        current_situation.current_step += v;
        if (current_situation.current_step < current_situation.sequence.Count())
            StartStep();
        else
            OnSituationEnd();
    }

    void OnSituationEnd()
    {
        completed_today++;
        GameManager.instance.AdvanceTime(current_situation.duration / 60);

        StartNextSituation();
    }

    public void StartNextSituation()
    {
        if (completed_today < day_situations.Count() && day_situations[completed_today].activation_time <= GameManager.instance.time)
        {
            current_situation = day_situations[completed_today];
            StartStep();
        }
    }

    public bool IsNextStepSelection()
    {
        return (current_situation.current_step + 1 < current_situation.sequence.Count() 
            && current_situation.sequence[current_situation.current_step + 1].Item1 == Step.SELECTION);
    }

    void StartStep()
    {
        switch (current_situation.sequence[current_situation.current_step].Item1)
        {
            case Step.NONE:
                break;
            case Step.DIALOGUE:
                GameManager.instance.dialogue_manager.StartDialogue(current_situation.sequence[current_situation.current_step].Item2);
                break;
            case Step.SELECTION:
                CreateSelection();
                break;
            case Step.SHOPPING:
                shopping_event.SetActive(true);
                break;
            case Step.BATHROOM:
                SceneManager.LoadScene("bathroom");
                break;
            case Step.SLEEP:
                if (GameManager.instance.current_character == CHARACTER.CARMEN)
                    GameManager.instance.carmen_day += (int)current_situation.duration;
                else
                    GameManager.instance.julian_day += (int)current_situation.duration;

                SceneManager.LoadScene("Main");
                break;
            default:
                break;
        }
       
    }

    void CreateSelection()
    {
        JSONObject options = current_situation.sequence[current_situation.current_step].Item2.GetField("options");
        foreach (JSONObject j_option in options.list)
        {
            SelectionChoice selection = JsonUtility.FromJson<SelectionChoice>(j_option.ToString());

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


            GameManager.instance.ui_manager.CreateSelection(selection);
        }
        
    }

    public void LoadSituations(string identifier)
    {
        TextAsset json_file = Resources.Load(identifier) as TextAsset;
		JSONObject json = new JSONObject(json_file.text);

        JSONObject situations_array = json.GetField("Situations");
        foreach (JSONObject situation_json in situations_array.list)
        {
            Situation situation = JsonUtility.FromJson<Situation>(situation_json.ToString());

            situation_json.GetField("sequence", delegate (JSONObject sequence)
            {
                foreach (JSONObject packet in sequence.list)
                {
                    string type = packet.GetField("type").str;
                    Step packet_type = Step.NONE;

                    if (type.Equals("Dialogue"))
                        packet_type = Step.DIALOGUE;
                    else if (type.Equals("Bathroom"))
                        packet_type = Step.BATHROOM;
                    else if (type.Equals("Selection"))
                        packet_type = Step.SELECTION;
                    else if (type.Equals("Shopping"))
                        packet_type = Step.SHOPPING;

                    situation.sequence.Add(new System.Tuple<Step, JSONObject>(packet_type, packet));
                }
            }, delegate (string name)
            {
                Debug.LogWarning("No sequence found on " + situation.identifier);
            });

            day_situations.Add(situation);
        }

        current_situation = day_situations[completed_today];
        StartStep();
    }
}
