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

public sealed class MainShipyardScreen : BaseShipDesignScreen
{
    #region Variables

    [SerializeField]
    ShipHullPanel shipHullPanel;

    #endregion


    protected override void Start()
    {
        BuildModuleSetLists();
        BuildShipHullDatas();
    }

    void BuildModuleSetLists()
    {
        List<ModuleSet> WeaponModuleSets = new List<ModuleSet>();
        List<ModuleSet> DefenseModuleSets = new List<ModuleSet>();
        List<ModuleSet> SystemModuleSets = new List<ModuleSet>();

        //Distribute modules into lists
        foreach (KeyValuePair<string, ModuleSet> keyVal in ResourceManager.ModuleSets)
        {
            if (CheckModuleSetAllowed(keyVal.Value))
            {
                if (keyVal.Value.ModuleCategory == ModuleCategory.Weapons)
                {
                    WeaponModuleSets.Add(keyVal.Value);
                }
                else if (keyVal.Value.ModuleCategory == ModuleCategory.Defences)
                {
                    DefenseModuleSets.Add(keyVal.Value);
                }
                else
                {
                    SystemModuleSets.Add(keyVal.Value);
                }             
            }
        }

        //Apply to module panel
        ModulePanel.SetWeaponModuleSetList(WeaponModuleSets);
        ModulePanel.SetDefenseModuleSetList(DefenseModuleSets);
        ModulePanel.SetSystemModuleSetList(SystemModuleSets);

        ModulePanel.BuildModuleSetButtons(WeaponModuleSets);
    }

    void BuildShipHullDatas()
    {
        List<ShipHullData> shipHullDatas = new List<ShipHullData>();

        foreach(KeyValuePair<string, ShipHullData> keyVal in ResourceManager.GetShipHulls())
        {
            shipHullDatas.Add(keyVal.Value);
        }

        shipHullPanel.SetShipHullDatas(shipHullDatas, ChangeHull);
    }

    public void ChangeHull(ShipHullData data)
    {

    }
}
