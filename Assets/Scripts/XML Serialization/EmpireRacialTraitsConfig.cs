/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class EmpireRacialTraitsConfig
{
    public Trait PopulationGrowth = new Trait();
    public Trait PopulationMax = new Trait();
    public Trait FoodConsumption = new Trait();
    public Trait SpeciesLifeSpan = new Trait();
    public Trait ClimateTolerance = new Trait();
    public Trait StartingMoney = new Trait();
    public Trait Taxation = new Trait();
    public Trait Farming = new Trait();
    public Trait Industry = new Trait();
    public Trait SpaceConstruction = new Trait();
    public Trait Research = new Trait();
    public Trait Mining = new Trait();
    public Trait Trade = new Trait();
    public Trait Diplomacy = new Trait();
    public Trait DiplomacyTade = new Trait();
    public Trait DiplomacyTolerance = new Trait();
    public Trait Espionage = new Trait();
    public Trait Counterespionage = new Trait();

    public Trait GroundCombat = new Trait();
    public Trait ArmyCost = new Trait();
    public Trait ArmyUpkeep = new Trait();
    public Trait SpaceWeaponDamage = new Trait();
    public Trait SpaceDamageReceived = new Trait();
    public Trait ShipCost = new Trait();
    public Trait ShipUpkeep = new Trait();
    public Trait ShipCombatSpeed = new Trait();
    public Trait ShipRepair = new Trait();
    public Trait StationCost = new Trait();
    public Trait StationUpkeep = new Trait();
    public Trait FTLSpeed = new Trait();
    public Trait FleetCommandLimit = new Trait();

    public Trait PlanetDistrictCost = new Trait();
    public Trait PlanetDistrictUpkeep = new Trait();
    public Trait PlanetOrbitalStructureCost = new Trait();
    public Trait PlanetOrbitalStructureUpkeep = new Trait();

    public Trait Slavery = new Trait();
    public Trait Terraforming = new Trait();
    public Trait Pollution = new Trait();
    public Trait MoralPerTurn = new Trait();
    public Trait Crime = new Trait();
    public Trait LawEnforcement = new Trait();

    public Trait HomeWorldSize = new Trait();
    public Trait HomeWorldFertility = new Trait();
    public Trait HomeWorldRichness = new Trait();

    public Trait LeaderQuality = new Trait();
    public Trait GovernorQuality = new Trait();
    public Trait AdmiralQuality = new Trait();
    public Trait ScientistQuality = new Trait();
    public Trait SpyQuality = new Trait();

    public Trait MarketBuyValue = new Trait();
    public Trait MarketSellValue = new Trait();

    public EmpireRacialTraitsConfig(){ }


    public class Trait
    {
        public string Icon = " ";
        public float DefaultValue = 0;
        public float ValuePointRatio = 0;
        public float ValueMin = 0;
        public float ValueMax = 0;
        public float ValueIncrement = 0;
        public bool Percentage = false;
        public string ToolTipTitle = " ";
        public string ToolTipBody = " ";

        public Trait(){}
    }
}
