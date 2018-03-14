using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDefinition
{
    public string name;

    public string description;

    public string image;

    public bool empireUnique;

    public bool planetUnique;

    public float costMoney;

    public float costMetal;

    public float costCrystal;

    public float costInfluence;

    public int costDays;

    public bool isBaseBuilding;

    public string[] upgradeToBuildings = new string[0];

    public float maintenance;

    public float money;

    public float moneyPerPop;

    public float moneyAdjacentBonus;

    public float moneyPlanetBonus;

    public float population;

    public float populationAdjacentBonus;

    public float populationPlanetBonus;

    public float populationGrowth;

    public float moral;

    public float food;

    public float foodPerFertility;

    public float foodPerPop;

    public float foodAdjacentBonus;

    public float foodPlanetBonus;

    public float metal;

    public float metalPerRichness;

    public float metalPerPop;

    public float metalAdjacentBonus;

    public float metalPlanetBonus;

    public float crystal;

    public float crystalPerRichness;

    public float crystalPerPop;

    public float crystalAdjacentBonus;

    public float crystalPlanetBonus;

    public float researchPhysics;

    public float researchPhysicsPerPop;

    public float researchPhysicsAdjacentBonus;

    public float researchPhysicsPlanetBonus;

    public float researchSociety;

    public float researchSocietyPerPop;

    public float researchSocietyAdjacentBonus;

    public float researchSocietyPlanetBonus;

    public float researchEngineering;

    public float researchEngineeringPerPop;

    public float researchEngineeringAdjacentBonus;

    public float researchEngineeringPlanetBonus;

    public float unity;

    public float unityPerPop;

    public float unityAdjacentBonus;

    public float unityPlanetBonus;

    public int commandPoints;

    //Default constructor for xml serialization and setting default values
    public BuildingDefinition()
    {
        name = "Error";
        description = "Error";
    }

    public string GetDisplayName()
    {
        return ResourceManager.instance.GetLocalization(name);
    }

    public string GetDescription()
    {
        return ResourceManager.instance.GetLocalization(description);
    }

    public Sprite GetImage()
    {
        return ResourceManager.instance.GetBuildingImage(image);
    }

    public float GetCostDays(float modifier)
    {
        return costDays * modifier;
    }

    public List<BuildingDefinition> GetUpgradeBuildings()
    {
        List<BuildingDefinition> buildings = new List<BuildingDefinition>();

        foreach (string buildingName in upgradeToBuildings)
        {
            BuildingDefinition building = ResourceManager.instance.GetBuilding(buildingName);

            if (building != null)
            {
                buildings.Add(building);
            }
        }

        return buildings;
    }
}
