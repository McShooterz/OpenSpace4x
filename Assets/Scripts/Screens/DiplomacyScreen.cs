﻿/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class DiplomacyScreen : ScreenParent
{

    public DiplomacyScreen()
    {

    }




    public override void Draw()
    {








        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.instance.ChangeScreen(new CampaignPlayScreen());
        }
    }

    protected override void CloseScreen()
    {

    }
}
