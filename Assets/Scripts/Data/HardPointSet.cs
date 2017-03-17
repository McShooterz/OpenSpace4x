/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections.Generic;

public class HardPointSet : MonoBehaviour
{
    public WeaponCategory Category = WeaponCategory.Universal;
    public List<GameObject> HardPoints;

    public void DestroyChildren()
    {
        foreach (GameObject hardPoint in HardPoints)
        {
            if(hardPoint != null)
                Destroy(hardPoint);
        }
    }
}
