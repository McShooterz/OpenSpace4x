/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes: Defines all possible types of controllable units
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

[System.Serializable]
public enum ShipType
{
    //Non combat
    Freighter,
    //Combat
    Dreadnought,
    Battleship,
    Battlecruiser,
    Cruiser,
    Frigate,
    Destroyer,
    Corvette
}
