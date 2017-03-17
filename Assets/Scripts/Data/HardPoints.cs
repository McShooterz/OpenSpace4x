/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections.Generic;

public class HardPoints : MonoBehaviour
{
    public List<HardPointSet> foreWeaponHardPoints;
    public List<HardPointSet> aftWeaponHardPoints;
    public List<HardPointSet> portWeaponHardPoints;
    public List<HardPointSet> starboardWeaponHardPoints;
    public List<GameObject> damageHardPoints = new List<GameObject>();

    public void DestroyAllObjects()
    {
        foreach(HardPointSet set in foreWeaponHardPoints)
        {
            set.DestroyChildren();
            Destroy(set.gameObject);
        }

        foreach (HardPointSet set in aftWeaponHardPoints)
        {
            set.DestroyChildren();
            Destroy(set.gameObject);
        }

        foreach (HardPointSet set in portWeaponHardPoints)
        {
            set.DestroyChildren();
            Destroy(set.gameObject);
        }

        foreach (HardPointSet set in starboardWeaponHardPoints)
        {
            set.DestroyChildren();
            Destroy(set.gameObject);
        }

        foreach (GameObject point in damageHardPoints)
        {
            Destroy(point);
        }
    }
}
