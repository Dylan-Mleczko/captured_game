using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trapdoor : MonoBehaviour, Interaction {

    public static bool isOpen;
    Animation anim;

    void Start() {
        isOpen = false;
        anim = gameObject.GetComponent<Animation>();
    }

    public void InteractWith() {
        if (isOpen) {
            anim.Play();
            isOpen = false;
        } else {
            anim.Play();
            isOpen = true;
        }
    }

}