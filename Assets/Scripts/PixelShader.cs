using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelShader : MonoBehaviour {
    public bool active;
    private Material material;

    // Creates a private material used to the effect
    void Awake ()
    {
        material = new Material( Shader.Find("Hidden/PixelShader") );
    }
    
    // Postprocess the image
    void OnRenderImage (RenderTexture source, RenderTexture destination)
    {
        if (!active)
        {
            Graphics.Blit (source, destination);
            return;
        }
        material.SetFloat("_Proximity", 0.5f);
        material.SetFloat("_Strength", 0.5f);
        Graphics.Blit (source, destination, material);
    }
}