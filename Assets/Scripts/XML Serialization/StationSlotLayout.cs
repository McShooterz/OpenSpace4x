/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections.Generic;

public class StationSlotLayout
{
    public List<Rect> SlotList { get; set; }

    public StationSlotLayout()
    {
        SlotList = new List<Rect>();
    }
}
