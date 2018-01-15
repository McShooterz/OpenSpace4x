/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using System.Collections.Generic;

public sealed class ShipDesign : UnitDesign
{
    public List<DesignModule> ForeModules { get; set; }
    public List<DesignModule> AftModules { get; set; }
    public List<DesignModule> PortModules { get; set; }
    public List<DesignModule> StarboardModules { get; set; }
    public List<DesignModule> CenterModules { get; set; }

    //Behavior
    public ShipBehaviorType AI_Profile { get; set; }
    public AttackStyle attackStyle { get; set; }
    public QuadrantTypes attackDirection { get; set; }
    public float attackRange { get; set; }

    //Set default values in Constructor
    public ShipDesign() : base()
    {
        AI_Profile = ShipBehaviorType.BestGuess;
        attackStyle = AttackStyle.holdPosition;
        attackDirection = QuadrantTypes.Fore;
        attackRange = -1;

        ForeModules = new List<DesignModule>();
        AftModules = new List<DesignModule>();
        PortModules = new List<DesignModule>();
        StarboardModules = new List<DesignModule>();
        CenterModules = new List<DesignModule>();
    }

    public ShipHullData GetHull()
    {
        return ResourceManager.instance.GetShipHull(Hull);
    }

    public ShipDesignData GetShipDesignData()
    {
        return ResourceManager.instance.GetShipDesignData(this);
    }
}
