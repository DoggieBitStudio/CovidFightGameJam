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
    }

    List<InteractableOptions> interactableOptions;
    UIManager uiManager;

    void ReadFromJson()
    {
        TextAsset json_file = Resources.Load("interactable") as TextAsset;
        JSONObject json = new JSONObject(json_file.text);

        JSONObject interactable_json = json.GetField(gameObject.name);
        Debug.Log(interactable_json);

        foreach (JSONObject j in interactable_json.list)
        {
            InteractableOptions interactable = JsonUtility.FromJson<InteractableOptions>(j.ToString());
            interactableOptions.Add(interactable);
            Debug.Log(interactable.optionText);
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
        foreach (InteractableOptions interactable in interactableOptions)
        {
            GameObject go = Instantiate(uiManager.buttonTask);
            go.GetComponentInChildren<Text>().text = interactable.optionText;
            go.transform.SetParent(uiManager.verticalTask.transform);
        }

        uiManager.isTaskMenuOpen = true;
    }
}
