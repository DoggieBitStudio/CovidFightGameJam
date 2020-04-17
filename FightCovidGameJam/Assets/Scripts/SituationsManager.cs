using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SituationsManager : MonoBehaviour
{
    List<Situation> completed_situations;
    List<Situation> day_situations;

    Situation current_situation;
    int completed_today = 0;

    [System.Serializable]
    public struct SelectionChoice
    {
        public string text;
        public float time_investment;

        public System.Tuple<string, int> int_requirement;
        public System.Tuple<string, bool> bool_requirement;

        public List<System.Tuple<string, int>> int_effects;
        public List<System.Tuple<string, bool>> bool_effects;
    }

    // Start is called before the first frame update
    void Start()
    {
        day_situations = new List<Situation>();
        LoadSituations("Day_1");
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
            && current_situation.sequence[current_situation.current_step + 1].Item1 == Situation.PacketType.SELECTION);
    }

    void StartStep()
    {
        switch (current_situation.sequence[current_situation.current_step].Item1)
        {
            case Situation.PacketType.NONE:
                break;
            case Situation.PacketType.DIALOGUE:
                GameManager.instance.dialogue_manager.StartDialogue(current_situation.sequence[current_situation.current_step].Item2);
                break;
            case Situation.PacketType.SELECTION:
                CreateSelection();
                break;
            case Situation.PacketType.ACTION:
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
            JSONObject stat_requirement = j_option.GetField("stat_requirement");
            if(stat_requirement)
            {
                if (stat_requirement.GetField("value").IsBool)
                    selection.bool_requirement = JsonUtility.FromJson<System.Tuple<string, bool>>(stat_requirement.ToString());
                else
                    selection.int_requirement = JsonUtility.FromJson<System.Tuple<string, int>>(stat_requirement.ToString());
            }
            JSONObject effects = j_option.GetField("effect");
            foreach(JSONObject j_eff in effects.list)
            {
                if (j_eff.GetField("value").IsBool)
                    selection.bool_effects.Add(JsonUtility.FromJson<System.Tuple<string, bool>>(j_eff.ToString()));
                else
                    selection.int_effects.Add(JsonUtility.FromJson<System.Tuple<string, int>>(j_eff.ToString()));
            }

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
                    Situation.PacketType packet_type = Situation.PacketType.NONE;

                    if (type.Equals("Dialogue"))
                        packet_type = Situation.PacketType.DIALOGUE;
                    else if (type.Equals("Action"))
                        packet_type = Situation.PacketType.ACTION;
                    else if (type.Equals("Selection"))
                        packet_type = Situation.PacketType.SELECTION;

                    situation.sequence.Add(new System.Tuple<Situation.PacketType, JSONObject>(packet_type, packet));
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
