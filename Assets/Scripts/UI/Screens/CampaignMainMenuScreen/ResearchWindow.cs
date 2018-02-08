

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
        Empire playerEmpire = EmpireManager.instance.GetPlayerEmpire();

        if (playerEmpire != null)
        {
            EmpireData playerEmpireData = playerEmpire.GetEmpireData();

            TechnologyEntry activeResearchPhysics = playerEmpireData.GetActiveResearchPhysics();
            TechnologyEntry activeResearchSociety = playerEmpireData.GetActiveResearchSociety();
            TechnologyEntry activeResearchEngineering = playerEmpireData.GetActiveResearchEngineering();

            if (activeResearchPhysics != null)
            {
                activeResearchPhysicsGroup.SetActive(true);
                selectResearchPhysicsButton.gameObject.SetActive(false);
            }
            else
            {
                activeResearchPhysicsGroup.SetActive(false);
                selectResearchPhysicsButton.gameObject.SetActive(true);
            }

            if (activeResearchSociety != null)
            {

            }
            else
            {

            }

            if (activeResearchEngineering != null)
            {

            }
            else
            {

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

    void BuildTechButtonList(TechnologyEntry[] techEntries, TechnologyButtonController.ButtonPress callBackFunction)
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

        Empire empire = EmpireManager.instance.GetPlayerEmpire();

        if (empire != null)
        {
            TechnologyEntry[] technologyEntries = empire.GetEmpireData().GetCurrentTechnologiesPhysics();

            BuildTechButtonList(technologyEntries, ClickedTechnologyEntryPhysics);
        }
    }

    public void ClickSocietySelectResearchButton()
    {
        ToggleTechnologyListPanel(true);

        Empire empire = EmpireManager.instance.GetPlayerEmpire();

        if (empire != null)
        {
            TechnologyEntry[] technologyEntries = empire.GetEmpireData().GetCurrentTechnologiesSociety();

            BuildTechButtonList(technologyEntries, ClickedTechnologyEntrySociety);
        }
    }

    public void ClickEngineeringSelectResearchButton()
    {
        ToggleTechnologyListPanel(true);

        Empire empire = EmpireManager.instance.GetPlayerEmpire();

        if (empire != null)
        {
            TechnologyEntry[] technologyEntries = empire.GetEmpireData().GetCurrentTechnologiesEngineering();

            BuildTechButtonList(technologyEntries, ClickedTechnologyEntryEngineering);
        }
    }

    public void ClickedTechnologyEntryPhysics(TechnologyEntry techEntry)
    {
        ToggleTechnologyListPanel(false);
    }

    public void ClickedTechnologyEntrySociety(TechnologyEntry techEntry)
    {
        ToggleTechnologyListPanel(false);
    }

    public void ClickedTechnologyEntryEngineering(TechnologyEntry techEntry)
    {
        ToggleTechnologyListPanel(false);
    }
}
