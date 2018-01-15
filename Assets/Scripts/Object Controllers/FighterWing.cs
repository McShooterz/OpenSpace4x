/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections.Generic;

public sealed class FighterWing : MonoBehaviour
{
    SpaceUnit parent;
    ShipManager shipManager;

    List<Fighter> Fighters = new List<Fighter>();

    int MaxUnits;

    int HighLightLayer;
    bool ActiveHighlight = false;
    bool HoveredHighlight = false;
    GameObject MiniMapObject;

    SpaceUnit targetUnit;

    // Use this for initialization
    void Start () {}

    // Update is called once per frame
    void Update () 
	{
        if (Fighters.Count == 0)
            DestroySelf();

        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);

        UpdateHighlight();
    }

    public void SetParent(SpaceUnit parentObject, ShipManager SM)
    {
        parent = parentObject;
        SM.AddFighterWing(this);
    }

    public void ToggleHighlight(bool state)
    {
        ActiveHighlight = state;
        foreach (Fighter fighter in Fighters)
        {
            fighter.ToggleHighlight(state);
        }
    }

    public void addFighter(Fighter fighter)
    {
        Fighters.Add(fighter);
        fighter.SetParent(this);
        if (Fighters.Count == 1)
        {
            AttachToObject(fighter.gameObject);
            SetIconTexture(fighter.GetIcon());
            MaxUnits = fighter.GetMaxSquadronSize();
        }
        if (targetUnit != null)
            SetNewTarget(targetUnit, shipManager.isEnemy(targetUnit));
    }

    public void RemoveUnit(Fighter fighter)
    {
        Fighters.Remove(fighter);
        if (Fighters.Count == 0)
        {
            DestroySelf();
        }
        else if (transform.parent == fighter.transform)
        {
            AttachToObject(Fighters[0].gameObject);
        }
    }

    public bool HasShields()
    {
        if (Fighters.Count > 0)
            return Fighters[0].HasShields();
        return false;
    }

    public float GetShieldRatio()
    {
        return Fighters[0].GetShieldRatio();
    }

    public float GetArmorRatio()
    {
        if (Fighters.Count > 0)
            return Fighters[0].GetArmorRatio();
        return 0;
    }

    public Fighter GetFirstFighter()
    {
        return Fighters[0];
    }

    void UpdateHighlight()
    {
        if (ActiveHighlight || HoveredHighlight)
        {
            transform.GetChild(0).gameObject.layer = HighLightLayer;
            HoveredHighlight = false;
        }
        else
        {
            transform.GetChild(0).gameObject.layer = 0;
        }
    }

    public void SetShipManager(ShipManager SM)
    {
        shipManager = SM;
        HighLightLayer = shipManager.GetHighLightLayer();
        AttachMiniMapObject(shipManager.GetHighlightColor());
    }

    public ShipManager GetShipManager()
    {
        return shipManager;
    }

    public SpaceUnit GetParent()
    {
        return parent;
    }

    public void UnParent()
    {
        parent = null;
    }

    public int GetUnitCapacity()
    {
        return MaxUnits;
    }

    void SetIconTexture(Texture2D icon)
    {
        GetComponent<Renderer>().material.SetTexture("_MainTex", icon);
    }

    public void AttachMiniMapObject(Color color)
    {
        MiniMapObject = ResourceManager.instance.CreateMiniMapObject();
        MiniMapObject.transform.position = gameObject.transform.position;
        MiniMapObject.transform.parent = gameObject.transform;
        SetMiniMapObjectColor(shipManager.GetHighlightColor());
    }

    public void SetMiniMapObjectColor(Color color)
    {
        if (MiniMapObject != null)
        {
            MiniMapObject.GetComponent<Renderer>().material.color = color;
        }
    }

    void AttachToObject(GameObject attachTarget)
    {
        Vector3 Position = attachTarget.transform.position;
        Position.y += 0.75f;
        transform.position = Position;
        transform.parent = attachTarget.transform;
    }

    public void DestroySelf()
    {
        DestroyUnits();
        shipManager.RemoveFighterWing(this);
        if (parent != null)
        {
            if (parent is Ship)
            {
                (parent as Ship).RemoveFighterWing(this);
            }
            else if (parent is Station)
            {
                (parent as Station).RemoveFighterWing(this);
            }
        }
        Destroy(gameObject);
    }

    public void DestroyUnits()
    {
        foreach (Fighter unit in Fighters)
        {
            shipManager.RemoveUnit(unit);
            unit.DestroyWeaponEffects();
            Destroy(unit.gameObject);
        }
        Fighters.Clear();
    }

    public bool OwnsUnit(Fighter unit)
    {
        return Fighters.Contains(unit);
    }

    public void ActivateOrdersLine()
    {
        foreach (Fighter unit in Fighters)
        {
            unit.ActivateOrdersLine();
        }
    }

    public void SetNewGoalPosition(Vector3 Position)
    {
        targetUnit = null;
        foreach (Fighter unit in Fighters)
        {
            unit.SetNewGoalPosition(Position);
        }
        ResourceManager.instance.CreateClickPing(Position);
    }

    public void SetNewTarget(SpaceUnit target, bool isEnemy)
    {
        targetUnit = target;
        foreach (Fighter unit in Fighters)
        {
            unit.SetNewTarget(target, isEnemy);
        }
    }

    public void Recall()
    {
        foreach (Fighter unit in Fighters)
        {
            unit.Recall();
        }
    }

    public Texture2D GetIcon()
    {
        if (Fighters.Count > 0)
            return Fighters[0].GetIcon();
        else
            return ResourceManager.instance.GetErrorTexture();
    }

    public float GetHealthRatio()
    {
        if (Fighters.Count > 0)
            return Fighters[0].GetHealthRatio();
        else
            return 0;
    }

    public void CancelOrders()
    {
        foreach (Fighter unit in Fighters)
        {
            unit.CancelOrders();
            unit.SetGoalPosition(transform.position);
        }
    }

    public int GetUnitCount()
    {
        return Fighters.Count;
    }

    public void Hovered()
    {
        HoveredHighlight = true;
        foreach (Fighter unit in Fighters)
        {
            unit.Hovered();
        }
    }

    public bool atCapacity()
    {
        return Fighters.Count == MaxUnits;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
