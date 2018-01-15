/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections.Generic;
using System;

public class ScenarioPlayScreen : CombatScreens
{
    #region Variables
    ScreenState screenState = ScreenState.setup;
    List<GameObject> PlayerPlacedObjects = new List<GameObject>();

    GameSpeedButton gameSpeedButton;
    CombatTimer combatTimer;

    ShipManager EnemyShipManager;
    ShipManager AlliedShipManager;
    ShipManager NeutralShipManager;

    FleetData PlayerFleet;
    FleetData EnemyFleet;
    FleetData AlliedFleet;
    FleetData NeutralFleet;

    //Pause
    Rect ContinueButtonRect;
    Rect QuitButtonRect;

    //Setup
    GameObject deploymentArea;
    Rect SetupPanel;
    Rect CommandRect;
    Rect MoneyRect;
    Rect StartButtonRect;
    Rect BackButtonRect;

    int CommandLimit;
    int CommandUsed;
    float MoneyLimit;
    float MoneyUsed;
    Texture2D CommandIcon;
    Texture2D MoneyIcon;

    //Play



    //Summary
    CombatSummaryScrollList SummaryScrollList;

    #endregion

    public ScenarioPlayScreen(Scenario selectedScenario)
    {

        CommandIcon = ResourceManager.instance.GetIconTexture("Icon_CommandPoint");
        MoneyIcon = ResourceManager.instance.GetIconTexture("Icon_Money");

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

        float MiniMapSize = Screen.height * 0.2f;
        Rect miniMapRect = new Rect(Screen.width - Screen.height * 0.215f, Screen.height - Screen.height * 0.215f, MiniMapSize, MiniMapSize);
        miniMap = new MiniMap(miniMapRect, GameManager.instance.miniMapTexture, ToolTip);
        float GameSpeedButtonSize = miniMapRect.width / 5f;
        gameSpeedButton = new GameSpeedButton(new Rect(miniMapRect.x - GameSpeedButtonSize, miniMapRect.yMax - GameSpeedButtonSize, GameSpeedButtonSize, GameSpeedButtonSize), ToolTip);

        Vector2 shipPanelSize = new Vector2(Screen.width * 0.5f, Screen.height * 0.25f);
        shipInfoPanel = new ShipCombatInfoPanel(this, new Rect(new Vector2((Screen.width - shipPanelSize.x) / 2, Screen.height - shipPanelSize.y), shipPanelSize), PlayerShipManager, ToolTip);

        SetupPanel = new Rect(0, 0, Screen.width * 0.25f, Screen.height * 0.6f);

        Rect battleTimerRect = new Rect(Screen.width * 0.475f, 0, Screen.width * 0.05f, Screen.height * 0.03f);
        combatTimer = new CombatTimer(battleTimerRect, ToolTip);

        shipHullList = new ShipHullScrollList(new Rect(SetupPanel.width * 0.1f, SetupPanel.height * 0.01f, SetupPanel.width * 0.8f, SetupPanel.height * 0.375f), ChangeHull, CheckHullValid);

        DesignScrollWindowRect = new Rect(shipHullList.getRect().x, shipHullList.getRect().yMax + SetupPanel.height * 0.01f, shipHullList.getRect().width, shipHullList.getRect().height);
        DesignScrollViewRect = new Rect(0, 0, DesignScrollWindowRect.width * 0.92f, DesignScrollWindowRect.height * 1.02f);
        DesignScrollPostion = Vector2.zero;

        CommandRect = new Rect((SetupPanel.width - GameManager.instance.StandardLabelSize.x) / 2f, DesignScrollWindowRect.yMax + SetupPanel.height * 0.01f, GameManager.instance.StandardLabelSize.x, GameManager.instance.StandardLabelSize.y);
        MoneyRect = new Rect(CommandRect.x, CommandRect.yMax + SetupPanel.height * 0.01f, GameManager.instance.StandardLabelSize.x, GameManager.instance.StandardLabelSize.y);
        StartButtonRect = new Rect((SetupPanel.width - GameManager.instance.StandardButtonSize.x * 2) / 4f, MoneyRect.yMax + SetupPanel.height * 0.01f, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);
        BackButtonRect = new Rect((SetupPanel.width / 2f) + (SetupPanel.width - GameManager.instance.StandardButtonSize.x * 2) / 4f, StartButtonRect.y, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);

        //Summary
        SummaryScrollList = new CombatSummaryScrollList();

        //Pause
        Vector2 pauseButtonSize = new Vector2(Screen.width * 0.078125f, Screen.height * 0.037037f);
        ContinueButtonRect = new Rect((Screen.width - pauseButtonSize.x) / 2f, Screen.height / 3.5f, pauseButtonSize.x, pauseButtonSize.y);
        QuitButtonRect = new Rect(ContinueButtonRect.x, ContinueButtonRect.yMax, pauseButtonSize.x, pauseButtonSize.y);

        shipHullList.CheckFirstHull(ChangeHull);

        LoadScenario(selectedScenario);
    }

    public override void Update()
    {
        base.Update();
        SetMousePosition();

        if (screenState == ScreenState.setup)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseScreen();
            }

            //Deselect
            if (Input.GetMouseButtonDown(1))
            {
                DeselectDesignData();
                if (PickedUpObject != null)
                {
                    PickedUpObject.transform.position = PickedUpOrigin;
                    PickedUpObject = null;
                }
            }

            //Delete placed objects
            if (Input.GetMouseButton(1))
            {
                Ray ray;
                RaycastHit hit;

                ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    if(hit.collider.transform.root.tag == "Ship" && PlayerPlacedObjects.Contains(hit.collider.transform.root.gameObject))
                    {
                        RemovePlacedShip(hit.collider.transform.root.GetComponent<Ship>());
                    }
                }
            }
            //Pivot 
            else if (DummyUnit != null && !ScreenElementsContains(mousePosition))
            {
                Ray ray;
                RaycastHit hit;

                ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    if (!PivotSet)
                        DummyUnit.transform.position = new Vector3(hit.point.x, 1f, hit.point.z);
                    //Check if it hits deployment area
                    if (Input.GetMouseButtonDown(0) && CanAffordDesign(selectedDesign) && DeploymentAreaContains(hit.point))
                    {
                        PivotPoint = DummyUnit.transform.position;
                        PivotSet = true;
                    }
                    else if (Input.GetMouseButton(0) && PivotSet)
                    {
                        Vector3 Direction = (new Vector3(hit.point.x, 1f, hit.point.z) - PivotPoint);
                        if (Direction.magnitude > DummyUnitRadius)
                        {
                            Quaternion Rotation = Quaternion.LookRotation(Direction);
                            DummyUnit.transform.rotation = Rotation;
                        }
                    }
                    else if (Input.GetMouseButtonUp(0) && PivotSet)
                    {
                        PlaceShip();
                        PivotSet = false;
                    }
                }
            }
            else if (PickedUpObject != null && !ScreenElementsContains(mousePosition))
            {
                Ray ray;
                RaycastHit hit;
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    if (!PivotSet)
                        PickedUpObject.transform.position = new Vector3(hit.point.x, 1f, hit.point.z);
                    if (Input.GetMouseButtonDown(0) && DeploymentAreaContains(PickedUpObject.transform.position - new Vector3(0,1,0)))
                    {
                        PivotPoint = PickedUpObject.transform.position;
                        PivotSet = true;
                    }
                    else if (Input.GetMouseButton(0) && PivotSet)
                    {
                        Vector3 Direction = (new Vector3(hit.point.x, 1f, hit.point.z) - PivotPoint);
                        if (Direction.magnitude > DummyUnitRadius)
                        {
                            Quaternion Rotation = Quaternion.LookRotation(Direction);
                            PickedUpObject.transform.rotation = Rotation;
                        }
                    }
                    else if (Input.GetMouseButtonUp(0) && PivotSet)
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
                        if (hit.collider.transform.root.tag == "Ship" && PlayerPlacedObjects.Contains(hit.collider.transform.root.gameObject))
                        {
                            PickedUpObject = hit.collider.transform.root.gameObject;
                            PickedUpOrigin = hit.collider.transform.root.transform.position;
                        }
                    }
                }
            }
        }
        else if (screenState == ScreenState.play)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                screenState = ScreenState.paused;
                SetPauseShipManagers(true);
            }
            //Update the ships managers
            UpdateShipManagers();
            if(combatTimer.CountDown())
            {
                EndCombat();
                return;
            }
            //Check if Player defeated
            if(PlayerShipManager.NoUnitsLeft())
            {
                SummaryScrollList.SetResult("Defeat");
                EndCombat();
                return;
            }
            if(EnemyShipManager.NoUnitsLeft())
            {
                SummaryScrollList.SetResult("Victory");
                EndCombat();
                return;
            }

            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SetPauseShipManagers(true);
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
                                UnityEngine.Object.Destroy(PivotObject);
                                foreach (GameObject marker in DirectionMarkers)
                                {
                                    UnityEngine.Object.Destroy(marker);
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
                                shipDragSelectionBox.SetActive(true);
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
                                UnityEngine.Object.Destroy(PivotObject);
                                foreach (GameObject marker in DirectionMarkers)
                                {
                                    UnityEngine.Object.Destroy(marker);
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
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseScreen();
            }
        }
        if (miniMap.Contains(mousePosition))
        {
            if (Input.GetMouseButton(0))
            {
                miniMap.GetWorldPosition(mousePosition);
            }
        }
    }

    public override void Draw()
    {
        miniMap.Draw();
        gameSpeedButton.Draw();
        combatTimer.Draw();

        if (screenState == ScreenState.setup)
        {

            DrawSetup();

        }
        else if(screenState == ScreenState.play)
        {
            shipInfoPanel.Draw(mousePosition);
            if (shipDragSelectionBox.isActive())
            {
                shipDragSelectionBox.Draw();
            }
        }
        else if(screenState == ScreenState.Summary)
        {
            SummaryScrollList.Draw();
        }
        else
        {
            DrawPauseMenu();
        }
    }

    void DrawSetup()
    {
        GUI.Box(SetupPanel, "");

        shipHullList.Draw();

        DesignScrollPostion = GUI.BeginScrollView(DesignScrollWindowRect, DesignScrollPostion, DesignScrollViewRect);
        foreach (DesignDataListEntry entry in DesignDataList)
        {
            entry.Draw(selectedDesign);
        }
        GUI.EndScrollView();

        GameManager.instance.UIContent.image = CommandIcon;
        GameManager.instance.UIContent.text = ": " + (CommandLimit - CommandUsed).ToString();
        GUI.Label(CommandRect, GameManager.instance.UIContent);
        GameManager.instance.UIContent.image = MoneyIcon;
        GameManager.instance.UIContent.text = ": " + (MoneyLimit - MoneyUsed).ToString("0.#");
        GUI.Label(MoneyRect, GameManager.instance.UIContent);
        if(GUI.Button(StartButtonRect, "Start", GameManager.instance.standardButtonStyle))
        {
            Play();
        }

        if(GUI.Button(BackButtonRect, "Back", GameManager.instance.standardButtonStyle))
        {
            CloseScreen();
        }
    }

    void DrawPauseMenu()
    {
        if(GUI.Button(ContinueButtonRect, "Continue"))
        {
            screenState = ScreenState.play;
            Play();
        }
        if(GUI.Button(QuitButtonRect, "Quit"))
        {
            CloseScreen();
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
        DummyUnit.transform.GetChild(0).gameObject.layer = 9;
    }

    protected override void LoadStationDesignData(StationDesignData designData)
    {
        selectedStationDesign = designData;
        CreateDummyStation();
    }

    protected override void DeselectDesignData()
    {
        selectedDesign = null;
        if (DummyUnit != null)
        {
            UnityEngine.Object.Destroy(DummyUnit);
        }
    }

    void Play()
    {
        DestroyDeploymentArea();
        DestroyDummyShip();
        screenState = ScreenState.play;
        PivotSet = false;

        EnemyShipManager.HighLightAllShips(false);
        AlliedShipManager.HighLightAllShips(false);
        NeutralShipManager.HighLightAllShips(false);

        SetPauseShipManagers(false);
    }

    void LoadScenario(Scenario scenario)
    {
        ResetCamera();
        Camera.main.transform.position = new Vector3(scenario.DeploymentAreaPosition.x, Camera.main.transform.position.y, scenario.DeploymentAreaPosition.z);

        deploymentArea = ResourceManager.CreateDeploymentArea(scenario.DeploymentAreaPosition);
        deploymentArea.transform.localScale = new Vector3(2.5f, 1f, 2.5f);

        CommandLimit = scenario.CommandLimit;
        MoneyLimit = scenario.MoneyLimit;
        combatTimer.SetTime(scenario.TimeLimit);

        EnemyShipManager.HighLightAllShips(true);
        AlliedShipManager.HighLightAllShips(true);
        NeutralShipManager.HighLightAllShips(true);
    }

    void EndCombat()
    {
        PlayerShipManager.SetPause(true);
        EnemyShipManager.SetPause(true);
        AlliedShipManager.SetPause(true);
        NeutralShipManager.SetPause(true);

        screenState = ScreenState.Summary;
        BuildSummaryShipList();
    }

    bool ScreenElementsContains(Vector2 point)
    {
        if (miniMap.Contains(point) || gameSpeedButton.Contains(point))
        {
            return true;
        }
        else if(screenState == ScreenState.setup && SetupPanel.Contains(point))
        {
            return true;
        }
        else if(screenState == ScreenState.play && shipInfoPanel.ContainsMouse(point) && PlayerShipManager.HasSomethingSelected())
        {
            return true;
        }
        return false;
    }

    bool DeploymentAreaContains(Vector3 point)
    {
        return deploymentArea.GetComponent<BoxCollider>().bounds.Contains(point);
    }

    void DestroyDeploymentArea()
    {
        if (deploymentArea != null)
        {
            UnityEngine.Object.Destroy(deploymentArea);
        }
    }

    bool CanAffordDesign(ShipDesignData design)
    {
        if(design.CommandPoints <= CommandLimit - CommandUsed)
        {
            if(design.GetTotalValue() <= MoneyLimit - MoneyUsed)
            {
                return true;
            }
        }
        return false;
    }

    void PlaceShip()
    {
        CommandUsed += selectedDesign.CommandPoints;
        MoneyUsed += selectedDesign.GetTotalValue();

        ShipData shipData = new ShipData(selectedDesign);
        Ship newShip = ResourceManager.instance.CreateCombatShip(shipData, DummyUnit.transform.position, DummyUnit.transform.rotation);
        PlayerPlacedObjects.Add(newShip.gameObject);
        PlayerShipManager.AddSpaceUnit(newShip);
        PlayerFleet.AddShip(newShip.GetShipData());
    }

    void RemovePlacedShip(Ship ship)
    {
        CommandUsed -= ship.GetShipData().designData.CommandPoints;
        MoneyUsed -= ship.GetShipData().designData.GetTotalValue();
        PlayerPlacedObjects.Remove(ship.gameObject);
        ship.DeleteSelf();
    }

    protected override void CloseScreen()
    {
        base.CloseScreen();
        DestroyAllShips();
        DestroyDeploymentArea();
        GameManager.instance.ChangeScreen(new ScenarioSelectScreen());
    }

    public enum ScreenState
    {
        setup,
        play,
        Summary,
        paused
    }

    void BuildSummaryShipList()
    {
        SummaryScrollList.Clear();

        SummaryScrollList.AddFleetEntries(PlayerFleet);

        SummaryScrollList.AddFleetEntries(EnemyFleet);

        SummaryScrollList.AddFleetEntries(PlayerFleet);

        SummaryScrollList.AddFleetEntries(AlliedFleet);

        SummaryScrollList.AddFleetEntries(NeutralFleet);

        SummaryScrollList.UpdateViewWindowSize();
    }
}
