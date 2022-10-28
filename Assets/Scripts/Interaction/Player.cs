using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject prebrokenText;
    [SerializeField] GameObject lockedText;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject exitMenu;
    [SerializeField] AudioSource pauseSound;
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
    public bool isQueenChasing;
    private bool wasQueenChasing;
    GameObject currentText;

    void Start() {
        FrozenMode();
        characterController = GetComponent<CharacterController>();
        rotationX = 0;
        trail = new Queue<Vector3>();
        isQueenChasing = false;
        isAlive = false;
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
            AudioListener.pause = true;
            isAlive = false;
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
            isQueenChasing = wasQueenChasing;
        } else if (playerEnabled) {
            pauseSound.Play();
            InteractMode();
            isPaused = true;
            Time.timeScale = 0;
            AudioListener.pause = true;
            pauseMenu.SetActive(true);
            wasQueenChasing = isQueenChasing;
            isQueenChasing = false;
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
            if ((visibleObject.tag == "Door" && visibleObject.GetComponent<Door>().isOpen) || (visibleObject.tag == "Lever" && visibleObject.GetComponent<Lever>().isUp)) {
                return;
            }
            if (visibleObject.tag == "Door" && visibleObject.GetComponent<Door>().IsLocked()) {
                currentText = lockedText;
            } else if (visibleObject.tag == "Wall" && !BrokenWall.hasPickaxe) {
                currentText = prebrokenText;
            } else {
                currentText = visibleObject.text;
            }
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

    public void FrozenMode() {
        playerEnabled = false;
        Cursor.lockState = CursorLockMode.Locked;
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