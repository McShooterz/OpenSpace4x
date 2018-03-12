using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDefinition
{
    public string name;

    public string description;

    public string image;

    public float costMoney;

    public float costMetal;

    public float costCrystal;

    public float costInfluence;

    public int costDays;

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
}
