using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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

    //ActionValues
    float timePassed;
    int healthGained;
    int positivismGained;

    //Objects
    GameObject tv;
    GameObject sofa;
    GameObject shelf;
    GameObject couch;
    GameObject book;
    GameObject houseDoor;
    GameObject neighbourDoor;
    GameObject neighbour;
    GameObject smarthphone;
    GameObject smartphonePos;

    AudioSource houseDoorSource;

    //Player
    public GameObject player;
    NavMeshAgent agent;

    public AudioClip dramaticFx;

    //Timer
    bool firstAction = false;
    bool secondAction = false;
    public GameObject fadeToBlack;

    // Start is called before the first frame update
    void Start()
    {
        agent = player.GetComponent<NavMeshAgent>();
        tv = GameObject.Find("Television");
        sofa = GameObject.Find("Sofa");
        shelf = GameObject.Find("Estantería");
        couch = GameObject.Find("Sillón");
        book = GameObject.Find("Book");
        houseDoor = GameObject.Find("Puerta");
        houseDoorSource = houseDoor.GetComponent<AudioSource>();
        neighbourDoor = GameObject.Find("Puerta Vecino");
        smarthphone = GameObject.Find("Móvil");
        smartphonePos = GameObject.Find("SmartphonePos");
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
                        player.GetComponentInChildren<Animator>().Play("Sitting");
                        player.transform.LookAt(tv.transform);
                        firstAction = true;
                    }
                    else if (firstAction && !tv.GetComponent<AudioSource>().isPlaying)
                    {
                        tv.transform.GetChild(0).gameObject.SetActive(false);
                        currentAction = Actions.NONE;
                        firstAction = false;
                        FinalizeAction();
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
                        FinalizeAction();
                    }
                }
                break;
            case Actions.READ:
                {
                    if ((shelf.transform.position - player.transform.position).sqrMagnitude < 3 && !firstAction && !secondAction)
                    {
                        book.SetActive(false);
                        agent.SetDestination(couch.transform.position);
                        firstAction = true;
                    }
                    else if ((couch.transform.position - player.transform.position).sqrMagnitude < 3 && firstAction)
                    {
                        //Book
                        book.SetActive(true);
                        book.transform.position = player.transform.position;
                        //Sit in couch
                        //When finished reading
                        agent.SetDestination(shelf.transform.position);
                        book.SetActive(false);
                        secondAction = true;
                    }
                    else if ((shelf.transform.position - player.transform.position).sqrMagnitude < 3 && firstAction && secondAction)
                    {
                        book.transform.position = shelf.transform.position;
                        currentAction = Actions.NONE;
                        firstAction = false;
                        secondAction = false;
                        FinalizeAction();
                    }
                }
                break;
            case Actions.SLEEP_SOFA:
                {
                    if((sofa.transform.position - player.transform.position).sqrMagnitude < 3 && !firstAction)
                    {
                        // Estirarse en el sofa
                        sofa.GetComponent<AudioSource>().Play();
                        firstAction = true;
                    }
                    else if(firstAction && !sofa.GetComponent<AudioSource>().isPlaying)
                    {
                        //Get Up of sofa
                        firstAction = false;
                        currentAction = Actions.NONE;
                        FinalizeAction();
                    }
                }
                break;
            case Actions.TALK_NEIGHBOUR:
                {
                    if ((houseDoor.transform.position - player.transform.position).sqrMagnitude < 3 && !firstAction)
                    {
                        //OpenDoor
                        houseDoorSource.Play();
                        //When door is opened
                        agent.SetDestination(neighbourDoor.transform.position);
                        firstAction = true;
                    }
                    else if((neighbourDoor.transform.position - player.transform.position).sqrMagnitude < 3 && firstAction)
                    {
                        neighbourDoor.GetComponent<AudioSource>().Play();
                        //Vecina sale, hablan
                        //When finished talking, return house
                        agent.SetDestination(houseDoor.transform.position);
                        secondAction = true;
                    }
                    else if((houseDoor.transform.position - player.transform.position).sqrMagnitude < 3 && firstAction && secondAction)
                    {
                        //Close door
                        currentAction = Actions.NONE;
                        firstAction = false;
                        secondAction = false;
                        FinalizeAction();
                    }

                }
                break;
            case Actions.TAKE_WALK:
                {
                    if((houseDoor.transform.position - player.transform.position).sqrMagnitude < 3 && !firstAction)
                    {
                        Color col = fadeToBlack.GetComponent<Image>().color;
                        Debug.Log(col.a);
                        col.a += (float)0.5 * Time.deltaTime;
                        fadeToBlack.GetComponent<Image>().color = col;

                        if (col.a >= 1)
                        {
                            firstAction = true;
                            houseDoorSource.PlayOneShot(dramaticFx);
                        }
                    }
                    else if(firstAction && !houseDoorSource.isPlaying && !secondAction)
                    {
                        secondAction = true;
                        houseDoorSource.Play();
                    }
                    else if(firstAction  && secondAction)
                    {
                        Color col = fadeToBlack.GetComponent<Image>().color;
                        col.a -= (float)0.5 * Time.deltaTime;
                        fadeToBlack.GetComponent<Image>().color = col;

                        if (col.a <= 0)
                        {
                            fadeToBlack.SetActive(false);
                            firstAction = false;
                            secondAction = false;
                            currentAction = Actions.NONE;
                            FinalizeAction();
                        }
                    }
                }
                break;
            case Actions.VIDEOCALL:
                {
                    if ((smarthphone.transform.position - player.transform.position).sqrMagnitude < 3 && !firstAction)
                    {
                        agent.SetDestination(couch.transform.position);
                        smarthphone.SetActive(false);
                        firstAction = true;
                    }
                    else if ((couch.transform.position - player.transform.position).sqrMagnitude < 3 && firstAction && !secondAction)
                    {
                        //Smartphone
                        smarthphone.SetActive(true);
                        smarthphone.transform.position = player.transform.position;
                        //Sit in couch
                        smarthphone.GetComponent<AudioSource>().Play();
                        secondAction = true;
                    }
                    else if(firstAction && secondAction && !smarthphone.GetComponent<AudioSource>().isPlaying)
                    {
                        agent.SetDestination(smartphonePos.transform.position);
                        smarthphone.SetActive(false);
                        if ((smartphonePos.transform.position - player.transform.position).sqrMagnitude < 3)
                        {
                            smarthphone.SetActive(true);
                            smarthphone.transform.position = smartphonePos.transform.position;
                            currentAction = Actions.NONE;
                            firstAction = false;
                            secondAction = false;
                            FinalizeAction();
                        }
                    }
                }
                break;
        }
    }

    public void DoAction(int action, double time, int health, int positivism)
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
            case Actions.SLEEP_SOFA:
                agent.SetDestination(sofa.transform.position);
                break;
            case Actions.TALK_NEIGHBOUR:
                agent.SetDestination(houseDoor.transform.position);
                break;
            case Actions.TAKE_WALK:
                fadeToBlack.SetActive(true);
                houseDoorSource.Play();
                break;
            case Actions.VIDEOCALL:
                agent.SetDestination(smarthphone.transform.position);
                break;
        }
        timePassed = (float)time;
        healthGained = health;
        positivismGained = positivism;

    }

    void FinalizeAction()
    {
        GameManager.instance.AdvanceTime(timePassed);

        GameManager.instance.AddPositivism(positivismGained);
        GameManager.instance.AddHealth(healthGained);
    }
}
