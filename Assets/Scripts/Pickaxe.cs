using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickaxe : MonoBehaviour, Interaction {

    public void InteractWith() {
        BrokenWall.hasPickaxe = true;
        Destroy(gameObject);
    }

}