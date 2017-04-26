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

public class ShipHullDataButton : baseToggleButton
{
    [SerializeField]
    Image HullIcon;
    [SerializeField]
    Text HullName;
    [SerializeField]
    Text HullClassification;
    [SerializeField]
    Text HullDescription;

    ShipHullData shipHullData;

    public delegate void ButtonPress(ShipHullData data);
    protected ButtonPress buttonCallBack;

    public delegate void Select(ShipHullDataButton button);
    protected Select selectCallBack;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetShipHullData(ShipHullData data, ButtonPress callBack, Select select)
    {
        shipHullData = data;
        buttonCallBack = callBack;
        selectCallBack = select;

        HullName.text = shipHullData.Name;
        HullClassification.text = shipHullData.Classification.ToString();
        HullDescription.text = "Temp Description";

        Texture2D texture = shipHullData.GetIcon();
        HullIcon.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        HullIcon.preserveAspect = true;
    }

    public void ButtonClick()
    {
        selectCallBack(this);
        buttonCallBack(shipHullData);
    }
}
