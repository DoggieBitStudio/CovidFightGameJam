using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Specialized;

[System.Serializable]
public enum Step_Type
{
    NONE = -1,
    DIALOGUE,
    SELECTION,
    SHOPPING,
    BATHROOM,
    SLEEP,
    DOCTOR_VOTE
}

[System.Serializable]
public struct Step
{
    public Step_Type step_type;
    public int next_step;
    public int index;

    public GameManager.Stat<int> int_requirement;
    public GameManager.Stat<bool> bool_requirement;
}

[System.Serializable]
public class Situation
{
    public List<Action> actions;
    public string identifier;
    public float activation_time;
    public float duration;
    public Step current_step;

    Situation()
    {
        sequence = new List<System.Tuple<Step, JSONObject>>();
    }
    public List<System.Tuple<Step, JSONObject>> sequence;

    public bool Finished_Properly { get; set; }
    public List<DialogueManager.DialogueInfo> dialogues;
}
