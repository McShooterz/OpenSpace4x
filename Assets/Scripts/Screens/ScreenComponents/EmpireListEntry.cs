/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class EmpireListEntry : ListEntryBase
{
    EmpireDefinition Definition;

    EmpireFlag empireFlag;

    public delegate void ButtonPress(EmpireDefinition empireDefinition);
    ButtonPress buttonCallBack;


    public EmpireListEntry(Rect rect, int index, EmpireDefinition empireDefinition, ChangeSelectedIndex changeIndex, ButtonPress buttonPress)
    {
        baseRect = rect;

        Index = index;

        changeSelectionIndex = changeIndex;

        Definition = empireDefinition;

        buttonCallBack = buttonPress;

        Rect EmpireFlagRect = new Rect(new Rect(baseRect.x, baseRect.y, baseRect.height, baseRect.height));
        empireFlag = new EmpireFlag(EmpireFlagRect, Definition.FlagBackgroundIndex, Definition.FlagEmblemIndex, Definition.FlagBackgroundColor.GetColor(), Definition.FlagEmblemColor.GetColor());
    }

    public void Draw(int index)
    {

        if (Index == index)
        {
            if (GUI.Button(baseRect, Definition.Name, GameManager.instance.SquareButtonGreenStyle))
            {
                buttonCallBack(Definition);
                AudioManager.instance.PlayUIClip("SelectButtonClick");
            }
        }
        else
        {
            if (GUI.Button(baseRect, Definition.Name))
            {
                buttonCallBack(Definition);
                changeSelectionIndex(Index);
                AudioManager.instance.PlayUIClip("SelectButtonClick");
            }
        }

        empireFlag.Draw();
    }

    public EmpireDefinition GetDefinition()
    {
        return Definition;
    }
}
