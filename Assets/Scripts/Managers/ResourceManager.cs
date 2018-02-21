/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Linq;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager instance;

    [Header("Loading Controls for testing")]

    [SerializeField]
    bool doLoadMiscObjects;

    [SerializeField]
    bool doLoadSkyboxes;

    [SerializeField]
    bool doLoadExplosions;

    [SerializeField]
    bool doLoadLocalization;

    [SerializeField]
    bool doLoadTextureUI;

    [SerializeField]
    bool doLoadIcons;

    [SerializeField]
    bool doLoadAudio;

    [SerializeField]
    bool doLoadModules;

    [SerializeField]
    bool doLoadWeapons;

    [SerializeField]
    bool doLoadScenarios;

    [SerializeField]
    bool doLoadEmpires;

    [SerializeField]
    bool doLoadShips;

    [SerializeField]
    bool doLoadFighters;

    [SerializeField]
    bool doLoadTechnology;

    [SerializeField]
    bool doLoadBuildings;

    [SerializeField]
    bool doLoadPlanets;

    [SerializeField]
    bool doLoadMods;

    [Header("Data")]

    Dictionary<string, string> localizationDictionary = new Dictionary<string, string>();

    //ShipDesigns is keyed on first the hull name, then design name
    Dictionary<string, Dictionary<string, ShipDesign>> shipDesigns = new Dictionary<string, Dictionary<string, ShipDesign>>();
    Dictionary<string, Dictionary<string, StationDesign>> stationDesigns = new Dictionary<string, Dictionary<string, StationDesign>>();
    Dictionary<string, ShipDesignData> shipDesignDatas = new Dictionary<string, ShipDesignData>();
    Dictionary<string, StationDesignData> stationDesignDatas = new Dictionary<string, StationDesignData>();
    Dictionary<string, Module> modules = new Dictionary<string, Module>();
    Dictionary<string, ModuleSet> moduleSets = new Dictionary<string, ModuleSet>();
    Dictionary<string, Sprite> moduleTextures = new Dictionary<string, Sprite>();
    Dictionary<string, Weapon> weapons = new Dictionary<string, Weapon>();
    Dictionary<string, GameObject> beams = new Dictionary<string, GameObject>();
    Dictionary<string, GameObject> projectiles = new Dictionary<string, GameObject>();
    Dictionary<string, GameObject> explosions = new Dictionary<string, GameObject>();

    Dictionary<string, ShipHullData> shipHulls = new Dictionary<string, ShipHullData>();
    Dictionary<string, StationHullData> stationHulls = new Dictionary<string, StationHullData>();
    Dictionary<string, ShipSlotLayout> shipLayouts = new Dictionary<string, ShipSlotLayout>();
    Dictionary<string, StationSlotLayout> stationLayouts = new Dictionary<string, StationSlotLayout>();
    Dictionary<string, GameObject> ships = new Dictionary<string, GameObject>();
    Dictionary<string, GameObject> stations = new Dictionary<string, GameObject>();
    Dictionary<string, GameObject> fighters = new Dictionary<string, GameObject>();

    Dictionary<string, FighterDefinition> fighterDefinitions = new Dictionary<string, FighterDefinition>();

    Dictionary<string, HardPointsStored> hardPointsStored = new Dictionary<string, HardPointsStored>();

    Dictionary<string, EmpireDefinition> empires = new Dictionary<string, EmpireDefinition>();

    Dictionary<string, Sprite> iconTextures = new Dictionary<string, Sprite>();
    Dictionary<string, Sprite> UI_Textures = new Dictionary<string, Sprite>();

    Dictionary<string, Sprite> unitIcons = new Dictionary<string, Sprite>();

    Dictionary<string, GameObject> planets = new Dictionary<string, GameObject>();
    Dictionary<string, PlanetTypeDefinition> planetDefinitions = new Dictionary<string, PlanetTypeDefinition>();
    Dictionary<string, Sprite> planetTiles = new Dictionary<string, Sprite>();
    Dictionary<string, Sprite> planetIcons = new Dictionary<string, Sprite>();

    [SerializeField]
    List<Sprite> flagBackgrounds = new List<Sprite>();

    [SerializeField]
    List<Sprite> flagEmblems = new List<Sprite>();

    Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    Dictionary<string, Scenario> scenariors = new Dictionary<string, Scenario>();

    Dictionary<string, EmpireAttribute> empireAttributes = new Dictionary<string, EmpireAttribute>();

    Dictionary<string, Technology> technologies = new Dictionary<string, Technology>();
    Dictionary<string, TechnologyTree> technologyTrees = new Dictionary<string, TechnologyTree>();
    Dictionary<string, Sprite> technologyIcons = new Dictionary<string, Sprite>();

    Dictionary<string, BuildingDefinition> buildings = new Dictionary<string, BuildingDefinition>();
    Dictionary<string, Sprite> buildingIcons = new Dictionary<string, Sprite>();

    Dictionary<string, ShipNameSet> shipNameSets = new Dictionary<string, ShipNameSet>();
    Dictionary<string, CharacterNameSet> characterNameSets = new Dictionary<string, CharacterNameSet>();

    Dictionary<string, DilpomacyDialogueSet> dilpomacyDialogueSets = new Dictionary<string, DilpomacyDialogueSet>();

    Dictionary<string, Material> skyBoxes = new Dictionary<string, Material>();

    Dictionary<string, GameObject> screens = new Dictionary<string, GameObject>();

    [SerializeField]
    GameConstants gameConstants;

    [SerializeField]
    EmpireRacialTraitsConfig racialTraitsConfig;

    [SerializeField]
    Sprite errorTexture;

    [SerializeField]
    GameObject shieldsObject;

    [SerializeField]
    ModLoadConfig modLoadConfig;

    [SerializeField]
    List<ModInfo> modInfos = new List<ModInfo>();

    [SerializeField]
    Credits credits;

    void Awake()
    {
        //Make a Singleton
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
        //This stays in every scene
        DontDestroyOnLoad(gameObject);

        LoadAll();
    }

    // Use this for initialization
    void Start()
    {
        ChangeSkyBox("PurpleNebula");

        //ScreenManager.instance.ChangeScreen("MainMenuScreen");
    }

    void LoadAll()
    {
        System.DateTime LoadStartTime = System.DateTime.Now;

        LoadBuiltInScreens();

        //errorTexture = Resources.Load("Textures/ErrorTexture") as Sprite;

        LoadGameConstants(Application.streamingAssetsPath + "/Misc/GameConstants.xml");

        LoadCredits(Application.streamingAssetsPath + "/Credits/Credits.xml");

        //Load misc objects
        if (doLoadMiscObjects && Directory.Exists(Application.streamingAssetsPath + "/Objects"))
        {
            LoadGameObjects(Application.streamingAssetsPath + "/Objects/");
        }

        if (doLoadSkyboxes && Directory.Exists(Application.streamingAssetsPath + "/SkyBoxes"))
        {
            LoadSkyBoxes(Application.streamingAssetsPath + "/SkyBoxes/");
        }

        //Load explosions
        if (doLoadExplosions && Directory.Exists(Application.streamingAssetsPath + "/Explosions"))
        {
            LoadExplosionsDirectory(Application.streamingAssetsPath + "/Explosions/");
        }

        if (doLoadLocalization && Directory.Exists(Application.streamingAssetsPath + "/Localization"))
        {
            LoadLocalization(Application.streamingAssetsPath + "/Localization/");
        }

        if (Directory.Exists(Application.streamingAssetsPath + "/StationDesigns"))
        {
            //LoadStationDesigns(Application.streamingAssetsPath + "/StationDesigns");
        }

        //load icon textures
        if (doLoadIcons && Directory.Exists(Application.streamingAssetsPath + "/Icons"))
        {
            LoadTextures(Application.streamingAssetsPath + "/Icons", iconTextures);
        }
        if (doLoadTextureUI && Directory.Exists(Application.streamingAssetsPath + "/UITextures"))
        {
            LoadTextures(Application.streamingAssetsPath + "/UITextures", UI_Textures);
        }
        if (doLoadAudio && Directory.Exists(Application.streamingAssetsPath + "/Audio"))
        {
            LoadAudioClips(Application.streamingAssetsPath + "/Audio");
        }

        //Load modules
        if (doLoadModules && Directory.Exists(Application.streamingAssetsPath + "/Modules"))
        {
            LoadModulesDirectory(Application.streamingAssetsPath + "/Modules");
        }

        if (doLoadWeapons && Directory.Exists(Application.streamingAssetsPath + "/Weapons"))
        {
            LoadWeaponsDirectory(Application.streamingAssetsPath + "/Weapons");
        }

        //Load Scenarios
        if (doLoadScenarios && Directory.Exists(Application.streamingAssetsPath + "/Scenarios"))
        {
            LoadScenarios(Application.streamingAssetsPath + "/Scenarios");
        }

        //Load Empire Attributes
        if (doLoadEmpires && Directory.Exists(Application.streamingAssetsPath + "/Empires"))
        {
            LoadEmpiresDirectory(Application.streamingAssetsPath + "/Empires");
        }

        //Load in Ship asset bundles
        if (doLoadShips && Directory.Exists(Application.streamingAssetsPath + "/Ships"))
        {
            LoadShipsDirectory(Application.streamingAssetsPath + "/Ships");
        }

        if (Directory.Exists(Application.streamingAssetsPath + "/Stations"))
        {
            //LoadStationsDirectory(Application.streamingAssetsPath + "/Stations/");
        }

        if (doLoadFighters && Directory.Exists(Application.streamingAssetsPath + "/Fighters"))
        {
            LoadFightersDirectory(Application.streamingAssetsPath + "/Fighters/");
        }

        if (doLoadTechnology && Directory.Exists(Application.streamingAssetsPath + "/Technology"))
        {
            LoadTechnologyDirectory(Application.streamingAssetsPath + "/Technology/");
        }

        if (doLoadBuildings && Directory.Exists(Application.streamingAssetsPath + "/Buildings"))
        {
            LoadBuildingsDirectory(Application.streamingAssetsPath + "/Buildings/");
        }

        if (doLoadPlanets && Directory.Exists(Application.streamingAssetsPath + "/Planets"))
        {
            LoadPlanetsDirectory(Application.streamingAssetsPath + "/Planets/");
        }

        if (doLoadMods && Directory.Exists(Application.streamingAssetsPath + "/Mods"))
        {
            LoadModLoadConfig();
            LoadModInfos();
            LoadMods();
        }

        if (doLoadWeapons)
        {
            ApplyFiringRangeFactorForAllWeapons();
            ApplyFiringRangeFactorForAllModules();
        }

        if (doLoadModules)
        {
            ConnectModulesToModuleSet();
        }

        //Get the timer it took to load
        print("Resource Load time: " + (System.DateTime.Now - LoadStartTime).TotalSeconds.ToString());
    }

    public GameObject CreateShip(ShipHullData hullData, Vector3 Position, Quaternion Rotation)
    {
        GameObject NewShip = hullData.GetShipObject();
        if (NewShip != null)
        {
            NewShip = Instantiate(NewShip, Position, Rotation) as GameObject;
            NewShip.SetActive(true);
            return NewShip;
        }
        return null;
    }

    public Ship CreateCombatShip(ShipData data, Vector3 Position, Quaternion Rotation)
    {
        GameObject NewShip = CreateShip(data.designData.Hull, Position, Rotation);
        if (NewShip != null)
        {
            AttachShields(NewShip);
            Ship ship = NewShip.AddComponent<Ship>();
            data.AttachShip(ship);
            ship.SetAudioSource(NewShip.AddComponent<AudioSource>());
            ship.Initialize(data);
            return ship;
        }
        return null;
    }

    public GameObject CreateStation(StationHullData hullData, Vector3 Position, Quaternion Rotation)
    {
        GameObject NewStation = hullData.GetStationObject();
        if (NewStation != null)
        {
            NewStation = Instantiate(NewStation, Position, Rotation) as GameObject;
            NewStation.SetActive(true);
            return NewStation;
        }
        return null;
    }

    public Station CreateCombatStation(StationData data, Vector3 Position, Quaternion Rotation)
    {
        GameObject stationObject = CreateStation(data.designData.Hull, Position, Rotation);
        if(stationObject != null)
        {
            AttachShields(stationObject);
            Station station = stationObject.AddComponent<Station>();
            data.AttachCurrentStation(station);
            station.SetAudioSource(stationObject.AddComponent<AudioSource>());
            station.Initialize(data);
            return station;
        }
        return null;
    }

    public GameObject CreateFighter(FighterDefinition Definition, Vector3 Position, Quaternion Rotation)
    {
        GameObject fighterObject = Definition.GetFighterObject();
        if (fighterObject != null)
        {
            fighterObject = Instantiate(fighterObject, Position, Rotation) as GameObject;
            fighterObject.SetActive(true);
            return fighterObject;
        }
        return null;
    }

    public Fighter CreateCombatFighter(FighterDefinition Definition, Vector3 Position, Quaternion Rotation)
    {
        GameObject fighterObject = CreateFighter(Definition, Position, Rotation);
        if(fighterObject != null)
        {
            AttachShields(fighterObject);
            Fighter fighter = fighterObject.AddComponent<Fighter>();
            fighter.SetAudioSource(fighterObject.AddComponent<AudioSource>());
            fighter.Initialize(Definition);
            return fighter;
        }
        return null;
    }

    public Beam CreateBeam(Weapon weapon)
    {
        GameObject beamObject;
        if (beams.TryGetValue(weapon.WeaponEffect, out beamObject))
        {
            beamObject = Instantiate(beamObject);
            beamObject.SetActive(true);
            beamObject.transform.localScale *= weapon.Scale;
            LineRenderer lineRenderer = beamObject.GetComponent<LineRenderer>();
            lineRenderer.startWidth = weapon.Scale;
            lineRenderer.endWidth = weapon.Scale;
            return beamObject.GetComponent<Beam>();
        }
        return null;
    }

    public Projectile CreateProjectile(Weapon weapon)
    {
        GameObject projectile;
        if (projectiles.TryGetValue(weapon.WeaponEffect, out projectile))
        {
            projectile = Instantiate(projectile);
            projectile.SetActive(true);
            projectile.transform.localScale *= weapon.Scale;
            return projectile.GetComponent<Projectile>();
        }
        return null;
    }

    public PopupMessage CreatePopupMessage(Vector3 Position, string message, Color color, float lifeTime)
    {
        GameObject popupObject = new GameObject();
        popupObject.transform.position = Position;
        PopupMessage popupController = popupObject.AddComponent<PopupMessage>();
        popupController.Initialize(lifeTime, 1.2f);
        TextMesh textMesh = popupController.GetTextMesh();
        textMesh.text = message;
        textMesh.alignment = TextAlignment.Center;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.characterSize = 0.1f;
        textMesh.lineSpacing = 0.7f;
        textMesh.fontSize = 18;
        textMesh.color = color;
        return popupController;
    }

    public TextMeshController CreateWorldMessage(Vector3 Position, string message, Color color, int size)
    {
        GameObject messageObject = new GameObject();
        messageObject.transform.position = Position;
        TextMeshController messageController = messageObject.AddComponent<TextMeshController>();
        messageController.Initialize(color, size, 0.1f, 0.7f, message);
        return messageController;
    }

    public BoardingBalanceBar CreateBoardingBalanceBar(Vector3 Position)
    {
        GameObject ObjectInstance = Instantiate(Resources.Load("BoardingBalanceBar")) as GameObject;
        ObjectInstance.transform.position = Position;
        BoardingBalanceBar boardingBalanceBar = ObjectInstance.GetComponent<BoardingBalanceBar>();
        boardingBalanceBar.Initialize();
        return boardingBalanceBar;
    }

    public void CreateClickPing(Vector3 Position)
    {
        GameObject ObjectInstance = Instantiate(Resources.Load("ClickPing"), Position, Quaternion.identity) as GameObject;
        SelfDestructor detonator = ObjectInstance.AddComponent<SelfDestructor>();
        detonator.setLifeTimer(0.6f);
    }

    public void CreateExplosion(Vector3 Position, string explosionName, string explosionAudio, float Size, float lifeTime, bool AlwaysPlayAudio)
    {
        GameObject sourceExplosionObject;
        if(explosions.TryGetValue(explosionName, out sourceExplosionObject))
        {
            GameObject newExplosionObject = Instantiate(sourceExplosionObject, Position, Quaternion.identity) as GameObject;
            newExplosionObject.SetActive(true);
            newExplosionObject.transform.localScale *= Size;
            if (explosionAudio != "")
            {
                AudioManager.instance.AddEffectClip(newExplosionObject, explosionAudio, AlwaysPlayAudio);
            }
            SelfDestructor detonator = newExplosionObject.AddComponent<SelfDestructor>();
            if (lifeTime < 0)
                lifeTime = 1f;
            detonator.setLifeTimer(lifeTime);
        }
    }

    public static GameObject CreateShipOrderLine()
    {
        return Instantiate(Resources.Load("ShipOrderLine")) as GameObject;
    }

    public static GameObject CreateRangeCircle()
    {
        return Instantiate(Resources.Load("RangeCircle")) as GameObject;
    }

    public static FighterWing CreateFighterWing()
    {
        GameObject WingObject = Instantiate(Resources.Load("FighterWing")) as GameObject;
        return WingObject.GetComponent<FighterWing>();
    }

    public static GameObject CreateDirectionMarker(Vector3 Position)
    {
        GameObject marker = Instantiate(Resources.Load("DirectionMark"), Position, Quaternion.identity) as GameObject;
        return marker;
    }

    public static GameObject CreateDeploymentArea(Vector3 Position)
    {
        return Instantiate(Resources.Load("DeploymentArea"), Position, Quaternion.identity) as GameObject;
    }

    public static GameObject CreateShipFireDamage(float scale)
    {
        GameObject fireDamage = Instantiate(Resources.Load("DamageFire")) as GameObject;
        fireDamage.transform.localScale *= scale;
        return fireDamage;
    }

    public GameObject CreateMiniMapObject()
    {
        return Instantiate(Resources.Load("MiniMapObject")) as GameObject;
    }

    public GameObject CreateFleet(Vector3 Position, Quaternion Rotation)
    {
        return Instantiate(Resources.Load("Fleet"), Position, Rotation) as GameObject;
    }

    public GameObject CreateStar(string Name, Vector3 Position, Quaternion Rotation)
    {
        return Instantiate(Resources.Load("Stars/" + Name), Position, Rotation) as GameObject;
    }

    public GameObject CreatePlanet(string Name, Vector3 Position, Quaternion Rotation)
    {
        return Instantiate(Resources.Load("Planets/" + Name), Position, Rotation) as GameObject;
    }

    public GameObject CreateSector(SectorData data)
    {
        GameObject Sector = Instantiate(Resources.Load("Sector")) as GameObject;
        data.sector = Sector.GetComponent<Sector>();
        data.sector.data = data;
        Sector.transform.position = data.Position;
        return Sector;
    }

    public ShipDesignData GetShipDesignData(ShipDesign design)
    {
        ShipDesignData designData;
        if (shipDesignDatas.TryGetValue(design.Name, out designData))
        {
            return designData;
        }
        else
        {
            designData = new ShipDesignData(design);
            shipDesignDatas.Add(design.Name, designData);
            return designData;
        }
    }

    public StationDesignData GetStationDesignData(StationDesign design)
    {
        StationDesignData designData;
        if (stationDesignDatas.TryGetValue(design.Name, out designData))
        {
            return designData;
        }
        else
        {
            designData = new StationDesignData(design);
            stationDesignDatas.Add(design.Name, designData);
            return designData;
        }
    }

    public Material GetCloackedShipMaterial()
    {
        return new Material(Resources.Load("CloakedShip", typeof(Material)) as Material);
    }

    public Material GetShipDissolveMaterial()
    {
        return new Material(Resources.Load("ShipDissolver", typeof(Material)) as Material);
    }

    public Sprite GetIconTexture(string textureName)
    {
        return GetTexture(textureName, iconTextures);
    }

    public Sprite GetUITexture(string textureName)
    {
        return GetTexture(textureName, UI_Textures);
    }

    public Sprite GetModuleTexture(string textureName)
    {
        return GetTexture(textureName, moduleTextures);
    }

    public Sprite GetUnitIcon(string textureName)
    {
        return GetTexture(textureName, unitIcons);
    }

    public Sprite GetTechnologyIcon(string textureName)
    {
        return GetTexture(textureName, technologyIcons);
    }

    Sprite GetTexture(string textureName, Dictionary<string, Sprite> textureDictionary)
    {
        if(textureName != null)
        {
            Sprite texture;
            if (textureDictionary.TryGetValue(textureName, out texture))
            {
                return texture;
            }
            else
            {
                return errorTexture;
            }
        }
        return errorTexture;
    }

    public Sprite GetFlagBackground(int index)
    {
        return GetTexture(index, flagBackgrounds);
    }

    public Sprite GetFlagEmblem(int index)
    {
        return GetTexture(index, flagEmblems);
    }

    Sprite GetTexture(int index, List<Sprite> textures)
    {
        if (textures.Count == 0)
        {
            return errorTexture;
        }
        else
        {
            if (index >= textures.Count)
                index = index % textures.Count;
            else if (index < 0)
                index = (index % textures.Count + textures.Count) % textures.Count;
            return textures[index];
        }
    }

    public Sprite GetErrorTexture()
    {
        return errorTexture;
    }

    public AudioClip GetAudioClip(string name)
    {
        AudioClip clip;
        if(audioClips.TryGetValue(name, out clip))
        {
            return clip;
        }
        return null;
    }

    public void ChangeSkyBox(string SkyboxName)
    {
        if(SkyboxName != null)
        {
            Material skyBox;
            if(skyBoxes.TryGetValue(SkyboxName, out skyBox))
            {
                RenderSettings.skybox = skyBox;
            }
        }
    }

    void LoadGameObjects(string path)
    {
        if (File.Exists(path + "shields.unity3d"))
        {
            if (shieldsObject != null)
                Destroy(shieldsObject);
            AssetBundle asset = AssetBundle.LoadFromFile(path + "shields.unity3d");
            shieldsObject = (GameObject)asset.LoadAsset(asset.GetAllAssetNames()[0]);
            shieldsObject.SetActive(false);
        }
    }

    void LoadGameConstants(string path)
    {
        if (File.Exists(path))
        {
            try
            {
                gameConstants = (GameConstants)new XmlSerializer(typeof(GameConstants)).Deserialize(new FileInfo(path).OpenRead());
            }
            catch
            {
                //print("Game Constants could not load: " + path);
            }
        }
        else
        {
            print("Game constants not found: " + path);
        }
    }

    void LoadCredits(string path)
    {
        if (File.Exists(path))
        {
            try
            {
                credits = (Credits)new XmlSerializer(typeof(Credits)).Deserialize(new FileInfo(path).OpenRead());
            }
            catch
            {
                print("Game Constants could not load: " + path);
            }
        }
    }

    void LoadRacialTraitsConfig(string path)
    {
        if (File.Exists(path))
        {
            try
            {
                racialTraitsConfig = (EmpireRacialTraitsConfig)new XmlSerializer(typeof(EmpireRacialTraitsConfig)).Deserialize(new FileInfo(path).OpenRead());
            }
            catch
            {
                print("Racial Traits Config could not load: " + path);
            }          
        }
        else
        {
            //Write a blank file
            //XmlSerializer Writer = new XmlSerializer(typeof(EmpireRacialTraitsConfig));
            //FileStream file = File.Create(path);
            //Writer.Serialize(file, new EmpireRacialTraitsConfig());
            //file.Close();
            //print("Racial Traits Config file written: " + path);
        }
   }

    void LoadLocalization(string path)
    {
        string fileName = Application.systemLanguage.ToString() + ".txt";

        if (!File.Exists(path + fileName))
            fileName = "English.txt";
        if (!File.Exists(path + fileName))
            return;

        using (StreamReader Reader = new StreamReader(path + fileName))
        {
            // Read the stream to a string, and write the string to the console.
            string line;
            while ((line = Reader.ReadLine()) != null)
            {
                if (line == "")
                    continue;
                string[] parts = line.Split(' ');
                string KeyWord = parts[0];
                string Value = parts[1];
                for (int i = 2; i < parts.Length; i++)
                {
                    Value = Value + " " + parts[i];
                }

                if (localizationDictionary.ContainsKey(KeyWord))
                {
                    localizationDictionary[KeyWord] = Value;
                }
                else
                {
                    localizationDictionary.Add(KeyWord, Value);
                }
            }
        }
    }

    public string GetLocalization(string key)
    {
        string value;
        if (localizationDictionary.TryGetValue(key, out value))
        {
            return value;
        }
        return key;
    }

    void LoadModLoadConfig()
    {
        if (File.Exists(Application.streamingAssetsPath + "/Mods" + "/ModLoadConfig.xml"))
        {
            modLoadConfig = (ModLoadConfig)new XmlSerializer(typeof(ModLoadConfig)).Deserialize((Stream)new FileInfo(Application.streamingAssetsPath + "/Mods" + "/ModLoadConfig.xml").OpenRead());
        }
        else
        {
            print(Application.streamingAssetsPath + "/Mods" + "/ModLoadConfig.xml" + " not found");
        }
    }

    void LoadModInfos()
    {
        modInfos.Clear();
        if (Directory.Exists(Application.streamingAssetsPath + "/Mods"))
        {
            FileInfo[] files = GetFilesByType(Application.streamingAssetsPath + "/Mods", "ModInfo.xml");
            foreach (FileInfo file in files)
            {
                try
                {
                    ModInfo modInfo = new ModInfo();
                    modInfo = (ModInfo)new XmlSerializer(typeof(ModInfo)).Deserialize((Stream)file.OpenRead());
                    modInfo.SetPath(file.Directory.FullName);
                    modInfos.Add(modInfo);
                    //print(modInfo.Name + " mod info loaded");
                }
                catch
                {
                    print("Failed to load: " + file.Name);
                }
            }
        }
    }

    void LoadMods()
    {
        foreach (ModLoadConfig.ModEntry entry in modLoadConfig.ModLoadOrder)
        {
            if (entry.Load)
            {
                foreach (ModInfo modInfo in modInfos)
                {
                    if (entry.Name == modInfo.Name)
                    {
                        LoadMod(modInfo);
                    }
                }
            }
        }
    }

    void LoadMod(ModInfo mod)
    {
        string path = mod.GetPath();

        //Load localization
        LoadLocalization(path + "/Localization/");

        //Load objects
        if (Directory.Exists(path + "/Objects"))
        {
            LoadGameObjects(path + "/Objects/");
        }

        //Load explosions
        if (Directory.Exists(path + "/Explosions"))
        {
            LoadExplosionsDirectory(path + "/Explosions/");
        }

        if (Directory.Exists(path + "/Audio"))
        {
            LoadAudioClips(path + "/Audio");
        }

        LoadWeaponsDirectory(path + "/Weapons");

        //Load modules
        LoadModulesDirectory(path + "/Modules", mod);

        //Load Scenarios
        if (Directory.Exists(path + "/Scenarios"))
        {
            LoadScenarios(path + "/Scenarios");
        }

        //Load in Ship asset bundles
        if (Directory.Exists(path + "/Ships"))
        {
            LoadShipsDirectory(path + "/Ships");
        }

        if (Directory.Exists(path + "/Stations"))
        {
            LoadStationsDirectory(path + "/Stations/");
        }

        if (Directory.Exists(path + "/Fighters"))
        {
            LoadFightersDirectory(path + "/Fighters/");
        }

        if (Directory.Exists(Application.streamingAssetsPath + "/Technology"))
        {

        }
    }

    void LoadShips(string path)
    {
        FileInfo[] files = GetFilesByType(path, "*.unity3d");
        foreach (FileInfo file in files)
        {
            AssetBundle asset = AssetBundle.LoadFromFile(path + file.Name);
            foreach (string assetName in asset.GetAllAssetNames())
            {
                try
                {
                    LoadShip((GameObject)asset.LoadAsset(assetName));
                }
                catch
                {
                    print("Failed to load ship: " + assetName);
                }         
            }
        }
    }

    void LoadShip(GameObject shipObject)
    {
        //GameObject shipMesh = shipObject.transform.GetChild(0).gameObject;
        //Set tags and layers
        shipObject.tag = "Ship";

        GameObject ExistingShip;
        if (ships.TryGetValue(shipObject.name, out ExistingShip))
        {
            //Destroy(ExistingShip);
            ExistingShip = shipObject;
        }
        else
        {
            ships.Add(shipObject.name, shipObject);
        }
        shipObject.SetActive(false);
    }

    void LoadStations(string path)
    {
        FileInfo[] files = GetFilesByType(path, "*.unity3d");
        foreach (FileInfo file in files)
        {
            AssetBundle asset = AssetBundle.LoadFromFile(path + file.Name);
            foreach (string assetName in asset.GetAllAssetNames())
            {
                try
                {
                    LoadStation((GameObject)asset.LoadAsset(assetName));
                }
                catch
                {
                    print("Failed to load station: " + assetName);
                }
            }
        }
    }

    void LoadStation(GameObject stationObject)
    {
        //GameObject stationMesh = stationObject.transform.GetChild(0).gameObject;
        //Set tags and layers
        stationObject.tag = "Station";

        GameObject ExistingStation;
        if (stations.TryGetValue(stationObject.name, out ExistingStation))
        {
            //Destroy(ExistingStation);
            ExistingStation = stationObject;
        }
        else
        {
            stations.Add(stationObject.name, stationObject);
        }
        stationObject.SetActive(false);
    }

    void LoadFighters(string path)
    {
        FileInfo[] files = GetFilesByType(path, "*.unity3d");
        foreach (FileInfo file in files)
        {
            AssetBundle asset = AssetBundle.LoadFromFile(path + file.Name);
            foreach (string assetName in asset.GetAllAssetNames())
            {
                try
                {
                    LoadFighter((GameObject)asset.LoadAsset(assetName));
                }
                catch
                {
                    print("Failed to load fighter: " + assetName);
                }
            }
        }
    }

    void LoadFighter(GameObject fighterObject)
    {
        //GameObject fighterMesh = fighterObject.transform.GetChild(0).gameObject;

        //Add Tags
        fighterObject.tag = "Fighter";

        //Fighter fighter = fighterObject.GetComponent<Fighter>();

        GameObject ExistingFighter;
        if (fighters.TryGetValue(fighterObject.name, out ExistingFighter))
        {
            //Destroy(ExistingFighter);
            ExistingFighter = fighterObject;
        }
        else
        {
            fighters.Add(fighterObject.name, fighterObject);
        }
        fighterObject.SetActive(false);
    }

    void LoadPlanets(string path)
    {
        FileInfo[] files = GetFilesByType(path, "*.unity3d");
        foreach (FileInfo file in files)
        {
            AssetBundle asset = AssetBundle.LoadFromFile(path + file.Name);
            foreach (string assetName in asset.GetAllAssetNames())
            {
                try
                {
                    LoadPlanet((GameObject)asset.LoadAsset(assetName));
                }
                catch
                {
                    print("Failed to load planet: " + assetName);
                }
            }
        }
    }

    void LoadPlanet(GameObject planetObject)
    {
        //GameObject shipMesh = planetObject.transform.GetChild(0).gameObject;
        //Set tags and layers
        planetObject.tag = "Planet";

        GameObject existingPlanet;
        if (planets.TryGetValue(planetObject.name, out existingPlanet))
        {
            //Destroy(ExistingShip);
            existingPlanet = planetObject;
        }
        else
        {
            planets.Add(planetObject.name, planetObject);
        }
        planetObject.SetActive(false);
    }

    void LoadSkyBoxes(string path)
    {
        FileInfo[] files = GetFilesByType(path, "*.unity3d");
        foreach (FileInfo file in files)
        {
            AssetBundle asset = AssetBundle.LoadFromFile(path + file.Name);
            foreach (string assetName in asset.GetAllAssetNames())
            {
                try
                {
                    Material SkyboxMaterial = (Material)asset.LoadAsset(assetName);

                    Material ExistingSkyBox;
                    if(skyBoxes.TryGetValue(SkyboxMaterial.name, out ExistingSkyBox))
                    {
                        ExistingSkyBox = SkyboxMaterial;
                    }
                    else
                    {
                        skyBoxes.Add(SkyboxMaterial.name, SkyboxMaterial);
                    }
                }
                catch
                {
                    print("Failed to load skybox: " + assetName);
                }
            }
        }
    }

    public void AttachShields(GameObject UnitNeedingShields)
    {
        GameObject ParentMesh = UnitNeedingShields.transform.GetChild(0).gameObject;
        MeshFilter ParentMeshFilter = ParentMesh.GetComponent<MeshFilter>();

        GameObject newShields = Instantiate(shieldsObject) as GameObject;
        newShields.SetActive(true);
        newShields.tag = "HitBox";

        newShields.transform.position = ParentMesh.transform.position;
        newShields.transform.rotation = ParentMesh.transform.rotation;
        newShields.transform.parent = ParentMesh.transform;

        newShields.GetComponent<MeshFilter>().mesh = ParentMeshFilter.mesh;
        newShields.GetComponent<MeshCollider>().sharedMesh = ParentMeshFilter.mesh;
    }

    void LoadBuiltInScreens()
    {
        Object[] ScreenObjects = Resources.LoadAll("UI/Screens");

        foreach (Object screenObject in ScreenObjects)
        {
            GameObject gameObjectScreen = screenObject as GameObject;

            screens.Add(gameObjectScreen.name, gameObjectScreen);
        }
    }

    void LoadShipDesigns(string path)
    {
        FileInfo[] files = GetFilesByType(path, "*.xml");
        foreach (FileInfo file in files)
        {
            try
            {
                ShipDesign design = (ShipDesign)new XmlSerializer(typeof(ShipDesign)).Deserialize(file.OpenRead());
                design.SetPath(file.FullName);
                StoreShipDesign(design);
            }
            catch
            {
                print("Failed to load design: " + file.Name);
            }
        }
    }

    void LoadStationDesigns(string path)
    {
        FileInfo[] files = GetFilesByType(path, "*.xml");
        foreach (FileInfo file in files)
        {
            try
            {
                StationDesign design = (StationDesign)new XmlSerializer(typeof(StationDesign)).Deserialize(file.OpenRead());
                design.SetPath(file.FullName);
                StoreStationDesign(design);
            }
            catch
            {
                print("Failed to load design: " + file.Name);
            }
        }
    }

    void LoadAudioClips(string path)
    {
        //Load audio asset bundles
        FileInfo[] files = GetFilesByType(path, "*.unity3d");
        foreach (FileInfo file in files)
        {
            AssetBundle asset = AssetBundle.LoadFromFile(path + "/" + file.Name);
            foreach (string assetName in asset.GetAllAssetNames())
            {
                GameObject audioObject = (GameObject)Instantiate(asset.LoadAsset(assetName));
                AudioContainer container = audioObject.GetComponent<AudioContainer>();
                foreach(AudioClip clip in container.AudioList)
                {
                    if (audioClips.ContainsKey(clip.name))
                    {
                        audioClips[clip.name] = clip;
                    }
                    else
                    {
                        audioClips.Add(clip.name, clip);
                    }
                }
                Destroy(audioObject);
            }
        }

        //Load individual audio files
        files = GetFilesByType(path, "*.wav", "*.ogg");
        foreach (FileInfo file in files)
        {
            StartCoroutine(LoadClipCoroutine(file));
        }
    }

    IEnumerator<WWW> LoadClipCoroutine(FileInfo file)
    {
        string name = Path.GetFileNameWithoutExtension(file.Name);
        WWW www = new WWW("file://" + file.FullName);
        yield return www;
        AudioClip clip = www.GetAudioClip();
        if (audioClips.ContainsKey(name))
        {
            audioClips[name] = clip;
        }
        else
        {
            audioClips.Add(name, clip);
        }
    }

    void LoadTextures(string path, Dictionary<string, Sprite> dictionary)
    {
        FileInfo[] files = GetFilesByType(path, "*.png");
        foreach (FileInfo file in files)
        {
            Texture2D texture = new Texture2D(2, 2);
            try
            {
                byte[] data = File.ReadAllBytes(file.FullName);
                texture.LoadImage(data);
                Sprite sprite = TextureToSprite(texture);
                string name = Path.GetFileNameWithoutExtension(file.Name);
                if (dictionary.ContainsKey(name))
                {
                    dictionary[name] = sprite;
                }
                else
                {
                    dictionary.Add(name, sprite);
                }
                //print("Loaded Texture: " + name);
            }
            catch
            {
                print("Failed to load texture: " + file.Name);
            }
        }
    }

    void LoadTextures(string path, List<Sprite> list)
    {
        FileInfo[] files = GetFilesByType(path, "*.png");
        foreach (FileInfo file in files)
        {
            Texture2D texture = new Texture2D(2, 2);
            try
            {
                byte[] data = File.ReadAllBytes(file.FullName);
                texture.LoadImage(data);
                Sprite sprite = TextureToSprite(texture);
                list.Add(sprite);
            }
            catch
            {
                print("Failed to load texture: " + file.Name);
            }
        }
    }

    Sprite TextureToSprite(Texture2D texture)
    {
        if (texture != null)
        {
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0), 100f);
        }

        return null;
    }

    void LoadModules(string path)
    {
        FileInfo[] files = GetFilesByType(path, "*.xml");
        foreach (FileInfo file in files)
        {
            Module module = new Module();
            try
            {
                module = (Module)new XmlSerializer(typeof(Module)).Deserialize((Stream)file.OpenRead());
                string name = Path.GetFileNameWithoutExtension(file.Name);
                if (modules.ContainsKey(name))
                {
                    modules[name] = module;
                }
                else
                {
                    modules.Add(name, module);
                }
                //print("Loaded Module: " + name);
            }
            catch
            {
                print("Failed to load: " + file.Name);
            }
        }
    }

    void LoadModuleSets(string path)
    {
        FileInfo[] files = GetFilesByType(path, "*.xml");
        foreach (FileInfo file in files)
        {
            ModuleSet moduleSet = new ModuleSet();
            try
            {
                moduleSet = (ModuleSet)new XmlSerializer(typeof(ModuleSet)).Deserialize(file.OpenRead());
                string name = Path.GetFileNameWithoutExtension(file.Name);

                if (moduleSets.ContainsKey(name))
                {
                    moduleSets[name] = moduleSet;
                }
                else
                {
                    moduleSets.Add(name, moduleSet);
                }
                //print("Loaded Module Set: " + name);
            }
            catch
            {
                print("Failed to load: " + file.Name);
            }
        }
    }

    void LoadModuleSets(string path, ModInfo modInfo)
    {
        FileInfo[] files = GetFilesByType(path, "*.xml");
        foreach (FileInfo file in files)
        {
            ModuleSet moduleSet = new ModuleSet();
            try
            {
                moduleSet = (ModuleSet)new XmlSerializer(typeof(ModuleSet)).Deserialize(file.OpenRead());
                moduleSet.SetParentMod(modInfo);
                string name = Path.GetFileNameWithoutExtension(file.Name);

                if (moduleSets.ContainsKey(name))
                {
                    moduleSets[name] = moduleSet;
                }
                else
                {
                    moduleSets.Add(name, moduleSet);
                }
                //print("Loaded Module Set: " + name);
            }
            catch
            {
                print("Failed to load: " + file.Name);
            }
        }
    }

    void LoadWeapons(string path)
    {
        FileInfo[] files = GetFilesByType(path, "*.xml");
        foreach (FileInfo file in files)
        {
            Weapon weapon = new Weapon();
            try
            {
                weapon = (Weapon)new XmlSerializer(typeof(Weapon)).Deserialize(file.OpenRead());
                string name = Path.GetFileNameWithoutExtension(file.Name);
                //weapon.SetName(name);
                if (weapons.ContainsKey(name))
                {
                    weapons[name] = weapon;
                }
                else
                {
                    weapons.Add(name, weapon);
                }
                //print("Loaded Weapon: " + name);
            }
            catch
            {
                print("Failed to load: " + file.Name);
            }
        }
    }

    void LoadBeams(string path)
    {
        FileInfo[] files = GetFilesByType(path, "*.unity3d");
        foreach (FileInfo file in files)
        {
            AssetBundle asset;
            try
            {
                asset = AssetBundle.LoadFromFile(path + file.Name);
                string name = Path.GetFileNameWithoutExtension(file.Name);
                GameObject beam = (GameObject)asset.LoadAsset(asset.GetAllAssetNames()[0]);
                if (beam.GetComponent<Beam>() == null)
                    beam.AddComponent<Beam>();

                //Store in dictionaries
                if (beams.ContainsKey(name))
                {
                    beams[name] = beam;
                }
                else
                {
                    beams.Add(name, beam);
                }

                beam.SetActive(false);
                //print("Loaded Beam: " + name);
            }
            catch
            {
                print("Failed to load beam: " + file.Name);
            }
        }
    }

    void LoadScenarios(string path)
    {
        FileInfo[] files = GetFilesByType(path, "*.xml");

        foreach (FileInfo file in files)
        {
            Scenario scenario = new Scenario();
            try
            {
                scenario = (Scenario)new XmlSerializer(typeof(Scenario)).Deserialize(file.OpenRead());
                scenario.SetPath(file.FullName);
                string name = Path.GetFileNameWithoutExtension(file.Name);
                if (scenariors.ContainsKey(name))
                {
                    scenariors[name] = scenario;
                }
                else
                {
                    scenariors.Add(name, scenario);
                }
                //print("Loaded Scenario: " + name);
            }
            catch
            {
                print("Failed to load Scenario: " + file.Name);
            }
        }
    }

    void LoadEmpireDefinitions(string path)
    {
        FileInfo[] files = GetFilesByType(path, "*.xml");

        foreach (FileInfo file in files)
        {
            try
            {
                EmpireDefinition empireDefinition = (EmpireDefinition)new XmlSerializer(typeof(EmpireDefinition)).Deserialize(file.OpenRead());
                string name = Path.GetFileNameWithoutExtension(file.Name);
                if (empires.ContainsKey(name))
                {
                    empires[name] = empireDefinition;
                }
                else
                {
                    empires.Add(name, empireDefinition);
                }
            }
            catch
            {
                print("Failed to load empire definition: " + file.Name);
            }
        }
    }

    void LoadEmpireAttributes(string path)
    {
        FileInfo[] files = GetFilesByType(path, "*.xml");

        foreach (FileInfo file in files)
        {
            try
            {
                EmpireAttribute empireAttribute = (EmpireAttribute)new XmlSerializer(typeof(EmpireAttribute)).Deserialize(file.OpenRead());
                string name = Path.GetFileNameWithoutExtension(file.Name);
                if (empireAttributes.ContainsKey(name))
                {
                    empireAttributes[name] = empireAttribute;
                }
                else
                {
                    empireAttributes.Add(name, empireAttribute);
                }
            }
            catch
            {
                print("Failed to load empire attribute: " + file.Name);
            }
        }
    }

    void LoadShipHullDatas(string path)
    {
        FileInfo[] files = GetFilesByType(path, "*.xml");

        foreach (FileInfo file in files)
        {
            try
            {
                ShipHullData shipHullData = (ShipHullData)new XmlSerializer(typeof(ShipHullData)).Deserialize(file.OpenRead());
                string name = Path.GetFileNameWithoutExtension(file.Name);
                if (shipHulls.ContainsKey(name))
                {
                    shipHulls[name] = shipHullData;
                }
                else
                {
                    shipHulls.Add(name, shipHullData);
                }
            }
            catch
            {
                print("Failed to load ship hull data: " + file.Name);
            }
        }
    }

    void LoadStationHullDatas(string path)
    {
        FileInfo[] files = GetFilesByType(path, "*.xml");

        foreach (FileInfo file in files)
        {
            try
            {
                StationHullData stationHullData = (StationHullData)new XmlSerializer(typeof(StationHullData)).Deserialize(file.OpenRead());
                string name = Path.GetFileNameWithoutExtension(file.Name);
                if (stationHulls.ContainsKey(name))
                {
                    stationHulls[name] = stationHullData;
                }
                else
                {
                    stationHulls.Add(name, stationHullData);
                }
            }
            catch
            {
                print("Failed to load station hull data: " + file.Name);
            }
        }
    }

    void LoadShipLayouts(string path)
    {
        FileInfo[] files = GetFilesByType(path, "*.xml");

        foreach (FileInfo file in files)
        {
            try
            {
                ShipSlotLayout slotLayout = (ShipSlotLayout)new XmlSerializer(typeof(ShipSlotLayout)).Deserialize(file.OpenRead());
                string name = Path.GetFileNameWithoutExtension(file.Name);
                if (shipLayouts.ContainsKey(name))
                {
                    shipLayouts[name] = slotLayout;
                }
                else
                {
                    shipLayouts.Add(name, slotLayout);
                }
            }
            catch
            {
                print("Failed to load ship layout: " + file.Name);
            }
        }
    }

    void LoadStationLayouts(string path)
    {
        FileInfo[] files = GetFilesByType(path, "*.xml");

        foreach (FileInfo file in files)
        {
            try
            {
                StationSlotLayout slotLayout = (StationSlotLayout)new XmlSerializer(typeof(StationSlotLayout)).Deserialize(file.OpenRead());
                string name = Path.GetFileNameWithoutExtension(file.Name);
                if (shipLayouts.ContainsKey(name))
                {
                    stationLayouts[name] = slotLayout;
                }
                else
                {
                    stationLayouts.Add(name, slotLayout);
                }
            }
            catch
            {
                print("Failed to load station layout: " + file.Name);
            }
        }
    }

    void LoadHardpoints(string path)
    {
        FileInfo[] files = GetFilesByType(path, "*.xml");

        foreach (FileInfo file in files)
        {
            try
            {
                HardPointsStored hardPoints = (HardPointsStored)new XmlSerializer(typeof(HardPointsStored)).Deserialize(file.OpenRead());
                string name = Path.GetFileNameWithoutExtension(file.Name);
                if (hardPointsStored.ContainsKey(name))
                {
                    hardPointsStored[name] = hardPoints;
                }
                else
                {
                    hardPointsStored.Add(name, hardPoints);
                }
            }
            catch
            {
                print("Failed to load ship hard points: " + file.Name);
            }
        }
    }

    void LoadFighterDefinitions(string path)
    {
        FileInfo[] files = GetFilesByType(path, "*.xml");

        foreach (FileInfo file in files)
        {
            try
            {
                FighterDefinition fighterDefinition = (FighterDefinition)new XmlSerializer(typeof(FighterDefinition)).Deserialize(file.OpenRead());
                string name = Path.GetFileNameWithoutExtension(file.Name);
                if (fighterDefinitions.ContainsKey(name))
                {
                    fighterDefinitions[name] = fighterDefinition;
                }
                else
                {
                    fighterDefinitions.Add(name, fighterDefinition);
                }
            }
            catch
            {
                print("Failed to load fighter definition: " + file.Name);
            }
        }
    }

    void LoadTechnologies(string path)
    {
        FileInfo[] files = GetFilesByType(path, "*.xml");
        foreach (FileInfo file in files)
        {
            Technology technology = new Technology();
            try
            {
                technology = (Technology)new XmlSerializer(typeof(Technology)).Deserialize(file.OpenRead());
                string name = Path.GetFileNameWithoutExtension(file.Name);
                if (technologies.ContainsKey(name))
                {
                    technologies[name] = technology;
                }
                else
                {
                    technologies.Add(name, technology);
                }
            }
            catch
            {
                print("Failed to load technology: " + file.Name);
            }
        }
    }

    void LoadTechnologyTrees(string path)
    {
        FileInfo[] files = GetFilesByType(path, "*.xml");
        foreach (FileInfo file in files)
        {
            TechnologyTree technologyTree = new TechnologyTree();
            try
            {
                technologyTree = (TechnologyTree)new XmlSerializer(typeof(TechnologyTree)).Deserialize(file.OpenRead());
                string name = Path.GetFileNameWithoutExtension(file.Name);
                if (technologyTrees.ContainsKey(name))
                {
                    technologyTrees[name] = technologyTree;
                }
                else
                {
                    technologyTrees.Add(name, technologyTree);
                }

                print("Loaded technology tree: " + file.Name);
            }
            catch
            {
                print("Failed to load technology: " + file.Name);
            }
        }
    }

    void LoadBuildingDefinitions(string path)
    {
        FileInfo[] files = GetFilesByType(path, "*.xml");
        foreach (FileInfo file in files)
        {
            BuildingDefinition buildingDefinition = new BuildingDefinition();
            try
            {
                buildingDefinition = (BuildingDefinition)new XmlSerializer(typeof(BuildingDefinition)).Deserialize(file.OpenRead());
                string name = Path.GetFileNameWithoutExtension(file.Name);
                if (buildings.ContainsKey(name))
                {
                    buildings[name] = buildingDefinition;
                }
                else
                {
                    buildings.Add(name, buildingDefinition);
                }

                print("Loaded building tree: " + file.Name);
            }
            catch
            {
                print("Failed to load building: " + file.Name);
            }
        }
    }

    void LoadPlanetDefinitions(string path)
    {
        FileInfo[] files = GetFilesByType(path, "*.xml");
        foreach (FileInfo file in files)
        {
            PlanetTypeDefinition planetDefinition = new PlanetTypeDefinition();
            try
            {
                planetDefinition = (PlanetTypeDefinition)new XmlSerializer(typeof(PlanetTypeDefinition)).Deserialize(file.OpenRead());
                string name = Path.GetFileNameWithoutExtension(file.Name);
                if (planetDefinitions.ContainsKey(name))
                {
                    planetDefinitions[name] = planetDefinition;
                }
                else
                {
                    planetDefinitions.Add(name, planetDefinition);
                }
            }
            catch
            {
                print("Failed to load planet definition: " + file.Name);
            }
        }
    }

    void LoadProjectiles(string path)
    {
        FileInfo[] files = GetFilesByType(path, "*.unity3d");
        foreach (FileInfo file in files)
        {
            AssetBundle asset;
            try
            {
                asset = AssetBundle.LoadFromFile(path + file.Name);
                string name = Path.GetFileNameWithoutExtension(file.Name);
                GameObject projectile = (GameObject)asset.LoadAsset(asset.GetAllAssetNames()[0]);
                if (projectile.GetComponent<Projectile>() == null)
                    projectile.AddComponent<Projectile>();

                //Store in dictionaries
                if (projectiles.ContainsKey(name))
                {
                    projectiles[name] = projectile;
                }
                else
                {
                    projectiles.Add(name, projectile);
                }

                projectile.SetActive(false);
                //print("Loaded Projectile: " + name);
            }
            catch
            {
                print("Failed to load Projectile: " + file.Name);
            }
        }
    }

    void LoadExplosionsDirectory(string path)
    {
        if(Directory.Exists(path + "Audio"))
        {
            LoadAudioClips(path + "Audio");
        }

        if (Directory.Exists(path + "Effects"))
        {
            LoadExplosionEffects(path + "Effects/");
        }
    }


    void LoadExplosionEffects(string path)
    {
        FileInfo[] files = GetFilesByType(path, "*.unity3d");
        foreach (FileInfo file in files)
        {
            AssetBundle asset;
            try
            {
                asset = AssetBundle.LoadFromFile(path + file.Name);
                string name = Path.GetFileNameWithoutExtension(file.Name);
                GameObject explosion = (GameObject)asset.LoadAsset(asset.GetAllAssetNames()[0]);

                //Store in dictionaries
                if (explosions.ContainsKey(name))
                {
                    explosions[name] = explosion;
                }
                else
                {
                    explosions.Add(name, explosion);
                }

                explosion.SetActive(false);
                //print("Loaded Projectile: " + name);
            }
            catch
            {
                print("Failed to load Explosion: " + file.Name);
            }
        }
    }

    public ShipDesign GetShipDesign(string hull, string designName)
    {
        Dictionary<string, ShipDesign> hullDesigns;
        ShipDesign design;

        if (shipDesigns.TryGetValue(hull, out hullDesigns))
        {
            if (hullDesigns.TryGetValue(designName, out design))
            {
                return design;
            }
        }
        return null;
    }

    public StationDesign GetStationDesign(string hull, string designName)
    {
        Dictionary<string, StationDesign> hullDesigns;
        StationDesign design;

        if (stationDesigns.TryGetValue(hull, out hullDesigns))
        {
            if (hullDesigns.TryGetValue(designName, out design))
            {
                return design;
            }
        }
        return null;
    }

    public Credits GetCredits()
    {
        return credits;
    }

    public GameConstants GetGameConstants()
    {
        return gameConstants;
    }

    public EmpireDefinition GetEmpireDefinition(string key)
    {
        EmpireDefinition empire;

        if (empires.TryGetValue (key, out empire))
        {
            return empire;
        }

        return null;
    }

    

    void LoadWeaponsDirectory(string path)
    {
        if (Directory.Exists(path))
        {
            if (Directory.Exists(path + "/Definitions"))
            {
                LoadWeapons(path + "/Definitions");
            }

            if (Directory.Exists(path + "/Beams"))
            {
                LoadBeams(path + "/Beams/");
            }

            if (Directory.Exists(path + "/Projectiles"))
            {
                LoadProjectiles(path + "/Projectiles/");
            }

            if (Directory.Exists(path + "/Audio"))
            {
                LoadAudioClips(path + "/Audio");
            }
        }
    }

    void LoadModulesDirectory(string path)
    {
        if (Directory.Exists(path))
        {
            //Load module textures
            if (Directory.Exists(path + "/Textures"))
            {
                LoadTextures(path + "/Textures", moduleTextures);
            }
            //Load module definitions
            if (Directory.Exists(path + "/Definitions"))
            {
                LoadModules(path + "/Definitions");
            }
            //Load module sets
            if (Directory.Exists(path + "/Sets"))
            {
                LoadModuleSets(path + "/Sets");
            }
            //Load module localization
            if (Directory.Exists(path + "/Localization"))
            {
                LoadLocalization(path + "/Localization/");
            }
        }
    }

    void LoadModulesDirectory(string path, ModInfo mod)
    {
        if (Directory.Exists(path))
        {
            //Load module textures
            if (Directory.Exists(path + "/Textures"))
            {
                LoadTextures(path + "/Textures", moduleTextures);
            }
            //Load module definitions
            if (Directory.Exists(path + "/Definitions"))
            {
                LoadModules(path + "/Definitions");
            }
            //Load module sets
            if (Directory.Exists(path + "/Sets"))
            {
                LoadModuleSets(path + "/Sets", mod);
            }
            //Load module localization
            if (Directory.Exists(path + "/Localization"))
            {
                LoadLocalization(path + "/Localization/");
            }
        }
    }

    void LoadEmpiresDirectory(string path)
    {
        if(Directory.Exists(path))
        {
            if (Directory.Exists(path + "/Definitions"))
            {
                LoadEmpireDefinitions(path + "/Definitions/");
            }

            if (Directory.Exists(path + "/EmpireAttributes"))
            {
                LoadEmpireAttributes(path + "/EmpireAttributes/");
            }

            if (Directory.Exists(path + "/Localization"))
            {
                LoadLocalization(path + "/Localization/");
            }

            if (Directory.Exists(path + "/FlagBackgrounds"))
            {
                LoadTextures(path + "/FlagBackgrounds/", flagBackgrounds);
            }

            if (Directory.Exists(path + "/FlagEmblems"))
            {
                LoadTextures(path + "/FlagEmblems/", flagEmblems);
            }

            LoadRacialTraitsConfig(path + "/RacialTraitsConfig.xml");
        }
    }

    void LoadShipsDirectory(string path)
    {
        if(Directory.Exists(path + "/GameObjects"))
        {
            LoadShips(path + "/GameObjects/");
        }
        if (Directory.Exists(path + "/HardPoints"))
        {
            LoadHardpoints(path + "/HardPoints");
        }
        if (Directory.Exists(path + "/HullData"))
        {
            LoadShipHullDatas(path + "/HullData");
        }
        if (Directory.Exists(path + "/Icons"))
        {
            LoadTextures(path + "/Icons/", unitIcons);
        }
        if (Directory.Exists(path + "/SlotLayouts"))
        {
            LoadShipLayouts(path + "/SlotLayouts");
        }
        if (Directory.Exists(path + "/Designs"))
        {
            LoadShipDesigns(path + "/Designs");
        }
    }

    void LoadStationsDirectory(string path)
    {
        if (Directory.Exists(path + "/GameObjects"))
        {
            LoadStations(path + "/GameObjects/");
        }
        if (Directory.Exists(path + "/HardPoints"))
        {
            LoadHardpoints(path + "/HardPoints");
        }
        if (Directory.Exists(path + "/HullData"))
        {
            LoadStationHullDatas(path + "/HullData");
        }
        if (Directory.Exists(path + "/Icons"))
        {
            LoadTextures(path + "/Icons/", unitIcons);
        }
        if (Directory.Exists(path + "/SlotLayouts"))
        {
            LoadStationLayouts(path + "/SlotLayouts");
        }
        if (Directory.Exists(path + "/Designs"))
        {
            LoadStationDesigns(path + "/Designs");
        }
    }

    void LoadFightersDirectory(string path)
    {
        if (Directory.Exists(path + "/GameObjects"))
        {
            LoadFighters(path + "/GameObjects/");
        }
        if (Directory.Exists(path + "/Definitions"))
        {
            LoadFighterDefinitions(path + "/Definitions");
        }
        if (Directory.Exists(path + "/Icons"))
        {
            LoadTextures(path + "/Icons/", unitIcons);
        }
    }

    void LoadTechnologyDirectory(string path)
    {
        if (Directory.Exists(path + "/Definitions"))
        {
            LoadTechnologies(path + "/Definitions");
        }
        if (Directory.Exists(path + "/Trees"))
        {
            LoadTechnologyTrees(path + "/Trees");
        }
        if (Directory.Exists(path + "/Icons"))
        {
            LoadTextures(path + "/Icons/", technologyIcons);
        }
        if (Directory.Exists(path + "/Localization"))
        {
            LoadLocalization(path + "/Localization/");
        }
    }

    void LoadBuildingsDirectory(string path)
    {
        if (Directory.Exists(path + "/Definitions"))
        {
            LoadBuildingDefinitions(path + "/Definitions");
        }
        if (Directory.Exists(path + "/Icons"))
        {
            LoadTextures(path + "/Icons/", buildingIcons);
        }
        if (Directory.Exists(path + "/Localization"))
        {
            LoadLocalization(path + "/Localization/");
        }
    }

    void LoadPlanetsDirectory(string path)
    {
        if (Directory.Exists(path + "/Definitions"))
        {
            LoadPlanetDefinitions(path + "/Definitions");
        }
        if (Directory.Exists(path + "/Localization"))
        {
            LoadLocalization(path + "/Localization/");
        }
        if (Directory.Exists(path + "/Tiles"))
        {
            LoadTextures(path + "/Tiles", planetTiles);
        }
        if (Directory.Exists(path + "/GameObjects"))
        {
            LoadPlanets(path + "/GameObjects/");
        }
        if (Directory.Exists(path + "/Icons"))
        {
            LoadTextures(path + "/Icons/", planetIcons);
        }
    }

    void ApplyFiringRangeFactorForAllWeapons()
    {
        foreach(KeyValuePair<string, Weapon> weapon in weapons)
        {
            weapon.Value.ApplyFiringRangeFactor();
            weapon.Value.SetProjectileLife();
        }
    }

    void ApplyFiringRangeFactorForAllModules()
    {
        foreach(KeyValuePair<string, Module> module in modules)
        {
            module.Value.ApplyFiringRangeFactor();
        }
    }

    public bool CheckForDesign(string hull, string designName)
    {
        if(shipDesigns.ContainsKey(hull))
        {
            if(shipDesigns[hull].ContainsKey(designName))
            {
                return true;
            }
        }
        return false;
    }

    public void SaveShipDesign(ShipDesign shipDesign)
    {
        StoreShipDesign(shipDesign);
        if (shipDesign.GetPath() == "")
        {
            string path = Application.streamingAssetsPath + "/Ships/Designs/" + shipDesign.Hull + "/";
            Directory.CreateDirectory(path);
            shipDesign.SetPath(path + shipDesign.Name + ".xml");
        }
        XmlSerializer Writer = new XmlSerializer(typeof(ShipDesign));
        FileStream file = File.Create(shipDesign.GetPath());
        Writer.Serialize(file, shipDesign);
        file.Close();
    }

    public void StoreShipDesign(ShipDesign shipDesign)
    {
        if (!shipDesigns.ContainsKey(shipDesign.Hull))
        {
            shipDesigns.Add(shipDesign.Hull, new Dictionary<string, ShipDesign>());
        }
        if (shipDesigns[shipDesign.Hull].ContainsKey(shipDesign.Name))
        {
            shipDesigns[shipDesign.Hull][shipDesign.Name] = shipDesign;
            //Update design data
            if (shipDesignDatas.ContainsKey(shipDesign.Name))
            {
                shipDesignDatas[shipDesign.Name] = new ShipDesignData(shipDesign);
            }
        }
        else
        {
            shipDesigns[shipDesign.Hull].Add(shipDesign.Name, shipDesign);
        }
    }

    public bool CheckForStationDesign(string hull, string designName)
    {
        if (stationDesigns.ContainsKey(hull))
        {
            if (stationDesigns[hull].ContainsKey(designName))
            {
                return true;
            }
        }
        return false;
    }

    public void SaveStationDesign(StationDesign design)
    {
        StoreStationDesign(design);
        if (design.GetPath() == "")
        {
            string path = Application.streamingAssetsPath + "/Stations/Designs/" + design.Hull + "/";
            Directory.CreateDirectory(path);
            design.SetPath(path + design.Name + ".xml");
        }
        XmlSerializer Writer = new XmlSerializer(typeof(StationDesign));
        FileStream file = File.Create(design.GetPath());
        Writer.Serialize(file, design);
        file.Close();
    }

    public void StoreStationDesign(StationDesign design)
    {
        if (!stationDesigns.ContainsKey(design.Hull))
        {
            stationDesigns.Add(design.Hull, new Dictionary<string, StationDesign>());
        }
        if (stationDesigns[design.Hull].ContainsKey(design.Name))
        {
            stationDesigns[design.Hull][design.Name] = design;
            //Update design data
            if (stationDesignDatas.ContainsKey(design.Name))
            {
                stationDesignDatas[design.Name] = new StationDesignData(design);
            }
        }
        else
        {
            stationDesigns[design.Hull].Add(design.Name, design);
        }
    }

    public void SaveScenario(Scenario scenario)
    {
        if(scenariors.ContainsKey(scenario.Name))
        {
            scenariors[scenario.Name] = scenario;
        }
        else
        {
            scenariors.Add(scenario.Name, scenario);
        }

        Directory.CreateDirectory(Application.streamingAssetsPath + "/Scenarios/");
        if(scenario.GetPath() == "")
        {
            scenario.SetPath(Application.streamingAssetsPath + "/Scenarios/" + scenario.Name + ".xml");
        }
        XmlSerializer Writer = new XmlSerializer(typeof(Scenario));
        FileStream file = File.Create(scenario.GetPath());
        Writer.Serialize(file, scenario);
        file.Close();
    }

    public void StoreEmpireDefinition(EmpireDefinition empire)
    {
        if (!empires.ContainsKey(empire.Name))
        {
            empires.Add(empire.Name, empire);
        }
        else
        {
            empires[empire.Name] = empire;
        }
    }

    public void SaveEmpireDefinition(EmpireDefinition empire)
    {
        StoreEmpireDefinition(empire);

        Directory.CreateDirectory(Application.streamingAssetsPath + "/Empires/Definitions/");
        XmlSerializer Writer = new XmlSerializer(typeof(EmpireDefinition));
        FileStream file = File.Create(Application.streamingAssetsPath + "/Empires/Definitions/" + empire.Name + ".xml");
        Writer.Serialize(file, empire);
        file.Close();
    }

    public Module GetModule(string moduleName)
    {
        Module module;
        if(modules.TryGetValue(moduleName, out module))
        {
            return module;
        }
        return null;
    }

    public bool ModuleExists(string moduleName)
    {
        return modules.ContainsKey(moduleName);
    }

    public Weapon GetWeapon(string WeaponName)
    {
        Weapon weapon;
        if(weapons.TryGetValue(WeaponName, out weapon))
        {
            return weapon;
        }
        return null;
    }

    public string GetModuleName(Module module)
    {
        return modules.First(x => x.Value == module).Key;
    }

    public ModuleSet GetModuleSet(string moduleSetName)
    {
        ModuleSet moduleSet;
        if (moduleSets.TryGetValue(moduleSetName, out moduleSet))
        {
            return moduleSet;
        }
        return null;
    }

    public Dictionary<string, ModuleSet> GetModuleSets()
    {
        return moduleSets;
    }

    public bool WeaponExists(string weaponName)
    {
        return weapons.ContainsKey(weaponName);
    }

    public Technology GetTechnology(string techName)
    {
        Technology tech;

        if (technologies.TryGetValue(techName, out tech))
        {
            return tech;
        }

        return null;
    }

    public TechnologyTree GetTechnologyTree(string techTreeName)
    {
        TechnologyTree techTree;

        if (technologyTrees.TryGetValue(techTreeName, out techTree))
        {
            return techTree;
        }

        return null;
    }

    public GameObject GetShipObject(string shipName)
    {
        if(shipName != null)
        {
            GameObject shipObject;
            if(ships.TryGetValue(shipName, out shipObject))
            {
                return shipObject;
            }
        }
        return null;
    }

    public GameObject GetStationObject(string stationName)
    {
        if (stationName != null)
        {
            GameObject stationObject;
            if (stations.TryGetValue(stationName, out stationObject))
            {
                return stationObject;
            }
        }
        return null;
    }

    public GameObject GetFighterObject(string fighterName)
    {
        if (fighterName != null)
        {
            GameObject fighterObject;
            if (fighters.TryGetValue(fighterName, out fighterObject))
            {
                return fighterObject;
            }
        }
        return null;
    }

    public GameObject GetPlanetObject(string planetName)
    {
        if (planetName != null)
        {
            GameObject planetObject;
            if (planets.TryGetValue(planetName, out planetObject))
            {
                return planetObject;
            }
        }
        return null;
    }

    public ShipHullData GetShipHull(string hullName)
    {
        if(hullName != null)
        {
            ShipHullData hullData;
            if(shipHulls.TryGetValue(hullName, out hullData))
            {
                return hullData;
            }
            else
            {
                print("Ship hull data not found: " + hullName);
            }
        }
        return null;
    }

    public GameObject GetScreenObject(string screenName)
    {
        if(screenName != null)
        {
            GameObject screenObject;
            if(screens.TryGetValue(screenName, out screenObject))
            {
                return Instantiate(screenObject);
            }
            else
            {
                print("Screen not found: " + screenName);
            }
        }
        return null;
    }

    public List<ShipHullData> GetShipHulls()
    {
        List<ShipHullData> shipHulls = new List<ShipHullData>();

        foreach(KeyValuePair<string, ShipHullData> keyVal in this.shipHulls)
        {
            shipHulls.Add(keyVal.Value);
        }

        return shipHulls;
    }

    public List<ShipDesign> GetShipDesigns(ShipHullData hull, bool includeDeletedDesigns)
    {
        Dictionary<string, ShipDesign> designDictionary;

        List<ShipDesign> designs = new List<ShipDesign>();

        if (shipDesigns.TryGetValue(hull.Name, out designDictionary))
        {
            foreach(KeyValuePair<string, ShipDesign> keyVal in designDictionary)
            {
                ShipDesign design = keyVal.Value;
                if (includeDeletedDesigns || !design.Deleted)
                {
                    designs.Add(design);
                }
            }
        }
        return designs;
    }

    public ShipSlotLayout GetShipSlotLayout(string slotLayoutName)
    {
        if (slotLayoutName != null)
        {
            ShipSlotLayout slotLayout;
            if (shipLayouts.TryGetValue(slotLayoutName, out slotLayout))
            {
                return slotLayout;
            }
            else
            {
                print("Ship slot layout not found: " + slotLayoutName);
            }
        }
        return null;
    }

    public StationHullData GetStationHull(string hullName)
    {
        if (hullName != null)
        {
            StationHullData hullData;
            if (stationHulls.TryGetValue(hullName, out hullData))
            {
                return hullData;
            }
            else
            {
                print("Station hull data not found: " + hullName);
            }
        }
        return null;
    }

    public Dictionary<string, StationHullData> GetStationHulls()
    {
        return stationHulls;
    }

    public StationSlotLayout GetStationSlotLayout(string slotLayoutName)
    {
        if (slotLayoutName != null)
        {
            StationSlotLayout slotLayout;
            if (stationLayouts.TryGetValue(slotLayoutName, out slotLayout))
            {
                return slotLayout;
            }
            else
            {
                print("Station slot Layout not found: " + slotLayoutName);
            }
        }
        return null;
    }

    public HardPointsStored GetHardPoints(string hardPointsName)
    {
        if (hardPointsName != null)
        {
            HardPointsStored hardPoints;
            if (hardPointsStored.TryGetValue(hardPointsName, out hardPoints))
            {
                return hardPoints;
            }
            else
            {
                print("Hard points not found: " + hardPointsName);
            }
        }
        return null;
    }

    public FighterDefinition GetFighterDefinition(string FighterName)
    {
        if(FighterName != null)
        {
            FighterDefinition fighterDefinition;
            if(fighterDefinitions.TryGetValue(FighterName, out fighterDefinition))
            {
                return fighterDefinition;
            }
        }
        return null;
    }

    public Dictionary<string, FighterDefinition> GetFighterDefinitions()
    {
        return fighterDefinitions;
    }

    void ConnectModulesToModuleSet()
    {
        List<ModuleSet> Sets = new List<ModuleSet>();
        foreach(KeyValuePair<string, ModuleSet> keyVal in moduleSets)
        {
            Sets.Add(keyVal.Value);
        }
        foreach(ModuleSet set in Sets)
        {
            foreach(Module module in set.GetModules())
            {
                module.SetParentSet(set);
            }
        }
    }

    FileInfo[] GetFilesByType(string path, params string[] extensions)
    {
        DirectoryInfo dirInfo = new DirectoryInfo(path);
        List<FileInfo> allFiles = new List<FileInfo>();
        FileInfo[] files;
        for (int i = 0; i < extensions.Length; i++)
        {
            try
            {
                files = dirInfo.GetFiles(extensions[i], SearchOption.AllDirectories);
                for (int j = 0; j < files.Length; j++)
                {
                    allFiles.Add(files[j]);
                }
            }
            catch
            {
                files = new FileInfo[0];
            }
        }
        return allFiles.ToArray();
    }
}