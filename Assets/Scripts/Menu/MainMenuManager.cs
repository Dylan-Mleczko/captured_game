using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    [SerializeField] bool isContinue;
    [SerializeField] Animator anim;
    [SerializeField] AudioSource sound;
    [SerializeField] GameObject black;

    public void PointerEnter() {
        if (!isContinue || !DataManager.handle.isNew) {
            transform.localScale = new Vector2(1.2f, 1.2f);
        }
    }

    public void PointerExit() {
        if (!isContinue || !DataManager.handle.isNew) {
            transform.localScale = new Vector2(1f, 1f);
        }   
    }

    public void NewGame() {
        sound.Stop();
        black.SetActive(true);
        anim.SetBool("Fade", true);
        StartCoroutine(Fade(true));
    }

    public void OpenSettings() {
        
    }

    public void ContinueGame() {
        sound.Stop();
        black.SetActive(true);
        anim.SetBool("Fade", true);
        StartCoroutine(Fade(false));
    }

    private IEnumerator Fade(bool newGame) {
        yield return new WaitForSeconds(5);
        if (newGame) {
            DataManager.handle.NewGame();
        } else {
            DataManager.handle.Continue();
        }
    }

}