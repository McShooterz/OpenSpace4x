using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TechnologyButtonController : MonoBehaviour
{
    [SerializeField]
    Image background;

    [SerializeField]
    Text techNameText;

    [SerializeField]
    Text techDescriptionText;

    [SerializeField]
    Image techImage;

    [SerializeField]
    Text pointsProgressText;

    [SerializeField]
    Image fieldIcon;

    [SerializeField]
    TechnologyEntry techEntry;

    public delegate void ButtonPress(TechnologyEntry technologyEntry);
    protected ButtonPress buttonCallBack;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void SetTechnologyEntry(TechnologyEntry technologyEntry)
    {
        techEntry = technologyEntry;

        techNameText.text = techEntry.GetTechnology().GetName();
        techDescriptionText.text = techEntry.GetTechnology().GetDescription();

        pointsProgressText.text = techEntry.GetResearchPoints().ToString() + "/" + techEntry.GetTechnology().baseCost.ToString();
    }

    public void SetCallBackFunction(ButtonPress callBackFunction)
    {
        buttonCallBack = callBackFunction;
    }

    public void ClickButton()
    {
        buttonCallBack(techEntry);
    }
}
