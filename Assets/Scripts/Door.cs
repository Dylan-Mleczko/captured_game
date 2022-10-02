using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, Interaction {

    public static bool hasKey;
    Animation anim;

    void Start() {
        hasKey = false;
        anim = gameObject.GetComponent<Animation>();
    }

    public void InteractWith() {
        Debug.Log("HELLO");
        if (hasKey) {
            anim.Play();
            hasKey = false;
        }
    }

}