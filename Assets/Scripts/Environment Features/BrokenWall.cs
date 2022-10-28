using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenWall : MonoBehaviour, Interaction {

    public static bool hasPickaxe;
    [SerializeField] GameObject wall1;
    [SerializeField] GameObject wall2;
    [SerializeField] AudioSource sound;
    [SerializeField] GameObject source;
    [SerializeField] GameObject newText;
    private Animator anim;

    void Start() {
        hasPickaxe = false;
        anim = source.GetComponent<Animator>();
    }

    public void InteractWith() {
        if (hasPickaxe) {
            source.SetActive(true);
            sound.Play();
            anim.SetBool("Fade", true);
            StartCoroutine(Fade());
        }
    }

    private IEnumerator Fade() {
        yield return new WaitForSeconds(5);
        wall1.SetActive(false);
        wall2.SetActive(true);
        anim.SetBool("Fade", false);
        source.SetActive(false);
    }

}