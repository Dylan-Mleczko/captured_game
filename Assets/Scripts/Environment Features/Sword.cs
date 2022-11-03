using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour, Interaction {

    [SerializeField] AudioSource sound;
    [SerializeField] Queen queen;

    public void InteractWith() {
        sound.Play();
        queen.canKill = true;
        Destroy(gameObject);
    }

}