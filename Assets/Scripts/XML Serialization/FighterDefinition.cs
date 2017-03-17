/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections.Generic;

public class FighterDefinition
{
    public string Name;

    public string FighterObject;

    public string Icon;

    public int MaxSquadronSize = 6;

    //Stats
    public float Health;
    public float ArmorHealth;
    public float ArmorRating;
    public float ShieldHealth;
    public float ShieldRating;
    public float ShieldRecharge;
    public float ShieldDelay;

    public bool AssaultPod;
    public int Troops = 0;
    public int Crew = 1;

    public float EngineTrust;
    public float EngineTurn;
    public float ExperienceKillValue;

    public List<string> WeaponNames = new List<string>();

    public FighterDefinition() { }

    public Texture2D GetIcon()
    {
        return ResourceManager.GetUnitIcon(Icon);
    }

    public GameObject GetFighterObject()
    {
        return ResourceManager.GetFighterObject(FighterObject);
    }

    public float GetFirePower()
    {
        float firePower = 0;
        foreach (string weaponName in WeaponNames)
        {
            Weapon weapon = ResourceManager.GetWeapon(weaponName);
            if (weapon != null)
            {
                firePower += weapon.GetAverageDPS();
            }
        }
        return firePower;
    }

    public float GetSquadronFirePower()
    {
        return GetFirePower() * MaxSquadronSize;
    }
}
