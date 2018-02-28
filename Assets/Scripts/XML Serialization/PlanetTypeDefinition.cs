using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetTypeDefinition
{
    public string name;

    public string description;

    public string[] planetObjects;

    public float randomGalaxyWeight;

    public bool hasSurface;

    public int sizeMin;

    public int sizeMax;
    
    public bool colonizable;

    public float populationMultiplier;

    public float fertility;

    public float metalRichnessMin;

    public float metalRichnessMax;

    public float crystalRichnessMin;

    public float crystalRichnessMax;


    public string GetName()
    {
        return ResourceManager.instance.GetLocalization(name);
    }

    public string GetDescription()
    {
        return ResourceManager.instance.GetLocalization(description);
    }

    public PlanetController CreatePlanetInstance()
    {
        PlanetController planetController = ResourceManager.instance.CreatePlanet(planetObjects[Random.Range(0, planetObjects.Length)]);
        planetController.SetPlanetType(this);
        return planetController;
    }

    public int GetRandomSize()
    {
        return Random.Range(sizeMin, sizeMax + 1);
    }

    public float GetRandomMetal()
    {
        return Random.Range(metalRichnessMin, metalRichnessMax);
    }

    public float GetRandomCrystal()
    {
        return Random.Range(crystalRichnessMin, crystalRichnessMax);
    }
}
