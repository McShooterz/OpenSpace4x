/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections.Generic;

public sealed class AttachedWeapon
{
    #region Variables
    SpaceUnit parentUnit;
    GameObject Target;
    Vector3 TargetOffset;
    public Weapon baseWeapon;
    float Delay = 0;
    int SalvoCount = 0;
    float BeamDuration = 0;
    float SalvoDelay = 0;
    Beam beam;
    AudioSource audioSource;
	Vector3[] CurrentHardPoints;
	List<Projectile> Projectiles = new List<Projectile>();
    QuadrantTypes FiringQuadrant = QuadrantTypes.Center;
    #endregion

    public AttachedWeapon(Weapon weapon)
    {     
        baseWeapon = weapon;
    }

    public void SetParent(SpaceUnit parent)
    {
        parentUnit = parent;
    }

    public void SetFiringQuadrant(QuadrantTypes direction)
    {
        FiringQuadrant = direction;
    }

    public void Update()
    {
        DecayTimer(ref Delay);       
        DecayTimer(ref SalvoDelay);

		if(baseWeapon.isBeam)
		{
			if (beam != null)
			{
				if (BeamDuration > 0)
				{
					DecayTimer(ref BeamDuration);
					if(!ConsumePower())
					{
                        PartialBeamDelay();
                        beam.DestroySelf();
						beam = null;
						BeamDuration = 0;
					}
				}
				else
				{
					beam.DestroySelf();
					beam = null;
				}
			}
		}
		else if(SalvoCount > 0 && SalvoDelay == 0)
		{
			FireSalvo();          
		}
    }

    void DecayTimer(ref float timer)
    {
        if (timer > 0)
            timer -= GameManager.instance.GetDeltaTime();
        else
            timer = 0;
    }

    public bool CanFire()
    {
        if (Delay > 0)
            return false;
        return CheckPowerAndAmmo();
    }

    public void Fire(Vector3[] firePoints, GameObject target, AudioSource audio)
    {
        CurrentHardPoints = firePoints;
        audioSource = audio;
		PlayFireClip();
        Target = target;
        if (target.tag == "Ship")
        {
            Ship ship = Target.GetComponent<Ship>();
            TargetOffset = ship.GetDamageHardPoint();
            Target = ship.unitMeshObject;
        }
        else if(target.tag == "Station")
        {
            Station station = Target.GetComponent<Station>();
            TargetOffset = station.GetDamageHardPoint();
            Target = station.unitMeshObject;
        }
        else
        {
            TargetOffset = Vector3.zero;
        }
        Delay = baseWeapon.Delay * GetDelayModifier();
        if(baseWeapon.isBeam)
        {
            BeamDuration = baseWeapon.BeamDuration;
            beam = ResourceManager.CreateBeam(baseWeapon);
            beam.Initialize(this, parentUnit.unitMeshObject, GetWorldOffset(CurrentHardPoints[0]), Target, TargetOffset);
        }
        else
        {
            if (baseWeapon.MultiHardpoint)
            {
                foreach (Vector3 firePoint in CurrentHardPoints)
                {
					if (CheckPowerAndAmmo ())
                    {
						Projectile projectile = ResourceManager.CreateProjectile (baseWeapon);
						projectile.Initialize(this, baseWeapon, GetWorldOffset(firePoint), Target, TargetOffset);
						Projectiles.Add (projectile);
						ConsumePowerAndAmmo();
					} else
						return;
                }
            }
            else
            {
                for(int i = 0; i < baseWeapon.Projectiles; i++)
                {
					if (CheckPowerAndAmmo ())
                    {
						Projectile projectile = ResourceManager.CreateProjectile (baseWeapon);
						projectile.Initialize(this, baseWeapon, GetWorldOffset(CurrentHardPoints[0]), Target, TargetOffset);
						Projectiles.Add (projectile);
						ConsumePowerAndAmmo();
					} else
						return;
                }
            }
        }
        if(baseWeapon.SalvoSize > 1)
        {
            SalvoCount = baseWeapon.SalvoSize - 1;
            SalvoDelay = baseWeapon.SalvoDelay;
        }
    }

    void FireSalvo()
    {
        if (Target == null || !Target.activeSelf)
            return;

        if (baseWeapon.MultiHardpoint)
        {
            foreach (Vector3 firePoint in CurrentHardPoints)
            {
				if (CheckPowerAndAmmo())
                {
                    Projectile projectile = ResourceManager.CreateProjectile(baseWeapon);
                    projectile.Initialize(this, baseWeapon, GetWorldOffset(firePoint), Target, TargetOffset);
                    Projectiles.Add(projectile);
					ConsumePowerAndAmmo();
                }
                else
                {
                    SalvoCount = 0;
                    return;
                }
            }
        }
        else
        {
            for (int i = 0; i < baseWeapon.Projectiles; i++)
            {
				if (CheckPowerAndAmmo())
                {
                    Projectile projectile = ResourceManager.CreateProjectile(baseWeapon);
                    projectile.Initialize(this, baseWeapon, GetWorldOffset(CurrentHardPoints[0]), Target, TargetOffset);
                    Projectiles.Add(projectile);
					ConsumePowerAndAmmo();
                }
                else
                {
                    SalvoCount = 0;
                    return;
                }
            }
        }

        SalvoCount--;
        if (SalvoCount > 0)
            SalvoDelay = baseWeapon.SalvoDelay;

        if (!baseWeapon.SingleFireSound)
        {
			PlayFireClip();
        }
    }

    public void RemoveProjectile(Projectile projectile)
    {
        Projectiles.Remove(projectile);
    }

	ShipManager GetShipManager()
	{
		return parentUnit.GetShipManager();
	}

    public bool isEnemy(SpaceUnit target)
    {
        if (target == null)
            return false;
		if (GetShipManager () != null)
			return GetShipManager().isEnemy(target);
		return false;
    }

    public bool isEnemy(Projectile target)
    {
        if (target == null)
            return false;
        if (GetShipManager() != null)
            return GetShipManager().isEnemy(target);
        return false;
    }

    public float GetDamageBonus()
    {
		return parentUnit.GetDamageBonus();
    }

    public void Pause(bool state)
    {
        if(beam != null)
        {
            beam.Pause(state);
        }
        foreach(Projectile projectile in Projectiles)
        {
            if(projectile != null)
                projectile.Pause(state);
        }
    }

    public void DestroyBeam()
    {
        if(beam != null)
            beam.DestroySelf();
    }

    public void DestroyAllProjectiles()
    {
        foreach(Projectile projectile in Projectiles)
        {
            if(projectile != null)
                Object.Destroy(projectile.gameObject);
        }
        Projectiles.Clear();
    }

	void PlayFireClip()
	{
		if (baseWeapon.FireSound != "")
		{
            AudioManager.instance.PlayEffectClip(audioSource, baseWeapon.FireSound, false);
		}
	}

	void ConsumePowerAndAmmo()
    {
        parentUnit.ConsumePowerAndAmmo(baseWeapon.PowerCost, baseWeapon.AmmoCost);
    }

	bool ConsumePower()
	{
		return parentUnit.ConsumePower(baseWeapon.BeamPowerCost * GameManager.instance.GetDeltaTime());
	}

	bool CheckPowerAndAmmo()
	{
        return parentUnit.HasEnoughPowerAndAmmo(baseWeapon.PowerCost, baseWeapon.AmmoCost);
	}

    public void GetKill(SpaceUnit target)
    {
        parentUnit.GetKill(target);
    }

    public void RecordDamage(float damage)
    {
        parentUnit.RecordDamage(damage);
    }

    public void PartialBeamDelay()
    {
        Delay -= baseWeapon.Delay * (BeamDuration / baseWeapon.BeamDuration); 
    }

    Vector3 GetWorldOffset(Vector3 offset)
    {
        return parentUnit.transform.TransformPoint(offset);
    }

    public SpaceUnit GetParentUnit()
    {
        return parentUnit;
    }

    public QuadrantTypes GetFiringDirection()
    {
        return FiringQuadrant;
    }

    float GetDelayModifier()
    {
        return 1f + parentUnit.GetWeaponDelayModifier();
    }

    public void AddProjectile(Projectile projectile)
    {
        Projectiles.Add(projectile);
    }
}
