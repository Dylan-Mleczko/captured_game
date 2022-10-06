using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class DataManager : MonoBehaviour {

    public static DataManager handle;

    public Data data;

    void Awake() {
        if (handle != null) {
            Destroy(gameObject);
            return;
        }
        handle = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadData() {
        string FILEPATH = Application.persistentDataPath + "/savedata.json";
        if (File.Exists(FILEPATH)) {
            data = JsonUtility.FromJson<Data>(File.ReadAllText(FILEPATH));
        } else {
            data = new Data(0, 0, 5, 5, 5, 10);
        }
    }

    public void NewGame() {
        data.level = 1;
        data.notes = 0;
        SceneManager.LoadScene(1);
    }

    public void Continue() {
        SceneManager.LoadScene(data.level);
    }

    public void NextLevel() {
        SceneManager.LoadScene(++data.level);
    }

    public void FinishGame() {
        data.level = 0;
        data.notes = 0;
        SceneManager.LoadScene(0);
    }

    public void Exit() {
        File.WriteAllText(Application.persistentDataPath + "/savedata.json", JsonUtility.ToJson(data));
        Application.Quit();
    }

}