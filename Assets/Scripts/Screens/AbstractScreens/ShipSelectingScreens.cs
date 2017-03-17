/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections.Generic;

public abstract class ShipSelectingScreens : ScreenParent
{
    protected ShipDesignData selectedDesign;
    protected StationDesignData selectedStationDesign;
    protected FighterDefinition selectedFighter;

    protected ShipHullScrollList shipHullList;
    protected StationHullScrollList stationHullList;
    protected FighterScrollList fighterList;

    protected Rect DesignScrollWindowRect;
    protected Rect DesignScrollViewRect;
    protected Vector2 DesignScrollPostion;

    protected List<DesignDataListEntry> DesignDataList = new List<DesignDataListEntry>();
    protected List<StationDesignDataListEntry> StationDesignDataList = new List<StationDesignDataListEntry>();

    protected void BuildDeignDataList(ShipHullData hullData)
    {
        DesignDataList.Clear();
        DeselectDesignData();

        float EntrySize = DesignScrollWindowRect.height / 5f;
        if (ResourceManager.shipDesigns.ContainsKey(hullData.Name))
        {
            if (ResourceManager.shipDesigns[hullData.Name].Count > 0)
            {
                foreach (KeyValuePair<string, ShipDesign> keyVal in ResourceManager.shipDesigns[hullData.Name])
                {
                    if (!keyVal.Value.Deleted)
                    {
                        Rect rect = new Rect(0, EntrySize * DesignDataList.Count, DesignScrollViewRect.width, EntrySize);
                        ShipDesignData shipDesignData = ResourceManager.GetShipDesignData(keyVal.Value);
                        DesignDataListEntry DLE = new DesignDataListEntry(rect, shipDesignData, LoadDesignData);
                        DesignDataList.Add(DLE);
                    }
                }
            }
        }

        ResizeViewListWindow(ref DesignScrollViewRect, DesignScrollWindowRect, DesignDataList.Count, EntrySize);
    }

    protected void BuildStationDesignDataList(StationHullData hullData)
    {
        StationDesignDataList.Clear();
        DeselectDesignData();

        float EntrySize = DesignScrollWindowRect.height / 5f;
        if (ResourceManager.stationDesigns.ContainsKey(hullData.Name))
        {
            if (ResourceManager.stationDesigns[hullData.Name].Count > 0)
            {
                foreach (KeyValuePair<string, StationDesign> keyVal in ResourceManager.stationDesigns[hullData.Name])
                {
                    if (!keyVal.Value.Deleted)
                    {
                        Rect rect = new Rect(0, EntrySize * StationDesignDataList.Count, DesignScrollViewRect.width, EntrySize);
                        StationDesignData stationDesignData = ResourceManager.GetStationDesignData(keyVal.Value);
                        StationDesignDataListEntry DLE = new StationDesignDataListEntry(rect, stationDesignData, LoadStationDesignData);
                        StationDesignDataList.Add(DLE);
                    }
                }
            }
        }

        ResizeViewListWindow(ref DesignScrollViewRect, DesignScrollWindowRect, DesignDataList.Count, EntrySize);
    }

    protected abstract void ChangeHull(ShipHullData hullData);

    protected abstract void DeselectDesignData();

    protected abstract void LoadDesignData(ShipDesignData designData);

    protected abstract void LoadStationDesignData(StationDesignData designData);

    protected virtual bool CheckHullValid(ShipHullData data)
    {
        return true;
    }

    protected virtual bool CheckStationHullValid(StationHullData data)
    {
        return true;
    }

    protected virtual bool CheckFighterValid(FighterDefinition fighter)
    {
        return true;
    }
}
