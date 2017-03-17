/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections.Generic;

public class ScenarioCreateScreen : ShipSelectingScreens
{
    #region Variables
    MiniMap miniMap;

    GameObject DummyUnit = null;
    Scenario selectedScenario = null;

    GameObject DeploymentArea;

    Rect MainPanel;

    State state = State.Setup;

    //Tabs
    Rect SetupButtonRect;
    Rect UnitsButtonRect;
    Rect MiscButtonRect;

    Rect CloseButtonRect;

    //Setup panel
    List<ScenarioListEntry> Scenarios = new List<ScenarioListEntry>();
    Rect ScenarioListWindowRect;
    Rect ScenarioListViewRect;
    Vector2 ScenarioListPosition = Vector2.zero;
    Rect NameRect;
    Rect NameFieldRect;
    Rect DescriptionRect;
    Rect DescriptionFieldRect;
    Rect SaveButtonRect;
    Rect LoadButtonRect;
    Rect DeleteButtonRect;

    string ScenarioName = "";
    string ScenarioDescription = "";

    Rect DifficultyRect;
    Rect DifficultyButtonRect;
    Rect TimeLimitRect;
    Rect TimeLimitSliderRect;
    Rect CommandLimitRect;
    Rect CommandLimitSliderRect;
    Rect MoneyLimitRect;
    Rect MoneyLimitSliderRect;

    ScenarioDifficulty Difficulty = ScenarioDifficulty.normal;
    float TimeLimit = 300;
    int CommandLimit = 60;
    float MoneyLimit = 20000;

    //Units panel
    Rect PlayerButtonRect;
    Rect EnemyButtonRect;
    Rect AllyButtonRect;
    Rect NeutralButtonRect;

    bool PlayerSelected = true;
    bool EnemySelected = false;
    bool AllySelected = false;
    bool NeutralSelected = false;

    Rect ShipNameRect;
    Rect ShipNameFieldRect;
    Rect ShipLevelRect;
    Rect ShipLevelSliderRect;

    string shipName = "";
    int shipLevel = 0;

    Rect ClearShipsButtonRect;

    bool PivotSet = false;
    Vector3 PivotPoint = Vector3.zero;
    float DummyShipRadius = 0;

    Dictionary<GameObject, ScenarioShipData> PlayerShips = new Dictionary<GameObject, ScenarioShipData>();
    Dictionary<GameObject, ScenarioShipData> EnemyShips = new Dictionary<GameObject, ScenarioShipData>();
    Dictionary<GameObject, ScenarioShipData> AlliedShips = new Dictionary<GameObject, ScenarioShipData>();
    Dictionary<GameObject, ScenarioShipData> NeutralShips = new Dictionary<GameObject, ScenarioShipData>();

    List<GameObject> PlacedObjects = new List<GameObject>();
    #endregion

    public ScenarioCreateScreen()
    {

        MainPanel = new Rect(0, 0, Screen.width * 0.28f, Screen.height);

        float toolTipWidth = Screen.width * 0.175f;
        ToolTip = new GUIToolTip(new Vector2(Screen.width - toolTipWidth, 0), toolTipWidth);

        float MiniMapSize = Screen.height * 0.2f;
        Rect miniMapRect = new Rect(Screen.width - Screen.height * 0.215f, Screen.height - Screen.height * 0.215f, MiniMapSize, MiniMapSize);
        miniMap = new MiniMap(miniMapRect, GameManager.instance.miniMapTexture, ToolTip);

        //Tabs
        UnitsButtonRect = new Rect(MainPanel.width / 2f - GameManager.instance.StandardButtonSize.x / 2f, Screen.height * 0.02f, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);
        SetupButtonRect = new Rect(UnitsButtonRect.x / 2f - GameManager.instance.StandardButtonSize.x / 2f, UnitsButtonRect.y, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);
        MiscButtonRect = new Rect(UnitsButtonRect.xMax + UnitsButtonRect.x / 2f - GameManager.instance.StandardButtonSize.x / 2f, UnitsButtonRect.y, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);

        CloseButtonRect = new Rect(UnitsButtonRect.x, Screen.height - GameManager.instance.StandardButtonSize.y * 1.35f, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);

        //Setup panel
        ScenarioListWindowRect = new Rect(MainPanel.width * 0.04f, UnitsButtonRect.yMax + MainPanel.height * 0.01f, MainPanel.width * 0.92f, MainPanel.height * 0.45f);
        ScenarioListViewRect = new Rect(0, 0, ScenarioListWindowRect.width * 0.95f, ScenarioListWindowRect.height * 1.02f);

        NameRect = new Rect(ScenarioListWindowRect.x, ScenarioListWindowRect.yMax + MainPanel.height * 0.005f, GameManager.instance.StandardLabelSize.x, GameManager.instance.StandardLabelSize.y);
        NameFieldRect = new Rect(NameRect.xMax, NameRect.y, ScenarioListWindowRect.width - NameRect.width - MainPanel.width * 0.02f, GameManager.instance.StandardButtonSize.y);
        DescriptionRect = new Rect(NameRect.x, NameRect.yMax, GameManager.instance.StandardLabelSize.x, GameManager.instance.StandardLabelSize.y);
        DescriptionFieldRect = new Rect(NameRect.x, DescriptionRect.yMax + MainPanel.height * 0.002f, ScenarioListWindowRect.width, MainPanel.height * 0.15f);
        SaveButtonRect = new Rect(SetupButtonRect.x, CloseButtonRect.y - GameManager.instance.StandardButtonSize.y * 1.35f, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);
        LoadButtonRect = new Rect(UnitsButtonRect.x, SaveButtonRect.y, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);
        DeleteButtonRect = new Rect(MiscButtonRect.x, SaveButtonRect.y, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);

        DifficultyRect = new Rect(NameRect.x, DescriptionFieldRect.yMax + MainPanel.height * 0.01f, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);
        DifficultyButtonRect = new Rect(DifficultyRect.xMax, DifficultyRect.y, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);
        TimeLimitRect = new Rect(NameRect.x, DifficultyRect.yMax + MainPanel.height * 0.01f, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);
        TimeLimitSliderRect = new Rect(TimeLimitRect.xMax, TimeLimitRect.y, ScenarioListWindowRect.width - TimeLimitRect.width, GameManager.instance.StandardButtonSize.y);
        CommandLimitRect = new Rect(NameRect.x, TimeLimitRect.yMax + MainPanel.height * 0.01f, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);
        CommandLimitSliderRect = new Rect(CommandLimitRect.xMax, CommandLimitRect.y, ScenarioListWindowRect.width - CommandLimitRect.width, GameManager.instance.StandardButtonSize.y);
        MoneyLimitRect = new Rect(NameRect.x, CommandLimitRect.yMax + MainPanel.height * 0.01f, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);
        MoneyLimitSliderRect = new Rect(MoneyLimitRect.xMax, MoneyLimitRect.y, ScenarioListWindowRect.width - MoneyLimitRect.width, GameManager.instance.StandardButtonSize.y);

        //unit panel
        shipHullList = new ShipHullScrollList(new Rect(MainPanel.width * 0.1f, UnitsButtonRect.yMax + MainPanel.height * 0.02f, MainPanel.width * 0.8f, MainPanel.height * 0.26f), ChangeHull, CheckHullValid);

        DesignScrollWindowRect = new Rect(shipHullList.getRect().x, shipHullList.getRect().yMax + MainPanel.height * 0.02f, shipHullList.getRect().width, shipHullList.getRect().height);
        DesignScrollViewRect = new Rect(0, 0, DesignScrollWindowRect.width * 0.95f, DesignScrollWindowRect.height * 1.02f);

        PlayerButtonRect = new Rect(MainPanel.width / 2f - GameManager.instance.StandardButtonSize.x / 2f, DesignScrollWindowRect.yMax + MainPanel.height * 0.02f, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);
        EnemyButtonRect = new Rect(PlayerButtonRect.x, PlayerButtonRect.yMax, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);
        AllyButtonRect = new Rect(EnemyButtonRect.x, EnemyButtonRect.yMax, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);
        NeutralButtonRect = new Rect(AllyButtonRect.x, AllyButtonRect.yMax, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);

        ShipNameRect = new Rect(SetupButtonRect.x, NeutralButtonRect.yMax + MainPanel.height * 0.01f, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);
        ShipNameFieldRect = new Rect(ShipNameRect.xMax, ShipNameRect.y, shipHullList.getRect().width - ShipNameRect.width, GameManager.instance.StandardButtonSize.y);
        ShipLevelRect = new Rect(ShipNameRect.x, ShipNameRect.yMax + MainPanel.height * 0.01f, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);
        ShipLevelSliderRect = new Rect(ShipLevelRect.xMax, ShipLevelRect.y, shipHullList.getRect().width - ShipLevelRect.width, GameManager.instance.StandardButtonSize.y);

        ClearShipsButtonRect = new Rect(UnitsButtonRect.x, ShipLevelRect.yMax + MainPanel.height * 0.02f, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);

        DeploymentArea = ResourceManager.CreateDeploymentArea(new Vector3(-40, 0, 0));
        DeploymentArea.transform.localScale = new Vector3(2.5f, 1f, 2.5f);

        shipHullList.CheckFirstHull(ChangeHull);
        BuildScenarioList();

        ResetCamera();
        Camera.main.transform.position = new Vector3(-40, Camera.main.transform.position.y, Camera.main.transform.position.z);
    }

    public override void Update()
    {
        base.Update();

        mousePosition = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseScreen();
        }

        if (!ScreenElementsContains(mousePosition))
        {
            Ray ray;
            RaycastHit hit;

            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //If we hit...
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (DummyUnit != null)
                {
                    if (!PivotSet)
                        DummyUnit.transform.position = new Vector3(hit.point.x, 1f, hit.point.z);
                    if (Input.GetMouseButtonDown(1))
                    {
                        DeselectDesignData();
                    }
                    else if (Input.GetMouseButtonDown(0))
                    {
                        PivotPoint = DummyUnit.transform.position;
                        PivotSet = true;
                    }
                    else if (Input.GetMouseButton(0))
                    {
                        Vector3 Direction = (new Vector3(hit.point.x, 1f, hit.point.z) - PivotPoint);
                        if (Direction.magnitude > DummyShipRadius)
                        {
                            Quaternion Rotation = Quaternion.LookRotation(Direction);
                            DummyUnit.transform.rotation = Rotation;
                        }
                    }
                    else if (Input.GetMouseButtonUp(0))
                    {
                        GameObject PlacedShip = ResourceManager.CreateShip(selectedDesign.Design.GetHull(), DummyUnit.transform.position, DummyUnit.transform.rotation);
                        PlacedObjects.Add(PlacedShip);
                        ScenarioShipData data = new ScenarioShipData(selectedDesign.Design.Hull, selectedDesign.Design.Name, shipName, shipLevel);

                        if (PlayerSelected)
                        {
                            PlacedShip.transform.GetChild(0).gameObject.layer = 9;
                            PlayerShips.Add(PlacedShip, data);
                            attachMiniMapObject(PlacedShip, ResourceManager.gameConstants.Highlight_Player.GetColor());
                        }
                        else if (EnemySelected)
                        {
                            PlacedShip.transform.GetChild(0).gameObject.layer = 10;
                            EnemyShips.Add(PlacedShip, data);
                            attachMiniMapObject(PlacedShip, ResourceManager.gameConstants.Highlight_Enemy.GetColor());
                        }
                        else if (AllySelected)
                        {
                            PlacedShip.transform.GetChild(0).gameObject.layer = 11;
                            AlliedShips.Add(PlacedShip, data);
                            attachMiniMapObject(PlacedShip, ResourceManager.gameConstants.Highlight_Ally.GetColor());
                        }
                        else if (NeutralSelected)
                        {
                            PlacedShip.transform.GetChild(0).gameObject.layer = 13;
                            NeutralShips.Add(PlacedShip, data);
                            attachMiniMapObject(PlacedShip, ResourceManager.gameConstants.Highlight_Neutral.GetColor());
                        }

                        PivotSet = false;
                    }
                }
                else
                {
                    if (Input.GetMouseButton(1))
                    {
                        //Debug.Log(hit.transform.gameObject.name);
                        if (PlacedObjects.Contains(hit.transform.gameObject))
                        {
                            RemovePlacedObject(hit.transform.gameObject);
                        }
                    }
                }
            }
        }
    }

    public override void Draw()
    {
        GUI.Box(MainPanel, "", GameManager.instance.standardBackGround);

        //Tabs
        if (GUI.Button(SetupButtonRect, "Setup", GameManager.instance.standardButtonStyle))
        {
            state = State.Setup;
            DeselectDesignData();
            PlayMainButtonClick();
        }
        if (GUI.Button(UnitsButtonRect, "Units", GameManager.instance.standardButtonStyle))
        {
            state = State.Units;
            DeselectDesignData();
            PlayMainButtonClick();
        }
        if (GUI.Button(MiscButtonRect, "Misc", GameManager.instance.standardButtonStyle))
        {
            state = State.Misc;
            DeselectDesignData();
            PlayMainButtonClick();
        }

        if (state == State.Setup)
        {
            DrawSetup();
        }
        else if (state == State.Units)
        {
            DrawUnits();
        }
        else
        {
            DrawMisc();
        }

        if(GUI.Button(CloseButtonRect, "Close", GameManager.instance.standardButtonStyle))
        {
            CloseScreen();
        }
        

        miniMap.Draw();
    }

    void DrawSetup()
    {
        ScenarioListPosition = GUI.BeginScrollView(ScenarioListWindowRect, ScenarioListPosition, ScenarioListViewRect);
        foreach (ScenarioListEntry entry in Scenarios)
        {
            entry.Draw(selectedScenario);
        }
        GUI.EndScrollView();

        GUI.Label(NameRect, "Name: ");
        ScenarioName = GUI.TextField(NameFieldRect, ScenarioName);

        ScenarioDescription = GUI.TextArea(DescriptionFieldRect, ScenarioDescription);

        GUI.Label(DifficultyRect, "Difficulty: ");
        if(Difficulty == ScenarioDifficulty.normal)
        {
            if(GUI.Button(DifficultyButtonRect, "Normal", GameManager.instance.standardButtonStyle))
            {
                Difficulty = ScenarioDifficulty.hard;
                PlayMainButtonClick();
            }
        }
        else if(Difficulty == ScenarioDifficulty.hard)
        {
            if (GUI.Button(DifficultyButtonRect, "Hard", GameManager.instance.standardButtonStyle))
            {
                Difficulty = ScenarioDifficulty.veryHard;
                PlayMainButtonClick();
            }
        }
        else if(Difficulty == ScenarioDifficulty.veryHard)
        {
            if (GUI.Button(DifficultyButtonRect, "Very Hard", GameManager.instance.standardButtonStyle))
            {
                Difficulty = ScenarioDifficulty.veryEasy;
                PlayMainButtonClick();
            }
        }
        else if(Difficulty == ScenarioDifficulty.veryEasy)
        {
            if (GUI.Button(DifficultyButtonRect, "Very Easy", GameManager.instance.standardButtonStyle))
            {
                Difficulty = ScenarioDifficulty.easy;
                PlayMainButtonClick();
            }
        }
        else
        {
            if (GUI.Button(DifficultyButtonRect, "Easy", GameManager.instance.standardButtonStyle))
            {
                Difficulty = ScenarioDifficulty.normal;
                PlayMainButtonClick();
            }
        }

        GUI.Label(TimeLimitRect, "Time: " + GetFormatedTime(TimeLimit));
        GUI.Box(TimeLimitSliderRect, "", GameManager.instance.standardBackGround);
        TimeLimit = GUI.HorizontalSlider(TimeLimitSliderRect, TimeLimit, 60, 1800);

        GUI.Label(CommandLimitRect, "Command: " + CommandLimit.ToString());
        GUI.Box(CommandLimitSliderRect, "", GameManager.instance.standardBackGround);
        CommandLimit = (int)GUI.HorizontalSlider(CommandLimitSliderRect, CommandLimit, 0, 200);

        GUI.Label(MoneyLimitRect, "Money: " + MoneyLimit.ToString("#,##0.#"));
        GUI.Box(MoneyLimitSliderRect, "", GameManager.instance.standardBackGround);
        MoneyLimit = GUI.HorizontalSlider(MoneyLimitSliderRect, MoneyLimit, 0, 100000);
        MoneyLimit = Mathf.Round(MoneyLimit / 10f) * 10;

        if (ScenarioName != "")
        {
            if (GUI.Button(SaveButtonRect, "Save", GameManager.instance.standardButtonStyle))
            {
                SaveScenario();
                PlayMainButtonClick();
            }
        }
        else
        {
            GUI.enabled = false;
            if (GUI.Button(SaveButtonRect, "Save", GameManager.instance.standardButtonStyle))
            {
            }
            GUI.enabled = true;
        }
        if (selectedScenario != null)
        {
            if (GUI.Button(LoadButtonRect, "Load", GameManager.instance.standardButtonStyle))
            {
                LoadScenario(selectedScenario);
                PlayMainButtonClick();
            }
            if (GUI.Button(DeleteButtonRect, "Delete", GameManager.instance.standardButtonStyle))
            {
                DeleteScenario(selectedScenario);
                PlayMainButtonClick();
            }
        }
        else
        {
            GUI.enabled = false;
            if (GUI.Button(LoadButtonRect, "Load", GameManager.instance.standardButtonStyle))
            {
            }
            if (GUI.Button(DeleteButtonRect, "Delete", GameManager.instance.standardButtonStyle))
            {
            }
            GUI.enabled = true;
        }
    }

    void DrawUnits()
    {
        shipHullList.Draw();

        DesignScrollPostion = GUI.BeginScrollView(DesignScrollWindowRect, DesignScrollPostion, DesignScrollViewRect);
        foreach (DesignDataListEntry entry in DesignDataList)
        {
            entry.Draw(selectedDesign);
        }
        GUI.EndScrollView();

        if (GUI.Toggle(PlayerButtonRect, PlayerSelected, "Player"))
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
        if (GUI.Toggle(EnemyButtonRect, EnemySelected, "Enemy"))
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
        if (GUI.Toggle(AllyButtonRect, AllySelected, "Ally"))
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
        if (GUI.Toggle(NeutralButtonRect, NeutralSelected, "Neutral"))
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

        GUI.Label(ShipNameRect, "Display Name: ");
        shipName = GUI.TextField(ShipNameFieldRect, shipName);
        GUI.Label(ShipLevelRect, "Level: " + shipLevel.ToString());
        GUI.Box(ShipLevelSliderRect, "", GameManager.instance.standardBackGround);
        shipLevel = (int)GUI.HorizontalSlider(ShipLevelSliderRect, shipLevel, 0, 10);

        if(GUI.Button(ClearShipsButtonRect, "Clear"))
        {
            DestroyPlacedObjects();
        }
    }

    void DrawMisc()
    {

    }

    void PlaceShip()
    {

    }

    void RemovePlacedObject(GameObject placedObject)
    {
        PlacedObjects.Remove(placedObject);
        PlayerShips.Remove(placedObject);
        EnemyShips.Remove(placedObject);
        AlliedShips.Remove(placedObject);
        NeutralShips.Remove(placedObject);
        Object.Destroy(placedObject);
    }

    void SaveScenario()
    {
        Scenario scenario = new Scenario();

        scenario.Name = ScenarioName;
        scenario.Description = ScenarioDescription;
        scenario.Difficulty = Difficulty;
        scenario.TimeLimit = TimeLimit;
        scenario.CommandLimit = CommandLimit;
        scenario.MoneyLimit = MoneyLimit;
        scenario.DeploymentAreaPosition = DeploymentArea.transform.position;

        foreach(KeyValuePair<GameObject, ScenarioShipData> keyVal in PlayerShips)
        {
            Scenario.ShipEntry shipEntry = new Scenario.ShipEntry(keyVal.Value.DisplayName, keyVal.Value.Hull, keyVal.Value.Design, keyVal.Value.Level, keyVal.Key);
            scenario.PlayerShips.Add(shipEntry);
        }
        foreach (KeyValuePair<GameObject, ScenarioShipData> keyVal in EnemyShips)
        {
            Scenario.ShipEntry shipEntry = new Scenario.ShipEntry(keyVal.Value.DisplayName, keyVal.Value.Hull, keyVal.Value.Design, keyVal.Value.Level, keyVal.Key);
            scenario.EnemyShips.Add(shipEntry);
        }
        foreach (KeyValuePair<GameObject, ScenarioShipData> keyVal in AlliedShips)
        {
            Scenario.ShipEntry shipEntry = new Scenario.ShipEntry(keyVal.Value.DisplayName, keyVal.Value.Hull, keyVal.Value.Design, keyVal.Value.Level, keyVal.Key);
            scenario.AlliedShips.Add(shipEntry);
        }
        foreach (KeyValuePair<GameObject, ScenarioShipData> keyVal in NeutralShips)
        {
            Scenario.ShipEntry shipEntry = new Scenario.ShipEntry(keyVal.Value.DisplayName, keyVal.Value.Hull, keyVal.Value.Design, keyVal.Value.Level, keyVal.Key);
            scenario.NeutralShips.Add(shipEntry);
        }

        ResourceManager.SaveScenario(scenario);
        BuildScenarioList();
    }

    void SelectScenario(Scenario scenario)
    {
        selectedScenario = scenario;
    }

    void LoadScenario(Scenario scenario)
    {
        DestroyPlacedObjects();
        ScenarioName = scenario.Name;
        ScenarioDescription = scenario.Description;
        Difficulty = scenario.Difficulty;
        TimeLimit = scenario.TimeLimit;
        CommandLimit = scenario.CommandLimit;
        MoneyLimit = scenario.MoneyLimit;

        foreach(Scenario.ShipEntry entry in scenario.PlayerShips)
        {
            LoadScenarioShip(entry, 9, PlayerShips, ResourceManager.gameConstants.Highlight_Player.GetColor());
        }
        foreach (Scenario.ShipEntry entry in scenario.EnemyShips)
        {
            LoadScenarioShip(entry, 10, EnemyShips, ResourceManager.gameConstants.Highlight_Enemy.GetColor());
        }
        foreach (Scenario.ShipEntry entry in scenario.AlliedShips)
        {
            LoadScenarioShip(entry, 11, AlliedShips, ResourceManager.gameConstants.Highlight_Ally.GetColor());
        }
        foreach (Scenario.ShipEntry entry in scenario.NeutralShips)
        {
            LoadScenarioShip(entry, 13, NeutralShips, ResourceManager.gameConstants.Highlight_Neutral.GetColor());
        }
    }

    void LoadScenarioShip(Scenario.ShipEntry shipEntry, int layer, Dictionary<GameObject, ScenarioShipData> Container, Color miniMapColor)
    {
        ShipHullData hullData = ResourceManager.GetShipHull(shipEntry.Hull);
        if (hullData != null)
        {
            GameObject newShip = ResourceManager.CreateShip(hullData, shipEntry.Position, shipEntry.Rotation);
            if (newShip != null)
            {
                newShip.transform.GetChild(0).gameObject.layer = layer;
                ScenarioShipData data = new ScenarioShipData(shipEntry.Hull, shipEntry.Design, shipEntry.Name, shipEntry.Level);
                Container.Add(newShip, data);
                PlacedObjects.Add(newShip);
                attachMiniMapObject(newShip, miniMapColor);
            }
        }
    }

    protected override void ChangeHull(ShipHullData hullData)
    {
        BuildDeignDataList(hullData);
        DesignScrollPostion = Vector2.zero;
    }

    protected override void DeselectDesignData()
    {
        selectedDesign = null;
        shipName = "";
        DestroyDummyShip();
    }

    protected override void LoadDesignData(ShipDesignData designData)
    {
        selectedDesign = designData;
        shipName = selectedDesign.Design.Name;
        shipLevel = 0;
        CreateDummyShip();
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

    protected override void LoadStationDesignData(StationDesignData designData)
    {
        selectedStationDesign = designData;
        //CreateDummyStation();
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

    void BuildScenarioList()
    {
        Scenarios.Clear();
        selectedScenario = null;
        ScenarioName = "";
        ScenarioDescription = "";
        float entrySize = ScenarioListWindowRect.height / 2.4f;

        foreach (KeyValuePair<string, Scenario> keyVal in ResourceManager.Scenariors)
        {
            if (!keyVal.Value.Deleted)
            {
                Rect rect = new Rect(0, entrySize * Scenarios.Count, ScenarioListViewRect.width, entrySize);
                ScenarioListEntry entry = new ScenarioListEntry(rect, keyVal.Value, SelectScenario, GameManager.instance);
                Scenarios.Add(entry);
            }
        }
        if(Scenarios.Count > 0)
        {
            selectedScenario = Scenarios[0].scenario;
        }

        ResizeViewListWindow(ref ScenarioListViewRect, ScenarioListWindowRect, Scenarios.Count, entrySize);
    }

    void DeleteScenario(Scenario scenario)
    {
        if (selectedScenario == scenario)
        {
            selectedScenario = null;
        }
        ScenarioName = "";
        ScenarioDescription = "";
        scenario.Deleted = true;
        ResourceManager.SaveScenario(scenario);
        BuildScenarioList();
    }

    void CreateDummyShip()
    {
        if (DummyUnit != null)
        {
            Object.Destroy(DummyUnit);
        }
        DummyUnit = ResourceManager.CreateShip(selectedDesign.Hull, Vector3.zero, Quaternion.identity);
        Bounds bounds = DummyUnit.GetComponentInChildren<MeshFilter>().mesh.bounds;
        DummyShipRadius = bounds.max.magnitude;
        foreach (Collider collider in DummyUnit.GetComponentsInChildren<Collider>())
        {
            collider.enabled = false;
        }
    }

    void DestroyDummyShip()
    {
        if(DummyUnit != null)
        {
            Object.Destroy(DummyUnit);
        }
    }

    void DestroyPlacedObjects()
    {
        foreach(GameObject placedObject in PlacedObjects)
        {
            Object.Destroy(placedObject);
        }
        PlacedObjects.Clear();
        PlayerShips.Clear();
        EnemyShips.Clear();
        AlliedShips.Clear();
        NeutralShips.Clear();
    }

    void DestroyDeploymentArea()
    {
        Object.Destroy(DeploymentArea);
    }

    protected override void CloseScreen()
    {
        DestroyDummyShip();
        DestroyPlacedObjects();
        DestroyDeploymentArea();

        GameManager.instance.ChangeScreen(new ScenarioSelectScreen());
    }

    bool ScreenElementsContains(Vector2 point)
    {
        if (MainPanel.Contains(point) || miniMap.Contains(point))
            return true;
        return false;
    }

    void attachMiniMapObject(GameObject obj, Color color)
    {
        GameObject MiniMapObject = ResourceManager.CreateMiniMapObject();
        MiniMapObject.transform.position = obj.transform.position;
        MiniMapObject.transform.parent = obj.transform;
        MiniMapObject.GetComponent<Renderer>().material.color = color;
    }

    public class ScenarioShipData
    {
        public string Hull;
        public string Design;        
        public string DisplayName;
        public int Level;

        public ScenarioShipData(string hull, string designName, string displayName, int level)
        {
            Hull = hull;
            Design = designName;
            DisplayName = displayName;
            Level = level;
        }
    }

    public enum State
    {
        Setup,
        Units,
        Misc
    }
}
