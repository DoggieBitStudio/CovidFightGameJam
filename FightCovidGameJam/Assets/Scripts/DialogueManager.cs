using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    }

    //Dialogue objects
    public GameObject dialogue;
    public GameObject player_name;
    public GameObject npc_name;
    SituationsManager situations_manager;

    int current_dialog_index, current_situation_dialogs_size = 0;

    //Dialogue texts
    Text dialogue_text;
    Text player_name_text;
    Text npc_name_text;

    enum DIALOGUE_STATE { NONE, TWEENING, ENDED };
    DIALOGUE_STATE state = DIALOGUE_STATE.NONE;

    float base_text_speed = 40.0f;

    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init();

        dialogue_text = dialogue.GetComponentInChildren<Text>();
        player_name_text = player_name.GetComponentInChildren<Text>();
        npc_name_text = npc_name.GetComponentInChildren<Text>();

        situations_manager = GameManager.instance.GetComponent<SituationsManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
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
                    //SetDialogueText(next_text);
                    if (current_dialog_index >= current_situation_dialogs_size)
                    {
                        Finish();
                        state = DIALOGUE_STATE.NONE;
                    }
                    else
                    {
                        EndDialogue();
                        StartDialogue(situations_manager.current_situation.dialogues[current_dialog_index]);
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
        dialogue_text.text = "";
        dialogue_text.DOText(text, speed).OnComplete(EndedTextTween);
        state = DIALOGUE_STATE.TWEENING;
    }

    void EndedTextTween()
    {
        state = DIALOGUE_STATE.ENDED;
    }

    void StartDialogue(DialogueInfo d_info)
    {
        if (d_info.player)
        {
            player_name.SetActive(true);
            SetPlayerName(d_info.name);
        }
        else
        {
            SetNPCName(d_info.name);
            npc_name.SetActive(true);
        }

        current_situation_dialogs_size = situations_manager.current_situation.dialogues.Count;
        current_dialog_index++;
        dialogue.SetActive(true);
        SetDialogueText(d_info.text, d_info.speed);
    }

    void EndDialogue()
    {
        if (player_name.activeSelf)
            player_name.SetActive(false);
        else if(npc_name.activeSelf)
            npc_name.SetActive(false);
    }

    void Finish()
    {
        if (player_name.activeSelf)
            player_name.SetActive(false);
        else if (npc_name.activeSelf)
            npc_name.SetActive(false);

        dialogue.SetActive(false);
        current_situation_dialogs_size = 0;
        current_dialog_index = 0;
    }
}
