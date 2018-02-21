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

public class ModuleSetScrollList : BaseScrollList
{
    ModuleSetButton selectedButton;

    // Use this for initialization
    protected override void Start ()
    {
        base.Start();

    }

    // Update is called once per frame
    protected override void Update ()
    {
        base.Update();
	}

    public void BuildModuleSetsButtons(List<ModuleSet> ModuleSets, ModuleSetButton.ButtonPress ChangeModuleSet)
    {
        Clear();
        selectedButton = null;

        foreach (ModuleSet moduleSet in ModuleSets)
        {
            GameObject buttonObject = InstantiateEntry(moduleSet.GetName());
            ModuleSetButton newButton = buttonObject.GetComponent<ModuleSetButton>();
            newButton.SetModuleSet(moduleSet, ChangeModuleSet, SetModuleSetSelectionChange);

            if(selectedButton == null)
            {
                selectedButton = newButton;
                selectedButton.SetHighlight(true);
                ChangeModuleSet(moduleSet);
            }
        }
    }

    public void SetModuleSetSelectionChange(ModuleSetButton newSelectedButton)
    {
        if (selectedButton != newSelectedButton)
        {
            if (selectedButton != null)
            {
                selectedButton.SetHighlight(false);
            }

            selectedButton = newSelectedButton;
            selectedButton.SetHighlight(true);
        }
    }
}
