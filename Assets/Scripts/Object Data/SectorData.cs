/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System;
using System.Collections;

public class SectorData
{
    public Guid GUID = new Guid();

    public Sector sector;

    public Vector3 Position;


    public SectorData()
    {
        GUID = Guid.NewGuid();
    }
}
