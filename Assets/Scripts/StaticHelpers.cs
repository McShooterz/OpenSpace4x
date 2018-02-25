/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class StaticHelpers
{




    public static Vector2 GetRandomDirection()
    {
        float azimuth = Random.Range(0, 2) * Mathf.PI;

        return new Vector2(Mathf.Cos(azimuth), Mathf.Sin(azimuth));
    }
}
