using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenWall : MonoBehaviour, Interaction {

    public static bool hasPickaxe;
    [SerializeField] GameObject wall1;
    [SerializeField] GameObject wall2;

    void Start() {
        hasPickaxe = false;
    }

    public void InteractWith() {
        if (hasPickaxe) {
            wall1.SetActive(false);
            wall2.SetActive(true);
        }
    }

}