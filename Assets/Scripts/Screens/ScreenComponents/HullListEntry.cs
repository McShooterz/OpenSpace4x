/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public sealed class HullListEntry : ListEntryBase
{
    public ShipHullData hullData;
    Texture2D Icon;

    public delegate void ButtonPress(ShipHullData hullData);
    ButtonPress buttonCallBack;

    public HullListEntry(Rect rect, ShipHullData HD, int index, ButtonPress callBack, ChangeSelectedIndex changeIndex)
    {
        baseRect = rect;
        hullData = HD;
        Index = index;
        buttonCallBack = callBack;
        changeSelectionIndex = changeIndex;

        Icon = hullData.GetIcon();
    }

    public void Draw(int selectedIndex)
    {
        if (selectedIndex == Index)
        {
            if (GUI.Button(baseRect, "", GameManager.instance.SquareButtonGreenStyle))
            {
                buttonCallBack(hullData);
                changeSelectionIndex(Index);
                AudioManager.instance.PlayUIClip("SelectButtonClick");
            }
        }
        else
        {
            if (GUI.Button(baseRect, ""))
            {
                buttonCallBack(hullData);
                changeSelectionIndex(Index);
                AudioManager.instance.PlayUIClip("SelectButtonClick");
            }
        }     
        GUI.DrawTexture(baseRect, Icon);
    }
}
