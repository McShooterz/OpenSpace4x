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

public class ModuleButtonList : MonoBehaviour
{
    [SerializeField]
    GameObject ModuleButtonPrefab;

    ModuleButton selectedModuleButton;


    List<ModuleButton> ModuleButtons = new List<ModuleButton>();

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void Clear()
    {
        foreach (ModuleButton button in ModuleButtons)
        {
            Destroy(button.gameObject);
        }
        ModuleButtons.Clear();
        selectedModuleButton = null;
    }

    public void ChangeSelection(ModuleButton newButton)
    {
        if (selectedModuleButton != newButton)
        {
            if (selectedModuleButton != null)
            {
                selectedModuleButton.SetHighlight(false);
            }
            selectedModuleButton = newButton;
            selectedModuleButton.SetHighlight(true);
        }
    }

    public void CreateModuleButtons(List<Module> Modules, ModuleButton.ButtonPress buttonPress)
    {
        Clear();

        foreach(Module module in Modules)
        {
            GameObject buttonObject = Instantiate(ModuleButtonPrefab);
            buttonObject.transform.SetParent(transform);
            buttonObject.transform.localScale = Vector3.one;
            ModuleButton moduleButton = buttonObject.GetComponent<ModuleButton>();
            moduleButton.SetModule(module, buttonPress, ChangeSelection);
            ModuleButtons.Add(moduleButton);

            if (selectedModuleButton == null)
            {
                selectedModuleButton = moduleButton;
                selectedModuleButton.SetHighlight(true);
                buttonPress(module);
            }
        }
    }
}
