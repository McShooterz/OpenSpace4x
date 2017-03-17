/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public enum ShipBehaviorType
{
    BestGuess, //The AI just uses it as it see fit
    Skirmisher, //Constantly moving around
    Brawler, //Moves in close and takes most of the damage
    Artillery, //Sits at max range
    Support //Sits back and doesn't engage
}
