using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarScript : MonoBehaviour
{
    public Color full_color = Color.green;
    public Color low_color = Color.red;
    public string stat;
    public float max_stat;

    Slider slider;
    Image fill;
    private void Start()
    {
        slider = GetComponent<Slider>();
        fill = transform.GetChild(3).GetComponentInChildren<Image>();
    }
    // Update is called once per frame
    void Update()
    {
        slider.value = Mathf.Lerp(slider.value, GameManager.instance.int_stats[stat] / max_stat, Time.deltaTime * 5);
        fill.color = Color.Lerp(low_color, full_color, slider.value);

        if (slider.value < 0.5)
        {
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
