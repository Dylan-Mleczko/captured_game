using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour, Interaction {

    [SerializeField] AudioSource sound;
    [SerializeField] GameObject noteText;
    [SerializeField] GameObject playerObject;
    Player playerComponent;

    public void Start() {
        playerComponent = playerObject.GetComponent<Player>();
    }

    public void InteractWith() {
        sound.Play();
        playerComponent.InteractMode();
        noteText.SetActive(true);
    }

}