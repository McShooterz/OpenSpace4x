/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class EmpireDefinition
{
    public string Name { get; set; }

    public ColorXML EmpireColor { get; set; }

    public int FlagEmblemIndex { get; set; }
    public ColorXML FlagEmblemColor { get; set; }
    public int FlagBackgroundIndex { get; set; }
    public ColorXML FlagBackgroundColor { get; set; }

    public string EmpireLeaderTitle { get; set; }
    public string EmpireAdmiralTitle { get; set; }
    public string EmpireGovernorTitle { get; set; }
    public string EmpireSpyTitle { get; set; }
    public string EmpireScientistTitle { get; set; }
    public string EmpireSpeciesSingular { get; set; }
    public string EmpireSpeciesPural { get; set; }
    public string EmpireHomePlanet { get; set; }
    public string EmpireHomeSolarSystem { get; set; }
    public string EmpireShipPrefix { get; set; }
    public string EmpireStationPrefix { get; set; }

    public string EmpireGovernment { get; set; }
    public string EmpireEconomy { get; set; }
    public string EmpireCulture { get; set; }

    public float TraitPopulationGrowth { get; set; }
    public float TraitPopulationMax { get; set; }
    public float TraitFoodConsumption { get; set; }
    public float TraitSpeciesLifeSpan { get; set; }
    public float TraitClimateTolerance { get; set; }
    public float TraitStartingMoney { get; set; }
    public float TraitTaxation { get; set; }
    public float TraitFarming { get; set; }
    public float TraitIndustry { get; set; }
    public float TraitSpaceConstruction { get; set; }
    public float TraitResearch { get; set; }
    public float TraitMining { get; set; }
    public float TraitTrade { get; set; }
    public float TraitDiplomacy { get; set; }
    public float TraitDiplomacyTade { get; set; }
    public float TraitDiplomacyTolerance { get; set; }
    public float TraitEspionage { get; set; }
    public float TraitCounterespionage { get; set; }

    public float TraitGroundCombat { get; set; }
    public float TraitArmyCost { get; set; }
    public float TraitArmyUpkeep { get; set; }
    public float TraitSpaceWeaponDamage { get; set; }
    public float TraitSpaceDamageReceived { get; set; }
    public float TraitShipCost { get; set; }
    public float TraitShipUpkeep { get; set; }
    public float TraitShipCombatSpeed { get; set; }
    public float TraitShipRepair { get; set; }
    public float TraitStationCost { get; set; }
    public float TraitStationUpkeep { get; set; }
    public float TraitFTLSpeed { get; set; }
    public float TraitFleetCommandLimit { get; set; }

    public float TraitPlanetDistrictCost { get; set; }
    public float TraitPlanetDistrictUpkeep { get; set; }
    public float TraitPlanetOrbitalStructureCost { get; set; }
    public float TraitPlanetOrbitalStructureUpkeep { get; set; }

    public float TraitSlavery { get; set; }
    public float TraitTerraforming { get; set; }
    public float TraitPollution { get; set; }
    public float TraitMoralPerTurn { get; set; }
    public float TraitCrime { get; set; }
    public float TraitLawEnforcement { get; set; }

    public float TraitHomeWorldSize { get; set; }
    public float TraitHomeWorldFertility { get; set; }
    public float TraitHomeWorldRichness { get; set; }

    public float TraitLeaderQuality { get; set; }
    public float TraitGovernorQuality { get; set; }
    public float TraitAdmiralQuality { get; set; }
    public float TraitScientistQuality { get; set; }
    public float TraitSpyQuality { get; set; }

    public float TraitMarketBuyValue { get; set; }
    public float TraitMarketSellValue { get; set; }

    //Set default values in Constructor
    public EmpireDefinition()
    {

    }
}
