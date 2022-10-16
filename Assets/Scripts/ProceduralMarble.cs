using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class ProceduralTexture : MonoBehaviour
{
    public bool isDirty = true;
    public int noiseLayer = 0;
    public int turbolenceSize = 8;
    public float turbolenceScaleX = 4.0f;
    public float turbolenceScaleY = 4.0f;
    public float turbolenceStrength = 1.0f;
    public int pixWidth = 512;
    public int pixHeight = 512;
    public float scale = 1.0f;
    public Color fromColor = Color.white;
    public Color toColor = Color.black;
    public float cutoff = -1.0f;

    public float xOrg;
    public float yOrg;

    private Texture2D noiseTex;
    private Color[] pix;

    public int width
    {
        get { return pixWidth; }
    }

    public int height
    {
        get { return pixHeight; }
    }

    void Awake()
    {
        init();

        if (gameObject.GetComponent<Renderer>())
            gameObject.GetComponent<Renderer>().material.mainTexture = noiseTex;
    }

    public Color[] pixColors()
    {
        init();
        return pix;
    }

    void init()
    {
        if (noiseTex == null || isDirty)
        {
            isDirty = false;

            // Marble defaults
            scale = 0.15f;
            turbolenceSize = 9;
            turbolenceStrength = 2.8f;

            noiseTex = new Texture2D(pixWidth, pixHeight);
            pix = new Color[noiseTex.width * noiseTex.height];

            for (int y = 0; y < noiseTex.height; y++)
            {
                for (int x = 0; x < noiseTex.width; x++)
                {
                    int i = y * noiseTex.width + x;

                    float xCoord = xOrg + x / (float)noiseTex.width * scale;
                    float yCoord = yOrg + y / (float)noiseTex.height * scale;

                    float xyValue = xCoord + yCoord + turbulence(x * scale, y * scale, turbolenceSize) * turbolenceStrength;
                    float sample = Mathf.Abs(Mathf.Sin(xyValue));

                    float fromRatio = sample;
                    float toRatio = 1.0f - sample;
                    pix[i] = new Color(fromColor.r * fromRatio, fromColor.g * fromRatio, fromColor.b * fromRatio) + new Color(toColor.r * toRatio, toColor.g * toRatio, toColor.b * toRatio);
                }
            }

            noiseTex.SetPixels(pix);
            noiseTex.Apply();
        }
    }

    void BuildTexture()
    {
        // create a material holding the texture
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        Material material = new Material(Shader.Find("Sprites/Default"));
        material.color = Color.white;
        material.SetTexture("texture", noiseTex);

        // apply material to mesh
        meshRenderer.sharedMaterial = material;
        GetComponent<Renderer>().sharedMaterial = material;
        GetComponent<Renderer>().material.mainTexture = noiseTex;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        init();

        Gizmos.DrawGUITexture(new Rect(0f, 0f, Screen.height, Screen.height), noiseTex);
    }
#endif

    float turbulence(float x, float y, float size)
    {
        float value = 0.0f;
        float initialSize = size;

        while (size >= 1)
        {
            value += Mathf.PerlinNoise(x / size, y / size) * size;
            size /= 2.0f;
        }

        return (value / initialSize);
    }
}