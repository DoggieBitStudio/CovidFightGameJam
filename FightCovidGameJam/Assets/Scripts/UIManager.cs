using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class UIManager : MonoBehaviour
{
    //Dialogue objects
    public GameObject dialogue;
    public GameObject player_name;
    public GameObject npc_name;
    public GameObject time;

    //Dialogue texts
    Text dialogue_text;
    Text player_name_text;
    Text npc_name_text;
    Text time_text;

    enum DIALOGUE_STATE {NONE, TWEENING, ENDED};
    DIALOGUE_STATE state = DIALOGUE_STATE.NONE;

    float base_text_speed = 40.0f;

    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init();

        dialogue_text = dialogue.GetComponentInChildren<Text>();
        player_name_text = player_name.GetComponentInChildren<Text>();
        npc_name_text = npc_name.GetComponentInChildren<Text>();
        time_text = time.GetComponent<Text>();

        SetDialogueText("Chicos, voy a salir a comprar. Necesitais que os traiga alguna cosa de la tienda? Recordad que hay que ir a comprar como mucho tres veces por semana");
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

    void SetDialogueText(string text)
    {
        float speed = text.Length / base_text_speed;
        dialogue_text.text = "";
        dialogue_text.DOText(text, speed).OnComplete(EndedTextTween);
        state = DIALOGUE_STATE.TWEENING;
    }

    void SetTimeText(string time)
    {
        time_text.text = time;
    }

    void EndedTextTween()
    {
        state = DIALOGUE_STATE.ENDED;
    }
}
