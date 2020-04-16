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

    public void LoadSituations(string identifier)
    {
        TextAsset json_file = Resources.Load(identifier) as TextAsset;
		JSONObject json = new JSONObject(json_file.text);

        JSONObject situations_array = json.GetField("Situations");
        foreach (JSONObject situation_json in situations_array.list)
        {
            Situation situation = JsonUtility.FromJson<Situation>(situation_json.ToString());

            situation_json.GetField("dialogues", delegate (JSONObject dialogues)
            {
                foreach (JSONObject dialogue_json in dialogues.list)
                {
                    DialogueManager.DialogueInfo dialogue_info = JsonUtility.FromJson<DialogueManager.DialogueInfo>(dialogue_json.ToString());
                    situation.dialogues.Add(dialogue_info);
                }
            }, delegate (string name)
            {
                Debug.LogWarning("No dialogues found on " + situation.identifier);
            });

            day_situations.Add(situation);
        }

        current_situation = day_situations[0];
    }
}
