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
    public GalaxyManager instance;


    //Hides public variables below this from appearing in the inspector
    [HideInInspector]


    Dictionary<Guid, SectorData> Sectors = new Dictionary<Guid, SectorData>();

    List<PlanetData> Planets = new List<PlanetData>();

    void Awake()
    {
        //Make a Singleton
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
        //This stays in every scene
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start ()
    {
        //BuildSectorDataGrid(5);
        //InstantiateSectors();

    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
    /*
    public void BuildSectorDataGrid(int Dimensions)
    {
        //Get the size of a sector
        float SectorWidth = gameController.resourceManager.SectorPrefab.transform.GetChild(0).GetComponent<Renderer>().bounds.size.x;
        float SectorHeight = gameController.resourceManager.SectorPrefab.transform.GetChild(0).GetComponent<Renderer>().bounds.size.z;

        Vector3 initialPos = new Vector3(-SectorWidth * Dimensions / 2f + SectorWidth / 2, -1, Dimensions / 2f * SectorHeight - SectorHeight / 2);

        for (int y = 0; y < Dimensions; y++)
        {
            for (int x = 0; x < Dimensions; x++)
            {
                SectorData sectorData = new SectorData();
                float offset = 0;
                if (y % 2 != 0)
                    offset = SectorWidth / 2;
                sectorData.Position = new Vector3(initialPos.x + offset + x * SectorWidth, -3f, initialPos.z - y * SectorHeight * 0.75f);
                Sectors.Add(sectorData.GUID, sectorData);
            }
        }
    }

    public void InstantiateSectors()
    {
        foreach(KeyValuePair<Guid, SectorData> sd in Sectors)
        {
            ResourceManager.CreateSector(sd.Value);
        }
    }

    public void HideAll()
    {
        foreach(KeyValuePair<Guid, SectorData> keyVal in Sectors)
        {
            keyVal.Value.sector.gameObject.SetActive(false);
        }
    }

    public void UnHideAll()
    {
        foreach (KeyValuePair<Guid, SectorData> keyVal in Sectors)
        {
            keyVal.Value.sector.gameObject.SetActive(true);
        }
    }*/
}
