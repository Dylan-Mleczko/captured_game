// Credit to https://www.youtube.com/watch?time_continue=161&v=XiJ-kb-NvV4&feature=emb_title&ab_channel=JTAGames

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class Brightness : MonoBehaviour
{
    public GameObject brightnessSlider;
    public PostProcessProfile brightness;
    public GameObject layer;
    AutoExposure exposure;

    void Start() {
        brightness.TryGetSettings(out exposure);
    }

    void Update() {
        if (brightnessSlider == null) {
            brightnessSlider = GameObject.Find("BrightnessSlider");
        }

        if (layer == null) {
            layer = GameObject.Find("Main Camera");
        }
    }

    public void AdjustBrightness(float value) {
        if (value > 0.05f) {
            exposure.keyValue.value = value;
        } else {
            exposure.keyValue.value = 0.05f;
        }
    }
}
