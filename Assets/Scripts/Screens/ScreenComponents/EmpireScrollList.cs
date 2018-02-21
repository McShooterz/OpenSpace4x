/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections.Generic;

public class EmpireScrollList : ScrollListBase
{
    List<EmpireListEntry> EmpireEntries = new List<EmpireListEntry>();

    EmpireListEntry.ButtonPress LoadEmpireDefinition;

    public EmpireScrollList(Rect rect, EmpireListEntry.ButtonPress loadEmpireDefinition)
    {
        ScrollWindowRect = rect;
        LoadEmpireDefinition = loadEmpireDefinition;

        ScrollViewRect = new Rect(0, 0, ScrollWindowRect.width * 0.9f, ScrollWindowRect.height * 5f);

        ListEntrySize = new Vector2(ScrollViewRect.width, ScrollWindowRect.height * 0.125f);

        RebuildList();
    }

    public void Draw()
    {
        GUI.Box(ScrollWindowRect, "");

        ScrollPosition = GUI.BeginScrollView(ScrollWindowRect, ScrollPosition, ScrollViewRect);
        foreach (EmpireListEntry entry in EmpireEntries)
        {
            entry.Draw(SelectionIndex);
        }
        GUI.EndScrollView();
    }

    public void AddEmpireListEntry(EmpireDefinition empire)
    {


        Rect rect = new Rect(new Vector2(0, EmpireEntries.Count * ListEntrySize.y), ListEntrySize);


        EmpireEntries.Add(new EmpireListEntry(rect, EmpireEntries.Count, empire, ChangeSelectionIndex, LoadEmpireDefinition));
    }

    public void RebuildList()
    {
        EmpireEntries.Clear();

        //foreach (KeyValuePair<string, EmpireDefinition> entry in ResourceManager.Empires)
        //{
            //AddEmpireListEntry(entry.Value);
        //}



        ScrollViewRect.height = Mathf.Max(EmpireEntries.Count * ListEntrySize.y, ScrollWindowRect.height * 1.05f);
    }

    public void LoadFirstEmpire()
    {
        if (EmpireEntries.Count > 0)
        {
            LoadEmpireDefinition(EmpireEntries[0].GetDefinition());
        }
    }
}
