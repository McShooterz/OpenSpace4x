/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections.Generic;

public class ShipNameSet
{
    public List<string> UniversalShipNames { get; set; }

    public List<string> CorvetteNames { get; set; }
    public List<string> DestroyerNames { get; set; }
    public List<string> FrigateNames { get; set; }
    public List<string> CruiserNames { get; set; }
    public List<string> BattleCruiserNames { get; set; }
    public List<string> BattleShipNames { get; set; }
    public List<string> DreadnoughtNames { get; set; }

    public List<string> UniversalStationNames { get; set; }

    public List<string> PlatformNames { get; set; }
    public List<string> OutpostNames { get; set; }
    public List<string> StarbaseNames { get; set; }

    public ShipNameSet()
    {
        UniversalShipNames = new List<string>();
        CorvetteNames = new List<string>();
        DestroyerNames = new List<string>();
        FrigateNames = new List<string>();
        CruiserNames = new List<string>();
        BattleCruiserNames = new List<string>();
        BattleShipNames = new List<string>();
        DreadnoughtNames = new List<string>();

        UniversalStationNames = new List<string>();

        PlatformNames = new List<string>();
        OutpostNames = new List<string>();
        StarbaseNames = new List<string>();
    }
}
