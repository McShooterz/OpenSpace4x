using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDesignScrollList : BaseScrollList
{
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

    public void BuildShipDesignButtons(List<ShipDesign> shipDesigns, ShipDesignButton.ButtonPress ShipDesignButtonCallback, ShipDesignButton.DeleteButtonPress DeleteButtonCallBack)
    {
        Clear();

        foreach(ShipDesign shipDesign in shipDesigns)
        {
            GameObject buttonObject = InstantiateEntry(shipDesign.Name);
            ShipDesignButton newButton = buttonObject.GetComponentInChildren<ShipDesignButton>();
            newButton.SetShipDesign(shipDesign, ShipDesignButtonCallback, DeleteButtonCallBack);
        }
    }
}
