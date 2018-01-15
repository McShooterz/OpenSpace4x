using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CampaignPlanetPanel : MonoBehaviour
{
    [SerializeField]
    Planet planet;

    [SerializeField]
    bool playerControl;

    [SerializeField]
    Text planetName;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void SetPlanet(Planet newPlanet, bool controllable)
    {
        planet = newPlanet;
        playerControl = controllable;

        PlanetData planetData = planet.GetPlanetData();
        planetName.text = planetData.GetDisplayName();
    }

}
