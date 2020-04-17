using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int health = 5;
    public int positivism = 50;
    public int mask = 1;
    public bool infection = false;
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
        positivism += p;
        if (positivism < 0)
            positivism = 0;
    }

    public void AddHealth(int h)
    {
        health += h;
        if (health < 0)
            health = 0;
    }
}
