using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, Interaction {

    [SerializeField] int key;
    [SerializeField] AudioSource openSound;
    [SerializeField] AudioSource lockedSound;
    [SerializeField] AudioSource unlockSound;
    [SerializeField] bool isProcedural;

    public static bool hasKey1;
    public static bool hasKey2;
    public bool isOpen;
    Animator anim;

    void Start() {
        hasKey1 = false;
        hasKey2 = false;
        isOpen = false;
        anim = gameObject.GetComponent<Animator>();
    }

    public void InteractWith() {
        if (isProcedural && hasKey1) {
            GameObject player = GameObject.Find("Player");
            player.GetComponent<Player>().playerEnabled = false;
            GameObject.Find("OpenText").SetActive(false);
            unlockSound.Play();
            openSound.Play();
            GameObject.Find("Fade").GetComponent<Animator>().SetBool("Fade2", true);
            StartCoroutine(Wait());
        } else if ((key == 1 && hasKey1) || (key == 2 && hasKey2)) {
            unlockSound.Play();
            openSound.Play();
            anim.SetBool("open", true);
            isOpen = true;
        } else if (!isProcedural) {
            lockedSound.Play();
        }
    }

    public bool IsLocked() {
        return !((key == 1 && hasKey1) || (key == 2 && hasKey2));
    }

    private IEnumerator Wait() {
        yield return new WaitForSeconds(3);
        GameObject.Find("DataManager").GetComponent<DataManager>().NextLevel();
    }

}