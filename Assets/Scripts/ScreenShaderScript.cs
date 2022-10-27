using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShaderScript : MonoBehaviour
{
    public bool normal;
    private Material material;

    // Creates a private material used to the effect
    void Awake ()
    {
        material = new Material( Shader.Find("Hidden/ScreenShader") );
    }
    
    // Postprocess the image
    void OnRenderImage (RenderTexture source, RenderTexture destination)
    {
        if (normal)
        {
            Graphics.Blit (source, destination);
            return;
        }
        // material.SetFloat("_bwBlend", intensity);
        Graphics.Blit (source, destination, material);
    }
}
