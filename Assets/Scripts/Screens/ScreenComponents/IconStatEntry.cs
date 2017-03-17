/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class IconStatEntry
{
    Rect baseRect;
    GUIContent content;
    string ToolTipTitle;
    string ToolTipBody;
    GUIToolTip ToolTip;

    public IconStatEntry(Rect rectangle, string iconTexture, string value, string toolTipTitle, string toolTipBody, GUIToolTip toolTip)
    {
        baseRect = rectangle;
        content = new GUIContent();
        content.image = ResourceManager.GetIconTexture(iconTexture);
        content.text = value;
        ToolTipTitle = toolTipTitle;
        ToolTipBody = toolTipBody;
        ToolTip = toolTip;
    }

    public void Draw()
    {
        GUI.Label(baseRect, content, GameManager.instance.standardLabelStyle);
    }

    public bool CheckForToolTip(Vector2 mousePosition)
    {
        if (baseRect.Contains(mousePosition))
        {
            ToolTip.SetText(ToolTipTitle, ToolTipBody);
            return true;
        }
        return false;
    }


    public void DrawOffset(Vector2 newPosition)
    {
        GUI.Label(new Rect(newPosition, baseRect.size), content, GameManager.instance.standardLabelStyle);
    }
}
