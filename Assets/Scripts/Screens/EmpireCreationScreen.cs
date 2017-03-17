/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections.Generic;

public class EmpireCreationScreen : ScreenParent
{
    #region Variables
    //Empire variables
    string EmpireName = "";
    Color EmpireColor = Color.white;
    string EmpireLeaderTitle = "Emperor";
    string EmpireAdmiralTitle = "Admiral";
    string EmpireGovernorTitle = "Governor";
    string EmpireSpyTitle = "Spy";
    string EmpireScientistTitle = "Doctor";
    string EmpireSpeciesSingular = "";
    string EmpireSpeciesPural = "";
    string EmpireHomePlanet = "";
    string EmpireHomeSolarSystem = "";
    string EmpireShipPrefix = "";
    string EmpireStationPrefix = "";
    EmpireFlag EmpireFlag;

    ColorPickerWindow colorPickerWindow;

    //Empire Attributes
    EmpireAttributeScrollList EmpireGovernmentScrollList;
    EmpireAttributeScrollList EmpireEconomyScrollList;
    EmpireAttributeScrollList EmpireCultureScrollList;

    ComboBox CharacterNameSet;
    ComboBox ShipNameSet;
    ComboBox TechnologyTree;
    ComboBox DilpomacyDialogue;

    //Racial Traits
    float PointsLimit = 10f;

    //Screen Elements
    EmpireScrollList empireScrollList;

    Rect MainPanelRect;
    Rect AttributePanelRect;
    Rect EmpireTraitsPanelRect;

    //Main Panel Rects
    Rect EmpireNameRect;
    Rect EmpireNameFieldRect;
    Rect EmpireSaveButtonRect;
    Rect EmpireColorRect;
    Rect EmpireColorButtonRect;
    Rect EmpireLeaderTitleRect;
    Rect EmpireLeaderTitleFieldRect;
    Rect EmpireAdmiralTitleRect;
    Rect EmpireAdmiralTitleFieldRect;
    Rect EmpireGovernorTitleRect;
    Rect EmpireGovernorTitleFieldRect;
    Rect EmpireSpyTitleRect;
    Rect EmpireSpyTitleFieldRect;
    Rect EmpireScientistTitleRect;
    Rect EmpireScientistTitleFieldRect;

    Rect EmpireSpeciesSingularRect;
    Rect EmpireSpeciesSingularFieldRect;
    Rect EmpireSpeciesPuralRect;
    Rect EmpireSpeciesPuralFieldRect;
    Rect EmpireHomeWorldRect;
    Rect EmpireHomeWorldFieldRect;
    Rect EmpireHomeSolarSystemRect;
    Rect EmpireHomeSolarSytemFieldRect;
    Rect EmpireShipPrefixRect;
    Rect EmpireShipPrefixFieldRect;
    Rect EmpireStationPrefixRect;
    Rect EmpireStationPrefixFieldRect;

    Rect EmpireTechnologyTreeTitleRect;
    Rect EmpireDiplomacyDialogueTitleRect;
    Rect EmpireCharacterNameSetTitleRect;
    Rect EmpireShipNameSetTitleRect;

    Rect EmpireTechnologyTreeButtonRect;
    Rect EmpireDiplomacyDialogueButtonRect;
    Rect EmpireCharacterNameSetButtonRect;
    Rect EmpireShipNameSetButtonRect;

    Rect EmpireFlagBackgroundForwardButtonRect;
    Rect EmpireFlagBackgroundBackwardButtonRect;
    Rect EmpireFlagBackgroundColorButtonRect;
    Rect EmpireFlagEmblemForwardButtonRect;
    Rect EmpireFlagEmblemBackwardButtonRect;
    Rect EmpireFlagEmblemColorButtonRect;

    //Attributes Panel Rects
    Rect EmpireGovernmentTitleRect;
    Rect EmpireEconomyTitleRect;
    Rect EmpireCultureTitleRect;

    Vector2 EmpireAttributeStatSize;
    Vector2 GovernmentStatsPosition;
    Vector2 EconomyStatsPosition;
    Vector2 CultureStatsPosition;

    List<IconStatEntry> GovernmentStats = new List<IconStatEntry>();
    List<IconStatEntry> EconomyStats = new List<IconStatEntry>();
    List<IconStatEntry> CultureStats = new List<IconStatEntry>();

    //Empire Traits Panel Rects
    Rect RacialTraitsTitleRect;
    Rect RacialTraitsPointsBoxRect;
    Rect RacialTraitsPointsRect;

    List<RacialTrait> RacialTraits = new List<RacialTrait>();
    
    RacialTrait TraitPopulationGrowth;
    RacialTrait TraitPopulationMax;
    RacialTrait TraitFoodConsumption;
    RacialTrait TraitSpeciesLifeSpan;
    RacialTrait TraitClimateTolerance;
    RacialTrait TraitStartingMoney;
    RacialTrait TraitTaxation;
    RacialTrait TraitFarming;
    RacialTrait TraitIndustry;
    RacialTrait TraitSpaceConstruction;
    RacialTrait TraitResearch;
    RacialTrait TraitMining;
    RacialTrait TraitTrade;
    RacialTrait TraitDiplomacy;
    RacialTrait TraitDiplomacyTade;
    RacialTrait TraitDiplomacyTolerance;
    RacialTrait TraitEspionage;
    RacialTrait TraitCounterespionage;

    RacialTrait TraitGroundCombat;
    RacialTrait TraitArmyCost;
    RacialTrait TraitArmyUpkeep;
    RacialTrait TraitSpaceWeaponDamage;
    RacialTrait TraitSpaceDamageReceived;
    RacialTrait TraitShipCost;
    RacialTrait TraitShipUpkeep;
    RacialTrait TraitShipCombatSpeed;
    RacialTrait TraitShipRepair;
    RacialTrait TraitStationCost;
    RacialTrait TraitStationUpkeep;
    RacialTrait TraitFTLSpeed;
    RacialTrait TraitFleetCommandLimit;

    RacialTrait TraitPlanetDistrictCost;
    RacialTrait TraitPlanetDistrictUpkeep;
    RacialTrait TraitPlanetOrbitalStructureCost;
    RacialTrait TraitPlanetOrbitalStructureUpkeep;

    RacialTrait TraitSlavery;
    RacialTrait TraitTerraforming;
    RacialTrait TraitPollution;
    RacialTrait TraitMoralPerTurn;
    RacialTrait TraitCrime;
    RacialTrait TraitLawEnforcement;

    RacialTrait TraitHomeWorldSize;
    RacialTrait TraitHomeWorldFertility;
    RacialTrait TraitHomeWorldRichness;

    RacialTrait TraitLeaderQuality;
    RacialTrait TraitGovernorQuality;
    RacialTrait TraitAdmiralQuality;
    RacialTrait TraitScientistQuality;
    RacialTrait TraitSpyQuality;

    RacialTrait TraitMarketBuyValue;
    RacialTrait TraitMarketSellValue;

    Texture2D blankTexture;

    ScreenParent LastScreen;

    #endregion

    public EmpireCreationScreen(ScreenParent lastScreen)
    {
        LastScreen = lastScreen;

        Rect EmpireScollListRect = new Rect(Screen.width * 0.0075f, Screen.height * 0.02f, Screen.width * 0.17f, Screen.height * 0.96f);
        empireScrollList = new EmpireScrollList(EmpireScollListRect, LoadEmpire);

        ToolTip = new GUIToolTip(new Vector2(Screen.width - Screen.width * 0.14f, 0), Screen.width * 0.14f);

        MainPanelRect = new Rect(EmpireScollListRect.xMax + Screen.width * 0.0075f, EmpireScollListRect.y, Screen.width * 0.6675f, Screen.height * 0.25f);
        AttributePanelRect = new Rect(MainPanelRect.x, MainPanelRect.yMax + Screen.height * 0.02f, Screen.width * 0.7375f, Screen.height * 0.2f);
        EmpireTraitsPanelRect = new Rect(MainPanelRect.x, AttributePanelRect.yMax + Screen.height * 0.02f, Screen.width * 0.8075f, Screen.height - (AttributePanelRect.yMax + Screen.height * 0.04f));

        float Spacing = Screen.height * 0.0065f;

        //Main Panel
        Rect EmpireFlagRect = new Rect(MainPanelRect.x + Spacing, MainPanelRect.y + Spacing, MainPanelRect.height * 0.667f, MainPanelRect.height * 0.667f);
        EmpireFlag = new EmpireFlag(EmpireFlagRect, 0, 0, Color.white, Color.black);
        Vector2 smallButtonSize = new Vector2(GameManager.instance.StandardButtonSize.x * 0.334f, GameManager.instance.StandardButtonSize.y * 0.75f);
        EmpireFlagBackgroundBackwardButtonRect = new Rect(EmpireFlagRect.x, EmpireFlagRect.yMax + Spacing, smallButtonSize.x, smallButtonSize.y);
        EmpireFlagBackgroundColorButtonRect = new Rect(EmpireFlagRect.x + (EmpireFlagRect.width - smallButtonSize.x) / 2f, EmpireFlagBackgroundBackwardButtonRect.y, smallButtonSize.x, smallButtonSize.y);
        EmpireFlagBackgroundForwardButtonRect = new Rect(EmpireFlagRect.xMax - smallButtonSize.x, EmpireFlagBackgroundBackwardButtonRect.y, smallButtonSize.x, smallButtonSize.y);   
        EmpireFlagEmblemBackwardButtonRect = new Rect(EmpireFlagBackgroundBackwardButtonRect.x, EmpireFlagBackgroundBackwardButtonRect.yMax + Spacing, EmpireFlagBackgroundBackwardButtonRect.width, EmpireFlagBackgroundBackwardButtonRect.height);
        EmpireFlagEmblemColorButtonRect = new Rect(EmpireFlagBackgroundColorButtonRect.x, EmpireFlagEmblemBackwardButtonRect.y, smallButtonSize.x, smallButtonSize.y);
        EmpireFlagEmblemForwardButtonRect = new Rect(EmpireFlagBackgroundForwardButtonRect.x, EmpireFlagEmblemBackwardButtonRect.y, smallButtonSize.x, smallButtonSize.y);

        EmpireNameRect = new Rect(EmpireFlagRect.xMax + Spacing, EmpireFlagRect.y, GameManager.instance.StandardLabelSize.x, GameManager.instance.StandardLabelSize.y);
        EmpireNameFieldRect = new Rect(EmpireNameRect.xMax, EmpireNameRect.y, GameManager.instance.StandardLabelSize.x * 2.25f, GameManager.instance.StandardLabelSize.y);
        EmpireSaveButtonRect = new Rect(EmpireNameFieldRect.xMax + Spacing, EmpireNameRect.y, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardLabelSize.y);
        EmpireColorRect = new Rect(EmpireNameRect.x, EmpireNameRect.yMax + Spacing, GameManager.instance.StandardLabelSize.x, GameManager.instance.StandardLabelSize.y);
        EmpireColorButtonRect = new Rect(EmpireColorRect.xMax, EmpireColorRect.y, GameManager.instance.StandardLabelSize.y, GameManager.instance.StandardLabelSize.y);
        EmpireLeaderTitleRect = new Rect(EmpireColorRect.x, EmpireColorRect.yMax + Spacing, GameManager.instance.StandardLabelSize.x, GameManager.instance.StandardLabelSize.y);
        EmpireLeaderTitleFieldRect = new Rect(EmpireLeaderTitleRect.xMax, EmpireLeaderTitleRect.y, GameManager.instance.StandardLabelSize.x * 2.25f, GameManager.instance.StandardLabelSize.y);
        EmpireAdmiralTitleRect = new Rect(EmpireLeaderTitleRect.x, EmpireLeaderTitleRect.yMax + Spacing, GameManager.instance.StandardLabelSize.x, GameManager.instance.StandardLabelSize.y);
        EmpireAdmiralTitleFieldRect = new Rect(EmpireAdmiralTitleRect.xMax, EmpireAdmiralTitleRect.y, GameManager.instance.StandardLabelSize.x * 2.25f, GameManager.instance.StandardLabelSize.y);
        EmpireGovernorTitleRect = new Rect(EmpireAdmiralTitleRect.x, EmpireAdmiralTitleRect.yMax + Spacing, GameManager.instance.StandardLabelSize.x, GameManager.instance.StandardLabelSize.y);
        EmpireGovernorTitleFieldRect = new Rect(EmpireGovernorTitleRect.xMax, EmpireGovernorTitleRect.y, GameManager.instance.StandardLabelSize.x * 2.25f, GameManager.instance.StandardLabelSize.y);
        EmpireSpyTitleRect = new Rect(EmpireGovernorTitleRect.x, EmpireGovernorTitleRect.yMax + Spacing, GameManager.instance.StandardLabelSize.x, GameManager.instance.StandardLabelSize.y);
        EmpireSpyTitleFieldRect = new Rect(EmpireSpyTitleRect.xMax, EmpireSpyTitleRect.y, GameManager.instance.StandardLabelSize.x * 2.25f, GameManager.instance.StandardLabelSize.y);
        EmpireScientistTitleRect = new Rect(EmpireSpyTitleRect.x, EmpireSpyTitleRect.yMax + Spacing, GameManager.instance.StandardLabelSize.x, GameManager.instance.StandardLabelSize.y);
        EmpireScientistTitleFieldRect = new Rect(EmpireScientistTitleRect.xMax, EmpireScientistTitleRect.y, GameManager.instance.StandardLabelSize.x * 2.25f, GameManager.instance.StandardLabelSize.y);

        EmpireSpeciesSingularRect = new Rect(EmpireSaveButtonRect.x, EmpireColorRect.y, GameManager.instance.StandardLabelSize.x, GameManager.instance.StandardLabelSize.y);
        EmpireSpeciesSingularFieldRect = new Rect(EmpireSpeciesSingularRect.xMax, EmpireSpeciesSingularRect.y, GameManager.instance.StandardLabelSize.x * 2.25f, GameManager.instance.StandardLabelSize.y);
        EmpireSpeciesPuralRect = new Rect(EmpireSpeciesSingularRect.x, EmpireSpeciesSingularRect.yMax + Spacing, GameManager.instance.StandardLabelSize.x, GameManager.instance.StandardLabelSize.y);
        EmpireSpeciesPuralFieldRect = new Rect(EmpireSpeciesPuralRect.xMax, EmpireSpeciesPuralRect.y, GameManager.instance.StandardLabelSize.x * 2.25f, GameManager.instance.StandardLabelSize.y);
        EmpireHomeWorldRect = new Rect(EmpireSpeciesPuralRect.x, EmpireSpeciesPuralRect.yMax + Spacing, GameManager.instance.StandardLabelSize.x, GameManager.instance.StandardLabelSize.y);
        EmpireHomeWorldFieldRect = new Rect(EmpireHomeWorldRect.xMax, EmpireHomeWorldRect.y, GameManager.instance.StandardLabelSize.x * 2.25f, GameManager.instance.StandardLabelSize.y);
        EmpireHomeSolarSystemRect = new Rect(EmpireHomeWorldRect.x, EmpireHomeWorldRect.yMax + Spacing, GameManager.instance.StandardLabelSize.x, GameManager.instance.StandardLabelSize.y);
        EmpireHomeSolarSytemFieldRect = new Rect(EmpireHomeSolarSystemRect.xMax, EmpireHomeSolarSystemRect.y, GameManager.instance.StandardLabelSize.x * 2.25f, GameManager.instance.StandardLabelSize.y);
        EmpireShipPrefixRect = new Rect(EmpireHomeSolarSystemRect.x, EmpireHomeSolarSystemRect.yMax + Spacing, GameManager.instance.StandardLabelSize.x, GameManager.instance.StandardLabelSize.y);
        EmpireShipPrefixFieldRect = new Rect(EmpireShipPrefixRect.xMax, EmpireShipPrefixRect.y, GameManager.instance.StandardLabelSize.x * 2.25f, GameManager.instance.StandardLabelSize.y);
        EmpireStationPrefixRect = new Rect(EmpireShipPrefixRect.x, EmpireShipPrefixRect.yMax + Spacing, GameManager.instance.StandardLabelSize.x, GameManager.instance.StandardLabelSize.y);
        EmpireStationPrefixFieldRect = new Rect(EmpireStationPrefixRect.xMax, EmpireStationPrefixRect.y, GameManager.instance.StandardLabelSize.x * 2.25f, GameManager.instance.StandardLabelSize.y);

        EmpireTechnologyTreeTitleRect = new Rect(EmpireSpeciesSingularFieldRect.xMax + Spacing, EmpireSpeciesSingularRect.y, GameManager.instance.StandardLabelSize.x, GameManager.instance.StandardLabelSize.y);
        EmpireDiplomacyDialogueTitleRect = new Rect(EmpireTechnologyTreeTitleRect.x, EmpireSpeciesPuralRect.y, GameManager.instance.StandardLabelSize.x, GameManager.instance.StandardLabelSize.y);
        EmpireCharacterNameSetTitleRect = new Rect(EmpireTechnologyTreeTitleRect.x, EmpireHomeWorldRect.y, GameManager.instance.StandardLabelSize.x, GameManager.instance.StandardLabelSize.y);
        EmpireShipNameSetTitleRect = new Rect(EmpireTechnologyTreeTitleRect.x, EmpireHomeSolarSystemRect.y, GameManager.instance.StandardLabelSize.x, GameManager.instance.StandardLabelSize.y);

        EmpireTechnologyTreeButtonRect = new Rect(EmpireTechnologyTreeTitleRect.xMax + Spacing, EmpireTechnologyTreeTitleRect.y, GameManager.instance.StandardLabelSize.x * 2.25f, GameManager.instance.StandardLabelSize.y);
        EmpireDiplomacyDialogueButtonRect = new Rect(EmpireTechnologyTreeButtonRect.x, EmpireDiplomacyDialogueTitleRect.y, GameManager.instance.StandardLabelSize.x * 2.25f, GameManager.instance.StandardLabelSize.y);
        EmpireCharacterNameSetButtonRect = new Rect(EmpireTechnologyTreeButtonRect.x, EmpireCharacterNameSetTitleRect.y, GameManager.instance.StandardLabelSize.x * 2.25f, GameManager.instance.StandardLabelSize.y);
        EmpireShipNameSetButtonRect = new Rect(EmpireTechnologyTreeButtonRect.x, EmpireShipNameSetTitleRect.y, GameManager.instance.StandardLabelSize.x * 2.25f, GameManager.instance.StandardLabelSize.y);

        CharacterNameSet = new ComboBox(EmpireCharacterNameSetButtonRect);
        ShipNameSet = new ComboBox(EmpireShipNameSetButtonRect);
        TechnologyTree = new ComboBox(EmpireTechnologyTreeButtonRect);
        DilpomacyDialogue = new ComboBox(EmpireDiplomacyDialogueButtonRect);

        //test entries
        CharacterNameSet.AddEntry("Test1");
        CharacterNameSet.AddEntry("Test2");
        CharacterNameSet.AddEntry("Test3");
        CharacterNameSet.AddEntry("Test4");
        CharacterNameSet.AddEntry("Test5");
        CharacterNameSet.AddEntry("Test6");

        ShipNameSet.AddEntry("Test1");
        ShipNameSet.AddEntry("Test2");
        ShipNameSet.AddEntry("Test3");
        ShipNameSet.AddEntry("Test4");
        ShipNameSet.AddEntry("Test5");
        ShipNameSet.AddEntry("Test6");

        TechnologyTree.AddEntry("Test1");
        TechnologyTree.AddEntry("Test2");
        TechnologyTree.AddEntry("Test3");
        TechnologyTree.AddEntry("Test4");
        TechnologyTree.AddEntry("Test5");
        TechnologyTree.AddEntry("Test6");

        DilpomacyDialogue.AddEntry("Test1");
        DilpomacyDialogue.AddEntry("Test2");
        DilpomacyDialogue.AddEntry("Test3");
        DilpomacyDialogue.AddEntry("Test4");
        DilpomacyDialogue.AddEntry("Test5");
        DilpomacyDialogue.AddEntry("Test6");

        //Attributes Panel
        EmpireGovernmentTitleRect = new Rect(AttributePanelRect.x + Spacing, AttributePanelRect.y + Spacing, GameManager.instance.StandardLabelSize.x, GameManager.instance.StandardLabelSize.y);
        EmpireEconomyTitleRect = new Rect(AttributePanelRect.x + AttributePanelRect.width * 0.334f, EmpireGovernmentTitleRect.y, GameManager.instance.StandardLabelSize.x, GameManager.instance.StandardLabelSize.y);
        EmpireCultureTitleRect = new Rect(AttributePanelRect.x + AttributePanelRect.width * 0.667f, EmpireGovernmentTitleRect.y, GameManager.instance.StandardLabelSize.x, GameManager.instance.StandardLabelSize.y);

        EmpireAttributeStatSize = new Vector2(GameManager.instance.StandardLabelSize.x * 0.75f, GameManager.instance.StandardLabelSize.y);  

        Rect GovernmentScrollListRect = new Rect(EmpireGovernmentTitleRect.x, EmpireGovernmentTitleRect.yMax + Spacing, GameManager.instance.StandardLabelSize.x * 2.25f, AttributePanelRect.yMax - Spacing * 2f - EmpireGovernmentTitleRect.yMax);
        EmpireGovernmentScrollList = new EmpireAttributeScrollList(GovernmentScrollListRect, EmpireAttributeType.Government, SelectGovernment);
        Rect EconomyScrollListRect = new Rect(EmpireEconomyTitleRect.x, GovernmentScrollListRect.y, GovernmentScrollListRect.width, GovernmentScrollListRect.height);
        EmpireEconomyScrollList = new EmpireAttributeScrollList(EconomyScrollListRect, EmpireAttributeType.Economy, SelectEconomy);       
        Rect CultureScrollListRect = new Rect(EmpireCultureTitleRect.x, GovernmentScrollListRect.y, GovernmentScrollListRect.width, GovernmentScrollListRect.height);
        EmpireCultureScrollList = new EmpireAttributeScrollList(CultureScrollListRect, EmpireAttributeType.Culture, SelectCulture);

        GovernmentStatsPosition = new Vector2(GovernmentScrollListRect.xMax + Spacing * 2.5f, EmpireGovernmentTitleRect.y);
        EconomyStatsPosition = new Vector2(EconomyScrollListRect.xMax + Spacing, GovernmentStatsPosition.y);
        CultureStatsPosition = new Vector2(CultureScrollListRect.xMax + Spacing, GovernmentStatsPosition.y);

        SelectGovernment(EmpireGovernmentScrollList.GetFirstEntry());
        SelectEconomy(EmpireEconomyScrollList.GetFirstEntry());
        SelectCulture(EmpireCultureScrollList.GetFirstEntry());

        //Empire Traits Panel
        RacialTraitsTitleRect = new Rect(EmpireTraitsPanelRect.x + EmpireTraitsPanelRect.width * 0.015f, EmpireTraitsPanelRect.y, EmpireTraitsPanelRect.width * 0.12f, EmpireTraitsPanelRect.height * 0.08f);
        RacialTraitsPointsBoxRect = new Rect(EmpireTraitsPanelRect.xMax - EmpireTraitsPanelRect.width * 0.07f, EmpireTraitsPanelRect.y, EmpireTraitsPanelRect.width * 0.07f, EmpireTraitsPanelRect.height * 0.08f);
        RacialTraitsPointsRect = new Rect(RacialTraitsPointsBoxRect.x + RacialTraitsPointsBoxRect.width * 0.1f, RacialTraitsPointsBoxRect.y + RacialTraitsPointsBoxRect.height * 0.1f, RacialTraitsPointsBoxRect.width * 0.8f, RacialTraitsPointsBoxRect.height * 0.8f);

        TraitPopulationGrowth = AddRacialTrait(ResourceManager.RacialTraitsConfig.PopulationGrowth);
        TraitPopulationMax = AddRacialTrait(ResourceManager.RacialTraitsConfig.PopulationMax);
        TraitFoodConsumption = AddRacialTrait(ResourceManager.RacialTraitsConfig.FoodConsumption);
        TraitSpeciesLifeSpan = AddRacialTrait(ResourceManager.RacialTraitsConfig.SpeciesLifeSpan);
        TraitClimateTolerance = AddRacialTrait(ResourceManager.RacialTraitsConfig.ClimateTolerance);
        TraitStartingMoney = AddRacialTrait(ResourceManager.RacialTraitsConfig.StartingMoney);
        TraitTaxation = AddRacialTrait(ResourceManager.RacialTraitsConfig.Taxation);
        TraitFarming = AddRacialTrait(ResourceManager.RacialTraitsConfig.Farming);
        TraitIndustry = AddRacialTrait(ResourceManager.RacialTraitsConfig.Industry);
        TraitSpaceConstruction = AddRacialTrait(ResourceManager.RacialTraitsConfig.SpaceConstruction);
        TraitResearch = AddRacialTrait(ResourceManager.RacialTraitsConfig.Research);
        TraitMining = AddRacialTrait(ResourceManager.RacialTraitsConfig.Mining);
        TraitTrade = AddRacialTrait(ResourceManager.RacialTraitsConfig.Trade);
        TraitDiplomacy = AddRacialTrait(ResourceManager.RacialTraitsConfig.Diplomacy);
        TraitDiplomacyTade = AddRacialTrait(ResourceManager.RacialTraitsConfig.DiplomacyTade);
        TraitDiplomacyTolerance = AddRacialTrait(ResourceManager.RacialTraitsConfig.DiplomacyTolerance);
        TraitEspionage = AddRacialTrait(ResourceManager.RacialTraitsConfig.Espionage);
        TraitCounterespionage = AddRacialTrait(ResourceManager.RacialTraitsConfig.Counterespionage);

        TraitGroundCombat = AddRacialTrait(ResourceManager.RacialTraitsConfig.GroundCombat);
        TraitArmyCost = AddRacialTrait(ResourceManager.RacialTraitsConfig.ArmyCost);
        TraitArmyUpkeep = AddRacialTrait(ResourceManager.RacialTraitsConfig.ArmyUpkeep);
        TraitSpaceWeaponDamage = AddRacialTrait(ResourceManager.RacialTraitsConfig.SpaceWeaponDamage);
        TraitSpaceDamageReceived = AddRacialTrait(ResourceManager.RacialTraitsConfig.SpaceDamageReceived);
        TraitShipCost = AddRacialTrait(ResourceManager.RacialTraitsConfig.ShipCost);
        TraitShipUpkeep = AddRacialTrait(ResourceManager.RacialTraitsConfig.ShipUpkeep);
        TraitShipCombatSpeed = AddRacialTrait(ResourceManager.RacialTraitsConfig.ShipCombatSpeed);
        TraitShipRepair = AddRacialTrait(ResourceManager.RacialTraitsConfig.ShipRepair);
        TraitStationCost = AddRacialTrait(ResourceManager.RacialTraitsConfig.StationCost);
        TraitStationUpkeep = AddRacialTrait(ResourceManager.RacialTraitsConfig.StationUpkeep);
        TraitFTLSpeed = AddRacialTrait(ResourceManager.RacialTraitsConfig.FTLSpeed);
        TraitFleetCommandLimit = AddRacialTrait(ResourceManager.RacialTraitsConfig.FleetCommandLimit);

        TraitPlanetDistrictCost = AddRacialTrait(ResourceManager.RacialTraitsConfig.PlanetDistrictCost);
        TraitPlanetDistrictUpkeep = AddRacialTrait(ResourceManager.RacialTraitsConfig.PlanetDistrictUpkeep);
        TraitPlanetOrbitalStructureCost = AddRacialTrait(ResourceManager.RacialTraitsConfig.PlanetOrbitalStructureCost);
        TraitPlanetOrbitalStructureUpkeep = AddRacialTrait(ResourceManager.RacialTraitsConfig.PlanetOrbitalStructureUpkeep);

        TraitSlavery = AddRacialTrait(ResourceManager.RacialTraitsConfig.Slavery);
        TraitTerraforming = AddRacialTrait(ResourceManager.RacialTraitsConfig.Terraforming);
        TraitPollution = AddRacialTrait(ResourceManager.RacialTraitsConfig.Pollution);
        TraitMoralPerTurn = AddRacialTrait(ResourceManager.RacialTraitsConfig.MoralPerTurn);
        TraitCrime = AddRacialTrait(ResourceManager.RacialTraitsConfig.Crime);
        TraitLawEnforcement = AddRacialTrait(ResourceManager.RacialTraitsConfig.LawEnforcement);

        TraitHomeWorldSize = AddRacialTrait(ResourceManager.RacialTraitsConfig.HomeWorldSize);
        TraitHomeWorldFertility = AddRacialTrait(ResourceManager.RacialTraitsConfig.HomeWorldFertility);
        TraitHomeWorldRichness = AddRacialTrait(ResourceManager.RacialTraitsConfig.HomeWorldRichness);

        TraitLeaderQuality = AddRacialTrait(ResourceManager.RacialTraitsConfig.LeaderQuality);
        TraitGovernorQuality = AddRacialTrait(ResourceManager.RacialTraitsConfig.GovernorQuality);
        TraitAdmiralQuality = AddRacialTrait(ResourceManager.RacialTraitsConfig.AdmiralQuality);
        TraitScientistQuality = AddRacialTrait(ResourceManager.RacialTraitsConfig.ScientistQuality);
        TraitSpyQuality = AddRacialTrait(ResourceManager.RacialTraitsConfig.SpyQuality);

        TraitMarketBuyValue = AddRacialTrait(ResourceManager.RacialTraitsConfig.MarketBuyValue);
        TraitMarketSellValue = AddRacialTrait(ResourceManager.RacialTraitsConfig.MarketSellValue);

        empireScrollList.LoadFirstEmpire();

        colorPickerWindow = new ColorPickerWindow();

        blankTexture = new Texture2D(1, 1);
    }

    // Update is called once per frame
    public override void Update()
    {
        mousePosition = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseScreen();
        }
    }

    public override void Draw()
    {
        if (colorPickerWindow.isOpen())
            GUI.enabled = false;

        DrawMainPanel();

        DrawAttributesPanel();

        DrawTraitsPanel();

        DrawComboBoxes();

        empireScrollList.Draw();

        if (!colorPickerWindow.isOpen())
            CheckToolTips();

        GUI.enabled = true;

        ToolTip.Draw();

        if (colorPickerWindow.isOpen())
            colorPickerWindow.Draw(mousePosition);
    }

    void DrawMainPanel()
    {
        Color oldGUIColor = GUI.color;

        GUI.Box(MainPanelRect, "");
        EmpireFlag.Draw();
        if (GUI.Button(EmpireFlagBackgroundBackwardButtonRect, "<"))
        {
            EmpireFlag.DecrementBackground();
        }

        GUI.color = EmpireFlag.GetBackgroundColor();
        GUI.DrawTexture(EmpireFlagBackgroundColorButtonRect, blankTexture);
        GUI.color = oldGUIColor;
        if (GUI.Button(EmpireFlagBackgroundColorButtonRect, ""))
        {
            colorPickerWindow.OpenWindow(EmpireFlag.GetBackgroundColor(),ChangeFlagBackgroundColor);
        }

        if (GUI.Button(EmpireFlagBackgroundForwardButtonRect, ">"))
        {
            EmpireFlag.IncrementBackground();
        }

        if (GUI.Button(EmpireFlagEmblemBackwardButtonRect, "<"))
        {
            EmpireFlag.DecrementEmblem();
        }

        GUI.color = EmpireFlag.GetEmblemColor();
        GUI.DrawTexture(EmpireFlagEmblemColorButtonRect, blankTexture);
        GUI.color = oldGUIColor;
        if (GUI.Button(EmpireFlagEmblemColorButtonRect, ""))
        {
            colorPickerWindow.OpenWindow(EmpireFlag.GetEmblemColor(), ChangeFlagEmblemColor);
        }

        if (GUI.Button(EmpireFlagEmblemForwardButtonRect, ">"))
        {
            EmpireFlag.IncrementEmblem();
        }

        GUI.Label(EmpireNameRect, "Empire Name:", GameManager.instance.ModuleTitleStyle);
        EmpireName = GUI.TextField(EmpireNameFieldRect, EmpireName);

        if (GUI.Button(EmpireSaveButtonRect, "Save"))
        {
            SaveEmpire();
            empireScrollList.RebuildList();
        }

        GUI.color = EmpireColor;
        GUI.DrawTexture(EmpireColorButtonRect, blankTexture);
        GUI.color = oldGUIColor;
        GUI.Label(EmpireColorRect, "Color:", GameManager.instance.ModuleTitleStyle);
        if (GUI.Button(EmpireColorButtonRect, ""))
        {
            colorPickerWindow.OpenWindow(EmpireColor, ChangeEmpireColor);
        }

        GUI.Label(EmpireLeaderTitleRect, "Leader Title:", GameManager.instance.ModuleTitleStyle);
        EmpireLeaderTitle = GUI.TextField(EmpireLeaderTitleFieldRect, EmpireLeaderTitle);

        GUI.Label(EmpireAdmiralTitleRect, "Admiral Title:", GameManager.instance.ModuleTitleStyle);
        EmpireAdmiralTitle = GUI.TextField(EmpireAdmiralTitleFieldRect, EmpireAdmiralTitle);

        GUI.Label(EmpireGovernorTitleRect, "Governor Title:", GameManager.instance.ModuleTitleStyle);
        EmpireGovernorTitle = GUI.TextField(EmpireGovernorTitleFieldRect, EmpireGovernorTitle);

        GUI.Label(EmpireSpyTitleRect, "Spy Title:", GameManager.instance.ModuleTitleStyle);
        EmpireSpyTitle = GUI.TextField(EmpireSpyTitleFieldRect, EmpireSpyTitle);

        GUI.Label(EmpireScientistTitleRect, "Scientist Title:", GameManager.instance.ModuleTitleStyle);
        EmpireScientistTitle = GUI.TextField(EmpireScientistTitleFieldRect, EmpireScientistTitle);

        GUI.Label(EmpireSpeciesSingularRect, "Species:", GameManager.instance.ModuleTitleStyle);
        EmpireSpeciesSingular = GUI.TextField(EmpireSpeciesSingularFieldRect, EmpireSpeciesSingular);

        GUI.Label(EmpireSpeciesPuralRect, "Species Pural:", GameManager.instance.ModuleTitleStyle);
        EmpireSpeciesPural = GUI.TextField(EmpireSpeciesPuralFieldRect, EmpireSpeciesPural);

        GUI.Label(EmpireHomeWorldRect, "Home World:", GameManager.instance.ModuleTitleStyle);
        EmpireHomePlanet = GUI.TextField(EmpireHomeWorldFieldRect, EmpireHomePlanet);

        GUI.Label(EmpireHomeSolarSystemRect, "Solar System:", GameManager.instance.ModuleTitleStyle);
        EmpireHomeSolarSystem = GUI.TextField(EmpireHomeSolarSytemFieldRect, EmpireHomeSolarSystem);

        GUI.Label(EmpireShipPrefixRect, "Ship Prefix:", GameManager.instance.ModuleTitleStyle);
        EmpireShipPrefix = GUI.TextField(EmpireShipPrefixFieldRect, EmpireShipPrefix);

        GUI.Label(EmpireStationPrefixRect, "Station Prefix:", GameManager.instance.ModuleTitleStyle);
        EmpireStationPrefix = GUI.TextField(EmpireStationPrefixFieldRect, EmpireStationPrefix);

        GUI.Label(EmpireTechnologyTreeTitleRect, "Tech Tree:", GameManager.instance.ModuleTitleStyle);       

        GUI.Label(EmpireDiplomacyDialogueTitleRect, "Dialogue:", GameManager.instance.ModuleTitleStyle);


        GUI.Label(EmpireCharacterNameSetTitleRect, "Name Set:", GameManager.instance.ModuleTitleStyle);


        GUI.Label(EmpireShipNameSetTitleRect, "Ship Names:", GameManager.instance.ModuleTitleStyle);

    }

    void DrawAttributesPanel()
    {
        GUI.Box(AttributePanelRect, "");

        GUI.Label(EmpireGovernmentTitleRect, "Government", GameManager.instance.ModuleTitleStyle);

        GUI.Label(EmpireEconomyTitleRect, "Economy", GameManager.instance.ModuleTitleStyle);

        GUI.Label(EmpireCultureTitleRect, "Culture", GameManager.instance.ModuleTitleStyle);


        EmpireGovernmentScrollList.Draw();
        foreach(IconStatEntry entry in GovernmentStats)
        {
            entry.Draw();
        }

        EmpireEconomyScrollList.Draw();
        foreach (IconStatEntry entry in EconomyStats)
        {
            entry.Draw();
        }

        EmpireCultureScrollList.Draw();
        foreach (IconStatEntry entry in CultureStats)
        {
            entry.Draw();
        }
    }

    void DrawTraitsPanel()
    {
        GUI.Box(EmpireTraitsPanelRect, "");

        GUI.Label(RacialTraitsTitleRect, "Racial Traits", GameManager.instance.ModuleTitleStyle);

        GUI.Box(RacialTraitsPointsBoxRect, "", GameManager.instance.standardBackGround);

        float usedPoints = GetPointsTotal();
        if (usedPoints > PointsLimit)
        {
            Color guiColor = GUI.color;
            GUI.color = Color.red;
            GUI.Box(RacialTraitsPointsRect, usedPoints.ToString("0.#") + "/" + PointsLimit.ToString("0.#"));
            GUI.color = guiColor;
        }
        else
        {
            GUI.Box(RacialTraitsPointsRect, usedPoints.ToString("0.#") + "/" + PointsLimit.ToString("0.#"));
        }

        foreach(RacialTrait trait in RacialTraits)
        {
            trait.Draw();
        }
    }

    void DrawComboBoxes()
    {
        if (DilpomacyDialogue.isOpen() || TechnologyTree.isOpen() || CharacterNameSet.isOpen())
            GUI.enabled = false;
        ShipNameSet.Draw(mousePosition);
        GUI.enabled = true;

        if (ShipNameSet.isOpen() || DilpomacyDialogue.isOpen() || TechnologyTree.isOpen())
            GUI.enabled = false;
        CharacterNameSet.Draw(mousePosition);
        GUI.enabled = true;

        if (ShipNameSet.isOpen() || TechnologyTree.isOpen() || CharacterNameSet.isOpen())
            GUI.enabled = false;
        DilpomacyDialogue.Draw(mousePosition);
        GUI.enabled = true;

        if (ShipNameSet.isOpen() || DilpomacyDialogue.isOpen() || CharacterNameSet.isOpen())
            GUI.enabled = false;
        TechnologyTree.Draw(mousePosition);
        GUI.enabled = true;
    }

    void CheckToolTips()
    {


        foreach (IconStatEntry entry in GovernmentStats)
        {
            if (entry.CheckForToolTip(mousePosition))
                return;
        }

        foreach (IconStatEntry entry in EconomyStats)
        {
            if (entry.CheckForToolTip(mousePosition))
                return;
        }

        foreach (IconStatEntry entry in CultureStats)
        {
            if (entry.CheckForToolTip(mousePosition))
                return;
        }

        foreach (RacialTrait trait in RacialTraits)
        {
            if(trait.CheckToolTip(mousePosition))
            {
                return;
            }
        }
    }

    public void SelectGovernment(EmpireAttribute government)
    {
        if(government != null)
        {
            GovernmentStats = BuildEmpireAttributeStatIcons(government, GovernmentStatsPosition);
        }
    }

    public void SelectEconomy(EmpireAttribute economy)
    {
        if (economy != null)
        {
            EconomyStats = BuildEmpireAttributeStatIcons(economy, EconomyStatsPosition);
        }
    }

    public void SelectCulture(EmpireAttribute culture)
    {
        if (culture != null)
        {
            CultureStats = BuildEmpireAttributeStatIcons(culture, CultureStatsPosition);
        }
    }

    List<IconStatEntry> BuildEmpireAttributeStatIcons(EmpireAttribute empireAttribute, Vector2 Position)
    {
        List<IconStatEntry> IconStatList = new List<IconStatEntry>();

        if(empireAttribute.StartingMoney != 0)
        {
            AddEmpireAttributeStatEntry(IconStatList, Position, "Icon_Money", empireAttribute.StartingMoney.ToString("0.#"), "StartingMoney", "StartingMoneyDesc");
        }
        if(empireAttribute.TaxRate != 0)
        {
            AddEmpireAttributeStatEntry(IconStatList, Position, "Icon_Taxation", empireAttribute.TaxRate.ToString("0.#"), "TaxRate", "TaxRateDesc");
        }
        if (empireAttribute.TaxModifier != 0)
        {
            AddEmpireAttributeStatEntry(IconStatList, Position, "Icon_TaxationBonus", "%" + (empireAttribute.TaxModifier * 100f).ToString("0.#"), "TaxModifier", "TaxModifierDesc");
        }
        if (empireAttribute.TradeModifier != 0)
        {
            AddEmpireAttributeStatEntry(IconStatList, Position, "Icon_Trade", "%" + (empireAttribute.TradeModifier * 100f).ToString("0.#"), "Trade", "TradeModifierDesc");
        }
        if (empireAttribute.MoralPerTurn != 0)
        {
            AddEmpireAttributeStatEntry(IconStatList, Position, "Icon_MoralHappy", empireAttribute.MoralPerTurn.ToString("0.#"), "MoralPerTurn", "MoralPerTurnDesc");
        }
        if (empireAttribute.ProductionModifier != 0)
        {
            AddEmpireAttributeStatEntry(IconStatList, Position, "Icon_Production", "%" + (empireAttribute.ProductionModifier * 100f).ToString("0.#"), "ProductionModifier", "ProductionModifierDesc");
        }
        if (empireAttribute.ProductionSpaceModifier != 0)
        {
            AddEmpireAttributeStatEntry(IconStatList, Position, "Icon_ProductionSpace", "%" + (empireAttribute.ProductionSpaceModifier * 100f).ToString("0.#"), "SpaceProduction", "StartingMoneyDesc");
        }
        if (empireAttribute.ResearchModifier != 0)
        {
            AddEmpireAttributeStatEntry(IconStatList, Position, "Icon_Research", "%" + (empireAttribute.ResearchModifier * 100f).ToString("0.#"), "Research", "ResearchModifierDesc");
        }
        if (empireAttribute.FarmingModifier != 0)
        {
            AddEmpireAttributeStatEntry(IconStatList, Position, "Icon_Food", "%" + (empireAttribute.FarmingModifier * 100f).ToString("0.#"), "Farming", "FarmingModifierDesc");
        }
        if (empireAttribute.MiningModifier != 0)
        {
            AddEmpireAttributeStatEntry(IconStatList, Position, "Icon_Mining", "%" + (empireAttribute.MiningModifier * 100f).ToString("0.#"), "Mining", "MiningModifierDesc");
        }
        if (empireAttribute.PlanetDistrictUpkeepModifier != 0)
        {
            AddEmpireAttributeStatEntry(IconStatList, Position, "Icon_PlanetaryDistrictUpkeep", "%" + (empireAttribute.PlanetDistrictUpkeepModifier * 100f).ToString("0.#"), "PlanetDistrictUpkeep", "PlanetDistrictUpkeepModifierDesc");
        }
        if (empireAttribute.PlanetObitalStructureUpkeepModifier != 0)
        {
            AddEmpireAttributeStatEntry(IconStatList, Position, "Icon_OrbitalStructureUpkeep", "%" + (empireAttribute.PlanetObitalStructureUpkeepModifier * 100f).ToString("0.#"), "PlanetObitalStructureUpkeep", "PlanetObitalStructureUpkeepModifierDesc");
        }
        if (empireAttribute.ShipUpkeepModifier != 0)
        {
            AddEmpireAttributeStatEntry(IconStatList, Position, "Icon_ShipUpkeep", "%" + (empireAttribute.ShipUpkeepModifier * 100f).ToString("0.#"), "ShipUpkeep", "ShipUpkeepDesc");
        }
        if (empireAttribute.StationUpkeepModifier != 0)
        {
            AddEmpireAttributeStatEntry(IconStatList, Position, "Icon_StationUpkeep", "%" + (empireAttribute.StationUpkeepModifier * 100f).ToString("0.#"), "StationUpkeep", "StationUpkeepDesc");
        }
        if (empireAttribute.ArmyUpkeepModifier != 0)
        {
            AddEmpireAttributeStatEntry(IconStatList, Position, "Icon_ArmyUpkeep", "%" + (empireAttribute.ArmyUpkeepModifier * 100f).ToString("0.#"), "ArmyUpkeep", "StartingMoneyDesc");
        }
        if (empireAttribute.LeaderQualityModifier != 0)
        {
            AddEmpireAttributeStatEntry(IconStatList, Position, "Icon_Leader", "%" + (empireAttribute.LeaderQualityModifier * 100f).ToString("0.#"), "LeaderQuality", "LeaderQualityDesc");
        }
        if (empireAttribute.GovernorQualityModifier != 0)
        {
            AddEmpireAttributeStatEntry(IconStatList, Position, "Icon_Governor", "%" + (empireAttribute.GovernorQualityModifier * 100f).ToString("0.#"), "GovernorQuality", "GovernorQualityDesc");
        }
        if (empireAttribute.AdmiralQualityModifier != 0)
        {
            AddEmpireAttributeStatEntry(IconStatList, Position, "Icon_Admiral", "%" + (empireAttribute.AdmiralQualityModifier * 100f).ToString("0.#"), "AdmiralQuality", "AdmiralQualityDesc");
        }
        if (empireAttribute.ScientistQualityModifier != 0)
        {
            AddEmpireAttributeStatEntry(IconStatList, Position, "Icon_Scientist", "%" + (empireAttribute.ScientistQualityModifier * 100f).ToString("0.#"), "ScientistQuality", "StartingMoneyDesc");
        }
        if (empireAttribute.SpyQualityModifier != 0)
        {
            AddEmpireAttributeStatEntry(IconStatList, Position, "Icon_Spy", "%" + (empireAttribute.SpyQualityModifier * 100f).ToString("0.#"), "SpyQuality", "SpyQualityDesc");
        }
        if (empireAttribute.PlanetDistrictCostModifier != 0)
        {
            AddEmpireAttributeStatEntry(IconStatList, Position, "Icon_PlanetaryDistrictCost", "%" + (empireAttribute.PlanetDistrictCostModifier * 100f).ToString("0.#"), "StartingMoney", "PlanetDistrictCostModifierDesc");
        }
        if (empireAttribute.PlanetObitalStructureCostModifier != 0)
        {
            AddEmpireAttributeStatEntry(IconStatList, Position, "Icon_OrbitalStructureCost", "%" + (empireAttribute.PlanetObitalStructureCostModifier * 100f).ToString("0.#"), "StartingMoney", "PlanetObitalStructureCostModifierDesc");
        }
        if (empireAttribute.ShipCostModifier != 0)
        {
            AddEmpireAttributeStatEntry(IconStatList, Position, "Icon_ShipCost", "%" + (empireAttribute.ShipCostModifier * 100f).ToString("0.#"), "ShipCost", "ShipCostDesc");
        }
        if (empireAttribute.StationCostModifier != 0)
        {
            AddEmpireAttributeStatEntry(IconStatList, Position, "Icon_StationCost", "%" + (empireAttribute.StationCostModifier * 100f).ToString("0.#"), "StationCostModifier", "StationCostModifierDesc");
        }
        if (empireAttribute.ArmyCostModifier != 0)
        {
            AddEmpireAttributeStatEntry(IconStatList, Position, "Icon_ArmyCost", "%" + (empireAttribute.ArmyCostModifier * 100f).ToString("0.#"), "ArmyCost", "ArmyCostModifierDesc");
        }
        if (empireAttribute.MarketResourceBuyModifier != 0)
        {
            AddEmpireAttributeStatEntry(IconStatList, Position, "Icon_MarketBuy", "%" + (empireAttribute.MarketResourceBuyModifier * 100f).ToString("0.#"), "MarketValueBuy", "MarketResourceBuyModifierDesc");
        }
        if (empireAttribute.MarketResourceSellModifier != 0)
        {
            AddEmpireAttributeStatEntry(IconStatList, Position, "Icon_MarketSell", "%" + (empireAttribute.MarketResourceSellModifier * 100f).ToString("0.#"), "MarketValueSell", "MarketResourceSellModifierDesc");
        }
    
        return IconStatList;
    }

    void AddEmpireAttributeStatEntry(List<IconStatEntry> list, Vector2 position, string iconTexture, string Value, string ToolTipTitle, string ToolTipBody)
    {
        Rect rect = new Rect(position.x + (list.Count % 3) * EmpireAttributeStatSize.x, position.y + (list.Count / 3) * EmpireAttributeStatSize.y, EmpireAttributeStatSize.x, EmpireAttributeStatSize.y);
        IconStatEntry iconStatEntry = new IconStatEntry(rect, iconTexture, Value, ToolTipTitle, ToolTipBody, ToolTip);
        list.Add(iconStatEntry);
    }

    RacialTrait AddRacialTrait(EmpireRacialTraitsConfig.Trait trait)
    {
        float Spacing = Screen.height * 0.0065f;
        float Width = GameManager.instance.StandardLabelSize.x * 3f;
        Rect rect = new Rect(RacialTraitsTitleRect.x + (RacialTraits.Count / 12) * (Width + Spacing * 2.5f), RacialTraitsTitleRect.yMax + Spacing + (RacialTraits.Count % 12) * (GameManager.instance.StandardLabelSize.y + Spacing), Width, GameManager.instance.StandardLabelSize.y);
        RacialTrait newRacialTrait = new RacialTrait(rect, trait.Icon, trait.DefaultValue, trait.ValuePointRatio, trait.ValueMin, trait.ValueMax, trait.ValueIncrement, trait.Percentage, ToolTip, trait.ToolTipTitle, trait.ToolTipBody);

        RacialTraits.Add(newRacialTrait);
        return newRacialTrait;
    }

    float GetPointsTotal()
    {
        float Sum = 0;
        foreach (RacialTrait trait in RacialTraits)
        {
            Sum += trait.GetPoints();
        }
        return Sum;
    }

    bool ValidEmpire()
    {
        if (EmpireName == null || EmpireName == "")
            return false;

        return true;
    }

    void SaveEmpire()
    {
        if (ValidEmpire())
        {
            EmpireDefinition empire = new EmpireDefinition();

            empire.Name = EmpireName;
            empire.EmpireColor = new ColorXML(EmpireColor);

            empire.FlagEmblemIndex = EmpireFlag.GetEmblemIndex();
            empire.FlagEmblemColor = new ColorXML(EmpireFlag.GetEmblemColor());
            empire.FlagBackgroundIndex = EmpireFlag.GetBackgroundIndex();
            empire.FlagBackgroundColor = new ColorXML(EmpireFlag.GetBackgroundColor());

            empire.EmpireLeaderTitle = EmpireLeaderTitle;
            empire.EmpireAdmiralTitle = EmpireAdmiralTitle;
            empire.EmpireGovernorTitle = EmpireGovernorTitle;
            empire.EmpireSpyTitle = EmpireSpyTitle;
            empire.EmpireScientistTitle = EmpireScientistTitle;
            empire.EmpireSpeciesSingular = EmpireSpeciesSingular;
            empire.EmpireSpeciesPural = EmpireSpeciesPural;
            empire.EmpireHomePlanet = EmpireHomePlanet;
            empire.EmpireHomeSolarSystem = EmpireHomeSolarSystem;
            empire.EmpireShipPrefix = EmpireShipPrefix;
            empire.EmpireStationPrefix = EmpireStationPrefix;

            empire.EmpireGovernment = EmpireGovernmentScrollList.GetSelectedEntryName();
            empire.EmpireEconomy = EmpireEconomyScrollList.GetSelectedEntryName();
            empire.EmpireCulture = EmpireCultureScrollList.GetSelectedEntryName();

            empire.TraitPopulationGrowth = TraitPopulationGrowth.GetValue();
            empire.TraitPopulationMax = TraitPopulationMax.GetValue();
            empire.TraitFoodConsumption = TraitFoodConsumption.GetValue();
            empire.TraitSpeciesLifeSpan = TraitSpeciesLifeSpan.GetValue();
            empire.TraitClimateTolerance = TraitClimateTolerance.GetValue();
            empire.TraitStartingMoney = TraitStartingMoney.GetValue();
            empire.TraitTaxation = TraitTaxation.GetValue();
            empire.TraitFarming = TraitFarming.GetValue();
            empire.TraitIndustry = TraitIndustry.GetValue();
            empire.TraitSpaceConstruction = TraitSpaceConstruction.GetValue();
            empire.TraitResearch = TraitResearch.GetValue();
            empire.TraitMining = TraitMining.GetValue();
            empire.TraitTrade = TraitTrade.GetValue();
            empire.TraitDiplomacy = TraitDiplomacy.GetValue();
            empire.TraitDiplomacyTade = TraitDiplomacyTade.GetValue();
            empire.TraitDiplomacyTolerance = TraitDiplomacyTolerance.GetValue();
            empire.TraitEspionage = TraitEspionage.GetValue();
            empire.TraitCounterespionage = TraitCounterespionage.GetValue();

            empire.TraitGroundCombat = TraitGroundCombat.GetValue();
            empire.TraitArmyCost = TraitArmyCost.GetValue();
            empire.TraitArmyUpkeep = TraitArmyUpkeep.GetValue();
            empire.TraitSpaceWeaponDamage = TraitSpaceWeaponDamage.GetValue();
            empire.TraitSpaceDamageReceived = TraitSpaceDamageReceived.GetValue();
            empire.TraitShipCost = TraitShipCost.GetValue();
            empire.TraitShipUpkeep = TraitShipUpkeep.GetValue();
            empire.TraitShipCombatSpeed = TraitShipCombatSpeed.GetValue();
            empire.TraitShipRepair = TraitShipRepair.GetValue();
            empire.TraitStationCost = TraitStationCost.GetValue();
            empire.TraitStationUpkeep = TraitStationUpkeep.GetValue();
            empire.TraitFTLSpeed = TraitFTLSpeed.GetValue();
            empire.TraitFleetCommandLimit = TraitFleetCommandLimit.GetValue();

            empire.TraitPlanetDistrictCost = TraitPlanetDistrictCost.GetValue();
            empire.TraitPlanetDistrictUpkeep = TraitPlanetDistrictUpkeep.GetValue();
            empire.TraitPlanetOrbitalStructureCost = TraitPlanetOrbitalStructureCost.GetValue();
            empire.TraitPlanetOrbitalStructureUpkeep = TraitPlanetOrbitalStructureUpkeep.GetValue();

            empire.TraitSlavery = TraitSlavery.GetValue();
            empire.TraitTerraforming = TraitTerraforming.GetValue();
            empire.TraitPollution = TraitPollution.GetValue();
            empire.TraitMoralPerTurn = TraitMoralPerTurn.GetValue();
            empire.TraitCrime = TraitCrime.GetValue();
            empire.TraitLawEnforcement = TraitLawEnforcement.GetValue();

            empire.TraitHomeWorldSize = TraitHomeWorldSize.GetValue();
            empire.TraitHomeWorldFertility = TraitHomeWorldFertility.GetValue();
            empire.TraitHomeWorldRichness = TraitHomeWorldRichness.GetValue();

            empire.TraitLeaderQuality = TraitLeaderQuality.GetValue();
            empire.TraitGovernorQuality = TraitGovernorQuality.GetValue();
            empire.TraitAdmiralQuality = TraitAdmiralQuality.GetValue();
            empire.TraitScientistQuality = TraitScientistQuality.GetValue();
            empire.TraitSpyQuality = TraitSpyQuality.GetValue();

            empire.TraitMarketBuyValue = TraitMarketBuyValue.GetValue();
            empire.TraitMarketSellValue = TraitMarketSellValue.GetValue();

            ResourceManager.SaveEmpireDefinition(empire);
        }
    }

    void LoadEmpire(EmpireDefinition empireDefinition)
    {
        EmpireName = empireDefinition.Name;
        EmpireColor = empireDefinition.EmpireColor.GetColor();

        EmpireFlag.SetEmblemTexture(empireDefinition.FlagEmblemIndex);
        EmpireFlag.SetEmblemColor(empireDefinition.FlagEmblemColor.GetColor());
        EmpireFlag.SetBackgroundTexture(empireDefinition.FlagBackgroundIndex);
        EmpireFlag.SetBackgroundColor(empireDefinition.FlagBackgroundColor.GetColor());

        EmpireLeaderTitle = empireDefinition.EmpireLeaderTitle;
        EmpireAdmiralTitle = empireDefinition.EmpireAdmiralTitle;
        EmpireGovernorTitle = empireDefinition.EmpireGovernorTitle;
        EmpireSpyTitle = empireDefinition.EmpireSpyTitle;
        EmpireScientistTitle = empireDefinition.EmpireScientistTitle;
        EmpireSpeciesSingular = empireDefinition.EmpireSpeciesSingular;
        EmpireSpeciesPural = empireDefinition.EmpireSpeciesPural;
        EmpireHomePlanet = empireDefinition.EmpireHomePlanet;
        EmpireHomeSolarSystem = empireDefinition.EmpireHomeSolarSystem;
        EmpireShipPrefix = empireDefinition.EmpireShipPrefix;
        EmpireStationPrefix = empireDefinition.EmpireStationPrefix;

        EmpireGovernmentScrollList.SetSelectedEntry(empireDefinition.EmpireGovernment);
        EmpireEconomyScrollList.SetSelectedEntry(empireDefinition.EmpireEconomy);
        EmpireCultureScrollList.SetSelectedEntry(empireDefinition.EmpireCulture);

        TraitPopulationGrowth.SetValue(empireDefinition.TraitPopulationGrowth);
        TraitPopulationMax.SetValue(empireDefinition.TraitPopulationMax);
        TraitFoodConsumption.SetValue(empireDefinition.TraitFoodConsumption);
        TraitSpeciesLifeSpan.SetValue(empireDefinition.TraitSpeciesLifeSpan);
        TraitClimateTolerance.SetValue(empireDefinition.TraitClimateTolerance);
        TraitStartingMoney.SetValue(empireDefinition.TraitStartingMoney);
        TraitTaxation.SetValue(empireDefinition.TraitTaxation);
        TraitFarming.SetValue(empireDefinition.TraitFarming);
        TraitIndustry.SetValue(empireDefinition.TraitIndustry);
        TraitSpaceConstruction.SetValue(empireDefinition.TraitSpaceConstruction);
        TraitResearch.SetValue(empireDefinition.TraitResearch);
        TraitMining.SetValue(empireDefinition.TraitMining);
        TraitTrade.SetValue(empireDefinition.TraitTrade);
        TraitDiplomacy.SetValue(empireDefinition.TraitDiplomacy);
        TraitDiplomacyTade.SetValue(empireDefinition.TraitDiplomacyTade);
        TraitDiplomacyTolerance.SetValue(empireDefinition.TraitDiplomacyTolerance);
        TraitEspionage.SetValue(empireDefinition.TraitEspionage);
        TraitCounterespionage.SetValue(empireDefinition.TraitCounterespionage);

        TraitGroundCombat.SetValue(empireDefinition.TraitGroundCombat);
        TraitArmyCost.SetValue(empireDefinition.TraitArmyCost);
        TraitArmyUpkeep.SetValue(empireDefinition.TraitArmyUpkeep);
        TraitSpaceWeaponDamage.SetValue(empireDefinition.TraitSpaceWeaponDamage);
        TraitSpaceDamageReceived.SetValue(empireDefinition.TraitSpaceDamageReceived);
        TraitShipCost.SetValue(empireDefinition.TraitShipCost);
        TraitShipUpkeep.SetValue(empireDefinition.TraitShipUpkeep);
        TraitShipCombatSpeed.SetValue(empireDefinition.TraitShipCombatSpeed);
        TraitShipRepair.SetValue(empireDefinition.TraitShipRepair);
        TraitStationCost.SetValue(empireDefinition.TraitStationCost);
        TraitStationUpkeep.SetValue(empireDefinition.TraitStationUpkeep);
        TraitFTLSpeed.SetValue(empireDefinition.TraitFTLSpeed);
        TraitFleetCommandLimit.SetValue(empireDefinition.TraitFleetCommandLimit);

        TraitPlanetDistrictCost.SetValue(empireDefinition.TraitPlanetDistrictCost);
        TraitPlanetDistrictUpkeep.SetValue(empireDefinition.TraitPlanetDistrictUpkeep);
        TraitPlanetOrbitalStructureCost.SetValue(empireDefinition.TraitPlanetOrbitalStructureCost);
        TraitPlanetOrbitalStructureUpkeep.SetValue(empireDefinition.TraitPlanetOrbitalStructureUpkeep);

        TraitSlavery.SetValue(empireDefinition.TraitSlavery);
        TraitTerraforming.SetValue(empireDefinition.TraitTerraforming);
        TraitPollution.SetValue(empireDefinition.TraitPollution);
        TraitMoralPerTurn.SetValue(empireDefinition.TraitMoralPerTurn);
        TraitCrime.SetValue(empireDefinition.TraitCrime);
        TraitLawEnforcement.SetValue(empireDefinition.TraitLawEnforcement);

        TraitHomeWorldSize.SetValue(empireDefinition.TraitHomeWorldSize);
        TraitHomeWorldFertility.SetValue(empireDefinition.TraitHomeWorldFertility);
        TraitHomeWorldRichness.SetValue(empireDefinition.TraitHomeWorldRichness);

        TraitLeaderQuality.SetValue(empireDefinition.TraitLeaderQuality);
        TraitGovernorQuality.SetValue(empireDefinition.TraitGovernorQuality);
        TraitAdmiralQuality.SetValue(empireDefinition.TraitAdmiralQuality);
        TraitScientistQuality.SetValue(empireDefinition.TraitScientistQuality);
        TraitSpyQuality.SetValue(empireDefinition.TraitSpyQuality);

        TraitMarketBuyValue.SetValue(empireDefinition.TraitMarketBuyValue);
        TraitMarketSellValue.SetValue(empireDefinition.TraitMarketSellValue);
    }

    public void ChangeEmpireColor(Color color)
    {
        EmpireColor = color;
    }

    public void ChangeFlagBackgroundColor(Color color)
    {
        EmpireFlag.SetBackgroundColor(color);
    }

    public void ChangeFlagEmblemColor(Color color)
    {
        EmpireFlag.SetEmblemColor(color);
    }

    protected override void CloseScreen()
    {
        GameManager.instance.ChangeScreen(LastScreen);
    }

    public class RacialTrait
    {
        Rect baseRect;
        Rect StatRect;
        Rect SliderRect;
        Rect PointsRect;

        float Value;
        float ValuePointRatio;
        float ValueDefault;
        float ValueMin;
        float ValueMax;
        float ValueIncrement;

        bool isPercentage;

        GUIContent content;

        //Tooltips
        GUIToolTip ToolTip;
        string ToolTipTitle;
        string ToolTipBody;

        public RacialTrait(Rect rect, string IconName, float startValue, float valuePointRatio, float valueMin, float valueMax, float increment, bool percentage, GUIToolTip tooltip, string tooltipTitle, string tooltipBody)
        {
            baseRect = rect;

            StatRect = new Rect(baseRect.x, baseRect.y, baseRect.width * 0.3f, baseRect.height);
            SliderRect = new Rect(StatRect.xMax, baseRect.y, baseRect.width * 0.5f, baseRect.height);
            PointsRect = new Rect(SliderRect.xMax, baseRect.y, baseRect.width * 0.2f, baseRect.height);

            Value = startValue;
            ValueDefault = startValue;
            ValuePointRatio = valuePointRatio;
            ValueMin = valueMin;
            ValueMax = valueMax;
            ValueIncrement = increment;
            isPercentage = percentage;

            content = new GUIContent();
            content.image = ResourceManager.GetIconTexture(IconName);

            ToolTip = tooltip;
            ToolTipTitle = tooltipTitle;
            ToolTipBody = tooltipBody;
        }

        public void Draw()
        {
            if(isPercentage)
            {
                if (Value < 0)
                {
                    content.text = " " + (Value * 100).ToString("0.#") + "%";
                }
                else
                {
                    content.text = " +" + (Value * 100).ToString("0.#") + "%";
                }
            }
            else
            {
                if (Value < 0)
                {
                    content.text = " " + Value.ToString("0.##");
                }
                else
                {
                    content.text = " +" + Value.ToString("0.##");
                }
            }

            GUI.Label(StatRect, content, GameManager.instance.standardLabelStyle);
            GUI.Box(SliderRect, "");
            Value = GUI.HorizontalSlider(SliderRect, Value, ValueMin, ValueMax);
            GUI.Label(PointsRect, "Pts:" + GetPoints().ToString("0.#"));
            if(Value > ValueMin && Value < ValueMax)
                Value -= Value % ValueIncrement;
        }

        public float GetValue()
        {
            return Value;
        }

        public void SetValue(float value)
        {
            Value = value;
            if (Value > ValueMax)
                Value = ValueMax;
            else if (Value < ValueMin)
                Value = ValueMin;
        }

        public float GetPoints()
        {
            return (Value - ValueDefault) * ValuePointRatio;
        }

        public Rect GetRect()
        {
            return baseRect;
        }

        public bool Contains(Vector2 point)
        {
            return baseRect.Contains(point);
        }

        public bool CheckToolTip(Vector2 MousePosition)
        {
            if(Contains(MousePosition))
            {
                ToolTip.SetText(ToolTipTitle, ToolTipBody);
                return true;
            }
            return false;
        }
    }
}