/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class MainStationyardScreen : StationyardBaseScreen
{
    #region Variables
    #endregion

    public MainStationyardScreen()
    {
        Initialize();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Draw()
    {
        base.Draw();
    }

    protected override void CloseScreen()
    {
        if (unitModel != null)
            Object.Destroy(unitModel);
        GameManager.instance.ChangeScreen(new MainMenuScreen());
    }
}
