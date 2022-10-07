using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : MonoBehaviour {

    [SerializeField] GameObject player;
    [SerializeField] float speed;
    Rigidbody rb;
    Queue<Vector3> trail;

    void Start() {
        rb = gameObject.GetComponent<Rigidbody>();
        trail = player.GetComponent<Player>().trail;
    }

    void Update() {
        Move();
    }

    void Move() {
        Vector3 position = trail.Dequeue();
        rb.velocity = (new Vector3(position.x - transform.position.x, 0.0f, position.z - transform.position.z)).normalized * speed;
    }

}