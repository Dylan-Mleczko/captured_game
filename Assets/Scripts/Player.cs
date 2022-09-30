using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField] float moveSpeed;
    [SerializeField] float sprintMultipler;
    [SerializeField] float mouseSensitivity;
    [SerializeField] float interactionDistance;
    
    CursorLockMode lockMode;

    public Queue<Vector3> trail;
    bool isQueenChasing;

    GameObject currentText;
 
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
        Look();
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

    void Look() {
        RaycastHit hit;
        Interactable visibleObject = Physics.Raycast(new Ray(transform.position, transform.forward), out hit) && hit.distance < interactionDistance ? hit.transform.gameObject.GetComponent<Interactable>() : null;
        if (visibleObject != null) {
            currentText = visibleObject.text;
            currentText.SetActive(true);
            if (Input.GetMouseButtonDown(0)) {
                visibleObject.InteractWith();
            }
        } else if (currentText != null) {
            currentText.SetActive(false);
            currentText = null;
        }
    }

}