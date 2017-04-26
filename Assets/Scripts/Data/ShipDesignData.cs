/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes: This is a ship design "baked" into just the stats of a design that shipData's will point to for their base stats
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections.Generic;

public class ShipDesignData
{
    #region variables
    public ShipDesign Design { get; set; }
    public ShipHullData Hull { get; set; }

    public Dictionary<FighterDefinition, int> Fighters { get; set; }
    public Dictionary<FighterDefinition, int> HeavyFighters { get; set; }
    public Dictionary<FighterDefinition, int> AssaultPods { get; set; }

    public int CommandPoints { get; set; }

    public float ProductionCost { get; set; }
    public float AlloyCost { get; set; }
    public float AdvancedAlloyCost { get; set; }
    public float SuperiorAlloyCost { get; set; }
    public float CrystalCost { get; set; }
    public float RareCrystalCost { get; set; }
    public float ExoticCrystalCost { get; set; }
    public float ExoticParticleCost { get; set; }

    public float Mass { get; set; }
    public float PowerGenerated { get; set; }
    public float Fuel { get; set; }
    public float Ammo { get; set; }
    public float PowerStorage { get; set; }
    public int Crew { get; set; }
    public int MinCrew { get; set; }
    public float RequiredCrew { get; set; }
    public float Supplies { get; set; }
    public float FTLSpeed { get; set; }

    public float Sensor { get; set; }
    public float LongRangeSensor { get; set; }
    public float AdvancedSensor { get; set; }

    public float DamageBonus { get; set; }
    public float DefenseBonus { get; set; }
    public float DamageBonusFleet { get; set; }
    public float DefenseBonusFleet { get; set; }
    public float EngineBonus { get; set; }
    public int CommandPointBonusFleet { get; set; }

    public float EngineForwardSpeed { get; set; }
    public float EngineTurnSpeed { get; set; }

    public float Research { get; set; }
    public float Mining { get; set; }
    public float Repair { get; set; }
    public float AmmoGenerated { get; set; }
    public int Transporter { get; set; }
    public int Troops { get; set; }
    public float Medical { get; set; }
    public float CloakingPower { get; set; }
    public float Stealth { get; set; }
    public float BoardingDefense { get; set; }
    public float Colonies { get; set; }
    public float Diplomacy { get; set; }
    public float Construction { get; set; }
    public float ExperienceBonus { get; set; }

    // Jamming
    public int JammingCount { get; set; }
    public float JammingRange { get; set; }
    public float JammingDelay { get; set; }

    public ShipDesignQuadData ForeQuadrant { get; set; }
    public ShipDesignQuadData AftQuadrant { get; set; }
    public ShipDesignQuadData PortQuadrant { get; set; }
    public ShipDesignQuadData StarboardQuadrant { get; set; }
    public ShipDesignQuadData CenterQuadrant { get; set; }

    public float maxRange { get; set; }
    public float[] DamageGraph { get; set; }
    #endregion

    public ShipDesignData(ShipDesign design)
    {
        Fighters = new Dictionary<FighterDefinition, int>();
        HeavyFighters = new Dictionary<FighterDefinition, int>();
        AssaultPods = new Dictionary<FighterDefinition, int>();

        ForeQuadrant = new ShipDesignQuadData();
        AftQuadrant = new ShipDesignQuadData();
        PortQuadrant = new ShipDesignQuadData();
        StarboardQuadrant = new ShipDesignQuadData();
        CenterQuadrant = new ShipDesignQuadData();

        DamageGraph = new float[20];

        Design = design;
        Hull = Design.GetHull();

        float EngineFTL = 0;
        float Power = 0;
        int CommandPointReduction = 0;
        float PowerEffiency = 0;
        float CloakingPowerPerMass = 0;

        int ForeSlotCount = 0;
        int AftSlotCount = 0;
        int PortSlotCount = 0;
        int StarboardSlotCount = 0;
        int CenterSlotCount = 0;
        ShipSlotLayout slotLayout = Hull.GetSlotLayout();
        if (slotLayout != null)
        {
            ForeSlotCount = slotLayout.ForeSlots.Count;
            AftSlotCount = slotLayout.AftSlots.Count;
            PortSlotCount = slotLayout.PortSlots.Count;
            StarboardSlotCount = slotLayout.StarboardSlots.Count;
            CenterSlotCount = slotLayout.CenterSlots.Count;
        }

        //Set base stats from hulldata
        CommandPoints = Hull.BaseCommandPoints;
        Research = Hull.BaseResearch;
        Repair = Hull.BaseRepair;
        Sensor = Hull.BaseSensor;
        LongRangeSensor = Hull.BaseLongRangeSensor;
        AdvancedSensor = Hull.BaseAdvancedSensor;
        Fuel = Hull.BaseFuel;
        Crew = Hull.BaseCrew;
        Supplies = Hull.BaseSupply;
        FTLSpeed = Hull.BaseFTL;
        ForeQuadrant.Health = Hull.BaseHealthPerSlot * ForeSlotCount;
        ForeQuadrant.ArmorHealth = Hull.BaseArmorPerSlot * ForeSlotCount;
        AftQuadrant.Health = Hull.BaseHealthPerSlot * AftSlotCount;
        AftQuadrant.ArmorHealth = Hull.BaseArmorPerSlot * AftSlotCount;
        PortQuadrant.Health = Hull.BaseHealthPerSlot * PortSlotCount;
        PortQuadrant.ArmorHealth = Hull.BaseArmorPerSlot * PortSlotCount;
        StarboardQuadrant.Health = Hull.BaseHealthPerSlot * StarboardSlotCount;
        StarboardQuadrant.ArmorHealth = Hull.BaseArmorPerSlot * StarboardSlotCount;
        CenterQuadrant.Health = Hull.BaseHealthPerSlot * CenterSlotCount;
        CenterQuadrant.ArmorHealth = Hull.BaseArmorPerSlot * CenterSlotCount;

        //Get stats from each module
        AddQuadModuleStats(ForeQuadrant, Design.ForeModules, ref Power, ref CloakingPowerPerMass, ref CommandPointReduction, ref PowerEffiency, ref EngineFTL);
        AddQuadModuleStats(AftQuadrant, Design.AftModules, ref Power, ref CloakingPowerPerMass, ref CommandPointReduction, ref PowerEffiency, ref EngineFTL);
        AddQuadModuleStats(PortQuadrant, Design.PortModules, ref Power, ref CloakingPowerPerMass, ref CommandPointReduction, ref PowerEffiency, ref EngineFTL);
        AddQuadModuleStats(StarboardQuadrant, Design.StarboardModules, ref Power, ref CloakingPowerPerMass, ref CommandPointReduction, ref PowerEffiency, ref EngineFTL);
        AddQuadModuleStats(CenterQuadrant, Design.CenterModules, ref Power, ref CloakingPowerPerMass, ref CommandPointReduction, ref PowerEffiency, ref EngineFTL);

        //Last stat calcs
        CommandPoints -= CommandPointReduction;
        FTLSpeed += EngineFTL / Mass;
        PowerGenerated -= Power * (1 - PowerEffiency);
        CloakingPower = CloakingPowerPerMass * Mass;
        MinCrew = (int)(Crew * ResourceManager.gameConstants.MinCrewPercent);

        maxRange = CalculateMaxRange();
        BuildDamageGraph();
    }

    void AddQuadModuleStats(ShipDesignQuadData designQuad, List<DesignModule> modules, ref float Power, ref float CloakingPowerPerMass, ref int CommandPointReduction, ref float PowerEffiency, ref float EngineFTL)
    {
        foreach (DesignModule designModule in modules)
        {
            Module module = ResourceManager.GetModule(designModule.Module);

            ProductionCost += module.ProductionCost;
            AlloyCost += module.AlloyCost;
            AdvancedAlloyCost += module.AdvancedAlloyCost;
            SuperiorAlloyCost += module.SuperiorAlloyCost;
            CrystalCost += module.CrystalCost;
            RareCrystalCost += module.RareCrystalCost;
            ExoticCrystalCost += module.ExoticCrystalCost;
            ExoticParticleCost += module.ExoticParticleCost;

            Mass += module.Mass;
            PowerGenerated += module.PowerGenerated;
            Fuel += module.Fuel;
            Ammo += module.Ammo;
            Power += module.Power;
            PowerStorage += module.PowerStorage;
            Crew += module.Crew;
            RequiredCrew += module.RequiredCrew;
            Supplies += module.Supplies;

            Sensor += module.Sensor;
            LongRangeSensor += module.LongRangeSensor;
            AdvancedSensor += module.AdvancedSensor;

            Research += module.Research;
            Mining += module.Mining;
            Repair += module.Repair;
            AmmoGenerated += module.AmmoGenerated;
            Transporter += module.Transporter;
            Troops += module.Troops;
            Medical += module.Medical;
            if (CloakingPowerPerMass == 0)
            {
                CloakingPowerPerMass = module.CloakingPowerPerMass;
            }
            else if (module.CloakingPowerPerMass > 0)
            {
                CloakingPowerPerMass = Mathf.Min(CloakingPowerPerMass, module.CloakingPowerPerMass);
            }
            Stealth += module.Stealth;
            BoardingDefense += module.BoardingDefense;
            Colonies += module.Colonies;
            Diplomacy += module.Diplomacy;
            Construction += module.Construction;
            ExperienceBonus = Mathf.Max(ExperienceBonus, module.ExperienceBonus);

            JammingCount += module.JammingCount;
            if (module.JammingRange > 0)
            {
                if (JammingRange == 0)
                    JammingRange = module.JammingRange;
                else
                    JammingRange = Mathf.Min(JammingRange, module.JammingRange);
            }
            if (module.JammingDelay > 0)
            {
                if (JammingDelay == 0)
                    JammingDelay = module.JammingDelay;
                else
                    JammingDelay = Mathf.Min(JammingDelay, module.JammingDelay);
            }

            DamageBonus = Mathf.Max(DamageBonus, module.DamageBonus);
            DefenseBonus = Mathf.Max(DefenseBonus, module.DefenseBonus);
            DamageBonusFleet = Mathf.Max(DamageBonusFleet, module.DamageBonusFleet);
            DefenseBonusFleet = Mathf.Max(DefenseBonusFleet, module.DefenseBonusFleet);
            CommandPointReduction = Mathf.Max(CommandPointReduction, module.CommandPointReduction);
            CommandPointBonusFleet = Mathf.Max(CommandPointBonusFleet, module.CommandPointBonusFleet);
            PowerEffiency = Mathf.Max(PowerEffiency, module.PowerEfficiency);
            EngineBonus = Mathf.Max(EngineBonus, module.EngineBonus);
            EngineFTL += module.EngineFTL;

            designQuad.Health += module.Health;
            designQuad.ArmorHealth += module.ArmorHealth;
            designQuad.ArmorRating += module.ArmorRating;
            designQuad.ShieldHealth += module.ShieldHealth;
            designQuad.ShieldRating += module.ShieldRating;
            designQuad.ShieldRechargeDelay = Mathf.Max(CenterQuadrant.ShieldRechargeDelay, module.ShieldRechargeDelay);
            designQuad.ShieldRechargeRate += module.ShieldRechargeRate;

            designQuad.EngineThrust += module.EngineThrust;
            designQuad.EngineTurn += module.EngineTurn;

            foreach (string fighter in module.Fighters)
            {
                AddFighters(fighter, Fighters);
            }
            foreach (string heavyFighter in module.HeavyFighters)
            {
                AddFighters(heavyFighter, HeavyFighters);
            }
            foreach(string assaultPod in module.AssaultPods)
            {
                AddFighters(assaultPod, AssaultPods);
            }

            Weapon weapon = module.GetWeapon();
            if (weapon != null)
            {
                designQuad.AddWeapon(designModule.Rotation, weapon);
            }
        }
    }

    public float GetTotalValue()
    {
        return ResourceManager.gameConstants.GetBaseResourceValue(ProductionCost, AlloyCost, AdvancedAlloyCost, SuperiorAlloyCost, CrystalCost, RareCrystalCost, ExoticCrystalCost, ExoticParticleCost);
    }

    void AddFighters(string fighterType, Dictionary<FighterDefinition, int> fighterDictionary)
    {
        FighterDefinition fighterDefinition = ResourceManager.GetFighterDefinition(fighterType);
        if (fighterDefinition != null)
        {
            if (fighterDictionary.ContainsKey(fighterDefinition))
            {
                fighterDictionary[fighterDefinition] += fighterDefinition.MaxSquadronSize;
            }
            else
            {
                fighterDictionary.Add(fighterDefinition, fighterDefinition.MaxSquadronSize);
            }
        }
    }

    void BuildDamageGraph()
    {
        float increment = maxRange / 20f;
        float maxDamage = 0;
        float damageSum;
        float range;

        for (int i = 0; i < 20; i++)
        {
            damageSum = 0;
            foreach(SavedWeapon weapon in ForeQuadrant.Weapons)
            {
                range = i * increment;
                if (range <= weapon.weapon.GetMaxRange())
                {
                    damageSum += weapon.weapon.GetBarGraphDamage(range);
                }
            }
            foreach (SavedWeapon weapon in AftQuadrant.Weapons)
            {
                range = i * increment;
                if (range <= weapon.weapon.GetMaxRange())
                {
                    damageSum += weapon.weapon.GetBarGraphDamage(range);
                }
            }
            foreach (SavedWeapon weapon in PortQuadrant.Weapons)
            {
                range = i * increment;
                if (range <= weapon.weapon.GetMaxRange())
                {
                    damageSum += weapon.weapon.GetBarGraphDamage(range);
                }
            }
            foreach (SavedWeapon weapon in StarboardQuadrant.Weapons)
            {
                range = i * increment;
                if (range <= weapon.weapon.GetMaxRange())
                {
                    damageSum += weapon.weapon.GetBarGraphDamage(range);
                }
            }
            foreach (SavedWeapon weapon in CenterQuadrant.Weapons)
            {
                range = i * increment;
                if (range <= weapon.weapon.GetMaxRange())
                {
                    damageSum += weapon.weapon.GetBarGraphDamage(range);
                }
            }
            DamageGraph[i] = damageSum;
            if (damageSum > maxDamage)
                maxDamage = damageSum;
        }

        for(int i = 0; i < 20; i++)
        {
            DamageGraph[i] /= maxDamage;
        }
    }

    public float CalculateMaxRange()
    {
        float maxRange = 0;
        float calRange;
        foreach (SavedWeapon weapon in ForeQuadrant.Weapons)
        {
            calRange = weapon.weapon.GetMaxRange();
            if (calRange > maxRange)
                maxRange = calRange;
        }
        foreach (SavedWeapon weapon in AftQuadrant.Weapons)
        {
            calRange = weapon.weapon.GetMaxRange();
            if (calRange > maxRange)
                maxRange = calRange;
        }
        foreach (SavedWeapon weapon in PortQuadrant.Weapons)
        {
            calRange = weapon.weapon.GetMaxRange();
            if (calRange > maxRange)
                maxRange = calRange;
        }
        foreach (SavedWeapon weapon in StarboardQuadrant.Weapons)
        {
            calRange = weapon.weapon.GetMaxRange();
            if (calRange > maxRange)
                maxRange = calRange;
        }
        foreach (SavedWeapon weapon in CenterQuadrant.Weapons)
        {
            calRange = weapon.weapon.GetMaxRange();
            if (calRange > maxRange)
                maxRange = calRange;
        }
        return maxRange;
    }
}
