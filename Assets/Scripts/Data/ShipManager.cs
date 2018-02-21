/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes: Manages ships for an empire during battle instances
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections.Generic;

public class ShipManager
{
    #region Veriables
    bool isPlayer;
    List<Ship> SelectedShips = new List<Ship>();
    List<Ship> Ships = new List<Ship>();
    List<Station> SelectedStations = new List<Station>();
    List<Station> Stations = new List<Station>();
	List<FighterWing> SelectedFighterWings = new List<FighterWing> ();
	List<FighterWing> FighterWings = new List<FighterWing> ();
    List<Fighter> Fighters = new List<Fighter>();
    List<Projectile> TargetableProjectiles = new List<Projectile>();
    List<Projectile> JammableProjectiles = new List<Projectile>();
    Ship UnOwnedSelectedShip;
    Station UnOwnedSelectedStation;
	FighterWing UnOwnedSelectedFighterWing;
    FleetData fleet;
    SpaceUnit enemyTarget;

    List<ShipManager> EnemyShipManagers = new List<ShipManager>();
    List<ShipManager> AlliedShipManagers = new List<ShipManager>();

    int highLightLayer;

    bool AIControl = false;
    bool isPaused = false;

    float AI_Timer = 0;

    //Abilities
    public bool TransporterTargetingTroop = false;
    public bool TransporterTargetingCrew = false;

    //Selection groups
    SelectionGroup[] SelectionGroups = new SelectionGroup[10];
    #endregion

    //Constructor
    public ShipManager(bool player, FleetData fleetData, int HighlightColor)
    {
        isPlayer = player;
        fleet = fleetData;
        highLightLayer = HighlightColor;
    }

    public void Update()
    {
        if(AIControl && EnemyShipManagers.Count > 0)
        {
            if (AI_Timer > 0)
            {
                AI_Timer -= Time.deltaTime;
            }
            else
            {
                AI_Timer = 1f;
                if (enemyTarget == null)
                {
                    enemyTarget = GetRandomEnemyTarget();
                    if(enemyTarget != null)
                        SetTargetForAllShips(enemyTarget);
                }
                else if (enemyTarget != null)
                {
                    if(enemyTarget.isDestroyed())
                    {
                        enemyTarget = null;
                    }
                    else if (enemyTarget is Ship)
                    {
                        if((enemyTarget as Ship).IsCloaked())
                            enemyTarget = null;
                    }
                }
            }
        }
    }

	public int GetHighLightLayer()
	{
		return highLightLayer;
	}

    public void AddToSelection(Ship ship)
    {
		if (ship.isDestroyed() || SelectedShips.Contains (ship))
			return;
        SelectedShips.Add(ship);
        if (isPlayer)
        {
            ship.ToggleHighlight(true);
            ship.ActivateOrdersLine();
            ship.SetMiniMapObjectColor(Color.white);
        }
    }

    public void AddToSelection(Station station)
    {
        if (station.isDestroyed() || SelectedStations.Contains(station))
            return;
        SelectedStations.Add(station);
        if (isPlayer)
        {
            station.ToggleHighlight(true);
            station.ActivateOrdersLine();
            station.SetMiniMapObjectColor(Color.white);
        }
    }

    public void AddToSelection(FighterWing fighterWing)
	{
		if (SelectedFighterWings.Contains (fighterWing))
			return;
		SelectedFighterWings.Add(fighterWing);
		if(isPlayer)
		{
			fighterWing.ToggleHighlight(true);
			fighterWing.ActivateOrdersLine();
            fighterWing.SetMiniMapObjectColor(Color.white);
        }
	}

    public void AddToSelection(Fighter fighter)
    {
        if (fighter.isDestroyed() || fighter.GetParentWing() == null)
            return;
		AddToSelection (fighter.GetParentWing());
    }

    public void AddToSelection(ShipHullData hull)
    {
        foreach(Ship ship in Ships)
        {
            if (ship.isDestroyed())
                continue;
            if (ship.GetHull() == hull)
                AddToSelection(ship);
        }
    }

    public void SelectUnownedShip(Ship ship)
    {
        DeselectShips();
        UnOwnedSelectedShip = ship;
        UnOwnedSelectedShip.ToggleHighlight(true);
    }

    public void SelectUnownedStation(Station station)
    {
        DeselectShips();
        UnOwnedSelectedStation = station;
        UnOwnedSelectedStation.ToggleHighlight(true);
    }

    public void SelectUnownedFighter(Fighter fighter)
    {
        DeselectShips();
		FighterWing wing = fighter.GetParentWing ();
		UnOwnedSelectedFighterWing = wing;
		UnOwnedSelectedFighterWing.ToggleHighlight(true);
    }

	public void SelectUnownedFighter(FighterWing fighterWing)
	{
		DeselectShips();
		UnOwnedSelectedFighterWing = fighterWing;
		UnOwnedSelectedFighterWing.ToggleHighlight(true);
	}

    public void DeselectShips()
    {
        foreach (Ship ship in SelectedShips)
        {
            ship.ToggleHighlight(false);
            ship.SetMiniMapObjectColor(GetHighlightColor());
        }
        SelectedShips.Clear();
        foreach(Station station in SelectedStations)
        {
            station.ToggleHighlight(false);
            station.SetMiniMapObjectColor(GetHighlightColor());
        }
        SelectedStations.Clear();
        foreach (FighterWing fighterWing in SelectedFighterWings)
        {
			fighterWing.ToggleHighlight(false);
            fighterWing.SetMiniMapObjectColor(GetHighlightColor());
        }
		SelectedFighterWings.Clear();
        if (UnOwnedSelectedShip != null)
        {
            UnOwnedSelectedShip.ToggleHighlight(false);
            UnOwnedSelectedShip = null;
        }
        else if(UnOwnedSelectedStation != null)
        {
            UnOwnedSelectedStation.ToggleHighlight(false);
            UnOwnedSelectedStation = null;
        }
		else if (UnOwnedSelectedFighterWing != null) 
		{
			UnOwnedSelectedFighterWing.ToggleHighlight (false);
			UnOwnedSelectedFighterWing = null;
		}

        ResetAbilities();
    }

    public void DeselectShip(Ship ship)
    {
        ship.ToggleHighlight(false);
        ship.SetMiniMapObjectColor(GetHighlightColor());
        SelectedShips.Remove(ship);
    }

    public void SelectAllShips()
    {
        SelectedShips.Clear();
		SelectedFighterWings.Clear ();
        foreach (Ship ship in Ships)
        {
            AddToSelection(ship);
        }
        foreach (Station station in Stations)
        {
            AddToSelection(station);
        }
        foreach (FighterWing fighterWing in FighterWings)
        {
            AddToSelection(fighterWing);
        }
    }

    public void SetGoalPositionsForShips(List<Vector3> points, Quaternion rotation)
    {
        int i = 0;
        Vector3 newPosition; 
        foreach (Ship ship in SelectedShips)
        {
            newPosition = points[i];
            newPosition.y = 1f;
            ship.SetNewGoalPosition(newPosition, rotation);
            if(i < points.Count - 1)
                i++;
        }
		foreach(FighterWing fighterWing in SelectedFighterWings)
        {
            newPosition = points[i];
            newPosition.y = 1f;
            fighterWing.SetNewGoalPosition(newPosition);
            if (i < points.Count - 1)
                i++;
        }
    }

    public void SetTargetForSelectedShips(SpaceUnit target)
    {
        bool enemy = isEnemy(target);
        foreach (Ship ship in SelectedShips)
        {
			if(ship != target)
            	ship.SetNewTarget(target, enemy); 
        }
		foreach(FighterWing fighterWing in SelectedFighterWings)
        {
			if(!fighterWing.OwnsUnit(target as Fighter))
				fighterWing.SetNewTarget(target, enemy);
        }
        if(enemy)
        {
            foreach (Station station in SelectedStations)
            {
                if (station != target)
                    station.SetNewTarget(target, enemy);
            }
        }
    }

    public void SetTargetForAllShips(SpaceUnit target)
    {
        bool enemy = isEnemy(target);
        foreach (Ship ship in Ships)
        {
            if (!ship.isDestroyed() && ship != target)
                ship.SetNewTarget(target, enemy);
        }
        foreach (FighterWing fighterWing in FighterWings)
        {
            if (!fighterWing.OwnsUnit(target as Fighter))
                fighterWing.SetNewTarget(target, enemy);
        }
        if (enemy)
        {
            foreach (Station station in Stations)
            {
                if (!station.isDestroyed() && station != target)
                    station.SetNewTarget(target, enemy);
            }
        }
    }

    public void SelectShipsInArea(Vector2 Start,Vector2 End)
    {
        Vector3 ScreenPoint1 = Camera.main.ScreenToViewportPoint(Start);
        Vector3 ScreenPoint2 = Camera.main.ScreenToViewportPoint(End);


        Vector3 Min = Vector3.Min(ScreenPoint1, ScreenPoint2);
        Vector3 Max = Vector3.Max(ScreenPoint1, ScreenPoint2);
        Min.z = Camera.main.nearClipPlane;
        Max.z = Camera.main.farClipPlane;

        Bounds ViewPortBounds = new Bounds();
        ViewPortBounds.SetMinMax(Min, Max);

        foreach (Ship ship in Ships)
        {
            if (ship.isDestroyed())
                continue;
            if(ViewPortBounds.Contains(Camera.main.WorldToViewportPoint(ship.GetPosition())))
            {
                AddToSelection(ship);
            }
        }
        foreach (Station station in Stations)
        {
            if (station.isDestroyed())
                continue;
            if (ViewPortBounds.Contains(Camera.main.WorldToViewportPoint(station.GetPosition())))
            {
                AddToSelection(station);
            }
        }
        foreach (FighterWing fighterWing in FighterWings)
        {
            if (ViewPortBounds.Contains(Camera.main.WorldToViewportPoint(fighterWing.GetPosition())))
            {
                AddToSelection(fighterWing);
            }
        }
    }

    public void AddSpaceUnit(SpaceUnit unit)
    {
        if(unit is Ship)
        {
            AddShip(unit as Ship);
        }
        else if(unit is Station)
        {
            AddStation(unit as Station);
        }
        else if(unit is Fighter)
        {
            AddFighter(unit as Fighter);
        }
    }

    void AddShip(Ship ship)
    {
        Ships.Add(ship);
        ship.SetShipManager(this);
        ship.SetHighLightLayer(highLightLayer);
        ship.AttachMiniMapObject();
        ship.SetPause(isPaused);
        if (AIControl)
        {
            if (enemyTarget != null)
                ship.SetNewTarget(enemyTarget, true);
            ship.LaunchFighters();
            ship.LaunchHeavyFighters();
            ship.LaunchAssaultPods();
        }
        else
        {
            AddToSelection(ship);
        }
        if(isPlayer)
        {
            ship.AddOrderLine();
        }
        else
        {
            ship.RemoveOrderLine();
        }
    }

    void AddStation(Station station)
    {
        Stations.Add(station);
        station.SetShipManager(this);
        station.SetHighLightLayer(highLightLayer);
        station.AttachMiniMapObject();
        station.SetPause(isPaused);
        if (AIControl)
        {
            if (enemyTarget != null)
                station.SetNewTarget(enemyTarget, true);
            station.LaunchFighters();
            station.LaunchHeavyFighters();
            station.LaunchAssaultPods();
        }
        else
        {
            AddToSelection(station);
        }
        if (isPlayer)
        {
            station.AddOrderLine();
        }
        else
        {
            station.RemoveOrderLine();
        }
    }

    public void AddFighter(Fighter fighter)
    {
        fighter.SetHighLightLayer(highLightLayer);
        Fighters.Add(fighter);
        fighter.SetShipManager(this);
        fighter.SetPause(isPaused);
        if(isPlayer)
        {
            fighter.AddOrderLine();
        }
        else
        {
            fighter.RemoveOrderLine();
        }
    }


	public void AddFighterWing(FighterWing fighterWing)
	{
		FighterWings.Add(fighterWing);
        fighterWing.SetShipManager(this);
        if (AIControl)
        {
            if (enemyTarget != null)
                fighterWing.SetNewTarget(enemyTarget, true);
        }
        else
        {
            AddToSelection(fighterWing);
        }
    }

    public void DeleteShip(Ship ship)
    {
        if (fleet != null)
            fleet.RemoveShip(ship.GetShipData());
        RemoveUnit(ship);
        Object.Destroy(ship.gameObject);
    }

    public void DeleteStation(Station station)
    {
        if (fleet != null)
            fleet.RemoveStation(station.stationData);
        RemoveUnit(station);
        Object.Destroy(station.gameObject);
    }

    public void RemoveFromSelection(SpaceUnit unit)
    {
        if (unit is Ship)
        {
            SelectedShips.Remove(unit as Ship);
            RemoveShipFromSelectionGroups(unit as Ship);
            foreach (ShipManager manager in EnemyShipManagers)
            {
                if (manager.UnOwnedSelectedShip == unit)
                    manager.UnOwnedSelectedShip = null;
            }
        }
        else if (unit is Station)
        {
            SelectedStations.Remove(unit as Station);
            RemoveStationFromSelectionGroups(unit as Station);
            foreach (ShipManager manager in EnemyShipManagers)
            {
                if (manager.UnOwnedSelectedStation == unit)
                    manager.UnOwnedSelectedStation = null;
            }
        }
    }

    public void RemoveUnit(SpaceUnit unit)
    {
        if(unit is Ship)
        {
            SelectedShips.Remove(unit as Ship);
            Ships.Remove(unit as Ship);
            RemoveShipFromSelectionGroups(unit as Ship);
        }
        else if(unit is Station)
        {
            SelectedStations.Remove(unit as Station);
            Stations.Remove(unit as Station);
            RemoveStationFromSelectionGroups(unit as Station);
        }
        else if(unit is Fighter)
        {
            Fighters.Remove(unit as Fighter);
        }
    }

    public void RemoveFighterWing(FighterWing fighterWing)
    {
        RemoveFighterWingFromSelectionGroups(fighterWing);
        SelectedFighterWings.Remove(fighterWing);
        FighterWings.Remove(fighterWing);
    }

    public void DestroyAllUnits()
    {
        foreach(Ship ship in Ships)
        {
            ship.GetShipData().DestroyWeaponEffects();
            Object.Destroy(ship.gameObject);
        }
        Ships.Clear();
        foreach (Station station in Stations)
        {
            station.stationData.DestroyWeaponEffects();
            Object.Destroy(station.gameObject);
        }
        Stations.Clear();
        foreach (Fighter fighter in Fighters)
        {
            fighter.DestroyWeaponEffects();
            Object.Destroy(fighter.gameObject);
        }
        Fighters.Clear();
        foreach(FighterWing wing in FighterWings)
        {
            Object.Destroy(wing.gameObject);
        }
        FighterWings.Clear();
    }

    public void SetPause(bool state)
    {
        isPaused = state;
        foreach(Ship ship in Ships)
        {
            ship.SetPause(state);
        }
        foreach (Station station in Stations)
        {
            station.SetPause(state);
        }
        foreach (Fighter fighter in Fighters)
        {
            fighter.SetPause(state);
        }
    }

    public void SetAI(bool state)
    {
        AIControl = state;
    }

    public int GetSelectedMobileUnitsCount()
    {
		return SelectedShips.Count + SelectedFighterWings.Count;
    }

    public List<Ship> GetSelectedShips()
    {
        return SelectedShips;
    }

    public List<Station> GetSelectedStations()
    {
        return SelectedStations;
    }

    public List<Fighter> GetFighterList()
    {
        return Fighters;
    }

    public List<FighterWing> GetSelectedFighters()
    {
        return SelectedFighterWings;
    }

    public bool HasUnownedShipSelected()
    {
        return UnOwnedSelectedShip != null;
    }

    public bool HasUnownedFighterSelected()
    {
        return UnOwnedSelectedFighterWing != null;
    }

    public Ship GetUnOwnedShip()
    {
        return UnOwnedSelectedShip;
    }

    public Station GetUnOwnedStation()
    {
        return UnOwnedSelectedStation;
    }

    public FighterWing GetUnOwnedFighterWing()
    {
		return UnOwnedSelectedFighterWing;
    }

    public void AddEnemyShipManager(ShipManager SM)
    {
        EnemyShipManagers.Add(SM);
    }

    public List<ShipManager> GetEnemyShipManagers()
    {
        return EnemyShipManagers;
    }

    public void AddAlliedShipManager(ShipManager SM)
    {
        AlliedShipManagers.Add(SM);
    }

    public bool isEnemy(SpaceUnit target)
    {
        ShipManager targetManager = target.GetShipManager();

        if (EnemyShipManagers.Contains(targetManager))
            return true;
        else
            return false;
    }

    public bool isEnemy(Projectile target)
    {
        ShipManager targetManager = target.GetShipManager();

        if (EnemyShipManagers.Contains(targetManager))
            return true;
        else
            return false;
    }

    public bool Player()
    {
        return isPlayer;
    }

	public void ShipKilled(Ship ship)
	{
		foreach (ShipManager manager in EnemyShipManagers) 
		{
			foreach (FighterWing fighterWing in manager.FighterWings) 
			{
				if (fighterWing.GetFirstFighter ().GetTarget () == ship.gameObject) 
				{
					fighterWing.CancelOrders ();
				}
			}
			foreach(Ship enemyShip in manager.Ships)
			{
                if (enemyShip.isDestroyed())
                    continue;
				if(enemyShip.GetTarget() == ship.gameObject)
				{
					enemyShip.CancelOrders();
				}
			}
		}
	}

	public void FighterKilled(Fighter fighter)
	{
		if (fighter.GetParentWing () != null && fighter.GetParentWing ().GetUnitCount () > 0) 
		{
			Fighter newFighterTarget = fighter.GetParentWing().GetFirstFighter();
			foreach (ShipManager manager in EnemyShipManagers) 
			{
				manager.ReplaceTarget (fighter, newFighterTarget);
			}
		} 
		else 
		{
			foreach (ShipManager manager in EnemyShipManagers) 
			{
				foreach (FighterWing fighterWing in manager.FighterWings) 
				{
					if (fighterWing.GetFirstFighter ().GetTarget () == fighter.gameObject) 
					{
						fighterWing.CancelOrders();
					}
				}
			}
		}							
	}

	public void ReplaceTarget(SpaceUnit oldTarget, SpaceUnit newTarget)
	{
		foreach (Ship ship in Ships) 
		{
            if (ship.isDestroyed())
                continue;
			if (ship.GetTarget () == oldTarget) 
			{
				ship.SetNewTarget (newTarget, true);
			}
		}
		foreach (FighterWing fighterWing in FighterWings) 
		{
			if(fighterWing.GetUnitCount() > 0 && fighterWing.GetFirstFighter().GetTarget() == oldTarget)
			{
				fighterWing.SetNewTarget (newTarget, true);
			}
		}
	}

    public void CaptureUnit(SpaceUnit unit)
    {
        if(unit is Ship)
        {
            CaptureShip(unit as Ship);
        }
        else if(unit is Station)
        {
            CaptureStation(unit as Station);
        }
    }

	void CaptureShip(Ship ship)
	{
        ship.CancelOrders();
        ship.UnParentFighterWings();
        if (UnOwnedSelectedShip == ship)
            UnOwnedSelectedShip = null;
        ship.GetShipManager().RemoveUnit(ship);
		ship.GetShipData().RemoveFromFleet();
		fleet.AddShip (ship.GetShipData());
		AddShip(ship);
	}

    void CaptureStation(Station station)
    {
        station.CancelOrders();
        station.UnParentFighterWings();
        if (UnOwnedSelectedStation == station)
            UnOwnedSelectedStation = null;
        station.GetShipManager().RemoveUnit(station);
        station.stationData.RemoveFromFleet();
        fleet.AddStation(station.stationData);
        AddStation(station);
    }

    public bool HasSomethingSelected()
    {
        if (SelectedShips.Count > 0)
            return true;
        if (SelectedStations.Count > 0)
            return true;
        if (SelectedFighterWings.Count > 0)
            return true;
        if (UnOwnedSelectedShip != null)
            return true;
        if (UnOwnedSelectedStation != null)
            return true;
        if (UnOwnedSelectedFighterWing != null)
            return true;
        return false;
    }

    public int SelectedCount()
    {
        return SelectedShips.Count + SelectedStations.Count + SelectedFighterWings.Count;
    }

    public void ResetAbilities()
    {
        TransporterTargetingTroop = false;
        TransporterTargetingCrew = false;
    }

    public bool AbilityActivite()
    {
        if (TransporterTargetingTroop || TransporterTargetingCrew)
            return true;
        return false;
    }

    public void CastActiveAbility(Ship targetShip)
    {
        if(TransporterTargetingTroop)
        {
            foreach(Ship ship in SelectedShips)
            {
                if(ship.CanTransportTroops())
                    ship.SetTransportTroopTarget(targetShip);
            }
        }
        else if(TransporterTargetingCrew)
        {
            foreach (Ship ship in SelectedShips)
            {
                if(ship.CanTransportCrew())
                    ship.SetTransportCrewTarget(targetShip);
            }
        }
        ResetAbilities();
    }

    public Color GetHighlightColor()
    {
        switch (highLightLayer)
        {
            case 9:
                {
                    return ResourceManager.instance.GetGameConstants().Highlight_Player.GetColor();
                }
            case 10:
                {
                    return ResourceManager.instance.GetGameConstants().Highlight_Enemy.GetColor();
                }
            case 11:
                {
                    return ResourceManager.instance.GetGameConstants().Highlight_Ally.GetColor();
                }
            case 13:
                {
                    return ResourceManager.instance.GetGameConstants().Highlight_Neutral.GetColor();
                }
            default:
                {
                    return Color.black;
                }
        }
    }

    public void HighLightAllShips(bool state)
    {
        foreach(Ship ship in Ships)
        {
            if (ship.isDestroyed())
                continue;
            ship.ToggleHighlight(state);
        }
    }

    public void StoreSelectionGroup(int index)
    {
        if (SelectionGroups[index] == null)
            SelectionGroups[index] = new SelectionGroup();
        else
        {
            SelectionGroups[index].ships.Clear();
            SelectionGroups[index].fighterWings.Clear();
        }
        foreach(Ship ship in SelectedShips)
        {
            SelectionGroups[index].ships.Add(ship);
        }
        foreach(FighterWing fighter in SelectedFighterWings)
        {
            SelectionGroups[index].fighterWings.Add(fighter);
        }
    }

    public void RecallSelectionGroup(int index)
    {
        if(SelectionGroups[index] != null)
        {
            foreach(Ship ship in SelectionGroups[index].ships)
            {
                if (ship != null)
                    AddToSelection(ship);
                else
                    SelectionGroups[index].ships.Remove(null);
            }
            foreach (FighterWing fighter in SelectionGroups[index].fighterWings)
            {
                if(fighter != null)
                    AddToSelection(fighter);
            }
        }
    }

    public bool NoUnitsLeft()
    {
        foreach(Ship ship in Ships)
        {
            if (!ship.isDestroyed())
                return false;
        }
        if (Stations.Count > 0)
        {
            return false;
        }
        if (FighterWings.Count == 0)
        {
            return false;
        }
        return true;
    }

    void RemoveShipFromSelectionGroups(Ship ship)
    {
        foreach (SelectionGroup group in SelectionGroups)
        {
            if (group != null)
                group.ships.Remove(ship);
        }
    }

    void RemoveStationFromSelectionGroups(Station station)
    {
        foreach (SelectionGroup group in SelectionGroups)
        {
            if (group != null)
                group.stations.Remove(station);
        }
    }

    void RemoveFighterWingFromSelectionGroups(FighterWing fighterWing)
    {
        foreach(SelectionGroup group in SelectionGroups)
        {
            if(group != null)
                group.fighterWings.Remove(fighterWing);
        }
    }

    public GameObject GetClosestEnemyTarget(Vector2 position)
    {
        GameObject closestEnemyTarget = null;
        float Distance = 99999f;
        foreach (ShipManager manager in EnemyShipManagers)
        {
            foreach (Ship ship in manager.Ships)
            {
                if (ship.isDestroyed())
                    continue;
                Vector2 positionShip = new Vector2(ship.transform.position.x, ship.transform.position.z);
                float distanceCheck = (positionShip - position).sqrMagnitude;
                if (distanceCheck < Distance)
                {
                    Distance = distanceCheck;
                    closestEnemyTarget = ship.gameObject;
                }
            }
            foreach (FighterWing fighterWing in manager.FighterWings)
            {
                Vector2 positionfighterWing = new Vector2(fighterWing.transform.position.x, fighterWing.transform.position.z);
                float distanceCheck = (positionfighterWing - position).sqrMagnitude;
                if (distanceCheck < Distance)
                {
                    Distance = distanceCheck;
                    closestEnemyTarget = fighterWing.GetFirstFighter().gameObject;
                }
            }
        }

        return closestEnemyTarget;
    }

    public List<TargetInfo> GetPotentialTargets(Vector2 Origin, float rangeSqr)
    {
        List<TargetInfo> potentialTargets = new List<TargetInfo>();
        foreach(ShipManager enemyManager in EnemyShipManagers)
        {
            foreach(Ship ship in enemyManager.Ships)
            {
                if(ship.CanBeTargeted())
                {
                    Vector2 direction = new Vector2(ship.transform.position.x - Origin.x, ship.transform.position.z - Origin.y);
                    float targetRange = direction.sqrMagnitude;
                    if (targetRange < rangeSqr)
                        potentialTargets.Add(new TargetInfo(ship, direction, targetRange));
                }
            }
            foreach (Station station in enemyManager.Stations)
            {
                if (station.CanBeTargeted())
                {
                    Vector2 direction = new Vector2(station.transform.position.x - Origin.x, station.transform.position.z - Origin.y);
                    float targetRange = direction.sqrMagnitude;
                    if (targetRange < rangeSqr)
                        potentialTargets.Add(new TargetInfo(station, direction, targetRange));
                }
            }
            foreach (Fighter fighter in enemyManager.Fighters)
            {
                if(fighter.CanBeTargeted())
                {
                    Vector2 direction = new Vector2(fighter.transform.position.x - Origin.x, fighter.transform.position.z - Origin.y);
                    float targetRange = direction.sqrMagnitude;
                    if (targetRange < rangeSqr)
                        potentialTargets.Add(new TargetInfo(fighter, direction, targetRange));
                }
            }
        }
        return potentialTargets;
    }

    public List<PDTargetInfo> GetPotentialPDTargets(Vector2 Origin, float rangeSqr)
    {
        List<PDTargetInfo> potentialTargets = new List<PDTargetInfo>();
        foreach(ShipManager enemyManager in EnemyShipManagers)
        {
            foreach(Projectile projectile in enemyManager.TargetableProjectiles)
            {
                if(!projectile.isJammed())
                {
                    Vector2 direction = new Vector2(projectile.transform.position.x - Origin.x, projectile.transform.position.z - Origin.y);
                    float targetRange = direction.sqrMagnitude;
                    if (targetRange < rangeSqr)
                        potentialTargets.Add(new PDTargetInfo(projectile, direction, targetRange));
                }
            }
        }
        return potentialTargets;
    }

    public void AddDPTargetableProjectile(Projectile projectile)
    {
        TargetableProjectiles.Add(projectile);
    }

    public void AddJammableProjectile(Projectile projectile)
    {
        JammableProjectiles.Add(projectile);
    }

    public void RemoveProjectile(Projectile projectile)
    {
        TargetableProjectiles.Remove(projectile);
        JammableProjectiles.Remove(projectile);
    }

    public Projectile GetClosestTargetableProjectile(Vector3 Position)
    {
        Projectile projectile = null;
        float MinDistance = 9999f;
        foreach(ShipManager manager in EnemyShipManagers)
        {
            foreach(Projectile targetable in manager.TargetableProjectiles)
            {
                if (!targetable.isJammed())
                {
                    float distance = (new Vector2(targetable.transform.position.x, targetable.transform.position.z) - new Vector2(Position.x, Position.z)).sqrMagnitude;
                    if (distance < MinDistance)
                    {
                        MinDistance = distance;
                        projectile = targetable;
                    }
                }
            }
        }
        return projectile;
    }

    public void JamEnemyProjectilesInRange(Vector3 Position, float RangeSqr, int Count)
    {
        foreach(ShipManager manager in EnemyShipManagers)
        {
            if (Count == 0)
                break;
            foreach(Projectile projectile in manager.JammableProjectiles)
            {
                float distance = (projectile.transform.position - Position).sqrMagnitude;
                if(distance < RangeSqr && !projectile.isJammed())
                {
                    projectile.Jam();
                    Count--;
                    if (Count == 0)
                        break;
                }
            }
        }
    }

    public SpaceUnit GetRandomEnemyTarget()
    {
        ShipManager enemyManager = GetRandomEnemyShipManager();
        if(enemyManager != null)
        {
            SpaceUnit target = enemyManager.GetRandomShip();
            if (target != null)
                return target;
            target = enemyManager.GetRandomStation();
            if (target != null)
                return target;
            target = enemyManager.GetRandomFighter();
            if (target != null)
                return target;
            return null;
        }
        return null;
    }

    public ShipManager GetRandomEnemyShipManager()
    {
        if(EnemyShipManagers.Count > 0)
        {
            return EnemyShipManagers[Random.Range(0, EnemyShipManagers.Count)];
        }
        return null;
    }

    public Ship GetRandomShip()
    {
        List<Ship> ValidShips = new List<Ship>();
        foreach (Ship ship in Ships)
        {
            if (ship.isDestroyed())
                continue;
            ValidShips.Add(ship);
        }

        if (ValidShips.Count > 0)
        {
            return ValidShips[Random.Range(0, ValidShips.Count)];
        }
        return null;
    }

    public Station GetRandomStation()
    {
        List<Station> ValidStations = new List<Station>();
        foreach(Station station in Stations)
        {
            if (station.isDestroyed())
                continue;
            ValidStations.Add(station);
        }

        if(ValidStations.Count > 0)
        {
            return ValidStations[Random.Range(0, ValidStations.Count)];
        }
        return null;
    }

    public Fighter GetRandomFighter()
    {
        if(FighterWings.Count > 0)
        {
            return FighterWings[Random.Range(0, FighterWings.Count)].GetFirstFighter();
        }
        return null;
    }

    public Sprite GetSelectionGroupIcon(int index)
    {
        if (SelectionGroups[index] != null)
            return SelectionGroups[index].GetIcon();
        return new Sprite();
    }

    class SelectionGroup
    {
        public List<Ship> ships = new List<Ship>();
        public List<Station> stations = new List<Station>();
        public List<FighterWing> fighterWings = new List<FighterWing>();

        public Sprite GetIcon()
        {
            if (ships.Count > 0)
                return ships[0].GetHull().GetIcon();
            if (stations.Count > 0)
                return stations[0].GetHull().GetIcon();
            if (fighterWings.Count > 0)
                return fighterWings[0].GetIcon();
            return new Sprite();
        }
    }
}
