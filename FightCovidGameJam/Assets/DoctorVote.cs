using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DoctorVote : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Accept()
    {
        transform.GetChild(3).gameObject.SetActive(true);
        transform.GetChild(3).GetComponent<Text>().DOText("Carmen", 1).OnComplete(OnSign).SetEase(Ease.Linear);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
    }
    public void Decline()
    {
        this.gameObject.SetActive(false);
        GameManager.instance.situations_manager.OnStepFinish();
    }

    public void OnSign()
    {
        Invoke("Disappear", 1.2f);
    }

    public void Disappear()
    {
        this.gameObject.SetActive(false);
        GameManager.instance.boolean_stats["Doctor_Out"] = true;
        GameManager.instance.situations_manager.OnStepFinish();
    }
}
