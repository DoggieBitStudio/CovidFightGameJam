using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Specialized;

[System.Serializable]
public class Situation
{
    public List<Action> actions;
    public string identifier;
    public float activation_time;

    public int current_step = 0;

    Situation()
    {
        sequence = new List<System.Tuple<PacketType, JSONObject>>();
    }

    public enum PacketType{
        NONE = -1,
        DIALOGUE,
        SELECTION,
        ACTION
    }
    public List<System.Tuple<PacketType, JSONObject>> sequence;

    public bool Finished_Properly { get; set; }
    public List<DialogueManager.DialogueInfo> dialogues;
}
