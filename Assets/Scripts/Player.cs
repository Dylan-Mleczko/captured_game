using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField] float moveSpeed;
    [SerializeField] float sprintMultipler;
    [SerializeField] float mouseSensitivity;
    CursorLockMode lockMode;
    public Queue<Vector3> trail;
    bool isQueenChasing;
 
    void Awake () {
        lockMode = CursorLockMode.Locked;
        Cursor.lockState = lockMode;
    }

    void Start() {
        trail = new Queue<Vector3>();
        isQueenChasing = false;
    }

    void Update() {
        Move();
        LeaveTrail();
    }

    void Move() {
        double speedMultipler = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ? sprintMultipler : 1;
        if (Input.GetKey(KeyCode.W)) {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S)) {
            transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A)) {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D)) {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
        transform.Rotate(Input.GetAxis("Mouse Y") * -mouseSensitivity, Input.GetAxis("Mouse X") * mouseSensitivity, 0);
    }

    void LeaveTrail() {
        if (isQueenChasing) {
            trail.Enqueue(transform.position);
        }
    }

}