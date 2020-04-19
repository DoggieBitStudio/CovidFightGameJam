using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactable_2 : MonoBehaviour
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
        TextAsset json_file = Resources.Load("interactable_2") as TextAsset;
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
        }

        //Set Close button
        task = Instantiate(uiManager.buttonTask);
        task.GetComponentInChildren<Text>().text = "Mejor hago otra cosa";
        task.transform.SetParent(uiManager.verticalTask.transform);
        task.GetComponent<Button>().onClick.AddListener(delegate { uiManager.CloseTask(); });

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
