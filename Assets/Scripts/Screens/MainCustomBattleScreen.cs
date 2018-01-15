/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections.Generic;

public sealed class MainCustomBattleScreen : CombatScreens
{
    #region Variables

    bool SetupPhase = true;

    ShipManager EnemyShipManager;
    ShipManager AlliedShipManager;
    ShipManager NeutralShipManager;

    FleetData PlayerFleet;
    FleetData EnemyFleet;
    FleetData AlliedFleet;
    FleetData NeutralFleet;

    Rect ShipSelectionRect;

    Rect OwnerPlayerButtonRect;
    Rect OwnerEnemyButtonRect;
    Rect OwnerAlliedButtonRect;
    Rect OwnerNeutralButtonRect;

    Rect StartButtonRect;
    Rect SummaryButtonRect;

    GameSpeedButton gameSpeedButton;

    CombatTimer combatTimer;

    bool PlayerSelected = true;
    bool EnemySelected = false;
    bool AllySelected = false;
    bool NeutralSelected = false;

    FleetCombatInfoPanel fleetCombatInfoPanel;

    Rect ShipsCategoryButtonRect;
    Rect StationCategoryButtonRect;
    Rect FightersCategoryButtonRect;

    CombatStoredUnitPanel storedUnitPanel;

    DoubleClickDetector doubleClickDetector;

    CombatSummaryScrollList SummaryScrollList;

    QuitResumeSubScreen quitResumeSubScreen;

    #endregion

    public MainCustomBattleScreen()
    {
        quitResumeSubScreen = new QuitResumeSubScreen(this, CloseScreen);

        ShipSelectionRect = new Rect(0, 0, Screen.width * 0.25f, Screen.height * 0.65f);

        ShipsCategoryButtonRect = new Rect(ShipSelectionRect.x + ShipSelectionRect.width * 0.02f, ShipSelectionRect.y, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);
        StationCategoryButtonRect = new Rect(ShipSelectionRect.x + (ShipSelectionRect.width - GameManager.instance.StandardButtonSize.x) / 2f, ShipSelectionRect.y, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);
        FightersCategoryButtonRect = new Rect(ShipSelectionRect.xMax - (GameManager.instance.StandardButtonSize.x + ShipSelectionRect.width * 0.02f), ShipSelectionRect.y, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);

        Rect UnitListRect = new Rect(ShipSelectionRect.width * 0.05f, ShipsCategoryButtonRect.yMax + ShipSelectionRect.height * 0.01f, ShipSelectionRect.width * .9f, ShipSelectionRect.height * 0.25f);

        shipHullList = new ShipHullScrollList(UnitListRect, ChangeHull, CheckHullValid);
        stationHullList = new StationHullScrollList(UnitListRect, ChangeStationHull, CheckStationHullValid);
        fighterList = new FighterScrollList(new Rect(UnitListRect.x, UnitListRect.y, UnitListRect.width, UnitListRect.height * 2f), ChangeFighter, CheckFighterValid);

        DesignScrollWindowRect = new Rect(shipHullList.getRect().x, shipHullList.getRect().yMax + Screen.height * 0.025f, shipHullList.getRect().width, shipHullList.getRect().height);
        DesignScrollViewRect = new Rect(0, 0, DesignScrollWindowRect.width * 0.92f, DesignScrollWindowRect.height + 1);
        DesignScrollPostion = Vector2.zero;

        OwnerPlayerButtonRect = new Rect(ShipSelectionRect.width / 2 - GameManager.instance.StandardButtonSize.x / 2, DesignScrollWindowRect.yMax + GameManager.instance.StandardButtonSize.y, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);
        OwnerEnemyButtonRect = new Rect(OwnerPlayerButtonRect.x, OwnerPlayerButtonRect.yMax, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);
        OwnerAlliedButtonRect = new Rect(OwnerPlayerButtonRect.x, OwnerEnemyButtonRect.yMax, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);
        OwnerNeutralButtonRect = new Rect(OwnerPlayerButtonRect.x, OwnerAlliedButtonRect.yMax, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);

        float indent = (ShipSelectionRect.width - GameManager.instance.StandardButtonSize.x * 2f) / 3f;
        StartButtonRect = new Rect(indent, ShipSelectionRect.yMax - GameManager.instance.StandardButtonSize.y * 2, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);
        SummaryButtonRect = new Rect(StartButtonRect.xMax + indent, StartButtonRect.y, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);

        PlayerFleet = new FleetData();
        EnemyFleet = new FleetData();
        AlliedFleet = new FleetData();
        NeutralFleet = new FleetData();

        PlayerShipManager = new ShipManager(true, PlayerFleet, 9);
        EnemyShipManager = new ShipManager(false, EnemyFleet, 10);
        AlliedShipManager = new ShipManager(false, AlliedFleet, 11);
        NeutralShipManager = new ShipManager(false, NeutralFleet, 13);

        ShipManagers.Add(PlayerShipManager);
        ShipManagers.Add(EnemyShipManager);
        ShipManagers.Add(AlliedShipManager);
        ShipManagers.Add(NeutralShipManager);

        SetPauseShipManagers(true);

        EnemyShipManager.SetAI(true);
        AlliedShipManager.SetAI(true);

        //Set Combat diplomacy
        PlayerShipManager.AddEnemyShipManager(EnemyShipManager);
        PlayerShipManager.AddAlliedShipManager(AlliedShipManager);
        EnemyShipManager.AddEnemyShipManager(PlayerShipManager);
        EnemyShipManager.AddEnemyShipManager(AlliedShipManager);
        AlliedShipManager.AddEnemyShipManager(EnemyShipManager);
        AlliedShipManager.AddAlliedShipManager(PlayerShipManager);

        shipDragSelectionBox = new ShipDragSelectionBox();

        float toolTipWidth = Screen.width * 0.175f;
        ToolTip = new GUIToolTip(new Vector2(Screen.width - toolTipWidth, 0), toolTipWidth);

        Vector2 shipPanelSize = new Vector2(Screen.width * 0.5f, Screen.height * 0.25f);
        shipInfoPanel = new ShipCombatInfoPanel(this, new Rect(new Vector2((Screen.width - shipPanelSize.x) / 2, Screen.height - shipPanelSize.y), shipPanelSize), PlayerShipManager, ToolTip);

        float MiniMapSize = Screen.height * 0.2f;
        Rect miniMapRect = new Rect(Screen.width - Screen.height * 0.215f, Screen.height - Screen.height * 0.215f, MiniMapSize, MiniMapSize);
        miniMap = new MiniMap(miniMapRect, GameManager.instance.miniMapTexture, ToolTip);
        float GameSpeedButtonSize = miniMapRect.width / 5f;
        gameSpeedButton = new GameSpeedButton(new Rect(miniMapRect.x - GameSpeedButtonSize, miniMapRect.yMax - GameSpeedButtonSize, GameSpeedButtonSize, GameSpeedButtonSize), ToolTip);

        combatTimer = new CombatTimer(new Rect(Screen.width * 0.475f, 0, Screen.width * 0.05f, Screen.height * 0.03f), ToolTip);

        Rect fleetCombatInfoPanelRect = new Rect(0, Screen.height - shipPanelSize.y, shipPanelSize.y, shipPanelSize.y);
        fleetCombatInfoPanel = new FleetCombatInfoPanel(fleetCombatInfoPanelRect, PlayerFleet, GameManager.instance);

        shipHullList.CheckFirstHull(ChangeHull);

        doubleClickDetector = new DoubleClickDetector(0.25f);
        combatCameraMover = new CombatCameraMover();
        storedUnitPanel = new CombatStoredUnitPanel(PlayerShipManager, ToolTip);

        SummaryScrollList = new CombatSummaryScrollList();

        ContructDesignDisplayWindow();
    }

    // Update is called once per frame
    public override void Update()
    {
        combatCameraMover.Update();
        SetMousePosition();

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (SummaryScrollList.isOpen())
            {
                SummaryScrollList.SetOpen(false);
            }
            else if (designDisplayWindow.isOpen())
            {
                designDisplayWindow.SetOpen(false);
            }
            else
            {
                if (SetupPhase)
                {
                    if (quitResumeSubScreen.isOpen())
                        quitResumeSubScreen.SetOpen(false);
                    else
                        quitResumeSubScreen.SetOpen(true);
                }
                else
                {
                    SetPauseShipManagers(true);
                    SetupPhase = true;
                }
            }
        }

        if (SetupPhase)
        {
            if (Input.GetMouseButtonDown(1))
            {
                DeselectDesignData();
                if (PickedUpObject != null)
                {
                    PickedUpObject.transform.position = PickedUpOrigin;
                    PickedUpObject = null;
                }
            }

            if (Input.GetMouseButton(1))
            {
                Ray ray;
                RaycastHit hit;

                ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                //If we hit...
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    if (hit.collider.transform.root.tag == "Ship")
                    {
                        Ship ship = hit.collider.transform.root.GetComponent<Ship>();
                        ship.GetShipData().RemoveFromFleet();
                        ship.DestroyAllFighters();
                        ship.DestroyWeaponEffects();
                        ship.DeleteSelf();
                    }
                    if (hit.collider.transform.root.tag == "Station")
                    {
                        Station station = hit.collider.transform.root.GetComponent<Station>();
                        station.stationData.RemoveFromFleet();
                        station.DestroyAllFighters();
                        station.DeleteSelf();
                    }
                    else if (hit.collider.transform.root.tag == "Fighter")
                    {
                        Fighter fighter = hit.collider.transform.root.GetComponent<Fighter>();
                        fighter.GetParentWing().DestroySelf();
                    }
                    else if (hit.collider.transform.root.tag == "FighterWing")
                    {
                        hit.collider.transform.root.GetComponent<FighterWing>().DestroySelf();
                    }
                }
            }
            else if (DummyUnit != null && !ScreenElementsContains(mousePosition))
            {
                Ray ray;
                RaycastHit hit;

                ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                //If we hit...
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    if (isPositionInPlayArea(hit.point))
                    {
                        if (!PivotSet)
                            DummyUnit.transform.position = new Vector3(hit.point.x, 1f, hit.point.z);
                        if (Input.GetMouseButtonDown(0))
                        {
                            PivotPoint = DummyUnit.transform.position;
                            PivotSet = true;
                        }
                        else if (Input.GetMouseButton(0))
                        {
                            Vector3 Direction = (new Vector3(hit.point.x, 1f, hit.point.z) - PivotPoint);
                            if (Direction.magnitude > DummyUnitRadius)
                            {
                                Quaternion Rotation = Quaternion.LookRotation(Direction);
                                DummyUnit.transform.rotation = Rotation;
                            }
                        }
                        else if (Input.GetMouseButtonUp(0))
                        {
                            if (SelectedUnityCategory == UnitCategory.Ships && selectedDesign != null)
                            {
                                ShipData shipData = new ShipData(selectedDesign);
                                Ship newShip = ResourceManager.instance.CreateCombatShip(shipData, DummyUnit.transform.position, DummyUnit.transform.rotation);
                                if (newShip != null)
                                {
                                    if (PlayerSelected)
                                    {
                                        PlayerShipManager.AddSpaceUnit(newShip);
                                        PlayerFleet.AddShip(newShip.GetShipData());
                                    }
                                    else if (EnemySelected)
                                    {
                                        EnemyShipManager.AddSpaceUnit(newShip);
                                        EnemyFleet.AddShip(newShip.GetShipData());
                                    }
                                    else if (AllySelected)
                                    {
                                        AlliedShipManager.AddSpaceUnit(newShip);
                                        AlliedFleet.AddShip(newShip.GetShipData());
                                    }
                                    else if (NeutralSelected)
                                    {
                                        NeutralShipManager.AddSpaceUnit(newShip);
                                        NeutralFleet.AddShip(newShip.GetShipData());
                                    }
                                }
                            }
                            else if (SelectedUnityCategory == UnitCategory.Stations && selectedStationDesign != null)
                            {
                                StationData stationData = new StationData(selectedStationDesign);
                                Station newStation = ResourceManager.instance.CreateCombatStation(stationData, DummyUnit.transform.position, DummyUnit.transform.rotation);
                                if (PlayerSelected)
                                {
                                    PlayerShipManager.AddSpaceUnit(newStation);
                                    PlayerFleet.AddStation(newStation.stationData);
                                }
                                else if (EnemySelected)
                                {
                                    EnemyShipManager.AddSpaceUnit(newStation);
                                    EnemyFleet.AddStation(newStation.stationData);
                                }
                                else if (AllySelected)
                                {
                                    AlliedShipManager.AddSpaceUnit(newStation);
                                    AlliedFleet.AddStation(newStation.stationData);
                                }
                                else if (NeutralSelected)
                                {
                                    NeutralShipManager.AddSpaceUnit(newStation);
                                    NeutralFleet.AddStation(newStation.stationData);
                                }
                            }
                            else if (SelectedUnityCategory == UnitCategory.Fighters && selectedFighter != null)
                            {
                                FighterWing wing = ResourceManager.CreateFighterWing();
                                Fighter newFighter = ResourceManager.instance.CreateCombatFighter(selectedFighter, DummyUnit.transform.position, DummyUnit.transform.rotation);
                                wing.addFighter(newFighter);
                                List<Fighter> createdFighters = new List<Fighter>();
                                createdFighters.Add(newFighter);
                                for (int i = 1; i < selectedFighter.MaxSquadronSize; i++)
                                {
                                    Vector3 randomOffect = new Vector3(DummyUnit.transform.position.x + Random.Range(-0.5f, 0.5f), DummyUnit.transform.position.y + Random.Range(0.1f, 0.6f), DummyUnit.transform.position.z + Random.Range(-0.5f, 0.5f));
                                    newFighter = ResourceManager.instance.CreateCombatFighter(selectedFighter, randomOffect, DummyUnit.transform.rotation);
                                    wing.addFighter(newFighter);
                                    createdFighters.Add(newFighter);
                                }
                                if (PlayerSelected)
                                {
                                    PlayerShipManager.AddFighterWing(wing);
                                    foreach (Fighter fighter in createdFighters)
                                    {
                                        PlayerShipManager.AddSpaceUnit(fighter);
                                    }
                                }
                                else if (EnemySelected)
                                {
                                    EnemyShipManager.AddFighterWing(wing);
                                    foreach (Fighter fighter in createdFighters)
                                    {
                                        EnemyShipManager.AddSpaceUnit(fighter);
                                    }
                                }
                                else if (AllySelected)
                                {
                                    AlliedShipManager.AddFighterWing(wing);
                                    foreach (Fighter fighter in createdFighters)
                                    {
                                        AlliedShipManager.AddSpaceUnit(fighter);
                                    }
                                }
                                else if (NeutralSelected)
                                {
                                    NeutralShipManager.AddFighterWing(wing);
                                    foreach (Fighter fighter in createdFighters)
                                    {
                                        NeutralShipManager.AddSpaceUnit(fighter);
                                    }
                                }
                                wing.SetNewGoalPosition(DummyUnit.transform.position);
                            }
                            PivotSet = false;
                        }
                    }
                }
            }
            else if (PickedUpObject != null && !ScreenElementsContains(mousePosition))
            {
                Ray ray;
                RaycastHit hit;
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                //If we hit...
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    if (isPositionInPlayArea(hit.point))
                    {
                        if (!PivotSet)
                            PickedUpObject.transform.position = new Vector3(hit.point.x, 1f, hit.point.z);
                        if (Input.GetMouseButtonDown(0))
                        {
                            PivotPoint = PickedUpObject.transform.position;
                            PivotSet = true;
                        }
                        else if (Input.GetMouseButton(0))
                        {
                            Vector3 Direction = (new Vector3(hit.point.x, 1f, hit.point.z) - PivotPoint);
                            if (Direction.magnitude > DummyUnitRadius)
                            {
                                Quaternion Rotation = Quaternion.LookRotation(Direction);
                                PickedUpObject.transform.rotation = Rotation;
                            }
                        }
                        else if (Input.GetMouseButtonUp(0))
                        {
                            PickedUpObject = null;
                            PivotSet = false;
                        }
                    }
                }
            }
            else if (miniMap.Contains(mousePosition))
            {
                if (Input.GetMouseButton(0))
                {
                    combatCameraMover.GoTo(miniMap.GetWorldPosition(mousePosition));
                }
            }
            else if (DummyUnit == null)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    Ray ray;
                    RaycastHit hit;

                    ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    //If we hit...
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    {
                        if (hit.collider.transform.root.tag == "Ship" || hit.collider.transform.root.tag == "Station")
                        {
                            PickedUpObject = hit.collider.transform.root.gameObject;
                            PickedUpOrigin = hit.collider.transform.root.transform.position;
                        }
                    }
                }
            }
        }
        else if(!designDisplayWindow.isOpen())
        {
            //Update the ships managers
            UpdateShipManagers();

            combatTimer.CountUp();

            doubleClickDetector.Update();

            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

            storedUnitPanel.Update();

            if (!ScreenElementsContains(mousePosition))
            {
                Ray ray;
                RaycastHit hit;
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    //Highlight hovered units
                    if (hit.collider.transform.root.tag == "Ship" || hit.collider.transform.root.tag == "Station")
                    {
                        hit.collider.transform.root.GetComponent<SpaceUnit>().Hovered();
                    }
                    else if (hit.collider.transform.root.tag == "Fighter")
                    {
                        hit.collider.transform.root.GetComponent<Fighter>().GetParentWing().Hovered();
                    }
                    else if (hit.collider.transform.root.tag == "FighterWing")
                    {
                        hit.collider.transform.root.GetComponent<FighterWing>().Hovered();
                    }
                    //Process abilities
                    if (PlayerShipManager.AbilityActivite())
                    {
                        if (hit.collider.transform.root.tag == "Ship")
                        {
                            Ship hoveredShip = hit.collider.transform.root.GetComponent<Ship>();
                            if (hoveredShip.ValidTransporterTarget(PlayerShipManager))
                            {
                                if (PlayerShipManager.TransporterTargetingTroop)
                                {
                                    Cursor.SetCursor(ResourceManager.instance.GetIconTexture("Icon_TransportTroop"), Vector2.zero, CursorMode.Auto);
                                    if (Input.GetMouseButtonDown(0))
                                        PlayerShipManager.CastActiveAbility(hoveredShip);
                                }
                                else if (PlayerShipManager.TransporterTargetingCrew)
                                {
                                    Cursor.SetCursor(ResourceManager.instance.GetIconTexture("Icon_TransportCrew"), Vector2.zero, CursorMode.Auto);
                                    if (Input.GetMouseButtonDown(0))
                                        PlayerShipManager.CastActiveAbility(hoveredShip);
                                }
                            }
                        }
                        if (Input.GetMouseButtonDown(1))
                            PlayerShipManager.ResetAbilities();
                    }
                    else //Normal movement and commands
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            //Cancel movement order
                            if (PivotSet)
                            {
                                PivotSet = false;
                                Object.Destroy(PivotObject);
                                foreach (GameObject marker in DirectionMarkers)
                                {
                                    Object.Destroy(marker);
                                }
                                DirectionMarkers.Clear();
                            }
                            //Check if clicked a ship
                            if (hit.collider.transform.root.tag == "Ship")
                            {
                                Ship ship = hit.collider.transform.root.GetComponent<Ship>();
                                if (ship.GetShipManager() == PlayerShipManager)
                                {
                                    if (!Input.GetKey(KeyCode.LeftShift))
                                        PlayerShipManager.DeselectShips();
                                    if(doubleClickDetector.GetClicks() > 1)
                                    {
                                        PlayerShipManager.AddToSelection(ship.GetHull());
                                    }
                                    else
                                        PlayerShipManager.AddToSelection(ship);
                                    combatCameraMover.SetFollowTarget(ship.gameObject);
                                }
                                else
                                {
                                    PlayerShipManager.SelectUnownedShip(ship);
                                }
                            }
                            else if (hit.collider.transform.root.tag == "Station")
                            {
                                Station station = hit.collider.transform.root.GetComponent<Station>();
                                if (station.GetShipManager() == PlayerShipManager)
                                {
                                    if (!Input.GetKey(KeyCode.LeftShift))
                                        PlayerShipManager.DeselectShips();
                                    PlayerShipManager.AddToSelection(station);
                                }
                                else
                                {
                                    PlayerShipManager.SelectUnownedStation(station);
                                }
                            }
                            else if (hit.collider.transform.root.tag == "FighterWing")
                            {
                                FighterWing fighterWing = hit.collider.transform.root.GetComponent<FighterWing>();
                                if (fighterWing.GetShipManager() == PlayerShipManager)
                                {
                                    if (!Input.GetKey(KeyCode.LeftShift))
                                        PlayerShipManager.DeselectShips();
                                    PlayerShipManager.AddToSelection(fighterWing);
                                }
                                else
                                {
                                    PlayerShipManager.SelectUnownedFighter(fighterWing);
                                }
                            }
                            else if (hit.collider.transform.root.tag == "Fighter")
                            {
                                Fighter fighter = hit.collider.transform.root.GetComponent<Fighter>();
                                if (fighter.GetShipManager() == PlayerShipManager)
                                {
                                    if (!Input.GetKey(KeyCode.LeftShift))
                                        PlayerShipManager.DeselectShips();
                                    PlayerShipManager.AddToSelection(fighter);
                                }
                                else
                                {
                                    PlayerShipManager.SelectUnownedFighter(fighter);
                                }
                            }
                            else if (hit.collider.gameObject.tag == "PlayArea")
                            {
                                if (!Input.GetKey(KeyCode.LeftShift))
                                    PlayerShipManager.DeselectShips();
                                shipDragSelectionBox.SetStart(Input.mousePosition);
                                shipDragSelectionBox.SetActive(true);
                            }
                        }
                        else if (Input.GetMouseButtonDown(1) && PlayerShipManager.HasSomethingSelected())
                        {
                            if (hit.collider.gameObject.tag == "PlayArea")
                            {
                                if (isPositionInPlayArea(hit.point))
                                {
                                    PivotSet = true;
                                    PivotObject = new GameObject();
                                    PivotObject.transform.position = hit.point;
                                    Vector3 shipGroupCenter;
                                    Vector3 sumVector = Vector3.zero;
                                    foreach (Ship ship in PlayerShipManager.GetSelectedShips())
                                    {
                                        sumVector += ship.transform.position;
                                    }
                                    foreach (FighterWing fighterWing in PlayerShipManager.GetSelectedFighters())
                                    {
                                        sumVector += fighterWing.transform.position;
                                    }
                                    shipGroupCenter = sumVector / PlayerShipManager.GetSelectedMobileUnitsCount();
                                    Vector3 Direction = PivotObject.transform.position - shipGroupCenter;
                                    if (Direction != Vector3.zero)
                                    {
                                        Quaternion rotation = Quaternion.LookRotation(Direction, Vector3.up);
                                        PivotObject.transform.rotation = rotation;
                                        foreach (Ship ship in PlayerShipManager.GetSelectedShips())
                                        {
                                            Vector3 markerPosition = ship.transform.position + Direction;
                                            //Try to keep ships 2 units apart
                                            foreach(GameObject markers in DirectionMarkers)
                                            {
                                                Vector3 otherMarkerDirection = markerPosition - markers.transform.position;
                                                if (otherMarkerDirection.sqrMagnitude < 4f)
                                                {
                                                    markerPosition = markers.transform.position + otherMarkerDirection.normalized * 2f;
                                                }
                                            }
                                            GameObject marker = ResourceManager.CreateDirectionMarker(markerPosition);
                                            marker.transform.rotation = rotation;
                                            DirectionMarkers.Add(marker);
                                            //marker.transform.parent = PivotObject.transform;
                                        }
                                        foreach (FighterWing fighterWing in PlayerShipManager.GetSelectedFighters())
                                        {
                                            GameObject marker = ResourceManager.CreateDirectionMarker(fighterWing.transform.position + Direction);
                                            marker.transform.rotation = rotation;
                                            DirectionMarkers.Add(marker);
                                            //marker.transform.parent = PivotObject.transform;
                                        }
                                    }
                                }
                            }
                            else if (hit.collider.transform.root.tag == "Ship")
                            {
                                Ship ship = hit.collider.transform.root.GetComponent<Ship>();
                                PlayerShipManager.SetTargetForSelectedShips(ship);
                            }
                            else if (hit.collider.transform.root.tag == "Station")
                            {
                                Station station = hit.collider.transform.root.GetComponent<Station>();
                                PlayerShipManager.SetTargetForSelectedShips(station);
                            }
                            else if (hit.collider.transform.root.tag == "FighterWing")
                            {
                                FighterWing fighterWing = hit.collider.transform.root.GetComponent<FighterWing>();
                                PlayerShipManager.SetTargetForSelectedShips(fighterWing.GetFirstFighter());
                            }
                            else if (hit.collider.transform.root.tag == "Fighter")
                            {
                                Fighter fighter = hit.collider.transform.root.GetComponent<Fighter>();
                                PlayerShipManager.SetTargetForSelectedShips(fighter);
                            }
                        }
                        else if (Input.GetMouseButton(1) && PivotSet)
                        {
                            if (hit.collider.gameObject.tag == "PlayArea")
                            {
                                Vector3 Direction = hit.point - PivotObject.transform.position;
                                if (Direction.sqrMagnitude > 1f)
                                {
                                    Quaternion rotation = Quaternion.LookRotation(Direction, Vector3.up);
                                    PivotObject.transform.rotation = rotation;
                                    foreach (GameObject marker in DirectionMarkers)
                                    {
                                        marker.transform.rotation = rotation;
                                    }
                                }
                            }
                        }
                        else if (PivotSet)
                        {
                            if (hit.collider.gameObject.tag == "PlayArea")
                            {
                                List<Vector3> points = new List<Vector3>();
                                foreach (GameObject marker in DirectionMarkers)
                                {
                                    points.Add(marker.transform.position);
                                }
                                PlayerShipManager.SetGoalPositionsForShips(points, PivotObject.transform.rotation);

                                PivotSet = false;
                                Object.Destroy(PivotObject);
                                foreach (GameObject marker in DirectionMarkers)
                                {
                                    Object.Destroy(marker);
                                }
                                DirectionMarkers.Clear();
                            }
                        }
                    }
                }

                //Drag left click
                if (Input.GetMouseButton(0))
                {
                    shipDragSelectionBox.SetEnd(Input.mousePosition);
                }
                else if (shipDragSelectionBox.isActive())
                {
                    //Deselect ships from before
                    if (!Input.GetKey(KeyCode.LeftShift))
                        PlayerShipManager.DeselectShips();
                    shipDragSelectionBox.SetActive(false);
                    shipDragSelectionBox.SelectShips(PlayerShipManager);
                }
            }
            else if (miniMap.Contains(mousePosition))
            {
                if (Input.GetMouseButton(0))
                {
                    combatCameraMover.GoTo(miniMap.GetWorldPosition(mousePosition));
                }
            }
        }
    }

    public override void Draw()
    {
        if(quitResumeSubScreen.isOpen())
        {
            quitResumeSubScreen.Draw();
        }
        else if (SummaryScrollList.isOpen())
        {
            SummaryScrollList.Draw();
        }
        else if (designDisplayWindow.isOpen())
        {
            designDisplayWindow.Draw();
        }
        else
        {

            miniMap.Draw();
            shipInfoPanel.Draw(mousePosition);
            fleetCombatInfoPanel.Draw();
            gameSpeedButton.Draw();
            combatTimer.Draw();

            if (SetupPhase)
            {
                GUI.Box(ShipSelectionRect, "", GameManager.instance.standardBackGround);

                if (GUI.Button(ShipsCategoryButtonRect, "Ships", GameManager.instance.standardButtonStyle))
                {
                    ChangeUnitCategory(UnitCategory.Ships);
                    PlayMainButtonClick();
                }
                if (GUI.Button(StationCategoryButtonRect, "Stations", GameManager.instance.standardButtonStyle))
                {
                    ChangeUnitCategory(UnitCategory.Stations);
                    PlayMainButtonClick();
                }
                if (GUI.Button(FightersCategoryButtonRect, "Fighters", GameManager.instance.standardButtonStyle))
                {
                    ChangeUnitCategory(UnitCategory.Fighters);
                    PlayMainButtonClick();
                }

                if (SelectedUnityCategory == UnitCategory.Ships)
                {
                    shipHullList.Draw();

                    DesignScrollPostion = GUI.BeginScrollView(DesignScrollWindowRect, DesignScrollPostion, DesignScrollViewRect);
                    foreach (DesignDataListEntry entry in DesignDataList)
                    {
                        entry.Draw(selectedDesign);
                    }
                    GUI.EndScrollView();
                }
                else if (SelectedUnityCategory == UnitCategory.Stations)
                {
                    stationHullList.Draw();

                    DesignScrollPostion = GUI.BeginScrollView(DesignScrollWindowRect, DesignScrollPostion, DesignScrollViewRect);
                    foreach (StationDesignDataListEntry entry in StationDesignDataList)
                    {
                        entry.Draw(selectedStationDesign);
                    }
                    GUI.EndScrollView();
                }
                else
                {
                    fighterList.Draw();
                }

                //Owner Toggles
                if (GUI.Toggle(OwnerPlayerButtonRect, PlayerSelected, "Player"))
                {
                    if (!PlayerSelected)
                    {
                        PlayerSelected = true;
                        EnemySelected = false;
                        AllySelected = false;
                        NeutralSelected = false;
                        if (DummyUnit != null)
                        {
                            DummyUnit.transform.GetChild(0).gameObject.layer = 9;
                        }
                        PlayMainButtonClick();
                    }
                }
                if (GUI.Toggle(OwnerEnemyButtonRect, EnemySelected, "Enemy"))
                {
                    if (!EnemySelected)
                    {
                        PlayerSelected = false;
                        EnemySelected = true;
                        AllySelected = false;
                        NeutralSelected = false;
                        if (DummyUnit != null)
                        {
                            DummyUnit.transform.GetChild(0).gameObject.layer = 10;
                        }
                        PlayMainButtonClick();
                    }
                }
                if (GUI.Toggle(OwnerAlliedButtonRect, AllySelected, "Ally"))
                {
                    if (!AllySelected)
                    {
                        PlayerSelected = false;
                        EnemySelected = false;
                        AllySelected = true;
                        NeutralSelected = false;
                        if (DummyUnit != null)
                        {
                            DummyUnit.transform.GetChild(0).gameObject.layer = 11;
                        }
                        PlayMainButtonClick();
                    }
                }
                if (GUI.Toggle(OwnerNeutralButtonRect, NeutralSelected, "Neutral"))
                {
                    if (!NeutralSelected)
                    {
                        PlayerSelected = false;
                        EnemySelected = false;
                        AllySelected = false;
                        NeutralSelected = true;
                        if (DummyUnit != null)
                        {
                            DummyUnit.transform.GetChild(0).gameObject.layer = 13;
                        }
                        PlayMainButtonClick();
                    }
                }

                //Button to start Combat
                if (GUI.Button(StartButtonRect, "Start", GameManager.instance.standardButtonStyle))
                {
                    DeselectDesignData();
                    SetPauseShipManagers(false);
                    SetupPhase = false;
                    PlayMainButtonClick();
                }

                if(GUI.Button(SummaryButtonRect, "Summary", GameManager.instance.standardButtonStyle))
                {
                    AddEntriesToSummaryList();
                    SummaryScrollList.SetOpen(true);
                }
            }
            else
            {
                storedUnitPanel.Draw();
                storedUnitPanel.CheckToolTip(mousePosition);
                if (shipDragSelectionBox.isActive())
                {
                    shipDragSelectionBox.Draw();
                }
            }

            if (combatTimer.CheckToolTip(mousePosition)) { }
            else if (miniMap.CheckToolTip(mousePosition)) { }
            else if (gameSpeedButton.ToolTipCheck(mousePosition)) { }

            ToolTip.Draw();
        }
    }

    protected override void ChangeHull(ShipHullData data)
    {
        BuildDeignDataList(data);
        DesignScrollPostion = Vector2.zero;
    }

    void ChangeStationHull(StationHullData data)
    {
        BuildStationDesignDataList(data);
        DesignScrollPostion = Vector2.zero;
    }

    void ChangeFighter(FighterDefinition newFighter)
    {
        selectedFighter = newFighter;
        CreateDummyFighter();
        if (DummyUnit != null)
        {
            if (PlayerSelected)
            {
                DummyUnit.transform.GetChild(0).gameObject.layer = 9;
            }
            else if (EnemySelected)
            {
                DummyUnit.transform.GetChild(0).gameObject.layer = 10;
            }
            else if (AllySelected)
            {
                DummyUnit.transform.GetChild(0).gameObject.layer = 11;
            }
            else if (NeutralSelected)
            {
                DummyUnit.transform.GetChild(0).gameObject.layer = 13;
            }
        }
    }

    protected override void LoadDesignData(ShipDesignData designData)
    {
        selectedDesign = designData;
        CreateDummyShips();
        if (DummyUnit != null)
        {
            if (PlayerSelected)
            {
                DummyUnit.transform.GetChild(0).gameObject.layer = 9;
            }
            else if (EnemySelected)
            {
                DummyUnit.transform.GetChild(0).gameObject.layer = 10;
            }
            else if (AllySelected)
            {
                DummyUnit.transform.GetChild(0).gameObject.layer = 11;
            }
            else if (NeutralSelected)
            {
                DummyUnit.transform.GetChild(0).gameObject.layer = 13;
            }
        }
    }

    protected override void LoadStationDesignData(StationDesignData designData)
    {
        selectedStationDesign = designData;
        CreateDummyStation();
        if (PlayerSelected)
        {
            DummyUnit.transform.GetChild(0).gameObject.layer = 9;
        }
        else if (EnemySelected)
        {
            DummyUnit.transform.GetChild(0).gameObject.layer = 10;
        }
        else if (AllySelected)
        {
            DummyUnit.transform.GetChild(0).gameObject.layer = 11;
        }
        else if (NeutralSelected)
        {
            DummyUnit.transform.GetChild(0).gameObject.layer = 13;
        }
    }

    protected override void DeselectDesignData()
    {
        selectedDesign = null;
        selectedStationDesign = null;
        if (DummyUnit != null)
        {
            Object.Destroy(DummyUnit);
        }
    }

    protected override void CloseScreen()
    {
        base.CloseScreen();
        DestroyAllShips();
        //GameManager.instance.ChangeScreen(new MainMenuScreen());
    }

    bool ScreenElementsContains(Vector2 point)
    {
        if (gameSpeedButton.Contains(point))
        {
            return true;
        }
        else if (miniMap.Contains(point))
        {
            return true;
        }
        else if(combatTimer.Contains(point))
        {
            return true;
        }
        else if(storedUnitPanel.Contains(point))
        {
            return true;
        }
        else if (SetupPhase && ShipSelectionRect.Contains(point))
        {
            return true;
        }
        else if (PlayerShipManager.HasSomethingSelected() && shipInfoPanel.ContainsMouse(point))
        {
            return true;
        }
        else if (fleetCombatInfoPanel.Contains(point))
        {
            return true;
        }

        return false;
    }

    void ChangeUnitCategory(UnitCategory newCategory)
    {
        SelectedUnityCategory = newCategory;
        DestroyDummyShip();
        selectedDesign = null;
        selectedStationDesign = null;

        if (SelectedUnityCategory == UnitCategory.Fighters)
        {
            fighterList.CheckFirstEntry(ChangeFighter);
        }
        else if(SelectedUnityCategory == UnitCategory.Stations)
        {
            stationHullList.CheckFirstHull(ChangeStationHull);
        }
        else
        {
            shipHullList.CheckFirstHull(ChangeHull);
        }
    }

    public bool isPositionInPlayArea(Vector3 position)
    {
        if (position.z > ResourceManager.instance.GetGameConstants().CameraLimitUp)
        {
            return false;
        }
        else if (position.z < ResourceManager.instance.GetGameConstants().CameraLimitDown)
        {
            return false;
        }
        if (position.x > ResourceManager.instance.GetGameConstants().CameraLimitRight)
        {
            return false;
        }
        else if (position.x < ResourceManager.instance.GetGameConstants().CameraLimitLeft)
        {
            return false;
        }
        return true; ;
    }

    void AddEntriesToSummaryList()
    {
        SummaryScrollList.Clear();
        SummaryScrollList.AddFleetEntries(PlayerFleet);
        SummaryScrollList.AddFleetEntries(EnemyFleet);
        SummaryScrollList.AddFleetEntries(AlliedFleet);
        SummaryScrollList.AddFleetEntries(NeutralFleet);
        SummaryScrollList.UpdateViewWindowSize();
    }
}
