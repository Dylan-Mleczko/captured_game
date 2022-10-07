using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : MonoBehaviour {

    [SerializeField] float speed;
    int xDirection = 1;
    int zDirection = 1;

    void Update() {
        Move();
    }

    void OnCollisionEnter(Collision collision) {
        Vector3 normal = collision.contacts[0].normal;
        if (normal == transform.forward) {
            zDirection = 1;
        } else if (normal == -transform.forward) {
            zDirection = -1;
        } if (normal == transform.right) {
            xDirection = 1;
        } else if (normal == -transform.right) {
            xDirection = -1;
        }
    }

    void Move() {
        transform.Translate(Vector3.forward * speed * zDirection * Time.deltaTime);
        transform.Translate(Vector3.right * speed * xDirection * Time.deltaTime);
    }

}