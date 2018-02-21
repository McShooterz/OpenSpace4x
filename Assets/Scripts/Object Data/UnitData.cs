/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections.Generic;

public abstract class UnitData
{
    public SpaceUnit CurrentUnit;

    //Naming
    public string DisplayName;
    public Empire Owner;
    public FleetData fleetData;
    public bool Destroyed = false;

    //Fighters
    public List<FighterComplement> Fighters = new List<FighterComplement>();
    public List<FighterComplement> HeavyFighters = new List<FighterComplement>();
    public List<FighterComplement> AssaultPods = new List<FighterComplement>();

    //Boarding
    protected List<BoardingForce> boardingForces = new List<BoardingForce>();
    protected float boardingTimer = 0;

    //Dynamic Stats
    public float ammo;
    public float ammoMax;
    public float power;
    public float powerMax;
    public int crew;
    public int crewMax;
    public int troops;
    public int troopsMax;

    //Static Stats
    public float powerGenerated;
    public float research;
    public float mining;
    public float repair;
    public float ammoGenerated;
    public int transporter;
    public float medical;
    public float boardingDefense;
    public float diplomacy;
    public float sensor;
    public float longRangeSensor;
    public float advancedSensor;

    //Calculated Stats
    protected float crewEfficiency;

    //Battlestats
    public int Kills = 0;
    public float damageDealt = 0;
    public float damageTaken = 0;

    public void SetFleet(FleetData FD)
    {
        fleetData = FD;
    }

    public FleetData GetFleet()
    {
        return fleetData;
    }

    protected void UpdateBoarding()
    {
        if (boardingForces.Count > 0)
        {
            if (boardingTimer > 0)
            {
                boardingTimer -= GetDeltaTime();
            }
            else
            {
                ProcessBoardingForces(boardingForces[0]);
                if (boardingForces.Count > 0)
                    boardingTimer = Random.Range(ResourceManager.instance.GetGameConstants().boardingIntervalMin, ResourceManager.instance.GetGameConstants().boardingIntervalMax);
                else
                    CurrentUnit.RemoveBoardingBalanceBar();
                      
            }
        }
    }

    public abstract float GetDamageBonus();

    public virtual float GetWeaponDamageBonus(Weapon weapon)
    {
        //Get base damage bonus
        float damageBonus = GetDamageBonus();
        //Apply weapon specific bonuses

        return damageBonus;
    }

    public abstract float GetDefenseBonus();

    public bool HasFighters()
    {
        foreach (FighterComplement complement in Fighters)
        {
            if (complement.GetCount() > 0)
                return true;
        }
        return false;
    }

    public bool HasHeavyFighters()
    {
        foreach (FighterComplement complement in HeavyFighters)
        {
            if (complement.GetCount() > 0)
                return true;
        }
        return false;
    }

    public bool HasAssaultPods()
    {
        foreach(FighterComplement complement in AssaultPods)
        {
            if (complement.GetCount() > 0)
                return true;
        }
        return false;
    }

    public abstract void RemoveFromFleet();

    public abstract void DestroyAllBeams();

    public abstract void DestroyWeaponEffects();

    public bool ConsumePower(float powerUsed)
    {
        if (powerUsed > power)
            return false;
        else
            power -= powerUsed;
        return true;
    }

    public bool CanGetFleetAmmo(float amount)
    {
        if (fleetData != null)
        {
            return fleetData.CanTransportAmmo(amount, ammo);
        }
        return false;
    }

    public bool CanGetAmmo(float amount)
    {
        if (ammo >= amount)
            return true;
        return CanGetFleetAmmo(amount);
    }

    public void ConsumePowerAndAmmo(float powerNeed, float AmmoNeed)
    {
        power -= powerNeed;
        if (AmmoNeed < ammo)
        {
            ammo -= AmmoNeed;
        }
        else
        {
            fleetData.ConsumeFleetAmmo(this, AmmoNeed);
        }
    }

    public abstract float GetMaxHealth();

    public void DamageCrew(float damage)
    {
        removeCrew(Mathf.CeilToInt(damage / GetMaxHealth() * crew));
    }

    public void AddCrew(int count)
    {
        crew += count;
        if (crew > crewMax)
            crew = crewMax;
        SetCrewEffeciency();
    }

    public void removeCrew(int count)
    {
        crew -= count;
        if (crew < 0)
            crew = 0;
        SetCrewEffeciency();
    }

    public void AddTroops(int count)
    {
        troops += count;
        if (troops > troopsMax)
            troops = troopsMax;
    }

    public void removeTroops(int count)
    {
        troops -= count;
        if (troops < 0)
            troops = 0;
    }

    public int RequestCrew(int amount)
    {
        int minCrew = GetMinCrew();
        if(crew > minCrew)
        {
            if(amount > minCrew)
            {
                amount = crew - minCrew;
                crew = minCrew;
                return amount;
            }
            else
            {
                crew -= amount;
                return amount;
            }
        }
        return 0;
    }

    public int RequestTroops(int amount)
    {
        if(troops > 0)
        {
            if(amount > troops)
            {
                amount = troops;
                troops = 0;
                return amount;
            }
            else
            {
                troops -= amount;
                return amount;
            }
        }
        return 0;
    }

    public float GetCrewRatio()
    {
        if (crewMax > 0)
            return Mathf.Min(1f, (float)crew / crewMax);
        else
            return 0;
    }

    public float GetTroopRatio()
    {
        if (troopsMax > 0)
            return Mathf.Min(1f, (float)troops / troopsMax);
        else
            return 0;
    }

    protected abstract void SetCrewEffeciency();

    public float GetcrewEfficiency()
    {
        return crewEfficiency;
    }

    protected abstract int GetMinCrew();

    public abstract int GetJammingCount();

    public abstract float GetJammingRange();

    public abstract float GetJammingRangeSqr();

    public abstract float GetJammingDelay();

    public abstract bool hasPointDefense();

    protected float GetDeltaTime()
    {
        return GameManager.instance.GetDeltaTime();
    }

    public void CreateShieldDamagePopup(Vector3 position, float value)
    {
        CurrentUnit.CreateShieldDamagePopup(position, value);
    }

    public void CreateArmorDamagePopup(Vector3 position, float value)
    {
        CurrentUnit.CreateArmorDamagePopup(position, value);
    }

    public void CreateHealthDamagePopup(Vector3 position, float value)
    {
        CurrentUnit.CreateHealthDamagePopup(position, value);
    }

    public void ReturnFighter(Fighter fighter)
    {
        AddCrew(fighter.GetCrew());

        foreach(FighterComplement complement in Fighters)
        {
            if(complement.GetFighterType() == fighter.GetDefinition())
            {
                complement.AddFighter();
            }
        }
    }

    public void ReturnHeavyFighter(Fighter fighter)
    {
        AddCrew(fighter.GetCrew());

        foreach (FighterComplement complement in HeavyFighters)
        {
            if (complement.GetFighterType() == fighter.GetDefinition())
            {
                complement.AddFighter();
            }
        }
    }

    public void ReturnAssaultPod(Fighter fighter)
    {
        AddCrew(fighter.GetCrew());
        AddTroops(fighter.GetTroops());

        foreach (FighterComplement complement in AssaultPods)
        {
            if (complement.GetFighterType() == fighter.GetDefinition())
            {
                complement.AddFighter();
            }
        }
    }

    public class FighterComplement
    {
        FighterDefinition fighterType;
        int Count;
        int MaxCount;

        public FighterComplement(FighterDefinition fighterDefinition, int count)
        {
            fighterType = fighterDefinition;
            Count = count;
            MaxCount = Count;
        }

        public FighterDefinition GetFighterType()
        {
            return fighterType;
        }

        public int GetCount()
        {
            return Count;
        }

        public void SetCount(int count)
        {
            Count = count;
            if (Count > MaxCount)
                Count = MaxCount;
        }

        public void AddFighter()
        {
            Count++;
            if (Count > MaxCount)
                Count = MaxCount;
        }
    }

    public float GetCombatStrength()
    {
        return troops + crew * 0.5f;
    }

    public void AddBoardingForce(ShipManager manager, int Troops, int Crew)
    {
        foreach (BoardingForce force in boardingForces)
        {
            if (force.GetShipManager() == manager)
            {
                force.AddForces(Troops, Crew);
                return;
            }
        }
        boardingForces.Add(new BoardingForce(manager, Troops, Crew));
    }

    void ProcessBoardingForces(BoardingForce enemyBoarders)
    {
        if (!CurrentUnit.GetShipManager().GetEnemyShipManagers().Contains(enemyBoarders.GetShipManager()))
        {
            AbsorbBoardingForce(enemyBoarders);
            return;
        }

        //Create boarding balance bar to show who has the advantage
        float AttackerCombatStrength = enemyBoarders.GetCombatStrength();
        float DefendersCombatStrength = GetCombatStrength();

        CurrentUnit.SetBoardingForceBalance(1 - (AttackerCombatStrength / (DefendersCombatStrength)));

        //Temporary hardcoded values
        float troopMin = 0f;
        float troopMax = 15f;
        float crewMin = 0f;
        float crewMax = 10f;

        float attacker = 0;
        float defender = 0;
        if (enemyBoarders.HasTroops())
        {
            attacker = Random.Range(troopMin, troopMax);
        }
        else if (enemyBoarders.HasCrew())
        {
            attacker = Random.Range(crewMin, crewMax);
        }
        else
        {
            boardingForces.Remove(enemyBoarders);
            return;
        }

        if (troops > 0)
        {
            defender = Random.Range(troopMin, troopMax);
        }
        else if (crew > 0)
        {
            defender = Random.Range(crewMin, crewMax);
        }
        else
        {
            enemyBoarders.GetShipManager().CaptureUnit(CurrentUnit);
            AbsorbBoardingForce(enemyBoarders);
            return;
        }

        //Apply defense bonus
        defender += Random.Range(0f, boardingDefense);

        if (attacker > defender)
        {
            if (troops > 0)
                removeTroops(1);
            else
                removeCrew(1);
        }
        else
        {
            if (enemyBoarders.HasTroops())
                enemyBoarders.KillTrooper();
            else
                enemyBoarders.KillCrew();
        }
    }

    void AbsorbBoardingForce(BoardingForce force)
    {
        AddCrew(force.GetCrewCount());
        AddTroops(force.GetTroopCount());
        boardingForces.Remove(force);
    }

    public abstract Sprite GetIcon();
}
