/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class EmpireFlag
{
    Rect baseRect;
    Rect innerRect;

    Texture2D Background;
    Texture2D Emblem;

    int EmblemIndex;
    int BackgroundIndex;

    Color BackgroundColor;
    Color EmblemColor;

    public EmpireFlag(Rect rect, int BackgroundIndex, int EmblemIndex, Color backgroundColor, Color emblemColor)
    {
        baseRect = rect;
        innerRect = new Rect(baseRect.x + baseRect.width * 0.05f, baseRect.y + baseRect.height * 0.05f, baseRect.width * 0.9f, baseRect.height * 0.9f);

        SetBackgroundTexture(BackgroundIndex);
        SetEmblemTexture(EmblemIndex);

        SetBackgroundColor(backgroundColor);
        SetEmblemColor(emblemColor);
    }

    public void Draw()
    {
        GUI.Box(baseRect, "");
        Color guiColor = GUI.color;
        GUI.color = BackgroundColor;
        GUI.DrawTexture(innerRect, Background);
        GUI.color = EmblemColor;
        GUI.DrawTexture(innerRect, Emblem);
        GUI.color = guiColor;
    }

    public int GetEmblemIndex()
    {
        return EmblemIndex;
    }

    public Color GetEmblemColor()
    {
        return EmblemColor;
    }

    public int GetBackgroundIndex()
    {
        return BackgroundIndex;
    }

    public Color GetBackgroundColor()
    {
        return BackgroundColor;
    }

    public void SetBackgroundTexture(int index)
    {
        BackgroundIndex = index;
        Background = ResourceManager.GetFlagBackground(BackgroundIndex);
    }

    public void SetEmblemTexture(int index)
    {
        EmblemIndex = index;
        Emblem = ResourceManager.GetFlagEmblem(EmblemIndex);
    }

    public void SetBackgroundColor(Color color)
    {
        BackgroundColor = color;
    }

    public void SetEmblemColor(Color color)
    {
        EmblemColor = color;
    }

    public void IncrementBackground()
    {
        SetBackgroundTexture(BackgroundIndex + 1);
    }

    public void DecrementBackground()
    {
        SetBackgroundTexture(BackgroundIndex - 1);
    }

    public void IncrementEmblem()
    {
        SetEmblemTexture(EmblemIndex + 1);
    }

    public void DecrementEmblem()
    {
        SetEmblemTexture(EmblemIndex - 1);
    }
}
