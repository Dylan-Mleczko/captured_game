using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour {

    [SerializeField] GameObject source;
    [SerializeField] AudioSource sound;
    
    private void OnTriggerEnter(Collider collider) {
        source.SetActive(true);
        sound.Play();
        source.GetComponent<Animator>().SetBool("Fade", true);
        StartCoroutine(Fade());
    }

    private IEnumerator Fade() {
        yield return new WaitForSeconds(5);
        DataManager.handle.NextLevel();
    }

}