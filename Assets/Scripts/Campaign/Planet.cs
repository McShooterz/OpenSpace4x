/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour 
{
    [SerializeField]
    PlanetData planetData;

    [SerializeField]
    GameObject planetObject;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	

    public PlanetData GetPlanetData()
    {
        return planetData;
    }

    public void SetPlanetData(PlanetData data)
    {
        planetData = data;
    }

}
