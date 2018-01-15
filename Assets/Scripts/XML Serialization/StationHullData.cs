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
public class StationHullData
{
    public string Name { get; set; }

    public string StationObject { get; set; }
    public string SlotLayout { get; set; }
    public string HardPoints { get; set; }

    public StationType Classification { get; set; }
    public string Icon { get; set; }

    //Base Stats
    public int BaseCrew { get; set; }
    public float BaseSensor { get; set; }
    public float BaseLongRangeSensor { get; set; }
    public float BaseAdvancedSensor { get; set; }
    public float BaseResearch { get; set; }
    public float BaseTrading { get; set; }
    public float BaseMining { get; set; }
    public float BaseRepair { get; set; }
    public float BaseHealthPerSlot { get; set; }
    public float BaseArmorPerSlot { get; set; }

    public float ExperienceKillValue { get; set; }

    public ModuleLimitType ModuleLimitations { get; set; }

    public StationHullData()
    {
        Name = "";
        StationObject = "";
        SlotLayout = "";
        HardPoints = "";
        Icon = "";
        BaseCrew = 100;
        BaseSensor = 20f;
        BaseLongRangeSensor = 25f;
        BaseAdvancedSensor = 0f;
        BaseResearch = 0f;
        BaseTrading = 0f;
        BaseMining = 0f;
        BaseRepair = 0.5f;
        BaseHealthPerSlot = 5f;
        BaseArmorPerSlot = 2f;
        ExperienceKillValue = 1;
        ModuleLimitations = ModuleLimitType.NoEngines;
}

    public Texture2D GetIcon()
    {
        return ResourceManager.instance.GetUnitIcon(Icon);
    }

    public GameObject GetStationObject()
    {
        return ResourceManager.instance.GetStationObject(StationObject);
    }

    public StationSlotLayout GetSlotLayout()
    {
        return ResourceManager.instance.GetStationSlotLayout(SlotLayout);
    }

    public HardPointsStored GetHardPoints()
    {
        return ResourceManager.instance.GetHardPoints(HardPoints);
    }
}
