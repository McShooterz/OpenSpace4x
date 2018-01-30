using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;

[System.Serializable]
public class Weapon
{
    public WeaponCategory Category;

    // Visuals
    public string WeaponEffect;
    public string FireSound;
    public string HitEffect;
    public string HitSound;
    public float HitEffectTime;
    public float HitEffectScale;

    public bool isBeam;
    public bool isHoming;
    public bool AlwaysForward;
    public bool SingleFireSound;

    public float AmmoCost;
    public float PowerCost;
    public float BeamPowerCost;
    public float Delay;
    public int SalvoSize;
    public float SalvoDelay;
    public float Arc;
    public float BeamDuration;
    public int projectiles;
    public int Projectiles
    {
        get { return Mathf.Max(1, projectiles); }
        set{ projectiles = Mathf.Max(1, value); }
    }
    public bool MultiHardpoint;
    public float Scale;

    //Projectile properties
    public float ProjectileLife;
    public float ProjectileSpeed;
    public float ProjectileTurnRate;
    public float Spread;

    //Attributes
    public bool DamageAllQuads;
    public int HomingAdditionalTargets;
    public float ShieldDamageModifier;
    public float ArmorDamageModifier;
    public float HealthDamageModifier;
    public float FighterDamageModifier;
    public float ProjectileDamageModifier;
    public float PowerDamageModifier;
    public float IgnoreShieldChance;
    public float IgnoreArmorChance;
    public float IgnoreArmorRatingChance;

    //Missile defense
    public bool PDTargetable;
    public float ProjectileHealth;
    public bool Jammable;
    public bool dieOnProjectileCollision;
    public bool PointDefense;
    public bool PointDefenseOnly;

    public List<DamageNode> DamageGraph;

    //SecondaryWeapon
    public string SecondaryWeapon;

    float halfArc = -1;
    float maxRangeSqr = -1;

    //Default constructor for xml serialization and setting default values
    public Weapon()
    {

    }

    public void ApplyFiringRangeFactor()
    {
        foreach(DamageNode node in DamageGraph)
        {
            node.Range *= ResourceManager.instance.GetGameConstants().FiringRangeFactor;
        }
    }

    public void SetProjectileLife()
    {
        if(!isBeam && ProjectileLife == 0 && ProjectileSpeed != 0)
        {
            ProjectileLife = GetMaxRange() / ProjectileSpeed * 1.05f;
        }
    }

    public float GetMaxRange()
    {
        return DamageGraph[DamageGraph.Count - 1].Range;
    }

    public float GetMaxRangeDisplay()
    {
        return GetMaxRange() * (1f / ResourceManager.instance.GetGameConstants().FiringRangeFactor);
    }

    public float GetMaxDamage()
    {
        float MaxDamage = 0;
        foreach(DamageNode node in DamageGraph)
        {
            if (node.Damage > MaxDamage)
                MaxDamage = node.Damage;
        }
        return MaxDamage;
    }

    public float CalculateDamage(float rangeSquared)
    {
        if (DamageGraph.Count == 1)
        {
            return DamageGraph[0].Damage;
        }

        if(rangeSquared < DamageGraph[0].GetRangedSquared())
        {
            return DamageGraph[0].Damage;
        }
        
        for (int i = 1; i < DamageGraph.Count; i++)
        {
            float UpperRangeBounds = DamageGraph[i].GetRangedSquared();

            if(rangeSquared < UpperRangeBounds)
            {
                float LowerBounds = DamageGraph[i - 1].GetRangedSquared();
                float RangeDif = UpperRangeBounds - LowerBounds;
                float DamageDif = DamageGraph[i].Damage - DamageGraph[i - 1].Damage;
                rangeSquared -= LowerBounds;
                return DamageGraph[i - 1].Damage + (DamageDif * (rangeSquared / RangeDif));
            }
        }
        return DamageGraph[DamageGraph.Count - 1].Damage;
    }

    public float GetBarGraphDamage(float range)
    {
        if (range > GetMaxRange())
            return 0;
        if(range < DamageGraph[0].Range)
            return DamageGraph[0].Damage;

        for (int i = 1; i < DamageGraph.Count; i++)
        {
            if (range < DamageGraph[i].Range)
            {
                float RangeDif = DamageGraph[i].Range - DamageGraph[i - 1].Range;
                float DamageDif = DamageGraph[i].Damage - DamageGraph[i - 1].Damage;
                range -= DamageGraph[i - 1].Range;
                return DamageGraph[i - 1].Damage + (DamageDif * (range / RangeDif));
            }
        }
        return 0;
    }

    public float GetHalfArc()
    {
        if(halfArc == -1)
        {
            halfArc = Arc / 2f;
        }
        return halfArc;
    }

    public float GetMaxRangeSqr()
    {
        if(maxRangeSqr == -1)
        {
            maxRangeSqr = Mathf.Pow(GetMaxRange(), 2f);
        }
        return maxRangeSqr;
    }

    public float GetAverageDPS()
    {
        float maxDamage = 0;
        float minDamage = Mathf.Infinity;

        foreach(DamageNode node in DamageGraph)
        {
            if (node.Damage > maxDamage)
                maxDamage = node.Damage;
            if (node.Damage < minDamage)
                minDamage = node.Damage;
        }

        if (isBeam)
            return (maxDamage + minDamage) / 2f / Delay;
        else
            return (maxDamage + minDamage) * Projectiles * SalvoSize / 2f / Delay;
    }

    public Weapon GetSecondaryWeapon()
    {
        return ResourceManager.instance.GetWeapon(SecondaryWeapon);
    }

    public Vector2[] GetGraphDamagePoints()
    {
        Vector2[] Points = new Vector2[DamageGraph.Count];

        float MaxDamage = GetMaxDamage();
        float MaxRange = GetMaxRange();

        for(int i = 0; i < DamageGraph.Count; i++)
        {
            Points[i] = new Vector2(DamageGraph[i].Range / MaxRange, DamageGraph[i].Damage / MaxDamage);
        }

        return Points;
    }

    public class DamageNode
    {
        public float Range;
        public float Damage;
        float RangeSqr = -1;

        public float GetDisplayRange()
        {
            return Range * (1f / ResourceManager.instance.GetGameConstants().FiringRangeFactor);
        }

        public float GetRangedSquared()
        {
            if (RangeSqr == -1)
                RangeSqr = Mathf.Pow(Range, 2f);
            return RangeSqr;
        }
    }
}
