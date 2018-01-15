/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class DesignIconStatEntry
{
    Rect baseRect;
    GUIContent content;
    public delegate string GetValue();
    GetValue getValue;
    StatDrawType DrawType;
    string ToolTipTitle;
    string ToolTipBody;
    GUIToolTip ToolTip;

    public DesignIconStatEntry(Rect rect, string IconName, GetValue valueFunction, string toolTipTitle, string toolTipBody, GUIToolTip toolTip)
    {
        baseRect = rect;
        ToolTipTitle = toolTipTitle;
        ToolTipBody = toolTipBody;
        ToolTip = toolTip;
        getValue = valueFunction;
        content = new GUIContent();
        content.image = ResourceManager.instance.GetIconTexture(IconName);
    }

    public void Draw()
    {
        content.text = getValue();
        GUI.Label(baseRect, content, GameManager.instance.standardLabelStyle);
    }

    public bool CheckToolTip(Vector2 mousePosition)
    {
        if (baseRect.Contains(mousePosition))
        {
            ToolTip.SetText(ToolTipTitle, ToolTipBody);
            return true;
        }
        return false;
    }

    public enum StatDrawType
    {
        normal,
        PositiveOnly,
        percent
    }
}
