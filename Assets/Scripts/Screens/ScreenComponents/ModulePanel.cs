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
        //testing
        List<ModuleSet> ModuleSets = new List<ModuleSet>();
        foreach(KeyValuePair<string, ModuleSet> keyVal in ResourceManager.ModuleSets)
        {
            ModuleSets.Add(keyVal.Value);
        }
        moduleSetScrollList.BuildModuleSetsButtons(ModuleSets, ChangeModuleSet);


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

    void BuildModuleSetButtons(List<ModuleSet> moduleSetList)
    {

    }

    public void ClickWeaponsCategory()
    {
        if(Category != ModuleCategory.Weapons)
        {
            Category = ModuleCategory.Weapons;

            ClearModuleButtons();
        }
    }

    public void ClickDefenseCategory()
    {
        if (Category != ModuleCategory.Defenses)
        {
            Category = ModuleCategory.Defenses;

            ClearModuleButtons();
        }
    }

    public void ClickSystemsCategory()
    {
        if (Category != ModuleCategory.Systems)
        {
            Category = ModuleCategory.Systems;

            ClearModuleButtons();
        }
    }

    void ClearModuleButtons()
    {
        moduleSetScrollList.Clear();
    }

    public ModuleSet GetSelectedModuleSet()
    {
        return SelectedModuleSet;
    }

    public void ChangeModuleSet(ModuleSet moduleSet)
    {

    }

    public enum ModuleCategory
    {
        Weapons,
        Defenses,
        Systems
    }
}
