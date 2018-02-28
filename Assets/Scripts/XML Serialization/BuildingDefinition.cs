using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDefinition
{
    public string name;

    public string description;

    public string icon;


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
}
