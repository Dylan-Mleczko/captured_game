using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, Interaction {

    [SerializeField] AudioSource sound;
    [SerializeField] int key;

    public void InteractWith() {
        sound.Play();
        if (key == 1) {
            Door.hasKey1 = true;
        } else {
            Door.hasKey2 = true;
        }
        Destroy(gameObject);
    }

}