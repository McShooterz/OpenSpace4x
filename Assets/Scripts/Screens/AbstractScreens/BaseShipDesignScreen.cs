/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseShipDesignScreen : BaseSpaceUnitDesignScreen
{
#region Variables

    [SerializeField]
    protected ShipHullPanel shipHullPanel;

    protected ShipHullData selectedShipHull;

#endregion

    //Slots
    protected List<Rect> ForeSlots = new List<Rect>();
    protected List<Rect> AftSlots = new List<Rect>();
    protected List<Rect> PortSlots = new List<Rect>();
    protected List<Rect> StarboardSlots = new List<Rect>();
    protected List<Rect> CenterSlots = new List<Rect>();
    protected Rect ForeSlotsAreaRect;
    protected Rect AftSlotsAreaRect;
    protected Rect PortSlotsAreaRect;
    protected Rect StarboardSlotsAreaRect;
    protected Rect CenterSlotsAreaRect;

    //Modules added to current ship
    protected List<SlotedModule> ForeSlotedModules = new List<SlotedModule>();
    protected List<SlotedModule> AftSlotedModules = new List<SlotedModule>();
    protected List<SlotedModule> PortSlotedModules = new List<SlotedModule>();
    protected List<SlotedModule> StarboardSlotedModules = new List<SlotedModule>();
    protected List<SlotedModule> CenterSlotedModules = new List<SlotedModule>();
    protected List<SlotedModule> ModulesToRemove = new List<SlotedModule>();

    // Use this for initialization
    protected override void Start ()
    {
        base.Start();
	}
	
	// Update is called once per frame
	protected override void Update ()
    {
        base.Update();
	}

    protected virtual void OnGUI()
    {
        DrawSlots();

        DrawPlacedModules();

        ModulePanel.DrawSelectedModuleTexture(mousePosition);

        if(ModulePanel.GetUseMirrorMode())
        {
            ModulePanel.DrawSelectedModuleTexture(mirrorMousePosition);
        }
    }

    protected override void BuildModuleSetLists()
    {

    }

    protected virtual void BuildShipHullDatas()
    {

    }

    public virtual void BuildShipDesigns(ShipHullData hullData)
    {

    }

    protected virtual void ChangeHull(ShipHullData data)
    {
        selectedShipHull = data;

        if (selectedShipHull != null)
        {
            BuildShipDesigns(selectedShipHull);

            ShipSlotLayout slotLayout = ResourceManager.instance.GetShipSlotLayout(selectedShipHull.Name);

            if (slotLayout != null)
            {
                FormatSlotLayout(slotLayout);
            }
        }
    }

    protected virtual void LoadShipDesign(ShipDesign design)
    {

    }

    protected virtual void DeleteShipDesign(ShipDesign design)
    {
        design.Deleted = true;

        if(selectedShipHull != null)
        {
            BuildShipDesigns(selectedShipHull);
        }
    }

#region GUI functions

    protected void DrawSlots()
    {
        if (selectedShipHull != null)
        {
            Module selectedModule = GetSelectedModule();
            ModuleSet selectedModuleSet = GetSelectedModuleSet();

            if (selectedModule != null)
            {
                if (CheckHullModuleAllow(selectedShipHull.ModuleLimitFore, selectedModuleSet.ModuleCategory))
                {
                    GUI.color = Color.white;
                }
                else
                {
                    GUI.color = Color.red;
                }
            }
            foreach (Rect slot in ForeSlots)
            {
                GUI.DrawTexture(slot, SlotTexture);
            }
            if (selectedModule != null)
            {
                if (CheckHullModuleAllow(selectedShipHull.ModuleLimitAft, selectedModuleSet.ModuleCategory))
                {
                    GUI.color = Color.white;
                }
                else
                {
                    GUI.color = Color.red;
                }
            }
            foreach (Rect slot in AftSlots)
            {
                GUI.DrawTexture(slot, SlotTexture);
            }
            if (selectedModule != null)
            {
                if (CheckHullModuleAllow(selectedShipHull.ModuleLimitPort, selectedModuleSet.ModuleCategory))
                {
                    GUI.color = Color.white;
                }
                else
                {
                    GUI.color = Color.red;
                }
            }
            foreach (Rect slot in PortSlots)
            {
                GUI.DrawTexture(slot, SlotTexture);
            }
            if (selectedModule != null)
            {
                if (CheckHullModuleAllow(selectedShipHull.ModuleLimitStarboard, selectedModuleSet.ModuleCategory))
                {
                    GUI.color = Color.white;
                }
                else
                {
                    GUI.color = Color.red;
                }
            }
            foreach (Rect slot in StarboardSlots)
            {
                GUI.DrawTexture(slot, SlotTexture);
            }
            if (selectedModule != null)
            {
                if (CheckHullModuleAllow(selectedShipHull.ModuleLimitCenter, selectedModuleSet.ModuleCategory))
                {
                    GUI.color = Color.white;
                }
                else
                {
                    GUI.color = Color.red;
                }
            }
            foreach (Rect slot in CenterSlots)
            {
                GUI.DrawTexture(slot, SlotTexture);
            }
            GUI.color = Color.white;
        }
    }

    protected void DrawPlacedModules()
    {
        foreach (SlotedModule module in ForeSlotedModules)
        {
            module.Draw();
        }
        foreach (SlotedModule module in AftSlotedModules)
        {
            module.Draw();
        }
        foreach (SlotedModule module in PortSlotedModules)
        {
            module.Draw();
        }
        foreach (SlotedModule module in StarboardSlotedModules)
        {
            module.Draw();
        }
        foreach (SlotedModule module in CenterSlotedModules)
        {
            module.Draw();
        }
    }

#endregion

    protected void FormatSlotLayout(ShipSlotLayout sourceLayout)
    {
        float moduleScale = ModulePanel.GetModuleScale();

        ClearFormatedSlots();
        ClearSlottedModules();

        Vector2 ForePosition, AftPosition, PortPosition, StarboardPosition, CenterPosition;
        Vector2 ForeSize, AftSize, PortSize, StarboardSize, CenterSize;
        float MinX, MaxX, MinY, MaxY;
        Vector2 ForeMin, AftMin, PortMin, StarboardMin, CenterMin;

        //Get size of center quadrant
        if (sourceLayout.CenterSlots.Count > 0)
        {
            Rect CenterOuterRect = sourceLayout.GetCenterOuterRect();
            CenterSize = new Vector2(CenterOuterRect.width / 16, CenterOuterRect.height / 16);
            CenterMin = CenterOuterRect.position;
        }
        else
        {
            CenterSize = Vector2.zero;
            CenterMin = Vector2.zero;
        }

        //Get size of port quadrant
        if (sourceLayout.PortSlots.Count > 0)
        {
            Rect PortOuterRect = sourceLayout.GetPortOuterRect();

            PortSize = new Vector2(PortOuterRect.width / 16, PortOuterRect.height / 16);
            PortMin = PortOuterRect.position;
        }
        else
        {
            PortSize = Vector2.zero;
            PortMin = Vector2.zero;
        }

        //Get size of starboard quadrant
        if (sourceLayout.StarboardSlots.Count > 0)
        {
            Rect StarboardOuterRect = sourceLayout.GetStarboardOuterRect();

            StarboardSize = new Vector2(StarboardOuterRect.width / 16, StarboardOuterRect.height / 16);
            StarboardMin = StarboardOuterRect.position;
        }
        else
        {
            StarboardSize = Vector2.zero;
            StarboardMin = Vector2.zero;
        }

        //Get size of fore quadrant
        if (sourceLayout.ForeSlots.Count > 0)
        {
            Rect ForeOuterRect = sourceLayout.GetForeOuterRect();

            ForeSize = new Vector2(ForeOuterRect.width / 16, ForeOuterRect.height / 16);
            ForeMin = ForeOuterRect.position;
        }
        else
        {
            ForeSize = Vector2.zero;
            ForeMin = Vector2.zero;
        }

        //Get size of aft quadrant
        if (sourceLayout.AftSlots.Count > 0)
        {
            Rect AftOuterRect = sourceLayout.GetAftOuterRect();

            AftSize = new Vector2(AftOuterRect.width / 16, AftOuterRect.height / 16);
            AftMin = AftOuterRect.position;
        }
        else
        {
            AftSize = Vector2.zero;
            AftMin = Vector2.zero;
        }

        //Determine scale of modules
        float MaxSlotsVertical = 1 + ForeSize.y + AftSize.y + Mathf.Max(CenterSize.y, PortSize.y, StarboardSize.y);
        float MaxSlotsHorizontal = Mathf.Max(ForeSize.x, AftSize.x, 1 + CenterSize.x + PortSize.x + StarboardSize.x);
        if (MaxSlotsVertical * moduleScale > Screen.height * 0.9f)
        {
            moduleScale = Screen.height * 0.9f / MaxSlotsVertical;
        }
        if (MaxSlotsHorizontal * moduleScale > Screen.width * 0.45f)
        {
            moduleScale = Screen.width * 0.45f / MaxSlotsHorizontal;
        }

        //Apply scale
        ForeSize *= moduleScale;
        AftSize *= moduleScale;
        PortSize *= moduleScale;
        StarboardSize *= moduleScale;
        CenterSize *= moduleScale;
        MaxSlotsVertical *= moduleScale;
        MaxSlotsHorizontal *= moduleScale;
        if (GetSelectedModule() != null)
        {
            SelectedModuleRect = new Rect(0, 0, GetSelectedModule().SizeX * moduleScale, GetSelectedModule().SizeY * moduleScale);
        }

        //Determine the center of the layout
        //float centerY = (Screen.height - MaxSlotsVertical) / 2 + ForeSize.y + (MaxSlotsVertical - ForeSize.y - AftSize.y) / 2;
        //float centerX = Screen.width * 0.25f + (Screen.width - MaxSlotsHorizontal) / 2 + PortSize.x + moduleScale;
        float centerY = (Screen.height - MaxSlotsVertical) / 2 + ForeSize.y + moduleScale / 2 + (Mathf.Max(CenterSize.y, PortSize.y, StarboardSize.y) - CenterSize.y) / 2;
        float centerX = Screen.width * 0.25f + (Screen.width * 0.5f - MaxSlotsHorizontal) / 2 + PortSize.x + moduleScale / 2;

        CenterPosition = new Vector2(centerX, centerY);
        StarboardPosition = new Vector2(CenterPosition.x + CenterSize.x + moduleScale / 2, CenterPosition.y + CenterSize.y / 2 - StarboardSize.y / 2);
        PortPosition = new Vector2(CenterPosition.x - PortSize.x - moduleScale / 2, CenterPosition.y + CenterSize.y / 2 - PortSize.y / 2);
        ForePosition = new Vector2(CenterPosition.x + CenterSize.x / 2 - ForeSize.x / 2, Mathf.Min(CenterPosition.y, StarboardPosition.y, PortPosition.y) - moduleScale / 2 - ForeSize.y);
        AftPosition = new Vector2(CenterPosition.x + CenterSize.x / 2 - AftSize.x / 2, Mathf.Max(CenterPosition.y + CenterSize.y, StarboardPosition.y + StarboardSize.y, PortPosition.y + PortSize.y) + moduleScale / 2);

        MinX = Screen.width;
        MaxX = 0;
        MinY = Screen.height;
        MaxY = 0;

        float CenterMinX = Screen.width;
        float CenterMaxX = 0;
        float CenterMinY = Screen.height;
        float CenterMaxY = 0;

        //Build new slots
        foreach (Rect slot in sourceLayout.CenterSlots)
        {
            Rect newSlot = new Rect((slot.x - CenterMin.x) / 16f * moduleScale + CenterPosition.x, (slot.y - CenterMin.y) / 16f * moduleScale + CenterPosition.y, moduleScale, moduleScale);
            if (newSlot.x < MinX)
                MinX = newSlot.x;
            if (newSlot.xMax > MaxX)
                MaxX = newSlot.xMax;
            if (newSlot.y < MinY)
                MinY = newSlot.y;
            if (newSlot.yMax > MaxY)
                MaxY = newSlot.yMax;

            if (newSlot.x < CenterMinX)
                CenterMinX = newSlot.x;
            if (newSlot.xMax > CenterMaxX)
                CenterMaxX = newSlot.xMax;
            if (newSlot.y < CenterMinY)
                CenterMinY = newSlot.y;
            if (newSlot.yMax > CenterMaxY)
                CenterMaxY = newSlot.yMax;
            CenterSlots.Add(newSlot);
        }
        CenterSlotsAreaRect = new Rect(CenterMinX, CenterMinY, CenterMaxX - CenterMinX, CenterMaxY - CenterMinY);
        float StarboardMinX = Screen.width;
        float StarboardMaxX = 0;
        float StarboardMinY = Screen.height;
        float StarboardMaxY = 0;

        foreach (Rect slot in sourceLayout.StarboardSlots)
        {
            Rect newSlot = new Rect((slot.x - StarboardMin.x) / 16f * moduleScale + StarboardPosition.x, (slot.y - StarboardMin.y) / 16f * moduleScale + StarboardPosition.y, moduleScale, moduleScale);
            if (newSlot.x < MinX)
                MinX = newSlot.x;
            if (newSlot.xMax > MaxX)
                MaxX = newSlot.xMax;
            if (newSlot.y < MinY)
                MinY = newSlot.y;
            if (newSlot.yMax > MaxY)
                MaxY = newSlot.yMax;

            if (newSlot.x < StarboardMinX)
                StarboardMinX = newSlot.x;
            if (newSlot.xMax > StarboardMaxX)
                StarboardMaxX = newSlot.xMax;
            if (newSlot.y < StarboardMinY)
                StarboardMinY = newSlot.y;
            if (newSlot.yMax > StarboardMaxY)
                StarboardMaxY = newSlot.yMax;
            StarboardSlots.Add(newSlot);
        }
        StarboardSlotsAreaRect = new Rect(StarboardMinX, StarboardMinY, StarboardMaxX - StarboardMinX, StarboardMaxY - StarboardMinY);
        float PortMinX = Screen.width;
        float PortMaxX = 0;
        float PortMinY = Screen.height;
        float PortMaxY = 0;

        foreach (Rect slot in sourceLayout.PortSlots)
        {
            Rect newSlot = new Rect((slot.x - PortMin.x) / 16f * moduleScale + PortPosition.x, (slot.y - PortMin.y) / 16f * moduleScale + PortPosition.y, moduleScale, moduleScale);
            if (newSlot.x < MinX)
                MinX = newSlot.x;
            if (newSlot.xMax > MaxX)
                MaxX = newSlot.xMax;
            if (newSlot.y < MinY)
                MinY = newSlot.y;
            if (newSlot.yMax > MaxY)
                MaxY = newSlot.yMax;

            if (newSlot.x < PortMinX)
                PortMinX = newSlot.x;
            if (newSlot.xMax > PortMaxX)
                PortMaxX = newSlot.xMax;
            if (newSlot.y < PortMinY)
                PortMinY = newSlot.y;
            if (newSlot.yMax > PortMaxY)
                PortMaxY = newSlot.yMax;
            PortSlots.Add(newSlot);
        }
        PortSlotsAreaRect = new Rect(PortMinX, PortMinY, PortMaxX - PortMinX, PortMaxY - PortMinY);
        float ForeMinX = Screen.width;
        float ForeMaxX = 0;
        float ForeMinY = Screen.height;
        float ForeMaxY = 0;

        foreach (Rect slot in sourceLayout.ForeSlots)
        {
            Rect newSlot = new Rect((slot.x - ForeMin.x) / 16f * moduleScale + ForePosition.x, (slot.y - ForeMin.y) / 16f * moduleScale + ForePosition.y, moduleScale, moduleScale);
            if (newSlot.x < MinX)
                MinX = newSlot.x;
            if (newSlot.xMax > MaxX)
                MaxX = newSlot.xMax;
            if (newSlot.y < MinY)
                MinY = newSlot.y;
            if (newSlot.yMax > MaxY)
                MaxY = newSlot.yMax;

            if (newSlot.x < ForeMinX)
                ForeMinX = newSlot.x;
            if (newSlot.xMax > ForeMaxX)
                ForeMaxX = newSlot.xMax;
            if (newSlot.y < ForeMinY)
                ForeMinY = newSlot.y;
            if (newSlot.yMax > ForeMaxY)
                ForeMaxY = newSlot.yMax;
            ForeSlots.Add(newSlot);
        }
        ForeSlotsAreaRect = new Rect(ForeMinX, ForeMinY, ForeMaxX - ForeMinX, ForeMaxY - ForeMinY);
        float AftMinX = Screen.width;
        float AftMaxX = 0;
        float AftMinY = Screen.height;
        float AftMaxY = 0;

        foreach (Rect slot in sourceLayout.AftSlots)
        {
            Rect newSlot = new Rect((slot.x - AftMin.x) / 16f * moduleScale + AftPosition.x, (slot.y - AftMin.y) / 16f * moduleScale + AftPosition.y, moduleScale, moduleScale);
            if (newSlot.x < MinX)
                MinX = newSlot.x;
            if (newSlot.xMax > MaxX)
                MaxX = newSlot.xMax;
            if (newSlot.y < MinY)
                MinY = newSlot.y;
            if (newSlot.yMax > MaxY)
                MaxY = newSlot.yMax;

            if (newSlot.x < AftMinX)
                AftMinX = newSlot.x;
            if (newSlot.xMax > AftMaxX)
                AftMaxX = newSlot.xMax;
            if (newSlot.y < AftMinY)
                AftMinY = newSlot.y;
            if (newSlot.yMax > AftMaxY)
                AftMaxY = newSlot.yMax;
            AftSlots.Add(newSlot);
        }
        AftSlotsAreaRect = new Rect(AftMinX, AftMinY, AftMaxX - AftMinX, AftMaxY - AftMinY);
        SlotsAreaRect = new Rect(MinX, MinY, MaxX - MinX, MaxY - MinY);

        ModulePanel.SetModuleScale(moduleScale);
    }

    protected void ClearFormatedSlots()
    {
        ForeSlots.Clear();
        AftSlots.Clear();
        PortSlots.Clear();
        StarboardSlots.Clear();
        CenterSlots.Clear();
    }

    protected void ClearSlottedModules()
    {
        ForeSlotedModules.Clear();
        AftSlotedModules.Clear();
        PortSlotedModules.Clear();
        StarboardSlotedModules.Clear();
        CenterSlotedModules.Clear();
    }
}
