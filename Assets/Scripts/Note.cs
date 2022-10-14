using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour, Interaction {

    [SerializeField] GameObject noteText;
    [SerializeField] GameObject playerObject;
    Player playerComponent;
    bool newNote;

    public void Start() {
        playerComponent = playerObject.GetComponent<Player>();
        newNote = true;
    }

    public void InteractWith() {
        playerComponent.InteractMode();
        noteText.SetActive(true);
        if (newNote) {
            DataManager.handle.data.notes++;
            newNote = false;
        }
    }

}