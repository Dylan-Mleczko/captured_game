using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : MonoBehaviour, Interaction {

    [SerializeField] Player player;
    [SerializeField] float speed;
    [SerializeField] AudioSource death;
    [SerializeField] AudioSource chase;
    [SerializeField] GameObject black;
    [SerializeField] GameObject text;
    [SerializeField] GameObject blood;
    [SerializeField] GameObject speech;
    [SerializeField] Animator door1;
    [SerializeField] Animator door2;
    [SerializeField] AudioSource doorSound;
    public bool canKill = false;
    public bool isAlive = true;
    Rigidbody rb;
    Queue<Vector3> trail;
    public GameObject floor;
    public GameObject camera;

    void Start() {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update() {
        if (isAlive) {
            Move();
        }
    }

    void Move() {
        Vector3 position = player.trail.Dequeue();
        rb.velocity = (new Vector3(position.x - transform.position.x, 0.0f, position.z - transform.position.z)).normalized * speed;
    }

    public void InteractWith() {
        if (canKill) {
            StartCoroutine(Kill());
        }
    }

    IEnumerator Kill() {
        floor.GetComponent<Floor>().StopRipple();
        camera.GetComponent<PixelShader>().active = false;
        rb.velocity = Vector3.zero;
        player.queenCanKill = false;
        text.SetActive(false);
        isAlive = false;
        player.isQueenChasing = false;
        chase.Stop();
        player.playerEnabled = false;
        black.SetActive(true);
        transform.Rotate(Vector3.right, 90);
        blood.SetActive(true);
        death.Play();
        yield return new WaitForSeconds(3);
        black.SetActive(false);
        yield return new WaitForSeconds(2);
        player.playerEnabled = true;

        foreach (Transform child in speech.transform) {
            GameObject text = child.gameObject;
            text.SetActive(true);
            yield return new WaitForSeconds(3);
            text.SetActive(false);
            yield return new WaitForSeconds(2);
        }

        doorSound.Play();
        door1.SetBool("Open", true);
        door2.SetBool("Open", true);
    }
}