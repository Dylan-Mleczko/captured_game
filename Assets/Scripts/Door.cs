using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, Interaction {

    public static bool hasKey;
    public bool isOpen;
    Animator anim;

    void Start() {
        hasKey = false;
        isOpen = false;
        anim = gameObject.GetComponent<Animator>();
    }

    public void InteractWith() {
        if (hasKey) {
            anim.SetBool("open", true);
            hasKey = false;
            isOpen = true;
        }
    }

}