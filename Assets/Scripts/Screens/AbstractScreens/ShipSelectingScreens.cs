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

    }

    protected void BuildStationDesignDataList(StationHullData hullData)
    {

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
