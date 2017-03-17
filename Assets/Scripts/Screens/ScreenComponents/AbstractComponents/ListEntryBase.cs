/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public abstract class ListEntryBase
{
    protected Rect baseRect;
    protected int Index;
    public delegate void ChangeSelectedIndex(int index);
    protected ChangeSelectedIndex changeSelectionIndex;

    public bool Contains(Vector2 Point)
    {
        return baseRect.Contains(Point);
    }

    public Rect GetRect()
    {
        return baseRect;
    }
}
