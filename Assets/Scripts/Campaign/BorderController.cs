using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderController : MonoBehaviour
{
    MeshFilter meshFilter;

    MeshRenderer meshRender;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void Initialize()
    {
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRender = gameObject.AddComponent<MeshRenderer>();

        meshRender.sharedMaterial = (Material)Resources.Load("Materials/EmpireBorder");
    }

    public void CreateMesh(Vector3[] vertices)
    {
        Vector2[] vertices2D = new Vector2[vertices.Length];
        int[] triangles;

        for(int i = 0; i < vertices.Length; i++)
        {
            vertices2D[i] = new Vector2(vertices[i].x, vertices[i].z);
        }

        Triangulator triangulator = new Triangulator(vertices2D);
        triangles = triangulator.Triangulate();

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        meshFilter.mesh = mesh;
    }
}
