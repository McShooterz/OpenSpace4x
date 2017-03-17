/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class StationDesignListEntry
{
    Rect selectRect;
    Rect DeleteRect;
    public StationDesign Design;
    public delegate void ButtonPress(StationDesign design);
    public delegate void DeletePress(StationDesign design);
    protected ButtonPress buttonCallBack;
    protected DeletePress deleteCallBack;

    GUIStyle ButtonActiveStyle;

    public StationDesignListEntry(Rect rect, StationDesign design, ButtonPress callback, DeletePress deleteCall, GameManager gameController)
    {
        Design = design;
        buttonCallBack = callback;
        deleteCallBack = deleteCall;

        ButtonActiveStyle = gameController.SquareButtonGreenStyle;

        selectRect = new Rect(rect.x, rect.y, rect.width - rect.height, rect.height);
        DeleteRect = new Rect(selectRect.xMax, rect.y, rect.width - selectRect.width, rect.height);
    }

    public void Draw(StationDesign selected)
    {
        if (selected == Design)
        {
            if (GUI.Button(selectRect, Design.Name, ButtonActiveStyle))
            {
                buttonCallBack(Design);
                AudioManager.instance.PlayUIClip("SelectButtonClick");
            }
            if (GUI.Button(DeleteRect, "X", ButtonActiveStyle))
            {
                deleteCallBack(Design);
                PlayMainButtonClick();
            }
        }
        else
        {
            if (GUI.Button(selectRect, Design.Name))
            {
                buttonCallBack(Design);
                AudioManager.instance.PlayUIClip("SelectButtonClick");
            }
            if (GUI.Button(DeleteRect, "X"))
            {
                deleteCallBack(Design);
                PlayMainButtonClick();
            }
        }
    }

    void PlayMainButtonClick()
    {
        AudioManager.instance.PlayUIClip("MainButtonClick");
    }
}