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
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
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
}
