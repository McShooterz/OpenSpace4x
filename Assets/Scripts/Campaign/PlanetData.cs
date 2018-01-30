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

[System.Serializable]
public class PlanetData
{
    [SerializeField]
    string displayName;

    [SerializeField]
    PlanetType planetType;

    [SerializeField]
    int planetSize;

    [SerializeField]
    PlanetTileData [,] planetTiles;

    [SerializeField]
    float population;

    [SerializeField]
    float populationMax;

    [SerializeField]
    float taxRate;

    public PlanetData(int size)
    {
        planetSize = size;
        CreatePlanetTilesData(planetSize);
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

    public PlanetType GetPlanetType()
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

        for (int i = 0; i < planetTiles.GetLength(0); i ++)
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

        for (int i = 0; i < columnCount; i++)
        {
            for (int j = 0; j < rowCount; j++)
            {
                if (remainderRowsCount == 0 || i == columnCount -1 && j < remainderRowsCount)
                {
                    planetTiles[i, j] = new PlanetTileData(this);
                }
            }
        }
    }
}
