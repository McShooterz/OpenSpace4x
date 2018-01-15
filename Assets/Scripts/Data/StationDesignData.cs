/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes: This is a ship design "baked" into just the stats of a design that shipData's will point to for their base stats
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections.Generic;

public class StationDesignData
{
    #region variables
    public StationDesign Design;
    public StationHullData Hull;

    public Dictionary<FighterDefinition, int> Fighters = new Dictionary<FighterDefinition, int>();
    public Dictionary<FighterDefinition, int> HeavyFighters = new Dictionary<FighterDefinition, int>();
    public Dictionary<FighterDefinition, int> AssaultPods = new Dictionary<FighterDefinition, int>();

    public float ProductionCost = 0;
    public float AlloyCost = 0;
    public float AdvancedAlloyCost = 0;
    public float SuperiorAlloyCost = 0;
    public float CrystalCost = 0;
    public float RareCrystalCost = 0;
    public float ExoticCrystalCost = 0;
    public float ExoticParticleCost = 0;

    public float PowerGenerated = 0;
    public float Ammo = 0;
    public float PowerStorage = 0;
    public int Crew;
    public int MinCrew;
    public float RequiredCrew;

    public float Sensor;
    public float LongRangeSensor;
    public float AdvancedSensor;

    public float DamageBonus = 0;
    public float DefenseBonus = 0;

    public float Research;
    public float Mining = 0;
    public float Repair;
    public float AmmoGenerated = 0;
    public int Transporter = 0;
    public int Troops = 0;
    public float Medical = 0;
    public float BoardingDefense = 0;
    public float Diplomacy = 0;

    //Defenses
    public float Health;
    public float ArmorHealth;
    public float ArmorRating;
    public float ShieldHealth;
    public float ShieldRating;
    public float ShieldRechargeRate;
    public float ShieldRechargeDelay;

    // Jamming
    public int JammingCount;
    public float JammingRange = 0;
    public float JammingDelay = 0;

    public float maxRange;

    public List<SavedWeapon> Weapons = new List<SavedWeapon>();
    #endregion

    public StationDesignData(StationDesign design)
    {
        Design = design;
        Hull = design.GetHull();

        float Power = 0;
        float PowerEffiency = 0;

        StationSlotLayout SlotLayout = Hull.GetSlotLayout();
        int SlotCount = SlotLayout.SlotList.Count;

        //Set base stats from hulldata
        Research = Hull.BaseResearch;
        Repair = Hull.BaseRepair;
        Sensor = Hull.BaseSensor;
        LongRangeSensor = Hull.BaseLongRangeSensor;
        AdvancedSensor = Hull.BaseAdvancedSensor;
        Crew = Hull.BaseCrew;
        Health = Hull.BaseHealthPerSlot * SlotCount;
        ArmorHealth = Hull.BaseArmorPerSlot * SlotCount;

        //Get stats from each module
        foreach (DesignModule designModule in Design.Modules)
        {
            Module module = ResourceManager.instance.GetModule(designModule.Module);

            ProductionCost += module.ProductionCost;
            AlloyCost += module.AlloyCost;
            AdvancedAlloyCost += module.AdvancedAlloyCost;
            SuperiorAlloyCost += module.SuperiorAlloyCost;
            CrystalCost += module.CrystalCost;
            RareCrystalCost += module.RareCrystalCost;
            ExoticCrystalCost += module.ExoticCrystalCost;
            ExoticParticleCost += module.ExoticParticleCost;

            PowerGenerated += module.PowerGenerated;
            Ammo += module.Ammo;
            Power += module.Power;
            PowerStorage += module.PowerStorage;
            Crew += module.Crew;
            RequiredCrew += module.RequiredCrew;

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
            BoardingDefense += module.BoardingDefense;
            Diplomacy += module.Diplomacy;

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
                    JammingDelay = Mathf.Max(JammingDelay, module.JammingDelay);
            }

            DamageBonus = Mathf.Max(DamageBonus, module.DamageBonus);
            DefenseBonus = Mathf.Max(DefenseBonus, module.DefenseBonus);
            PowerEffiency = Mathf.Max(PowerEffiency, module.PowerEfficiency);

            Health += module.Health;
            ArmorHealth += module.ArmorHealth;
            ArmorRating += module.ArmorRating;
            ShieldHealth += module.ShieldHealth;
            ShieldRating += module.ShieldRating;
            ShieldRechargeDelay = Mathf.Max(ShieldRechargeDelay, module.ShieldRechargeDelay);
            ShieldRechargeRate += module.ShieldRechargeRate;

            foreach (string fighter in module.Fighters)
            {
                AddFighters(fighter, Fighters);
            }
            foreach (string fighter in module.HeavyFighters)
            {
                AddFighters(fighter, HeavyFighters);
            }

            Weapon weapon = module.GetWeapon();
            if (weapon != null)
            {
                Weapons.Add(new SavedWeapon(designModule.Rotation, weapon));
            }
        }

        MinCrew = (int)(Crew * ResourceManager.instance.GetGameConstants().MinCrewPercent);
        maxRange = CalculateMaxRange();
    }

    public float GetTotalValue()
    {
        return ResourceManager.instance.GetGameConstants().GetBaseResourceValue(ProductionCost, AlloyCost, AdvancedAlloyCost, SuperiorAlloyCost, CrystalCost, RareCrystalCost, ExoticCrystalCost, ExoticParticleCost);
    }

    void AddFighters(string fighterType, Dictionary<FighterDefinition, int> fighterDictionary)
    {
        FighterDefinition fighterDefinition = ResourceManager.instance.GetFighterDefinition(fighterType);
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

    public float CalculateMaxRange()
    {
        float maxRange = 0;
        float calRange;
        foreach (SavedWeapon weapon in Weapons)
        {
            calRange = weapon.weapon.GetMaxRange();
            if (calRange > maxRange)
                maxRange = calRange;
        }
        return maxRange;
    }
}
