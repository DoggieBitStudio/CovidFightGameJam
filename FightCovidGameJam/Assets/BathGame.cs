using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BathGame : MonoBehaviour
{
    public GameObject[] ui_images;
    public float distance = 50f;
    int selected_objects = 0;
    bool fucked_up = false;
    bool cleared = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
    }

    public void Finish()
    {
        if (cleared)
        {
            Debug.Log("All correct");
        }
        else
        {
            Debug.Log("Fucked up");
        }
    }
}

