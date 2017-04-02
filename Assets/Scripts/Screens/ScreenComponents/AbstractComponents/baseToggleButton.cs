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

public class baseToggleButton : MonoBehaviour
{
    [SerializeField]
    protected Image Background;

    [SerializeField]
    protected Color HighlightColor = Color.black;

    [SerializeField]
    protected Color NormalColor = Color.black;

    // Use this for initialization
    protected virtual void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void SetHighlight(bool state)
    {
        if(state)
        {
            Background.color = HighlightColor;
        }
        else
        {
            Background.color = NormalColor;
        }
    }
}
