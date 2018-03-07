using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetTile
{  
    PlanetController parentPlanet;

    TileDefinition tileDefinition;

    PlanetTile adjacentTileUp;
    PlanetTile adjacentTileDown;
    PlanetTile adjacentTileLeft;
    PlanetTile adjacentTileRight;

    int imageIndex;

    // Tile bonus
    TileBonusType bonusType = TileBonusType.None;

    float bonusValue = 0f;

    public PlanetTile(PlanetController owner)
    {
        parentPlanet = owner;
    }

    public TileDefinition GetDefinition()
    {
        return tileDefinition;
    }

    public void SetDefinition(TileDefinition definition)
    {
        tileDefinition = definition;
        imageIndex = tileDefinition.GetRandomImageIndex();
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

    public Sprite GetImage()
    {
        return tileDefinition.GetImage(imageIndex);
    }
}
