using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, Interaction {

    [SerializeField] int key;
    [SerializeField] AudioSource openSound;
    [SerializeField] AudioSource lockedSound;
    [SerializeField] AudioSource unlockSound;

    public static bool hasKey1;
    public static bool hasKey2;
    public bool isOpen;
    Animator anim;

    void Start() {
        hasKey1 = false;
        hasKey2 = false;
        isOpen = false;
        anim = gameObject.GetComponent<Animator>();
    }

    public void InteractWith() {
        if ((key == 1 && hasKey1) || (key == 2 && hasKey2)) {
            unlockSound.Play();
            openSound.Play();
            anim.SetBool("open", true);
            isOpen = true;
        } else {
            lockedSound.Play();
        }
    }

}