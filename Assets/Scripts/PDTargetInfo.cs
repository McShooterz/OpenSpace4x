using UnityEngine;
using System.Collections;

public class PDTargetInfo
{
    public Projectile Target;
    public Vector2 Direction;
    public float RangeSqr;    

    public PDTargetInfo(Projectile target, Vector2 direction, float rangeSqr)
    {
        Target = target;       
        Direction = direction;
        RangeSqr = rangeSqr;
    }
}
