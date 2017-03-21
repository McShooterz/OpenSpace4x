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

public class ScreenManager : MonoBehaviour
{
    #region Variables
    public static ScreenManager instance;


    GameObject CurrentScreen;


    #endregion

    void Awake()
    {
        //Make a Singleton
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
        //This stays in every scene
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void ChangeScreen(string screenName)
    {
        GameObject newScreen = ResourceManager.GetScreenObject(screenName);
        if(newScreen != null)
        {
            if (CurrentScreen != null)
                Destroy(CurrentScreen);
            CurrentScreen = newScreen;
        }
        else
        {
            print("Screen not returned: " + screenName);
        }
    }
}
