using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    [System.Serializable]
    struct InteractableOptions
    {
        public int action;
        public string optionText;
        public uint time;
        public int health;
        public int positivism;
    }

    List<InteractableOptions> interactableOptions;
    UIManager uiManager;

    void ReadFromJson()
    {
        TextAsset json_file = Resources.Load("interactable") as TextAsset;
        JSONObject json = new JSONObject(json_file.text);

        JSONObject interactable_json = json.GetField(gameObject.name);

        foreach (JSONObject j in interactable_json.list)
        {
            InteractableOptions interactable = JsonUtility.FromJson<InteractableOptions>(j.ToString());
            interactableOptions.Add(interactable);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        interactableOptions = new List<InteractableOptions>();
        uiManager = GameObject.Find("GameManager").GetComponent<UIManager>();
        ReadFromJson();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTap(Vector3 hitPos)
    {
        //Set Title
        GameObject task;
        task = Instantiate(uiManager.titleTask);
        task.GetComponentInChildren<Text>().text = gameObject.name;
        task.transform.SetParent(uiManager.verticalTask.transform);

        //Set options
        foreach (InteractableOptions interactable in interactableOptions)
        {
            task = Instantiate(uiManager.buttonTask);
            task.GetComponentInChildren<Text>().text = interactable.optionText;
            task.transform.SetParent(uiManager.verticalTask.transform);
            task.GetComponent<Button>().onClick.AddListener(delegate { uiManager.RealizeAction(interactable.action, interactable.time, interactable.health, interactable.positivism); });
            RawImage postivism_task = task.transform.GetChild(1).GetComponent<RawImage>();
            RawImage health_task = task.transform.GetChild(2).GetComponent<RawImage>();

            //Positivism
            GameObject eff_obj = task.transform.GetChild(1).gameObject;
            float scale = 1.2f;

            if (interactable.positivism == 0)
                eff_obj.SetActive(false);
            else
            {
                eff_obj.SetActive(true);
                eff_obj.GetComponent<RawImage>().texture = interactable.positivism > 0 ? Resources.Load<Texture>("UI/green_face") : Resources.Load<Texture>("UI/Positivism");
            }

            if (Mathf.Abs(interactable.positivism) <= 5)
                scale = 0.7f;
            else if (Mathf.Abs(interactable.positivism) > 5 && Mathf.Abs(interactable.positivism) < 10)
                scale = 1.0f;

            eff_obj.transform.localScale = new Vector3(scale, scale, 1);

            //Health
            eff_obj = task.transform.GetChild(2).gameObject;

            if (interactable.health == 0 || (!GameManager.instance.boolean_stats["Went_Out"] && interactable.health > 0))
                eff_obj.SetActive(false);
            else
            {
                eff_obj.SetActive(true);
                eff_obj.GetComponent<RawImage>().texture = interactable.health > 0 ? Resources.Load<Texture>("UI/Health_Positive") : Resources.Load<Texture>("UI/Health");
            }

            if (Mathf.Abs(interactable.health) <= 1)
                scale = 0.7f;

            eff_obj.transform.localScale = new Vector3(scale, scale, 1);
        }

        //Set Close button
        task = Instantiate(uiManager.buttonTask);
        task.GetComponentInChildren<Text>().text = "Mejor hago otra cosa";
        task.transform.SetParent(uiManager.verticalTask.transform);
        task.GetComponent<Button>().onClick.AddListener(delegate { uiManager.CloseTask(); });
        task.transform.GetChild(1).gameObject.SetActive(false);
        task.transform.GetChild(2).gameObject.SetActive(false);

        uiManager.isTaskMenuOpen = true;
        uiManager.verticalTask.transform.position = CalculatePositionOffset(Camera.main.WorldToScreenPoint(hitPos));
        uiManager.verticalTask.SetActive(true);
    }

    Vector3 CalculatePositionOffset(Vector3 pos)
    {
        Vector3 position = pos;
        if (pos.x < Screen.width * 0.5)
            position.x += (float)(uiManager.verticalTask.GetComponent<Image>().rectTransform.rect.width * 0.7);
        else
            position.x -= (float)(uiManager.verticalTask.GetComponent<Image>().rectTransform.rect.width * 0.7);

        if (pos.y < Screen.height * 0.5)
            position.y += (float)(uiManager.verticalTask.GetComponent<Image>().rectTransform.rect.height * 0.7);
        else
            position.y -= (float)(uiManager.verticalTask.GetComponent<Image>().rectTransform.rect.height * 0.7);

        return position;
    }
}
