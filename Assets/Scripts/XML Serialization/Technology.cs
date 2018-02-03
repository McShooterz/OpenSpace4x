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
    public TechnologyType technologyType;

    public string name;

    public string description;

    public int baseCost;

    public string[] requiredTechnologies;

    // effects

    public string[] unlockShipHulls;









    public bool MeetsRequirements(EmpireData empire)
    {



        return true;
    }
}
