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
        READ = 2,
        SLEEP_SOFA = 3,
        TALK_NEIGHBOUR = 4,
        TAKE_WALK = 5,
        VIDEOCALL = 6,
        BUY_ONLINE = 7,
        CRAFT_MASK = 8,
        WASH_MASK = 9,
        TRASH_OUT = 10,
        WATER_PLANTS = 11,
        DINNER = 12,
        GO_BATHROOM = 13,
        WASH_HANDS = 14,
        HYGIENE_GEL = 15
    }
    Actions currentAction = Actions.NONE;

    //Objects
    GameObject tv;
    GameObject sofa;
    GameObject shelf;
    GameObject couch;

    //Player
    public GameObject player;
    NavMeshAgent agent;

    //Timer
    bool firstAction = false;
    bool secondAction = false;

    // Start is called before the first frame update
    void Start()
    {
        agent = player.GetComponent<NavMeshAgent>();
        tv = GameObject.Find("Television");
        sofa = GameObject.Find("Sofa");
        shelf = GameObject.Find("Shelf");
        couch = GameObject.Find("Couch");
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
                        player.transform.LookAt(tv.transform);
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
                    if ((sofa.transform.position - player.transform.position).sqrMagnitude < 3 && !firstAction)
                    {
                        tv.GetComponent<AudioSource>().Play();
                        tv.transform.GetChild(0).gameObject.SetActive(true);
                        //DoSitAnimation
                        player.transform.LookAt(tv.transform);
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
            case Actions.READ:
                {
                    if ((shelf.transform.position - player.transform.position).sqrMagnitude < 3 && !firstAction)
                    {
                        tv.GetComponent<AudioSource>().Play();
                        tv.transform.GetChild(0).gameObject.SetActive(true);
                        //DoSitAnimation
                        player.transform.LookAt(tv.transform);
                        firstAction = true;
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
                agent.SetDestination(sofa.transform.position);
                break;
            case Actions.READ:
                agent.SetDestination(shelf.transform.position);
                break;
        }
    }
}
