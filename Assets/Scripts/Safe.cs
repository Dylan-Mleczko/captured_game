using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Safe : MonoBehaviour, Interaction {
    
    [SerializeField] GameObject safeScreen;
    [SerializeField] GameObject playerObject;
    Player playerComponent;
    Animation anim;
    [SerializeField] GameObject[] screenObjects;
    Text[] screens;
    int[] combination;
    int pointer;

    public void Start() {
        playerComponent = playerObject.GetComponent<Player>();
        anim = gameObject.GetComponent<Animation>();
        screens = new Text[4];
        for (int i = 0; i < 4; i++) {
            screens[i] = screenObjects[i].GetComponent<Text>();
        }
        combination = new int[4];
        ResetCode();
    }

    public void InteractWith() {
        playerComponent.playerEnabled = false;
        safeScreen.SetActive(true);
    }

    public void InputNumber(int number) {
        if (pointer < 4) {
            combination[pointer++] = number;
        }
    }

    public void CheckCode() {
        if (combination[0] == 1 && combination[1] == 8 && combination[2] == 5 && combination[3] == 1) {
            safeScreen.SetActive(false);
            anim.Play();
        } else {
            ResetCode();
        }
    }

    public void ResetCode() {
        for (int i = 0; i < 4; i++) {
            screens[i].text = "";
            combination[i] = -1;
        }
        pointer = 0;
    }

}