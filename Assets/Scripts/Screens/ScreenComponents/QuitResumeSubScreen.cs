/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes: Base class for all controllable units in space battles
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class QuitResumeSubScreen
{
    Rect ResumeButtonRect;
    Rect OptionsButtonRect;
    Rect QuitButtonRect;

    bool Open = false;

    public delegate void QuitFunction();
    QuitFunction quitFunction;

    ScreenParent screenParent;  

    public QuitResumeSubScreen(ScreenParent parent, QuitFunction quit)
    {
        screenParent = parent;
        quitFunction = quit;

        Vector2 ButtonSize = new Vector2(Screen.width * 0.084f, Screen.height * 0.045f);
        float Indent = (Screen.width - ButtonSize.x) / 2f;

        ResumeButtonRect = new Rect(new Vector2(Indent, Screen.height * 0.4f), ButtonSize);
        OptionsButtonRect = new Rect(new Vector2(Indent, ResumeButtonRect.yMax), ButtonSize);
        QuitButtonRect = new Rect(new Vector2(Indent, OptionsButtonRect.yMax), ButtonSize);
    }

    public void Draw()
    {
        if(GUI.Button(ResumeButtonRect, "Resume"))
        {
            Open = false;
        }

        if(GUI.Button(OptionsButtonRect, "Options"))
        {
            Open = false;
            //GameManager.instance.ChangeScreen(new OptionsScreen(screenParent));
        }

        if (GUI.Button(QuitButtonRect, "Quit"))
        {
            quitFunction();
        }
    }

    public void SetOpen(bool state)
    {
        Open = state;
    }

    public bool isOpen()
    {
        return Open;
    }
}
