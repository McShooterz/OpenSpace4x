using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    [SerializeField]
    List<PlanetController> planets = new List<PlanetController>();

    [SerializeField]
    PlanetController selectedPlanet;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public List<PlanetController> GetPlanets()
    {
        return planets;
    }

    public void AddPlanet(PlanetController planet)
    {
        planets.Add(planet);
    }

    public bool OwnsPlanet(PlanetController planet)
    {
        return planets.Contains(planet);
    }

    public PlanetController GetSelectedPlanet()
    {
        return selectedPlanet;
    }

    public void SetSelectedPlanet(PlanetController planet)
    {
        selectedPlanet = planet;
    }

    public void ChangeDay()
    {
        foreach (PlanetController planet in planets)
        {
            planet.ChangeDay();
        }
    }
}
