using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlanetTileData
{

    [SerializeField]
    PlanetData owner;



    public PlanetTileData(PlanetData data)
    {
        owner = data;
    }



}
