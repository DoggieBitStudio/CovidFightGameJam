using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientManager : MonoBehaviour
{
    public List<GameObject> patient;
    public int maxCamillas = 5;
    public float previous_time = 0;
    public float current_time;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 1; i <= maxCamillas; ++i)
        {
            patient.Add(GameObject.Find("Camilla " + i));
        }
        
    }

    public void CheckTime(float time)
    {
        current_time += time;
        if(current_time - previous_time >= 1)
        {
            previous_time = current_time;
            for(int i = 0; i< maxCamillas;++i)
            {
                patient[i].GetComponent<PatientData>().AddHealth(-1);
            }
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        for (int i = 1; i <= maxCamillas; ++i)
        {
            patient.Add(GameObject.Find("Camilla " + i));
        }
    }
}
