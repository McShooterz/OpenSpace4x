/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class TextMeshController : MonoBehaviour
{
    TextMesh textMesh;

    // Use this for initialization
    void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }

    public void Initialize(Color color, int fontSize, float characterSize, float lineSpacing, string text)
    {
        gameObject.AddComponent<MeshRenderer>();
        textMesh = gameObject.AddComponent<TextMesh>();
        textMesh.alignment = TextAlignment.Center;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.characterSize = characterSize;
        textMesh.lineSpacing = lineSpacing;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.text = text;
    }

    public void SetText(string text)
    {
        textMesh.text = text;
    }

    public void SetColor(Color color)
    {
        textMesh.color = color;
    }
}
