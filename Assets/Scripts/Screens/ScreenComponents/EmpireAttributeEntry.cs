/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections.Generic;

public class EmpireAttributeEntry : ListEntryBase
{
    public KeyValuePair<string, EmpireAttribute> Attribute;
    string DisplayName;
    public delegate void ButtonPress(EmpireAttribute EmpireAttribute);
    ButtonPress buttonCallBack;


    public EmpireAttributeEntry(Rect rect, KeyValuePair<string, EmpireAttribute> attribute, int index, ButtonPress callBack, ChangeSelectedIndex changeIndex)
    {
        baseRect = rect;
        Attribute = attribute;
        Index = index;
        buttonCallBack = callBack;
        changeSelectionIndex = changeIndex;

        DisplayName = ResourceManager.instance.GetLocalization(Attribute.Value.Name);
    }

    public void Draw(int selectedIndex)
    {
        if (selectedIndex == Index)
        {
            if (GUI.Button(baseRect, DisplayName, GameManager.instance.SquareButtonGreenStyle))
            {
                buttonCallBack(Attribute.Value);
                changeSelectionIndex(Index);
                AudioManager.instance.PlayUIClip("SelectButtonClick");
            }
        }
        else
        {
            if (GUI.Button(baseRect, DisplayName))
            {
                buttonCallBack(Attribute.Value);
                changeSelectionIndex(Index);
                AudioManager.instance.PlayUIClip("SelectButtonClick");
            }
        }
    }

    public void Select()
    {
        buttonCallBack(Attribute.Value);
    }
}
