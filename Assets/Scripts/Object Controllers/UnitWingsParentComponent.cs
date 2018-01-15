/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections.Generic;

public class UnitWingsParentComponent
{
    SpaceUnit Parent;
    UnitData ParentData;

    //Fighters
    List<FighterWing> FighterWings = new List<FighterWing>();
    List<FighterWing> HeavyFighterWings = new List<FighterWing>();
    List<FighterWing> AssaultWings = new List<FighterWing>();

    public UnitWingsParentComponent(SpaceUnit parentUnit, UnitData parentUnitData)
    {
        Parent = parentUnit;
        ParentData = parentUnitData;
    }

    public void LaunchFighters()
    {
        LaunchUnits(ParentData.Fighters, FighterWings);
    }

    public void LaunchHeavyFighters()
    {
        LaunchUnits(ParentData.HeavyFighters, HeavyFighterWings);
    }

    public void LaunchAssaultPods()
    {
        LaunchUnits(ParentData.AssaultPods, AssaultWings);
    }

    void LaunchUnits(List<UnitData.FighterComplement> Complements, List<FighterWing> WingsContainer)
    {
        ShipManager shipManager = Parent.GetShipManager();
        foreach (UnitData.FighterComplement complement in Complements)
        {
            for (int i = 0; i < complement.GetCount(); i++)
            {
                int troopCount = 0;
                    if (ParentData.RequestCrew(complement.GetFighterType().Crew) > 0)
                        troopCount = ParentData.RequestTroops(complement.GetFighterType().Troops);
                    else
                        break;

                Fighter newUnit = ResourceManager.instance.CreateCombatFighter(complement.GetFighterType(), Parent.transform.position, Quaternion.Euler(-Parent.transform.forward));
                newUnit.SetTroops(troopCount);
                if (newUnit != null)
                {
                    newUnit.SetNewGoalPosition(Parent.transform.position + -Parent.transform.forward * 2f);
                    if (WingsContainer.Count == 0 || WingsContainer[WingsContainer.Count - 1].atCapacity() || WingsContainer[WingsContainer.Count - 1].GetFirstFighter().GetName() != newUnit.GetName())
                    {
                        FighterWing unitWing = ResourceManager.CreateFighterWing();
                        unitWing.SetParent(Parent, shipManager);
                        unitWing.addFighter(newUnit);
                        WingsContainer.Add(unitWing);
                    }
                    else
                    {
                        WingsContainer[WingsContainer.Count - 1].addFighter(newUnit);
                    }
                    shipManager.AddSpaceUnit(newUnit);
                }
            }
            complement.SetCount(0);
        }
    }

    public void RecallFighters()
    {
        foreach (FighterWing wing in FighterWings)
        {
            wing.Recall();
        }
    }

    public void RecallHeavyFighters()
    {
        foreach (FighterWing wing in HeavyFighterWings)
        {
            wing.Recall();
        }
    }

    public void RecallAssaultPods()
    {
        foreach(FighterWing wing in AssaultWings)
        {
            wing.Recall();
        }
    }

    public void DockFighter(Fighter fighter)
    {
        bool found = false;
        FighterWing parentWing = null;
        foreach (FighterWing wing in FighterWings)
        {
            if (found)
                break;
            if (wing.OwnsUnit(fighter))
            {
                ParentData.ReturnFighter(fighter);
                parentWing = wing;
                found = true;
                break;
            }
        }
        foreach (FighterWing wing in HeavyFighterWings)
        {
            if (found)
                break;
            if (wing.OwnsUnit(fighter))
            {
                ParentData.ReturnHeavyFighter(fighter);
                parentWing = wing;
                found = true;
                break;
            }
        }
        foreach (FighterWing wing in AssaultWings)
        {
            if (found)
                break;
            if (wing.OwnsUnit(fighter))
            {
                ParentData.ReturnAssaultPod(fighter);
                parentWing = wing;
                found = true;
                break;
            }
        }
        if (parentWing != null)
            parentWing.RemoveUnit(fighter);
        fighter.DeleteSelf();
    }

    public void RemoveFighterWing(FighterWing wing)
    {
        FighterWings.Remove(wing);
        HeavyFighterWings.Remove(wing);
        AssaultWings.Remove(wing);
    }

    public void UnParentFighterWings()
    {
        foreach (FighterWing wing in FighterWings)
        {
            wing.UnParent();
        }
        FighterWings.Clear();
        foreach (FighterWing wing in HeavyFighterWings)
        {
            wing.UnParent();
        }
        HeavyFighterWings.Clear();
        foreach (FighterWing wing in AssaultWings)
        {
            wing.UnParent();
        }
        AssaultWings.Clear();
    }

    public void DestroyAllFighters()
    {
        foreach (FighterWing wing in FighterWings)
        {
            wing.DestroyUnits();
            Parent.GetShipManager().RemoveFighterWing(wing);
            Object.Destroy(wing.gameObject);
        }
        foreach (FighterWing wing in HeavyFighterWings)
        {
            wing.DestroyUnits();
            Parent.GetShipManager().RemoveFighterWing(wing);
            Object.Destroy(wing.gameObject);
        }
        foreach(FighterWing wing in AssaultWings)
        {
            wing.DestroyUnits();
            Parent.GetShipManager().RemoveFighterWing(wing);
            Object.Destroy(wing.gameObject);
        }
        FighterWings.Clear();
        HeavyFighterWings.Clear();
        AssaultWings.Clear();
    }

    public bool hasLaunchedFighters()
    {
        return FighterWings.Count > 0;
    }

    public bool hasLaunchedHeavyFighters()
    {
        return HeavyFighterWings.Count > 0;
    }

    public bool hasLaunchedAssaultPods()
    {
        return AssaultWings.Count > 0;
    }
}
