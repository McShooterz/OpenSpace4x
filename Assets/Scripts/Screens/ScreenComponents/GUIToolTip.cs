/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes: Used to display usesful information about hovered UI elements
Usage: Create in constructor of screen and call draw in that screens draw call. Has localization and optimization checks built in.
******************************************************************************************************************************************/

using UnityEngine;
using System;

public class GUIToolTip
{
    Rect ToolTipRect, TitleRect, BodyRect;
    string TitleText;
    string BodyText;
    float Width;
    Vector2 Anchor;

    string LastTitle = "";
    string LastBody = "";
    bool textSet = false;

    public GUIToolTip(Vector2 anchor, float width)
    {
        //Store arguments
        Anchor = anchor;
        Width = width;
    }

    public void SetText(string title, string body)
    {
        textSet = true;
        if (LastTitle != title || LastBody != body)
        {
            LastTitle = title;
            LastBody = body;

            TitleText = ResourceManager.GetLocalization(title);
            BodyText = ResourceManager.GetLocalization(body);

            GUIContent content = new GUIContent();

            //Determine Sizes
            float width = Width;
            float TitleHeight = 0;
            float BodyHeight = 0;
            float Height = 0;
            content.text = TitleText;
            TitleHeight = GameManager.instance.ToolTipTitleStyle.CalcHeight(content, width * 0.9f);
            content.text = BodyText;
            BodyHeight = GameManager.instance.ToolTipBodyStyle.CalcHeight(content, width * 0.9f);
            Height = (BodyHeight + TitleHeight) * 1.2f;

            //Build Rects
            ToolTipRect = new Rect(Anchor.x, Anchor.y, width, Height);
            width *= 0.9f;
            TitleRect = new Rect(Anchor.x + ToolTipRect.width * 0.05f, ToolTipRect.y + Height * 0.05f, width, TitleHeight);
            BodyRect = new Rect(TitleRect.x, TitleRect.yMax + Height * 0.05f, width, BodyHeight);
        }
    }

    public void Draw()
    {
        if (!textSet)
            return;
        else
            textSet = false;

        GUI.Box(ToolTipRect, "", GameManager.instance.standardBackGround);

        GUI.Label(TitleRect, TitleText, GameManager.instance.ToolTipTitleStyle);
        GUI.Label(BodyRect, BodyText, GameManager.instance.ToolTipBodyStyle);
    }
}
