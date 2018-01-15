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

public class Empire : MonoBehaviour
{
    [SerializeField]
    EmpireData empireData;

    [SerializeField]
    bool isPlayer = false;

    [SerializeField]
    List<ShipData> Ships = new List<ShipData>();

    [SerializeField]
    List<FleetData> Fleets = new List<FleetData>();

    [SerializeField]
    FleetManager fleetManager;

    [SerializeField]
    PlanetManager planetManager;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void AddShipData(string DesignName)
    {
        //ShipData shipData = new ShipData(ResourceManager.shipDesignDatas[DesignName]);
        //shipData.Owner = this;
        //Temp add galaxy class
        //Ships.Add(shipData);
    }

    public void AddEmptyFleet(Vector3 Position)
    {
        FleetData FD = new FleetData();
        FD.Owner = this;
        fleetManager.CreateFleet(FD, Position);
    }

    public FleetManager GetFleetManager()
    {
        return fleetManager;
    }

    public PlanetManager GetPlanetManager()
    {
        return planetManager;
    }
}
