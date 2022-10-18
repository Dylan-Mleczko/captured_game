using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, Interaction {

    [SerializeField] int key;

    public void InteractWith() {
        if (key == 1) {
            Door.hasKey1 = true;
        } else {
            Door.hasKey2 = true;
        }
        Destroy(gameObject);
    }

}