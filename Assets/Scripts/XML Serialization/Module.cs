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
    public string Texture = "";
    public int SizeX;
    public int SizeY;

    //Cost
    public float ProductionCost;
    public float AlloyCost;
    public float AdvancedAlloyCost;
    public float SuperiorAlloyCost;
    public float CrystalCost;
    public float RareCrystalCost;
    public float ExoticCrystalCost;
    public float ExoticParticleCost;

    //General
    public float Health;
    public float Mass;
    public float Power;
    public float PowerGenerated;
    public float Fuel;
    public float Ammo;
    public float PowerStorage;

    //Defense
    public float ArmorHealth;
    public float ArmorRating;
    public float ShieldHealth;
    public float ShieldRating;
    public float ShieldRechargeRate;
    public float ShieldRechargeDelay;

    //Supplies
    public int Crew;
    public float RequiredCrew;
    public float Supplies;

    //Engines
    public float EngineFTL;
    public float EngineThrust;
    public float EngineTurn;
    public float EngineBonus;

    //Sensors
    public float Sensor;
    public float LongRangeSensor;
    public float AdvancedSensor;

    //Bonuses
    public float DamageBonus;
    public float DefenseBonus;
    public float DamageBonusFleet;
    public float DefenseBonusFleet;
    public float PowerEfficiency;
    public int CommandPointBonusFleet;

    //Fighters
    public List<string> Fighters = new List<string>();
    public List<string> HeavyFighters = new List<string>();
    public List<string> AssaultPods = new List<string>();

    //Misc
    public float Research;
    public float Mining;
    public float Repair;
    public float AmmoGenerated;
    public int Transporter;
    public int Troops;
    public int CommandPointReduction;
    public float Medical;
    public float CloakingPowerPerMass;
    public float Stealth;
    public float BoardingDefense;
    public float Colonies;
    public float Diplomacy;
    public float Construction;
    public float ExperienceBonus;

    //Bombing
    public float BombArmyDamage;
    public float BombStructureDamage;
    public float BombPopulationDamage;
    public float BombPollution;

    //Jamming
    public int JammingCount;
    public float JammingRange;
    public float JammingDelay;

    //Weapon
    public string Weapon = "";

    ModuleSet ParentSet = null;
    #endregion

    public Module() { }

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
