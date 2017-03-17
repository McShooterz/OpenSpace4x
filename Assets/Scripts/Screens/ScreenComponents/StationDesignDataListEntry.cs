/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class StationDesignDataListEntry
{
    Rect rect;
    StationDesignData Design;
    string Name;
    public delegate void ButtonPress(StationDesignData design);
    protected ButtonPress buttonCallBack;

    GUIContent Money;

    Rect NameRect;
    Rect MoneyRect;

    public StationDesignDataListEntry(Rect rectangle, StationDesignData design, ButtonPress callback)
    {
        rect = rectangle;
        Design = design;
        buttonCallBack = callback;
        Name = Design.Design.Name + "  ";

        Money = new GUIContent();

        GameManager.instance.UIContent.image = null;
        GameManager.instance.UIContent.text = Name;

        Money.image = ResourceManager.GetIconTexture("Icon_Money");
        Money.text = ":" + Design.GetTotalValue().ToString("0.#");

        float nameWidthMax;
        float nameWidthMin;
        float moneyWidthMax;
        float moneyWidthMin;

        GameManager.instance.largeLabelStyle.CalcMinMaxWidth(GameManager.instance.UIContent, out nameWidthMin, out nameWidthMax);
        GameManager.instance.largeLabelStyle.CalcMinMaxWidth(Money, out moneyWidthMin, out moneyWidthMax);

        float indent = (rect.width - (nameWidthMax + moneyWidthMax)) / 2f;
        NameRect = new Rect(rect.x + indent, rect.y, nameWidthMax, rect.height);
        MoneyRect = new Rect(NameRect.xMax, rect.y, moneyWidthMax, rect.height);
    }

    public void Draw(StationDesignData selected)
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
        GUI.Label(MoneyRect, Money, GameManager.instance.largeLabelStyle);
    }
}
