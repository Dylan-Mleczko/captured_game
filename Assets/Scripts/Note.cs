using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour, Interaction {

    [SerializeField] GameObject noteText;
    [SerializeField] GameObject playerObject;
    Player playerComponent;

    public void Start() {
        playerComponent = playerObject.GetComponent<Player>();
    }

    public void InteractWith() {
        playerComponent.InteractMode();
        noteText.SetActive(true);
    }

}