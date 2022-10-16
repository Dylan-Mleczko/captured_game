using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, Interaction {

    public void InteractWith() {
        Door.hasKey = true;
        Destroy(gameObject);
    }

}