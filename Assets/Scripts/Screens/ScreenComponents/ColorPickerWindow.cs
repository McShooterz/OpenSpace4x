/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class ColorPickerWindow
{
    Rect baseRect;

    Rect ColorSampleSpaceRect;

    Rect CurrentColorRect;

    Rect RedValueRect;
    Rect RedSliderRect;
    Rect GreenValueRect;
    Rect GreenSliderRect;
    Rect BlueValueRect;
    Rect BlueSliderRect;
    Rect AlphaValueRect;
    Rect AlphaSliderRect;

    Rect AcceptButtonRect;
    Rect CancelButtonRect;

    Color CurrentColor = Color.white;

    Texture2D ColorSpace;
    Texture2D CurrentColorTexture;

    public delegate void AcceptColor(Color selectedColor);
    AcceptColor acceptColorCallBack;

    bool Open = false;

    public ColorPickerWindow()
    {
        Vector2 Size = new Vector2(Screen.width * 0.25f, Screen.height * 0.45f);
        baseRect = new Rect(new Vector2((Screen.width - Size.x) / 2f, (Screen.height - Size.y) / 2f), Size);

        float Spacing = baseRect.height * 0.05f;

        ColorSampleSpaceRect = new Rect(baseRect.x + Spacing, baseRect.y + Spacing, baseRect.width * 0.5f, baseRect.height * 0.5f);

        CurrentColorRect = new Rect(ColorSampleSpaceRect.xMax + Spacing, ColorSampleSpaceRect.y, baseRect.width * 0.35f, baseRect.height * 0.35f);

        Spacing = baseRect.height * 0.025f;

        RedSliderRect = new Rect(ColorSampleSpaceRect.x, ColorSampleSpaceRect.yMax + Spacing, baseRect.width * 0.4f, GameManager.instance.StandardLabelSize.y);
        GreenSliderRect = new Rect(ColorSampleSpaceRect.x, RedSliderRect.yMax + Spacing, RedSliderRect.width, GameManager.instance.StandardLabelSize.y);
        BlueSliderRect = new Rect(ColorSampleSpaceRect.x, GreenSliderRect.yMax + Spacing, RedSliderRect.width, GameManager.instance.StandardLabelSize.y);
        AlphaSliderRect = new Rect(ColorSampleSpaceRect.x, BlueSliderRect.yMax + Spacing, RedSliderRect.width, GameManager.instance.StandardLabelSize.y);

        RedValueRect = new Rect(RedSliderRect.xMax + Spacing, RedSliderRect.y, GameManager.instance.StandardLabelSize.x, GameManager.instance.StandardLabelSize.y);
        GreenValueRect = new Rect(RedValueRect.x, GreenSliderRect.y, GameManager.instance.StandardLabelSize.x, GameManager.instance.StandardLabelSize.y);
        BlueValueRect = new Rect(RedValueRect.x, BlueSliderRect.y, GameManager.instance.StandardLabelSize.x, GameManager.instance.StandardLabelSize.y);
        AlphaValueRect = new Rect(RedValueRect.x, AlphaSliderRect.y, GameManager.instance.StandardLabelSize.x, GameManager.instance.StandardLabelSize.y);

        CancelButtonRect = new Rect(baseRect.xMax - GameManager.instance.StandardButtonSize.x - Spacing, baseRect.yMax - GameManager.instance.StandardButtonSize.y - Spacing, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);
        AcceptButtonRect = new Rect(CancelButtonRect.x, CancelButtonRect.y - GameManager.instance.StandardButtonSize.y - Spacing, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);

        //ColorSpace = ResourceManager.instance.GetUITexture("HSV_Space");
        CurrentColorTexture = new Texture2D(1, 1);
    }


    public void Draw(Vector2 mousePosition)
    {
        if(Input.GetMouseButton(0) && ColorSampleSpaceRect.Contains(mousePosition))
        {
            Vector2 RectPosition = new Vector2(mousePosition.x - ColorSampleSpaceRect.x, ColorSampleSpaceRect.yMax - mousePosition.y);

            int x = Mathf.FloorToInt(RectPosition.x / ColorSampleSpaceRect.width * ColorSpace.width);
            int y = Mathf.FloorToInt(RectPosition.y / ColorSampleSpaceRect.height * ColorSpace.height);

            CurrentColor = ColorSpace.GetPixel(x,y);
        }

        GUI.Box(baseRect, "",GameManager.instance.standardBackGround);

        GUI.DrawTexture(ColorSampleSpaceRect, ColorSpace);

        GUI.Box(RedSliderRect, "");
        CurrentColor.r = GUI.HorizontalSlider(RedSliderRect, CurrentColor.r, 0f, 1f);

        GUI.Box(GreenSliderRect, "");
        CurrentColor.g = GUI.HorizontalSlider(GreenSliderRect, CurrentColor.g, 0f, 1f);

        GUI.Box(BlueSliderRect, "");
        CurrentColor.b = GUI.HorizontalSlider(BlueSliderRect, CurrentColor.b, 0f, 1f);

        GUI.Box(AlphaSliderRect, "");
        CurrentColor.a = GUI.HorizontalSlider(AlphaSliderRect, CurrentColor.a, 0f, 1f);

        GUI.Label(RedValueRect, "R: " + (CurrentColor.r * 255).ToString("0.#"));

        GUI.Label(GreenValueRect, "G: " + (CurrentColor.g * 255).ToString("0.#"));

        GUI.Label(BlueValueRect, "B: " + (CurrentColor.b * 255).ToString("0.#"));

        GUI.Label(AlphaValueRect, "A: " + (CurrentColor.a * 255).ToString("0.#"));

        if(GUI.Button(CancelButtonRect, "Cancel"))
        {
            Open = false;
        }

        if(GUI.Button(AcceptButtonRect, "Accept"))
        {
            acceptColorCallBack(CurrentColor);
            Open = false;
        }

        Color oldColor = GUI.color;
        GUI.color = CurrentColor;

        GUI.DrawTexture(CurrentColorRect, CurrentColorTexture, ScaleMode.StretchToFill);

        GUI.color = oldColor;
    }

    public Color GetColor()
    {
        return CurrentColor;
    }

    public bool isOpen()
    {
        return Open;
    }

    public void OpenWindow(Color startingColor, AcceptColor acceptColorMethod)
    {
        Open = true;
        CurrentColor = startingColor;
        acceptColorCallBack = acceptColorMethod;
    }
}
