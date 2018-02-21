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

public class FleetManager : MonoBehaviour
{
    //Variables
    [SerializeField]
    List<FleetController> fleets = new List<FleetController>();

    [SerializeField]
    List<FleetController> selectedFleets = new List<FleetController>();

    [SerializeField]
    FleetController selectedOtherFleet;

    [SerializeField]
    Empire Owner;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public List<FleetController> GetFleets()
    {
        return fleets;
    }

    public List<FleetController> GetSelectedFleets()
    {
        return selectedFleets;
    }

    public FleetController GetSelectedOtherFleet()
    {
        return selectedOtherFleet;
    }

    public void DeslectFleets()
    {
        foreach(FleetController fleet in selectedFleets)
        {
            fleet.ToggleLineRender(false);
        }
        selectedFleets.Clear();
        selectedOtherFleet = null;
    }

    public void AddToSelection(FleetController fleet)
    {
        if (!selectedFleets.Contains(fleet))
        {
            selectedFleets.Add(fleet);
            fleet.ToggleLineRender(true);
        }
    }

    public void SetSelectedOtherFleet(FleetController fleet)
    {
        selectedOtherFleet = fleet;
    }

    public void CreateFleet(FleetData FD, Vector3 Position)
    {
        GameObject fleet = ResourceManager.instance.CreateFleet(Position, Quaternion.identity);
        FleetController FleetScript = fleet.GetComponent<FleetController>();
        FleetScript.SetFleetData(FD);
        fleets.Add(FleetScript);
    }

    public void SetSelectedGoalPosition(Vector3 position)
    {
        foreach (FleetController fleet in selectedFleets)
        {
            fleet.SetGoalPosition(position);
        }
    }

    public void SelectFleetsInArea(Vector3 Start, Vector3 End)
    {
        float MinX = Mathf.Min(Start.x, End.x);
        float MaxX = Mathf.Max(Start.x, End.x);
        float MinZ = Mathf.Min(Start.z, End.z);
        float MaxZ = Mathf.Max(Start.z, End.z);

        foreach (FleetController fleet in fleets)
        {
            Vector3 Position = fleet.gameObject.transform.position;
            if (Position.x > MinX && Position.x < MaxX && Position.z > MinZ && Position.z < MaxZ)
            {
                AddToSelection(fleet);
            }
        }
    }

    public bool OwnsFleet(FleetController fleet)
    {
        return fleets.Contains(fleet);
    }
}
