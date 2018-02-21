/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class GameSpeedButton
{
    Rect baseRect;
    Texture2D Paused;
    Texture2D HalfSpeed;
    Texture2D NormalSpeed;
    Texture2D FastSpeed;
    Texture2D UltraSpeed;

    GUIToolTip ToolTip;

    public GameSpeedButton(Rect rect, GUIToolTip toolTip)
    {
        baseRect = rect;
        ToolTip = toolTip;
        //Paused = ResourceManager.instance.GetIconTexture("Icon_Pause");
        //HalfSpeed = ResourceManager.instance.GetIconTexture("Icon_HalfSpeed");
        //NormalSpeed = ResourceManager.instance.GetIconTexture("Icon_NormalSpeed");
        //FastSpeed = ResourceManager.instance.GetIconTexture("Icon_FastSpeed");
        //UltraSpeed = ResourceManager.instance.GetIconTexture("Icon_UltraSpeed");
    }

    public void Draw()
    {
        if (GameManager.instance.GetGameSpeed() == 1f)
        {
            if (GUI.Button(baseRect, NormalSpeed))
            {
                GameManager.instance.SetGameSpeed(2f);
            }
        }
        else if (GameManager.instance.GetGameSpeed() == 2f)
        {
            if (GUI.Button(baseRect, FastSpeed))
            {
                GameManager.instance.SetGameSpeed(4f);
            }
        }
        else if (GameManager.instance.GetGameSpeed() == 4f)
        {
            if (GUI.Button(baseRect, UltraSpeed))
            {
                GameManager.instance.SetGameSpeed(0f);
            }
        }
        else if (GameManager.instance.GetGameSpeed() == 0f)
        {
            if (GUI.Button(baseRect, Paused))
            {
                GameManager.instance.SetGameSpeed(0.5f);
            }
        }
        else
        {
            if (GUI.Button(baseRect, HalfSpeed))
            {
                GameManager.instance.SetGameSpeed(1f);
            }
        }
    }

    public bool Contains(Vector2 point)
    {
        if (baseRect.Contains(point))
        {
            return true;
        }
        return false;
    }

    public bool ToolTipCheck(Vector2 point)
    {
        if (baseRect.Contains(point))
        {
            if (GameManager.instance.GetGameSpeed() == 1f)
            {
                ToolTip.SetText("normalSpeed", "normalSpeedDesc");
            }
            else if (GameManager.instance.GetGameSpeed() == 2f)
            {
                ToolTip.SetText("doubleSpeed", "doubleSpeedDesc");
            }
            else if (GameManager.instance.GetGameSpeed() == 4f)
            {
                ToolTip.SetText("quadrupleSpeed", "quadrupleSpeedDesc");
            }
            else if (GameManager.instance.GetGameSpeed() == 0f)
            {
                ToolTip.SetText("paused", "pausedDesc");
            }
            else
            {
                ToolTip.SetText("halfSpeed", "halfSpeedDesc");
            }
            return true;
        }
        return false;
    }
}
