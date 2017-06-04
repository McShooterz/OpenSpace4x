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
    GameObject BehaviorGroup;

    [SerializeField]
    GameObject VisualGroup;

    [Header("Hulls Group")]
    [SerializeField]
    ShipHullDataScrollList shipHullDataScrollList;

    [SerializeField]
    ShipDesignScrollList shipDesignScrollList;

    // Use this for initialization
    void Start ()
    {
        HullsButtonClick();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void HullsButtonClick()
    {
        HullsGroup.SetActive(true);
        StatsGroup.SetActive(false);
        BehaviorGroup.SetActive(false);
        VisualGroup.SetActive(false);
    }

    public void StatsButtonClick()
    {
        HullsGroup.SetActive(false);
        StatsGroup.SetActive(true);
        BehaviorGroup.SetActive(false);
        VisualGroup.SetActive(false);
    }

    public void BehaviorButtonClick()
    {
        HullsGroup.SetActive(false);
        StatsGroup.SetActive(false);
        BehaviorGroup.SetActive(true);
        VisualGroup.SetActive(false);
    }

    public void VisualButtonClick()
    {
        HullsGroup.SetActive(false);
        StatsGroup.SetActive(false);
        BehaviorGroup.SetActive(false);
        VisualGroup.SetActive(true);
    }

    public void SaveDesignButtonClick()
    {

    }

    public void DeleteDesignButtonClick()
    {

    }


    public void SetShipHullDatas(List<ShipHullData> shipHullDatas, ShipHullDataButton.ButtonPress buttonPress)
    {
        shipHullDataScrollList.BuildShipHullDataButtons(shipHullDatas, buttonPress);
    }

    public void SetShipDesigns(List<ShipDesign> shipDesigns, ShipDesignButton.ButtonPress buttonPress, ShipDesignButton.DeleteButtonPress deleteButtonPress)
    {
        shipDesignScrollList.BuildShipDesignButtons(shipDesigns, buttonPress, deleteButtonPress);
    }
}
