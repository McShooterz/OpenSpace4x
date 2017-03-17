/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public abstract class ScrollListBase
{
    protected Rect ScrollWindowRect;
    protected Rect ScrollViewRect;
    protected Vector2 ScrollPosition;

    protected Vector2 ListEntrySize;

    protected int SelectionIndex = 0;


    public void ChangeSelectionIndex(int index)
    {
        SelectionIndex = index;
    }

    public bool Contains(Vector2 Point)
    {
        return ScrollWindowRect.Contains(Point);
    }

    public Rect getRect()
    {
        return ScrollWindowRect;
    }

}
