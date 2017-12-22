/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FleetManager
{
    //Variables
    List<FleetController> Fleets = new List<FleetController>();
    List<FleetController> SelectedFleets = new List<FleetController>();
    Empire Owner;


	public FleetManager(Empire owner)
    {
        Owner = owner;
    }

    public void DeslectFleets()
    {
        foreach(FleetController fleet in SelectedFleets)
        {
            fleet.Highlight(false);
        }
        SelectedFleets.Clear();
    }

    public void AddToSelection(FleetController fleet)
    {
        SelectedFleets.Add(fleet);
        fleet.Highlight(true);
    }

    public void CreateFleet(FleetData FD, Vector3 Position)
    {
        GameObject fleet = ResourceManager.CreateFleet(Position, Quaternion.identity);
        FleetController FleetScript = fleet.GetComponent<FleetController>();
        FleetScript.SetFleetData(FD);
        Fleets.Add(FleetScript);
    }

    public void SetGoalPoint(Vector3 point)
    {
        foreach (FleetController fleet in SelectedFleets)
        {
            fleet.SetGoalPosition(point);
        }
    }

    public void SelectFleetsInArea(Vector3 Start, Vector3 End)
    {
        float MinX = Mathf.Min(Start.x, End.x);
        float MaxX = Mathf.Max(Start.x, End.x);
        float MinZ = Mathf.Min(Start.z, End.z);
        float MaxZ = Mathf.Max(Start.z, End.z);

        foreach (FleetController fleet in Fleets)
        {
            Vector3 Position = fleet.GetPosition();
            if (Position.x > MinX && Position.x < MaxX && Position.z > MinZ && Position.z < MaxZ)
            {
                AddToSelection(fleet);
            }
        }
    }
}
