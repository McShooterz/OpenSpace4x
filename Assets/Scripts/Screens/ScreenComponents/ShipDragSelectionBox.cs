/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class ShipDragSelectionBox
{
    Texture2D texture;
    Vector2 StartPoint = Vector2.zero;
    Vector2 EndPoint = Vector2.zero;

    bool Active = false;

    public ShipDragSelectionBox()
    {
        //texture = ResourceManager.instance.GetUITexture("SelectionBox");
    }

    public void Draw()
    {
        int X = (int)Mathf.Min(StartPoint.x, EndPoint.x);
        int Y = (int)Mathf.Min(Screen.height - StartPoint.y, Screen.height - EndPoint.y);
        int Height = (int)Mathf.Abs(StartPoint.y - EndPoint.y);
        int Width = (int)Mathf.Abs(StartPoint.x - EndPoint.x);
        Rect selectionRect = new Rect(X, Y, Width, Height);

        GUI.DrawTexture(selectionRect, texture);
        GUI.DrawTexture(new Rect(selectionRect.x, selectionRect.y, selectionRect.width, 3f), texture);
        GUI.DrawTexture(new Rect(selectionRect.x, selectionRect.y, 3f, selectionRect.height), texture);
        GUI.DrawTexture(new Rect(selectionRect.xMax - 3f, selectionRect.y, 3f, selectionRect.height), texture);
        GUI.DrawTexture(new Rect(selectionRect.x, selectionRect.yMax - 3f, selectionRect.width, 3f), texture);
    }

    public void SetActive(bool state)
    {
        Active = state;
    }

    public bool isActive()
    {
        return Active;
    }

    public void SetStart(Vector2 start)
    {
        StartPoint = start;
    }

    public void SetEnd(Vector2 end)
    {
        EndPoint = end;
    }

    public void SelectShips(ShipManager manager)
    {
        manager.SelectShipsInArea(StartPoint, EndPoint);
    }
}
