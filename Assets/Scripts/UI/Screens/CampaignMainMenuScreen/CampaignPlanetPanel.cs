using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CampaignPlanetPanel : MonoBehaviour
{
    [SerializeField]
    PlanetController planet;

    [SerializeField]
    bool playerControl;

    [SerializeField]
    Text planetName;




    [SerializeField]
    PlanetSurfaceGrid planetSurfaceGrid;

    [SerializeField]
    CampaignPlanetPanelExtension panelExtension;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void SetPlanet(PlanetController newPlanet, bool controllable)
    {
        planet = newPlanet;
        playerControl = controllable;

        PlanetData planetData = planet.GetPlanetData();
        planetName.text = planetData.GetDisplayName();
    }

}
