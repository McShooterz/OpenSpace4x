/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes: Add to UI Image to get the icon
******************************************************************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconFetcher : MonoBehaviour
{
    [SerializeField]
    Image imageTarget;

    [SerializeField]
    [Tooltip("The name of the icon")]
    string iconName = "";

    // Use this for initialization
    void Start()
    {
        if (imageTarget != null)
        {
            imageTarget.sprite = ResourceManager.instance.GetIconTexture(iconName); ;
        }

        //Destroy Self
        Destroy(this);
    }
}
