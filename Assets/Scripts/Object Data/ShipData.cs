/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections.Generic;
using System;

public sealed class ShipData : UnitData
{
    #region Variables
    //Naming
    public ShipDesignData designData;
     
    public bool Retreated = false;

    //Quandrants of the ship
    public ShipQuadrant ForeSection;
    public ShipQuadrant AftSection;
    public ShipQuadrant StarboardSection;
    public ShipQuadrant PortSection;
    public ShipQuadrant CenterSection;

    //Leveling
    public int Level = 0;
    float Experience = 0;

    //Dynamic Stats  
    public float fuel;
    public float fuelMax;
    public float supplies;
    public float suppliesMax;

    //Static Stats
    public int commandPoints;
    public float mass;   
    public float FTLSpeed;
    public float cloakingPower;
    public float stealth;
    public float colonies;
    public float construction;

    //Calculated Stats
    public float engineThrust;
    public float engineTurn;

    //AI
    float attackRange;
	public QuadrantTypes AttackDirection;
	public AttackStyle AttackStyle;

    #endregion

    public ShipData(ShipDesignData shipDesignData)
    {
        designData = shipDesignData;
        DisplayName = designData.Design.Name;

        ForeSection = new ShipQuadrant(this, designData.ForeQuadrant, QuadrantTypes.Fore);
        AftSection = new ShipQuadrant(this, designData.AftQuadrant, QuadrantTypes.Aft);
        StarboardSection = new ShipQuadrant(this, designData.StarboardQuadrant, QuadrantTypes.Starboard);
        PortSection = new ShipQuadrant(this, designData.PortQuadrant, QuadrantTypes.Port);
        CenterSection = new ShipQuadrant(this, designData.CenterQuadrant, QuadrantTypes.Center);

        //Add Fighters
        foreach(KeyValuePair<FighterDefinition, int> keyVal in shipDesignData.Fighters)
        {
            Fighters.Add(new FighterComplement(keyVal.Key, keyVal.Value));
        }
        foreach (KeyValuePair<FighterDefinition, int> keyVal in shipDesignData.HeavyFighters)
        {
            HeavyFighters.Add(new FighterComplement(keyVal.Key, keyVal.Value));
        }
        foreach (KeyValuePair<FighterDefinition, int> keyVal in shipDesignData.AssaultPods)
        {
            AssaultPods.Add(new FighterComplement(keyVal.Key, keyVal.Value));
        }

        fuelMax = designData.Fuel;
        fuel = fuelMax;
        ammo = designData.Ammo;
        ammoMax = designData.Ammo;
        power = designData.PowerStorage;
        powerMax = designData.PowerStorage;
        crew = designData.Crew;
        crewMax = designData.Crew;
        supplies = designData.Supplies;
        suppliesMax = designData.Supplies;
        troops = designData.Troops;
        troopsMax = designData.Troops;
        commandPoints = designData.CommandPoints;
        mass = designData.Mass;
        powerGenerated = designData.PowerGenerated;
        FTLSpeed = designData.FTLSpeed;
        research = designData.Research;
        mining = designData.Mining;
        repair = designData.Repair;
        ammoGenerated = designData.AmmoGenerated;
        transporter = designData.Transporter;
        medical = designData.Medical;
        cloakingPower = designData.CloakingPower;
        stealth = designData.Stealth;
        boardingDefense = designData.BoardingDefense;
        colonies = designData.Colonies;
        diplomacy = designData.Diplomacy;
        sensor = designData.Sensor;
        longRangeSensor = designData.LongRangeSensor;
        advancedSensor = designData.AdvancedSensor;

        CalculateEngines();
        AttackDirection = designData.Design.attackDirection;
        AttackStyle = designData.Design.attackStyle;
        if (designData.Design.attackRange != -1)
        {
            attackRange = designData.Design.attackRange;
        }
        else
        {
            attackRange = designData.maxRange;
        }

        SetCrewEffeciency();
    }

    public void Update()
    {
        if (CurrentUnit == null || Destroyed)
            return;

        if ((CurrentUnit as Ship).IsCloaked())
        {
            if(power > 0)
            {
                power -= cloakingPower * GetDeltaTime();
            }
            else
            {
                power = 0;
                (CurrentUnit as Ship).DeCloak();
            }
        }
        else
        {
            if (power < powerMax)
            {
                power += powerGenerated * GetDeltaTime();
                if (power > powerMax)
                {
                    power = powerMax;
                }
            }
            if (ammoGenerated > 0 && ammo < ammoMax)
            {
                ammo += ammoGenerated * GetDeltaTime();
                if (ammo > ammoMax)
                {
                    ammo = ammoMax;
                }
            }

            ForeSection.Update();
            AftSection.Update();
            StarboardSection.Update();
            PortSection.Update();
            CenterSection.Update();
        }

        UpdateBoarding();
    }

    public void CalculateEngines()
    {
        float ThrustSum = 0;
        float TurnSum = 0;

        if(!ForeSection.isDestroyed())
        {
            ThrustSum += ForeSection.engineThrust;
            TurnSum += ForeSection.engineTurn;
        }
        if(!AftSection.isDestroyed())
        {
            ThrustSum += AftSection.engineThrust;
            TurnSum += AftSection.engineTurn;
        }
        if(!StarboardSection.isDestroyed())
        {
            ThrustSum += StarboardSection.engineThrust;
            TurnSum += StarboardSection.engineTurn;
        }
        if(!PortSection.isDestroyed())
        {
            ThrustSum += PortSection.engineThrust;
            TurnSum += PortSection.engineTurn;
        }
        if(!CenterSection.isDestroyed())
        {
            ThrustSum += CenterSection.engineThrust;
            TurnSum += CenterSection.engineTurn;
        }

        engineThrust = ThrustSum / (mass * 2.25f) * (1f + designData.EngineBonus) * ResourceManager.instance.GetGameConstants().ShipSTLSpeedMultiplier;
		engineTurn = TurnSum / (mass * (mass / 25f)) * (1f + designData.EngineBonus) * ResourceManager.instance.GetGameConstants().ShipSTLTurnMultiplier;
    }

    public void AttachShip(Ship ship)
    {
        if(CurrentUnit != null)
        {
            UnityEngine.Object.Destroy(CurrentUnit);
        }
        CurrentUnit = ship;
        ParentAttachedWeapons(CurrentUnit);
    }

    public void TakeDamage(AttachedWeapon weapon, Vector3 position, float damage, QuadrantTypes quad, bool ignoreShields, bool ignoreArmor, bool ignoreArmorRating)
    {
        //Apply defense bonus
        damage -= damage * GetDefenseBonus();

        //Record damage taken
        damageTaken += damage;

        if (CurrentUnit == null)
            return;

        if(weapon.baseWeapon.PowerDamageModifier != 0)
        {
            power -= damage * weapon.baseWeapon.PowerDamageModifier;
            if (power < 0)
                power = 0;
        }

        if (weapon.baseWeapon.DamageAllQuads)
        {
            float ForeDamage = damage / 4f;
            float PortDamage = ForeDamage;
            float StarboardDamage = ForeDamage;
            float AftDamage = ForeDamage;

            if (!ignoreShields)
            {
                ForeDamage = CenterSection.TakeShieldDamage(weapon, position, ForeDamage);
                PortDamage = CenterSection.TakeShieldDamage(weapon, position, PortDamage);
                StarboardDamage = CenterSection.TakeShieldDamage(weapon, position, StarboardDamage);
                AftDamage = CenterSection.TakeShieldDamage(weapon, position, AftDamage);
            }

            if(!ForeSection.isDestroyed())
            {
                ForeSection.TakeDamage(weapon, position, ForeDamage, ignoreShields, ignoreArmor, ignoreArmorRating);
            }
            else
            {
                CenterSection.TakeDamage(weapon, position, ForeDamage, ignoreShields, ignoreArmor, ignoreArmorRating);
            }

            if (!Destroyed)
            {
                if (!PortSection.isDestroyed())
                {
                    PortSection.TakeDamage(weapon, position, PortDamage, ignoreShields, ignoreArmor, ignoreArmorRating);
                }
                else
                {
                    CenterSection.TakeDamage(weapon, position, PortDamage, ignoreShields, ignoreArmor, ignoreArmorRating);
                }
            }

            if (!Destroyed)
            {
                if (!StarboardSection.isDestroyed())
                {
                    StarboardSection.TakeDamage(weapon, position, StarboardDamage, ignoreShields, ignoreArmor, ignoreArmorRating);
                }
                else
                {
                    CenterSection.TakeDamage(weapon, position, StarboardDamage, ignoreShields, ignoreArmor, ignoreArmorRating);
                }
            }

            if (!Destroyed)
            {
                if (!AftSection.isDestroyed())
                {
                    AftSection.TakeDamage(weapon, position, AftDamage, ignoreShields, ignoreArmor, ignoreArmorRating);
                }
                else
                {
                    CenterSection.TakeDamage(weapon, position, AftDamage, ignoreShields, ignoreArmor, ignoreArmorRating);
                }
            }
        }
        else
        {
            //Center shield take damage first
            if (!ignoreShields)
                damage = CenterSection.TakeShieldDamage(weapon, position, damage);

            if (quad == QuadrantTypes.Fore && !ForeSection.isDestroyed())
            {
                ForeSection.TakeDamage(weapon, position, damage, ignoreShields, ignoreArmor, ignoreArmorRating);
            }
            else if (quad == QuadrantTypes.Aft && !AftSection.isDestroyed())
            {
                AftSection.TakeDamage(weapon, position, damage, ignoreShields, ignoreArmor, ignoreArmorRating);
            }
            else if (quad == QuadrantTypes.Port && !PortSection.isDestroyed())
            {
                PortSection.TakeDamage(weapon, position, damage, ignoreShields, ignoreArmor, ignoreArmorRating);
            }
            else if (quad == QuadrantTypes.Starboard && !StarboardSection.isDestroyed())
            {
                StarboardSection.TakeDamage(weapon, position, damage, ignoreShields, ignoreArmor, ignoreArmorRating);
            }
            else
            {
                CenterSection.TakeDamage(weapon, position, damage, ignoreShields, ignoreArmor, ignoreArmorRating);
            }
        }
    }

    public override float GetDefenseBonus()
    {
        if(fleetData != null)
        {
			return Mathf.Min(0.99f, Mathf.Max(designData.DefenseBonus, fleetData.GetFleetDefenseBonus()) + GetLevelDefenseBonus());
        }
        return designData.DefenseBonus + GetLevelDefenseBonus();
    }

    float GetLevelDefenseBonus()
    {
        return Level * ResourceManager.instance.GetGameConstants().ShipLevelDefenseBonus;
    }

    public override float GetDamageBonus()
    {
        if (fleetData != null)
        {
            return Mathf.Max(designData.DamageBonus, fleetData.GetFleetDamageBonus()) + GetLevelDamageBonus();
        }
        return designData.DamageBonus + GetLevelDamageBonus();
    }

    float GetLevelDamageBonus()
    {
        return Level * ResourceManager.instance.GetGameConstants().ShipLevelDamageBonus;
    }

    public void QuadrantDestroyed(ShipQuadrant quad, AttachedWeapon weapon, float damage, bool ignoreShields, bool ignoreArmor, bool ignoreArmorRating)
    {
        if(quad == CenterSection)
        {
            if (Destroyed)
                return;
			weapon.GetKill (CurrentUnit);
            Destroyed = true;
			if (fleetData != null)
				fleetData.RecalculateStats ();

            if(CurrentUnit != null)
                (CurrentUnit as Ship).Die();
            CurrentUnit = null;
            return;
        }
        else if (CurrentUnit != null)
        {
            CurrentUnit.AttachFireDamage((CurrentUnit as Ship).GetDamageHardPoint(), designData.Hull.ExplosionScale);
            if(damage > 0)
                CenterSection.TakeDamage(weapon, CurrentUnit.transform.position, damage, ignoreShields, ignoreArmor, ignoreArmorRating);
        }

        CalculateEngines();
    }

    public void GetKill(UnitData unit)
    {
        Kills++;

        if(unit is ShipData)
            AddExperience((unit as ShipData).designData.Hull.BaseCommandPoints);
        else if(unit is StationData)
            AddExperience((unit as StationData).designData.Hull.ExperienceKillValue);
    }

    public void Pause(bool state)
    {
        CenterSection.Pause(state);
        ForeSection.Pause(state);
        AftSection.Pause(state);
        PortSection.Pause(state);
        StarboardSection.Pause(state);
    }

    public bool HasCenterShields()
    {
        return CenterSection.shieldHealth > 0;
    }

    public bool HasForeShields()
    {
        return ForeSection.shieldHealth > 0;
    }

    public bool HasAftShields()
    {
        return AftSection.shieldHealth > 0;
    }

    public bool HasPortShields()
    {
        return PortSection.shieldHealth > 0;
    }

    public bool HasStarboardShields()
    {
        return StarboardSection.shieldHealth > 0;
    }

	public bool HasShieldsDown()
	{
		if (HasCenterShields ())
			return false;

		if (!HasForeShields ())
			return true;
		else if (!HasAftShields ())
			return true;
		else if (!HasPortShields ())
			return true;
		else if (!HasStarboardShields ())
			return true;
		return false;
	}

    public override void RemoveFromFleet()
	{
		if (fleetData != null)
			fleetData.RemoveShip (this);
	}

    public override void DestroyAllBeams()
    {
        ForeSection.DestroyBeams();
        AftSection.DestroyBeams();
        PortSection.DestroyBeams();
        StarboardSection.DestroyBeams();
        CenterSection.DestroyBeams();
    }

    public override void DestroyWeaponEffects()
    {
        ForeSection.DestroyWeaponEffects();
        AftSection.DestroyWeaponEffects();
        PortSection.DestroyWeaponEffects();
        StarboardSection.DestroyWeaponEffects();
        CenterSection.DestroyWeaponEffects();
    }

    public float GetOuterHealthPercent()
    {
        float health = ForeSection.health + AftSection.health + PortSection.health + StarboardSection.health;
        float healthMax = ForeSection.healthMax + AftSection.healthMax + PortSection.healthMax + StarboardSection.healthMax;
        return 1f - (health / healthMax);
    }

    public override float GetMaxHealth()
    {
        return ForeSection.healthMax + AftSection.healthMax + PortSection.healthMax + StarboardSection.healthMax + CenterSection.healthMax;
    }

    public void SelfDestruct()
    {
        if(CurrentUnit != null)
        {
            (CurrentUnit as Ship).Die();
            CurrentUnit = null;
        }
    }

    public void AddExperience(float experienceValue)
    {
        Experience += experienceValue + experienceValue * GetExperienceBonus();

        //Check if enough experience to go to the next level
        ShipHullData hullData = designData.Design.GetHull();
        if (hullData != null)
        {
            if (Experience >= (Level + 1) * hullData.BaseCommandPoints)
            {
                Experience -= (Level + 1) * hullData.BaseCommandPoints;
                Level++;
            }
        }
    }

    protected override void SetCrewEffeciency()
    {
        crewEfficiency = Mathf.Min(1f, crew / designData.RequiredCrew);
        repair = designData.Repair * crewEfficiency;
    }

    public float ExperienceRatio()
    {
        ShipHullData hullData = designData.Design.GetHull();
        if (hullData != null)
        {
            return Experience / ((Level + 1) * hullData.BaseCommandPoints);
        }
        return 0;
    }

    public override int GetJammingCount()
    {
        return designData.JammingCount;
    }

    public override float GetJammingRange()
    {
        return designData.JammingRange;
    }

    public override float GetJammingRangeSqr()
    {
        return designData.JammingRange * designData.JammingRange;
    }

    public override float GetJammingDelay()
    {
        return designData.JammingDelay;
    }

    public override bool hasPointDefense()
    {
        foreach(AttachedWeapon weapon in ForeSection.Weapons)
        {
            if (weapon.baseWeapon.PointDefense)
                return true;
        }
        foreach (AttachedWeapon weapon in AftSection.Weapons)
        {
            if (weapon.baseWeapon.PointDefense)
                return true;
        }
        foreach (AttachedWeapon weapon in PortSection.Weapons)
        {
            if (weapon.baseWeapon.PointDefense)
                return true;
        }
        foreach (AttachedWeapon weapon in StarboardSection.Weapons)
        {
            if (weapon.baseWeapon.PointDefense)
                return true;
        }
        foreach (AttachedWeapon weapon in CenterSection.Weapons)
        {
            if (weapon.baseWeapon.PointDefense)
                return true;
        }
        return false;
    }

    float GetExperienceBonus()
    {
        return designData.ExperienceBonus;
    }

    public float GetAttackRangeDisplay()
    {
        return attackRange / ResourceManager.instance.GetGameConstants().FiringRangeFactor;
    }

    public float GetAttackRangeSqr()
    {
        return attackRange * attackRange;
    }

    public float GetAttackRange()
    {
        return attackRange;
    }

    public void SetAttackRange(float range)
    {
        attackRange = range;
    }

    public override Texture2D GetIcon()
    {
        if (designData.Hull.Icon != null)
            return designData.Hull.GetIcon();
        return ResourceManager.instance.GetErrorTexture();
    }

    protected override int GetMinCrew()
    {
        return designData.MinCrew;
    }

    void ParentAttachedWeapons(SpaceUnit parentUnit)
    {
        ForeSection.SetParentForAttachedWeapons(parentUnit);
        AftSection.SetParentForAttachedWeapons(parentUnit);
        PortSection.SetParentForAttachedWeapons(parentUnit);
        StarboardSection.SetParentForAttachedWeapons(parentUnit);
        CenterSection.SetParentForAttachedWeapons(parentUnit);
    }
}
