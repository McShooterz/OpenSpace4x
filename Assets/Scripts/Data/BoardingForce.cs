/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class BoardingForce 
{
	int Troops;
	int Crew;
	ShipManager manager;

	public BoardingForce(ShipManager shipManager, int troopCount, int crewCount)
	{
		manager = shipManager;
		Troops = troopCount;
		Crew = crewCount;
	}

	public ShipManager GetShipManager()
	{
		return manager;
	}

	public void AddForces(int troops, int crew)
	{
		Troops += troops;
		Crew += crew;
	}

	public bool HasTroops()
	{
		return Troops > 0;
	}

	public bool HasCrew()
	{
		return Crew > 0;
	}

	public void KillTrooper()
	{
		Troops--;
	}

	public void KillCrew()
	{
		Crew--;
	}

    public int GetTroopCount()
    {
        return Troops;
    }

    public int GetCrewCount()
    {
        return Crew;
    }

    public float GetCombatStrength()
    {
        return Troops + Crew * 0.5f;
    }
}
