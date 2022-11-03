using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseManager : MonoBehaviour {

    [SerializeField] Player player;
    [SerializeField] Queen queen;
    [SerializeField] Floor floor;
    [SerializeField] AudioSource chase;
    [SerializeField] AudioSource crash;
    [SerializeField] AudioSource rumble;

    private void Start() {
        player.playerEnabled = false;
        StartCoroutine(Chase());
    }

    private IEnumerator Chase() {
        yield return new WaitForSeconds(1);
        chase.Play();
        yield return new WaitForSeconds(1);
        crash.Play();
        rumble.Play();
        floor.HitFloor();
        yield return new WaitForSeconds(1);
        player.playerEnabled = true;
        player.isQueenChasing = true;
        queen.enabled = true;
    }

}