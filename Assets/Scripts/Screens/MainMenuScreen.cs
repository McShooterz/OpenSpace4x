/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class MainMenuScreen : MonoBehaviour
{
    public void ClickNewCampaign()
    {
        ScreenManager.instance.ChangeScreen("NewCampaignScreen");
    }

    public void ClickEmpireEditor()
    {
        ScreenManager.instance.ChangeScreen("EmpireEditorScreen");
    }

    public void ClickScenarios()
    {
        ScreenManager.instance.ChangeScreen("ScenariosScreen");
    }

    public void ClickShipyard()
    {
        ScreenManager.instance.ChangeScreen("ShipyardScreen");
    }

    public void ClickStationyard()
    {
        ScreenManager.instance.ChangeScreen("StationyardScreen");
    }

    public void ClickCombatSimulator()
    {
        ScreenManager.instance.ChangeScreen("CombatSimulatorScreen");
    }

    public void ClickOptions()
    {
        ScreenManager.instance.ChangeScreen("OptionsScreen");
    }

    public void ClickCredits()
    {
        ScreenManager.instance.ChangeScreen("CreditsScreen");
    }

    public void ClickQuit()
    {
        Application.Quit();
    }
}
