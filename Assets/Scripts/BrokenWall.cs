using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenWall : MonoBehaviour, Interaction {

    public static bool hasPickaxe;

    void Start() {
        hasPickaxe = false;
    }

    public void InteractWith() {
        if (hasPickaxe) {
            hasPickaxe = false;
        }
    }

}