using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class PlanetTileController : MonoBehaviour
{
    [SerializeField]
    Image tileBackGround;

    [SerializeField]
    Image tileBoarder;

    [SerializeField]
    Image buildingImage;

    [SerializeField]
    Slider progressBar;

    [SerializeField]
    Image tileBonusIcon;

    [SerializeField]
    Text tileBonusText;

    PlanetTileData planetTileData;





    public delegate void ButtonPress(PlanetTileController planetTileController);
    protected ButtonPress buttonCallBack;




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

    public void SetCallBackFunction(ButtonPress callBack)
    {
        buttonCallBack = callBack;
    }

    public void ClickTile()
    {
        if (buttonCallBack != null)
        {
            buttonCallBack(this);
        }
    }
}
