/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    bool isPaused = false;
    bool isHoming;
    float lifeTimer = 10f;
    Vector3 Origin;
    GameObject Target;
    Vector3 TargetOffset;
    int additionalHomingTargets;
    bool Jammed;
    Vector3 JamTargetPosition;
    float Health;

    AttachedWeapon attachedWeapon;
    Weapon PrimaryWeapon;
    Weapon SecondaryWeapon;

    public void Initialize(AttachedWeapon attached, Weapon weapon, Vector3 origin, GameObject target, Vector3 targetOffset)
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        attachedWeapon = attached;
        PrimaryWeapon = weapon;
        SecondaryWeapon = PrimaryWeapon.GetSecondaryWeapon();
        Origin = origin;
        transform.position = origin;
        lifeTimer = PrimaryWeapon.ProjectileLife;
        Target = target;
        TargetOffset = targetOffset;
        if (PrimaryWeapon.isHoming)
        {
            isHoming = true;
            transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);
            additionalHomingTargets = PrimaryWeapon.HomingAdditionalTargets;
            if (PrimaryWeapon.PDTargetable)
            {
                transform.tag = "PDTarget";
                Rigidbody rigidBody = gameObject.AddComponent<Rigidbody>();
                rigidBody.isKinematic = true;
                Health = PrimaryWeapon.ProjectileHealth;
                GetShipManager().AddDPTargetableProjectile(this);
            }
            if (PrimaryWeapon.Jammable)
            {
                GetShipManager().AddJammableProjectile(this);
            }
        }
        else
        {
            if (target.transform.root.tag == "Ship" || target.transform.root.tag == "Fighter")
            {
                MobileSpaceUnit targetScript = target.transform.root.GetComponent<MobileSpaceUnit>();
                RotateTowardsMovingTarget(targetScript);
            }
            else
            {
                transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);
            }
        }
        if (PrimaryWeapon.Spread > 0)
        {
            float spreadHalf = PrimaryWeapon.Spread / 2f;
            transform.Rotate(Random.Range(-spreadHalf, spreadHalf), Random.Range(-spreadHalf, spreadHalf), 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
            if (lifeTimer > 0)
            {
                lifeTimer -= GetDeltaTime();
            }
            else
            {
                DestroySelf();
            }

            if (isHoming)
            {
                if(Jammed)
                {
                    Vector3 Direction = JamTargetPosition - transform.position;
                    Quaternion goalRotation = Quaternion.LookRotation(Direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, goalRotation, PrimaryWeapon.ProjectileTurnRate * GetDeltaTime());
                }
                else if(Target != null)
                {
                    if (!Target.activeSelf)
                    {
                        Target = null;
                    }
                    else
                    {
                        Vector3 Direction = GetTargetOffset() - transform.position;
                        Quaternion goalRotation = Quaternion.LookRotation(Direction);
                        transform.rotation = Quaternion.Slerp(transform.rotation, goalRotation, PrimaryWeapon.ProjectileTurnRate * GetDeltaTime());
                        if(SecondaryWeapon != null)
                        {
                            if(Direction.sqrMagnitude < SecondaryWeapon.GetMaxRangeSqr())
                            {
                                FireSecondaryWeapon();
                                Die();
                            }
                        }
                    }
                }
                else
                {
                    if (additionalHomingTargets > 0)
                    {
                        additionalHomingTargets--;
                        if (PrimaryWeapon.PointDefenseOnly)
                        {
                            Projectile newTarget = GetShipManager().GetClosestTargetableProjectile(transform.position);
                            if (newTarget != null)
                                Target = GetShipManager().GetClosestTargetableProjectile(transform.position).gameObject;
                            else
                                isHoming = false;
                        }
                        else
                        {
                            Target = GetShipManager().GetClosestEnemyTarget(new Vector2(transform.position.x, transform.position.z));
                        }
                        if(Target != null)
                        {
                            if(Target.tag == "Ship")
                                TargetOffset = Target.GetComponent<Ship>().GetDamageHardPoint();
                            else if(Target.tag == "Station")
                                TargetOffset = Target.GetComponent<Station>().GetDamageHardPoint();
                        }
                    }
                }
            }
            transform.position += transform.forward * PrimaryWeapon.ProjectileSpeed * GetDeltaTime();
        }
    }

    void OnTriggerEnter(Collider hitObject)
    {
        if (hitObject.gameObject.tag == "HitBox")
        {
            if (hitObject.transform.root.gameObject.tag == "Ship")
            {
                Ship hitShip = hitObject.transform.root.gameObject.GetComponent<Ship>();
                if (GetShipManager().isEnemy(hitShip))
                {
                    float range = (new Vector2(transform.position.x - Origin.x, transform.position.z - Origin.z)).sqrMagnitude;
                    float damage = GetDamage(range);
                    //Add damage bonus
                    damage += damage * attachedWeapon.GetDamageBonus();
                    attachedWeapon.RecordDamage(damage);
                    hitShip.TakeDamage(attachedWeapon, transform.position, Origin, damage, GetShieldIgnore(), GetArmorIgnore(), GetArmorRatingIgnore());
                    Die();
                }
            }
            else if (hitObject.transform.root.gameObject.tag == "Station")
            {
                Station hitStation = hitObject.transform.root.gameObject.GetComponent<Station>();
                if (GetShipManager().isEnemy(hitStation))
                {
                    float range = (new Vector2(transform.position.x - Origin.x, transform.position.z - Origin.z)).sqrMagnitude;
                    float damage = GetDamage(range);
                    //Add damage bonus
                    damage += damage * attachedWeapon.GetDamageBonus();
                    attachedWeapon.RecordDamage(damage);
                    hitStation.TakeDamage(attachedWeapon, transform.position, Origin, damage, GetShieldIgnore(), GetArmorIgnore(), GetArmorRatingIgnore());
                    Die();
                }
            }
            else if (hitObject.transform.root.gameObject.tag == "Fighter")
            {
                Fighter hitFighter = hitObject.transform.root.gameObject.GetComponent<Fighter>();
                if (GetShipManager().isEnemy(hitFighter))
                {
                    float range = (new Vector2(transform.position.x - Origin.x, transform.position.z - Origin.z)).sqrMagnitude;
                    float damage = GetDamage(range);
                    //Add damage bonus
                    damage += damage * attachedWeapon.GetDamageBonus();
                    attachedWeapon.RecordDamage(damage);
                    hitFighter.CheckShieldHit(transform.position, Vector2.zero);
                    hitFighter.TakeDamage(attachedWeapon, damage, GetShieldIgnore(), GetArmorIgnore(), GetArmorRatingIgnore());
                    Die();
                }
            }
        }
        else if(PrimaryWeapon.PointDefense && hitObject.gameObject.tag == "PDTarget")
        {
            Projectile hitProjectile = hitObject.transform.root.gameObject.GetComponent<Projectile>();
            if(GetShipManager().isEnemy(hitProjectile))
            {
                float range = (new Vector2(transform.position.x - Origin.x, transform.position.z - Origin.z)).sqrMagnitude;
                float damage = GetDamage(range);
                //Add damage bonus
                damage += damage * attachedWeapon.GetDamageBonus();
                attachedWeapon.RecordDamage(damage);
                hitProjectile.TakeDamage(PrimaryWeapon, damage);
                if (PrimaryWeapon.dieOnProjectileCollision)
                {
                    Die();
                }
            }
        }
    }

    public void Die()
    {
        if (attachedWeapon != null)
        {
            if (PrimaryWeapon.HitEffect != "")
            {
                ResourceManager.instance.CreateExplosion(transform.position, PrimaryWeapon.HitEffect, PrimaryWeapon.HitSound, PrimaryWeapon.HitEffectScale, PrimaryWeapon.HitEffectTime, false);
            }
            attachedWeapon.RemoveProjectile(this);
        }
        DestroySelf();
    }

    public void DestroySelf()
    {
        ShipManager manager = GetShipManager();
        if(manager != null)
            manager.RemoveProjectile(this);
        Destroy(gameObject);
    }

    public void Pause(bool state)
    {
        isPaused = state;
    }

    float GetDeltaTime()
    {
        return GameManager.instance.GetDeltaTime();
    }

    //Method to predict position of moving target
    public void RotateTowardsMovingTarget(MobileSpaceUnit target)
    {
        Vector3 targetDirection = target.transform.position - transform.position;
        float projectileSpeedSquared = PrimaryWeapon.ProjectileSpeed * attachedWeapon.baseWeapon.ProjectileSpeed;
        float tSpeed2 = target.GetVelocity().sqrMagnitude * GameManager.instance.GetGameSpeed();
        float fDot1 = Vector3.Dot(targetDirection, target.GetVelocity());
        float targetDistanceSquared = targetDirection.sqrMagnitude;
        float d = (fDot1 * fDot1) - targetDistanceSquared * (tSpeed2 - projectileSpeedSquared);
        if (d < 0.1f)  // negative == no possible course because the interceptor isn't fast enough
        {
            transform.rotation = Quaternion.LookRotation(target.transform.position, Vector3.up);
            return;
        }
        float sqrt = Mathf.Sqrt(d);
        float S1 = (-fDot1 - sqrt) / targetDistanceSquared;
        float S2 = (-fDot1 + sqrt) / targetDistanceSquared;
        if (S1 < 0.0001f)
        {
            if (S2 < 0.0001f)
            {
                transform.rotation = Quaternion.LookRotation(target.transform.position, Vector3.up);
                return;
            }
            else
            {
                transform.rotation = Quaternion.LookRotation((S2) * targetDirection + target.GetVelocity(), Vector3.up);
                return;
            }
        }
        else if (S2 < 0.0001f)
        {
            transform.rotation = Quaternion.LookRotation((S1) * targetDirection + target.GetVelocity(), Vector3.up);
            return;
        }
        else if (S1 < S2)
        {
            transform.rotation = Quaternion.LookRotation((S2) * targetDirection + target.GetVelocity(), Vector3.up);
            return;
        }
        else
        {
            transform.rotation = Quaternion.LookRotation((S1) * targetDirection + target.GetVelocity(), Vector3.up);
            return;
        }
    }

    public void RotateTowardsMovingTarget(Projectile target)
    {
        Vector3 targetDirection = target.transform.position - transform.position;
        float projectileSpeedSquared = attachedWeapon.baseWeapon.ProjectileSpeed * attachedWeapon.baseWeapon.ProjectileSpeed;
        float tSpeed2 = target.GetVelocity().sqrMagnitude * GameManager.instance.GetGameSpeed();
        float fDot1 = Vector3.Dot(targetDirection, target.GetVelocity());
        float targetDistanceSquared = targetDirection.sqrMagnitude;
        float d = (fDot1 * fDot1) - targetDistanceSquared * (tSpeed2 - projectileSpeedSquared);
        if (d < 0.1f)  // negative == no possible course because the interceptor isn't fast enough
        {
            transform.rotation = Quaternion.LookRotation(target.transform.position, Vector3.up);
            return;
        }
        float sqrt = Mathf.Sqrt(d);
        float S1 = (-fDot1 - sqrt) / targetDistanceSquared;
        float S2 = (-fDot1 + sqrt) / targetDistanceSquared;
        if (S1 < 0.0001f)
        {
            if (S2 < 0.0001f)
            {
                transform.rotation = Quaternion.LookRotation(target.transform.position, Vector3.up);
                return;
            }
            else
            {
                transform.rotation = Quaternion.LookRotation((S2) * targetDirection + target.GetVelocity(), Vector3.up);
                return;
            }
        }
        else if (S2 < 0.0001f)
        {
            transform.rotation = Quaternion.LookRotation((S1) * targetDirection + target.GetVelocity(), Vector3.up);
            return;
        }
        else if (S1 < S2)
        {
            transform.rotation = Quaternion.LookRotation((S2) * targetDirection + target.GetVelocity(), Vector3.up);
            return;
        }
        else
        {
            transform.rotation = Quaternion.LookRotation((S1) * targetDirection + target.GetVelocity(), Vector3.up);
            return;
        }
    }

    Vector3 GetTargetOffset()
    {
        return Target.transform.TransformPoint(TargetOffset);
    }

    bool GetShieldIgnore()
    {
        if (attachedWeapon.baseWeapon.IgnoreShieldChance != 0)
        {
            if (attachedWeapon.baseWeapon.IgnoreShieldChance == 1f)
            {
                return true;
            }
            else
            {
                return Random.Range(0f, 1f) < attachedWeapon.baseWeapon.IgnoreShieldChance;
            }
        }
        return false;
    }

    bool GetArmorIgnore()
    {
        if (attachedWeapon.baseWeapon.IgnoreArmorChance != 0)
        {
            if (attachedWeapon.baseWeapon.IgnoreArmorChance == 1f)
            {
                return true;
            }
            else
            {
                return Random.Range(0f, 1f) < attachedWeapon.baseWeapon.IgnoreArmorChance;
            }
        }
        return false;
    }

    bool GetArmorRatingIgnore()
    {
        if (attachedWeapon.baseWeapon.IgnoreArmorRatingChance != 0)
        {
            if (attachedWeapon.baseWeapon.IgnoreArmorRatingChance == 1f)
            {
                return true;
            }
            else
            {
                return Random.Range(0f, 1f) < attachedWeapon.baseWeapon.IgnoreArmorRatingChance;
            }
        }
        return false;
    }

    float GetDamage(float Range)
    {
        return PrimaryWeapon.CalculateDamage(Range);
    }

    public void Jam()
    {
        if (Target != null && Target.activeSelf && SecondaryWeapon != null)
        {
            FireSecondaryWeapon();
            Die();
        }
        else
        {
            Jammed = true;
            JamTargetPosition = new Vector3(Random.Range(-200f, 200f), Random.Range(-200f, 200f), Random.Range(-200f, 200f)) + transform.position;
            TextMeshController popupObject = ResourceManager.instance.CreateWorldMessage(transform.position, "Jammed", Color.red, 18);
            popupObject.transform.parent = transform;
        }
    }

    public bool isJammed()
    {
        return Jammed;
    }

    public void TakeDamage(Weapon weapon, float damage)
    {
        if(weapon.ProjectileDamageModifier != 1f)
        {
            damage *= weapon.ProjectileDamageModifier;
        }

        if(GameManager.instance.GetShowCombatDamage())
        {
            ResourceManager.instance.CreatePopupMessage(transform.position, damage.ToString("0"), Color.red, 3f);
        }

        Health -= damage;
        if(Health < 0)
        {
            if (Target != null && Target.activeSelf && SecondaryWeapon != null)
            {
                FireSecondaryWeapon();
            }
            Die();
        }
    }

    public Vector3 GetVelocity()
    {
        return transform.forward * PrimaryWeapon.ProjectileSpeed;
    }

    public SpaceUnit GetParent()
    {
            return attachedWeapon.GetParentUnit();
    }

    public ShipManager GetShipManager()
    {
            return GetParent().GetShipManager();
    }

    void FireSecondaryWeapon()
    {
        for(int i = 0; i < SecondaryWeapon.Projectiles; i++)
        {
            Projectile projectile = ResourceManager.instance.CreateProjectile(SecondaryWeapon);
            projectile.Initialize(attachedWeapon, SecondaryWeapon, transform.position, Target, TargetOffset);
            attachedWeapon.AddProjectile(projectile);
        }
    }
}
