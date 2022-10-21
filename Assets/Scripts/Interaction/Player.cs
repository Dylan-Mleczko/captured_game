using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField] Camera playerCamera;
    [SerializeField] float moveSpeed;
    [SerializeField] float sprintMultipler;
    [SerializeField] float mouseSensitivity;
    [SerializeField] float interactionDistance;

    private CharacterController characterController;
    private float rotationX;
    public bool playerEnabled;
    public Queue<Vector3> trail;
    bool isQueenChasing;
    GameObject currentText;

    void Start() {
        PlayMode();
        characterController = GetComponent<CharacterController>();
        rotationX = 0;
        trail = new Queue<Vector3>();
        isQueenChasing = false;
    }

    void Update() {
        if (playerEnabled) {
            Move();
            Look();
        }
        LeaveTrail();
    }

    void Move() {
        float speedMultipler = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ? sprintMultipler : 1;
        float currentSpeed = moveSpeed * speedMultipler;
        if (Input.GetKey(KeyCode.W)) {
            characterController.Move(transform.TransformDirection(Vector3.forward) * currentSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S)) {
            characterController.Move(transform.TransformDirection(Vector3.back) * currentSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A)) {
            characterController.Move(transform.TransformDirection(Vector3.left) * currentSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D)) {
            characterController.Move(transform.TransformDirection(Vector3.right) * currentSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.Q)) {
            InteractMode();
        }
        rotationX += -Input.GetAxis("Mouse Y") * mouseSensitivity;
        rotationX = Mathf.Clamp(rotationX, -45.0f, 45.0f);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * mouseSensitivity, 0);
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
            if ((visibleObject.tag == "Door" && visibleObject.GetComponent<Door>().isOpen) || (visibleObject.tag == "Safe" && visibleObject.GetComponent<Safe>().isOpen)) {
                return;
            }
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

    public void PlayMode() {
        playerEnabled = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void InteractMode() {
        playerEnabled = false;
        Cursor.lockState = CursorLockMode.None;
    }

}