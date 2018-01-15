using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetTileController : MonoBehaviour
{
    [SerializeField]
    Image tileBackGround;

    [SerializeField]
    Image tileBoarder;

    [SerializeField]
    Image buildingImage;

    [SerializeField]
    Image populationImage;





    [SerializeField]
    Image adjacencyBottom;

    [SerializeField]
    Image adjacencyTop;

    [SerializeField]
    Image adjacencyLeft;

    [SerializeField]
    Image adjacencyRight;

    [SerializeField]
    PlanetTileData planetTileData;

    //to do
    // moral bar
    // construction bar

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void SetPlanetTileData(PlanetTileData data)
    {
        planetTileData = data;
    }
}
