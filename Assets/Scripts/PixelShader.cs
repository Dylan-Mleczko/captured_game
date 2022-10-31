using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelShader : MonoBehaviour {
    public bool active;
    private Material material;
    
    // values for distance to closest piece
    public List<GameObject> pieces;
    private float minDistance;

    void Awake ()
    {
        material = new Material(Shader.Find("Hidden/PixelShader"));
    }

    public static Vector2 xz(Vector3 vec) {
        return new Vector2(vec.x, vec.z);
    }

    void Update () {
        float minDistance = float.MaxValue;

        foreach (GameObject piece in pieces)
        {
            minDistance = Math.Min(minDistance, Vector2.Distance(xz(transform.position), xz(piece.transform.position)));
        }

        Debug.Log(minDistance);
        material.SetFloat("_Proximity", minDistance);
    }
    
    // Postprocess the image
    void OnRenderImage (RenderTexture source, RenderTexture destination)
    {
        if (!active)
        {
            Graphics.Blit (source, destination);
            return;
        }

        // material.SetFloat("_Proximity", 0.5f);
        // material.SetFloat("_Strength", 0.5f);
        Graphics.Blit (source, destination, material);
    }
}