/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class EspionageScreen : ScreenParent
{

    public EspionageScreen()
    {

    }

    public override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseScreen();
        }
    }

    public override void Draw()
    {

    }

    protected override void CloseScreen()
    {
        GameManager.instance.ChangeScreen(new CampaignPlayScreen());
    }
}
