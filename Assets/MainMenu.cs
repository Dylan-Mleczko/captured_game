using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PointerEnter() {
        transform.localScale = new Vector2(1.2f, 1.2f);
    }

    public void PointerExit() {
        transform.localScale = new Vector2(1f, 1f);
    }
}
