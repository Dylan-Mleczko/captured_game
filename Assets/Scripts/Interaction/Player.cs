using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject exitMenu;
    [SerializeField] AudioSource pauseSound;
    [SerializeField] AudioSource ambiance;
    [SerializeField] GameObject deathScreen;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] AudioSource deathSound;
    [SerializeField] AudioSource gameOverSound;
    [SerializeField] GameObject ui;
    [SerializeField] Camera playerCamera;
    [SerializeField] float moveSpeed;
    [SerializeField] float sprintMultipler;
    [SerializeField] float mouseSensitivity;
    [SerializeField] float interactionDistance;

    public bool isPaused;
    public bool isAlive;
    private CharacterController characterController;
    private float rotationX;
    public bool playerEnabled;
    public Queue<Vector3> trail;
    private bool isQueenChasing;
    private bool wasQueenChasing;
    GameObject currentText;

    void Start() {
        PlayMode();
        characterController = GetComponent<CharacterController>();
        rotationX = 0;
        trail = new Queue<Vector3>();
        isQueenChasing = false;
        isAlive = true;
        isPaused = false;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && isAlive) {
            Pause();
        }
        if (!isPaused) {
            if (playerEnabled) {
                Move();
                Look();
            }
            LeaveTrail();
        }
    }

    void OnTriggerEnter(Collider collider) {
        if (isAlive && collider.tag == "Enemy") {
            isAlive = false;
            ambiance.Stop();
            deathScreen.SetActive(true);
            deathSound.Play();
            StartCoroutine(GameOver());
        }
    }

    void Pause() {
        if (isPaused) {
            pauseSound.Play();
            PlayMode();
            isPaused = false;
            Time.timeScale = 1;
            AudioListener.pause = false;
            ResetPauseMenu();
            pauseMenu.SetActive(false);
        } else if (playerEnabled) {
            pauseSound.Play();
            InteractMode();
            isPaused = true;
            Time.timeScale = 0;
            AudioListener.pause = true;
            pauseMenu.SetActive(true);
        }
    }

    void ResetPauseMenu() {
        settingsMenu.SetActive(false);
        exitMenu.SetActive(false);
        mainMenu.SetActive(true);
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
            if (visibleObject.tag == "Door" && visibleObject.GetComponent<Door>().isOpen) {
                return;
            }
            currentText = visibleObject.text;
            currentText.SetActive(true);
            if (Input.GetMouseButtonDown(0)) {
                visibleObject.InteractWith();
            }
        }
        else if (currentText != null) {
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

    private IEnumerator GameOver() {
		ui.SetActive(true);
        yield return new WaitForSeconds(3);
        gameOverSound.Play();
        ui.GetComponent<Animator>().SetBool("Activate", true);
        yield return new WaitForSeconds(3);
        gameOverScreen.SetActive(true);
        InteractMode();
    }

}