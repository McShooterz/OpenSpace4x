using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderManager : MonoBehaviour
{

    [SerializeField]
    Material borderMaterial;

    [SerializeField]
    List<BorderController> borders = new List<BorderController>();

	// Use this for initialization
	void Start ()
    {
        Vector3[] vertices = new Vector3[60];

        float anglePart = (2f * Mathf.PI) / 61;
        float angle = 0;

        for(int i = 0; i < vertices.Length; i++)
        {
            Vector2 vertice2D = StaticHelpers.GetUnitVector2(angle);
            vertices[i] = new Vector3(vertice2D.x, 0f, vertice2D.y);

            angle += anglePart;
        }

        BorderController border = CreateBorder();
        border.CreateMesh(vertices);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    BorderController CreateBorder()
    {
        GameObject border = new GameObject();

        border.name = "Empire Border";

        BorderController borderController = border.AddComponent<BorderController>();

        borderController.Initialize();

        return borderController;
    }

}
