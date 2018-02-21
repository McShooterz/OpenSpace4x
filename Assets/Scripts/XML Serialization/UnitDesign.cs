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
    public string Name { get; set; }
    public string Hull { get; set; }
    public bool Deleted { get; set; }

    protected string Path = "";

    public UnitDesign()
    {
        Name = "";
        Hull = "";
    }

    public string GetPath()
    {
        return Path;
    }

    public void SetPath(string newPath)
    {
        Path = newPath;
    }
}
