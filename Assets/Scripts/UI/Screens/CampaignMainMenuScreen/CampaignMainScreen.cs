using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CampaignMainScreen : MonoBehaviour
{
    [SerializeField]
    RayCasterCameraToMouse rayCasterCameraToMouse;

    [SerializeField]
    CampaignPlanetPanel planetPanel;

    [SerializeField]
    CampaignEmpireInfoBar empireInfoBar;

    // Use this for initialization
    void Start ()
    {
        planetPanel.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (Input.GetMouseButtonUp(0))
        {
            if (rayCasterCameraToMouse.IsValidCast())
            {
                RaycastHit hit = rayCasterCameraToMouse.GetRayCastHit();

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

                    SelectPlanet(hit.transform.root.gameObject.GetComponent<Planet>());
                }
                else
                {
                    Empire playerEmpire = EmpireManager.instance.GetPlayerEmpire();
                    playerEmpire.GetFleetManager().DeslectFleets();

                    DeselectPlanet();
                }
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            Empire playerEmpire = EmpireManager.instance.GetPlayerEmpire();

            if (playerEmpire.GetFleetManager().GetSelectedFleets().Count > 0)
            {
                if (rayCasterCameraToMouse.IsValidCast())
                {
                    RaycastHit hit = rayCasterCameraToMouse.GetRayCastHit();

                    if (hit.transform.root.tag == "PlayArea")
                    {
                        playerEmpire.GetFleetManager().SetSelectedGoalPosition(hit.point);
                    }
                }
            }
        }

	}







    void SelectPlanet(Planet planet)
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
}
