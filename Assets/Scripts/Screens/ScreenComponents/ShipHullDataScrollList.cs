using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipHullDataScrollList : BaseScrollList
{
    ShipHullDataButton selectedButton;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public void BuildShipHullDataButtons(List<ShipHullData> shipHullDatas, ShipHullDataButton.ButtonPress ChangeModuleSet)
    {
        Clear();
        selectedButton = null;

        foreach (ShipHullData hullData in shipHullDatas)
        {
            GameObject buttonObject = InstantiateEntry(hullData.Name);
            ShipHullDataButton newButton = buttonObject.GetComponent<ShipHullDataButton>();
            newButton.SetShipHullData(hullData, ChangeModuleSet, SetSetSelectionChange);

            if (selectedButton == null)
            {
                selectedButton = newButton;
                selectedButton.SetHighlight(true);
                ChangeModuleSet(hullData);
            }
        }
    }

    public void SetSetSelectionChange(ShipHullDataButton newSelectedButton)
    {
        if (selectedButton != newSelectedButton)
        {
            if (selectedButton != null)
            {
                selectedButton.SetHighlight(false);
            }

            selectedButton = newSelectedButton;
            selectedButton.SetHighlight(true);
        }
    }

    public ShipHullData GetSelectedShipHullData()
    {
        if(selectedButton != null)
        {
            selectedButton.GetShipHullData();
        }
        return null;
    }
}
