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
    [SerializeField] GameObject controlsMenu;
    [SerializeField] GameObject exitMenu;
    [SerializeField] AudioSource pauseSound;
    [SerializeField] GameObject deathScreen;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject pickupText;
    [SerializeField] AudioSource deathSound;
    [SerializeField] AudioSource gameOverSound;
    [SerializeField] GameObject ui;
    [SerializeField] Camera playerCamera;
    [SerializeField] float moveSpeed;
    [SerializeField] float sprintMultipler;
    [SerializeField] float mouseSensitivity;
    [SerializeField] float interactionDistance;
    [SerializeField] Knight knight1;
    [SerializeField] Knight knight2;
    [SerializeField] Knight knight3;
    [SerializeField] Knight knight4;
    [SerializeField] GameObject kingText;
    [SerializeField] AudioSource music;
    [SerializeField] GameObject black;
    [SerializeField] AudioSource kill;
    
    public bool queenCanKill = true;
    public bool isPaused;
    public bool isAlive;
    private CharacterController characterController;
    private float rotationX;
    public bool playerEnabled;
    public Queue<Vector3> trail;
    public bool isQueenChasing;
    private bool wasQueenChasing;
    GameObject currentText;
    public bool initialAnimation;

    void Start() {
        if (initialAnimation) {
            FrozenMode();
        } else {
            PlayMode();
        }
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
        if (isAlive && (collider.tag == "Enemy" || (collider.tag == "Queen" && queenCanKill))) {
            AudioListener.pause = true;
            isAlive = false;
            deathScreen.SetActive(true);
            deathSound.Play();
            StartCoroutine(GameOver());
        } else if (collider.tag == "KnightManager1") {
            knight1.enabled = true;
            knight2.enabled = true;
            knight3.enabled = true;
            knight4.enabled = true;
        } else if (collider.tag == "KnightManager2") {
            knight1.enabled = false;
            knight2.enabled = false;
            knight3.enabled = false;
            knight4.enabled = false;
        } else if (collider.tag == "King") {
            kingText.SetActive(true);
        }
    }

    void OnTriggerExit(Collider collider) {
        if (collider.tag == "King") {
            kingText.SetActive(false);
        }
    }

    void OnTriggerStay(Collider collider) {
        if (collider.tag == "King" && Input.GetKey(KeyCode.E)) {
            StartCoroutine(PlayEnding());
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
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        exitMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    void Move() {
        float speedMultipler = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ? sprintMultipler : 1;
        float currentSpeed = moveSpeed * speedMultipler;

        Vector3 direction = new Vector3();
        if (Input.GetKey(KeyCode.W)) {
            direction += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S)) {
            direction += Vector3.back;
        }
        if (Input.GetKey(KeyCode.A)) {
            direction += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D)) {
            direction += Vector3.right;
        }

        direction.Normalize();
        characterController.Move(transform.TransformDirection(direction) * currentSpeed * Time.deltaTime);

        rotationX += -Input.GetAxis("Mouse Y") * mouseSensitivity;
        rotationX = Mathf.Clamp(rotationX, -75.0f, 75.0f);
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
            Debug.Log("HELLO");
            if (visibleObject.tag == "Key") {
                lockedText.SetActive(false);
            } else if ((visibleObject.tag == "Door" && visibleObject.GetComponent<Door>().isOpen) || (visibleObject.tag == "Safe" && visibleObject.GetComponent<Safe>().isOpen) ||(visibleObject.tag == "Lever" && visibleObject.GetComponent<Lever>().isUp) || (visibleObject.tag == "Queen" && (!visibleObject.GetComponent<Queen>().canKill || !visibleObject.GetComponent<Queen>().isAlive)) || (pickupText != null && pickupText.activeSelf)) {
                visibleObject.text.SetActive(false);
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
            if (Input.GetKey(KeyCode.E)) {
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

    private IEnumerator PlayEnding() {
        music.Stop();
        kingText.SetActive(false);
        black.SetActive(true);
        kill.Play();
        yield return new WaitForSeconds(3);
    }

}