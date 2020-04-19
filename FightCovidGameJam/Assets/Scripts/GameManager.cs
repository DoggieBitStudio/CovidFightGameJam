using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public enum CHARACTER
{
    CARMEN,
    JULIO
}
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
    public Image fade;

    public bool ui_opened = false;

    public bool new_day = true;
    public bool change_character = false;
    public Text character_text;
    public Text day_text;
    public AudioSource audio_source;

    internal int carmen_day = 1;
    internal int julio_day = 8;
    public CHARACTER current_character = CHARACTER.CARMEN;

    public AudioClip new_day_sfx;

    bool show_stats = false;
    GUIStyle debug_style;

    public string prevScene;

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
            audio_source = GetComponent<AudioSource>();

            int_stats = new Dictionary<string, int>();
            boolean_stats = new Dictionary<string, bool>();

            int_stats.Add("Health", 5);
            int_stats.Add("Positivism", 50);

            boolean_stats.Add("Mask", true);
            boolean_stats.Add("Mask_Crafted", false);
            boolean_stats.Add("DoorClosed", false);
            boolean_stats.Add("Infection", false);
            boolean_stats.Add("Badly_Washed", false);
            boolean_stats.Add("Gel", false);
            boolean_stats.Add("Clap", false);
            boolean_stats.Add("Shop", false);
            boolean_stats.Add("Plant", false);
            boolean_stats.Add("Went_Out", false);
            boolean_stats.Add("Doctor_Out", false);
            boolean_stats.Add("More_Sick_People", false);

            boolean_stats.Add("End_Carmen_Depressed", false);
            boolean_stats.Add("End_Child_Live", false);
            boolean_stats.Add("End_Child_Die", false);


            debug_style = new GUIStyle();
            debug_style.fontSize = 22;
            debug_style.normal.textColor = Color.red;
            new_day = false;
        }
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1920, 1080, true);
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
        if (Input.GetKeyDown(KeyCode.F1))
        {
            AdvanceTime(1);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            show_stats = !show_stats;
        }
    }

    public void AddPositivism(int p)
    {
        instance.int_stats["Positivism"] += p;
        if (instance.int_stats["Positivism"] < 0)
            instance.int_stats["Positivism"] = 0;
        else if (instance.int_stats["Positivism"] > 100)
            instance.int_stats["Positivism"] = 100;

    }

    public void AddHealth(int h)
    {
        if (instance.int_stats["Health"] > 0)
            instance.int_stats["Health"] += h;
        else if(instance.int_stats["Health"] >= 5)
            instance.int_stats["Health"] = 5;
    }

    public void LoadSceneFade(string name)
    {
        prevScene = SceneManager.GetActiveScene().name;
        fade.DOFade(1.0f, 2.0f).OnComplete(()=>LoadScene(name));
    }

    public void DoInSceneFade(float time, float value)
    {
        ui_opened = true;
        if (value == 0.0f)
            fade.DOFade(value, time * 0.5f).OnComplete(() => OnEndFade());
        else
            fade.DOFade(value, time * 0.5f).OnComplete(() => DoInSceneFade(time, 0.0f));
    }
    public void OnEndFade()
    {
        ui_opened = false;
        situations_manager.OnStepFinish();
    }
    void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if(prevScene == "News")
        {
            new_day = true;
            action_manager.enabled = true;
            ui_manager.time_text.gameObject.transform.parent.gameObject.SetActive(true);
            ui_manager.mask_ui.gameObject.transform.parent.gameObject.SetActive(true);
        }
        ui_opened = true;
        HideFade();
    }



    internal void ResetTime()
    {
        time = 8;
        ui_manager.SetTimeText(time);

        boolean_stats["Plant"] = false;
    }

    void ShowDayText()
    {
        day_text.DOFade(1.0f, 2.0f).OnComplete(HidePrepTexts);
        audio_source.PlayOneShot(new_day_sfx);
    }

    void HidePrepTexts()
    {
        day_text.DOFade(0.0f, 2.0f);
        character_text.DOFade(0.0f, 2.0f).OnComplete(() => situations_manager.OnLevelFinshedLoading(SceneManager.GetActiveScene()));
        ui_opened = false;
    }

    void HideFade()
    {
        if (new_day)
        {
            fade.DOFade(0.0f, 2.0f).OnComplete(NewDay);
        }
        else
        {
            fade.DOFade(0.0f, 2.0f);
            ui_opened = false;
        }

        if (prevScene == "bathroom")
            situations_manager.OnStepFinish();        
    }         
   

    void NewDay()
    {
        switch (current_character)
        {
            case CHARACTER.CARMEN:
                character_text.text = "Carmen";
                day_text.text = "Día " + carmen_day + " de confinamiento";
                break;
            case CHARACTER.JULIO:
                day_text.text = "Día " + julio_day + " del estado de alarma";
                character_text.text = "Julio";
                break;
            default:
                break;
        }
        character_text.DOFade(1.0f, 2.0f).OnComplete(ShowDayText);

        new_day = false;
    }

    void OnGUI()
    {
        if(show_stats)
        {
            int y = 10;
            foreach (var item in int_stats)
            {
                GUI.Label(new Rect(Camera.main.pixelWidth-220, y, 200, 50), item.Key + ": " + item.Value.ToString(), debug_style);
                y += 30;
            }

            y = 10;
            foreach (var item in boolean_stats)
            {
                GUI.Label(new Rect(Camera.main.pixelWidth - 450, y, 200, 50), item.Key + ": " + item.Value.ToString(), debug_style);
                y += 30;
            }
        }

    }
}
