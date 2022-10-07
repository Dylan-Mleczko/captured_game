using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Interactable : MonoBehaviour {

    [SerializeField] GameObject[] interactableObjects;
    public GameObject text;

    public void InteractWith() {
        for (int i = 0; i < interactableObjects.Length; i++) {
            interactableObjects[i].GetComponent<Interaction>().InteractWith();
        }
    }

}