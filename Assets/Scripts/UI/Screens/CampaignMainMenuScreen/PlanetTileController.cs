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

    public PlanetTile GetPlanetTile()
    {
        return planetTile;
    }

    public void SetPlanetTile(PlanetTile tile)
    {
        planetTile = tile;

        UpdateUI();
        UpdateBuildingUI();
    }

    public void UpdateUI()
    {
        if (planetTile != null)
        {
            SetTileBackground(planetTile.GetImage());

            if (planetTile.HasBonus())
            {
                tileBonusIcon.gameObject.SetActive(true);
                tileBonusText.gameObject.SetActive(true);

                tileBonusIcon.sprite = planetTile.GetBonusIcon();
                tileBonusText.text = "+" + (planetTile.GetTileBonusValue() * 100f).ToString("0.#") + "%";
            }
            else
            {
                tileBonusIcon.gameObject.SetActive(false);
                tileBonusText.gameObject.SetActive(false);
            }
        }
    }

    public void UpdateBuildingUI()
    {
        if (planetTile != null)
        {
            if (planetTile.GetCurrentBuilding() != null)
            {
                buildingImage.gameObject.SetActive(true);
                buildingImage.sprite = planetTile.GetCurrentBuilding().GetImage();
                buildingImage.color = Color.white;


                if (planetTile.GetNextBuilding() != null)
                {
                    progressBar.gameObject.SetActive(true);
                    progressBar.value = planetTile.GetBuidlingProgress(1.0f);
                }
                else
                {
                    progressBar.gameObject.SetActive(false);
                }

            }
            else if (planetTile.GetNextBuilding() != null)
            {
                buildingImage.gameObject.SetActive(true);
                buildingImage.sprite = planetTile.GetNextBuilding().GetImage();
                buildingImage.color = new Color(1f, 1f, 1f, 0.8f);

                progressBar.gameObject.SetActive(true);
                print("Returned progress bar: " + planetTile.GetBuidlingProgress(1.0f).ToString());
                progressBar.value = planetTile.GetBuidlingProgress(1.0f);
            }
            else
            {
                buildingImage.gameObject.SetActive(false);
                progressBar.gameObject.SetActive(false);
            }
        }
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
