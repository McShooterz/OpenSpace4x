/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System;

public class GUILine
{
    Texture2D lineTex = new Texture2D(1, 1);
    Vector2 PointA;
    float Angle;
    Rect rect;
    Color color;

    public GUILine(Vector2 pointA, Vector2 pointB, Color color)
    {
        PointA = pointA;
        this.color = color;
        Angle = Vector2.Angle(pointB - pointA, Vector2.right);
        // If pointB is above pointA, then angle needs to be negative.
        if (PointA.y > pointB.y)
        {
            Angle = -Angle;
        }
        rect = new Rect(PointA.x, PointA.y, (pointB - PointA).magnitude, 2);
    }

    public void Draw()
    {
        // Save the current GUI matrix, since we're going to make changes to it.
        Matrix4x4 matrix = GUI.matrix;
        Color SavedColor = GUI.color;

        GUIUtility.RotateAroundPivot(Angle, PointA);
        GUI.color = color;
        GUI.DrawTexture(rect, lineTex, ScaleMode.StretchToFill);

        GUI.matrix = matrix;
        GUI.color = SavedColor;
    }
}