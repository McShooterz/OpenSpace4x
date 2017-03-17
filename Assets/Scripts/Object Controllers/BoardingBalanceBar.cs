/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class BoardingBalanceBar : MonoBehaviour
{
    Material material;

	// Use this for initialization
	void Start ()
    {
    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }

    public void Initialize()
    {
        material = GetComponent<Renderer>().material;
    }

    public void SetGradient(float value)
    {
        value = Mathf.Clamp01(value);
        material.SetFloat("_Gradient", value);
    }

    public void SetColors(Color main, Color secondary)
    {
        material.SetColor("_ColorMain", main);
        material.SetColor("_ColorSecondary", secondary);
    }
}
