using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetTile
{  
    PlanetController parentPlanet;

    PlanetTile adjacentTileUp;
    PlanetTile adjacentTileDown;
    PlanetTile adjacentTileLeft;
    PlanetTile adjacentTileRight;

    // Tile bonus
    TileBonusType bonusType = TileBonusType.none;

    float bonusValue = 0f;

    public PlanetTile(PlanetController owner)
    {
        parentPlanet = owner;
    }

    public void SetAdjacentTileUp(PlanetTile tile)
    {
        adjacentTileUp = tile;
    }

    public void SetAdjacentTileDown(PlanetTile tile)
    {
        adjacentTileDown = tile;
    }

    public void SetAdjacentTileLeft(PlanetTile tile)
    {
        adjacentTileLeft = tile;
    }

    public void SetAdjacentTileRight(PlanetTile tile)
    {
        adjacentTileRight = tile;
    }

    public PlanetTile GetAdjacentTileUp()
    {
        return adjacentTileUp;
    }

    public PlanetTile GetAdjacentTileDown()
    {
        return adjacentTileDown;
    }

    public PlanetTile GetAdjacentTileLeft()
    {
        return adjacentTileLeft;
    }

    public PlanetTile GetAdjacentTileRight()
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
}
