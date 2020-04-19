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
    JULIAN
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
    public Text character_text;
    public Text day_text;
    public AudioSource audio_source;

    internal int carmen_day = 1;
    internal int julian_day = 1;
    public CHARACTER current_character = CHARACTER.CARMEN;

    public AudioClip new_day_sfx;

    bool show_stats = false;
    GUIStyle debug_style;

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

            debug_style = new GUIStyle();
            debug_style.fontSize = 22;
            debug_style.normal.textColor = Color.red;
            new_day = true;
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
    }

    public void AddHealth(int h)
    {
        if(instance.int_stats["Health"] > 0)
            instance.int_stats["Health"] += h;
    }

    public void LoadSceneFade(string name)
    {
        fade.DOFade(1.0f, 2.0f).OnComplete(()=>LoadScene(name));
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
        ui_opened = true;
        HideFade();
        situations_manager.OnLevelFinshedLoading(scene);
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
        character_text.DOFade(0.0f, 2.0f).OnComplete(situations_manager.StartSituation);
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
           
    }

    void NewDay()
    {
        switch (current_character)
        {
            case CHARACTER.CARMEN:
                character_text.text = "Carmen";
                day_text.text = "Día " + carmen_day + " de confinamiento";
                break;
            case CHARACTER.JULIAN:
                day_text.text = "Día " + julian_day + " de confinamiento";
                character_text.text = "Julian";
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
