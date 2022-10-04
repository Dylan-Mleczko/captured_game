using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, Interaction {

    [SerializeField] GameObject keyText;

    public void InteractWith() {
        keyText.SetActive(true);
        Door.hasKey = true;
        Destroy(gameObject);
    }

}