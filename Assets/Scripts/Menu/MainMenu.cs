using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    public void NewGame() {
        SceneManager.LoadScene("floor1");
    }

    public void PointerEnter() {
        transform.localScale = new Vector2(1.2f, 1.2f);
    }

    public void PointerExit() {
        transform.localScale = new Vector2(1f, 1f);
    }

    public void ExitGame() {
        Application.Quit();
    }
}
