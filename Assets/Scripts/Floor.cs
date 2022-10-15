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
    [SerializeField][Min(1)] private int tileCountY = 1 ;

    private GameObject[,] tiles;

    private void Awake()
    {
        GenerateFloorTiles(tileCountX, tileCountY);
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
        GameObject tile = new GameObject(string.Format("X:{0}, Y:{1}", x, y));
        tile.transform.parent = transform;

        Mesh mesh = new Mesh();
        tile.AddComponent<MeshFilter>().mesh = mesh;
        tile.AddComponent<MeshRenderer>().material = material;

        Vector3[] vertices = new Vector3[4];
        Vector2[] uvs = new Vector2[vertices.Length];

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

        tile.AddComponent<BoxCollider>();

        return tile;
    }
}
