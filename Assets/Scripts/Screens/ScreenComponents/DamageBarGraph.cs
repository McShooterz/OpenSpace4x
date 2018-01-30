/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections.Generic;

public class DamageBarGraph
{
    Rect baseRect;
    float sliderRange;
    Rect[] DamageBars = new Rect[20];
    Texture2D barTexture;
    float maxRange;

    public DamageBarGraph(Rect rect)
    {
        baseRect = rect;
        //barTexture = ResourceManager.instance.GetUITexture("DamageBar");

        float increment = baseRect.width / 20;
        for (int i = 0; i < 20; i++)
        {
            DamageBars[i] = new Rect(baseRect.x + increment * i, baseRect.y, increment, baseRect.height);
        }
    }

    public void SetDamageBars(float newRange,List<Weapon> weapons)
    {
        foreach (Weapon weapon in weapons)
        {
            float weaponRange = weapon.GetMaxRange();
            if (weaponRange > maxRange)
                maxRange = weaponRange;
        }

        float[] DamageValues = new float[20];
        float rangeInterval = maxRange / 20f;
        float maxDamage = 0;

        for (int i = 0; i < 20; i++)
        {
            float range = i * rangeInterval;
            float damageSum = 0;
            foreach (Weapon weapon in weapons)
            {
                damageSum += weapon.GetBarGraphDamage(range);
            }
            if (damageSum > maxDamage)
            {
                maxDamage = damageSum;
            }
            DamageValues[i] = damageSum;
        }

        for (int i = 0; i < 20; i++)
        {
            if (maxDamage > 0)
            {
                DamageBars[i].height = baseRect.height * (DamageValues[i] / maxDamage);
                DamageBars[i].y = baseRect.y + (baseRect.height - DamageBars[i].height) / 2f;
            }
            else
            {
                DamageBars[i].height = 0;
            }
        }

        if(newRange != -1)
        {
            sliderRange = newRange;
        }
        else
        {
            sliderRange = maxRange;
        }
    }

    public float Draw()
    {
        GUI.Box(baseRect, "");
        foreach (Rect bar in DamageBars)
        {
            GUI.DrawTexture(bar, barTexture);
        }
        sliderRange = GUI.HorizontalSlider(baseRect, sliderRange, 0f, maxRange);
        return sliderRange;
    }

    public bool Contains(Vector2 position)
    {
        return baseRect.Contains(position);
    }
}
