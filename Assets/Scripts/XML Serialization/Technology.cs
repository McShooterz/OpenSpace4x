/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

[System.Serializable]
public class Technology
{
    public TechnologyType technologyType = TechnologyType.physics;

    public string name;

    public string description;

    public int baseCost;

    public string[] requiredTechnologies;

    // effects

    public string[] unlockShipHulls;





    public string GetName()
    {
        return ResourceManager.instance.GetLocalization(name);
    }

    public string GetDescription()
    {
        return ResourceManager.instance.GetLocalization(description);
    }





    public bool MeetsRequirements(Empire empire)
    {
        foreach(string requiredTechName in requiredTechnologies)
        {
            Technology requiredTech = ResourceManager.instance.GetTechnology(requiredTechName);

            if (requiredTech != null)
            {
                if (!empire.HasResearchedTechnology(requiredTech))
                {
                    return false;
                }
            }
        }

        return true;
    }
}
