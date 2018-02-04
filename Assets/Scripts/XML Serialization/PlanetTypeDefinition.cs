using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlanetTypeDefinition
{


    public string planetObject;


    
    public bool colonizable;



    public float fertilityMin;

    public float fertilityMax;



    public GameObject GetPlanetObject()
    {
        return ResourceManager.instance.GetPlanetObject(planetObject);
    }

}
