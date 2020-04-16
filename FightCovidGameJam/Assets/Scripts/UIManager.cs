using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class UIManager : MonoBehaviour
{
    //Dialogue objects
    public GameObject time;

    //Dialogue texts
    Text time_text;

    void SetTimeText(string time)
    {
        time_text.text = time;
    }
}
