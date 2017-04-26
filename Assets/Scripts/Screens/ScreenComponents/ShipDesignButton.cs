/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipDesignButton : baseToggleButton
{
    [SerializeField]
    Text DesignName;
    [SerializeField]
    Image CommandIcon;
    [SerializeField]
    Text CommandValue;
    [SerializeField]
    Image CostIcon;
    [SerializeField]
    Text CostValue;

    ShipDesign shipDesign;

    public delegate void ButtonPress(ShipDesign design);
    protected ButtonPress buttonCallBack;

    public delegate void Select(ShipDesignButton button);
    protected Select selectCallBack;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update ()
    {
		
	}

    public void SetShipDesign(ShipDesign design, ButtonPress callBack, Select select)
    {
        shipDesign = design;
        buttonCallBack = callBack;
        selectCallBack = select;

        ShipDesignData designData = shipDesign.GetShipDesignData();

        DesignName.text = shipDesign.Name;
        CommandValue.text = designData.CommandPoints.ToString();
        CostValue.text = designData.GetTotalValue().ToString("0.#");

        Texture2D texture = ResourceManager.GetIconTexture("Icon_Command");
        CommandIcon.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        CommandIcon.preserveAspect = true;

        texture = ResourceManager.GetIconTexture("Icon_Money");
        CostIcon.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        CostIcon.preserveAspect = true;
    }

    public void ButtonClick()
    {
        selectCallBack(this);
        buttonCallBack(shipDesign);
    }
}
