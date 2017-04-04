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
    #region Variables

    public string Name { get; set; }
    public string Description { get; set; }

    public EmpireAttributeType AttributeType { get; set; }

    //Money
    public float StartingMoney { get; set; }
    public float TaxRate { get; set; }
    public float TaxModifier { get; set; }
    public float TradeModifier { get; set; }

    public float MoralPerTurn { get; set; }

    //Producing
    public float ProductionModifier { get; set; }
    public float ProductionSpaceModifier { get; set; }
    public float ResearchModifier { get; set; }
    public float FarmingModifier { get; set; }
    public float MiningModifier { get; set; }

    //Upkeep
    public float PlanetDistrictUpkeepModifier { get; set; }
    public float PlanetObitalStructureUpkeepModifier { get; set; }
    public float ShipUpkeepModifier { get; set; }
    public float StationUpkeepModifier { get; set; }
    public float ArmyUpkeepModifier { get; set; }

    //Characters
    public float LeaderQualityModifier { get; set; }
    public float GovernorQualityModifier { get; set; }
    public float AdmiralQualityModifier { get; set; }
    public float ScientistQualityModifier { get; set; }
    public float SpyQualityModifier { get; set; }

    //Cost
    public float PlanetDistrictCostModifier { get; set; }
    public float PlanetObitalStructureCostModifier { get; set; }
    public float ShipCostModifier { get; set; }
    public float StationCostModifier { get; set; }
    public float ArmyCostModifier { get; set; }
    public float MarketResourceBuyModifier { get; set; }
    public float MarketResourceSellModifier { get; set; }

    #endregion

    //Set default values in Constructor
    public EmpireAttribute()
    {

    }
}
