/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class DoubleClickDetector
{
    int Clicks = 0;
    float Timer = 0f;
    float TimeWindow;

    public DoubleClickDetector(float timeWindow)
    {
        TimeWindow = timeWindow;
    }

    public void Update()
    {
        if(Timer > 0)
        {
            Timer -= Time.deltaTime;
            if (Input.GetMouseButtonDown(0))
            {
                Clicks++;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                Timer = TimeWindow;
                Clicks = 1;
            }
            else
            {
                Clicks = 0;
            }
        }
    }

    public int GetClicks()
    {
        return Clicks;
    }
}
