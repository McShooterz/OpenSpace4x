/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using System.Collections.Generic;

public sealed class StationDesign : UnitDesign
{
    public List<DesignModule> Modules { get; set; }

    public StationDesign() : base()
    {
        Modules = new List<DesignModule>();
    }

    public StationHullData GetHull()
    {
        return ResourceManager.GetStationHull(Hull);
    }
}
