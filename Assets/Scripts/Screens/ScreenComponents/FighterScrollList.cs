/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections.Generic;

public sealed class FighterScrollList : ScrollListBase
{
    public delegate bool CheckHullValid(FighterDefinition fighterData);

    List<FighterListEntry> FighterList = new List<FighterListEntry>();

    public FighterScrollList(Rect rect, FighterListEntry.ButtonPress changeFighterCallBack, CheckHullValid checkFighters)
    {
        ScrollWindowRect = rect;
        ScrollViewRect = new Rect(0, 0, ScrollWindowRect.width * 0.92f, ScrollWindowRect.height * 1.02f);
        ScrollPosition = Vector2.zero;

        FighterList.Clear();
        float EntrySize = ScrollViewRect.width / 5f;
        int index = 0;
        foreach (KeyValuePair<string, FighterDefinition> fighterDefinitions in ResourceManager.GetFighterDefinitions())
        {
            if (checkFighters(fighterDefinitions.Value))
            {
                Rect entryRect = new Rect(EntrySize * (FighterList.Count % 5), EntrySize * (FighterList.Count / 5), EntrySize, EntrySize);
                FighterListEntry HLE = new FighterListEntry(entryRect, fighterDefinitions.Value, index, changeFighterCallBack, ChangeSelectionIndex);
                FighterList.Add(HLE);
                index++;
            }
        }
        ScrollViewRect.height = Mathf.Max(ScrollWindowRect.height * 1.02f, EntrySize * (FighterList.Count / 5 + (FighterList.Count % 5 != 0 ? 1 : 0)));
    }

    public void Draw()
    {
        ScrollPosition = GUI.BeginScrollView(ScrollWindowRect, ScrollPosition, ScrollViewRect);
        foreach (FighterListEntry entry in FighterList)
        {
            entry.Draw(SelectionIndex);
        }
        GUI.EndScrollView();
    }

    public void CheckFirstEntry(FighterListEntry.ButtonPress changeFighterCallBack)
    {
        if (FighterList.Count > 0)
        {
            changeFighterCallBack(FighterList[0].fighterDefinition);
        }
    }

    public FighterDefinition GetFirstEntryItem()
    {
        if (FighterList.Count > 0)
        {
            return FighterList[0].fighterDefinition;
        }
        return null;
    }
}
