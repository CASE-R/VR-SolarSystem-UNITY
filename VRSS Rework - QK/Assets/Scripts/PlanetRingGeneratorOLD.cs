using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetRingGenerator : MonoBehaviour // https://www.youtube.com/watch?v=Rze4GEFrYYs
{
    [Range(3, 360)]
    public int segments = 3; // Determines resolution of ring
    public float innerRadius = 0.6f; // Where the ring starts. Radius of sphere is half a unity unit hence scale of ring = 1.2
    public float thickness = 0.5f;
    public Material ringMaterial;

    //cached references
    GameObject ring;
    Mesh ringMesh;
    MeshFilter ringMF;
    MeshRenderer ringMR;


    // Start is called before the first frame update
    private void OnValidate()
    {
        //create ring object
        ring = new GameObject(name + " Ring");
        ring.transform.parent = gameObject.transform;
        ring.transform.localScale = Vector3.one;
        ring.transform.localPosition = Vector3.zero;
        ring.transform.localRotation = Quaternion.identity;

        ringMF = ring.AddComponent<MeshFilter>();
        ringMesh = ringMF.mesh;
        ringMR = ring.AddComponent<MeshRenderer>();
        ringMR.material = ringMaterial;

        // build ring mesh
        Vector3[] vertices = new Vector3[(segments + 1) * 2 * 2];
        int[] triangles = new int[segments * 6 * 2];
        Vector2[] uv = new Vector2[(segments + 1) * 2 * 2];
        int halfway = (segments + 1) * 2;

        for (int i = 0; i < segments + 1; i++)
        {
            float progress = (float)i / (float)segments;
            float angle = Mathf.Deg2Rad * progress * 360;
            float x = Mathf.Sign(angle);
            float z = Mathf.Cos(angle);

            vertices[i*2] = vertices[i*2 + halfway] = new Vector3(x, 0f, z) * (innerRadius * thickness);
            vertices[i * 2 + 1] = vertices[i * 2 + halfway + 1] = new Vector3(x, 0f, z) * innerRadius;
            uv[i*2] = uv[i*2*halfway] = new Vector2(progress, 0f);
            uv[i * 2 + 1] = uv[i * 2 + 1 + halfway] = new Vector2(progress, 1f);

            if (i != segments)
            {
                triangles[i * 12] = i * 2;
                triangles[i * 12 + 1] = triangles[i * 12 + 4] = (i + 1 * 2);
                triangles[i * 12 + 2] = triangles[i * 12 + 3] = i*2+1;
                triangles[i * 12 + 5] = (i + 1) * 2 + 1;

                triangles[i * 12 + 6] = i * 2 + halfway;
                triangles[i * 12 + 7] = triangles[i * 12 + 10] = i*2+1 + halfway;
                triangles[i * 12 + 8] = triangles[i * 12 + 9] = (i + 1 * 2) + halfway;
                triangles[i * 12 + 11] = (i + 1) * 2 + 1 + halfway;
            }
        }

        ringMesh.vertices = vertices;
        ringMesh.triangles = triangles;
        ringMesh.uv = uv;
        ringMesh.RecalculateNormals();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
