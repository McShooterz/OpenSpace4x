/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections.Generic;

public class ModuleSet
{
    //In xml files
    public string Name;
    public string Description;
    public ModuleCategory ModuleCategory;
    public ModuleSetRestriction SetRestriction;
    public ModuleSetSwapType SwapType;
    public List<string> Modules = new List<string>();

    //Set when loading
    ModInfo parentMod;

    public void SetParentMod(ModInfo mod)
    {
        parentMod = mod;
    }

    public ModInfo GetParentMod()
    {
        return parentMod;
    }

    public string GetName()
    {
        return ResourceManager.GetLocalization(Name);
    }

    public string GetDescription()
    {
        return ResourceManager.GetLocalization(Description);
    }

    public List<Module> GetModules()
    {
        List<Module> modules = new List<Module>();
        foreach(string moduleName in Modules)
        {
            Module module = ResourceManager.GetModule(moduleName);
            if (module != null)
                modules.Add(module);
        }
        return modules;
    }

    public Module GetFirstModule()
    {
        foreach (string moduleName in Modules)
        {
            Module module = ResourceManager.GetModule(moduleName);
            if (module != null)
                return module;
        }
        return null;
    }

    public Texture2D GetTexture()
    {
        foreach (string moduleName in Modules)
        {
            Module module = ResourceManager.GetModule(moduleName);
            if (module != null)
                return module.GetTexture();
        }
        return ResourceManager.ErrorTexture;
    }
}
