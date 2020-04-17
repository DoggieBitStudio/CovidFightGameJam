using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public uint health = 5;
    public uint positivism = 5;
    public uint mask = 1;
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
        }
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        situations_manager = GetComponent<SituationsManager>();
        dialogue_manager = GetComponent<DialogueManager>();
        ui_manager = GetComponent<UIManager>();
    }

    public void AdvanceTime()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
