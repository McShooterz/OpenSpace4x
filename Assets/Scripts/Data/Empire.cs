/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes: Players are instances of the Empire class
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Empire
{
    //Variables
    public bool isPlayer = false;
    public List<ShipData> Ships = new List<ShipData>();
    public List<FleetData> Fleets = new List<FleetData>();

    public FleetManager fleetManager;

    public void Initialize()
    {
        //fleetManager = new FleetManager(this);
        //Testing create fleet
        //AddEmptyFleet(new Vector3(0,0,0));
    }

    public void AddShipData(string DesignName)
    {
        ShipData shipData = new ShipData(ResourceManager.shipDesignDatas[DesignName]);
        shipData.Owner = this;
        //Temp add galaxy class
        Ships.Add(shipData);
    }

    public void AddEmptyFleet(Vector3 Position)
    {
        FleetData FD = new FleetData();
        FD.Owner = this;
        fleetManager.CreateFleet(FD, Position);
    }
}
