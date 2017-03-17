/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class DesignModule
{
    public string Module;
    public float Rotation;
    public int Position;

    public DesignModule(string module, float rotation, int position)
    {
        Module = module;
        Rotation = rotation;
        Position = position;
    }

    public DesignModule(){}

    public bool ModuleExists()
    {
        return ResourceManager.ModuleExists(Module);
    }
}
