/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class CampaignSetupScreen : ScreenParent
{
    Rect RectStartCampaignButton;
    Rect RectBackButton;


    public CampaignSetupScreen()
    {

        Vector2 BottomButtonSize = new Vector2(Screen.width / 12.8f, Screen.height / 27f);
        RectStartCampaignButton = new Rect(new Vector2(Screen.width - BottomButtonSize.x * 1.25f, Screen.height - BottomButtonSize.y * 1.5f), BottomButtonSize);
        RectBackButton = new Rect(new Vector2(RectStartCampaignButton.x - BottomButtonSize.x * 1.25f, RectStartCampaignButton.y), BottomButtonSize);
    }

    public override void Draw()
    {

        if (GUI.Button(RectStartCampaignButton, "Start Campaign"))
        {
            GameManager.instance.ChangeScreen(new CampaignPlayScreen());
            //Application.LoadLevel("CampaignMap");
        }
        if (GUI.Button(RectBackButton, "Back"))
        {
            CloseScreen();
        }

    }

    protected override void CloseScreen()
    {
        //GameManager.instance.ChangeScreen(new MainMenuScreen());
    }
}
