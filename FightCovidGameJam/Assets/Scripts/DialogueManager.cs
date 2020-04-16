using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject myDialogueGO;
    public GameObject NPCDialogueGO;
    Text myDialogueText;
    Text NPCDialogueText;

    // Start is called before the first frame update
    void Start()
    {
        myDialogueText = myDialogueGO.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetText(string text)
    {
        myDialogueText.text = text;
    }
}
