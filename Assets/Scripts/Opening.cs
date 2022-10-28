using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opening : MonoBehaviour {

    [SerializeField] Player player;
    [SerializeField] GameObject black;
    [SerializeField] GameObject part1;
    [SerializeField] GameObject part2;

    private void Start() {
        StartCoroutine(Title());
    }

    private IEnumerator Title() {
        player.FrozenMode();
        yield return new WaitForSeconds(2);
        part1.SetActive(true);
        yield return new WaitForSeconds(3);
        part1.SetActive(false);
        yield return new WaitForSeconds(2);
        part2.SetActive(true);
        yield return new WaitForSeconds(3);
        part2.SetActive(false);
        yield return new WaitForSeconds(2);
        black.GetComponent<Animator>().SetBool("Fade", true);
        yield return new WaitForSeconds(3);
        black.SetActive(false);
        player.PlayMode();
        player.isAlive = true;
    }

}