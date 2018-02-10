

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchWindow : MonoBehaviour
{
    [SerializeField]
    CampaignMainScreen mainScreen;

    [SerializeField]
    GameObject technologyListPanel;

    [Header("Scientist Physics")]

    [SerializeField]
    Text scientistNamePhysics;

    [SerializeField]
    Text researchBonusPhysicsText;

    [SerializeField]
    Button selectResearchPhysicsButton;

    [SerializeField]
    GameObject activeResearchPhysicsGroup;

    [SerializeField]
    Image activeTechnologyPhysicsBackground;

    [SerializeField]
    Text activeTechnologyPhysicsNameText;

    [SerializeField]
    Text activeTechnologyPhysicsProgressPointsText;

    [SerializeField]
    Image activeTechnologyPhysicsImage;

    [SerializeField]
    Text activeTechnologyPhysicsDescriptionText;

    [SerializeField]
    Image activeTechnologyPhysicsFieldIcon;

    [SerializeField]
    AdjustableBarController activeTechnologyPhysicsProgressBar;

    [Header("Scientist Society")]

    [SerializeField]
    Text scientistNameSociety;

    [SerializeField]
    Text researchBonusSocietyText;

    [SerializeField]
    Button selectResearchSocietyButton;

    [SerializeField]
    GameObject activeResearchSocietyGroup;

    [SerializeField]
    Image activeTechnologySocietyBackground;

    [SerializeField]
    Text activeTechnologySocietyNameText;

    [SerializeField]
    Text activeTechnologySocietyProgressPointsText;

    [SerializeField]
    Image activeTechnologySocietyImage;

    [SerializeField]
    Text activeTechnologySocietyDescriptionText;

    [SerializeField]
    Image activeTechnologySocietyFieldIcon;

    [SerializeField]
    AdjustableBarController activeTechnologySocietyProgressBar;

    [Header("Scientist Engineering")]

    [SerializeField]
    Text scientistNameEngineering;

    [SerializeField]
    Text researchBonusEngineeringText;

    [SerializeField]
    Button selectResearchEngineeringButton;

    [SerializeField]
    GameObject activeResearchEngineeringGroup;

    [SerializeField]
    Image activeTechnologyEngineeringBackground;

    [SerializeField]
    Text activeTechnologyEngineeringNameText;

    [SerializeField]
    Text activeTechnologyEngineeringProgressPointsText;

    [SerializeField]
    Image activeTechnologyEngineeringImage;

    [SerializeField]
    Text activeTechnologyEngineeringDescriptionText;

    [SerializeField]
    Image activeTechnologyEngineeringFieldIcon;

    [SerializeField]
    AdjustableBarController activeTechnologyEngineeringProgressBar;

    [Header("Technology list")]

    [SerializeField]
    GameObject technologyButtonPrefab;

    [SerializeField]
    Transform contentTransform;

    [SerializeField]
    List<TechnologyButtonController> technologyButtons = new List<TechnologyButtonController>();

    [SerializeField]
    Text technologyListInstructions;

    // Use this for initialization
    void Start ()
    {
        ToggleTechnologyListPanel(false);

    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}


    void OnEnable()
    {
        UpdateCurrentResearch();
    }

    public void UpdateCurrentResearch()
    {
        Empire playerEmpire = EmpireManager.instance.GetPlayerEmpire();

        if (playerEmpire != null)
        {
            TechnologyEntry activeResearchPhysics = playerEmpire.GetActiveResearchPhysics();
            TechnologyEntry activeResearchSociety = playerEmpire.GetActiveResearchSociety();
            TechnologyEntry activeResearchEngineering = playerEmpire.GetActiveResearchEngineering();

            if (activeResearchPhysics != null)
            {
                SetActiveResearchPhysics(activeResearchPhysics);
            }
            else
            {
                activeResearchPhysicsGroup.SetActive(false);
                selectResearchPhysicsButton.gameObject.SetActive(true);
            }

            if (activeResearchSociety != null)
            {
                SetActiveResearchSociety(activeResearchSociety);
            }
            else
            {
                activeResearchSocietyGroup.SetActive(false);
                selectResearchSocietyButton.gameObject.SetActive(true);
            }

            if (activeResearchEngineering != null)
            {
                SetActiveResearchEngineering(activeResearchEngineering);
            }
            else
            {
                activeResearchEngineeringGroup.SetActive(false);
                selectResearchEngineeringButton.gameObject.SetActive(true);
            }
        }
    }

    public void ToggleTechnologyListPanel(bool state)
    {
        technologyListPanel.SetActive(state);
    }

    void ClearTechButtonList()
    {
        foreach(TechnologyButtonController button in technologyButtons)
        {
            if (button != null)
            {
                Destroy(button.gameObject);
            }
        }

        technologyButtons.Clear();
    }

    void BuildTechButtonList(List<TechnologyEntry> techEntries, TechnologyButtonController.ButtonPress callBackFunction)
    {
        ClearTechButtonList();

        foreach(TechnologyEntry techEntry in techEntries)
        {
            GameObject techButton = Instantiate(technologyButtonPrefab, contentTransform);
            TechnologyButtonController techButtonController = techButton.GetComponent<TechnologyButtonController>();
            Button button = techButton.GetComponent<Button>();

            technologyButtons.Add(techButtonController);
            techButtonController.SetTechnologyEntry(techEntry);
            techButtonController.SetCallBackFunction(callBackFunction);
        }
    }

    public void ClickReseachedButton()
    {

    }

    public void ClickCloseButton()
    {
        mainScreen.ToggleResearchWindow(false);
    }

    public void ClickPanelCloseButton()
    {
        ToggleTechnologyListPanel(false);
    }

    public void ClickPhysicsSelectResearchButton()
    {
        ToggleTechnologyListPanel(true);

        BuildTechButtonList(EmpireManager.instance.GetPlayerEmpire().GetCurrentTechnologiesPhysics(), ClickedTechnologyEntryPhysics);
    }

    public void ClickSocietySelectResearchButton()
    {
        ToggleTechnologyListPanel(true);

        BuildTechButtonList(EmpireManager.instance.GetPlayerEmpire().GetCurrentTechnologiesSociety(), ClickedTechnologyEntrySociety);
    }

    public void ClickEngineeringSelectResearchButton()
    {
        ToggleTechnologyListPanel(true);

        BuildTechButtonList(EmpireManager.instance.GetPlayerEmpire().GetCurrentTechnologiesEngineering(), ClickedTechnologyEntryEngineering);
    }

    public void ClickedTechnologyEntryPhysics(TechnologyEntry techEntry)
    {
        ToggleTechnologyListPanel(false);

        Empire empire = EmpireManager.instance.GetPlayerEmpire();
        empire.SetActiveResearchPhysics(techEntry);

        UpdateCurrentResearch();
    }

    public void ClickedTechnologyEntrySociety(TechnologyEntry techEntry)
    {
        ToggleTechnologyListPanel(false);

        Empire empire = EmpireManager.instance.GetPlayerEmpire();
        empire.SetActiveResearchSociety(techEntry);

        UpdateCurrentResearch();
    }

    public void ClickedTechnologyEntryEngineering(TechnologyEntry techEntry)
    {
        ToggleTechnologyListPanel(false);

        Empire empire = EmpireManager.instance.GetPlayerEmpire();
        empire.SetActiveResearchEngineering(techEntry);

        UpdateCurrentResearch();
    }

    public void ClickedChangeTechnologyPhysics()
    {
        activeResearchPhysicsGroup.SetActive(false);

        ToggleTechnologyListPanel(true);

        Empire playerEmpire = EmpireManager.instance.GetPlayerEmpire();

        BuildTechButtonList(playerEmpire.GetCurrentTechnologiesPhysics(), ClickedTechnologyEntryPhysics);

        playerEmpire.SetActiveResearchPhysics(null);
    }

    public void ClickedChangeTechnologySociety()
    {
        activeResearchSocietyGroup.SetActive(false);

        ToggleTechnologyListPanel(true);

        Empire playerEmpire = EmpireManager.instance.GetPlayerEmpire();

        BuildTechButtonList(playerEmpire.GetCurrentTechnologiesSociety(), ClickedTechnologyEntrySociety);

        playerEmpire.SetActiveResearchSociety(null);
    }

    public void ClickedChangeTechnologyEngineering()
    {
        activeResearchEngineeringGroup.SetActive(false);

        ToggleTechnologyListPanel(true);

        Empire playerEmpire = EmpireManager.instance.GetPlayerEmpire();

        BuildTechButtonList(playerEmpire.GetCurrentTechnologiesEngineering(), ClickedTechnologyEntryEngineering);

        playerEmpire.SetActiveResearchEngineering(null);
    }

    void SetActiveResearchPhysics(TechnologyEntry techEntry)
    {
        if (techEntry != null)
        {
            activeResearchPhysicsGroup.SetActive(true);
            selectResearchPhysicsButton.gameObject.SetActive(false);

            //scientistNamePhysics;
            //researchBonusPhysicsText;
            //activeTechnologyPhysicsBackground;

            activeTechnologyPhysicsNameText.text = techEntry.GetTechnology().GetName();
            activeTechnologyPhysicsDescriptionText.text = techEntry.GetTechnology().GetDescription();
            activeTechnologyPhysicsProgressPointsText.text = techEntry.GetResearchPoints().ToString() + "/" + techEntry.GetResearchCost(1.0f).ToString();

            //activeTechnologyPhysicsImage;
            //activeTechnologyPhysicsFieldIcon;

            activeTechnologyPhysicsProgressBar.SetPercentage(techEntry.GetResearchPercentCompleted(1.0f));
        }
    }

    void SetActiveResearchSociety(TechnologyEntry techEntry)
    {
        if (techEntry != null)
        {
            activeResearchSocietyGroup.SetActive(true);
            selectResearchSocietyButton.gameObject.SetActive(false);

            activeTechnologySocietyNameText.text = techEntry.GetTechnology().GetName();
            activeTechnologySocietyDescriptionText.text = techEntry.GetTechnology().GetDescription();
            activeTechnologySocietyProgressPointsText.text = techEntry.GetResearchPoints().ToString() + "/" + techEntry.GetResearchCost(1.0f).ToString();

            activeTechnologySocietyProgressBar.SetPercentage(techEntry.GetResearchPercentCompleted(1.0f));
        }
    }

    void SetActiveResearchEngineering(TechnologyEntry techEntry)
    {
        if (techEntry != null)
        {
            activeResearchEngineeringGroup.SetActive(true);
            selectResearchEngineeringButton.gameObject.SetActive(false);


            activeTechnologyEngineeringNameText.text = techEntry.GetTechnology().GetName();
            activeTechnologyEngineeringDescriptionText.text = techEntry.GetTechnology().GetDescription();
            activeTechnologyEngineeringProgressPointsText.text = techEntry.GetResearchPoints().ToString() + "/" + techEntry.GetResearchCost(1.0f).ToString();

            activeTechnologyEngineeringProgressBar.SetPercentage(techEntry.GetResearchPercentCompleted(1.0f));
        }
    }
}
