using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientData : MonoBehaviour
{
    public bool infected = false;
    public bool goodBye = false;
    public bool dead = false;
    public int health = 5;

    public void AddHealth(int h)
    {
        health += h;
        if (health == 0)
            KillPatient();
        else if (health >= 5)
            health = 5;
    }

    void KillPatient()
    {

    }
}
