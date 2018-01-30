/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ModuleSet
{
    //In xml files
    public string Name { get; set; }
    public string Description { get; set; }
    public ModuleCategory ModuleCategory { get; set; }
    public ModuleSetRestriction SetRestriction { get; set; }
    public ModuleSetSwapType SwapType { get; set; }
    public List<string> Modules { get; set; }

    //Set when loading
    ModInfo parentMod;

    //Set defaults in constructor
    public ModuleSet()
    {
        Name = "";
        Description = "";
        ModuleCategory = ModuleCategory.Weapons;
        SetRestriction = ModuleSetRestriction.Universal;
        SwapType = ModuleSetSwapType.None;

        Modules = new List<string>();
    }

    public void SetParentMod(ModInfo mod)
    {
        parentMod = mod;
    }

    public ModInfo GetParentMod()
    {
        return parentMod;
    }

    public string GetParentModName()
    {
        if(parentMod != null)
        {
            return parentMod.Name;
        }
        return "N/A";
    }

    public string GetName()
    {
        return ResourceManager.instance.GetLocalization(Name);
    }

    public string GetDescription()
    {
        return ResourceManager.instance.GetLocalization(Description);
    }

    public List<Module> GetModules()
    {
        List<Module> modules = new List<Module>();
        foreach(string moduleName in Modules)
        {
            Module module = ResourceManager.instance.GetModule(moduleName);
            if (module != null)
                modules.Add(module);
        }
        return modules;
    }

    public Module GetFirstModule()
    {
        foreach (string moduleName in Modules)
        {
            Module module = ResourceManager.instance.GetModule(moduleName);
            if (module != null)
                return module;
        }
        return null;
    }

    public Sprite GetTexture()
    {
        foreach (string moduleName in Modules)
        {
            Module module = ResourceManager.instance.GetModule(moduleName);
            if (module != null)
                return module.GetTexture();
        }
        return ResourceManager.instance.GetErrorTexture();
    }
}
