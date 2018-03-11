﻿using System.Collections;
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
    GameObject constructionInformationGroup;

    [SerializeField]
    Image constructionIcon;

    [SerializeField]
    Text constructionNameText;

    [SerializeField]
    Text constructionDescriptionText;

    [Header("Building List Group")]

    [SerializeField]
    GameObject buildingButtonPrefab;

    [SerializeField]
    Transform buildingListContentTransform;

    [SerializeField]
    Scrollbar buildingScrollbar;

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

        foreach (BuildingDefinition building in buildingsList)
        {
            BuildingButtonController buildingButton = Instantiate(buildingButtonPrefab, buildingListContentTransform).GetComponent<BuildingButtonController>();
            buildingButton.SetBuilding(building);
            buildingButton.SetCallBackFunction(selectBuildingFunction);
        }

        buildingScrollbar.value = 1f;
    }











}
