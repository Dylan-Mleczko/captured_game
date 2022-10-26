using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnorePauseSound : MonoBehaviour {

    void Start() {
        gameObject.GetComponent<AudioSource>().ignoreListenerPause = true;
    }

}