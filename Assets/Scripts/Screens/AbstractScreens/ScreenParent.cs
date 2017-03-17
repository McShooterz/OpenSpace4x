/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections.Generic;

public abstract class ScreenParent
{
    protected Vector2 mousePosition;
    protected GUIToolTip ToolTip;

    public virtual void Update(){}

    public abstract void Draw();


    protected void ResetCamera()
    {
        Camera.main.transform.rotation = GameManager.instance.startingCameraRotation;
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 30, Camera.main.transform.position.z);
    }

    protected void ResizeSquareViewListWindow(ref Rect viewRect, Rect windowRect, int count, float EntrySize, int RowCount)
    {
        viewRect.height = Mathf.Max(windowRect.height * 1.02f, EntrySize * (count / RowCount + (count % RowCount != 0 ? 1 : 0)));
    }

    protected void ResizeViewListWindow(ref Rect viewRect, Rect windowRect, int listCount, float EntrySize)
    {
        viewRect.height = Mathf.Max(windowRect.height * 1.02f, EntrySize * (listCount + 0.1f));
    }

    protected abstract void CloseScreen();

    protected void SetMousePosition()
    {
        mousePosition = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
    }

    public static string GetFormatedTime(float time)
    {
        return (Mathf.Floor(time / 60)).ToString("0") + ":" + (Mathf.Floor(time % 60)).ToString("00");
    }

    protected void PlayMainButtonClick()
    {
        AudioManager.instance.PlayUIClip("MainButtonClick");
    }
}
