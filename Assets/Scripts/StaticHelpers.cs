/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using System.Text;
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

    public static string DecimalToRoman(int number)
    {
        StringBuilder stringBuidler = new StringBuilder();

        if ((number < 0) || (number > 3999))
        {
            return "Error: Value out of range.";
        }

        while (number > 0)
        {
            if (number >= 1000)
            {
                stringBuidler.Append("M");
                number -= 1000;
            }
            if (number >= 900)
            {
                stringBuidler.Append("CM");
                number -= 900;
            }
            if (number >= 500)
            {
                stringBuidler.Append("D");
                number -= 500;
            }
            if (number >= 400)
            {
                stringBuidler.Append("CD");
                number -= 400;
            }
            if (number >= 100)
            {
                stringBuidler.Append("C");
                number -= 100;
            }
            if (number >= 90)
            {
                stringBuidler.Append("XC");
                number -= 90;
            }
            if (number >= 50)
            {
                stringBuidler.Append("L");
                number -= 50;
            }
            if (number >= 40)
            {
                stringBuidler.Append("XL");
                number -= 40;
            }
            if (number >= 10)
            {
                stringBuidler.Append("X");
                number -= 10;
            }
            if (number >= 9)
            {
                stringBuidler.Append("IX");
                number -= 9;
            }
            if (number >= 5)
            {
                stringBuidler.Append("V");
                number -= 5;
            }
            if (number >= 4)
            {
                stringBuidler.Append("IV");
                number -= 4;
            }
            if (number >= 1)
            {
                stringBuidler.Append("I");
                number -= 1;
            }
        }

        return stringBuidler.ToString();
    }
}
