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
    public float Red { get; set; }
    public float Green { get; set; }
    public float Blue { get; set; }
    public float Alpha { get; set; }

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
