/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections.Generic;

public class Module
{
    #region Variables
    public string Texture { get; set; }
    public int SizeX { get; set; }
    public int SizeY { get; set; }

    //Cost
    public float ProductionCost { get; set; }
    public float AlloyCost { get; set; }
    public float AdvancedAlloyCost { get; set; }
    public float SuperiorAlloyCost { get; set; }
    public float CrystalCost { get; set; }
    public float RareCrystalCost { get; set; }
    public float ExoticCrystalCost { get; set; }
    public float ExoticParticleCost { get; set; }

    //General
    public float Health { get; set; }
    public float Mass { get; set; }
    public float Power { get; set; }
    public float PowerGenerated { get; set; }
    public float Fuel { get; set; }
    public float Ammo { get; set; }
    public float PowerStorage { get; set; }

    //Defense
    public float ArmorHealth { get; set; }
    public float ArmorRating { get; set; }
    public float ShieldHealth { get; set; }
    public float ShieldRating { get; set; }
    public float ShieldRechargeRate { get; set; }
    public float ShieldRechargeDelay { get; set; }

    //Supplies
    public int Crew { get; set; }
    public float RequiredCrew { get; set; }
    public float Supplies { get; set; }

    //Engines
    public float EngineFTL { get; set; }
    public float EngineThrust { get; set; }
    public float EngineTurn { get; set; }
    public float EngineBonus { get; set; }

    //Sensors
    public float Sensor { get; set; }
    public float LongRangeSensor { get; set; }
    public float AdvancedSensor { get; set; }

    //Bonuses
    public float DamageBonus { get; set; }
    public float DefenseBonus { get; set; }
    public float DamageBonusFleet { get; set; }
    public float DefenseBonusFleet { get; set; }
    public float PowerEfficiency { get; set; }
    public int CommandPointBonusFleet { get; set; }

    //Fighters
    public List<string> Fighters { get; set; }
    public List<string> HeavyFighters { get; set; }
    public List<string> AssaultPods { get; set; }

    //Misc
    public float Research { get; set; }
    public float Mining { get; set; }
    public float Repair { get; set; }
    public float AmmoGenerated { get; set; }
    public int Transporter { get; set; }
    public int Troops { get; set; }
    public int CommandPointReduction { get; set; }
    public float Medical { get; set; }
    public float CloakingPowerPerMass { get; set; }
    public float Stealth { get; set; }
    public float BoardingDefense { get; set; }
    public float Colonies { get; set; }
    public float Diplomacy { get; set; }
    public float Construction { get; set; }
    public float ExperienceBonus { get; set; }

    //Bombing
    public float BombArmyDamage { get; set; }
    public float BombStructureDamage { get; set; }
    public float BombPopulationDamage { get; set; }
    public float BombPollution { get; set; }

    //Jamming
    public int JammingCount { get; set; }
    public float JammingRange { get; set; }
    public float JammingDelay { get; set; }

    //Weapon
    public string Weapon { get; set; }

    ModuleSet ParentSet = null;
    #endregion

    //Set default values in constructor
    public Module()
    {
        Texture = "";
        Weapon = "";

        Fighters = new List<string>();
        HeavyFighters = new List<string>();
        AssaultPods = new List<string>();
    }

    public void SetParentSet(ModuleSet parent)
    {
        ParentSet = parent;
    }

    public ModuleSet GetParentSet()
    {
        return ParentSet;
    }

    public Texture2D GetTexture()
    {
        return ResourceManager.GetModuleTexture(Texture);
    }

    public Weapon GetWeapon()
    {
        return ResourceManager.GetWeapon(Weapon);
    }

    public bool GetWeaponExists()
    {
        return ResourceManager.WeaponExists(Weapon);
    }

    public int GetFighterCount()
    {
        return SumFightersFromList(Fighters);
    }

    public int GetHeavyFighterCount()
    {
        return SumFightersFromList(HeavyFighters);
    }

    public int GetAssaultPodCount()
    {
        return SumFightersFromList(AssaultPods);
    }

    int SumFightersFromList(List<string> fighterList)
    {
        int count = 0;
        foreach (string fighterName in fighterList)
        {
            FighterDefinition fighterDefinition = ResourceManager.GetFighterDefinition(fighterName);
            if (fighterDefinition != null)
            {
                count += fighterDefinition.MaxSquadronSize;
            }
        }
        return count;
    }

    public float GetFighterFirePower()
    {
        float value = 0;
        foreach (string fighterName in Fighters)
        {
            FighterDefinition fighterDefinition = ResourceManager.GetFighterDefinition(fighterName);
            if(fighterDefinition != null)
            {
                value += fighterDefinition.GetSquadronFirePower();
            }
        }
        foreach (string fighterName in HeavyFighters)
        {
            FighterDefinition fighterDefinition = ResourceManager.GetFighterDefinition(fighterName);
            if (fighterDefinition != null)
            {
                value += fighterDefinition.GetSquadronFirePower();
            }
        }
        return value;
    }

    public FighterDefinition GetFirstFighter()
    {
        return ResourceManager.GetFighterDefinition(Fighters[0]);
    }

    public FighterDefinition GetFirstHeavyFighter()
    {
        return ResourceManager.GetFighterDefinition(HeavyFighters[0]);
    }

    public FighterDefinition GetFirstAssaultPod()
    {
        return ResourceManager.GetFighterDefinition(AssaultPods[0]);
    }

    public void ApplyFiringRangeFactor()
    {
        JammingRange *= ResourceManager.gameConstants.FiringRangeFactor;
    }

    public float GetJammingRangeDisplay()
    {
        return JammingRange / ResourceManager.gameConstants.FiringRangeFactor;
    }

    public float GetCost()
    {
        return ResourceManager.gameConstants.GetBaseResourceValue(ProductionCost, AlloyCost, AdvancedAlloyCost, SuperiorAlloyCost, CrystalCost, RareCrystalCost, ExoticCrystalCost, ExoticParticleCost);
    }

    public float GetDefenseRating()
    {
        return (ShieldRating / 18f + ShieldRechargeRate * 1.4f + ShieldHealth / 200f + ArmorRating + ArmorHealth / 200f + Health / 200f) / 8f;
    }
}
