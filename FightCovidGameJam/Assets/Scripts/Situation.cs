using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Situation
{
    public List<Action> actions;
    public string identifier;
    public float activation_time;

    public enum PacketType{
        NONE = -1,
        DIALOGUE,
        SELECTION,
        ACTION
    }
    public Dictionary<PacketType, JSONObject> sequence;

    public bool Finished_Properly { get; set; }
    public List<DialogueManager.DialogueInfo> dialogues;
}
