/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class DesignDataListEntry
{
    Rect rect;
    ShipDesignData Design;
    string Name;
    public delegate void ButtonPress(ShipDesignData design);
    protected ButtonPress buttonCallBack;

    GUIContent Command;
    GUIContent Money;

    Rect NameRect;
    Rect CommandRect;
    Rect MoneyRect;

    public DesignDataListEntry(Rect r, ShipDesignData design, ButtonPress callback)
    {
        rect = r;
        Design = design;
        buttonCallBack = callback;
        Name = Design.Design.Name + "  ";

        Command = new GUIContent();
        Money = new GUIContent();

        GameManager.instance.UIContent.image = null;
        GameManager.instance.UIContent.text = Name;
        //Command.image = ResourceManager.instance.GetIconTexture("Icon_CommandPoint");
        Command.text = ":" + Design.CommandPoints.ToString("0") + "  ";
        //Money.image = ResourceManager.instance.GetIconTexture("Icon_Money");
        Money.text = ":" + Design.GetTotalValue().ToString("0.#");

        float nameWidthMax;
        float nameWidthMin;
        float commandWidthMax;
        float commandWidthMin;
        float moneyWidthMax;
        float moneyWidthMin;

        GameManager.instance.largeLabelStyle.CalcMinMaxWidth(GameManager.instance.UIContent, out nameWidthMin, out nameWidthMax);
        GameManager.instance.largeLabelStyle.CalcMinMaxWidth(Command, out commandWidthMin, out commandWidthMax);
        GameManager.instance.largeLabelStyle.CalcMinMaxWidth(Money, out moneyWidthMin, out moneyWidthMax);

        float indent = (rect.width - (nameWidthMax + commandWidthMax + moneyWidthMax)) / 2f;
        NameRect = new Rect(rect.x + indent, rect.y, nameWidthMax, rect.height);
        CommandRect = new Rect(NameRect.xMax, rect.y, commandWidthMax, rect.height);
        MoneyRect = new Rect(CommandRect.xMax, rect.y, moneyWidthMax, rect.height);
    }

    public void Draw(ShipDesignData selected)
    {
        if (selected == Design)
        {
            if (GUI.Button(rect, "", GameManager.instance.SquareButtonGreenStyle))
            {
                buttonCallBack(Design);
                AudioManager.instance.PlayUIClip("SelectButtonClick");
            }
        }
        else
        {
            if (GUI.Button(rect, ""))
            {
                buttonCallBack(Design);
                AudioManager.instance.PlayUIClip("SelectButtonClick");
            }
        }

        GUI.Label(NameRect, Name, GameManager.instance.largeLabelStyle);
        GUI.Label(CommandRect, Command, GameManager.instance.largeLabelStyle);
        GUI.Label(MoneyRect, Money, GameManager.instance.largeLabelStyle);
    }
}
