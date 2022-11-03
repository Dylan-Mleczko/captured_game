using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrightnessSlider : MonoBehaviour
{
    Slider slider;
    DataManager manager;
    
    void Start() {
        slider = GetComponent<Slider>();
        manager = FindObjectOfType<DataManager>();

        Debug.Log("Slider", slider);
        Debug.Log("Manager", manager);

        if (slider != null && manager != null) {
            slider.onValueChanged.AddListener(value => manager.GetComponent<Brightness>().AdjustBrightness(slider.value));
        } else {
            Debug.LogError("Slider data manager not attached");
        }
    }

    void Update()
    {
        if (slider.onValueChanged != null) {
            // slider.onValueChanged.AddListener(GameObject.Find("DataManager"));
        }
    }
}
