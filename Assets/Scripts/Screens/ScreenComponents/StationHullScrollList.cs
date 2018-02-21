/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections.Generic;

public sealed class StationHullScrollList : ScrollListBase
{
    public delegate bool CheckHullValid(StationHullData hullData);

    List<StationHullListEntry> HullList = new List<StationHullListEntry>();

    public StationHullScrollList(Rect rect, StationHullListEntry.ButtonPress changeHullCallBack, CheckHullValid checkHulls)
    {
        ScrollWindowRect = rect;
        ScrollViewRect = new Rect(0, 0, ScrollWindowRect.width * 0.92f, ScrollWindowRect.height * 1.02f);
        ScrollPosition = Vector2.zero;

        HullList.Clear();
        float EntrySize = ScrollViewRect.width / 5f;
        int index = 0;
        foreach (KeyValuePair<string, StationHullData> hull in ResourceManager.instance.GetStationHulls())
        {
            if (checkHulls(hull.Value))
            {
                Rect entryRect = new Rect(EntrySize * (HullList.Count % 5), EntrySize * (HullList.Count / 5), EntrySize, EntrySize);
                StationHullListEntry HLE = new StationHullListEntry(entryRect, hull.Value, index, changeHullCallBack, ChangeSelectionIndex);
                HullList.Add(HLE);
                index++;
            }
        }

        ScrollViewRect.height = Mathf.Max(ScrollWindowRect.height * 1.02f, EntrySize * (HullList.Count / 5 + (HullList.Count % 5 != 0 ? 1 : 0)));
    }

    public void Draw()
    {
        ScrollPosition = GUI.BeginScrollView(ScrollWindowRect, ScrollPosition, ScrollViewRect);
        foreach (StationHullListEntry entry in HullList)
        {
            entry.Draw(SelectionIndex);
        }
        GUI.EndScrollView();
    }

    public void CheckFirstHull(StationHullListEntry.ButtonPress changeHullCallBack)
    {
        if (HullList.Count > 0)
        {
            changeHullCallBack(HullList[0].stationHullData);
        }
        SelectionIndex = 0;
    }

    public StationHullData GetFirstHullItem()
    {
        if (HullList.Count > 0)
        {
            return HullList[0].stationHullData;
        }
        return null;
    }
}
