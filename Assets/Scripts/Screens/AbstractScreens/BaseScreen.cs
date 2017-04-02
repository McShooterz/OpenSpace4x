/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseScreen : MonoBehaviour
{
    #region Variables
    protected Vector2 mousePosition;



    #endregion

    // Use this for initialization
    protected virtual void Start ()
    {
		
	}

    // Update is called once per frame
    protected virtual void Update ()
    {
		
	}

    public void ChangeScreen(string ScreenName)
    {
        ScreenManager.instance.ChangeScreen(ScreenName);
    }

    public void ChangeLastScreen()
    {
        ScreenManager.instance.ChangeLastScreen();
    }

    public void PlayButtonClick()
    {
        AudioManager.instance.PlayUIClip("MainButtonClick");
    }

    protected void SetMousePosition()
    {
        mousePosition = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
    }

    public static string GetFormatedTime(float time)
    {
        return (Mathf.Floor(time / 60)).ToString("0") + ":" + (Mathf.Floor(time % 60)).ToString("00");
    }
}
