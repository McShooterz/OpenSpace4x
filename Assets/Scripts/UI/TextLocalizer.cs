/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes: Add to UI text to get the localization
******************************************************************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextLocalizer : MonoBehaviour
{
    [SerializeField]
    Text textField;

    [SerializeField]
    [Tooltip("Gets passed to localization system and sets the text to the localized value")]
    string localization = "";

	// Use this for initialization
	void Start ()
    {
        if (textField != null)
        {
            textField.text = ResourceManager.instance.GetLocalization(localization);
        }

        //Destroy Self
        Destroy(this);
	}
}
