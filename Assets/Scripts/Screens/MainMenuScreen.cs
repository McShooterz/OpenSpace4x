/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class MainMenuScreen : ScreenParent
{
    Rect CampaignButtonRect;
    Rect QuitButtonRect;
    Rect EmpireEditorRect;
    Rect ScenarioButtonRect;
    Rect ShipYardButtonRect;
    Rect StationyardButtonRect;
    Rect CombatSimulatorButtonRect;
    Rect OptionsMenuButtonRect;
    Rect CreditsButtonRect;

    public MainMenuScreen()
    {
        Vector2 mainMenuButtonSize = new Vector2(Screen.width * 0.078f, Screen.height * 0.037f);
        float Indent = Screen.width * 0.026f;

        CampaignButtonRect = new Rect(Indent, 200, mainMenuButtonSize.x, mainMenuButtonSize.y);
        EmpireEditorRect = new Rect(Indent, CampaignButtonRect.yMax, mainMenuButtonSize.x, mainMenuButtonSize.y);
        QuitButtonRect = new Rect(Indent, Screen.height - mainMenuButtonSize.y * 2f, mainMenuButtonSize.x, mainMenuButtonSize.y);
        ScenarioButtonRect = new Rect(Indent, EmpireEditorRect.yMax, mainMenuButtonSize.x, mainMenuButtonSize.y);
        ShipYardButtonRect = new Rect(Indent, ScenarioButtonRect.yMax, mainMenuButtonSize.x, mainMenuButtonSize.y);
        StationyardButtonRect = new Rect(Indent, ShipYardButtonRect.yMax, mainMenuButtonSize.x, mainMenuButtonSize.y);
        CombatSimulatorButtonRect = new Rect(Indent, StationyardButtonRect.yMax, mainMenuButtonSize.x, mainMenuButtonSize.y);
        OptionsMenuButtonRect = new Rect(Indent, CombatSimulatorButtonRect.yMax, mainMenuButtonSize.x, mainMenuButtonSize.y);
        CreditsButtonRect = new Rect(Indent, OptionsMenuButtonRect.yMax, mainMenuButtonSize.x, mainMenuButtonSize.y);
    }

    public override void Draw()
    {
        if (GUI.Button(CampaignButtonRect, "New Campaign"))
        {
            GameManager.instance.ChangeScreen(new CampaignSetupScreen());
        }
        if(GUI.Button(EmpireEditorRect, "Empire Editor"))
        {
            GameManager.instance.ChangeScreen(new EmpireCreationScreen(this));
        }
        if (GUI.Button(ScenarioButtonRect, "Scenarios"))
        {
            //GameManager.instance.ChangeScreen(new ScenarioSelectScreen());
        }
        if(GUI.Button(ShipYardButtonRect, "Shipyard"))
        {
            GameManager.instance.ChangeScreen(new MainShipyardScreen());
        }
        if (GUI.Button(StationyardButtonRect, "Station Yard"))
        {
            GameManager.instance.ChangeScreen(new MainStationyardScreen());
        }
        if (GUI.Button(CombatSimulatorButtonRect, "Combat Simulator"))
        {
            GameManager.instance.ChangeScreen(new MainCustomBattleScreen());
        }
        if (GUI.Button(OptionsMenuButtonRect, "Options"))
        {
            GameManager.instance.ChangeScreen(new OptionsScreen(this));
        }
        if (GUI.Button(CreditsButtonRect, "Credits"))
        {
            GameManager.instance.ChangeScreen(new CreditsScreen());
        }
        if (GUI.Button(QuitButtonRect, "Quit"))
        {
            Application.Quit();
        }
    }

    protected override void CloseScreen()
    {

    }
}
