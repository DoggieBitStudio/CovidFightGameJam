using System.Collections;
using System.Collections.Generic;
using System.IO;
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
                    string type = packet.GetField("type").ToString();
                    Situation.PacketType packet_type = Situation.PacketType.NONE;

                    if (type == "Dialogue")
                        packet_type = Situation.PacketType.DIALOGUE;
                    else if (type == "Action")
                        packet_type = Situation.PacketType.ACTION;
                    else if (type == "Selection")
                        packet_type = Situation.PacketType.SELECTION;

                    situation.sequence.Add(packet_type, packet);
                }
            }, delegate (string name)
            {
                Debug.LogWarning("No sequence found on " + situation.identifier);
            });

            day_situations.Add(situation);
        }

        current_situation = day_situations[0];
        GameManager.instance.dialogue_manager.StartDialogue(current_situation.sequence[0]);
    }
}
