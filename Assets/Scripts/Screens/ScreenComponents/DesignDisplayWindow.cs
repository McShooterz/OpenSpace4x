/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections.Generic;

public class DesignDisplayWindow
{
    Rect baseRect;
    bool Open = false;

    Rect TitleRect;
    Rect CloseButtonRect;

    Texture2D SlotTexture;

    List<Rect> SlotRects = new List<Rect>();
    List<SlotedModule> SlotedModules = new List<SlotedModule>();

    string DesignName;

    public DesignDisplayWindow(Rect rect)
    {
        baseRect = rect;
        //SlotTexture = ResourceManager.instance.GetUITexture("ShipSlot");

        CloseButtonRect = new Rect(baseRect.x + (baseRect.width - GameManager.instance.StandardButtonSize.x) / 2f, baseRect.yMax - GameManager.instance.StandardButtonSize.y, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);
        TitleRect = new Rect(CloseButtonRect.x, baseRect.y, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);
    }

    public void Draw()
    {
        GUI.Box(baseRect, "");

        foreach(Rect slot in SlotRects)
        {
            GUI.DrawTexture(slot, SlotTexture);
        }
        foreach(SlotedModule module in SlotedModules)
        {
            module.Draw();
        }

        GUI.Label(TitleRect, DesignName, GameManager.instance.largeLabelStyle);

        if(GUI.Button(CloseButtonRect, "Close"))
        {
            AudioManager.instance.PlayUIClip("MainButtonClick");
            Open = false;
        }
    }

    public void SetDesign(ShipDesign design)
    {
        Clear();

        List<Rect> ForeSlots = new List<Rect>();
        List<Rect> AftSlots = new List<Rect>();
        List<Rect> PortSlots = new List<Rect>();
        List<Rect> StarboardSlots = new List<Rect>();
        List<Rect> CenterSlots = new List<Rect>();

        DesignName = design.Name;

        ShipSlotLayout shipLayout = ResourceManager.instance.GetShipHull(design.Hull).GetSlotLayout();
        if(shipLayout != null)
        {
            Vector2 ForePosition, AftPosition, PortPosition, StarboardPosition, CenterPosition;
            Vector2 ForeSize, AftSize, PortSize, StarboardSize, CenterSize;
            float MinX, MaxX, MinY, MaxY, moduleScale = 16f;
            Vector2 ForeMin, AftMin, PortMin, StarboardMin, CenterMin;

            //Get size of center quadrant
            if (shipLayout.CenterSlots.Count > 0)
            {
                Rect CenterOuterRect = shipLayout.GetCenterOuterRect();
                CenterSize = new Vector2(CenterOuterRect.width / 16, CenterOuterRect.height / 16);
                CenterMin = CenterOuterRect.position;
            }
            else
            {
                CenterSize = Vector2.zero;
                CenterMin = Vector2.zero;
            }

            //Get size of port quadrant
            if (shipLayout.PortSlots.Count > 0)
            {
                Rect PortOuterRect = shipLayout.GetPortOuterRect();

                PortSize = new Vector2(PortOuterRect.width / 16, PortOuterRect.height / 16);
                PortMin = PortOuterRect.position;
            }
            else
            {
                PortSize = Vector2.zero;
                PortMin = Vector2.zero;
            }

            //Get size of starboard quadrant
            if (shipLayout.StarboardSlots.Count > 0)
            {
                Rect StarboardOuterRect = shipLayout.GetStarboardOuterRect();

                StarboardSize = new Vector2(StarboardOuterRect.width / 16, StarboardOuterRect.height / 16);
                StarboardMin = StarboardOuterRect.position;
            }
            else
            {
                StarboardSize = Vector2.zero;
                StarboardMin = Vector2.zero;
            }

            //Get size of fore quadrant
            if (shipLayout.ForeSlots.Count > 0)
            {
                Rect ForeOuterRect = shipLayout.GetForeOuterRect();

                ForeSize = new Vector2(ForeOuterRect.width / 16, ForeOuterRect.height / 16);
                ForeMin = ForeOuterRect.position;
            }
            else
            {
                ForeSize = Vector2.zero;
                ForeMin = Vector2.zero;
            }

            //Get size of aft quadrant
            if (shipLayout.AftSlots.Count > 0)
            {
                Rect AftOuterRect = shipLayout.GetAftOuterRect();

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
            if (MaxSlotsVertical * moduleScale > baseRect.height * 0.9f)
            {
                moduleScale = baseRect.height * 0.9f / MaxSlotsVertical;
            }
            if (MaxSlotsHorizontal * moduleScale > baseRect.width * 0.9f)
            {
                moduleScale = baseRect.width * 0.9f / MaxSlotsHorizontal;
            }

            //Apply scale
            ForeSize *= moduleScale;
            AftSize *= moduleScale;
            PortSize *= moduleScale;
            StarboardSize *= moduleScale;
            CenterSize *= moduleScale;
            MaxSlotsVertical *= moduleScale;
            MaxSlotsHorizontal *= moduleScale;

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
            foreach (Rect slot in shipLayout.CenterSlots)
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
            float StarboardMinX = Screen.width;
            float StarboardMaxX = 0;
            float StarboardMinY = Screen.height;
            float StarboardMaxY = 0;

            foreach (Rect slot in shipLayout.StarboardSlots)
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
            float PortMinX = Screen.width;
            float PortMaxX = 0;
            float PortMinY = Screen.height;
            float PortMaxY = 0;

            foreach (Rect slot in shipLayout.PortSlots)
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
            float ForeMinX = Screen.width;
            float ForeMaxX = 0;
            float ForeMinY = Screen.height;
            float ForeMaxY = 0;

            foreach (Rect slot in shipLayout.ForeSlots)
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
            float AftMinX = Screen.width;
            float AftMaxX = 0;
            float AftMinY = Screen.height;
            float AftMaxY = 0;

            foreach (Rect slot in shipLayout.AftSlots)
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

            //Add all slots to same list
            foreach(Rect slot in ForeSlots)
            {
                SlotRects.Add(slot);
            }
            foreach (Rect slot in AftSlots)
            {
                SlotRects.Add(slot);
            }
            foreach (Rect slot in PortSlots)
            {
                SlotRects.Add(slot);
            }
            foreach (Rect slot in StarboardSlots)
            {
                SlotRects.Add(slot);
            }
            foreach (Rect slot in CenterSlots)
            {
                SlotRects.Add(slot);
            }

            //Add sloted modules
            foreach (DesignModule designMod in design.ForeModules)
            {
                if (designMod.Position < ForeSlots.Count)
                {
                    SlotedModule slotedModule = SlotedModule.CreateSlotedModule(designMod, ForeSlots[designMod.Position]);
                    if (slotedModule != null)
                    {
                        SlotedModules.Add(slotedModule);
                    }
                }
            }
            foreach (DesignModule designMod in design.AftModules)
            {
                if (designMod.Position < AftSlots.Count)
                {
                    SlotedModule slotedModule = SlotedModule.CreateSlotedModule(designMod, AftSlots[designMod.Position]);
                    if (slotedModule != null)
                    {
                        SlotedModules.Add(slotedModule);
                    }
                }
            }
            foreach (DesignModule designMod in design.PortModules)
            {
                if (designMod.Position < PortSlots.Count)
                {
                    SlotedModule slotedModule = SlotedModule.CreateSlotedModule(designMod, PortSlots[designMod.Position]);
                    if (slotedModule != null)
                    {
                        SlotedModules.Add(slotedModule);
                    }
                }
            }
            foreach (DesignModule designMod in design.StarboardModules)
            {
                if (designMod.Position < StarboardSlots.Count)
                {
                    SlotedModule slotedModule = SlotedModule.CreateSlotedModule(designMod, StarboardSlots[designMod.Position]);
                    if (slotedModule != null)
                    {
                        SlotedModules.Add(slotedModule);
                    }
                }
            }
            foreach (DesignModule designMod in design.CenterModules)
            {
                if (designMod.Position < CenterSlots.Count)
                {
                    SlotedModule slotedModule = SlotedModule.CreateSlotedModule(designMod, CenterSlots[designMod.Position]);
                    if (slotedModule != null)
                    {
                        SlotedModules.Add(slotedModule);
                    }
                }
            }
        }
    }

    public void SetDesign(StationDesign design)
    {
        Clear();

    }

    public bool isOpen()
    {
        return Open;
    }

    public void SetOpen(bool state)
    {
        Open = state;
    }

    public bool Contains(Vector2 point)
    {
        return baseRect.Contains(point);
    }

    public void Clear()
    {
        SlotRects.Clear();
        SlotedModules.Clear();
        DesignName = "";
    }
}
