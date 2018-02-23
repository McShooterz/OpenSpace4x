using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlanetTileData
{  
    PlanetData planetData;

    PlanetTileData adjacentTileUp;
    PlanetTileData adjacentTileDown;
    PlanetTileData adjacentTileLeft;
    PlanetTileData adjacentTileRight;

    // Tile bonus
    TileBonusType bonusType = TileBonusType.none;

    float bonusValue = 0f;

    public PlanetTileData(PlanetData data)
    {
        planetData = data;
    }

    public void SetAdjacentTileUp(PlanetTileData tile)
    {
        adjacentTileUp = tile;
    }

    public void SetAdjacentTileDown(PlanetTileData tile)
    {
        adjacentTileDown = tile;
    }

    public void SetAdjacentTileLeft(PlanetTileData tile)
    {
        adjacentTileLeft = tile;
    }

    public void SetAdjacentTileRight(PlanetTileData tile)
    {
        adjacentTileRight = tile;
    }

    public PlanetTileData GetAdjacentTileUp()
    {
        return adjacentTileUp;
    }

    public PlanetTileData GetAdjacentTileDown()
    {
        return adjacentTileDown;
    }

    public PlanetTileData GetAdjacentTileLeft()
    {
        return adjacentTileLeft;
    }

    public PlanetTileData GetAdjacentTileRight()
    {
        return adjacentTileRight;
    }

    public void SetTileBonusType(TileBonusType bonus)
    {
        bonusType = bonus;
    }

    public TileBonusType GetTileBonusType()
    {
        return bonusType;
    }

    public void SetTileBonusValue(float value)
    {
        bonusValue = value;
    }

    public float GetTileBonusValue()
    {
        return bonusValue;
    }

    public enum TileBonusType
    {
        none,
        food,
        metal,
        cystal,
        population,
        sciencePhysics,
        scienceSociety,
        scienceEngineering,
        morale,
    }
}
