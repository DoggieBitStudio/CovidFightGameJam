using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    [System.Serializable]
    struct InteractableOptions
    {
        public string optionText;
        public uint time;
        public int health;
        public int positivism;
        public int mask;
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

    public void OnTap()
    {
        GameObject task;
        foreach (InteractableOptions interactable in interactableOptions)
        {
            task = Instantiate(uiManager.buttonTask);
            task.GetComponentInChildren<Text>().text = interactable.optionText;
            task.transform.SetParent(uiManager.verticalTask.transform);
            task.GetComponent<Button>().onClick.AddListener(delegate { uiManager.RealizeAction(interactable.time, interactable.health, interactable.positivism, interactable.mask); });
        }

        task = Instantiate(uiManager.closeButton);
        task.transform.SetParent(uiManager.verticalTask.transform);
        task.GetComponent<Button>().onClick.AddListener(delegate { uiManager.CloseTask(); });

        uiManager.isTaskMenuOpen = true;
    }
}
