/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections.Generic;

public class ShipHullScrollList : ScrollListBase
{
    public delegate bool CheckHullValid(ShipHullData hullData);

    protected List<HullListEntry> HullList = new List<HullListEntry>();

    public ShipHullScrollList(Rect rect, HullListEntry.ButtonPress changeHullCallBack, CheckHullValid checkHulls)
    {
        ScrollWindowRect = rect;
        ScrollViewRect = new Rect(0, 0, ScrollWindowRect.width * 0.92f, ScrollWindowRect.height * 1.02f);
        ScrollPosition = Vector2.zero;

        HullList.Clear();
        float EntrySize = ScrollViewRect.width / 5f;
        int index = 0;
        foreach (ShipHullData hull in ResourceManager.instance.GetShipHulls())
        {
            if (checkHulls(hull))
            {
                Rect entryRect = new Rect(EntrySize * (HullList.Count % 5), EntrySize * (HullList.Count / 5), EntrySize, EntrySize);
                HullListEntry HLE = new HullListEntry(entryRect, hull, index, changeHullCallBack, ChangeSelectionIndex);
                HullList.Add(HLE);
                index++;
            }
        }

        ScrollViewRect.height = Mathf.Max(ScrollWindowRect.height * 1.02f, EntrySize * (HullList.Count / 5 + (HullList.Count % 5 != 0 ? 1 : 0)));
    }

    public void Draw()
    {
        ScrollPosition = GUI.BeginScrollView(ScrollWindowRect, ScrollPosition, ScrollViewRect);
        foreach (HullListEntry entry in HullList)
        {
            entry.Draw(SelectionIndex);
        }
        GUI.EndScrollView();
    }

    public void CheckFirstHull(HullListEntry.ButtonPress changeHullCallBack)
    {
        if (HullList.Count > 0)
        {
            changeHullCallBack(HullList[0].hullData);
        }
        SelectionIndex = 0;
    }

    public ShipHullData GetSelectedHull()
    {
        if (HullList.Count > 0)
        {
            return HullList[SelectionIndex].hullData;
        }
        return null;
    }
}
