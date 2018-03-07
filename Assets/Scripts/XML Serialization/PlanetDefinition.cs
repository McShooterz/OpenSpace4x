using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetDefinition
{
    public string name;

    public string description;

    public string[] planetObjects;

    public float randomGalaxyWeight;

    public TileEntry[] potentialTiles = new TileEntry[0];

    public int sizeMin;

    public int sizeMax;
    
    public bool colonizable;

    public float populationMultiplier;

    public float fertility;

    public float metalRichnessMin;

    public float metalRichnessMax;

    public float crystalRichnessMin;

    public float crystalRichnessMax;

    public PlanetDefinition()
    {

    }

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

    public TileDefinition GetRandomTileDefinition()
    {
        if (potentialTiles.Length > 0)
        {
            float[] weights = new float[potentialTiles.Length];

            for (int i = 0; i < potentialTiles.Length; i++)
            {
                weights[i] = potentialTiles[i].randomWeight;
            }

            return potentialTiles[StaticHelpers.GetRandomIndexByWeight(weights)].GetTile();
        }

        return null;
    }

    public class TileEntry
    {
        public string tile;

        public float randomWeight;

        public TileDefinition GetTile()
        {
            return ResourceManager.instance.GetTileDefinition(tile);
        }
    }
}
