using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour, Interaction {

    [SerializeField] GameObject player;
    [SerializeField] GameObject queen1;
    [SerializeField] GameObject queen2;
    [SerializeField] AudioSource alarm;
    [SerializeField] AudioSource chase;
    [SerializeField] AudioSource ambiance;
    public bool isUp;

    private void Start() {
        isUp = false;
    }

    public void InteractWith() {
        isUp = true;
        ambiance.Stop();
        gameObject.GetComponent<Animator>().SetBool("LeverUp", true);
        gameObject.GetComponent<AudioSource>().Play();
        StartCoroutine(Alarm());
    }

    private IEnumerator Alarm() {
        yield return new WaitForSeconds(3);
        alarm.Play();
        yield return new WaitForSeconds(2);
        chase.Play();
        player.GetComponent<Player>().isQueenChasing = true;
        queen1.SetActive(false);
        queen2.SetActive(true);
    }

}