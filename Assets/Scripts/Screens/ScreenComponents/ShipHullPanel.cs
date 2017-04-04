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

public class ShipHullPanel : MonoBehaviour
{


    [SerializeField]
    GameObject HullsGroup;

    [SerializeField]
    GameObject StatsGroup;

    [SerializeField]
    GameObject VisualGroup;




	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}








    public void HullsButtonClick()
    {
        HullsGroup.SetActive(true);
        StatsGroup.SetActive(false);
        VisualGroup.SetActive(false);
    }

    public void StatsButtonClick()
    {
        HullsGroup.SetActive(false);
        StatsGroup.SetActive(true);
        VisualGroup.SetActive(false);
    }

    public void VisualButtonClick()
    {
        HullsGroup.SetActive(false);
        StatsGroup.SetActive(false);
        VisualGroup.SetActive(true);
    }

    public void SaveDesignButtonClick()
    {

    }

    public void DeleteDesignButtonClick()
    {

    }

}
