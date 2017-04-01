/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuScreen : BaseScreen
{
    #region Variables
    [SerializeField]
    Button NewCampaignButton;

    [SerializeField]
    Button EmpireEditorButton;

    [SerializeField]
    Button ScenariosButton;

    [SerializeField]
    Button ShipyardButton;

    [SerializeField]
    Button StationyardButton;

    [SerializeField]
    Button CombatSimulatorButton;

    [SerializeField]
    Button OptionsButton;

    [SerializeField]
    Button CreditsButton;

    [SerializeField]
    Button QuitButton;

    #endregion

    void Start()
    {
        NewCampaignButton.GetComponentInChildren<Text>().text = ResourceManager.GetLocalization("NewCampaign");

        EmpireEditorButton.GetComponentInChildren<Text>().text = ResourceManager.GetLocalization("EmpireEditor");

        ScenariosButton.GetComponentInChildren<Text>().text = ResourceManager.GetLocalization("Scenarios");

        ShipyardButton.GetComponentInChildren<Text>().text = ResourceManager.GetLocalization("Shipyard");

        StationyardButton.GetComponentInChildren<Text>().text = ResourceManager.GetLocalization("Stationyard");

        CombatSimulatorButton.GetComponentInChildren<Text>().text = ResourceManager.GetLocalization("CombatSimulator");

        OptionsButton.GetComponentInChildren<Text>().text = ResourceManager.GetLocalization("Options");

        CreditsButton.GetComponentInChildren<Text>().text = ResourceManager.GetLocalization("Credits");

        QuitButton.GetComponentInChildren<Text>().text = ResourceManager.GetLocalization("Quit");
    }

    public void ClickQuit()
    {
        Application.Quit();
    }
}
