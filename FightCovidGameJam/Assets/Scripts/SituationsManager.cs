using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SituationsManager : MonoBehaviour
{
    List<Situation> completed_situations;
    List<Situation> day_situations;

    public Situation current_situation;

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
        {
            StartStep();
        }
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
                break;
            case Situation.PacketType.ACTION:
                break;
            default:
                break;
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

        current_situation = day_situations[0];
        StartStep();
    }
}
