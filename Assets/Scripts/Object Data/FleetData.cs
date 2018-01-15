/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class FleetData
{
    [SerializeField]
    public Empire Owner;

    [SerializeField]
    List<ShipData> Ships = new List<ShipData>();

    [SerializeField]
    List<StationData> Stations = new List<StationData>();

    [SerializeField]
    ShipData FlagShip;

    [SerializeField]
	int CommandPoints = 0;

    [SerializeField]
    float SpeedFTL = 0;

    [SerializeField]
    float ShipDefenceBonus = 0;

    [SerializeField]
    float ShipDamageBonus = 0;

    [SerializeField]
	float Transport = 0;





    [SerializeField]
    float experience = 0;

    [SerializeField]
    int level = 0;

    [SerializeField]
    int legacyTradionPoints = 0;

    public void AddShip(ShipData data)
    {
        if (data.fleetData != null)
            data.fleetData.RemoveShip(data);
        data.SetFleet(this);
        Ships.Add(data);
		AddShipStats (data);      
    }

    public void AddStation(StationData data)
    {
        if (data.fleetData != null)
            data.fleetData.RemoveStation(data);
        data.SetFleet(this);
        Stations.Add(data);
    }

	public void AddShipStats(ShipData ship)
	{
		CommandPoints += ship.commandPoints;
		if(SpeedFTL == 0)
		{
			SpeedFTL = ship.FTLSpeed;
		}
		else
		{
			SpeedFTL = Mathf.Min(SpeedFTL, ship.FTLSpeed);
		}

		//Update best Ship Defense Bonus
		ShipDefenceBonus = Mathf.Max(ShipDefenceBonus, ship.designData.DefenseBonusFleet);
		ShipDamageBonus = Mathf.Max(ShipDamageBonus, ship.designData.DamageBonusFleet);

		Transport += ship.transporter;
	}

    public void RemoveShip(ShipData data)
    {
        Ships.Remove(data);
        data.SetFleet(null);
        if (FlagShip == data)
        {
            FlagShip = GetNewFlagShip();
        }  
        RecalculateStats();
    }

    public void RemoveStation(StationData data)
    {
        Stations.Remove(data);
        data.SetFleet(null);
    }

    public void RemoveAllShips()
    {
        Ships.Clear();
        FlagShip = null;
        ClearStats();
    }

    public void RecalculateStats()
    {
        ClearStats();
        foreach (ShipData data in Ships)
        {
			if(!data.Destroyed && !data.Retreated)
            	AddShipStats(data);
        }
    }

    void ClearStats()
    {
		CommandPoints = 0;
        SpeedFTL = 0;
        ShipDefenceBonus = 0;
        ShipDamageBonus = 0;
		Transport = 0;
    }

    ShipData GetNewFlagShip()
    {
        if (Ships.Count > 0)
        {
            return Ships[0];
        }
        else
            return null;
    }

    public void SetFlagShip(ShipData shipdata)
    {
        FlagShip = shipdata;
    }

	public float GetFleetDefenseBonus()
	{
		return ShipDefenceBonus;
	}

	public float GetFleetDamageBonus()
	{
		return ShipDamageBonus;
	}

	public float GetTransport()
	{
		return Transport;
	}

    public int GetCommand()
    {
        return CommandPoints;
    }

	public bool EnoughAmmoInFleet(float amount)
	{
		float ammo = 0;
		foreach (ShipData ship in Ships) 
		{
			if (!ship.Destroyed) {
				ammo += ship.ammo;
				if (ammo >= amount)
					return true;
			}
		}
		return false;
	}

	public bool CanTransportAmmo(float amount, float localAmount)
	{
		if (Transport < amount - localAmount)
			return false;
		else if (!EnoughAmmoInFleet(amount))
			return false;
		else
			return true;
	}

	public void ConsumeFleetAmmo(UnitData ship, float amount)
	{
		float transportedAmount = ship.ammo;
		ship.ammo = 0;
		foreach (ShipData otherShip in Ships) 
		{
			if (otherShip == ship || otherShip.Destroyed)
				continue;

			if (transportedAmount + otherShip.ammo < amount) 
			{
				transportedAmount += otherShip.ammo;
				otherShip.ammo = 0;
			}
			else 
			{
				float dif = amount - transportedAmount;
				otherShip.ammo -= dif;
				return;
			}
		}
	}

    public List<ShipData> GetShips()
    {
        return Ships;
    }

    public List<StationData> GetStations()
    {
        return Stations;
    }

    public float GetSpeedFTL()
    {
        return 0.1f;
    }
}
