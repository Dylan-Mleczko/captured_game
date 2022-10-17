using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dust : MonoBehaviour
{
    Quaternion iniRot;

    void Start()
    {
        iniRot = transform.rotation;
    }

    void Update()
    {
        transform.rotation = iniRot;
    }
}
