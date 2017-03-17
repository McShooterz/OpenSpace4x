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
    public List<DesignModule> ForeModules = new List<DesignModule>();
    public List<DesignModule> AftModules = new List<DesignModule>();
    public List<DesignModule> PortModules = new List<DesignModule>();
    public List<DesignModule> StarboardModules = new List<DesignModule>();
    public List<DesignModule> CenterModules = new List<DesignModule>();

    //Behavior
    public ShipBehaviorType AI_Profile = ShipBehaviorType.BestGuess;
    public AttackStyle attackStyle = AttackStyle.holdPosition;
    public QuadrantTypes attackDirection = QuadrantTypes.Fore;
    public float attackRange = -1;

    public ShipHullData GetHull()
    {
        return ResourceManager.GetShipHull(Hull);
    }
}
