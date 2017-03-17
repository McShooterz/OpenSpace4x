/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class Beam : MonoBehaviour
{
    public GameObject hitEffect;

    LineRenderer beamLine;
    float beamLineOffset = 0;
    GameObject Target;
    Vector3 TargetOffSet;
    AttachedWeapon attachedWeapon;
    bool isPaused = false;
    float damageBonus;
    float totalDamage = 0;

    //Optimization
    float AccumulatedDamage = 0;
    float DamageIntervalTimer = 0;
    GameObject LastHitObject = null;
    SpaceUnit LastHitUnit = null;
    Projectile targetProjectile;

    // Ignores
    bool ignoreShields;
    bool ignoreArmor;
    bool ignoreArmorRating;

	// Use this for initialization
	void Start ()
    {
        beamLine = GetComponent<LineRenderer>();
        beamLine.SetPosition(0, transform.position);
        beamLine.SetPosition(1, transform.position);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!isPaused)
        {
            beamLineOffset -= 5f * GameManager.instance.GetDeltaTime();
            beamLine.material.SetTextureOffset("_MainTex", new Vector2(beamLineOffset, 0));
            if (Target != null)
            {
                UpdatePoints();
            }
            else
            {
                attachedWeapon.PartialBeamDelay();
                DestroySelf();
            }
        }
    }

    public void Initialize(AttachedWeapon attachedWeapon, GameObject parentObject, Vector3 origin, GameObject target, Vector3 targetOffset)
    {
        transform.position = origin;
        transform.parent = parentObject.transform;
        this.attachedWeapon = attachedWeapon;
        damageBonus = this.attachedWeapon.GetDamageBonus();
        Target = target;
        TargetOffSet = targetOffset;

        if(Target.tag == "PDTarget")
        {
            targetProjectile = Target.GetComponent<Projectile>();
        }

        if(this.attachedWeapon.baseWeapon.IgnoreShieldChance != 0)
        {
            if(this.attachedWeapon.baseWeapon.IgnoreShieldChance == 1f)
            {
                ignoreShields = true;
            }
            else
            {
                ignoreShields = Random.Range(0f,1f) < this.attachedWeapon.baseWeapon.IgnoreShieldChance;
            }
        }

        if (this.attachedWeapon.baseWeapon.IgnoreArmorChance != 0)
        {
            if (this.attachedWeapon.baseWeapon.IgnoreArmorChance == 1f)
            {
                ignoreArmor = true;
            }
            else
            {
                ignoreArmor = Random.Range(0f, 1f) < this.attachedWeapon.baseWeapon.IgnoreArmorChance;
            }
        }

        if (this.attachedWeapon.baseWeapon.IgnoreArmorRatingChance != 0)
        {
            if (this.attachedWeapon.baseWeapon.IgnoreArmorRatingChance == 1f)
            {
                ignoreArmorRating = true;
            }
            else
            {
                ignoreArmorRating = Random.Range(0f, 1f) < this.attachedWeapon.baseWeapon.IgnoreArmorRatingChance;
            }
        }
    }

    void UpdatePoints()
    {
        beamLine.SetPosition(0, transform.position);
        beamLine.SetPosition(1, transform.position);
        if (targetProjectile != null)
        {
            if(hitEffect != null)
            {
                hitEffect.transform.position = targetProjectile.transform.position;
            }
            beamLine.SetPosition(1, targetProjectile.transform.position);
            beamLine.material.mainTextureScale = new Vector2(Vector3.Distance(transform.position, targetProjectile.transform.position), 1);
            float range = (targetProjectile.transform.position - transform.position).sqrMagnitude;
            float damage = attachedWeapon.baseWeapon.CalculateDamage(range) * GetDeltaTime();
            damage += damage * damageBonus;
            AccumulatedDamage += damage;
            totalDamage += damage;
            if (DamageIntervalTimer > 0)
            {
                DamageIntervalTimer -= GetDeltaTime();
            }
            else
            {
                DamageIntervalTimer = ResourceManager.gameConstants.BeamDamageIntervalTime;
                targetProjectile.TakeDamage(attachedWeapon.baseWeapon, AccumulatedDamage);
                AccumulatedDamage = 0;
            } 
        }
        else
        {
            RaycastHit[] hits;
            hits = Physics.RaycastAll(transform.position, GetWorldTargetOffset() - transform.position, attachedWeapon.baseWeapon.GetMaxRange(), 1 << 12, QueryTriggerInteraction.Ignore);
            foreach (RaycastHit hit in hits)
            {
                if (LastHitObject == null)
                {
                    if (hit.transform.root.gameObject.tag == "Ship" || hit.transform.root.gameObject.tag == "Fighter" || hit.transform.root.gameObject.tag == "Station")
                    {
                        SpaceUnit HitUnit = hit.transform.root.GetComponent<SpaceUnit>();
                        if (attachedWeapon.isEnemy(HitUnit))
                        {
                            LastHitObject = hit.transform.root.gameObject;
                            LastHitUnit = HitUnit;
                            LastHitUnit.CheckShieldHit(hit.point, transform.position);
                            if (hitEffect != null)
                            {
                                hitEffect.transform.position = hit.point;
                            }
                            beamLine.SetPosition(1, hit.point);
                            beamLine.material.mainTextureScale = new Vector2(Vector3.Distance(transform.position, hit.point), 1);
                            float range = (hit.point - transform.position).sqrMagnitude;
                            float damage = attachedWeapon.baseWeapon.CalculateDamage(range) * GetDeltaTime();
                            damage += damage * damageBonus;
                            totalDamage += damage;
                            AccumulatedDamage = damage;
                            DamageIntervalTimer = ResourceManager.gameConstants.BeamDamageIntervalTime;
                            return;
                        }
                    }
                }
                else
                {
                    if (hit.transform.root.gameObject == LastHitObject)
                    {
                        LastHitUnit.CheckShieldHit(hit.point, transform.position);
                        if (hitEffect != null)
                        {
                            hitEffect.transform.position = hit.point;
                        }
                        beamLine.SetPosition(1, hit.point);
                        beamLine.material.mainTextureScale = new Vector2(Vector3.Distance(transform.position, hit.point), 1);
                        float range = (hit.point - transform.position).sqrMagnitude;
                        float damage = attachedWeapon.baseWeapon.CalculateDamage(range) * GetDeltaTime();
                        damage += damage * damageBonus;
                        totalDamage += damage;
                        AccumulatedDamage += damage;

                        if (DamageIntervalTimer > 0)
                        {
                            DamageIntervalTimer -= GetDeltaTime();
                        }
                        else
                        {
                            DamageIntervalTimer = ResourceManager.gameConstants.BeamDamageIntervalTime;
                            DealDamage(LastHitUnit, AccumulatedDamage);
                            AccumulatedDamage = 0;
                        }
                        return;
                    }
                    else if (hit.transform.root.gameObject.tag == "Ship" || hit.transform.root.gameObject.tag == "Fighter" || hit.transform.root.gameObject.tag == "Station")
                    {
                        SpaceUnit HitUnit = hit.transform.root.GetComponent<SpaceUnit>();
                        if (attachedWeapon.isEnemy(HitUnit))
                        {
                            if (LastHitUnit != null && AccumulatedDamage > 0)
                            {
                                DealDamage(LastHitUnit, AccumulatedDamage);
                            }
                            LastHitObject = hit.transform.root.gameObject;
                            LastHitUnit = HitUnit;
                            LastHitUnit.CheckShieldHit(hit.point, transform.position);
                            if (hitEffect != null)
                            {
                                hitEffect.transform.position = hit.point;
                            }
                            beamLine.SetPosition(1, hit.point);
                            beamLine.material.mainTextureScale = new Vector2(Vector3.Distance(transform.position, hit.point), 1);
                            float range = (hit.point - transform.position).sqrMagnitude;
                            float damage = attachedWeapon.baseWeapon.CalculateDamage(range) * GetDeltaTime();
                            damage += damage * damageBonus;
                            totalDamage += damage;
                            AccumulatedDamage = damage;
                            DamageIntervalTimer = ResourceManager.gameConstants.BeamDamageIntervalTime;
                            return;
                        }
                    }
                }
            }
        }
    }

    public void DestroySelf()
    {
        if(AccumulatedDamage > 0)
        {
            if(targetProjectile != null)
            {
                targetProjectile.TakeDamage(attachedWeapon.baseWeapon, AccumulatedDamage);
            }
            else if (LastHitObject != null && LastHitUnit != null)
            {
                DealDamage(LastHitUnit, AccumulatedDamage);
            }
        }

        attachedWeapon.RecordDamage(totalDamage);
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

    void DealDamage(SpaceUnit target, float damage)
    {
        Vector3 Position;
        if(hitEffect != null)
        {
            Position = hitEffect.transform.position;
        }
        else
        {
            Position = target.transform.position;
        }

        if (target is Ship)
        {
            (target as Ship).TakeDamage(attachedWeapon, Position, transform.position, damage, ignoreShields, ignoreArmor, ignoreArmorRating);
        }
        else if (target is Station)
        {
            (target as Station).TakeDamage(attachedWeapon, Position, transform.position, damage, ignoreShields, ignoreArmor, ignoreArmorRating);
        }
        else if(target is Fighter)
        {
            (target as Fighter).TakeDamage(attachedWeapon, damage, ignoreShields, ignoreArmor, ignoreArmorRating);
        }
    }

    Vector3 GetWorldTargetOffset()
    {
        return Target.transform.TransformPoint(TargetOffSet);
    }
}
