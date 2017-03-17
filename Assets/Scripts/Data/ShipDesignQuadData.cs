/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections.Generic;

public class ShipDesignQuadData
{
    #region Variables
    //Defense
    private float health;
    private float armorHealth;
    private float armorRating;
    private float shieldHealth;
    private float shieldRating;
    private float shieldRechargeRate;
    private float shieldRechargeDelay;
    //Engines
    private float engineThrust;
    private float engineTurn;

    private List<SavedWeapon> weapons = new List<SavedWeapon>();
    #endregion
    #region Properties
    public float Health
    {
        get
        {
            return health;
        }

        set
        {
            health = value;
        }
    }

    public float ArmorHealth
    {
        get
        {
            return armorHealth;
        }

        set
        {
            armorHealth = value;
        }
    }

    public float ArmorRating
    {
        get
        {
            return armorRating;
        }

        set
        {
            armorRating = value;
        }
    }

    public float ShieldHealth
    {
        get
        {
            return shieldHealth;
        }

        set
        {
            shieldHealth = value;
        }
    }

    public float ShieldRating
    {
        get
        {
            return shieldRating;
        }

        set
        {
            shieldRating = value;
        }
    }

    public float ShieldRechargeRate
    {
        get
        {
            return shieldRechargeRate;
        }

        set
        {
            shieldRechargeRate = value;
        }
    }

    public float ShieldRechargeDelay
    {
        get
        {
            return shieldRechargeDelay;
        }

        set
        {
            shieldRechargeDelay = value;
        }
    }

    public float EngineThrust
    {
        get
        {
            return engineThrust;
        }

        set
        {
            engineThrust = value;
        }
    }

    public float EngineTurn
    {
        get
        {
            return engineTurn;
        }

        set
        {
            engineTurn = value;
        }
    }

    public List<SavedWeapon> Weapons
    {
        get
        {
            return weapons;
        }

        set
        {
            weapons = value;
        }
    }
    #endregion

    public void AddWeapon(float rotation, Weapon weapon)
    {
        weapons.Add(new SavedWeapon(rotation, weapon));
    }
}
