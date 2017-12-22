/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour 
{

	protected int HighlightLayer = 0;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void SetHighLightLayer(int layer)
	{
		HighlightLayer = layer;
	}

	public void ToggleHighlight(bool state)
	{
		if(state)
		{
			transform.GetChild(0).gameObject.layer = HighlightLayer;
		}
		else
		{
			transform.GetChild(0).gameObject.layer = 0;
		}
	}
}
