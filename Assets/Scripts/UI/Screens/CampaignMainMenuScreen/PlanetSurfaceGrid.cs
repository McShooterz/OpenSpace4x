using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetSurfaceGrid : MonoBehaviour
{
    [SerializeField]
    GridLayoutGroup gridLayoutGroup;

    [SerializeField]
    PlanetTileController[] planetTileControllers;

    [SerializeField]
    PlanetTileController selectedPlanetTileController;

    [SerializeField]
    GameObject planetTilePrefab;

    [SerializeField]
    PlanetController planetController;

	// Use this for initialization
	void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void SetPlanetData(PlanetController planet)
    {
        planetController = planet;
    }

    public void CreatePlanetTiles(List<PlanetTileData> planetTileDataList)
    {
        ClearTiles();

        planetTileControllers = new PlanetTileController[planetTileDataList.Count];

        for(int i = 0; i < planetTileDataList.Count; i++)
        {
            planetTileControllers[i] = Instantiate(planetTilePrefab, gameObject.transform).GetComponent<PlanetTileController>();
            planetTileControllers[i].SetPlanetTileData(planetTileDataList[i]);
            planetTileControllers[i].SetCallBackFunction(SelectPlanetTile);
        }

        // Adjust the number of rows in the grid
        gridLayoutGroup.constraintCount = Mathf.CeilToInt(Mathf.Sqrt(planetTileDataList.Count));
    }

    public void ClearTiles()
    {
        for (int i = 0; i < planetTileControllers.Length; i++)
        {
            Destroy(planetTileControllers[i].gameObject);
        }

        planetTileControllers = null;
    }

    public void SelectPlanetTile(PlanetTileController planetTileController)
    {
        selectedPlanetTileController = planetTileController;
    }
}
