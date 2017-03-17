/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections.Generic;

public sealed class Fighter : MobileSpaceUnit
{
    FighterDefinition Definition;

    FighterWing parentWing;

    //AI
    bool Recalled = false;
    float swarmTimer = 0;
    Vector3 swarmPoint = Vector3.zero;

    //Stats
    float Health;
    float HealthMax;
    float ArmorHealth;
    float ArmorHealthMax;
    float ArmorRating;
    float ShieldHealth;
    float ShieldHealthMax;
    float ShieldRating;
    float ShieldRecharge;
    float ShieldDelay;

    int Troops = 0;
    int Crew = 1;

    float EngineTrust;
    float EngineTurn;

    float ShieldTimer; 

    public List<AttachedWeapon> Weapons = new List<AttachedWeapon>();
    
    // Use this for initialization
    protected override void Start()
    {

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (!isPaused)
        {
            UpdateStats();
            UpdateVelocity();
            if (!Disabled)
            {
                if (Mobile)
                {
                    if (Target != null)
                    {
                        if (TargetIsEnemy)
                        {
                            if (Definition.AssaultPod && !(Target is Fighter))
                            {
                                AssaultTarget();
                            }
                            else
                            {
                                EngageTarget();
                            }
                        }
                        else
                            Swarm(Target.transform.position);
                    }
                    else if (ValidMoveOrder)
                    {
                        MoveTowardsGoal(goalPosition);
                    }
                    else if (Recalled && parentWing.GetParent() != null)
                    {
                        DockWithParent();
                    }
                    else
                    {
                        Swarm(goalPosition);
                    }
                }
                foreach (AttachedWeapon weapon in Weapons)
                {
                    weapon.Update();
                }
                if (WeaponCheckTimer > 0)
                {
                    WeaponCheckTimer -= GetDeltaTime();
                }
                else
                {
                    if (Target != null && Target.isDestroyed())
                    {
                        Target = null;
                    }
                    WeaponCheckTimer = ResourceManager.gameConstants.WeaponCheckTime;
                    CheckWeapons();
                }
            }
        }
    }

    public void Initialize(FighterDefinition definition)
    {
        Initialize();
        Definition = definition;
        SetWeapons();

        Health = Definition.Health;
        HealthMax = Definition.Health;
        ArmorHealth = Definition.ArmorHealth;
        ArmorHealthMax = Definition.ArmorHealth;
        ArmorRating = Definition.ArmorRating;
        ShieldHealth = Definition.ShieldHealth;
        ShieldHealthMax = Definition.ShieldHealth;
        ShieldRating = Definition.ShieldRating;
        ShieldRecharge = Definition.ShieldRecharge;
        ShieldDelay = Definition.ShieldDelay;

        Troops = Definition.Troops;
        Crew = Definition.Crew;

        EngineTrust = Definition.EngineTrust;
        EngineTurn = Definition.EngineTurn;

        acceleration = EngineTrust / 20f;
        attackRange = 1.5f;
    }

    public void SetWeapons()
    {
        foreach (string weaponName in Definition.WeaponNames)
        {
            Weapon weapon = ResourceManager.GetWeapon(weaponName);
            if (weapon != null)
            {
                AttachedWeapon attachedWeapon = new AttachedWeapon(weapon);
                attachedWeapon.SetParent(this);
                Weapons.Add(attachedWeapon);
                if (weapon.GetMaxRange() > attackRange)
                    attackRange = weapon.GetMaxRange();
            }
        }
    }

    public void SetParent(FighterWing wing)
    {
        parentWing = wing;
    }

    public void Recall()
    {
        Target = null;
        ValidMoveOrder = false;
        TargetIsEnemy = false;
        Recalled = true;
    }

    public FighterWing GetParentWing()
    {
        return parentWing as FighterWing;
    }

    void UpdateStats()
    {
        if (ShieldTimer > 0)
        {
            ShieldTimer -= GetDeltaTime();
        }
        else if (ShieldHealth < ShieldHealthMax)
        {
            ShieldHealth += ShieldRecharge * GetDeltaTime();
            if (ShieldHealth > ShieldHealthMax)
                ShieldHealth = ShieldHealthMax;
        }
    }

    void EngageTarget()
    {
        Vector3 Direction = Target.transform.position - transform.position;
        float Angle = Vector3.Angle(transform.forward, Direction);
        float goalDistanceSqr = Vector3.SqrMagnitude(Direction);

        if (goalDistanceSqr > Mathf.Pow(attackRange, 2f))
        {
            RotateTowards(Direction);
            if (Angle < 15f)
            {
                MoveForwards();
            }
        }
        else //Swarm target
        {
            if (swarmTimer > 0)
            {
                swarmTimer -= GetDeltaTime();
            }
            else
            {
                swarmTimer = 2f;
                swarmPoint = new Vector3(Target.transform.position.x + Random.Range(-attackRange, attackRange), Target.transform.position.y + Random.Range(0.5f, 1.2f), Target.transform.position.z + Random.Range(-attackRange, attackRange));
            }
            Direction = swarmPoint - transform.position;
            Angle = Vector3.Angle(transform.forward, Direction);
            RotateTowards(Direction);
            if (Angle < 15f)
            {
                MoveForwards();
            }
        }
    }

    void CheckWeapons()
    {
        if (shipManager == null)
            return;

        CacheTargets();
        
        foreach (AttachedWeapon weapon in Weapons)
        {
            if (weapon.CanFire())
            {
                if (!weapon.baseWeapon.PointDefenseOnly)
                {
                    if (Target != null && Target.CanBeTargeted() && isEnemy(Target) && MainTargetRange < weapon.baseWeapon.GetMaxRangeSqr())
                    {
                        Vector3[] hardPoint = new Vector3[1];
                        hardPoint[0] = Vector3.zero;
                        weapon.Fire(hardPoint, Target.gameObject, audioSource);
                        return;
                    }
                    else //Check for targets of opportunity
                    {
                        foreach (TargetInfo potentialTarget in CachedTargets)
                        {
                            if (potentialTarget.RangeSqr < weapon.baseWeapon.GetMaxRangeSqr())
                            {
                                Vector3[] hardPoint = new Vector3[1];
                                hardPoint[0] = Vector3.zero;
                                weapon.Fire(hardPoint, potentialTarget.Target.gameObject, audioSource);
                                return;
                            }
                        }
                        if (weapon.baseWeapon.PointDefense)
                        {
                            foreach (PDTargetInfo potentialTarget in PDCachedTargets)
                            {
                                if (potentialTarget.RangeSqr < weapon.baseWeapon.GetMaxRangeSqr())
                                {
                                    Vector3[] hardPoint = new Vector3[1];
                                    hardPoint[0] = Vector3.zero;
                                    weapon.Fire(hardPoint, potentialTarget.Target.gameObject, audioSource);
                                    return;
                                }
                            }
                        }
                    }
                }
                else if(weapon.baseWeapon.PointDefense)
                {
                    foreach (PDTargetInfo potentialTarget in PDCachedTargets)
                    {
                        if (potentialTarget.RangeSqr < weapon.baseWeapon.GetMaxRangeSqr())
                        {
                            Vector3[] hardPoint = new Vector3[1];
                            hardPoint[0] = Vector3.zero;
                            weapon.Fire(hardPoint, potentialTarget.Target.gameObject, audioSource);
                            return;
                        }
                    }
                }
            }
        }
    }

    public void TakeDamage(AttachedWeapon weapon, float damage, bool ignoreShields, bool ignoreArmor, bool ignoreArmorRating)
    {
        if(weapon.baseWeapon.FighterDamageModifier != 1f)
        {
            damage *= weapon.baseWeapon.FighterDamageModifier;
        }

        if (!ignoreShields && ShieldHealth > 0)
        {
            float effectiveDamage;

            if (weapon.baseWeapon.ShieldDamageModifier != 1f)
            {
                effectiveDamage = damage * weapon.baseWeapon.ShieldDamageModifier;
            }
            else
            {
                effectiveDamage = damage;
            }     

            if (effectiveDamage > 0)
            {
                float absorbDamage = effectiveDamage * Mathf.Clamp(ShieldRating / effectiveDamage, ResourceManager.gameConstants.MinShieldAbsorb, 1f);
                float damageUseRatio;

                if (ShieldHealth > absorbDamage)
                {
                    if (GameManager.instance.GetShowCombatDamage())
                    {
                        CreateShieldDamagePopup(absorbDamage);
                    }
                    ShieldHealth -= absorbDamage;
                    damageUseRatio = 1f - absorbDamage / effectiveDamage;
                }
                else
                {
                    if (GameManager.instance.GetShowCombatDamage())
                    {
                        CreateShieldDamagePopup(ShieldHealth);
                    }
                    damageUseRatio = 1f - ShieldHealth / effectiveDamage;
                    ShieldHealth = 0;
                    ShieldTimer = ShieldDelay;
                }
                damage *= damageUseRatio;
            }
            else
            {
                return;
            }
        }

        //Armor takes reduced damage based on armor rating
        if (!ignoreArmor && damage > 0 && ArmorHealth > 0)
        {
            float effectiveDamage;

            if (weapon.baseWeapon.ArmorDamageModifier != 1f)
            {
                effectiveDamage = damage * weapon.baseWeapon.ArmorDamageModifier;
            }
            else
            {
                effectiveDamage = damage;
            }

            if (effectiveDamage > 0)
            {
                if (!ignoreArmorRating && ArmorRating > 0)
                {
                    float damageCoeff = Mathf.Min(0.99f, Mathf.Pow(damage / ArmorRating, 0.366f) * 0.5f);
                    //float damageReduction = 1f - damageCoeff;
                    effectiveDamage *= damageCoeff;
                }   

                if (ArmorHealth > effectiveDamage)
                {
                    ArmorHealth -= effectiveDamage;
                    if (GameManager.instance.GetShowCombatDamage())
                    {
                        CreateArmorDamagePopup(effectiveDamage);
                    }
                    return;
                }
                else
                {
                    if (GameManager.instance.GetShowCombatDamage())
                    {
                        CreateArmorDamagePopup(ArmorHealth);
                    }
                    float ratio = 1f - ArmorHealth / effectiveDamage;
                    damage *= ratio;
                    ArmorHealth = 0;

                }
            }
            else
            {
                return;
            }
        }

        //Apply remaining damage to health
        if (damage > 0)
        {
            if (weapon.baseWeapon.HealthDamageModifier != 1f)
            {
                damage *= weapon.baseWeapon.HealthDamageModifier;
            }

            if(GameManager.instance.GetShowCombatDamage())
            {
                CreateHealthDamagePopup(damage);
            }

            Health -= damage;
            if (Health < 0)
            {
                Health = 0;
                Die(weapon);
            }
        }
    }

    public override void CheckShieldHit(Vector3 point, Vector2 origin)
    {
        if(ShieldHealth > 0)
        {
            ShieldHit(point);
        }
    }

    void Die(AttachedWeapon weapon)
    {
        Destroyed = true;
        weapon.GetKill(this);
        shipManager.FighterKilled(this);
        ResourceManager.CreateExplosion(transform.position, "effect_explosionfighter", "FighterExplosion", 0.33f, 5f, true);
        parentWing.RemoveUnit(this);
        gameObject.SetActive(false);
    }

    public void DeleteSelf()
    {
        foreach (AttachedWeapon weapon in Weapons)
        {
            weapon.DestroyBeam();
            weapon.DestroyAllProjectiles();
        }
        shipManager.RemoveUnit(this);
        parentWing.RemoveUnit(this);
        Destroy(gameObject);
    }

    public new void SetPause(bool state)
    {
        base.SetPause(state);
        foreach (AttachedWeapon weapon in Weapons)
        {
            weapon.Pause(state);
        }
    }

    public void DockWithParent()
    {
        if(MoveTowardsGoal(parentWing.GetParent().transform.position))
        {
            if(parentWing.GetParent() is Ship)
            {
                (parentWing.GetParent() as Ship).DockFighter(this);
            }
            else if(parentWing.GetParent() is Station)
            {
                (parentWing.GetParent() as Station).DockFighter(this);
            }
        }
    }

    void AssaultTarget()
    {
        if (MoveTowardsGoal(Target.transform.position))
        {
            if (Target is Ship)
            {
                (Target as Ship).AddBoardingForce(shipManager, Troops, Crew);
                DeleteSelf();
            }
            else if (Target is Station)
            {
                (Target as Station).AddBoardingForce(shipManager, Troops, Crew);
                DeleteSelf();
            }
        }
    }

    public override void CancelOrders()
    {
        base.CancelOrders();
        Recalled = false;
        goalPosition = parentWing.transform.position;
    }

    protected override float GetMaxWeaponRangeSqr()
    {
        float MaxRange = 0;
        foreach(AttachedWeapon weapon in Weapons)
        {
            float range = weapon.baseWeapon.GetMaxRangeSqr();
            if (range > MaxRange)
            {
                MaxRange = range;
            }
        }
        return MaxRange;
    }

    protected override float GetMaxSpeed()
    {
        return EngineTrust;
    }

    protected override float GetTurnRate()
    {
        return EngineTurn * ResourceManager.gameConstants.FighterTurnMultiplier;
    }

    void Swarm(Vector3 Position)
    {
        if (swarmTimer > 0)
        {
            swarmTimer -= GetDeltaTime();
        }
        else
        {
            swarmTimer = 2f;
            swarmPoint = new Vector3(Position.x + Random.Range(-0.5f, 0.5f), Position.y + Random.Range(0.3f, 0.6f), Position.z + Random.Range(-0.5f, 0.5f));
        }
        Vector3 Direction = swarmPoint - transform.position;
        float Angle = Vector3.Angle(transform.forward, Direction);
        RotateTowards(Direction);
        if (Angle < 15f)
        {
            MoveForwards();
        }
    }

    public override void SetNewGoalPosition(Vector3 point)
    {
        base.SetNewGoalPosition(point);
        point.y = Random.Range(0.7f, 1.1f);
        point.x += Random.Range(-0.25f, 0.25f);
        point.z += Random.Range(-0.25f, 0.25f);
        goalPosition = point;
    }

    public void DestroyWeaponEffects()
    {
        foreach (AttachedWeapon weapon in Weapons)
        {
            weapon.DestroyBeam();
            weapon.DestroyAllProjectiles();
        }
    }

    public Texture2D GetIcon()
    {
        return Definition.GetIcon();
    }

    public float GetKillExperience()
    {
        return Definition.ExperienceKillValue;
    }

    public int GetCrew()
    {
        return Crew;
    }

    public int GetTroops()
    {
        return Troops;
    }

    public void SetTroops(int count)
    {
        Troops = count;
    }

    public FighterDefinition GetDefinition()
    {
        return Definition;
    }

    public string GetName()
    {
        return Definition.Name;
    }

    public int GetMaxSquadronSize()
    {
        return Definition.MaxSquadronSize;
    }

    public bool HasShields()
    {
        return ShieldHealthMax > 0;
    }

    public float GetShieldRatio()
    {
        return ShieldHealth / ShieldHealthMax;
    }

    public float GetArmorRatio()
    {
        if (ArmorHealthMax == 0)
            return 0;
        if (ArmorHealth == 0)
            return 0;
        return ArmorHealth / ArmorHealthMax;
    }

    public float GetHealthRatio()
    {
        return Health / HealthMax;
    }
}
