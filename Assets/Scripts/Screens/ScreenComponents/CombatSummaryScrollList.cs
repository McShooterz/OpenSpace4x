/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections.Generic;

public class CombatSummaryScrollList
{
    Rect baseRect;
    Rect TitleRect;
    Rect ListWindowRect;
    Rect ListViewRect;
    Rect CloseButtonRect;

    Rect NameRect;
    Rect LevelRect;
    Rect StatusRect;
    Rect KillsRect;
    Rect DamageDealtRect;
    Rect DamageTakenRect;

    Vector2 SummaryListPosition;

    string Result = "Draw";

    float ListEntryHeight;

    List<CombatSummaryListEntry> SummaryUnitList = new List<CombatSummaryListEntry>();

    bool Open = false;

    public CombatSummaryScrollList()
    {
        baseRect = new Rect(Screen.width * 0.16f, Screen.height * 0.12f, Screen.width * 0.68f, Screen.height * 0.76f);
        TitleRect = new Rect(baseRect.x, baseRect.y + baseRect.height * 0.01f, baseRect.width, baseRect.height * 0.06f);

        float spacing = baseRect.width * 0.84f * 0.025f;
        NameRect = new Rect(baseRect.x + baseRect.width * 0.08f + GameManager.instance.StandardLabelSize.x, TitleRect.yMax, GameManager.instance.StandardLabelSize.x, GameManager.instance.StandardLabelSize.y);
        LevelRect = new Rect(NameRect.xMax + spacing, NameRect.y, GameManager.instance.StandardLabelSize.x, GameManager.instance.StandardLabelSize.y);
        StatusRect = new Rect(LevelRect.xMax + spacing, NameRect.y, GameManager.instance.StandardLabelSize.x, GameManager.instance.StandardLabelSize.y);
        KillsRect = new Rect(StatusRect.xMax + spacing, NameRect.y, GameManager.instance.StandardLabelSize.x, GameManager.instance.StandardLabelSize.y);
        DamageDealtRect = new Rect(KillsRect.xMax + spacing, NameRect.y, GameManager.instance.StandardLabelSize.x, GameManager.instance.StandardLabelSize.y);
        DamageTakenRect = new Rect(DamageDealtRect.xMax + spacing, NameRect.y, GameManager.instance.StandardLabelSize.x, GameManager.instance.StandardLabelSize.y);

        ListWindowRect = new Rect(baseRect.x + baseRect.width * 0.08f, NameRect.yMax + baseRect.height * 0.01f, baseRect.width * 0.84f, baseRect.height * 0.75f);
        ListViewRect = new Rect(0, 0, ListWindowRect.width * 0.975f, ListWindowRect.height * 1.02f);
        CloseButtonRect = new Rect(baseRect.x + (baseRect.width - GameManager.instance.StandardButtonSize.x) / 2f, baseRect.yMax - GameManager.instance.StandardButtonSize.y * 1.5f, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);

        SummaryListPosition = Vector2.zero;

        ListEntryHeight = ListWindowRect.height / 8;
    }

    public void Draw()
    {
        GUI.Box(baseRect, "", GameManager.instance.standardBackGround);
        GUI.Label(TitleRect, Result);

        GUI.Label(NameRect, "Name:");
        GUI.Label(LevelRect, "Level:");
        GUI.Label(StatusRect, "Status:");
        GUI.Label(KillsRect, "Kills:");
        GUI.Label(DamageDealtRect, "Damage Dealt:");
        GUI.Label(DamageTakenRect, "Damage Taken:");

        SummaryListPosition = GUI.BeginScrollView(ListWindowRect, SummaryListPosition, ListViewRect);
        foreach (CombatSummaryListEntry entry in SummaryUnitList)
        {
            entry.Draw();
        }
        GUI.EndScrollView();

        if (GUI.Button(CloseButtonRect, "Close"))
        {
            Open = false;
        }
    }

    public void SetOpen(bool state)
    {
        Open = state;
    }

    public bool isOpen()
    {
        return Open;
    }

    public void AddFleetEntries(FleetData fleet)
    {
        AddEntries(fleet.GetShips());
        AddEntries(fleet.GetStations());
    }

    void AddEntries(List<ShipData> unitDatas)
    {
        foreach (ShipData data in unitDatas)
        {
            AddEntry(data);
        }
    }

    void AddEntries(List<StationData> unitDatas)
    {
        foreach (StationData data in unitDatas)
        {
            AddEntry(data);
        }
    }

    void AddEntry(UnitData unitData)
    {
        Rect entryRect = new Rect(0, SummaryUnitList.Count * ListEntryHeight, ListViewRect.width, ListEntryHeight);
        SummaryUnitList.Add(new CombatSummaryListEntry(entryRect, unitData));
    }

    public void UpdateViewWindowSize()
    {
        ListViewRect.height = Mathf.Max(ListWindowRect.height, SummaryUnitList.Count * ListEntryHeight);
    }

    public void SetResult(string result)
    {
        Result = result;
    }

    public void Clear()
    {
        SummaryUnitList.Clear();
    }
}
