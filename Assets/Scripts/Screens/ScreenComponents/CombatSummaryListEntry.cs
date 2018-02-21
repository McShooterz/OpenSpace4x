/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class CombatSummaryListEntry
{
    Rect baseRect;
    Rect IconRect;
    Rect NameRect;
    GUIStatBar LevelBar;
    Rect StatusRect;
    Rect KillsRect;
    Rect DamageDealtRect;
    Rect DamageTakenRect;

    UnitData unitData;

    Texture2D unitIcon;

    public CombatSummaryListEntry(Rect rect, UnitData data)
    {
        baseRect = rect;
        unitData = data;

        float spacing = baseRect.width * 0.025f;

        IconRect = new Rect(baseRect.x + baseRect.width * 0.01f, baseRect.y + baseRect.height * 0.07f, baseRect.height * 0.86f, baseRect.height * 0.86f);
        NameRect = new Rect(IconRect.xMax + spacing, baseRect.y + baseRect.height * 0.1f, GameManager.instance.StandardLabelSize.x, baseRect.height * 0.8f);
        Rect LevelBarRect = new Rect(NameRect.xMax + spacing, baseRect.y + (baseRect.height - GameManager.instance.StandardLabelSize.y) / 2f, GameManager.instance.StandardLabelSize.x, GameManager.instance.StandardLabelSize.y);
        LevelBar = new GUIStatBar(LevelBarRect, "StatBarBackground", "StatBarExperience");
        StatusRect = new Rect(LevelBarRect.xMax + spacing, LevelBarRect.y, GameManager.instance.StandardLabelSize.x, GameManager.instance.StandardLabelSize.y);
        KillsRect = new Rect(StatusRect.xMax + spacing, StatusRect.y, GameManager.instance.StandardLabelSize.x, GameManager.instance.StandardLabelSize.y);
        DamageDealtRect = new Rect(KillsRect.xMax + spacing, KillsRect.y, GameManager.instance.StandardLabelSize.x, GameManager.instance.StandardLabelSize.y);
        DamageTakenRect = new Rect(DamageDealtRect.xMax + spacing, DamageDealtRect.y, GameManager.instance.StandardLabelSize.x, GameManager.instance.StandardLabelSize.y);

        //unitIcon = unitData.GetIcon();
    }

    public void Draw()
    {
        GUI.Box(baseRect, "");
        GUI.DrawTexture(IconRect, unitIcon);
        GUI.Label(NameRect, unitData.DisplayName);
        if(unitData is ShipData)
            LevelBar.Draw((unitData as ShipData).ExperienceRatio(), "Level: " + (unitData as ShipData).Level.ToString());
        if(unitData.Destroyed)
        {
            GUI.Label(StatusRect, "<color=red>Destroyed</color>");
        }
        else if(unitData is ShipData && (unitData as ShipData).Retreated)
        {
            GUI.Label(StatusRect, "<color=yellow>Retreated</color>");
        }
        else
        {
            GUI.Label(StatusRect, "Active");
        }
        GUI.Label(KillsRect, unitData.Kills.ToString());
        GUI.Label(DamageDealtRect, unitData.damageDealt.ToString("0.#"));
        GUI.Label(DamageTakenRect, unitData.damageTaken.ToString("0.#"));
    }
}
