/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public sealed class StationHullListEntry : ListEntryBase
{
    public StationHullData stationHullData;
    public delegate void ButtonPress(StationHullData hullData);
    ButtonPress buttonCallBack;

    Texture2D Icon;

    public StationHullListEntry(Rect rect, StationHullData hullData, int index, ButtonPress callBack, ChangeSelectedIndex changeIndex)
    {
        baseRect = rect;
        stationHullData = hullData;
        Index = index;
        buttonCallBack = callBack;
        changeSelectionIndex = changeIndex;

        Icon = stationHullData.GetIcon();
    }

    public void Draw(int selectedIndex)
    {
        if (selectedIndex == Index)
        {
            if (GUI.Button(baseRect, "", GameManager.instance.SquareButtonGreenStyle))
            {
                buttonCallBack(stationHullData);
                changeSelectionIndex(Index);
                AudioManager.instance.PlayUIClip("SelectButtonClick");
            }
        }
        else
        {
            if (GUI.Button(baseRect, ""))
            {
                buttonCallBack(stationHullData);
                changeSelectionIndex(Index);
                AudioManager.instance.PlayUIClip("SelectButtonClick");
            }
        }
        GUI.DrawTexture(baseRect, Icon);
    }
}

