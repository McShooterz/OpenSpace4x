﻿using System.Collections;
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

    BuildingDefinition currentBuilding;

    BuildingDefinition nextBuilding;

    int buildingDaysProgress;

    public PlanetTile(PlanetController owner)
    {
        parentPlanet = owner;
    }

    public void GenerateRandomTileData()
    {
        imageIndex = tileDefinition.GetRandomImageIndex();
        TileDefinition.BonusEntry bonus = tileDefinition.GetRandomTileBonus();
        bonusType = bonus.bonus;
        bonusValue = bonus.value;
    }

    public TileDefinition GetDefinition()
    {
        return tileDefinition;
    }

    public void SetDefinition(TileDefinition definition)
    {
        tileDefinition = definition;
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

    public BuildingDefinition GetCurrentBuilding()
    {
        return currentBuilding;
    }

    public BuildingDefinition GetNextBuilding()
    {
        return nextBuilding;
    }

    public void SetCurrentBuilding(BuildingDefinition building)
    {
        currentBuilding = building;
    }

    public void SetNextBuilding(BuildingDefinition building)
    {
        nextBuilding = building;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>If current building has changed</returns>

    public bool AddDay()
    {
        if (nextBuilding != null)
        {
            buildingDaysProgress++;

            int nextBuildingCost = nextBuilding.GetCostDays(1.0f);

            if (nextBuildingCost > buildingDaysProgress)
            {
                buildingDaysProgress = 0;
                currentBuilding = nextBuilding;
                nextBuilding = null;
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }

    public float GetBuidlingProgress(float modifier)
    {
        if (nextBuilding != null)
        {
            return Mathf.Min(buildingDaysProgress / nextBuilding.GetCostDays(modifier), 1.0f);
        }

        return 0f;
    }

    public bool HasBonus()
    {
        return bonusType != TileBonusType.None;
    }

    public Sprite GetBonusIcon()
    {
        switch (bonusType)
        {
            case TileBonusType.None:
                {
                    return ResourceManager.instance.GetErrorTexture();
                }
            case TileBonusType.Population:
                {
                    return ResourceManager.instance.GetIconTexture("Icon_Crew");
                }
            case TileBonusType.Food:
                {
                    return ResourceManager.instance.GetIconTexture("Icon_Food");
                }
            case TileBonusType.Metal:
                {
                    return ResourceManager.instance.GetIconTexture("Icon_Metal");
                }
            case TileBonusType.Cystal:
                {
                    return ResourceManager.instance.GetIconTexture("Icon_Crystal");
                }
            case TileBonusType.Morale:
                {
                    return ResourceManager.instance.GetIconTexture("Icon_MoralHappy");
                }
            case TileBonusType.SciencePhysics:
                {
                    return ResourceManager.instance.GetIconTexture("Icon_Physics");
                }
            case TileBonusType.ScienceSociety:
                {
                    return ResourceManager.instance.GetIconTexture("Icon_Society");
                }
            case TileBonusType.ScienceEngineering:
                {
                    return ResourceManager.instance.GetIconTexture("Icon_Engineering");
                }
            default:
                {
                    return ResourceManager.instance.GetErrorTexture();
                }
        }
    }
}