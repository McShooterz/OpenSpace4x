using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDefinition
{



    public string displayName;

    public string description;








    //Default constructor for xml serialization and setting default values
    public BuildingDefinition()
    {
        displayName = "Error";
        description = "Error";
    }

    public string GetDisplayName()
    {
        return ResourceManager.instance.GetLocalization(displayName);
    }

    public string GetDescription()
    {
        return ResourceManager.instance.GetLocalization(description);
    }
}
