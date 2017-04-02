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
using UnityEngine.UI;

public class StatEntry : MonoBehaviour
{
    [SerializeField]
    Image Icon;

    [SerializeField]
    Text Label;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void SetImage(Sprite sprite)
    {
        Icon.overrideSprite = sprite;
    }

    public void SetText(string text)
    {
        Label.text = text;
    }
}
