/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections.Generic;

public abstract class CombatScreens : ShipSelectingScreens
{
    protected GameObject DummyUnit = null;
    protected GameObject PickedUpObject = null;

    protected MiniMap miniMap;
    protected ShipCombatInfoPanel shipInfoPanel;

    protected ShipDragSelectionBox shipDragSelectionBox;

    protected CombatCameraMover combatCameraMover;

    protected ShipManager PlayerShipManager;
    protected List<ShipManager> ShipManagers = new List<ShipManager>();

    protected Vector3 PivotPoint;
    protected Vector3 PickedUpOrigin;
    protected bool PivotSet = false;
    protected float DummyUnitRadius;
    protected GameObject PivotObject;
    protected List<GameObject> DirectionMarkers = new List<GameObject>();

    protected UnitCategory SelectedUnityCategory = UnitCategory.Ships;

    protected DesignDisplayWindow designDisplayWindow;

    protected void ContructDesignDisplayWindow()
    {
        designDisplayWindow = new DesignDisplayWindow(new Rect(Screen.width * 0.25f, Screen.height * 0.1f, Screen.width * 0.5f, Screen.height * 0.8f));
    }

    protected void CreateDummyShips()
    {
        if (DummyUnit != null)
        {
            Object.Destroy(DummyUnit);
        }
        DummyUnit = ResourceManager.instance.CreateShip(selectedDesign.Hull, Vector3.zero, Quaternion.identity);
        if (DummyUnit != null)
        {
            Bounds bounds = DummyUnit.GetComponentInChildren<MeshFilter>().mesh.bounds;
            DummyUnitRadius = bounds.max.magnitude;
            foreach (Collider collider in DummyUnit.GetComponentsInChildren<Collider>())
            {
                collider.enabled = false;
            }
        }
    }

    protected void CreateDummyStation()
    {
        if (DummyUnit != null)
        {
            Object.Destroy(DummyUnit);
        }
        DummyUnit = ResourceManager.instance.CreateStation(selectedStationDesign.Hull, Vector3.zero, Quaternion.identity);
        if (DummyUnit != null)
        {
            Bounds bounds = DummyUnit.GetComponentInChildren<MeshFilter>().mesh.bounds;
            DummyUnitRadius = bounds.max.magnitude;
            foreach (Collider collider in DummyUnit.GetComponentsInChildren<Collider>())
            {
                collider.enabled = false;
            }
        }
    }

    protected void CreateDummyFighter()
    {
        if (DummyUnit != null)
        {
            Object.Destroy(DummyUnit);
        }
        DummyUnit = ResourceManager.instance.CreateFighter(selectedFighter, Vector3.zero, Quaternion.identity);
        if (DummyUnit != null)
        {
            Bounds bounds = DummyUnit.GetComponentInChildren<MeshFilter>().mesh.bounds;
            DummyUnitRadius = bounds.max.magnitude;
            foreach (Collider collider in DummyUnit.GetComponentsInChildren<Collider>())
            {
                collider.enabled = false;
            }
        }
    }

    protected void DestroyAllShips()
    {
        DestroyDummyShip();
        foreach (ShipManager manager in ShipManagers)
        {
            manager.DestroyAllUnits();
        }
    }

    protected void UpdateShipManagers()
    {
        foreach (ShipManager manager in ShipManagers)
        {
            manager.Update();
        }
    }

    protected void SetPauseShipManagers(bool pause)
    {
        foreach (ShipManager manager in ShipManagers)
        {
            manager.SetPause(pause);
        }
    }

    protected void DestroyDummyShip()
    {
        if (DummyUnit != null)
        {
            Object.Destroy(DummyUnit);
        }
    }

    protected override void CloseScreen()
    {
        GameManager.instance.SetGameSpeed(1f);
    }

    protected enum UnitCategory
    {
        Ships,
        Stations,
        Fighters
    }

    public virtual void SetDesignWindowDesign(ShipDesign design)
    {
        designDisplayWindow.SetDesign(design);
        designDisplayWindow.SetOpen(true);
    }
}
