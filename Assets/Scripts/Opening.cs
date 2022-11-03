using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opening : MonoBehaviour {

    [SerializeField] Player player;
    [SerializeField] GameObject key;
    [SerializeField] GameObject black;
    [SerializeField] GameObject text1;
    [SerializeField] GameObject text2;
    [SerializeField] GameObject skipCutscene;
    [SerializeField] GameObject guardObject1;
    [SerializeField] GameObject guardObject2;
    [SerializeField] GameObject prisonerObject;
    [SerializeField] Animator guard1;
    [SerializeField] Animator guard2;
    [SerializeField] Animator prisoner;
    [SerializeField] Animator door;
    [SerializeField] AudioSource doorSound;
    [SerializeField] AudioSource torture;
    [SerializeField] AudioSource keySound;

    private IEnumerator coroutine;

    private void Start() {
        coroutine = Title();
        StartCoroutine(coroutine);
    }

    void Update() {
        if (Input.GetKey(KeyCode.E)) {
            FinishOpening();
            StopCoroutine(coroutine);
        }
    }

    private IEnumerator Title() {
        // Animations - PART 0
        player.FrozenMode();
        yield return new WaitForSeconds(2);
        skipCutscene.SetActive(true);
        text1.SetActive(true);
        yield return new WaitForSeconds(3);
        text1.SetActive(false);
        yield return new WaitForSeconds(2);
        skipCutscene.SetActive(false);
        text2.SetActive(true);
        yield return new WaitForSeconds(3);
        text2.SetActive(false);
        yield return new WaitForSeconds(2);
        black.GetComponent<Animator>().SetBool("Fade", true);
        yield return new WaitForSeconds(3);
        black.SetActive(false);
        // Animations - PART 1
        guard1.SetBool("Part1", true);
        guard2.SetBool("Part1", true);
        prisoner.SetBool("Part1", true);
        yield return new WaitForSeconds(2.5f);
        player.PlayMode();
        player.isAlive = true;
        yield return new WaitForSeconds(2.5f);
        door.SetBool("Part1", true);
        doorSound.Play();
        yield return new WaitForSeconds(3);
        // Animations - PART 2
        guard1.SetBool("Part2", true);
        guard2.SetBool("Part2", true);
        prisoner.SetBool("Part2", true);
        yield return new WaitForSeconds(5);
        door.SetBool("Part2", true);
        doorSound.Play();
        yield return new WaitForSeconds(5);
        torture.Play();
        yield return new WaitForSeconds(5);
        prisonerObject.SetActive(false);
        // Animations - PART 3
        door.SetBool("Part3", true);
        doorSound.Play();
        yield return new WaitForSeconds(3);
        guard1.SetBool("Part3", true);
        guard2.SetBool("Part3", true);
        yield return new WaitForSeconds(2.5f);
        key.SetActive(true);
        keySound.Play();
        yield return new WaitForSeconds(2.5f);
        // Animations - PART 4
        door.SetBool("Part4", true);
        doorSound.Play();
        guardObject1.SetActive(false);
        guardObject2.SetActive(false);
    }

    private void FinishOpening() {
        text1.SetActive(false);
        text2.SetActive(false);
        black.SetActive(false);
        player.PlayMode();
        player.isAlive = true;
        prisonerObject.SetActive(false);
        guardObject1.SetActive(false);
        guardObject2.SetActive(false);
        key.SetActive(true);
        keySound.Play();
    }
}