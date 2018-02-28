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
    public static Vector2 GetUnitVector2(float angleTheta)
    {
        return new Vector2(Mathf.Sin(angleTheta), Mathf.Cos(angleTheta));
    }

    public static Vector2 GetRandomUnitVector2()
    {
        return GetUnitVector2(Random.Range(0, 2 * Mathf.PI));
    }

    public static int GetRandomIndexByWeight(float[] weights)
    {
        float weightSum = 0;

        foreach(float weight in weights)
        {
            weightSum += weight;
        }

        float randomValue = Random.Range(0f, weightSum);

        for(int i = 0; i < weights.Length; i++)
        {
            randomValue -= weights[i];

            if (randomValue <= 0)
            {
                return i;
            }
        }

        // Return -1 for error
        return -1;
    }
}
