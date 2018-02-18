using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlanetTileData
{

    [SerializeField]
    PlanetData planetData;



    public PlanetTileData(PlanetData data)
    {
        planetData = data;
    }



}
