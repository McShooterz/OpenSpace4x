using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CampaignPlanetPanel : MonoBehaviour
{
    [SerializeField]
    PlanetController planetController;

    [SerializeField]
    bool playerControl;

    [SerializeField]
    Text planetName;

    [SerializeField]
    CampaignPlanetPanelExtension panelExtension;

    [Header("Panel Buttons")]

    [SerializeField]
    Button surfaceButton;

    [SerializeField]
    Button militaryButton;

    [SerializeField]
    Button shipyardButton;

    [SerializeField]
    Button starbaseButton;

    [Header("Sub Panels")]

    [SerializeField]
    GameObject overviewSubPanel;

    [SerializeField]
    GameObject surfaceSubPanel;

    [SerializeField]
    GameObject militarySubPanel;

    [SerializeField]
    GameObject shipyardSubPanel;

    [SerializeField]
    GameObject starbaseSubPanel;

    [Header("Planet Information")]

    [SerializeField]
    Text planetTypeText;

    [SerializeField]
    Text planetSizeText;

    [SerializeField]
    Text planetPopulationText;

    [SerializeField]
    Text planetMoralText;

    [SerializeField]
    Text planetMoneyText;

    [SerializeField]
    Text planetFoodText;

    [SerializeField]
    Text planetMetalText;

    [SerializeField]
    Text planetCrystalText;

    [SerializeField]
    Text planetPhysicsText;

    [SerializeField]
    Text planetSocietyText;

    [SerializeField]
    Text planetEngineeringText;

    [SerializeField]
    Text planetUnityText;

    [Header("OverviewSubPanel")]


    [Header("SurfaceSubPanel")]

    [SerializeField]
    GameObject planetTilePrefab;

    [SerializeField]
    GridLayoutGroup gridLayoutGroup;

    [SerializeField]
    PlanetTileController[] planetTileControllers;

    [SerializeField]
    PlanetTileController selectedPlanetTileController;

    [Header("MilitarySubPanel")]

    [Header("ShipyardSubPanel")]

    [Header("StarbaseSubPanel")]

    [SerializeField]
    int starbaseSize;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void OnEnable()
    {
        ClickOverviewButton();
    }

    public void SetPlanet(PlanetController newPlanet, bool controllable)
    {
        planetController = newPlanet;
        playerControl = controllable;

        planetName.text = planetController.GetDisplayName();

        planetTypeText.text = planetController.GetTypeDefinition().GetName();

        planetSizeText.text = "Size: " + planetController.GetSize().ToString();

        if (planetController.GetTypeDefinition().colonizable)
        {
            if (playerControl)
            {
                militaryButton.interactable = true;
                shipyardButton.interactable = true;
                starbaseButton.interactable = true;
            }
            surfaceButton.interactable = true;          

            CreatePlanetTiles(planetController.GetPlanetTiles());
        }
        else
        {
            surfaceButton.interactable = false;
            militaryButton.interactable = false;
            shipyardButton.interactable = false;
            starbaseButton.interactable = false;
        }
    }

    public void CreatePlanetTiles(PlanetTile[] planetTileDataList)
    {
        ClearTiles();

        planetTileControllers = new PlanetTileController[planetTileDataList.Length];

        for (int i = 0; i < planetTileDataList.Length; i++)
        {
            planetTileControllers[i] = Instantiate(planetTilePrefab, gridLayoutGroup.transform).GetComponent<PlanetTileController>();
            planetTileControllers[i].SetPlanetTile(planetTileDataList[i]);
            planetTileControllers[i].SetCallBackFunction(SelectPlanetTile);
        }

        // Adjust the number of rows in the grid
        gridLayoutGroup.constraintCount = Mathf.CeilToInt(Mathf.Sqrt(planetTileDataList.Length));
    }

    public void ClearTiles()
    {
        for (int i = 0; i < planetTileControllers.Length; i++)
        {
            Destroy(planetTileControllers[i].gameObject);
        }

        planetTileControllers = null;
    }

    public void SelectPlanetTile(PlanetTileController planetTileController)
    {
        selectedPlanetTileController = planetTileController;

        panelExtension.gameObject.SetActive(true);

        panelExtension.SwitchToPlanetTilePanel(selectedPlanetTileController.GetPlanetTile());
    }

    public void SelectBuilding(BuildingDefinition build)
    {
        PlanetTile planetTile = selectedPlanetTileController.GetPlanetTile();

        planetTile.SetNextBuilding(build);

        selectedPlanetTileController.UpdateBuildingUI();

        panelExtension.SwitchToPlanetTilePanel(planetTile);
    }

    public void UpdateDay()
    {
        if (surfaceSubPanel.activeInHierarchy)
        {
            foreach(PlanetTileController tile in planetTileControllers)
            {
                tile.UpdateBuildingUI();
            }
        }

        if (panelExtension.gameObject.activeInHierarchy)
        {
            panelExtension.UpdatePlanetTileInformation(selectedPlanetTileController.GetPlanetTile());
        }
    }

    public void ClickOverviewButton()
    {
        overviewSubPanel.SetActive(true);
        surfaceSubPanel.SetActive(false);
        militarySubPanel.SetActive(false);
        shipyardSubPanel.SetActive(false);
        starbaseSubPanel.SetActive(false);

        panelExtension.gameObject.SetActive(false);
    }

    public void ClickSurfaceButton()
    {
        overviewSubPanel.SetActive(false);
        surfaceSubPanel.SetActive(true);
        militarySubPanel.SetActive(false);
        shipyardSubPanel.SetActive(false);
        starbaseSubPanel.SetActive(false);

        panelExtension.gameObject.SetActive(false);
    }

    public void ClickMilitaryButton()
    {
        overviewSubPanel.SetActive(false);
        surfaceSubPanel.SetActive(false);
        militarySubPanel.SetActive(true);
        shipyardSubPanel.SetActive(false);
        starbaseSubPanel.SetActive(false);

        panelExtension.gameObject.SetActive(false);
    }

    public void ClickShipyardButton()
    {
        overviewSubPanel.SetActive(false);
        surfaceSubPanel.SetActive(false);
        militarySubPanel.SetActive(false);
        shipyardSubPanel.SetActive(true);
        starbaseSubPanel.SetActive(false);

        panelExtension.gameObject.SetActive(false);
    }

    public void ClickStarbaseButton()
    {
        overviewSubPanel.SetActive(false);
        surfaceSubPanel.SetActive(false);
        militarySubPanel.SetActive(false);
        shipyardSubPanel.SetActive(false);
        starbaseSubPanel.SetActive(true);

        panelExtension.gameObject.SetActive(false);
    }






    // Extension button clicks

    public void ClickBuildButton()
    {
        panelExtension.SwitchToBuildingListPanel(EmpireManager.instance.GetPlayerEmpire().GetUnlockedBaseBuildings(), SelectBuilding);
    }

    public void ClickRemoveButton()
    {

    }

    public void ClickDisableButton()
    {

    }

    public void ClickReplaceButton()
    {

    }

    public void ClickUpgradeButton()
    {
        List<BuildingDefinition> upgradeBuildings = selectedPlanetTileController.GetPlanetTile().GetCurrentBuilding().GetUpgradeBuildings();
        List<BuildingDefinition> validUpgradeBuildings = new List<BuildingDefinition>();
        Empire playerEmpire = EmpireManager.instance.GetPlayerEmpire();

        foreach (BuildingDefinition building in upgradeBuildings)
        {
            if (playerEmpire.HasUnlockedBuilding(building))
            {
                validUpgradeBuildings.Add(building);
            }
        }

        panelExtension.SwitchToBuildingListPanel(validUpgradeBuildings, SelectBuilding);
    }

    public void ClickCancelButton()
    {
        selectedPlanetTileController.GetPlanetTile().CancelConstruction();
        panelExtension.UpdatePlanetTileInformation(selectedPlanetTileController.GetPlanetTile());
    }
}

