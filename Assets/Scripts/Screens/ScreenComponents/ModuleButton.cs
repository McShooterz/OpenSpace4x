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

public class ModuleButton : baseToggleButton
{

    [SerializeField]
    Image ModuleImage;

    Module module;

    public delegate void ButtonPress(Module mod);
    protected ButtonPress buttonCallBack;

    public delegate void Select(ModuleButton button);
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

    public void SetModule(Module mod, ButtonPress callBack, Select select)
    {
        module = mod;
        buttonCallBack = callBack;
        selectCallBack = select;

        ModuleImage.overrideSprite = module.GetTexture();
        ModuleImage.preserveAspect = true;
    }

    public void ButtonClick()
    {
        if(module != null)
        {
            buttonCallBack(module);
        }
        selectCallBack(this);
    }

    public Module GetModule()
    {
        return module;
    }
}
