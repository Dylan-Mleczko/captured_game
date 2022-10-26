using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class DataManager : MonoBehaviour {

    [SerializeField] GameObject button1;
    [SerializeField] GameObject button2;
    public static DataManager handle;
    public Data data;
    public bool isNew;

    void Awake() {
        if (handle != null) {
            Destroy(gameObject);
            return;
        }

        handle = this;
        LoadData();
        isNew = false;

        if (data.level == 0) {
            isNew = true;
            button1.SetActive(true);
        } else {
            button2.SetActive(true);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void LoadData() {
        string FILEPATH = Application.persistentDataPath + "/savedata.json";

        if (File.Exists(FILEPATH)) {
            data = JsonUtility.FromJson<Data>(File.ReadAllText(FILEPATH));
        } else {
            data = new Data(0, 5, 5, 5, 10);
        }
    }

    public void NewGame() {
        data.level = 1;
        SceneManager.LoadScene(data.level);
    }

    public void Continue() {
        SceneManager.LoadScene(data.level);
    }

    public void Retry() {
        SceneManager.LoadScene(data.level);
    }

    public void NextLevel() {
        SceneManager.LoadScene(++data.level);
    }

    public void FinishGame() {
        data.level = 0;
        SceneManager.LoadScene(0);
    }

    public void Exit() {
        File.WriteAllText(Application.persistentDataPath + "/savedata.json", JsonUtility.ToJson(data));
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}