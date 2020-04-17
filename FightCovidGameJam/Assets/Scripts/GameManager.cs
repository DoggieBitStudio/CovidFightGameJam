using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [System.Serializable]
    public struct Stat<T>
    {
        public string stat;
        public T value;

        public Stat(string stat, T value)
        {
            this.stat = stat;
            this.value = value;
        }

    }
    public Dictionary<string, int> int_stats;
    public Dictionary<string, bool> boolean_stats;

    public float time = 8.0f;

    public SituationsManager situations_manager;
    public DialogueManager dialogue_manager;
    public UIManager ui_manager;
    public ActionManager action_manager;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            situations_manager = GetComponent<SituationsManager>();
            dialogue_manager = GetComponent<DialogueManager>();
            ui_manager = GetComponent<UIManager>();

            int_stats = new Dictionary<string, int>();
            boolean_stats = new Dictionary<string, bool>();

            int_stats.Add("Health", 5);
            int_stats.Add("Positivism", 50);

            boolean_stats.Add("Mask", true);
            boolean_stats.Add("Mask_Crafted", false);
            boolean_stats.Add("Door", false);
            boolean_stats.Add("Infection", false);
            boolean_stats.Add("Badly_Washed", false);
            boolean_stats.Add("Gel", false);
            boolean_stats.Add("Clap", false);
            boolean_stats.Add("Shop", false);
            boolean_stats.Add("Plant", false);
            boolean_stats.Add("Went_Out", false);

            action_manager = GetComponent<ActionManager>();
        }
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        ui_manager.SetTimeText(time);
    }

    public void AdvanceTime(float t)
    {
        time += t;
        ui_manager.SetTimeText(time);

        situations_manager.StartNextSituation();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPositivism(int p)
    {
        instance.int_stats["Positivism"] += p;
    }

    public void AddHealth(int h)
    {
        instance.int_stats["Health"] += h;
    }
}
