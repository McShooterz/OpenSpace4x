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

public class ModulePanel : MonoBehaviour
{
    #region Variables

    [SerializeField]
    Button WeaponsButton;
    [SerializeField]
    Button DefensesButton;
    [SerializeField]
    Button SystemsButton;

    [SerializeField]
    ModuleSetScrollList moduleSetScrollList;

    [SerializeField]
    ModuleButtonList moduleButtonList;

    List<ModuleButton> ModuleButtons = new List<ModuleButton>();

    List<ModuleSet> WeaponModuleSets = new List<ModuleSet>();
    List<ModuleSet> DefenseModuleSets = new List<ModuleSet>();
    List<ModuleSet> SystemModuleSets = new List<ModuleSet>();

    ModuleSet SelectedModuleSet;

    Module SelectedModule;

    ModuleCategory Category = ModuleCategory.Weapons;

    #endregion

    // Use this for initialization
    void Start ()
    {
        // Set localization
        WeaponsButton.GetComponentInChildren<Text>().text = ResourceManager.GetLocalization("Weapons");
        DefensesButton.GetComponentInChildren<Text>().text = ResourceManager.GetLocalization("Defenses");
        SystemsButton.GetComponentInChildren<Text>().text = ResourceManager.GetLocalization("Systems");

    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void SetWeaponModuleSetList(List<ModuleSet> moduleSetList)
    {
        WeaponModuleSets = moduleSetList;
    }

    public void SetDefenseModuleSetList(List<ModuleSet> moduleSetList)
    {
        DefenseModuleSets = moduleSetList;
    }

    public void SetSystemModuleSetList(List<ModuleSet> moduleSetList)
    {
        SystemModuleSets = moduleSetList;
    }

    public void BuildModuleSetButtons(List<ModuleSet> moduleSetList)
    {
        moduleSetScrollList.BuildModuleSetsButtons(moduleSetList, ChangeModuleSet);
    }

    public void ClickWeaponsCategory()
    {
        if(Category != ModuleCategory.Weapons)
        {
            Category = ModuleCategory.Weapons;
            BuildModuleSetButtons(WeaponModuleSets);
        }
    }

    public void ClickDefenseCategory()
    {
        if (Category != ModuleCategory.Defenses)
        {
            Category = ModuleCategory.Defenses;
            BuildModuleSetButtons(DefenseModuleSets);
        }
    }

    public void ClickSystemsCategory()
    {
        if (Category != ModuleCategory.Systems)
        {
            Category = ModuleCategory.Systems;
            BuildModuleSetButtons(SystemModuleSets);
        }
    }

    void ClearModuleSetButtons()
    {
        moduleSetScrollList.Clear();
    }

    public ModuleSet GetSelectedModuleSet()
    {
        return SelectedModuleSet;
    }

    public void ChangeModuleSet(ModuleSet moduleSet)
    {
        moduleButtonList.CreateModuleButtons(moduleSet.GetModules(), ChangeModule);
    }

    public void ChangeModule(Module module)
    {
        
    }

    public enum ModuleCategory
    {
        Weapons,
        Defenses,
        Systems
    }
}
