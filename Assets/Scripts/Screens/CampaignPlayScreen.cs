/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class CampaignPlayScreen : ScreenParent
{
    //Temp Message
    Rect MessageRect;
    string Message;

    Rect RectMenuButton;
    Rect RectTechTreeButton;
    Rect RectEmpireManagerButton;
    Rect RectEspionageButton;
    Rect RectDiplomacyButton;
    Rect RectFleetManagerButton;
    Rect RectShipyardButton;
    Rect RectCustomBattleButton;

    GUIToolTip toolTip;

    //End Turn element
    Rect RectEndTurnButton = new Rect(Screen.width - 280, Screen.height - 37, 120, 35);

    //Empire Info Panel
    Rect RectEmpireInfo = new Rect(Screen.width - 400, 0, 400, 30);

    //Resource Info Panel
    Rect RectResourceInfo = new Rect(Screen.width - 120, 30, 120, 150);

    //Mini Map
    Rect RectMiniMap = new Rect(Screen.width - 140, Screen.height - 140, 140, 140);

    //Fleet Info Panel
    Rect RectFleetInfo = new Rect(0, Screen.height - 200, 160, 200);

    //Ship Info Panel
    Rect RectShipInfo = new Rect(160, Screen.height - 150, 180, 150);


    public CampaignPlayScreen()
    {
        Vector2 messageSize = new Vector2(Screen.width * 0.15f, Screen.height * 0.15f);
        MessageRect = new Rect((Screen.width - messageSize.x) / 2, (Screen.height - messageSize.y) / 2, messageSize.x, messageSize.y);
        Message = "Pre Alpha Build, to build ship designs click on SY above, and to test out the design click on CB above.";

        RectMenuButton = new Rect(0, 0, 50, 20);
        RectTechTreeButton = new Rect(50, 0, 50, 20);
        RectEmpireManagerButton = new Rect(100, 0, 50, 20);
        RectEspionageButton = new Rect(150, 0, 50, 20);
        RectDiplomacyButton = new Rect(200, 0, 50, 20);
        RectFleetManagerButton = new Rect(250, 0, 50, 20);
        RectShipyardButton = new Rect(300, 0, 50, 20);
        RectCustomBattleButton = new Rect(350, 0, 50, 20);

        toolTip = new GUIToolTip(new Vector2(0, RectMenuButton.yMax + RectMenuButton.height * 0.25f), Screen.width * 0.1f);
    }

    public override void Draw()
    {
        mousePosition = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);

        GUI.Box(MessageRect, Message, GameManager.instance.ToolTipTitleStyle);

        DrawToolTip(mousePosition);

        //Draw top left hand navigation buttons
        if (GUI.Button(RectMenuButton, "Menu"))
        {
            GameManager.instance.ChangeScreen(new CampaignPlayMenu());
        }
        if (GUI.Button(RectTechTreeButton, "TT"))
        {
            GameManager.instance.ChangeScreen(new ResearchScreen());
        }
        if (GUI.Button(RectEmpireManagerButton, "EM"))
        {
            GameManager.instance.ChangeScreen(new EmpireManagementScreen());
        }
        if (GUI.Button(RectEspionageButton, "ES"))
        {
            GameManager.instance.ChangeScreen(new EspionageScreen());
        }
        if (GUI.Button(RectDiplomacyButton, "DP"))
        {
            GameManager.instance.ChangeScreen(new DiplomacyScreen());
        }
        if (GUI.Button(RectFleetManagerButton, "FM"))
        {
            GameManager.instance.ChangeScreen(new FleetManagementScreen());
        }
        if (GUI.Button(RectShipyardButton, "SY"))
        {
            GameManager.instance.ChangeScreen(new CampaignShipYardScreen());
        }
        if(GUI.Button(RectCustomBattleButton, "CB"))
        {
            GameManager.instance.ChangeScreen(new CustomBattleScreen());
        }


        //Draw top right hand empire info
        GUI.Box(RectEmpireInfo, "Empire Info");

        //Draw top right hand resource info panel
        GUI.Box(RectResourceInfo, "Resource Info");

        //Draw bottom left fleet info panel with bottom middle ship info panel
        GUI.Box(RectFleetInfo, "Fleet Info");
        GUI.Box(RectShipInfo, "Ship Info");

        //Draw botton right End Turn button
        if (GUI.Button(RectEndTurnButton, "End Turn"))
        {

        }

        //Draw bottom right mini map
        GUI.Box(RectMiniMap, "Mini Map");

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.instance.ChangeScreen(new CampaignPlayMenu());
        }

        toolTip.Draw();
    }

    void DrawToolTip(Vector2 mouse)
    {
        if(RectShipyardButton.Contains(mouse))
        {
            toolTip.SetText("screenShipyard", "screenShipyardDesc");
        }
        else if(RectCustomBattleButton.Contains(mouse))
        {
            toolTip.SetText("screenBattleSim", "screenBattleSimDesc");
        }
    }

    protected override void CloseScreen()
    {

    }
}
