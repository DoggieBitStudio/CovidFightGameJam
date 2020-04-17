using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    UIManager uiManager;

    public int health = 5;
    public int positivism = 5;
    public int mask = 1;
    public float time = 8.0f;

    public SituationsManager situations_manager;
    public DialogueManager dialogue_manager;
    public UIManager ui_manager;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            situations_manager = GetComponent<SituationsManager>();
            dialogue_manager = GetComponent<DialogueManager>();
            ui_manager = GetComponent<UIManager>();
        }
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void AdvanceTime(float t)
    {
        time += t;
        uiManager.SetTimeText(time);
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
