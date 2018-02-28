using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CampaignMainScreen : MonoBehaviour
{
    [SerializeField]
    Canvas canvas;

    [SerializeField]
    EmpireInfoBarController empireInfoBar;

    [SerializeField]
    ResearchWindow researchWindow;

    [SerializeField]
    CampaignPlanetPanel planetPanel;

    [SerializeField]
    PlanetInfoQuick planetInfoQuick;

    [SerializeField]
    RectTransform planetInfoQuickRectTransform;

    [SerializeField]
    LayerMask cursorHoveringLayerMask;

    [SerializeField]
    LayerMask leftClickLayerMask;

    [SerializeField]
    LayerMask rightClickLayerMask;

    Vector2 planetInfoOffSet;


    // Hovered Objects

    GameObject hoverPlanet;

    // Use this for initialization
    void Start ()
    {
        ToggleResearchWindow(false);

        planetPanel.gameObject.SetActive(false);

        planetInfoOffSet = new Vector2(planetInfoQuickRectTransform.rect.width / 2, planetInfoQuickRectTransform.rect.height / 2);
    }
	
	// Update is called once per frame
	void Update ()
    {
        CheckMouseHovering();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleResearchWindow(false);
        }

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonUp(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, leftClickLayerMask, QueryTriggerInteraction.Collide))
                {
                    if (hit.transform.root.tag == "Fleet")
                    {
                        Empire playerEmpire = EmpireManager.instance.GetPlayerEmpire();

                        DeselectPlanet();

                        FleetController fleet = hit.collider.transform.root.gameObject.GetComponent<FleetController>();
                        bool playerOwned = playerEmpire.GetFleetManager().OwnsFleet(fleet);

                        if (!Input.GetKey(KeyCode.LeftShift) || !playerOwned)
                        {
                            playerEmpire.GetFleetManager().DeslectFleets();
                        }

                        if (playerEmpire.GetFleetManager().OwnsFleet(fleet))
                        {
                            playerEmpire.GetFleetManager().AddToSelection(fleet);
                        }
                        else
                        {
                            playerEmpire.GetFleetManager().SetSelectedOtherFleet(fleet);
                        }
                    }
                    else if (hit.transform.root.tag == "Planet")
                    {
                        Empire playerEmpire = EmpireManager.instance.GetPlayerEmpire();
                        playerEmpire.GetFleetManager().DeslectFleets();
                        DeselectPlanet();

                        SelectPlanet(hit.transform.root.gameObject.GetComponent<PlanetController>());
                    }
                    else
                    {
                        Empire playerEmpire = EmpireManager.instance.GetPlayerEmpire();
                        playerEmpire.GetFleetManager().DeslectFleets();

                        DeselectPlanet();
                    }
                }
                else
                {
                    Empire playerEmpire = EmpireManager.instance.GetPlayerEmpire();
                    playerEmpire.GetFleetManager().DeslectFleets();

                    DeselectPlanet();
                }
            }

            if (Input.GetMouseButtonUp(1))
            {
                Empire playerEmpire = EmpireManager.instance.GetPlayerEmpire();

                if (playerEmpire.GetFleetManager().GetSelectedFleets().Count > 0)
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, rightClickLayerMask, QueryTriggerInteraction.Collide))
                    {
                        if (hit.transform.root.tag == "PlayArea")
                        {
                            playerEmpire.GetFleetManager().SetSelectedGoalPosition(hit.point);
                        }
                    }
                }
            }
        }
	}

    void CheckMouseHovering()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, cursorHoveringLayerMask, QueryTriggerInteraction.Collide))
            {
                if (hit.transform.root.tag == "Planet")
                {
                    planetInfoQuick.gameObject.SetActive(true);
                    Vector2 pos;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, new Vector3(Input.mousePosition.x + planetInfoOffSet.x, Input.mousePosition.y - planetInfoOffSet.y, Input.mousePosition.z), canvas.worldCamera, out pos);
                    planetInfoQuickRectTransform.position = canvas.transform.TransformPoint(pos);

                    if (hoverPlanet != hit.transform.root.gameObject)
                    {
                        hoverPlanet = hit.transform.root.gameObject;

                        PlanetController planetController = hoverPlanet.GetComponent<PlanetController>();

                        planetInfoQuick.SetPlanetTypeText(planetController.GetTypeDefinition().GetName());
                        planetInfoQuick.SetPlanetSizeText("Size: " + planetController.GetSize().ToString());
                    }
                }
                else
                {
                    planetInfoQuick.gameObject.SetActive(false);
                }
            }
            else
            {
                planetInfoQuick.gameObject.SetActive(false);
            }
        }
        else
        {
            planetInfoQuick.gameObject.SetActive(false);
        }
    }

    void SelectPlanet(PlanetController planet)
    {
        Empire playerEmpire = EmpireManager.instance.GetPlayerEmpire();
        playerEmpire.GetPlanetManager().SetSelectedPlanet(planet);
        planetPanel.gameObject.SetActive(true);
        planetPanel.SetPlanet(planet, playerEmpire.GetPlanetManager().OwnsPlanet(planet));
    }

    void DeselectPlanet()
    {
        EmpireManager.instance.GetPlayerEmpire().GetPlanetManager().SetSelectedPlanet(null);
        planetPanel.gameObject.SetActive(false);
    }

    public void ToggleResearchWindow(bool state)
    {
        researchWindow.gameObject.SetActive(state);
    }

    public void ChangeMonth()
    {
        researchWindow.UpdateCurrentResearch();
    }
}
