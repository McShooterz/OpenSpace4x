/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class CombatTimer
{
    Rect baseRect;
    Rect timeRect;
    float Time = 0;
    GUIToolTip ToolTip;

    public CombatTimer(Rect rect, GUIToolTip toolTip)
    {
        baseRect = rect;
        ToolTip = toolTip;
        timeRect = new Rect(baseRect.x + baseRect.width * 0.05f, baseRect.y + baseRect.height * 0.05f, baseRect.width * 0.9f, baseRect.height * 0.9f);
    }

    public void Draw()
    {
        GUI.Box(baseRect, "", GameManager.instance.standardBackGround);
        GUI.Label(timeRect, ScreenParent.GetFormatedTime(Time));
    }

    public void CountUp()
    {
        Time += GameManager.instance.GetDeltaTime();
    }

    public bool CountDown()
    {
        Time -= GameManager.instance.GetDeltaTime();
        if (Time > 0)
            return false;
        return true;
    }

    public void SetTime(float time)
    {
        Time = time;
    }

    public float GetTime()
    {
        return Time;
    }

    public bool Contains(Vector2 point)
    {
        return baseRect.Contains(point);
    }

    public bool CheckToolTip(Vector2 point)
    {
        if(baseRect.Contains(point))
        {
            ToolTip.SetText("combatTimer", "combatTimerDesc");
            return true;
        }
        return false;
    }
}
