using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : MonoBehaviour {

    [SerializeField] float speed;
    Rigidbody rigidbody;

    void Start() {
        rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    void Update() {

    }

    void OnTriggerStay(Collider collider) {
        if (collider.name == "Player") {
            GameObject player = collider.gameObject;
            rigidbody.velocity = (new Vector3(transform.position.x - player.transform.position.x, 0.0f, transform.position.z - player.transform.position.z)).normalized * speed;
        }
    }

    void OnTriggerExit(Collider collider) {
        if (collider.name == "Player") {
            rigidbody.velocity = Vector3.zero;
        }
    }

}