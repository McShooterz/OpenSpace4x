using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingButtonController : MonoBehaviour
{
    BuildingDefinition buildingDefinition;

    [SerializeField]
    Image buildingImage;

    [SerializeField]
    Text buildingNameText;

    [SerializeField]
    Text buildingDescriptionText;

    [SerializeField]
    Transform buildingStatsTransform;

    [SerializeField]
    GameObject statEntryPrefab;

    public delegate void SelectBuilding(BuildingDefinition building);
    public SelectBuilding callbackFunction;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void SetBuilding(BuildingDefinition building, Sprite iconMoney, Sprite iconMetal, Sprite iconCystal, Sprite iconInfluence, Sprite iconTime)
    {
        buildingDefinition = building;

        buildingImage.sprite = buildingDefinition.GetImage();
        buildingNameText.text = buildingDefinition.GetDisplayName();
        buildingDescriptionText.text = buildingDefinition.GetDescription();

        if (buildingDefinition.costMoney > 0f)
        {
            StatEntry statEntry = Instantiate(statEntryPrefab, buildingStatsTransform).GetComponent<StatEntry>();
            statEntry.SetImage(iconMoney);
            statEntry.SetText(buildingDefinition.costMoney.ToString("0.##"));
        }

        if (buildingDefinition.costMetal > 0f)
        {
            StatEntry statEntry = Instantiate(statEntryPrefab, buildingStatsTransform).GetComponent<StatEntry>();
            statEntry.SetImage(iconMetal);
            statEntry.SetText(buildingDefinition.costMetal.ToString("0.##"));
        }

        if (buildingDefinition.costCrystal > 0f)
        {
            StatEntry statEntry = Instantiate(statEntryPrefab, buildingStatsTransform).GetComponent<StatEntry>();
            statEntry.SetImage(iconCystal);
            statEntry.SetText(buildingDefinition.costCrystal.ToString("0.##"));
        }

        if (buildingDefinition.costInfluence > 0f)
        {
            StatEntry statEntry = Instantiate(statEntryPrefab, buildingStatsTransform).GetComponent<StatEntry>();
            statEntry.SetImage(iconInfluence);
            statEntry.SetText(buildingDefinition.costInfluence.ToString("0.##"));
        }

        if (buildingDefinition.costDays > 0f)
        {
            StatEntry statEntry = Instantiate(statEntryPrefab, buildingStatsTransform).GetComponent<StatEntry>();
            statEntry.SetImage(iconTime);
            statEntry.SetText(buildingDefinition.costDays.ToString("0.##"));
        }
    }

    public void ClickButton()
    {
        callbackFunction(buildingDefinition);
    }

    public void SetCallBackFunction(SelectBuilding function)
    {
        callbackFunction = function;
    }
}
