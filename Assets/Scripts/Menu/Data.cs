using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Data {

    public int level;
    public float brightness;
    public float musicVolume;
    public float soundVolume;
    public float mouseSensitivity;

    public Data(int level, float brightness, float musicVolume, float soundVolume, float mouseSensitivity) {
        this.level = level;
        this.brightness = brightness;
        this.musicVolume = musicVolume;
        this.soundVolume = soundVolume;
        this.mouseSensitivity = mouseSensitivity;
    }

}