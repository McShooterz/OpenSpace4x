using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;

public class Weapon
{
    public WeaponCategory Category { get; set; }

    // Visuals
    public string WeaponEffect { get; set; }
    public string FireSound { get; set; }
    public string HitEffect { get; set; }
    public string HitSound { get; set; }
    public float HitEffectTime { get; set; }
    public float HitEffectScale { get; set; }

    public bool isBeam { get; set; }
    public bool isHoming { get; set; }
    public bool AlwaysForward { get; set; }
    public bool SingleFireSound { get; set; }

    public float AmmoCost { get; set; }
    public float PowerCost { get; set; }
    public float BeamPowerCost { get; set; }
    public float Delay { get; set; }
    public int SalvoSize { get; set; }
    public float SalvoDelay { get; set; }
    public float Arc { get; set; }
    public float BeamDuration { get; set; }
    int projectiles;
    public int Projectiles
    {
        get { return Mathf.Max(1, projectiles); }
        set{ projectiles = Mathf.Max(1, value); }
    }
    public bool MultiHardpoint { get; set; }
    public float Scale { get; set; }

    //Projectile properties
    public float ProjectileLife { get; set; }
    public float ProjectileSpeed { get; set; }
    public float ProjectileTurnRate { get; set; }
    public float Spread { get; set; }

    //Attributes
    public bool DamageAllQuads { get; set; }
    public int HomingAdditionalTargets { get; set; }
    public float ShieldDamageModifier { get; set; }
    public float ArmorDamageModifier { get; set; }
    public float HealthDamageModifier { get; set; }
    public float FighterDamageModifier { get; set; }
    public float ProjectileDamageModifier { get; set; }
    public float PowerDamageModifier { get; set; }
    public float IgnoreShieldChance { get; set; }
    public float IgnoreArmorChance { get; set; }
    public float IgnoreArmorRatingChance { get; set; }

    //Missile defense
    public bool PDTargetable { get; set; }
    public float ProjectileHealth { get; set; }
    public bool Jammable { get; set; }
    public bool dieOnProjectileCollision { get; set; }
    public bool PointDefense { get; set; }
    public bool PointDefenseOnly { get; set; }

    public List<DamageNode> DamageGraph { get; set; }

    //SecondaryWeapon
    public string SecondaryWeapon { get; set; }

    float halfArc = -1;
    float maxRangeSqr = -1;

    private string Name;

    //Default constructor for xml serialization and setting default values
    public Weapon()
    {
        Projectiles = 1;
        Scale = 1.0f;
        ShieldDamageModifier = 1f;
        ArmorDamageModifier = 1f;
        HealthDamageModifier = 1f;
        FighterDamageModifier = 1f;
        ProjectileDamageModifier = 1f;
        PowerDamageModifier = 0f;
        IgnoreShieldChance = 0f;
        IgnoreArmorChance = 0f;
        IgnoreArmorRatingChance = 0f;
        SecondaryWeapon = "";

        DamageGraph = new List<DamageNode>();
    }

    public void SetName(string name)
    {
        Name = name;
    }

    public string GetName()
    {
        return Name;
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
