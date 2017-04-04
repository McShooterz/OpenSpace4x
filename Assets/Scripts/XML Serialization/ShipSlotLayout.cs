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

public class ShipSlotLayout
{
    public List<Rect> ForeSlots { get; set; }
    public List<Rect> AftSlots { get; set; }
    public List<Rect> PortSlots { get; set; }
    public List<Rect> StarboardSlots { get; set; }
    public List<Rect> CenterSlots { get; set; }

    public ShipSlotLayout()
    {
        ForeSlots = new List<Rect>();
        AftSlots = new List<Rect>();
        PortSlots = new List<Rect>();
        StarboardSlots = new List<Rect>();
        CenterSlots = new List<Rect>();
    }

    public ShipSlotLayout(ShipSlotLayout slotLayout)
    {
        foreach(Rect slot in slotLayout.ForeSlots)
        {
            ForeSlots.Add(new Rect(slot));
        }
        foreach (Rect slot in slotLayout.AftSlots)
        {
            AftSlots.Add(new Rect(slot));
        }
        foreach (Rect slot in slotLayout.PortSlots)
        {
            PortSlots.Add(new Rect(slot));
        }
        foreach (Rect slot in slotLayout.StarboardSlots)
        {
            StarboardSlots.Add(new Rect(slot));
        }
        foreach (Rect slot in slotLayout.CenterSlots)
        {
            CenterSlots.Add(new Rect(slot));
        }
    }

    public Rect GetForeOuterRect()
    {
        return GetOuterRect(ForeSlots);
    }

    public Rect GetAftOuterRect()
    {
        return GetOuterRect(AftSlots);
    }

    public Rect GetPortOuterRect()
    {
        return GetOuterRect(PortSlots);
    }

    public Rect GetStarboardOuterRect()
    {
        return GetOuterRect(StarboardSlots);
    }

    public Rect GetCenterOuterRect()
    {
        return GetOuterRect(CenterSlots);
    }

    Rect GetOuterRect(List<Rect> slotList)
    {
        if (slotList.Count > 0)
        {
            float MaxX = slotList[0].xMax;
            float MinX = slotList[0].xMin;
            float MaxY = slotList[0].yMax;
            float MinY = slotList[0].yMin;

            foreach (Rect slot in slotList)
            {
                if (slot.xMin < MinX)
                {
                    MinX = slot.xMin;
                }
                else if (slot.xMax > MaxX)
                {
                    MaxX = slot.xMax;
                }
                if (slot.yMin < MinY)
                {
                    MinY = slot.yMin;
                }
                else if (slot.yMax > MaxY)
                {
                    MaxY = slot.yMax;
                }
            }
            return Rect.MinMaxRect(MinX, MinY, MaxX, MaxY);
        }
        return new Rect();
    }
}
