using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CampaignPlanetPanelExtension : MonoBehaviour
{
    [Header("Major Groups")]

    [SerializeField]
    GameObject planetTileInformationGroup;

    [SerializeField]
    GameObject buildListGroup;

    [Header("Tile Group")]

    [SerializeField]
    Image tileImage;

    [SerializeField]
    Text tileName;

    [SerializeField]
    Image tileBonusIcon;

    [SerializeField]
    Text tileBonusText;

    [SerializeField]
    Button buildButton;

    [SerializeField]
    GameObject buildingInformationGroup;

    [SerializeField]
    Image buildingIcon;

    [SerializeField]
    Text buildingNameText;

    [SerializeField]
    Text buildingDescriptionText;

    [SerializeField]
    Text disableButtonText;

    [SerializeField]
    Button upgradeButton;

    [SerializeField]
    GameObject constructionInformationGroup;

    [SerializeField]
    Image constructionIcon;

    [SerializeField]
    Text constructionNameText;

    [SerializeField]
    Text constructionDescriptionText;

    [SerializeField]
    Slider constructionProgressbar;

    [Header("Building List Group")]

    [SerializeField]
    GameObject buildingButtonPrefab;

    [SerializeField]
    Transform buildingListContentTransform;

    [SerializeField]
    Scrollbar buildingScrollbar;

    [SerializeField]
    List<GameObject> buildingButtonObjects = new List<GameObject>();

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void SwitchToPlanetTilePanel(PlanetTile tile)
    {
        planetTileInformationGroup.SetActive(true);
        buildListGroup.SetActive(false);

        tileImage.sprite = tile.GetImage();
        tileName.text = tile.GetDefinition().GetDisplayName();

        UpdatePlanetTileInformation(tile);
    }

    public void UpdatePlanetTileInformation(PlanetTile tile)
    {
        if (tile.HasBonus())
        {
            tileBonusIcon.gameObject.SetActive(true);
            tileBonusText.gameObject.SetActive(true);

            tileBonusIcon.sprite = tile.GetBonusIcon();
            tileBonusText.text = "+" + (tile.GetTileBonusValue() * 100f).ToString("0.#") + "%";
        }
        else
        {
            tileBonusIcon.gameObject.SetActive(false);
            tileBonusText.gameObject.SetActive(false);
        }

        if (tile.GetCurrentBuilding() != null)
        {
            buildingInformationGroup.SetActive(true);
            buildButton.gameObject.SetActive(false);

            buildingIcon.sprite = tile.GetCurrentBuilding().GetImage();
        }
        else
        {
            buildingInformationGroup.SetActive(false);
            buildButton.gameObject.SetActive(true);
        }

        if (tile.GetNextBuilding() != null)
        {
            constructionInformationGroup.SetActive(true);
            buildButton.gameObject.SetActive(false);

            constructionIcon.sprite = tile.GetNextBuilding().GetImage();
            constructionNameText.text = tile.GetNextBuilding().GetDisplayName();
            constructionDescriptionText.text = tile.GetNextBuilding().GetDescription();

            constructionProgressbar.value = tile.GetBuidlingProgress(1.0f);
        }
        else
        {
            constructionInformationGroup.SetActive(false);
        }
    }

    public void SwitchToBuildingListPanel(List<BuildingDefinition> buildingsList, BuildingButtonController.SelectBuilding selectBuildingFunction)
    {
        planetTileInformationGroup.SetActive(false);
        buildListGroup.SetActive(true);

        ClearBuildingList();

        Sprite iconMoney = ResourceManager.instance.GetIconTexture("Icon_Money");
        Sprite iconMetal = ResourceManager.instance.GetIconTexture("Icon_Metal");
        Sprite iconCystal = ResourceManager.instance.GetIconTexture("Icon_Crystal");
        Sprite iconInfluence = ResourceManager.instance.GetIconTexture("Icon_Influence");
        Sprite iconTime = ResourceManager.instance.GetIconTexture("Icon_Time");

        foreach (BuildingDefinition building in buildingsList)
        {
            GameObject buildingButtonObject = Instantiate(buildingButtonPrefab, buildingListContentTransform);
            buildingButtonObjects.Add(buildingButtonObject);
            BuildingButtonController buildingButton = buildingButtonObject.GetComponent<BuildingButtonController>();
            buildingButton.SetBuilding(building, iconMoney, iconMetal, iconCystal, iconInfluence, iconTime);
            buildingButton.SetCallBackFunction(selectBuildingFunction);
        }

        buildingScrollbar.value = 1f;
    }

    void ClearBuildingList()
    {
        foreach (GameObject buttonObject in buildingButtonObjects)
        {
            Destroy(buttonObject);
        }

        buildingButtonObjects.Clear();
    }











}
