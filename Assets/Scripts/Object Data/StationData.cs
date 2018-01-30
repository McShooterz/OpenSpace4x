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

public sealed class StationData : UnitData
{
    //Naming
    public StationDesignData designData;

    //Defense Stats
    public float health;
    public float healthMax;
    public float armorHealth;
    public float armorHealthMax;
    public float armorRating;
    public float shieldHealth;
    public float shieldHealthMax;
    public float shieldRating;
    public float shieldRechargeRate;
    public float shieldRechargeDelay;

    float shieldTimer;

    public List<AttachedWeapon> Weapons = new List<AttachedWeapon>();

    public StationData(StationDesignData data)
    {
        designData = data;
        DisplayName = designData.Design.Name;

        //Add Fighters
        foreach (KeyValuePair<FighterDefinition, int> keyVal in designData.Fighters)
        {
            Fighters.Add(new FighterComplement(keyVal.Key, keyVal.Value));
        }
        foreach (KeyValuePair<FighterDefinition, int> keyVal in designData.HeavyFighters)
        {
            HeavyFighters.Add(new FighterComplement(keyVal.Key, keyVal.Value));
        }
        foreach (KeyValuePair<FighterDefinition, int> keyVal in designData.AssaultPods)
        {
            AssaultPods.Add(new FighterComplement(keyVal.Key, keyVal.Value));
        }

        ammo = designData.Ammo;
        ammoMax = designData.Ammo;
        power = designData.PowerStorage;
        powerMax = designData.PowerStorage;
        crew = designData.Crew;
        crewMax = designData.Crew;
        troops = designData.Troops;
        troopsMax = designData.Troops;
        powerGenerated = designData.PowerGenerated;
        research = designData.Research;
        mining = designData.Mining;
        repair = designData.Repair;
        ammoGenerated = designData.AmmoGenerated;
        transporter = designData.Transporter;
        medical = designData.Medical;
        boardingDefense = designData.BoardingDefense;
        diplomacy = designData.Diplomacy;
        sensor = designData.Sensor;
        longRangeSensor = designData.LongRangeSensor;
        advancedSensor = designData.AdvancedSensor;

        health = designData.Health;
        healthMax = designData.Health;
        armorHealth = designData.ArmorHealth;
        armorHealthMax = designData.ArmorHealth;
        armorRating = designData.ArmorRating;
        shieldHealth = designData.ShieldHealth;
        shieldHealthMax = designData.ShieldHealth;
        shieldRating = designData.ShieldRating;
        shieldRechargeRate = designData.ShieldRechargeRate;
        shieldRechargeDelay = designData.ShieldRechargeDelay;

        SetCrewEffeciency();

        foreach(SavedWeapon savedWeapon in designData.Weapons)
        {
            Weapons.Add(new AttachedWeapon(savedWeapon.weapon));
        }
    }

    public void Update()
    {
        if (Destroyed)
            return;
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
        if(health < healthMax)
        {
            health += repair * GetDeltaTime();
            if (health > healthMax)
                health = healthMax;
        }
        else if(armorHealth < armorHealthMax)
        {
            armorHealth += repair * GetDeltaTime();
            if (armorHealth > armorHealthMax)
                armorHealth = armorHealthMax;
        }
        if(shieldTimer > 0)
        {
            shieldTimer -= GetDeltaTime();
        }
        else if(shieldHealth < shieldHealthMax)
        {
            shieldHealth += shieldRechargeRate * GetDeltaTime();
            if (shieldHealth > shieldHealthMax)
                shieldHealth = shieldHealthMax;
        }
        foreach (AttachedWeapon weapon in Weapons)
        {
            weapon.Update();
        }

        UpdateBoarding();
    }

    public void AttachCurrentStation(Station station)
    {
        if(CurrentUnit != null)
        {
            UnityEngine.Object.Destroy(CurrentUnit.gameObject);
        }
        CurrentUnit = station;
        foreach(AttachedWeapon weapon in Weapons)
        {
            weapon.SetParent(CurrentUnit);
        }
    }

    public override float GetDamageBonus()
    {
        if (fleetData != null)
        {
            return Mathf.Max(designData.DamageBonus, fleetData.GetFleetDamageBonus());
        }
        return designData.DamageBonus;
    }

    public override float GetDefenseBonus()
    {
        if (fleetData != null)
        {
            return Mathf.Min(0.99f, Mathf.Max(designData.DefenseBonus, fleetData.GetFleetDefenseBonus()));
        }
        return designData.DefenseBonus;
    }

    public override void RemoveFromFleet()
    {
        if (fleetData != null)
            fleetData.RemoveStation(this);
    }

    public override void DestroyAllBeams()
    {
        foreach(AttachedWeapon weapon in Weapons)
        {
            weapon.DestroyBeam();
        }
    }

    public override void DestroyWeaponEffects()
    {
        foreach (AttachedWeapon weapon in Weapons)
        {
            weapon.DestroyAllProjectiles();
        }
    }

    public override float GetMaxHealth()
    {
        return designData.Health;
    }

    protected override void SetCrewEffeciency()
    {
        crewEfficiency = Mathf.Min(1f, crew / designData.RequiredCrew);
        repair = designData.Repair * crewEfficiency;
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
        foreach (AttachedWeapon weapon in Weapons)
        {
            if (weapon.baseWeapon.PointDefense)
                return true;
        }
        return false;
    }

    public void GetKill(UnitData killedUnit)
    {
        Kills++;
    }

    public void TakeDamage(AttachedWeapon weapon, Vector3 position, float damage, bool ignoreShields, bool ignoreArmor, bool ignoreArmorRating)
    {
        //Apply defense bonus
        damage -= damage * GetDefenseBonus();

        //Record damage taken
        damageTaken += damage;

        if (CurrentUnit == null)
            return;

        if (weapon.baseWeapon.PowerDamageModifier != 0)
        {
            power -= damage * weapon.baseWeapon.PowerDamageModifier;
            if (power < 0)
                power = 0;
        }

        //shield absord damage
        if (!ignoreShields && damage > 0 && shieldHealth > 0 && shieldRating > 0)
        {
            float effectiveDamage;

            if (weapon.baseWeapon.ShieldDamageModifier != 1f)
            {
                effectiveDamage = damage * weapon.baseWeapon.ShieldDamageModifier;
            }
            else
            {
                effectiveDamage = damage;
            }

            if (effectiveDamage > 0)
            {
                float absorbDamage = effectiveDamage * Mathf.Clamp(shieldRating / effectiveDamage, ResourceManager.instance.GetGameConstants().MinShieldAbsorb, 1f);
                float damageUseRatio;

                if (shieldHealth > absorbDamage)
                {
                    if (GameManager.instance.GetShowCombatDamage())
                    {
                        CreateShieldDamagePopup(position, absorbDamage);
                    }
                    shieldHealth -= absorbDamage;
                    damageUseRatio = 1f - absorbDamage / effectiveDamage;
                }
                else
                {
                    if (GameManager.instance.GetShowCombatDamage())
                    {
                        CreateShieldDamagePopup(position, shieldHealth);
                    }
                    damageUseRatio = 1f - shieldHealth / effectiveDamage;
                    shieldHealth = 0;
                    shieldTimer = shieldRechargeDelay;
                }
                damage *= damageUseRatio;
            }
            else
            {
                return;
            }
        }

        //Armor takes reduced damage based on armor rating
        if (!ignoreArmor && damage > 0 && armorHealth > 0)
        {
            float effectiveDamage;

            if (weapon.baseWeapon.ArmorDamageModifier != 1f)
            {
                effectiveDamage = damage * weapon.baseWeapon.ArmorDamageModifier;
            }
            else
            {
                effectiveDamage = damage;
            }

            if (effectiveDamage > 0)
            {
                if (!ignoreArmorRating && armorRating > 0)
                {
                    float damageCoeff = Mathf.Min(0.99f, Mathf.Pow(effectiveDamage / armorRating, 0.366f) * 0.5f);
                    //float damageReduction = 1f - damageCoeff;
                    effectiveDamage *= damageCoeff;
                }

                if (armorHealth > effectiveDamage)
                {
                    if (GameManager.instance.GetShowCombatDamage())
                    {
                        CreateArmorDamagePopup(position, effectiveDamage);
                    }
                    armorHealth -= effectiveDamage;
                    return;
                }
                else
                {
                    if (GameManager.instance.GetShowCombatDamage())
                    {
                        CreateArmorDamagePopup(position, armorHealth);
                    }
                    float ratio = 1f - armorHealth / effectiveDamage;
                    damage *= ratio;
                    armorHealth = 0;

                }
            }
            else
            {
                return;
            }
        }

        //Apply remaining damage to health
        if (damage > 0)
        {
            if (weapon.baseWeapon.HealthDamageModifier != 1f)
            {
                damage *= weapon.baseWeapon.HealthDamageModifier;
            }

            if (damage > 0)
            {
                DamageCrew(damage);
                if (GameManager.instance.GetShowCombatDamage())
                {
                    CreateHealthDamagePopup(position, damage);
                }
                health -= damage;
                if (health < 0)
                {
                    health = 0;
                    weapon.GetKill(CurrentUnit);
                    (CurrentUnit as Station).Die();
                }
            }
        }
    }

    public void Pause(bool state)
    {
        foreach (AttachedWeapon weapon in Weapons)
        {
            weapon.Pause(state);
        }
    }

    public void SelfDestruct()
    {
        if (CurrentUnit != null)
        {
            (CurrentUnit as Station).Die();
            CurrentUnit = null;
        }
    }

    public float GetHealthPercentOverlay()
    {
        return 1f - health / healthMax;
    }

    public override Sprite GetIcon()
    {
        if (designData.Hull.Icon != null)
            return designData.Hull.GetIcon();
        return ResourceManager.instance.GetErrorTexture();
    }

    protected override int GetMinCrew()
    {
        return designData.MinCrew;
    }
}
