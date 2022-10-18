using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour, Interaction {

    [SerializeField] GameObject queen1;
    [SerializeField] GameObject queen2;
    private Animator anim;

    private void Start() {
        anim = gameObject.GetComponent<Animator>();
    }

    public void InteractWith() {
        anim.SetBool("LeverUp", true);
        queen1.SetActive(false);
        queen2.SetActive(true);
    }

}