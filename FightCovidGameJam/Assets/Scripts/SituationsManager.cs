using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SituationsManager : MonoBehaviour
{

    List<Situation> completed_situations;
    List<Situation> day_situations;

    Situation current_situation;

    // Start is called before the first frame update
    void Start()
    {
        LoadSituations("Day_1");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadSituations(string identifier)
    {
        StreamReader reader = new StreamReader(Path.Combine(Application.dataPath, "JSON/Situations.json"));
        string json_string = reader.ReadToEnd();
        reader.Close();

		JSONObject json = new JSONObject(json_string);
        JSONObject day_json = json.GetField(identifier);

        Debug.Log(day_json);
        JSONObject situations = day_json.GetField("Situations");
        foreach (JSONObject j in situations.list)
        {
            string situation_string = j.ToString();
            Debug.Log(situation_string);
            Situation situation = JsonUtility.FromJson<Situation>(situation_string);
            Debug.Log(situation.identifier);
        }
    }
}
