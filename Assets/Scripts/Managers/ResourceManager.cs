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

    static Dictionary<string, string> Localization = new Dictionary<string, string>();

    //ShipDesigns is keyed on first the hull name, then design name
    public static Dictionary<string, Dictionary<string, ShipDesign>> shipDesigns = new Dictionary<string, Dictionary<string, ShipDesign>>();
    public static Dictionary<string, Dictionary<string, StationDesign>> stationDesigns = new Dictionary<string, Dictionary<string, StationDesign>>();
    public static Dictionary<string, ShipDesignData> shipDesignDatas = new Dictionary<string, ShipDesignData>();
    public static Dictionary<string, StationDesignData> stationDesignDatas = new Dictionary<string, StationDesignData>();
    static Dictionary<string, Module> Modules = new Dictionary<string, Module>();
    public static Dictionary<string, ModuleSet> ModuleSets = new Dictionary<string, ModuleSet>();
    static Dictionary<string, Texture2D> ModuleTextures = new Dictionary<string, Texture2D>();
    static Dictionary<string, Weapon> Weapons = new Dictionary<string, Weapon>();
    public static Dictionary<string, GameObject> Beams = new Dictionary<string, GameObject>();
    public static Dictionary<string, GameObject> Projectiles = new Dictionary<string, GameObject>();
    public static Dictionary<string, GameObject> Explosions = new Dictionary<string, GameObject>();

    static Dictionary<string, ShipHullData> ShipHulls = new Dictionary<string, ShipHullData>();
    static Dictionary<string, StationHullData> StationHulls = new Dictionary<string, StationHullData>();
    static Dictionary<string, ShipSlotLayout> ShipLayouts = new Dictionary<string, ShipSlotLayout>();
    static Dictionary<string, StationSlotLayout> StationLayouts = new Dictionary<string, StationSlotLayout>();
    public static Dictionary<string, GameObject> Ships = new Dictionary<string, GameObject>();
    public static Dictionary<string, GameObject> Stations = new Dictionary<string, GameObject>();
    public static Dictionary<string, GameObject> Fighters = new Dictionary<string, GameObject>();

    static Dictionary<string, FighterDefinition> FighterDefinitions = new Dictionary<string, FighterDefinition>();

    public static Dictionary<string, HardPointsStored> HardPointsStored = new Dictionary<string, HardPointsStored>();

    public static Dictionary<string, EmpireDefinition> Empires = new Dictionary<string, EmpireDefinition>();

    static Dictionary<string, Texture2D> IconTextures = new Dictionary<string, Texture2D>();
    static Dictionary<string, Texture2D> UITextures = new Dictionary<string, Texture2D>();

    static Dictionary<string, Texture2D> UnitIcons = new Dictionary<string, Texture2D>();

    static List<Texture2D> FlagBackgrounds = new List<Texture2D>();
    static List<Texture2D> FlagEmblems = new List<Texture2D>();

    static Dictionary<string, AudioClip> AudioClips = new Dictionary<string, AudioClip>();

    public static Dictionary<string, Scenario> Scenariors = new Dictionary<string, Scenario>();

    public static Dictionary<string, EmpireAttribute> EmpireAttributes = new Dictionary<string, EmpireAttribute>();

    public static Dictionary<string, Technology> Technologies = new Dictionary<string, Technology>();
    public static Dictionary<string, TechnologyTree> TechnologyTrees = new Dictionary<string, TechnologyTree>();

    public static Dictionary<string, ShipNameSet> ShipNameSets = new Dictionary<string, ShipNameSet>();
    public static Dictionary<string, CharacterNameSet> CharacterNameSets = new Dictionary<string, CharacterNameSet>();

    public static Dictionary<string, DilpomacyDialogueSet> DilpomacyDialogueSets = new Dictionary<string, DilpomacyDialogueSet>();

    public static Dictionary<string, Material> SkyBoxes = new Dictionary<string, Material>();

    public static GameConstants gameConstants;
    public static EmpireRacialTraitsConfig RacialTraitsConfig;
    public static GameObject outLineSystem;
    public static GameObject ShieldsObject;
    ModLoadConfig modLoadConfig;

    List<ModInfo> ModInfos = new List<ModInfo>();

    Credits credits;

    public static Texture2D ErrorTexture;

    void Awake()
    {
        //Make a Singleton
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
        //This stays in every scene
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start()
    {
        LoadAll();

        ChangeSkyBox("PurpleNebula");
    }

    public static GameObject CreateShip(ShipHullData hullData, Vector3 Position, Quaternion Rotation)
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

    public static Ship CreateCombatShip(ShipData data, Vector3 Position, Quaternion Rotation)
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

    public static GameObject CreateStation(StationHullData hullData, Vector3 Position, Quaternion Rotation)
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

    public static Station CreateCombatStation(StationData data, Vector3 Position, Quaternion Rotation)
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

    public static GameObject CreateFighter(FighterDefinition Definition, Vector3 Position, Quaternion Rotation)
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

    public static Fighter CreateCombatFighter(FighterDefinition Definition, Vector3 Position, Quaternion Rotation)
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

    public static Beam CreateBeam(Weapon weapon)
    {
        GameObject beamObject;
        if (Beams.TryGetValue(weapon.WeaponEffect, out beamObject))
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

    public static Projectile CreateProjectile(Weapon weapon)
    {
        GameObject projectile;
        if (Projectiles.TryGetValue(weapon.WeaponEffect, out projectile))
        {
            projectile = Instantiate(projectile);
            projectile.SetActive(true);
            projectile.transform.localScale *= weapon.Scale;
            return projectile.GetComponent<Projectile>();
        }
        return null;
    }

    public static PopupMessage CreatePopupMessage(Vector3 Position, string message, Color color, float lifeTime)
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

    public static TextMeshController CreateWorldMessage(Vector3 Position, string message, Color color, int size)
    {
        GameObject messageObject = new GameObject();
        messageObject.transform.position = Position;
        TextMeshController messageController = messageObject.AddComponent<TextMeshController>();
        messageController.Initialize(color, size, 0.1f, 0.7f, message);
        return messageController;
    }

    public static BoardingBalanceBar CreateBoardingBalanceBar(Vector3 Position)
    {
        GameObject ObjectInstance = Instantiate(Resources.Load("BoardingBalanceBar")) as GameObject;
        ObjectInstance.transform.position = Position;
        BoardingBalanceBar boardingBalanceBar = ObjectInstance.GetComponent<BoardingBalanceBar>();
        boardingBalanceBar.Initialize();
        return boardingBalanceBar;
    }

    public static void CreateClickPing(Vector3 Position)
    {
        GameObject ObjectInstance = Instantiate(Resources.Load("ClickPing"), Position, Quaternion.identity) as GameObject;
        SelfDestructor detonator = ObjectInstance.AddComponent<SelfDestructor>();
        detonator.setLifeTimer(0.6f);
    }

    public static void CreateExplosion(Vector3 Position, string explosionName, string explosionAudio, float Size, float lifeTime, bool AlwaysPlayAudio)
    {
        GameObject sourceExplosionObject;
        if(Explosions.TryGetValue(explosionName, out sourceExplosionObject))
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

    public static OpenSpaceProtected.OutlineSystem CreateOutLineSystem()
    {
        GameObject outLine = Instantiate(outLineSystem) as GameObject;
        outLine.SetActive(true);
        return outLine.GetComponent<OpenSpaceProtected.OutlineSystem>();
    }

    public static GameObject CreateMiniMapObject()
    {
        return Instantiate(Resources.Load("MiniMapObject")) as GameObject;
    }

    public static GameObject CreateFleet(Vector3 Position, Quaternion Rotation)
    {
        return Instantiate(Resources.Load("Fleet"), Position, Rotation) as GameObject;
    }

    public static GameObject CreateStar(string Name, Vector3 Position, Quaternion Rotation)
    {
        return Instantiate(Resources.Load("Stars/" + Name), Position, Rotation) as GameObject;
    }

    public static GameObject CreatePlanet(string Name, Vector3 Position, Quaternion Rotation)
    {
        return Instantiate(Resources.Load("Planets/" + Name), Position, Rotation) as GameObject;
    }

    public static GameObject CreateSector(SectorData data)
    {
        GameObject Sector = Instantiate(Resources.Load("Sector")) as GameObject;
        data.sector = Sector.GetComponent<Sector>();
        data.sector.data = data;
        Sector.transform.position = data.Position;
        return Sector;
    }

    public static ShipDesignData GetShipDesignData(ShipDesign design)
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

    public static StationDesignData GetStationDesignData(StationDesign design)
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

    public static Material GetCloackedShipMaterial()
    {
        return new Material(Resources.Load("CloakedShip", typeof(Material)) as Material);
    }

    public static Material GetShipDissolveMaterial()
    {
        return new Material(Resources.Load("ShipDissolver", typeof(Material)) as Material);
    }

    public static Texture2D GetIconTexture(string textureName)
    {
        return GetTexture(textureName, IconTextures);
    }

    public static Texture2D GetUITexture(string textureName)
    {
        return GetTexture(textureName, UITextures);
    }

    public static Texture2D GetModuleTexture(string textureName)
    {
        return GetTexture(textureName, ModuleTextures);
    }

    public static Texture2D GetUnitIcon(string textureName)
    {
        return GetTexture(textureName, UnitIcons);
    }

    static Texture2D GetTexture(string textureName, Dictionary<string, Texture2D> textureDictionary)
    {
        if(textureName != null)
        {
            Texture2D texture;
            if (textureDictionary.TryGetValue(textureName, out texture))
            {
                return texture;
            }
            else
            {
                return ErrorTexture;
            }
        }
        return ErrorTexture;
    }

    public static Texture2D GetFlagBackground(int index)
    {
        return GetTexture(index, FlagBackgrounds);
    }

    public static Texture2D GetFlagEmblem(int index)
    {
        return GetTexture(index, FlagEmblems);
    }

    static Texture2D GetTexture(int index, List<Texture2D> textures)
    {
        if (textures.Count == 0)
        {
            return ErrorTexture;
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

    public static AudioClip GetAudioClip(string name)
    {
        AudioClip clip;
        if(AudioClips.TryGetValue(name, out clip))
        {
            return clip;
        }
        return null;
    }

    public static void ChangeSkyBox(string SkyboxName)
    {
        if(SkyboxName != null)
        {
            Material skyBox;
            if(SkyBoxes.TryGetValue(SkyboxName, out skyBox))
            {
                RenderSettings.skybox = skyBox;
            }
        }
    }

    void LoadAll()
    {
        System.DateTime LoadStartTime = System.DateTime.Now;

        ErrorTexture = Resources.Load("Textures/ErrorTexture") as Texture2D;

        LoadGameConstants(Application.streamingAssetsPath + "/Misc/GameConstants.xml");

        LoadCredits(Application.streamingAssetsPath + "/Credits/Credits.xml");

        //Load misc objects
        if (Directory.Exists(Application.streamingAssetsPath + "/Objects"))
        {
            LoadGameObjects(Application.streamingAssetsPath + "/Objects/");
        }

        if (Directory.Exists(Application.streamingAssetsPath + "/SkyBoxes"))
        {
            LoadSkyBoxes(Application.streamingAssetsPath + "/SkyBoxes/");
        }

        //Load explosions
        if (Directory.Exists(Application.streamingAssetsPath + "/Explosions"))
        {
            LoadExplosionsDirectory(Application.streamingAssetsPath + "/Explosions/");
        }

        LoadLocalization(Application.streamingAssetsPath + "/Localization/");

        if (Directory.Exists(Application.streamingAssetsPath + "/StationDesigns"))
        {
            LoadStationDesigns(Application.streamingAssetsPath + "/StationDesigns");
        }

        //load icon textures
        if (Directory.Exists(Application.streamingAssetsPath + "/Icons"))
        {
            LoadTextures(Application.streamingAssetsPath + "/Icons", IconTextures);
        }
        if (Directory.Exists(Application.streamingAssetsPath + "/UITextures"))
        {
            LoadTextures(Application.streamingAssetsPath + "/UITextures", UITextures);
        }
        if (Directory.Exists(Application.streamingAssetsPath + "/Audio"))
        {
            LoadAudioClips(Application.streamingAssetsPath + "/Audio");
        }

        //Load modules
        LoadModulesDirectory(Application.streamingAssetsPath + "/Modules");

        LoadWeaponsDirectory(Application.streamingAssetsPath + "/Weapons");

        //Load Scenarios
        if(Directory.Exists(Application.streamingAssetsPath + "/Scenarios"))
        {
            LoadScenarios(Application.streamingAssetsPath + "/Scenarios");
        }

        //Load Empire Attributes
        LoadEmpiresDirectory(Application.streamingAssetsPath + "/Empires");

        //Load in Ship asset bundles
        if (Directory.Exists(Application.streamingAssetsPath + "/Ships"))
        {
            LoadShipsDirectory(Application.streamingAssetsPath + "/Ships");
        }

        if (Directory.Exists(Application.streamingAssetsPath + "/Stations"))
        {
            LoadStationsDirectory(Application.streamingAssetsPath + "/Stations/");
        }

        if (Directory.Exists(Application.streamingAssetsPath + "/Fighters"))
        {
            LoadFightersDirectory(Application.streamingAssetsPath + "/Fighters/");
        }

        LoadModLoadConfig();
        LoadModInfos();

        LoadMods();

        ApplyFiringRangeFactorForAllWeapons();
        ApplyFiringRangeFactorForAllModules();

        ConnectModulesToSet();

        GameManager.instance.CreateOutLineSystems(gameConstants.Highlight_Player.GetColor(), gameConstants.Highlight_Enemy.GetColor(), gameConstants.Highlight_Ally.GetColor(), gameConstants.Highlight_Neutral.GetColor());

        //Get the timer it took to load
        print("Resource Load time: " + (System.DateTime.Now - LoadStartTime).TotalSeconds.ToString());
    }

    void LoadGameObjects(string path)
    {
        if (File.Exists(path + "outlinesystem.unity3d"))
        {
            if (outLineSystem != null)
                Destroy(outLineSystem);
            AssetBundle asset = AssetBundle.LoadFromFile(path + "outlinesystem.unity3d");
            outLineSystem = (GameObject)asset.LoadAsset(asset.GetAllAssetNames()[0]);
            outLineSystem.SetActive(false);
        }
        if (File.Exists(path + "shields.unity3d"))
        {
            if (ShieldsObject != null)
                Destroy(ShieldsObject);
            AssetBundle asset = AssetBundle.LoadFromFile(path + "shields.unity3d");
            ShieldsObject = (GameObject)asset.LoadAsset(asset.GetAllAssetNames()[0]);
            ShieldsObject.SetActive(false);
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
                RacialTraitsConfig = (EmpireRacialTraitsConfig)new XmlSerializer(typeof(EmpireRacialTraitsConfig)).Deserialize(new FileInfo(path).OpenRead());
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

                if (Localization.ContainsKey(KeyWord))
                {
                    Localization[KeyWord] = Value;
                }
                else
                {
                    Localization.Add(KeyWord, Value);
                }
            }
        }
    }

    public static string GetLocalization(string key)
    {
        string value;
        if (Localization.TryGetValue(key, out value))
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
        ModInfos.Clear();
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
                    ModInfos.Add(modInfo);
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
                foreach (ModInfo modInfo in ModInfos)
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
        GameObject shipMesh = shipObject.transform.GetChild(0).gameObject;
        //Set tags and layers
        shipObject.tag = "Ship";

        GameObject ExistingShip;
        if (Ships.TryGetValue(shipObject.name, out ExistingShip))
        {
            //Destroy(ExistingShip);
            ExistingShip = shipObject;
        }
        else
        {
            Ships.Add(shipObject.name, shipObject);
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
        GameObject stationMesh = stationObject.transform.GetChild(0).gameObject;
        //Set tags and layers
        stationObject.tag = "Station";

        GameObject ExistingStation;
        if (Stations.TryGetValue(stationObject.name, out ExistingStation))
        {
            //Destroy(ExistingStation);
            ExistingStation = stationObject;
        }
        else
        {
            Stations.Add(stationObject.name, stationObject);
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
        GameObject fighterMesh = fighterObject.transform.GetChild(0).gameObject;

        //Add Tags
        fighterObject.tag = "Fighter";

        //Fighter fighter = fighterObject.GetComponent<Fighter>();

        GameObject ExistingFighter;
        if (Fighters.TryGetValue(fighterObject.name, out ExistingFighter))
        {
            //Destroy(ExistingFighter);
            ExistingFighter = fighterObject;
        }
        else
        {
            Fighters.Add(fighterObject.name, fighterObject);
        }
        fighterObject.SetActive(false);
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
                    if(SkyBoxes.TryGetValue(SkyboxMaterial.name, out ExistingSkyBox))
                    {
                        ExistingSkyBox = SkyboxMaterial;
                    }
                    else
                    {
                        SkyBoxes.Add(SkyboxMaterial.name, SkyboxMaterial);
                    }
                }
                catch
                {
                    print("Failed to load skybox: " + assetName);
                }
            }
        }
    }

    public static void AttachShields(GameObject UnitNeedingShields)
    {
        GameObject ParentMesh = UnitNeedingShields.transform.GetChild(0).gameObject;
        MeshFilter ParentMeshFilter = ParentMesh.GetComponent<MeshFilter>();

        GameObject newShields = Instantiate(ShieldsObject) as GameObject;
        newShields.SetActive(true);
        newShields.tag = "HitBox";

        newShields.transform.position = ParentMesh.transform.position;
        newShields.transform.rotation = ParentMesh.transform.rotation;
        newShields.transform.parent = ParentMesh.transform;

        newShields.GetComponent<MeshFilter>().mesh = ParentMeshFilter.mesh;
        newShields.GetComponent<MeshCollider>().sharedMesh = ParentMeshFilter.mesh;
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
                    if (AudioClips.ContainsKey(clip.name))
                    {
                        AudioClips[clip.name] = clip;
                    }
                    else
                    {
                        AudioClips.Add(clip.name, clip);
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
        AudioClip clip = www.audioClip;
        if (AudioClips.ContainsKey(name))
        {
            AudioClips[name] = clip;
        }
        else
        {
            AudioClips.Add(name, clip);
        }
    }

    void LoadTextures(string path, Dictionary<string, Texture2D> dictionary)
    {
        FileInfo[] files = GetFilesByType(path, "*.png");
        foreach (FileInfo file in files)
        {
            Texture2D tex = new Texture2D(1, 1);
            try
            {
                byte[] data = File.ReadAllBytes(file.FullName);
                tex.LoadImage(data);
                string name = Path.GetFileNameWithoutExtension(file.Name);
                if (dictionary.ContainsKey(name))
                {
                    dictionary[name] = tex;
                }
                else
                {
                    dictionary.Add(name, tex);
                }
                //print("Loaded Texture: " + name);
            }
            catch
            {
                print("Failed to load texture: " + file.Name);
            }
        }
    }

    void LoadTextures(string path, List<Texture2D> list)
    {
        FileInfo[] files = GetFilesByType(path, "*.png");
        foreach (FileInfo file in files)
        {
            Texture2D texture = new Texture2D(1, 1);
            try
            {
                byte[] data = File.ReadAllBytes(file.FullName);
                texture.LoadImage(data);
                list.Add(texture);
            }
            catch
            {
                print("Failed to load texture: " + file.Name);
            }
        }
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
                if (Modules.ContainsKey(name))
                {
                    Modules[name] = module;
                }
                else
                {
                    Modules.Add(name, module);
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

                if (ModuleSets.ContainsKey(name))
                {
                    ModuleSets[name] = moduleSet;
                }
                else
                {
                    ModuleSets.Add(name, moduleSet);
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

                if (ModuleSets.ContainsKey(name))
                {
                    ModuleSets[name] = moduleSet;
                }
                else
                {
                    ModuleSets.Add(name, moduleSet);
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
                weapon.SetName(name);
                if (Weapons.ContainsKey(name))
                {
                    Weapons[name] = weapon;
                }
                else
                {
                    Weapons.Add(name, weapon);
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
                if (Beams.ContainsKey(name))
                {
                    Beams[name] = beam;
                }
                else
                {
                    Beams.Add(name, beam);
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
                if (Scenariors.ContainsKey(name))
                {
                    Scenariors[name] = scenario;
                }
                else
                {
                    Scenariors.Add(name, scenario);
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
                if (Empires.ContainsKey(name))
                {
                    Empires[name] = empireDefinition;
                }
                else
                {
                    Empires.Add(name, empireDefinition);
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
                if (EmpireAttributes.ContainsKey(name))
                {
                    EmpireAttributes[name] = empireAttribute;
                }
                else
                {
                    EmpireAttributes.Add(name, empireAttribute);
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
                if (ShipHulls.ContainsKey(name))
                {
                    ShipHulls[name] = shipHullData;
                }
                else
                {
                    ShipHulls.Add(name, shipHullData);
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
                if (StationHulls.ContainsKey(name))
                {
                    StationHulls[name] = stationHullData;
                }
                else
                {
                    StationHulls.Add(name, stationHullData);
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
                if (ShipLayouts.ContainsKey(name))
                {
                    ShipLayouts[name] = slotLayout;
                }
                else
                {
                    ShipLayouts.Add(name, slotLayout);
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
                if (ShipLayouts.ContainsKey(name))
                {
                    StationLayouts[name] = slotLayout;
                }
                else
                {
                    StationLayouts.Add(name, slotLayout);
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
                if (HardPointsStored.ContainsKey(name))
                {
                    HardPointsStored[name] = hardPoints;
                }
                else
                {
                    HardPointsStored.Add(name, hardPoints);
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
                if (FighterDefinitions.ContainsKey(name))
                {
                    FighterDefinitions[name] = fighterDefinition;
                }
                else
                {
                    FighterDefinitions.Add(name, fighterDefinition);
                }
            }
            catch
            {
                print("Failed to load fighter definition: " + file.Name);
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
                if (Projectiles.ContainsKey(name))
                {
                    Projectiles[name] = projectile;
                }
                else
                {
                    Projectiles.Add(name, projectile);
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
                if (Explosions.ContainsKey(name))
                {
                    Explosions[name] = explosion;
                }
                else
                {
                    Explosions.Add(name, explosion);
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

    public static ShipDesign GetShipDesign(string hull, string designName)
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

    public static StationDesign GetStationDesign(string hull, string designName)
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
                LoadTextures(path + "/Textures", ModuleTextures);
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
                LoadTextures(path + "/Textures", ModuleTextures);
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
                LoadTextures(path + "/FlagBackgrounds/", FlagBackgrounds);
            }

            if (Directory.Exists(path + "/FlagEmblems"))
            {
                LoadTextures(path + "/FlagEmblems/", FlagEmblems);
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
            LoadTextures(path + "/Icons/", UnitIcons);
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
            LoadTextures(path + "/Icons/", UnitIcons);
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
            LoadTextures(path + "/Icons/", UnitIcons);
        }
    }

    void ApplyFiringRangeFactorForAllWeapons()
    {
        foreach(KeyValuePair<string, Weapon> weapon in Weapons)
        {
            weapon.Value.ApplyFiringRangeFactor();
            weapon.Value.SetProjectileLife();
        }
    }

    void ApplyFiringRangeFactorForAllModules()
    {
        foreach(KeyValuePair<string, Module> module in Modules)
        {
            module.Value.ApplyFiringRangeFactor();
        }
    }

    public static bool CheckForDesign(string hull, string designName)
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

    public static void SaveShipDesign(ShipDesign shipDesign)
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

    public static void StoreShipDesign(ShipDesign shipDesign)
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

    public static bool CheckForStationDesign(string hull, string designName)
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

    public static void SaveStationDesign(StationDesign design)
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

    public static void StoreStationDesign(StationDesign design)
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

    public static void SaveScenario(Scenario scenario)
    {
        if(Scenariors.ContainsKey(scenario.Name))
        {
            Scenariors[scenario.Name] = scenario;
        }
        else
        {
            Scenariors.Add(scenario.Name, scenario);
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

    public static void StoreEmpireDefinition(EmpireDefinition empire)
    {
        if (!Empires.ContainsKey(empire.Name))
        {
            Empires.Add(empire.Name, empire);
        }
        else
        {
            Empires[empire.Name] = empire;
        }
    }

    public static void SaveEmpireDefinition(EmpireDefinition empire)
    {
        StoreEmpireDefinition(empire);

        Directory.CreateDirectory(Application.streamingAssetsPath + "/Empires/Definitions/");
        XmlSerializer Writer = new XmlSerializer(typeof(EmpireDefinition));
        FileStream file = File.Create(Application.streamingAssetsPath + "/Empires/Definitions/" + empire.Name + ".xml");
        Writer.Serialize(file, empire);
        file.Close();
    }

    public static Module GetModule(string moduleName)
    {
        Module module;
        if(Modules.TryGetValue(moduleName, out module))
        {
            return module;
        }
        return null;
    }

    public static bool ModuleExists(string moduleName)
    {
        return Modules.ContainsKey(moduleName);
    }

    public static Weapon GetWeapon(string WeaponName)
    {
        Weapon weapon;
        if(Weapons.TryGetValue(WeaponName, out weapon))
        {
            return weapon;
        }
        return null;
    }

    public static string GetModuleName(Module module)
    {
        return Modules.First(x => x.Value == module).Key;
    }

    public static ModuleSet GetModuleSet(string moduleSetName)
    {
        ModuleSet moduleSet;
        if (ModuleSets.TryGetValue(moduleSetName, out moduleSet))
        {
            return moduleSet;
        }
        return null;
    }

    public static bool WeaponExists(string weaponName)
    {
        return Weapons.ContainsKey(weaponName);
    }

    public static GameObject GetShipObject(string shipName)
    {
        if(shipName != null)
        {
            GameObject shipObject;
            if(Ships.TryGetValue(shipName, out shipObject))
            {
                return shipObject;
            }
        }
        return null;
    }

    public static GameObject GetStationObject(string stationName)
    {
        if (stationName != null)
        {
            GameObject shipObject;
            if (Stations.TryGetValue(stationName, out shipObject))
            {
                return shipObject;
            }
        }
        return null;
    }

    public static GameObject GetFighterObject(string fighterName)
    {
        if (fighterName != null)
        {
            GameObject shipObject;
            if (Fighters.TryGetValue(fighterName, out shipObject))
            {
                return shipObject;
            }
        }
        return null;
    }

    public static ShipHullData GetShipHull(string hullName)
    {
        if(hullName != null)
        {
            ShipHullData hullData;
            if(ShipHulls.TryGetValue(hullName, out hullData))
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

    public static Dictionary<string, ShipHullData> GetShipHulls()
    {
        return ShipHulls;
    }

    public static ShipSlotLayout GetShipSlotLayout(string slotLayoutName)
    {
        if (slotLayoutName != null)
        {
            ShipSlotLayout slotLayout;
            if (ShipLayouts.TryGetValue(slotLayoutName, out slotLayout))
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

    public static StationHullData GetStationHull(string hullName)
    {
        if (hullName != null)
        {
            StationHullData hullData;
            if (StationHulls.TryGetValue(hullName, out hullData))
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

    public static Dictionary<string, StationHullData> GetStationHulls()
    {
        return StationHulls;
    }

    public static StationSlotLayout GetStationSlotLayout(string slotLayoutName)
    {
        if (slotLayoutName != null)
        {
            StationSlotLayout slotLayout;
            if (StationLayouts.TryGetValue(slotLayoutName, out slotLayout))
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

    public static HardPointsStored GetHardPoints(string hardPointsName)
    {
        if (hardPointsName != null)
        {
            HardPointsStored hardPoints;
            if (HardPointsStored.TryGetValue(hardPointsName, out hardPoints))
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

    public static FighterDefinition GetFighterDefinition(string FighterName)
    {
        if(FighterName != null)
        {
            FighterDefinition fighterDefinition;
            if(FighterDefinitions.TryGetValue(FighterName, out fighterDefinition))
            {
                return fighterDefinition;
            }
        }
        return null;
    }

    public static Dictionary<string, FighterDefinition> GetFighterDefinitions()
    {
        return FighterDefinitions;
    }

    void ConnectModulesToSet()
    {
        List<ModuleSet> Sets = new List<ModuleSet>();
        foreach(KeyValuePair<string, ModuleSet> keyVal in ModuleSets)
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
