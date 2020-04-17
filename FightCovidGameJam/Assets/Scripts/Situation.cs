using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Situation
{
    /*public struct Action
    {
        public string name;
        public float time;
        bool completed;
    }*/

    public List<string> actions;
    public string identifier;
    public float activation_time;

    public bool Finished_Properly { get; set; }
    public List<DialogueManager.DialogueInfo> dialogues;
}
