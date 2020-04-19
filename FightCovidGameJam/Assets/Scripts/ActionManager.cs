using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        WASH_MASK_RIGHT = 9,
        TRASH_OUT = 10,
        WATER_PLANTS = 11,
        DINNER = 12,
        GO_BATHROOM = 13,
        WASH_HANDS = 14,
        HYGIENE_GEL = 15,
        WASH_MASK_WRONG = 16,
        TEST_PATIENTS = 17,
        GIVE_MEDICAMENTS = 18,
        RCP = 19,
        PATIENT_DEATH = 20,
        YOUTUBE_VIDEO = 21,
        CLAP_VIDEO = 22

    }
    Actions currentAction = Actions.NONE;

    //ActionValues
    float timePassed;
    int healthGained;
    int positivismGained;
    public bool isDoingAction;

    //Objects
    GameObject tv;
    GameObject sofa;
    GameObject shelf;
    GameObject couch;
    GameObject chair;
    GameObject book;
    GameObject bookPos;
    GameObject houseDoor;
    GameObject neighbourDoor;
    GameObject neighbour;
    GameObject smartphone;
    GameObject smartphonePos;
    GameObject bathroomDoor;
    GameObject swegingBox;
    GameObject washingMachine;
    GameObject plant;
    GameObject kitchen;
    GameObject hygieneGel;
    GameObject table;

    GameObject medicalCarrito;
    GameObject smartphoneMed;

    AudioSource houseDoorSource;

    //Player
    public GameObject player;
    public NavMeshAgent agent;
    public Animator animator;

    public AudioClip dramaticFx;
    public AudioClip openAppFx;
    public AudioClip buyOnlineFx;
    public AudioClip flushFx;
    public AudioClip danceMusic;

    //Timer
    bool firstAction = false;
    bool secondAction = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("MariCarmen");
        agent = player.GetComponent<NavMeshAgent>();
        if (SceneManager.GetActiveScene().name == "Main")
        {
            animator = player.GetComponentInChildren<Animator>();
            tv = GameObject.Find("Television");
            sofa = GameObject.Find("Sofa");
            shelf = GameObject.Find("Estantería");
            couch = GameObject.Find("Sillón");
            book = GameObject.Find("Book");
            bookPos = GameObject.Find("BookPos");
            houseDoor = GameObject.Find("Puerta");
            houseDoorSource = houseDoor.GetComponent<AudioSource>();
            neighbourDoor = GameObject.Find("Puerta Vecino");
            smartphone = GameObject.Find("Móvil");
            smartphonePos = GameObject.Find("SmartphonePos");
            bathroomDoor = GameObject.Find("Lavabo");
            chair = GameObject.Find("Silla");
            swegingBox = GameObject.Find("Costurero");
            washingMachine = GameObject.Find("Lavadora");
            kitchen = GameObject.Find("Kitchen");
            plant = GameObject.Find("Planta");
            hygieneGel = GameObject.Find("Gel Desinfectante");
            table = GameObject.Find("Mesa");
        }
        else if(SceneManager.GetActiveScene().name == "HospitalUpdated")
        {
            medicalCarrito = GameObject.Find("MedicalCarrito");
            smartphoneMed = GameObject.Find("Móvil Medico");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(animator)
        {
            animator.SetFloat("Positivism", GameManager.instance.int_stats["Positivism"]);
        }
        switch(currentAction)
        {
            case Actions.TELEVISION_WATCH:
                {
                    if ((sofa.transform.position - player.transform.position).sqrMagnitude < 3 && agent.remainingDistance == 0 && !firstAction)
                    {
                        tv.GetComponent<AudioSource>().Play();
                        tv.transform.GetChild(0).gameObject.SetActive(true);
                        animator.Play("Sitting");
                        player.transform.LookAt(tv.transform);
                        firstAction = true;
                    }
                    else if (firstAction && !tv.GetComponent<AudioSource>().isPlaying)
                    {
                        tv.transform.GetChild(0).gameObject.SetActive(false);
                        FinalizeAction();
                    }
                }
                break;
            case Actions.TELEVISION_EXERCISE:
                {
                    if ((tv.transform.position - player.transform.position).sqrMagnitude < 3 && agent.remainingDistance == 0 && !firstAction)
                    {
                        tv.GetComponent<AudioSource>().PlayOneShot(danceMusic);
                        tv.transform.GetChild(0).gameObject.SetActive(true);
                        animator.Play("Twist Dance");
                        player.transform.LookAt(tv.transform);
                        firstAction = true;
                    }
                    else if (firstAction && !tv.GetComponent<AudioSource>().isPlaying)
                    {
                        tv.transform.GetChild(0).gameObject.SetActive(false);
                        FinalizeAction();
                    }
                }
                break;
            case Actions.READ:
                {
                    if ((shelf.transform.position - player.transform.position).sqrMagnitude < 3 && agent.remainingDistance == 0 &&  !firstAction && !secondAction)
                    {
                        book.SetActive(false);
                        agent.SetDestination(couch.transform.position);
                        firstAction = true;
                    }
                    else if ((couch.transform.position - player.transform.position).sqrMagnitude < 3 && agent.remainingDistance == 0 && firstAction && !secondAction)
                    {
                        //Book
                        book.SetActive(true);
                        book.transform.position = player.transform.position;
                        book.GetComponent<AudioSource>().Play();
                        animator.Play("Read");
                        player.transform.LookAt(table.transform.position);
                        secondAction = true;
                    }
                    else if(firstAction && secondAction && !book.GetComponent<AudioSource>().isPlaying && book.active)
                    {
                        agent.SetDestination(shelf.transform.position);
                        book.SetActive(false);
                    }
                    else if ((shelf.transform.position - player.transform.position).sqrMagnitude < 3 && agent.remainingDistance == 0 && firstAction && secondAction)
                    {
                        book.transform.position = bookPos.transform.position;
                        book.SetActive(true);
                        FinalizeAction();
                    }
                }
                break;
            case Actions.SLEEP_SOFA:
                {
                    if((sofa.transform.position - player.transform.position).sqrMagnitude < 3 && !firstAction)
                    {
                        animator.Play("Lie Down");
                        sofa.GetComponent<AudioSource>().Play();
                        firstAction = true;
                    }
                    else if(firstAction && !sofa.GetComponent<AudioSource>().isPlaying)
                    {
                        animator.Play("Idle");
                        FinalizeAction();
                    }
                }
                break;
            case Actions.TALK_NEIGHBOUR:
                {
                    if ((houseDoor.transform.position - player.transform.position).sqrMagnitude < 3 && !firstAction)
                    {
                        Color col = GameManager.instance.fade.color;
                        col.a += (float)0.5 * Time.deltaTime;
                        GameManager.instance.fade.color = col;

                        if (col.a >= 1)
                        {
                            firstAction = true;
                            houseDoorSource.PlayOneShot(dramaticFx);
                        }
                    }
                    else if(!houseDoorSource.isPlaying && firstAction)
                    {
                        Color col = GameManager.instance.fade.color;
                        col.a -= (float)0.5 * Time.deltaTime;
                        GameManager.instance.fade.color = col;

                        if (col.a <= 0)
                        {
                            if (GameManager.instance.int_stats["Health"] <= 0)
                            {
                                GameManager.instance.boolean_stats["Infection"] = true;
                            }
                            GameManager.instance.boolean_stats["Went_Out"] = true;
                            FinalizeAction();
                        }
                    }

                }
                break;
            case Actions.TAKE_WALK:
                {
                    if((houseDoor.transform.position - player.transform.position).sqrMagnitude < 3 && !firstAction)
                    {
                        Color col = GameManager.instance.fade.color;
                        col.a += (float)0.5 * Time.deltaTime;
                        GameManager.instance.fade.color = col;

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
                        Color col = GameManager.instance.fade.color;
                        col.a -= (float)0.5 * Time.deltaTime;
                        GameManager.instance.fade.color = col;

                        if (col.a <= 0)
                        {
                            GameManager.instance.boolean_stats["Went_Out"] = true;
                            FinalizeAction();
                        }
                    }
                }
                break;
            case Actions.VIDEOCALL:
                {
                    if ((smartphone.transform.position - player.transform.position).sqrMagnitude < 3 && !firstAction)
                    {
                        agent.SetDestination(chair.transform.position);
                        smartphone.SetActive(false);
                        firstAction = true;
                    }
                    else if ((chair.transform.position - player.transform.position).sqrMagnitude < 3 && firstAction && !secondAction)
                    {
                        //Smartphone
                        smartphone.SetActive(true);
                        smartphone.transform.position = player.transform.position;
                        smartphone.GetComponent<AudioSource>().Play();
                        secondAction = true;
                    }
                    else if(firstAction && secondAction && !smartphone.GetComponent<AudioSource>().isPlaying)
                    {
                        agent.SetDestination(smartphonePos.transform.position);
                        smartphone.SetActive(false);
                        if ((smartphonePos.transform.position - player.transform.position).sqrMagnitude < 3)
                        {
                            smartphone.SetActive(true);
                            smartphone.transform.position = smartphonePos.transform.position;
                            FinalizeAction();
                        }
                    }
                }
                break;
            case Actions.BUY_ONLINE:
                if ((smartphone.transform.position - player.transform.position).sqrMagnitude < 3 && !firstAction)
                {
                    smartphone.SetActive(false);
                    agent.SetDestination(chair.transform.position);
                    firstAction = true;
                }
                else if((chair.transform.position - player.transform.position).sqrMagnitude < 3 && firstAction && !secondAction)
                {
                    smartphone.GetComponent<AudioSource>().PlayOneShot(openAppFx);
                    smartphone.SetActive(true);
                    smartphone.transform.position = player.transform.position;
                    //Open minigame buy online
                    //Do action buying online
                    //When finished action
                    secondAction = true;
                    smartphone.GetComponent<AudioSource>().PlayOneShot(buyOnlineFx);
                }
                else if (firstAction && secondAction && !smartphone.GetComponent<AudioSource>().isPlaying)
                {
                    agent.SetDestination(smartphonePos.transform.position);
                    smartphone.SetActive(false);
                    if ((smartphonePos.transform.position - player.transform.position).sqrMagnitude < 3)
                    {
                        smartphone.SetActive(true);
                        smartphone.transform.position = smartphonePos.transform.position;
                        GameManager.instance.boolean_stats["Shop"] = true;
                        GameManager.instance.boolean_stats["Went_Out"] = true;
                        FinalizeAction();
                    }
                }
                break;
            case Actions.CRAFT_MASK:
                if ((bathroomDoor.transform.position - player.transform.position).sqrMagnitude < 5 && !firstAction)
                {
                    GameManager.instance.fade.gameObject.SetActive(true);
                    Color col = GameManager.instance.fade.color;
                    col.a += (float)0.5 * Time.deltaTime;
                    GameManager.instance.fade.color = col;

                    if (col.a >= 1)
                    {
                        firstAction = true;
                        bathroomDoor.GetComponent<AudioSource>().Play();
                    }
                }
                else if (firstAction && !bathroomDoor.GetComponent<AudioSource>().isPlaying && !secondAction)
                {
                    Color col = GameManager.instance.fade.color;
                    col.a -= (float)0.5 * Time.deltaTime;
                    GameManager.instance.fade.color = col;

                    if (col.a <= 0)
                    {
                        GameManager.instance.fade.gameObject.SetActive(false);
                        secondAction = true;
                        agent.SetDestination(swegingBox.transform.position);
                    }
                }
                else if ((swegingBox.transform.position - player.transform.position).sqrMagnitude < 3 && firstAction && secondAction)
                {
                    firstAction = false;
                    swegingBox.GetComponent<AudioSource>().Play();
                    GameManager.instance.boolean_stats["Mask_Crafted"] = true;
                    GameManager.instance.boolean_stats["Mask"] = true;
                    swegingBox.tag = "Untagged";
                    washingMachine.tag = "Selectable";
                    FinalizeAction();
                }
                break;
            case Actions.WASH_MASK_RIGHT:
                if((washingMachine.transform.position - player.transform.position).sqrMagnitude < 5 && !firstAction)
                {
                    firstAction = true;
                    washingMachine.GetComponent<AudioSource>().Play();
                }
                else if(firstAction && !washingMachine.GetComponent<AudioSource>().isPlaying)
                {
                    washingMachine.tag = "Untagged";
                    GameManager.instance.boolean_stats["Mask"] = true;
                    FinalizeAction();
                }
                break;
            case Actions.WASH_MASK_WRONG:
                if ((washingMachine.transform.position - player.transform.position).sqrMagnitude < 5 && !firstAction)
                {
                    firstAction = true;
                    washingMachine.GetComponent<AudioSource>().Play();
                }
                else if (firstAction && !washingMachine.GetComponent<AudioSource>().isPlaying)
                {
                    washingMachine.tag = "Untagged";
                    GameManager.instance.boolean_stats["Mask"] = true;
                    GameManager.instance.boolean_stats["Badly_Wash"] = true;
                    FinalizeAction();
                }
                break;
            case Actions.TRASH_OUT:
                {
                    if ((houseDoor.transform.position - player.transform.position).sqrMagnitude < 3 && !firstAction)
                    {
                        //OpenDoor
                        Color col = GameManager.instance.fade.color;
                        col.a += (float)0.5 * Time.deltaTime;
                        GameManager.instance.fade.color = col;

                        if (col.a >= 1)
                        {
                            firstAction = true;
                            houseDoorSource.Play();
                        }
                    }
                    else if (firstAction && !houseDoorSource.isPlaying)
                    {
                        Color col = GameManager.instance.fade.color;
                        col.a -= (float)0.5 * Time.deltaTime;
                        GameManager.instance.fade.color = col;

                        if (col.a <= 0)
                        {
                            GameManager.instance.boolean_stats["Went_Out"] = true;
                            FinalizeAction();
                        }
                    }

                }
                break;
            case Actions.WATER_PLANTS:
                {
                    if ((plant.transform.position - player.transform.position).sqrMagnitude < 3 && !firstAction)
                    {
                        //Water plants animation
                        firstAction = true;
                        plant.GetComponent<AudioSource>().Play();
                    }
                    else if(firstAction /*&& player animation is finished*/)
                    {
                        GameManager.instance.boolean_stats["Plant"] = true;
                        plant.tag = "Untagged";
                        FinalizeAction();
                    }
                }
                break;
            case Actions.DINNER:
                break;
            case Actions.GO_BATHROOM:
                {
                    if ((bathroomDoor.transform.position - player.transform.position).sqrMagnitude < 3 && !firstAction)
                    {
                        Color col = GameManager.instance.fade.color;
                        col.a += (float)0.5 * Time.deltaTime;
                        GameManager.instance.fade.color = col;

                        if (col.a >= 1)
                        {
                            firstAction = true;
                            bathroomDoor.GetComponent<AudioSource>().PlayOneShot(flushFx);
                        }
                    }
                    else if(firstAction && !bathroomDoor.GetComponent<AudioSource>().isPlaying)
                    {
                        Color col = GameManager.instance.fade.color;
                        col.a -= (float)0.5 * Time.deltaTime;
                        GameManager.instance.fade.color = col;

                        if (col.a <= 0)
                        {
                            FinalizeAction();
                        }
                    }

                }
                break;
            case Actions.WASH_HANDS:
                if ((bathroomDoor.transform.position - player.transform.position).sqrMagnitude < 3 && !firstAction)
                {
                    Color col = GameManager.instance.fade.color;
                    col.a += (float)0.5 * Time.deltaTime;
                    GameManager.instance.fade.color = col;

                    if (col.a >= 1)
                    {
                        firstAction = true;
                        bathroomDoor.GetComponent<AudioSource>().Play();
                    }
                }
                else if (firstAction && !bathroomDoor.GetComponent<AudioSource>().isPlaying)
                {
                    Color col = GameManager.instance.fade.color;
                    col.a -= (float)0.5 * Time.deltaTime;
                    GameManager.instance.fade.color = col;

                    if (col.a <= 0)
                    {
                        FinalizeAction();
                    }
                }
                break;
            case Actions.HYGIENE_GEL:
                if ((hygieneGel.transform.position - player.transform.position).sqrMagnitude < 3 && !firstAction)
                {
                    hygieneGel.GetComponent<AudioSource>().Play();
                    FinalizeAction();
                }
                break;
            case Actions.TEST_PATIENTS:
                { }
                break;
            case Actions.GIVE_MEDICAMENTS:
                { }
                break;
            case Actions.RCP:
                { }
                break;
            case Actions.PATIENT_DEATH:
                { }
                break;
            case Actions.YOUTUBE_VIDEO:
                { }
                break;
            case Actions.CLAP_VIDEO:
                { }
                break;
        }
    }

    public void DoAction(int action, double time, int health, int positivism)
    {
        currentAction = (Actions)action;
        isDoingAction = true;
        switch(currentAction)
        {
            case Actions.TELEVISION_WATCH:
                agent.SetDestination(sofa.transform.position);
                break;
            case Actions.TELEVISION_EXERCISE:
                agent.SetDestination(tv.transform.position);
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
                houseDoorSource.Play();
                break;
            case Actions.VIDEOCALL:
                agent.SetDestination(smartphone.transform.position);
                break;
            case Actions.BUY_ONLINE:
                agent.SetDestination(smartphone.transform.position);
                break;
            case Actions.CRAFT_MASK:
                agent.SetDestination(bathroomDoor.transform.position);
                break;
            case Actions.WASH_MASK_RIGHT:
                agent.SetDestination(washingMachine.transform.position);
                break;
            case Actions.WASH_MASK_WRONG:
                agent.SetDestination(washingMachine.transform.position);
                break;
            case Actions.TRASH_OUT:
                agent.SetDestination(houseDoor.transform.position);
                break;
            case Actions.WATER_PLANTS:
                agent.SetDestination(plant.transform.position);
                break;
            case Actions.DINNER:
                agent.SetDestination(kitchen.transform.position);
                break;
            case Actions.GO_BATHROOM:
                agent.SetDestination(bathroomDoor.transform.position);
                break;
            case Actions.WASH_HANDS:
                agent.SetDestination(bathroomDoor.transform.position);
                break;
            case Actions.HYGIENE_GEL:
                agent.SetDestination(hygieneGel.transform.position);
                break;
            case Actions.TEST_PATIENTS:
                agent.SetDestination(medicalCarrito.transform.position);
                break;
            case Actions.GIVE_MEDICAMENTS:
                agent.SetDestination(medicalCarrito.transform.position);
                break;
            case Actions.RCP:
                agent.SetDestination(medicalCarrito.transform.position);
                break;
            case Actions.PATIENT_DEATH:
                agent.SetDestination(medicalCarrito.transform.position);
                break;
            case Actions.YOUTUBE_VIDEO:
                agent.SetDestination(smartphoneMed.transform.position);
                break;
            case Actions.CLAP_VIDEO:
                agent.SetDestination(smartphoneMed.transform.position);
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

        if (currentAction != Actions.HYGIENE_GEL && currentAction != Actions.WASH_HANDS)
            GameManager.instance.AddHealth(healthGained);
        else if(GameManager.instance.boolean_stats["Went_Out"])
            GameManager.instance.AddHealth(healthGained);

        if (currentAction != Actions.TAKE_WALK && currentAction != Actions.TRASH_OUT && currentAction != Actions.TALK_NEIGHBOUR)
            GameManager.instance.boolean_stats["Went_Out"] = false;

        firstAction = false;
        secondAction = false;
        isDoingAction = false;
        currentAction = Actions.NONE;
    }

    private void OnLevelWasLoaded(int level)
    {
        if (level == 0)
        {
            player = GameObject.Find("MariCarmen");
            agent = player.GetComponent<NavMeshAgent>();
            if (SceneManager.GetActiveScene().name == "Main")
            {
                tv = GameObject.Find("Television");
                sofa = GameObject.Find("Sofa");
                shelf = GameObject.Find("Estantería");
                couch = GameObject.Find("Sillón");
                book = GameObject.Find("Book");
                bookPos = GameObject.Find("BookPos");
                houseDoor = GameObject.Find("Puerta");
                houseDoorSource = houseDoor.GetComponent<AudioSource>();
                neighbourDoor = GameObject.Find("Puerta Vecino");
                smartphone = GameObject.Find("Móvil");
                smartphonePos = GameObject.Find("SmartphonePos");
                bathroomDoor = GameObject.Find("Lavabo");
                chair = GameObject.Find("Silla");
                swegingBox = GameObject.Find("Costurero");
                washingMachine = GameObject.Find("Lavadora");
                kitchen = GameObject.Find("Kitchen");
                plant = GameObject.Find("Planta");
                hygieneGel = GameObject.Find("Gel Desinfectante");
                animator = player.GetComponentInChildren<Animator>();
                table = GameObject.Find("Mesa");
            }
            else if (SceneManager.GetActiveScene().name == "HospitalUpdated")
            {
                medicalCarrito = GameObject.Find("MedicalCarrito");
            }
        }
        //When change day we need to activate
            // Planta no regada, poner que se pueda regar
    }
}
