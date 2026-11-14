using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class DonutGenerator : MonoBehaviour
{
    [Header("Donut Shape")]
    [Min(0.1f)] public float radius = 1f;
    [Min(0.05f)] public float thickness = 0.3f;

    [Header("Smoothness")]
    [Range(8, 128)] public int radialSegments = 48;
    [Range(6, 64)] public int tubeSegments = 24;

    private Mesh generatedMesh;

    private void OnEnable()
    {
        GenerateDonut();
    }

    private void OnValidate()
    {
        GenerateDonut();
    }

    [ContextMenu("Generate Donut")]
    public void GenerateDonut()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();

        if (meshFilter == null)
            return;

        // Remove the previous generated mesh.
        if (generatedMesh != null)
        {
            if (meshFilter.sharedMesh == generatedMesh)
                meshFilter.sharedMesh = null;

            if (Application.isPlaying)
                Destroy(generatedMesh);
            else
                DestroyImmediate(generatedMesh);
        }

        generatedMesh = new Mesh();
        generatedMesh.name = "Generated Donut";

        int rings = Mathf.Max(8, radialSegments);
        int sides = Mathf.Max(6, tubeSegments);

        Vector3[] vertices = new Vector3[rings * sides];
        int[] triangles = new int[rings * sides * 6];

        // Create the vertices around the donut.
        for (int i = 0; i < rings; i++)
        {
            float u = 2f * Mathf.PI * i / rings;

            for (int j = 0; j < sides; j++)
            {
                float v = 2f * Mathf.PI * j / sides;

                float x = (radius + thickness * Mathf.Cos(v))
                          * Mathf.Cos(u);

                float y = thickness * Mathf.Sin(v);

                float z = (radius + thickness * Mathf.Cos(v))
                          * Mathf.Sin(u);

                vertices[i * sides + j] =
                    new Vector3(x, y, z);
            }
        }

        // Connect the vertices to form the surface.
        int t = 0;

        for (int i = 0; i < rings; i++)
        {
            int nextRing = (i + 1) % rings;

            for (int j = 0; j < sides; j++)
            {
                int nextSide = (j + 1) % sides;

                int a = i * sides + j;
                int b = nextRing * sides + j;
                int c = nextRing * sides + nextSide;
                int d = i * sides + nextSide;

                triangles[t++] = a;
                triangles[t++] = d;
                triangles[t++] = b;

                triangles[t++] = b;
                triangles[t++] = d;
                triangles[t++] = c;
            }
        }

        generatedMesh.vertices = vertices;
        generatedMesh.triangles = triangles;
        generatedMesh.RecalculateNormals();
        generatedMesh.RecalculateBounds();

        meshFilter.sharedMesh = generatedMesh;
    }
}
