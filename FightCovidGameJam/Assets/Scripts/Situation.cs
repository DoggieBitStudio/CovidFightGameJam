using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Specialized;

public enum Step
{
    NONE = -1,
    DIALOGUE,
    SELECTION,
    SHOPPING,
    BATHROOM,
    SLEEP
}

[System.Serializable]
public class Situation
{
    public List<Action> actions;
    public string identifier;
    public float activation_time;
    public float duration;
    public int current_step = 0;

    Situation()
    {
        sequence = new List<System.Tuple<Step, JSONObject>>();
    }
    public List<System.Tuple<Step, JSONObject>> sequence;

    public bool Finished_Properly { get; set; }
    public List<DialogueManager.DialogueInfo> dialogues;
}
