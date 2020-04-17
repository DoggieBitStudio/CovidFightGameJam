using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActionManager : MonoBehaviour
{
    enum Actions
    {
        NONE = -1,
        TELEVISION_WATCH = 0,
        TELEVISION_EXERCISE = 1,
        READ = 2
    }
    Actions currentAction = Actions.NONE;

    //Objects
    GameObject tv;
    GameObject sofa;

    //Player
    public GameObject player;
    NavMeshAgent agent;

    //Timer
    bool firstAction = false;

    // Start is called before the first frame update
    void Start()
    {
        agent = player.GetComponent<NavMeshAgent>();
        tv = GameObject.Find("Television");
        sofa = GameObject.Find("Sofa");
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentAction)
        {
            case Actions.TELEVISION_WATCH:
                {
                    if ((sofa.transform.position - player.transform.position).sqrMagnitude < 5 && !firstAction)
                    {
                        tv.GetComponent<AudioSource>().Play();
                        tv.transform.GetChild(0).gameObject.SetActive(true);
                        //DoSitAnimation
                        //LookTorwardsTv
                        firstAction = true;
                    }
                    else if (firstAction && !tv.GetComponent<AudioSource>().isPlaying)
                    {
                        tv.transform.GetChild(0).gameObject.SetActive(false);
                        currentAction = Actions.NONE;
                        firstAction = false;
                    }
                }
                break;
            case Actions.TELEVISION_EXERCISE:
                {
                    if ((sofa.transform.position - player.transform.position).sqrMagnitude < 5 && !firstAction)
                    {
                        tv.GetComponent<AudioSource>().Play();
                        tv.transform.GetChild(0).gameObject.SetActive(true);
                        //DoSitAnimation
                        //LookTorwardsTv
                        firstAction = true;
                    }
                    else if (firstAction && !tv.GetComponent<AudioSource>().isPlaying)
                    {
                        tv.transform.GetChild(0).gameObject.SetActive(false);
                        currentAction = Actions.NONE;
                        firstAction = false;
                    }
                }
                break;
        }
    }

    public void DoAction(int action)
    {
        currentAction = (Actions)action;
        switch(currentAction)
        {
            case Actions.TELEVISION_WATCH:
                agent.SetDestination(sofa.transform.position);
                break;
            case Actions.TELEVISION_EXERCISE:
                break;
            case Actions.READ:
                break;
        }
    }
}
