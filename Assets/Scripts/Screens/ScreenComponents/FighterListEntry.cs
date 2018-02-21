/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public sealed class FighterListEntry : ListEntryBase
{
    public FighterDefinition fighterDefinition;
    public delegate void ButtonPress(FighterDefinition fighter);
    ButtonPress buttonCallBack;

    Texture2D Icon;

    public FighterListEntry(Rect rect, FighterDefinition fighter, int index, ButtonPress callBack, ChangeSelectedIndex changeIndex)
    {
        baseRect = rect;
        fighterDefinition = fighter;
        Index = index;
        buttonCallBack = callBack;
        changeSelectionIndex = changeIndex;

        //Icon = fighterDefinition.GetIcon();
    }

    public void Draw(int selectedIndex)
    {
        if (selectedIndex == Index)
        {
            if (GUI.Button(baseRect, "", GameManager.instance.SquareButtonGreenStyle))
            {
                buttonCallBack(fighterDefinition);
                changeSelectionIndex(Index);
                AudioManager.instance.PlayUIClip("SelectButtonClick");
            }
        }
        else
        {
            if (GUI.Button(baseRect, ""))
            {
                buttonCallBack(fighterDefinition);
                changeSelectionIndex(Index);
                AudioManager.instance.PlayUIClip("SelectButtonClick");
            }
        }
        GUI.DrawTexture(baseRect, Icon);
    }
}
