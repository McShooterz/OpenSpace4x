using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlanetTypeDefinition
{


    public string name;
    public string description;

    public string planetObject;


    
    public bool colonizable;



    public float fertilityMin;

    public float fertilityMax;


    public string GetName()
    {
        return ResourceManager.instance.GetLocalization(name);
    }

    public string GetDescription()
    {
        return ResourceManager.instance.GetLocalization(description);
    }

    public GameObject GetPlanetObject()
    {
        return ResourceManager.instance.GetPlanetObject(planetObject);
    }

}
