/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class GUIStatBar
{
    Rect baseRect;
    Rect ActiveRect;
    Rect BackRect;
    Texture2D BackGroundTexture;
    Texture2D ActiveTexture;


    public GUIStatBar(Rect rect, string backgroundTexture, string activeTexture)
    {
        baseRect = rect;
        ActiveRect = new Rect(Vector2.zero, baseRect.size);
        BackRect = new Rect(Vector2.zero, baseRect.size);
        BackGroundTexture = ResourceManager.GetUITexture(backgroundTexture);
        ActiveTexture = ResourceManager.GetUITexture(activeTexture);
    }

    public void Draw(float Ratio, string values)
    {
        GUI.DrawTexture(baseRect, BackGroundTexture);
        ActiveRect.width = baseRect.width * Ratio;
        GUI.BeginGroup(baseRect);
        GUI.BeginGroup(ActiveRect);
        GUI.DrawTexture(BackRect, ActiveTexture);
        GUI.EndGroup();
        GUI.EndGroup();
        GUI.Label(baseRect, values, GameManager.instance.StatBarStyle);
    }

    public bool Contains(Vector2 point)
    {
        return baseRect.Contains(point);
    }
}
