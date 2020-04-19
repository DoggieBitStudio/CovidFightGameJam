using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class BathGame : MonoBehaviour
{
    public GameObject[] ui_images;
    public GameObject[] secuence_images;
    public GameObject secuence;
    public GameObject soapWin;
    public GameObject soapLose;
    public GameObject mask;
    public Image help;
    public float distance = 50f;
    int selected_objects = 0;
    bool fucked_up = false;
    bool cleared = false;
    bool hasStarted = false;

    float current_time = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        help.gameObject.transform.DOPunchScale(new Vector3(0.4f, 0.5f, 0.1f), 1f);

        if (!GameManager.instance.boolean_stats["Mask"])
            mask.SetActive(false);
        else
            mask.SetActive(true);

        current_time = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_STANDALONE_WIN
        if(hasStarted)
            HandleStandaloneInput();
        else if(Time.time - current_time > 2)
        {
            Color h_col = help.color;
            h_col.a -= 0.01f;
            help.color = h_col;

            foreach(Transform child in help.gameObject.transform)
            {
                if(child.gameObject.GetComponent<Image>())
                    FadeObject(child.gameObject);
            }

            if (h_col.a <= 0)
                hasStarted = true;
        }
#endif
#if UNITY_ANDROID
        if (hasStarted)
            HandlePhoneInput();
        else if(Time.time - current_time > 4)
        {
            float alpha = 0.0f;
            foreach(Transform child in help.gameObject.transform)
            {
                if(child.gameObject.GetComponent<Image>())
                    alpha = FadeObject(child.gameObject);
            }

            if (alpha <= 0)
            {
                hasStarted = true;
            }
        }
           
#endif
    }

    void HandleStandaloneInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //create a ray cast and set it to the mouses cursor position in game
            Ray ray;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, distance))
            {
                //draw invisible ray cast/vector
                Debug.DrawLine(ray.origin, hit.point);
                //log hit area to the console
                if (hit.collider.CompareTag("BathSelectable"))
                {
                    CheckSelected(hit.collider.gameObject.GetComponent<BathInteractable>().name);
                    selected_objects++;
                }
            }
        }
    }

    void HandlePhoneInput()
    {
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended)
        {
            //create a ray cast and set it to the mouses cursor position in game
            Ray ray;
            ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, distance))
            {
                //draw invisible ray cast/vector
                Debug.DrawLine(ray.origin, hit.point);
                //log hit area to the console
                if (hit.collider.CompareTag("BathSelectable"))
                {
                    CheckSelected(hit.collider.gameObject.GetComponent<BathInteractable>().name);
                    selected_objects++;
                }
            }
        }
    }

    void CheckSelected(string name)
    {
        if(selected_objects == 0)
        {
            if (name.Equals("Soap"))
                ui_images[selected_objects].GetComponent<Image>().color = Color.green;
            else
            {
                ui_images[selected_objects].GetComponent<Image>().color = Color.red;
                fucked_up = true;
            }

        }else if(selected_objects == 1)
        {
            if (name.Equals("Mask") && !fucked_up)
                ui_images[selected_objects].GetComponent<Image>().color = Color.green;
            else
            {
                ui_images[selected_objects].GetComponent<Image>().color = Color.red;
                fucked_up = true;
            }
        }
        else if (selected_objects == 2)
        {
            if (name.Equals("Soap") && !fucked_up)
                ui_images[selected_objects].GetComponent<Image>().color = Color.green;
            else
            {
                ui_images[selected_objects].GetComponent<Image>().color = Color.red;
                fucked_up = true;
            }
        }
        else if (selected_objects == 3)
        {
            if (name.Equals("Gloves") && !fucked_up)
            {
                ui_images[selected_objects].GetComponent<Image>().color = Color.green;
                cleared = true;
            }
            else
            {
                ui_images[selected_objects].GetComponent<Image>().color = Color.red;
                fucked_up = true;
            }

            Finish();

        }

        FillStep(name);

        if (name.Equals("Mask"))
            GameManager.instance.boolean_stats["Mask"] = false;


    }
    public void Finish()
    {
        if (cleared)
        {
            soapWin.SetActive(true);
        }
        else
        {
            soapLose.SetActive(true);
            GameManager.instance.int_stats["Health"] -= 1;
        }

        GameManager.instance.boolean_stats["Went_Out"] = true;
        GameManager.instance.LoadSceneFade("Main");
    }

    void FillStep(string name)
    {
        if (selected_objects > 0)
            Instantiate(secuence_images[3], secuence.transform);

        if (name.Equals("Soap"))
        {
            Instantiate(secuence_images[0], secuence.transform);
        }
        if (name.Equals("Mask"))
        {
            Instantiate(secuence_images[1], secuence.transform);
        }
        if (name.Equals("Gloves"))
        {
            Instantiate(secuence_images[2], secuence.transform);
        }

    }

    float FadeObject(GameObject obj)
    {
        Color col = obj.GetComponent<Image>().color;
        col.a -= 0.01f;
        obj.GetComponent<Image>().color = col;

        return col.a;
    }
}