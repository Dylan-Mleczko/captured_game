using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelShader : MonoBehaviour {
    public bool active;
    private Material material;
    
    // values for distance to closest piece
    public List<GameObject> pieces;
    // private Object[]ok> pieces;
    // private GameObject[] pieces;

    void Awake()
    {
        material = new Material(Shader.Find("Hidden/PixelShader"));
    }

    void Start() {
        material.SetInt("_ScreenWidth", Screen.width);
        material.SetInt("_ScreenHeight", Screen.height);

        // pieces = new GameObject[100];
        // var rooks = FindObjectOfType<Rook>();
        // Debug.Log("rooks");
        // foreach (var rook in rooks) {
        //     Debug.Log("yeet");
        // }
        // pieces = null;
        // pieces = 
        // pieces = FindObjectOfType<Rook>();
        // pieces.AddRange(FindObjectOfType<Rook>());
        // pieces.AddRange(FindObjectOfType<Bishop>().Select(x => x.transform.GetChild(0).gameObject));
        // pieces.AddRange(FindObjectOfType<Rook>().Select(x => x.transform.GetChild(0).gameObject));
        // pieces.AddRange(FindObjectOfType<Queen>().Select(x => x.transform.GetChild(0).gameObject));
    }

    public static Vector2 xz(Vector3 vec) {
        return new Vector2(vec.x, vec.z);
    }

    void Update () {
        float minDistance = float.MaxValue;

        // GameObject curPiece;
        foreach (GameObject piece in pieces)
        {
            if (piece.activeInHierarchy) {
                minDistance = Math.Min(minDistance, Vector2.Distance(xz(transform.position), xz(piece.transform.position)));
            }
            // curPiece = piece.transform.GetChild(0).gameObject;
        }

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

        Graphics.Blit (source, destination, material);
    }
}