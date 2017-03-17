/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class ModuleListEntry
{
    Rect rect;
    Rect imageRect;
    Module module;
    Texture2D texture;
    Vector2 TextureSize;

    public delegate void ButtonPress(Module mod);
    protected ButtonPress buttonCallBack;

    public ModuleListEntry(Rect r, Module mod, Texture2D tex, ButtonPress callback)
    {
        rect = r;
        module = mod;
        texture = tex;
        buttonCallBack = callback;

        TextureSize = new Vector2(texture.width, texture.height) * 0.667f;
        if (TextureSize.x >= rect.width || TextureSize.y >= rect.height)
        {
            float ratio;
            if (TextureSize.x > TextureSize.y)
            {
                ratio = rect.width / TextureSize.x * 0.94f;
            }
            else
            {
                ratio = rect.height / TextureSize.y * 0.94f;
            }

            TextureSize *= ratio;
        }
        Vector2 Position = new Vector2(rect.x + rect.width / 2 - TextureSize.x / 2, rect.y + rect.height / 2 - TextureSize.y / 2);
        imageRect = new Rect(Position, TextureSize);
    }

    public void Draw(Module selected)
    {
        if (selected == module)
        {
            if (GUI.Button(rect, "", GameManager.instance.SquareButtonGreenStyle))
            {
                buttonCallBack(module);
                AudioManager.instance.PlayUIClip("SelectButtonClick");
            }
        }
        else
        {
            if (GUI.Button(rect, ""))
            {
                buttonCallBack(module);
                AudioManager.instance.PlayUIClip("SelectButtonClick");
            }
        }
        GUI.DrawTexture(imageRect, texture, ScaleMode.ScaleToFit);
    }
}
