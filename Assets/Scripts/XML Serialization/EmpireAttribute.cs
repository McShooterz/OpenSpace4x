/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class EmpireAttribute
{
    public string Name;
    public string Description;

    public EmpireAttributeType AttributeType;

    //Money
    public float StartingMoney;
    public float TaxRate;
    public float TaxModifier;
    public float TradeModifier;

    public float MoralPerTurn;

    //Producing
    public float ProductionModifier;
    public float ProductionSpaceModifier;
    public float ResearchModifier;
    public float FarmingModifier;
    public float MiningModifier;

    //Upkeep
    public float PlanetDistrictUpkeepModifier;
    public float PlanetObitalStructureUpkeepModifier;
    public float ShipUpkeepModifier;
    public float StationUpkeepModifier;
    public float ArmyUpkeepModifier;

    //Characters
    public float LeaderQualityModifier;
    public float GovernorQualityModifier;
    public float AdmiralQualityModifier;
    public float ScientistQualityModifier;
    public float SpyQualityModifier;

    //Cost
    public float PlanetDistrictCostModifier;
    public float PlanetObitalStructureCostModifier;
    public float ShipCostModifier;
    public float StationCostModifier;
    public float ArmyCostModifier;
    public float MarketResourceBuyModifier;
    public float MarketResourceSellModifier;
}
