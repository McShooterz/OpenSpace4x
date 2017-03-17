/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public abstract class UnitDesign
{
    public string Name;
    public string Hull;
    public bool Deleted;

    protected string Path = "";

    public string GetPath()
    {
        return Path;
    }

    public void SetPath(string newPath)
    {
        Path = newPath;
    }
}
