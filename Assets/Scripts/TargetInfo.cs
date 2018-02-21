/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class TargetInfo
{
    public SpaceUnit Target;
    public Vector2 Direction;
    public float RangeSqr;    

    public TargetInfo(SpaceUnit target, Vector2 direction, float rangeSqr)
    {
        Target = target;       
        Direction = direction;
        RangeSqr = rangeSqr;
    }
}
