/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanetController : MonoBehaviour 
{
    [SerializeField]
    System.Guid uniqueIndentifier;

    [SerializeField]
    string displayName;

    [SerializeField]
    PlanetDefinition planetDefinition;

    [SerializeField]
    int planetSize;

    [SerializeField]
    float population;

    [SerializeField]
    float populationMax;

    [SerializeField]
    float taxRate;

    PlanetTile[] planetTiles = new PlanetTile[0];

    // Use this for initialization
    void Start () 
	{
        uniqueIndentifier = System.Guid.NewGuid();
        GalaxyManager.instance.AddPlanet(uniqueIndentifier, this);

        // Testing add all planets to player empire
        EmpireManager.instance.GetPlayerEmpire().AddPlanet(this);
    }
	
	// Update is called once per frame
	void Update () 
	{
	
	}

    public System.Guid GetUniqueIndentifier()
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

        if (planetDefinition.colonizable)
        {
            CreatePlanetTiles(planetSize);
            GenerateRandomTileData();
        }
    }

    public void SetPlanetType(PlanetDefinition typeDefinition)
    {
        planetDefinition = typeDefinition;
    }

    public PlanetDefinition GetTypeDefinition()
    {
        return planetDefinition;
    }

    public PlanetTile[] GetPlanetTiles()
    {
        return planetTiles;
    }

    public void CreatePlanetTiles(int tileCount)
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

        planetTiles = new PlanetTile[tileCount];

        // Create tiles
        for (int i = 0; i < tileCount; i++)
        {
            planetTiles[i] = new PlanetTile(this);
            planetTiles[i].SetDefinition(planetDefinition.GetRandomTileDefinition());
        }

        // Assign adjacency
        for (int i = 0; i < columnCount; i++)
        {
            for (int j = 0; j < rowCount; j++)
            {
                int index = i * rowCount + j;
                if (index < tileCount)
                {
                    int indexDown = (i + 1) * rowCount + j;
                    int indexRight = i * rowCount + j + 1;

                    // Adjacency Up
                    if (i != 0)
                    {
                        planetTiles[index].SetAdjacentTileUp(planetTiles[(i - 1) * rowCount + j]);
                    }

                    // Adjacency Down
                    if (i != columnCount - 1 && indexDown < tileCount)
                    {
                        planetTiles[index].SetAdjacentTileUp(planetTiles[indexDown]);
                    }

                    // Adjacency Left
                    if (j != 0)
                    {
                        planetTiles[index].SetAdjacentTileUp(planetTiles[i * rowCount + j - 1]);
                    }

                    // Adjacency Right
                    if (j != rowCount - 1 && indexRight < tileCount)
                    {
                        planetTiles[index].SetAdjacentTileUp(planetTiles[indexRight]);
                    }
                }
                else
                {
                    break;
                }
            }
        }
    }

    public void GenerateRandomTileData()
    {
        foreach(PlanetTile tile in planetTiles)
        {
            tile.GenerateRandomTileData();
        }
    }

    public void ChangeDay()
    {
        foreach (PlanetTile tile in planetTiles)
        {
            tile.AddDay();
        }
    }
}
