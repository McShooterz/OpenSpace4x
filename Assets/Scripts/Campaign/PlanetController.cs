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

public class PlanetController : MonoBehaviour 
{
    [SerializeField]
    Guid uniqueIndentifier;

    [SerializeField]
    string displayName;

    [SerializeField]
    PlanetTypeDefinition planetType;

    [SerializeField]
    int planetSize;

    [SerializeField]
    float population;

    [SerializeField]
    float populationMax;

    [SerializeField]
    float taxRate;

    PlanetTileData[,] planetTiles;

    // Use this for initialization
    void Start () 
	{
        uniqueIndentifier = Guid.NewGuid();
        GalaxyManager.instance.AddPlanet(uniqueIndentifier, this);



    }
	
	// Update is called once per frame
	void Update () 
	{
	
	}

    public Guid GetUniqueIndentifier()
    {
        return uniqueIndentifier;
    }

    public void SetLightSourcePosition(Vector3 lightSourcePostion)
    {
        Renderer[] planetRenderers = GetComponentsInChildren<Renderer>();

        for (int i = 0; i < planetRenderers.Length; i++)
        {
            planetRenderers[i].sharedMaterial.SetVector("_SunPos", lightSourcePostion);
        }
    }

    public string GetDisplayName()
    {
        return displayName;
    }

    public void SetDisplayName(string name)
    {
        displayName = name;
    }

    public int GetSize()
    {
        return planetSize;
    }

    public void SetSize(int size)
    {
        planetSize = size;

        CreatePlanetTilesData(planetSize);
    }

    public void SetPlanetType(PlanetTypeDefinition typeDefinition)
    {
        planetType = typeDefinition;
    }

    public PlanetTypeDefinition GetTypeDefinition()
    {
        return planetType;
    }

    public PlanetTileData[,] GetPlanetTiles()
    {
        return planetTiles;
    }

    public List<PlanetTileData> GetPlanetTilesList()
    {
        List<PlanetTileData> planetTilesList = new List<PlanetTileData>();

        for (int i = 0; i < planetTiles.GetLength(0); i++)
        {
            for (int j = 0; j < planetTiles.GetLength(1); j++)
            {
                if (planetTiles[i, j] != null)
                {
                    planetTilesList.Add(planetTiles[i, j]);
                }
            }
        }

        return planetTilesList;
    }

    public void CreatePlanetTilesData(int tileCount)
    {
        // Makes 25 the maximum size of a planet at 5x5 tiles
        tileCount = Mathf.Clamp(tileCount, 1, 25);

        // Calculate the demensions of the grid
        int rowCount = Mathf.CeilToInt(Mathf.Sqrt(tileCount));
        int remainderRowsCount = tileCount % rowCount;
        int columnCount = tileCount / rowCount;
        if (remainderRowsCount > 0)
        {
            columnCount += 1;
        }

        planetTiles = new PlanetTileData[columnCount, rowCount];

        // Create tiles datas
        for (int i = 0; i < columnCount; i++)
        {
            for (int j = 0; j < rowCount; j++)
            {
                if (remainderRowsCount == 0 || i == columnCount - 1 && j < remainderRowsCount)
                {
                    planetTiles[i, j] = new PlanetTileData(this);
                }
            }
        }

        // Assign adjacency
        for (int i = 0; i < columnCount; i++)
        {
            for (int j = 0; j < rowCount; j++)
            {
                if (planetTiles[i, j] != null)
                {
                    // Adjacency Up
                    if (i != 0)
                    {
                        planetTiles[i, j].SetAdjacentTileUp(planetTiles[i - 1, j]);
                    }

                    // Adjacency Down
                    if (i != columnCount - 1)
                    {
                        planetTiles[i, j].SetAdjacentTileUp(planetTiles[i + 1, j]);
                    }

                    // Adjacency Left
                    if (j != 0)
                    {
                        planetTiles[i, j].SetAdjacentTileUp(planetTiles[i, j - 1]);
                    }

                    // Adjacency Right
                    if (j != rowCount - 1)
                    {
                        planetTiles[i, j].SetAdjacentTileUp(planetTiles[i, j + 1]);
                    }
                }
            }
        }
    }
}
