using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Safe : MonoBehaviour, Interaction {
    
    [SerializeField] GameObject safeScreen;
    [SerializeField] GameObject playerObject;
    Player playerComponent;
    Animator anim;
    [SerializeField] GameObject[] screenObjects;
    TMP_Text[] screens;
    int[] combination;
    int pointer;
    public bool isOpen;

    public void Start() {
        isOpen = false;
        playerComponent = playerObject.GetComponent<Player>();
        anim = gameObject.GetComponent<Animator>();
        screens = new TMP_Text[4];
        for (int i = 0; i < 4; i++) {
            screens[i] = screenObjects[i].GetComponent<TMP_Text>();
        }
        combination = new int[4];
        ResetCode();
    }

    public void InteractWith() {
        playerComponent.InteractMode();
        safeScreen.SetActive(true);
    }

    public void InputNumber(int number) {
        if (pointer < 4) {
            combination[pointer] = number;
            screens[pointer].text = number.ToString();
            pointer++;
        }
    }

    public void CheckCode() {
        if (combination[0] == 1 && combination[1] == 8 && combination[2] == 5 && combination[3] == 1) {
            safeScreen.SetActive(false);
            playerComponent.PlayMode();
            anim.SetBool("open", true);
            isOpen = true;
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