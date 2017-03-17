/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections.Generic;
using System;

public class ScenarioSelectScreen : ScreenParent
{
    List<ScenarioListEntry> Scenarios = new List<ScenarioListEntry>();
    Scenario selectedScenario = null;

    Rect ScenarioSelectionAreaRect;
    Rect ScenarioListWindowRect;
    Rect ScenarioListViewRect;
    Rect PlayButtonRect;
    Rect EditorButtonRect;
    Rect BackButtonRect;

    Vector2 ScenarioListPosition = Vector2.zero;
    Vector2 DescriptionPosition = Vector2.zero;

    public ScenarioSelectScreen()
    {
        ScenarioSelectionAreaRect = new Rect(Screen.width * 0.06f, Screen.height * 0.05f, Screen.width * 0.3f, Screen.height * 0.9f);
        ScenarioListWindowRect = new Rect(ScenarioSelectionAreaRect.x + ScenarioSelectionAreaRect.width * 0.1f, ScenarioSelectionAreaRect.y + ScenarioSelectionAreaRect.height * 0.03f, ScenarioSelectionAreaRect.width * 0.8f, ScenarioSelectionAreaRect.height * 0.875f);
        ScenarioListViewRect = new Rect(0,0, ScenarioListWindowRect.width * 0.92f, ScenarioListWindowRect.height * 1.03f);
        EditorButtonRect = new Rect(ScenarioSelectionAreaRect.x + (ScenarioSelectionAreaRect.width - GameManager.instance.StandardButtonSize.x) / 2f, ScenarioSelectionAreaRect.yMax - GameManager.instance.StandardButtonSize.y * 1.5f, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);
        float indent = (EditorButtonRect.x - ScenarioSelectionAreaRect.x - GameManager.instance.StandardButtonSize.x) / 2f;
        PlayButtonRect = new Rect(ScenarioSelectionAreaRect.x + indent, EditorButtonRect.y, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);
        BackButtonRect = new Rect(EditorButtonRect.xMax + indent, EditorButtonRect.y, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);

        BuildScenarioList();
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.instance.ChangeScreen(new MainMenuScreen());
        }
    }

    public override void Draw()
    {
        GUI.Box(ScenarioSelectionAreaRect, "");

        ScenarioListPosition = GUI.BeginScrollView(ScenarioListWindowRect, ScenarioListPosition, ScenarioListViewRect);
        foreach (ScenarioListEntry entry in Scenarios)
        {
            entry.Draw(selectedScenario);
        }
        GUI.EndScrollView();

        if (selectedScenario != null)
        {
            if (GUI.Button(PlayButtonRect, "Play", GameManager.instance.standardButtonStyle))
            {
                GameManager.instance.ChangeScreen(new ScenarioPlayScreen(selectedScenario));
            }
        }
        else
        {
            GUI.enabled = false;
            if (GUI.Button(PlayButtonRect, "Play", GameManager.instance.standardButtonStyle)){}
            GUI.enabled = true;
        }
        if (GUI.Button(EditorButtonRect, "Editor", GameManager.instance.standardButtonStyle))
        {
            GameManager.instance.ChangeScreen(new ScenarioCreateScreen());
        }
        if(GUI.Button(BackButtonRect, "Back", GameManager.instance.standardButtonStyle))
        {
            GameManager.instance.ChangeScreen(new MainMenuScreen());
        }
    }

    protected override void CloseScreen()
    {
        
    }

    void BuildScenarioList()
    {
        Scenarios.Clear();
        selectedScenario = null;
        float entrySize = ScenarioListWindowRect.height / 4f;

        foreach (KeyValuePair<string, Scenario> keyVal in ResourceManager.Scenariors)
        {
            if (!keyVal.Value.Deleted)
            {
                Rect rect = new Rect(0, entrySize * Scenarios.Count, ScenarioListViewRect.width, entrySize);
                ScenarioListEntry entry = new ScenarioListEntry(rect, keyVal.Value, SelectScenario, GameManager.instance);
                Scenarios.Add(entry);
            }
        }
        if (Scenarios.Count > 0)
        {
            selectedScenario = Scenarios[0].scenario;
        }

        ResizeViewListWindow(ref ScenarioListViewRect, ScenarioListWindowRect, Scenarios.Count, entrySize);
    }

    void SelectScenario(Scenario scenario)
    {
        selectedScenario = scenario;
    }
}
