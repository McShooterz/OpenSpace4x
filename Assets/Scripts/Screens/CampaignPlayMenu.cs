/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class CampaignPlayMenu : ScreenParent
{
    Rect RectMenuResumeButton = new Rect(Screen.width / 2 - 75, Screen.height / 4, 150, 40);
    Rect RectMenuOptionsButton = new Rect(Screen.width / 2 - 75, Screen.height / 4 + 40, 150, 40);
    Rect RectMenuSaveButton = new Rect(Screen.width / 2 - 75, Screen.height / 4 + 80, 150, 40);
    Rect RectMenuLoadButton = new Rect(Screen.width / 2 - 75, Screen.height / 4 + 120, 150, 40);
    Rect RectMenuQuitGameButton = new Rect(Screen.width / 2 - 75, Screen.height / 4 + 160, 150, 40);
    Rect RectMenuQuitProgramButton = new Rect(Screen.width / 2 - 75, Screen.height / 4 + 200, 150, 40);

    public CampaignPlayMenu()
    {

    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.instance.ChangeScreen(new CampaignPlayScreen());
        }
    }

    public override void Draw()
    {
        if (GUI.Button(RectMenuResumeButton, "Resume"))
        {
            GameManager.instance.ChangeScreen(new CampaignPlayScreen());
        }
        if (GUI.Button(RectMenuOptionsButton, "Options"))
        {
            GameManager.instance.ChangeScreen(new OptionsScreen(this));
        }
        if (GUI.Button(RectMenuSaveButton, "Save Game"))
        {
            
        }
        if (GUI.Button(RectMenuLoadButton, "Load Game"))
        {
            
        }
        if (GUI.Button(RectMenuQuitGameButton, "Quit Campaign"))
        {
            GameManager.instance.ChangeScreen(new MainMenuScreen());
        }
        if (GUI.Button(RectMenuQuitProgramButton, "Quit Program"))
        {
            Application.Quit();
        }
    }

    protected override void CloseScreen()
    {

    }
}
