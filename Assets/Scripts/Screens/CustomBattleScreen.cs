/**********************************************************************************************************************
Author: McShooterz
Notes: This screen is for setting up and playing custom battles. This screen needs a list of hulls with a list of designs
for the selected hull, as well as a way to choose who will own the ship to be placed. The stats that are relevant to combat
for the selected design should be displayed.
**************************************************************************************************************************/
using UnityEngine;
using System.Collections.Generic;

public class CustomBattleScreen : CombatScreens
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

    GameSpeedButton gameSpeedButton;

    bool PlayerSelected = true;
    bool EnemySelected = false;
    bool AllySelected = false;
    bool NeutralSelected = false;

    FleetCombatInfoPanel fleetCombatInfoPanel;

    #endregion

    public CustomBattleScreen()
    {
        ShipSelectionRect = new Rect(0, 0, Screen.width * 0.25f, Screen.height * 0.65f);

        shipHullList = new ShipHullScrollList(new Rect(ShipSelectionRect.width * 0.05f, ShipSelectionRect.height * 0.025f, ShipSelectionRect.width * .9f, ShipSelectionRect.height * 0.25f), ChangeHull, CheckHullValid);

        DesignScrollWindowRect = new Rect(shipHullList.getRect().x, shipHullList.getRect().yMax + Screen.height * 0.025f, shipHullList.getRect().width, shipHullList.getRect().height);
        DesignScrollViewRect = new Rect(0, 0, DesignScrollWindowRect.width * 0.92f, DesignScrollWindowRect.height + 1);
        DesignScrollPostion = Vector2.zero;

        OwnerPlayerButtonRect = new Rect(ShipSelectionRect.width / 2 - GameManager.instance.StandardButtonSize.x / 2, DesignScrollWindowRect.yMax + GameManager.instance.StandardButtonSize.y, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);
        OwnerEnemyButtonRect = new Rect(OwnerPlayerButtonRect.x, OwnerPlayerButtonRect.yMax, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);
        OwnerAlliedButtonRect = new Rect(OwnerPlayerButtonRect.x, OwnerEnemyButtonRect.yMax, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);
        OwnerNeutralButtonRect = new Rect(OwnerPlayerButtonRect.x, OwnerAlliedButtonRect.yMax, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);

        StartButtonRect = new Rect(OwnerPlayerButtonRect.x, ShipSelectionRect.yMax - GameManager.instance.StandardButtonSize.y * 2, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);

        PlayerFleet = new FleetData();
        EnemyFleet = new FleetData();
        AlliedFleet = new FleetData();
        NeutralFleet = new FleetData();

        PlayerShipManager = new ShipManager(true, PlayerFleet, 9);
        EnemyShipManager = new ShipManager(false,EnemyFleet, 10);
        AlliedShipManager = new ShipManager(false, AlliedFleet, 11);
        NeutralShipManager = new ShipManager(false, NeutralFleet, 13);

        ShipManagers.Add(PlayerShipManager);
        ShipManagers.Add(EnemyShipManager);
        ShipManagers.Add(AlliedShipManager);
        ShipManagers.Add(NeutralShipManager);

        PlayerShipManager.SetPause(true);
        EnemyShipManager.SetPause(true);
        AlliedShipManager.SetPause(true);
        NeutralShipManager.SetPause(true);
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

        Rect fleetCombatInfoPanelRect = new Rect(0, Screen.height - shipPanelSize.y, shipPanelSize.y, shipPanelSize.y);
        fleetCombatInfoPanel = new FleetCombatInfoPanel(fleetCombatInfoPanelRect, PlayerFleet, GameManager.instance);

        shipHullList.CheckFirstHull(ChangeHull);
    }

    // Update is called once per frame
    public override void Update ()
    {
        base.Update();
        SetMousePosition();

        if (SetupPhase)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseScreen();
            }

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
						Ship ship = hit.collider.transform.root.GetComponent<Ship> ();
						ship.GetShipData ().RemoveFromFleet ();
						ship.DestroyAllFighters ();
                        ship.DestroyWeaponEffects();
                        ship.DeleteSelf ();
					} 
					else if (hit.collider.transform.root.tag == "Fighter") 
					{
						Fighter fighter = hit.collider.transform.root.GetComponent<Fighter> ();
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
                    if(!PivotSet)
                        DummyUnit.transform.position = new Vector3(hit.point.x, 1f, hit.point.z);
                    if(Input.GetMouseButtonDown(0))
                    {
                        PivotPoint = DummyUnit.transform.position;
                        PivotSet = true;
                    }
                    else if(Input.GetMouseButton(0))
                    {
                        Vector3 Direction = (new Vector3(hit.point.x, 1f, hit.point.z)  - PivotPoint);
                        if (Direction.magnitude > DummyUnitRadius)
                        {
                            Quaternion Rotation = Quaternion.LookRotation(Direction);
                            DummyUnit.transform.rotation = Rotation;
                        }
                    }
                    else if (Input.GetMouseButtonUp(0))
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
                        PivotSet = false;
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
            else if (miniMap.Contains(mousePosition))
            {
                if (Input.GetMouseButton(0))
                {
                    miniMap.GetWorldPosition(mousePosition);
                }
            }
            else if(DummyUnit == null)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    Ray ray;
                    RaycastHit hit;

                    ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    //If we hit...
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    {
                        if (hit.collider.transform.root.tag == "Ship")
                        {
                            PickedUpObject = hit.collider.transform.root.gameObject;
                            PickedUpOrigin = hit.collider.transform.root.transform.position;
                        }
                    }
                }
            }
        }
        else
        {
            //Update the ships managers
            UpdateShipManagers();

            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SetPauseShipManagers(true);
                SetupPhase = true;
            }

            if (!ScreenElementsContains(mousePosition))
            {
                Ray ray;
                RaycastHit hit;
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
					//Highlight hovered units
					if (hit.collider.transform.root.tag == "Ship")
                    {
						hit.collider.transform.root.GetComponent<SpaceUnit> ().Hovered ();
					}
                    else if (hit.collider.transform.root.tag == "Fighter")
                    {
						hit.collider.transform.root.GetComponent<Fighter> ().GetParentWing ().Hovered ();
					}
                    else if (hit.collider.transform.root.tag == "FighterWing")
                    {
						hit.collider.transform.root.GetComponent<FighterWing> ().Hovered ();
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
                                    PlayerShipManager.AddToSelection(ship);
                                }
                                else
                                {
                                    PlayerShipManager.SelectUnownedShip(ship);
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
                            }
                        }
                        else if (Input.GetMouseButtonDown(1) && PlayerShipManager.HasSomethingSelected())
                        {
                            if (hit.collider.gameObject.tag == "PlayArea")
                            {
                                PivotSet = true;
                                PivotObject = new GameObject();
                                PivotObject.transform.position = hit.point;
                                Vector3 shipGroupCenter;
                                Vector3 sum = Vector3.zero;
                                foreach (Ship ship in PlayerShipManager.GetSelectedShips())
                                {
                                    sum += ship.transform.position;
                                }
                                foreach (FighterWing fighterWing in PlayerShipManager.GetSelectedFighters())
                                {
                                    sum += fighterWing.transform.position;
                                }
                                shipGroupCenter = sum / PlayerShipManager.GetSelectedMobileUnitsCount();
                                Vector3 Direction = PivotObject.transform.position - shipGroupCenter;
                                Quaternion rotation = Quaternion.LookRotation(Direction, Vector3.up);
                                PivotObject.transform.rotation = rotation;
                                foreach (Ship ship in PlayerShipManager.GetSelectedShips())
                                {
                                    GameObject marker = ResourceManager.CreateDirectionMarker(ship.transform.position + Direction);
                                    marker.transform.rotation = rotation;
                                    DirectionMarkers.Add(marker);
                                    marker.transform.parent = PivotObject.transform;
                                }
                                foreach (FighterWing fighterWing in PlayerShipManager.GetSelectedFighters())
                                {
                                    GameObject marker = ResourceManager.CreateDirectionMarker(fighterWing.transform.position + Direction);
                                    marker.transform.rotation = rotation;
                                    DirectionMarkers.Add(marker);
                                    marker.transform.parent = PivotObject.transform;
                                }
                            }
                            else if (hit.collider.transform.root.tag == "Ship")
                            {
                                Ship ship = hit.collider.transform.root.GetComponent<Ship>();
                                PlayerShipManager.SetTargetForSelectedShips(ship);
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
                                if (Direction.sqrMagnitude > 0)
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
            else if(miniMap.Contains(mousePosition))
            {
                if (Input.GetMouseButton(0))
                {
                    miniMap.GetWorldPosition(mousePosition);
                }
            }
        }
    }

    public override void Draw()
    {
        miniMap.Draw();
        shipInfoPanel.Draw(mousePosition);
        fleetCombatInfoPanel.Draw();

        gameSpeedButton.Draw();

        if (SetupPhase)
        {
            GUI.Box(ShipSelectionRect, "", GameManager.instance.standardBackGround);

            shipHullList.Draw();

            DesignScrollPostion = GUI.BeginScrollView(DesignScrollWindowRect, DesignScrollPostion, DesignScrollViewRect);
            foreach (DesignDataListEntry entry in DesignDataList)
            {
                entry.Draw(selectedDesign);
            }
            GUI.EndScrollView();

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
            if(GUI.Toggle(OwnerNeutralButtonRect, NeutralSelected, "Neutral"))
            {
                if(!NeutralSelected)
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
                PlayerShipManager.SetPause(false);
                EnemyShipManager.SetPause(false);
                AlliedShipManager.SetPause(false);
                SetupPhase = false;
                PlayMainButtonClick();
            }
        }
        else
        {
            if (shipDragSelectionBox.isActive())
            {
                shipDragSelectionBox.Draw();
            }
        }
    }

    protected override void ChangeHull(ShipHullData hullData)
    {
        BuildDeignDataList(hullData);
        DesignScrollPostion = Vector2.zero;
    }

    protected override void LoadDesignData(ShipDesignData designData)
    {
        selectedDesign = designData;
        CreateDummyShips();
        if (PlayerSelected)
        {
            DummyUnit.transform.GetChild(0).gameObject.layer = 9;
        }
        else if(EnemySelected)
        {
            DummyUnit.transform.GetChild(0).gameObject.layer = 10;
        }
        else if (AllySelected)
        {
            DummyUnit.transform.GetChild(0).gameObject.layer = 11;
        }
        else if(NeutralSelected)
        {
            DummyUnit.transform.GetChild(0).gameObject.layer = 13;
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
        if (DummyUnit != null)
        {
            Object.Destroy(DummyUnit);
        }
    }

    protected override void CloseScreen()
    {
        base.CloseScreen();
        DestroyAllShips();
        GameManager.instance.ChangeScreen(new CampaignPlayScreen());
    }

    bool ScreenElementsContains(Vector2 point)
    {
        if(gameSpeedButton.Contains(point))
        {
            return true;
        }
        else if(miniMap.Contains(point))
        {
            return true;
        }
        else if(SetupPhase && ShipSelectionRect.Contains(point))
        {
            return true;
        }
        else if(PlayerShipManager.HasSomethingSelected() && shipInfoPanel.ContainsMouse(point))
        {
            return true;
        }
        else if(fleetCombatInfoPanel.Contains(point))
        {
            return true;
        }

        return false;
    }
}
