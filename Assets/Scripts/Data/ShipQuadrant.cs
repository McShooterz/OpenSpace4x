/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes: Each ship is composed of 5 of these
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipQuadrant
{
    ShipData parentShip;
    bool Destroyed = false;

	public List<AttachedWeapon> Weapons = new List<AttachedWeapon>();

    //Variables
    public float health;
    public float healthMax;
    public float armorHealth;
    public float armorHealthMax;
    public float armorRating;
    public float shieldHealth;
    public float shieldHealthMax;
    public float shieldRating;
    public float shieldRecharge;
    public float shieldDelay;
    public float engineThrust;
    public float engineTurn;

    private float shieldTimer = 0;

    // Use this for initialization
    public ShipQuadrant(ShipData SD, ShipDesignQuadData quadData, QuadrantTypes quadType)
    {
        parentShip = SD;

        foreach(SavedWeapon weapon in quadData.Weapons)
        {
            QuadrantTypes firingDirection = quadType;

            if(weapon.weapon.AlwaysForward)
            {
                if (weapon.rotation == 0f)
                {
                    firingDirection = QuadrantTypes.Fore;
                }
                else if (weapon.rotation == 90f)
                {
                    firingDirection = QuadrantTypes.Starboard;
                }
                else if (weapon.rotation == 180f)
                {
                    firingDirection = QuadrantTypes.Aft;
                }
                else
                {
                    firingDirection = QuadrantTypes.Port;
                }
            }
            AttachedWeapon attachedWeapon = new AttachedWeapon(weapon.weapon);
            attachedWeapon.SetFiringQuadrant(firingDirection);
            Weapons.Add(attachedWeapon);
        }

        health = quadData.Health;
        healthMax = quadData.Health;
        armorHealth = quadData.ArmorHealth;
        armorHealthMax = quadData.ArmorHealth;
        armorRating = quadData.ArmorRating;
        shieldHealth = quadData.ShieldHealth;
        shieldHealthMax = quadData.ShieldHealth;
        shieldRating = quadData.ShieldRating;
        shieldRecharge = quadData.ShieldRechargeRate;
        shieldDelay = quadData.ShieldRechargeDelay;
        engineThrust = quadData.EngineThrust;
        engineTurn = quadData.EngineTurn;
    }
	
	public void Update ()
    {
        if (Destroyed)
            return;

        if(shieldTimer > 0)
        {
            shieldTimer -= GetDeltaTime();
        }
        else
        {
            ChargeShields();
        }
        if(ResourceManager.instance.GetGameConstants().AllowCombatRepair)
        {
            CombatRepair();
        }
	
        foreach(AttachedWeapon weapon in Weapons)
        {
            weapon.Update();
        }
	}

    public ShipData GetParent()
    {
        return parentShip;
    }

    public bool isDestroyed()
    {
        return Destroyed;
    }

    public float TakeShieldDamage(AttachedWeapon weapon, Vector3 position, float damage)
    {
        if (damage > 0 && shieldHealth > 0 && shieldRating > 0)
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
                        parentShip.CreateShieldDamagePopup(position, absorbDamage);
                    }
                    shieldHealth -= absorbDamage;
                    damageUseRatio = 1f - absorbDamage / effectiveDamage;
                }
                else
                {
                    if (GameManager.instance.GetShowCombatDamage())
                    {
                        parentShip.CreateShieldDamagePopup(position, shieldHealth);
                    }
                    damageUseRatio = 1f - shieldHealth / effectiveDamage;
                    shieldHealth = 0;
                    shieldTimer = shieldDelay;
                }
                return damage * damageUseRatio;
            }
            else
            {
                return 0f;
            }
        }
        else
        {
            return damage;
        }
    }

    public void TakeDamage(AttachedWeapon weapon, Vector3 position, float damage, bool ignoreShields, bool ignoreArmor, bool ignoreArmorRating)
    {           
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
                        parentShip.CreateShieldDamagePopup(position, absorbDamage);
                    }
                    shieldHealth -= absorbDamage;
                    damageUseRatio = 1f - absorbDamage / effectiveDamage;
                }
                else
                {
                    if (GameManager.instance.GetShowCombatDamage())
                    {
                        parentShip.CreateShieldDamagePopup(position, shieldHealth);
                    }
                    damageUseRatio = 1f - shieldHealth / effectiveDamage;
                    shieldHealth = 0;
                    shieldTimer = shieldDelay;
                }
                damage *= damageUseRatio;
            }
            else
            {
                return;
            }
        }

        //Armor takes reduced damage based on armor rating
        if(!ignoreArmor && damage > 0 && armorHealth > 0)
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
                    effectiveDamage *= damageCoeff;
                }

                if (armorHealth > effectiveDamage)
                {
                    if (GameManager.instance.GetShowCombatDamage())
                    {
                        parentShip.CreateArmorDamagePopup(position, effectiveDamage);
                    }
                    armorHealth -= effectiveDamage;
                    return;
                }
                else
                {
                    if (GameManager.instance.GetShowCombatDamage())
                    {
                        parentShip.CreateArmorDamagePopup(position, armorHealth);
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
        if(damage > 0)
        {
            if(weapon.baseWeapon.HealthDamageModifier != 1f)
            {
                damage *= weapon.baseWeapon.HealthDamageModifier;
            }

            if (damage > 0)
            {
                parentShip.DamageCrew(damage);
                if (GameManager.instance.GetShowCombatDamage())
                {
                    parentShip.CreateHealthDamagePopup(position, damage);
                }
                health -= damage;
                if (health < 0)
                {
                    health = 0;
                    Die(weapon, damage, ignoreShields, ignoreArmor, ignoreArmorRating);
                }
            }
        }
    }

    void Die(AttachedWeapon weapon, float damage, bool ignoreShields, bool ignoreArmor, bool ignoreArmorRating)
    {
        Destroyed = true;
        parentShip.QuadrantDestroyed(this, weapon, damage, ignoreShields, ignoreArmor, ignoreArmorRating);
    }

    void ChargeShields()
    {
        if (shieldHealth < shieldHealthMax)
        {
            shieldHealth += shieldRecharge * GetDeltaTime();
            if(shieldHealth > shieldHealthMax)
            {
                shieldHealth = shieldHealthMax;
            }
        } 
    }

    void CombatRepair()
    {
        if(health < healthMax)
        {
            health += parentShip.repair * Time.deltaTime;
            if(health > healthMax)
            {
                health = healthMax;
            }
        }
        else if(armorHealth < armorHealthMax)
        {
            armorHealth += parentShip.repair * Time.deltaTime;
            if(armorHealth > armorHealthMax)
            {
                armorHealth = armorHealthMax;
            }
        }
    }

    public void DestroyBeams()
    {
        foreach(AttachedWeapon weapon in Weapons)
        {
            weapon.DestroyBeam();
        }
    }

    public void DestroyWeaponEffects()
    {
        foreach(AttachedWeapon weapon in Weapons)
        {
            weapon.DestroyBeam();
            weapon.DestroyAllProjectiles();
        }
    }

    public void Pause(bool state)
    {
        foreach(AttachedWeapon weapon in Weapons)
        {
			weapon.Pause (state);
        }
    }

    float GetDeltaTime()
    {
        return GameManager.instance.GetDeltaTime();
    }

    public void SetParentForAttachedWeapons(SpaceUnit parentUnit)
    {
        foreach(AttachedWeapon weapon in Weapons)
        {
            weapon.SetParent(parentUnit);
        }
    }
}
