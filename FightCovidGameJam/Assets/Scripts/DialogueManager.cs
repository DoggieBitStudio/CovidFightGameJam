﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class DialogueManager : MonoBehaviour
{
    [System.Serializable]
    public struct DialogueInfo
    {
        public string text;
        public bool player;
        public string name;
        public string animation;
        public float speed;
        public string sound;
    }

    //Dialogue objects
    public GameObject dialogue;
    public GameObject player_name;
    public GameObject npc_name;

    //Dialogue texts
    Text dialogue_text;
    Text player_name_text;
    Text npc_name_text;

    public Transform player_model_position, npc_model_position;

    //Dialogue Characters
    [System.Serializable]
    public struct CharacterModel
    {
        public string name;
        public GameObject prefab;
    }

    public CharacterModel[] dialogue_models;

    GameObject FindPrefab(string _name)
    {
        foreach (CharacterModel cm in dialogue_models)
        {
            if (cm.name == _name)
            {
                return cm.prefab;
            }
        }
        return null;
    }

    enum DIALOGUE_STATE { NONE, TWEENING, ENDED };
    DIALOGUE_STATE state = DIALOGUE_STATE.NONE;

    // Start is called before the first frame update
    void Awake()
    {
        DOTween.Init();

        dialogue_text = dialogue.GetComponentInChildren<Text>();
        player_name_text = player_name.GetComponentInChildren<Text>();
        npc_name_text = npc_name.GetComponentInChildren<Text>();
    }

    private void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
#if UNITY_STANDALONE_WIN
        HandleStandaloneUpdate();
#endif

#if UNITY_ANDROID
        HandlePhoneUpdate();
#endif
    }

    void HandleStandaloneUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            switch (state)
            {
                case DIALOGUE_STATE.NONE:
                    break;
                case DIALOGUE_STATE.TWEENING:
                    dialogue_text.DOComplete();
                    state = DIALOGUE_STATE.ENDED;
                    break;
                case DIALOGUE_STATE.ENDED:
                    if (!GameManager.instance.situations_manager.IsNextStepSelection())
                        EndDialogue();
                    else
                    {
                        GameManager.instance.situations_manager.OnStepFinish();
                        state = DIALOGUE_STATE.NONE;
                        GameManager.instance.audio_source.Stop();
                    }
                    break;
                default:
                    break;
            }
        }
    }

    void HandlePhoneUpdate()
    {
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended)
        {
            switch (state)
            {
                case DIALOGUE_STATE.NONE:
                    break;
                case DIALOGUE_STATE.TWEENING:
                    dialogue_text.DOComplete();
                    state = DIALOGUE_STATE.ENDED;
                    break;
                case DIALOGUE_STATE.ENDED:
                    if (!GameManager.instance.situations_manager.IsNextStepSelection())
                        EndDialogue();
                    else
                    {
                        GameManager.instance.situations_manager.OnStepFinish();
                        state = DIALOGUE_STATE.NONE;
                        GameManager.instance.audio_source.Stop();
                    }            
                    break;
                default:
                    break;
            }
        }
    }

    void SetPlayerName(string name)
    {
        player_name_text.text = name;
    }
  

    void SetNPCName(string name)
    {
        npc_name_text.text = name;
    }

    void SetDialogueText(string text, float d_speed)
    {
        float speed = text.Length / d_speed;

        if (text.Contains("[anger]"))
        {
            dialogue.transform.DOShakePosition(speed, 1, 40, 0, false, false);
            text = text.Replace("[anger]", "");
        }

        dialogue_text.text = "";
        dialogue_text.DOText(text, speed).OnComplete(EndedTextTween).SetEase(Ease.Linear);

        state = DIALOGUE_STATE.TWEENING;
    }

    void EndedTextTween()
    {
        state = DIALOGUE_STATE.ENDED;
    }

    public void StartDialogue(JSONObject d_json)
    {
        DialogueInfo d_info = JsonUtility.FromJson<DialogueInfo>(d_json.ToString());

        if(d_info.sound != null)
        {
            GameManager.instance.audio_source.PlayOneShot(Resources.Load<AudioClip>("Sounds/" + d_info.sound));
        }
            

        if (d_info.player)
        {
            player_name.SetActive(true);
            SetPlayerName(d_info.name);
            GameObject model_dialogue = Instantiate(FindPrefab(d_info.name), player_model_position, false);
            if (d_info.animation != null)
                model_dialogue.GetComponent<Animator>().Play(d_info.animation);
        }
        else
        {
            npc_name.SetActive(true);
            SetNPCName(d_info.name);
            GameObject model_dialogue = Instantiate(FindPrefab(d_info.name), npc_model_position, false);
            if (d_info.animation != null)
                model_dialogue.GetComponent<Animator>().Play(d_info.animation);
        }

        GameManager.instance.ui_opened = true;
        dialogue.SetActive(true);
        SetDialogueText(d_info.text, d_info.speed);
    }

    public void EndDialogue()
    {
        if (player_name.activeSelf)
            player_name.SetActive(false);
        else if (npc_name.activeSelf)
            npc_name.SetActive(false);

        dialogue.SetActive(false);
        RemoveModelPrefabs();

        state = DIALOGUE_STATE.NONE;

        GameManager.instance.ui_opened = false;
        GameManager.instance.situations_manager.OnStepFinish();
    }

    void RemoveModelPrefabs()
    {
        if (player_model_position.childCount > 0)
        {
            Destroy(player_model_position.GetChild(0).gameObject);
        }       
        else if (npc_model_position.childCount > 0)
            Destroy(npc_model_position.GetChild(0).gameObject);
    }

    private void OnLevelWasLoaded(int level)
    {
        if(SceneManager.GetActiveScene().name == "Main" || SceneManager.GetActiveScene().name == "HospitalUpdated")
        {
            player_model_position = Camera.main.transform.GetChild(0);
            npc_model_position = Camera.main.transform.GetChild(1);
        }
    }
}
