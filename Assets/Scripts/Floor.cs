using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    [Header("Pattern")]
    [SerializeField] private Material tileMaterial1;
    [SerializeField] private Material tileMaterial2;

    [Header("Construction")]
    [SerializeField][Min(0)] private float tileSize;
    [SerializeField][Min(1)] private int tileCountX = 1;
    [SerializeField][Min(1)] private int tileCountY = 1;

    [Header("Rippling")]
    [SerializeField][Min(0)] private float amplitude = 0.05f;
    [SerializeField][Min(0)] private float spread = 2f;

    [Header("Player")]
    [SerializeField] private GameObject player = null;

    private GameObject[,] tiles;

    private void Awake()
    {
        GenerateFloorTiles(tileCountX, tileCountY);
        foreach (GameObject tile in tiles)
        {
            tile.GetComponent<Renderer>().sharedMaterial.SetVector("_RippleOrigin", new Vector2(player.transform.position.x, player.transform.position.z));
            tile.GetComponent<Renderer>().sharedMaterial.SetFloat("_Spread", spread);
            tile.GetComponent<Renderer>().sharedMaterial.SetFloat("_Amplitude", amplitude);
        }
    }

    void Update()
    {
        foreach (GameObject tile in tiles)
        {
            tile.GetComponent<Renderer>().sharedMaterial.SetVector("_RippleOrigin", new Vector2(player.transform.position.x, player.transform.position.z));
        }

        if (Input.GetKeyUp(KeyCode.E)) {
            foreach (GameObject tile in tiles)
            {
                tile.GetComponent<Renderer>().sharedMaterial.SetVector("_LandOrigin", new Vector2(player.transform.position.x, player.transform.position.z));
                tile.GetComponent<Renderer>().sharedMaterial.SetFloat("_LandTime", Time.time);
            }
        }
    }

    private void GenerateFloorTiles(int tileCountX, int tileCountY)
    {
        tiles = new GameObject[tileCountX, tileCountY];
        Material curMaterial;
        for (int x = 0; x < tileCountX; x++)
        {
            for (int y = 0; y < tileCountY; y++)
            {
                curMaterial = (x + y) % 2 == 0 ? tileMaterial1 : tileMaterial2;
                tiles[x, y] = GenerateTile(tileSize, x, y, curMaterial);
            }
        }
    }

    private GameObject GenerateTile(float tileSize, int x, int y, Material material)
    {
        // TODO: add tileSize square size transformation
        // CHECK THE ROTATIONS ON THIS OBJECT
        GameObject tile = new GameObject(string.Format("X:{0}, Y:{1}", x, y));
        Debug.Log(x + ", " + y);

        Mesh mesh = new Mesh();
        tile.AddComponent<MeshFilter>().mesh = mesh;
        tile.AddComponent<MeshRenderer>().material = material;

        Vector3[] vertices = new Vector3[4];
        Vector2[] uvs = new Vector2[4];

        vertices[0] = new Vector3(x * tileSize, 0, y * tileSize);
        vertices[1] = new Vector3(x * tileSize, 0, (y + 1) * tileSize);
        vertices[2] = new Vector3((x + 1) * tileSize, 0, y * tileSize);
        vertices[3] = new Vector3((x + 1) * tileSize, 0, (y + 1) * tileSize);

        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }

        int[] tris = new int[] { 0, 1, 2, 1, 3, 2 };

        mesh.vertices = vertices;
        mesh.triangles = tris;
        mesh.uv = uvs;
        mesh.RecalculateNormals();

        tile.AddComponent<MeshCollider>();
        tile.transform.position = tile.transform.position + new Vector3(0.5f, 0, 0.5f);

        return tile;
    }

    public void landQueen() {
        // create the ripple at the given position using the creation time as the current time
        foreach (GameObject tile in tiles)
        {
            tile.GetComponent<Renderer>().sharedMaterial.SetVector("_LandOrigin", new Vector2(player.transform.position.x, player.transform.position.z));
            tile.GetComponent<Renderer>().sharedMaterial.SetFloat("_LandTime", Time.time);
        }
    }
}
