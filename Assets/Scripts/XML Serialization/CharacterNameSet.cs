/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections.Generic;

public class CharacterNameSet
{
    List<string> FirstNames = new List<string>();
    List<string> LastNames = new List<string>();

    public string GetRandomName()
    {
        string Name = "";
        if(FirstNames.Count > 0)
        {
            Name = FirstNames[Random.Range(0, FirstNames.Count)] + " ";
        }
        if(LastNames.Count > 0)
        {
            Name = Name + LastNames[Random.Range(0, LastNames.Count)];
        }
        return Name;
    }
}
