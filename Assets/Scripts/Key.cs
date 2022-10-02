using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, Interaction {

    [SerializeField] GameObject key;

    public void InteractWith() {
        key.SetActive(true);
        Door.hasKey = true;
        Destroy(gameObject);
    }

}