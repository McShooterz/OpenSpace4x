/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class StationHullData
{
    public string Name;

    public string StationObject;
    public string SlotLayout;
    public string HardPoints;

    public StationType Classification;
    public string Icon;

    //Base Stats
    public int BaseCrew = 100;
    public float BaseSensor = 20f;
    public float BaseLongRangeSensor = 25f;
    public float BaseAdvancedSensor = 0f;
    public float BaseResearch = 0f;
    public float BaseTrading = 0f;
    public float BaseMining = 0f;
    public float BaseRepair = 0.5f;
    public float BaseHealthPerSlot = 5f;
    public float BaseArmorPerSlot = 2f;

    public float ExperienceKillValue;

    public ModuleLimitType ModuleLimitations = ModuleLimitType.NoEngines;

    public Texture2D GetIcon()
    {
        return ResourceManager.GetUnitIcon(Icon);
    }

    public GameObject GetStationObject()
    {
        return ResourceManager.GetStationObject(StationObject);
    }

    public StationSlotLayout GetSlotLayout()
    {
        return ResourceManager.GetStationSlotLayout(SlotLayout);
    }

    public HardPointsStored GetHardPoints()
    {
        return ResourceManager.GetHardPoints(HardPoints);
    }
}
