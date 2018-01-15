using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    [SerializeField]
    List<Planet> planets = new List<Planet>();

    [SerializeField]
    Planet selectedPlanet;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public List<Planet> GetPlanets()
    {
        return planets;
    }

    public bool OwnsPlanet(Planet planet)
    {
        return planets.Contains(planet);
    }

    public Planet GetSelectedPlanet()
    {
        return selectedPlanet;
    }

    public void SetSelectedPlanet(Planet planet)
    {
        selectedPlanet = planet;
    }
}
