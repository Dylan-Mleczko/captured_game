using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour {

    [SerializeField] GameObject player;
    [SerializeField] float speed;
    [SerializeField] float coolDown;
    float time;
    Rigidbody rb;
    Vector3 target;
    bool isTargeting;

    void Start() {
        rb = gameObject.GetComponent<Rigidbody>();
        time = coolDown;
        isTargeting = false;
    }

    void Update() {
        if (time > 0) {
            time -= Time.deltaTime;
            transform.LookAt(player.transform);
        } else {
            if (!isTargeting) {
                target = player.transform.position;
                isTargeting = true;
            }
            Move();
        }
    }

    void Move() {
        if (Vector3.Distance(transform.position, player.transform.position) < 0.0001f) {
            isTargeting = false;
            time = coolDown;
        } else {
            rb.velocity = (new Vector3(target.x - transform.position.x, 0.0f, target.z - transform.position.z)).normalized * speed;
        }
    }

}