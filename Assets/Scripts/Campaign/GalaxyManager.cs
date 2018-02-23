/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GalaxyManager : MonoBehaviour
{
    public static GalaxyManager instance;

    Dictionary<Guid, PlanetData> planetDatas = new Dictionary<Guid, PlanetData>();

    void Awake()
    {
        //Make a Singleton
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }

        //This stays in every scene
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start ()
    {


    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}


    public void AddPlanetData(Guid guid, PlanetData planetData)
    {
        if (planetDatas.ContainsKey(guid))
        {
            planetDatas[guid] = planetData;
        }
        else
        {
            planetDatas.Add(guid, planetData);
        }
    }

    public PlanetData GetPlanetData(Guid guid)
    {
        PlanetData planetData;

        if (planetDatas.TryGetValue(guid, out planetData))
        {
            return planetData;
        }

        return null;
    }
}
