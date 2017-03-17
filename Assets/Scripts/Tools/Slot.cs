/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

[System.Serializable]
public class Slot
{
    public Rect rect;
    public bool Active;
    public QuadrantTypes Quadrant;
    public Slot(Rect rectanle)
    {
        rect = rectanle;
    }
}
