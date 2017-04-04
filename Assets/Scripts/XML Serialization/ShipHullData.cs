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
    public string Name { get; set; }

    public string ShipObject { get; set; }
    public string SlotLayout { get; set; }
    public string HardPoints { get; set; }

    public ShipType Classification { get; set; }
    public string Icon { get; set; }
    public float MaxTurnTilt { get; set; }
    public float ExplosionScale { get; set; }

    //Base stats
    public int BaseCommandPoints { get; set; }
    public int BaseCrew { get; set; }
    public float BaseFTL { get; set; }
    public float BaseSupply { get; set; }
    public float BaseFuel { get; set; }
    public float BaseSensor { get; set; }
    public float BaseLongRangeSensor { get; set; }
    public float BaseAdvancedSensor { get; set; }
    public float BaseResearch { get; set; }
    public float BaseRepair { get; set; }
    public float BaseHealthPerSlot { get; set; }
    public float BaseArmorPerSlot { get; set; }

    //Quadrant limitations
    public ModuleLimitType ModuleLimitFore { get; set; }
    public ModuleLimitType ModuleLimitAft { get; set; }
    public ModuleLimitType ModuleLimitPort { get; set; }
    public ModuleLimitType ModuleLimitStarboard { get; set; }
    public ModuleLimitType ModuleLimitCenter { get; set; }

    //Set default values in Constructor
    public ShipHullData()
    {
        Name = "";
        ShipObject = "";
        SlotLayout = "";
        HardPoints = "";
        Icon = "";
        MaxTurnTilt = 30.0f;
        ExplosionScale = 1.0f;

        BaseCommandPoints = 1;
        BaseCrew = 100;
        BaseFTL = 3f;
        BaseSupply = 20f;
        BaseFuel = 100f;
        BaseSensor = 20f;
        BaseLongRangeSensor = 25f;
        BaseAdvancedSensor = 0f;
        BaseResearch = 0f;
        BaseRepair = 0.5f;
        BaseHealthPerSlot = 5f;
        BaseArmorPerSlot = 2f;

        ModuleLimitFore = ModuleLimitType.NoEngines;
        ModuleLimitAft = ModuleLimitType.AllowAll;
        ModuleLimitPort = ModuleLimitType.NoEngines;
        ModuleLimitStarboard = ModuleLimitType.NoEngines;
        ModuleLimitCenter = ModuleLimitType.NoWeaponsOrEngines;
    }

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
