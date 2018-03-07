using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class PlanetTileController : MonoBehaviour
{
    PlanetTile planetTile;

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

    public void SetPlanetTile(PlanetTile tile)
    {
        planetTile = tile;
        SetTileBackground(planetTile.GetImage());
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

    public void SetTileBackground(Sprite image)
    {
        tileBackGround.sprite = image;
    }
}
