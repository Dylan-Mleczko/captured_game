using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent<Collider>]
public class Interactable : MonoBehaviour
{

    [SerializeField] GameObject[] interactableObjects;
    public string interactMessage;

    void InteractWith() {
        foreach(GameObject object in interactableObjects) {
            object.GetComponent<Interaction>().InteractWith();
        }
    }

}