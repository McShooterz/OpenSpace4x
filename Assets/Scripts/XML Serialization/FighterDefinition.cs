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
    public string Name { get; set; }

    public string FighterObject { get; set; }

    public string Icon { get; set; }

    public int MaxSquadronSize { get; set; }

    //Stats
    public float Health { get; set; }
    public float ArmorHealth { get; set; }
    public float ArmorRating { get; set; }
    public float ShieldHealth { get; set; }
    public float ShieldRating { get; set; }
    public float ShieldRecharge { get; set; }
    public float ShieldDelay { get; set; }

    public bool AssaultPod { get; set; }
    public int Troops { get; set; }
    public int Crew { get; set; }

    public float EngineTrust { get; set; }
    public float EngineTurn { get; set; }
    public float ExperienceKillValue { get; set; }

    public List<string> WeaponNames { get; set; }

    public FighterDefinition()
    {
        Name = "";
        FighterObject = "";
        Icon = "";
        MaxSquadronSize = 6;
        Troops = 0;
        Crew = 1;

        WeaponNames = new List<string>();
    }

    public Sprite GetIcon()
    {
        return ResourceManager.instance.GetUnitIcon(Icon);
    }

    public GameObject GetFighterObject()
    {
        return ResourceManager.instance.GetFighterObject(FighterObject);
    }

    public float GetFirePower()
    {
        float firePower = 0;
        foreach (string weaponName in WeaponNames)
        {
            Weapon weapon = ResourceManager.instance.GetWeapon(weaponName);
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
