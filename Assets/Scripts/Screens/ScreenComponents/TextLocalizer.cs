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

public class TextLocalizer : MonoBehaviour
{
    [Header("Must be placed on same UI GameObject with text component to localize")]
    [Header("")]

    [SerializeField]
    [Tooltip("Gets passed to localization system and sets the text to the localized value")]
    string Localization = "";

	// Use this for initialization
	void Start ()
    {
        //Sets the text
        Text textComponent = GetComponent<Text>();
        if(textComponent != null)
        {
            textComponent.text = ResourceManager.GetLocalization(Localization);
        }

        //Destroy Self
        Destroy(this);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
