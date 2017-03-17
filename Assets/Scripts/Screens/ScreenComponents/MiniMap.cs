/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class MiniMap
{
    Rect mainRect;
    RenderTexture Texture;
    float Ratio;
    GUIToolTip ToolTip;
	
    public MiniMap(Rect rect, RenderTexture renderTexture, GUIToolTip toolTip)
    {
        mainRect = rect;
        Texture = renderTexture;
        ToolTip = toolTip;
        Ratio = 400f / mainRect.width;
    }

    public void Draw()
    {
        GUI.DrawTexture(mainRect, Texture);
    }

    public bool Contains(Vector2 point)
    {
        return mainRect.Contains(point);
    }

    public Vector3 GetWorldPosition(Vector2 point)
    {
        float MiniMapHalf = mainRect.width / 2;
        point -= mainRect.position;
        Vector3 newPosition = new Vector3(0, 0, 0);

        if(point.x > MiniMapHalf)
        {
            newPosition.x = (point.x - MiniMapHalf) * Ratio;
        }
        else
        {
            newPosition.x = (MiniMapHalf - point.x) * -Ratio;
        }

        if(point.y > MiniMapHalf)
        {
            newPosition.z = (point.y - MiniMapHalf) * -Ratio;
        }
        else
        {
            newPosition.z = (MiniMapHalf - point.y) * Ratio;
        }

        return newPosition;
    }

    public bool CheckToolTip(Vector2 point)
    {
        if(mainRect.Contains(point))
        {
            ToolTip.SetText("miniMap", "miniMapDesc");
            return true;
        }
        return false;
    }
}
