/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes: Defines attributes of ship hulls
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class ShipHullData
{
    public string Name;

    public string ShipObject;
    public string SlotLayout;
    public string HardPoints;

    public ShipType Classification;
    public string Icon;
    public float MaxTurnTilt = 30.0f;
    public float ExplosionScale = 1.0f;

    //Base stats
    public int BaseCommandPoints = 1;
    public int BaseCrew = 100;
    public float BaseFTL = 3f;
    public float BaseSupply = 20f;
    public float BaseFuel = 100f;
    public float BaseSensor = 20f;
    public float BaseLongRangeSensor = 25f;
    public float BaseAdvancedSensor = 0f;
    public float BaseResearch = 0f;
    public float BaseRepair = 0.5f;
    public float BaseHealthPerSlot = 5f;
    public float BaseArmorPerSlot = 2f;

    //Quadrant limitations
    public ModuleLimitType ModuleLimitFore = ModuleLimitType.NoEngines;
    public ModuleLimitType ModuleLimitAft = ModuleLimitType.AllowAll;
    public ModuleLimitType ModuleLimitPort = ModuleLimitType.NoEngines;
    public ModuleLimitType ModuleLimitStarboard = ModuleLimitType.NoEngines;
    public ModuleLimitType ModuleLimitCenter = ModuleLimitType.NoWeaponsOrEngines;

    public ShipHullData() { }

    public Texture2D GetIcon()
    {
        return ResourceManager.GetUnitIcon(Icon);
    }

    public GameObject GetShipObject()
    {
        return ResourceManager.GetShipObject(ShipObject);
    }

    public ShipSlotLayout GetSlotLayout()
    {
        return ResourceManager.GetShipSlotLayout(SlotLayout);
    }

    public HardPointsStored GetHardPoints()
    {
        return ResourceManager.GetHardPoints(HardPoints);
    }
}
