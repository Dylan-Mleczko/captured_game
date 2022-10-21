using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Data : MonoBehaviour {

    public int level;
    public int notes;
    public float brightness;
    public float musicVolume;
    public float soundVolume;
    public float mouseSensitivity;

    public Data(int level, int notes, float brightness, float musicVolume, float soundVolume, float mouseSensitivity) {
        this.level = level;
        this.notes = notes;
        this.brightness = brightness;
        this.musicVolume = musicVolume;
        this.soundVolume = soundVolume;
        this.mouseSensitivity = mouseSensitivity;
    }

}