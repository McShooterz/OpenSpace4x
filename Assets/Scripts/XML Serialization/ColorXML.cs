/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class ColorXML
{
    public float Red;
    public float Green;
    public float Blue;
    public float Alpha;

    public ColorXML() { }

    public ColorXML(Color color)
    {
        Red = color.r;
        Green = color.g;
        Blue = color.b;
        Alpha = color.a;
    }

    public Color GetColor()
    {
        return new Color(Red, Green, Blue, Alpha);
    }
}
