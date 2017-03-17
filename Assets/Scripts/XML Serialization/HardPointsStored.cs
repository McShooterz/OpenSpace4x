/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class HardPointsStored
{
    public HardPointSetStored[] ForeHardPointSets;
    public HardPointSetStored[] AftHardPointSets;
    public HardPointSetStored[] PortHardPointSets;
    public HardPointSetStored[] StarboardHardPointSets;
    public Vector3[] DamagePoints;

    public HardPointsStored() { }

    public HardPointsStored(HardPoints hardPoints)
    {
        ForeHardPointSets = new HardPointSetStored[hardPoints.foreWeaponHardPoints.Count];

        for(int i = 0; i < ForeHardPointSets.Length; i++)
        {
            ForeHardPointSets[i] = new HardPointSetStored(hardPoints.foreWeaponHardPoints[i]);
        }

        AftHardPointSets = new HardPointSetStored[hardPoints.aftWeaponHardPoints.Count];

        for (int i = 0; i < AftHardPointSets.Length; i++)
        {
            AftHardPointSets[i] = new HardPointSetStored(hardPoints.aftWeaponHardPoints[i]);
        }

        PortHardPointSets = new HardPointSetStored[hardPoints.portWeaponHardPoints.Count];

        for (int i = 0; i < PortHardPointSets.Length; i++)
        {
            PortHardPointSets[i] = new HardPointSetStored(hardPoints.portWeaponHardPoints[i]);
        }

        StarboardHardPointSets = new HardPointSetStored[hardPoints.starboardWeaponHardPoints.Count];

        for (int i = 0; i < StarboardHardPointSets.Length; i++)
        {
            StarboardHardPointSets[i] = new HardPointSetStored(hardPoints.starboardWeaponHardPoints[i]);
        }

        DamagePoints = new Vector3[hardPoints.damageHardPoints.Count];

        for(int i = 0; i < DamagePoints.Length; i++)
        {
            DamagePoints[i] = hardPoints.damageHardPoints[i].transform.localPosition;
        }
    }

    public Vector3[] GetRandomHardpoints(Weapon weapon, QuadrantTypes quad)
    {
        if (quad == QuadrantTypes.Fore && ForeHardPointSets.Length > 0)
        {
            return GetRandomHardpointsFromSet(weapon, ForeHardPointSets);
        }
        else if (quad == QuadrantTypes.Aft && AftHardPointSets.Length > 0)
        {
            return GetRandomHardpointsFromSet(weapon, AftHardPointSets);
        }
        else if (quad == QuadrantTypes.Port && PortHardPointSets.Length > 0)
        {
            return GetRandomHardpointsFromSet(weapon, PortHardPointSets);
        }
        else if (quad == QuadrantTypes.Starboard && StarboardHardPointSets.Length > 0)
        {
            return GetRandomHardpointsFromSet(weapon, StarboardHardPointSets);
        }
        else
        {
            return GetZeroHardPoints();
        }
    }

    Vector3[] GetRandomHardpointsFromSet(Weapon weapon, HardPointSetStored[] hardPointSet)
    {
        int count;
        if (weapon.isBeam || !weapon.MultiHardpoint)
        {
            count = 1;
        }
        else
        {
            count = weapon.Projectiles;
        }

        //Check for right hardpoint set type
        foreach (HardPointSetStored set in hardPointSet)
        {
            if (set.Category == weapon.Category)
            {
                return set.GetRandomHardPoint(count);
            }
        }

        //Heavy cannon parent to cannon
        if (weapon.Category == WeaponCategory.HeavyCannon)
        {
            foreach (HardPointSetStored set in hardPointSet)
            {
                if (set.Category == WeaponCategory.Cannon)
                {
                    return set.GetRandomHardPoint(count);
                }
            }
        }

        //Heavy Torpedo parent to Torpedo
        if (weapon.Category == WeaponCategory.HeavyTorpedo)
        {
            foreach (HardPointSetStored set in hardPointSet)
            {
                if (set.Category == WeaponCategory.Torpedo)
                {
                    return set.GetRandomHardPoint(count);
                }
            }
        }

        //Catch for universal as last resort
        foreach (HardPointSetStored set in hardPointSet)
        {
            if (set.Category == WeaponCategory.Universal)
            {
                return set.GetRandomHardPoint(count);
            }
        }

        return GetZeroHardPoints();
    }

    public Vector3 GetRandomDamageOffset()
    {
        if (DamagePoints.Length > 0)
            return DamagePoints[Random.Range(0, DamagePoints.Length)];
        return Vector3.zero;
    }

    Vector3[] GetZeroHardPoints()
    {
        Vector3[] hardPoints = new Vector3[1];
        hardPoints[0] = Vector3.zero;
        return hardPoints;
    }

    public class HardPointSetStored
    {
        public WeaponCategory Category = WeaponCategory.Universal;
        public Vector3[] HardPoints;

        public HardPointSetStored() { }

        public HardPointSetStored(HardPointSet hardPointSet)
        {
            Category = hardPointSet.Category;
            HardPoints = new Vector3[hardPointSet.HardPoints.Count];

            for(int i = 0; i < HardPoints.Length; i++)
            {
                HardPoints[i] = hardPointSet.HardPoints[i].transform.localPosition;
            }
        }

        public Vector3[] GetRandomHardPoint(int count)
        {
            if (HardPoints.Length > 0 && count > 0)
            {
                if (count > HardPoints.Length)
                    count = HardPoints.Length;
                Vector3[] points = new Vector3[count];

                int index = Random.Range(0, HardPoints.Length);
                points[0] = HardPoints[index];               
                for(int i = 1; i < count; i++)
                {
                    index = (index + 1) % count;
                    points[i] = HardPoints[index];
                }
                return points;
            }
            else
            {
                Vector3[] points = new Vector3[1];
                points[0] = Vector3.zero;
                return points;
            }
        }
    }
}
