using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickaxe : MonoBehaviour, Interaction {

    [SerializeField] AudioSource sound;
    
    public void InteractWith() {
        sound.Play();
        BrokenWall.hasPickaxe = true;
        Destroy(gameObject);
    }

}