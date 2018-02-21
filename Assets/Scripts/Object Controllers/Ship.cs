/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes: Attaches to ship game object to define its behavior during battle instances
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections.Generic;

public sealed class Ship : MobileSpaceUnit
{
    #region Variables
    //Components
    ShipData shipData;
    HardPointsStored hardPoints;
    Material shipMaterial;    
	GameObject RangeCircle;

    //Orders
    public bool HoldFire = false;
    Quaternion goalRotation = Quaternion.identity; //Direction ship should face when at destination
	Ship TransportTargetTroops;
	Ship TransportTargetCrew;
	float transportTimer;

    //Fighters
    UnitWingsParentComponent unitWingsComponent;

    //Cloaking
    bool fullyCloaked = false;
    bool cloakingOut = false;
    bool cloakingIn = false;
    float cloakTimer = 0;

    //Dissolve effect
    float dissolveValue = 0;

    float RetreatTimer = 0;
    bool retreating = false;
    float SelfDestructTimer = 0;

    bool hasJamming;
    float JammingTimer;
    #endregion

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update ()
    {
        if (Destroyed)
        {
            dissolveValue += Time.deltaTime;
            currentMaterial.SetFloat("_Dissolve", dissolveValue);
        }
        else
        {
            HighlightUpdate();
            if (RangeCircle != null)
            {
                RangeCircle.transform.localScale = Vector3.one * shipData.GetAttackRange() / 4.6f;
            }
            if (!isPaused)
            {
                if (retreating)
                {
                    if(RetreatTimer > 0)
                    {
                        RetreatMove();
                        RetreatTimer -= GetDeltaTime();
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }
                else
                {
                    UpdateVelocity();
                    //SelfDesctruct
                    if (SelfDestructTimer > 0)
                    {
                        SelfDestructTimer -= GetDeltaTime();
                        if (textMeshController != null)
                            textMeshController.SetText(SelfDestructTimer.ToString("0.#"));
                        if (SelfDestructTimer <= 0)
                        {
                            shipData.SelfDestruct();
                        }
                    }
                    //Retreat
                    else if (RetreatTimer > 0)
                    {
                        RetreatTimer -= GetDeltaTime();
                        if(textMeshController != null)
                                textMeshController.SetText(RetreatTimer.ToString("0.#"));
                        if (RetreatTimer <= 0)
                        {
                            Retreat();
                        }
                    }
                    else
                    {
                        if (textMeshController != null)
                            Destroy(textMeshController.gameObject);
                    }
                    //Cloaking
                    if (cloakTimer > 0)
                    {
                        cloakTimer -= GetDeltaTime();
                        float ratio = Mathf.Max(cloakTimer / ResourceManager.instance.GetGameConstants().CloakingTime, ResourceManager.instance.GetGameConstants().CloakingTransparency);
                        if (cloakingOut)
                        {
                            ratio = 1f - ratio;
                        }
                        currentMaterial.SetFloat("_Fade", ratio);
                    }
                    else
                    {
                        if (cloakingIn)
                        {
                            cloakingIn = false;
                            fullyCloaked = true;
                        }
                        else if (cloakingOut)
                        {
                            cloakingOut = false;
                            MakeOpaque();
                        }
                    }

                    shipData.Update();
                    if (!Disabled && shipData.crew > 0)
                    {
                        //Jamming
                        if(hasJamming)
                        {
                            if(JammingTimer >  0)
                            {
                                JammingTimer -= GetDeltaTime();
                            }
                            else
                            {
                                JammingTimer = shipData.GetJammingDelay();
                                shipManager.JamEnemyProjectilesInRange(transform.position, shipData.GetJammingRangeSqr(), shipData.GetJammingCount());
                            }
                        }

                        //Transporting
                        if (TransportTargetTroops != null || TransportTargetCrew != null)
                        {
                            if (transportTimer > 0)
                            {
                                transportTimer -= GetDeltaTime();
                            }
                            else
                            {
                                transportTimer = 10f / (shipData.fleetData.GetTransport() + 1);
                                if (TransportTargetTroops != null)
                                    TransportTroopsToShip(TransportTargetTroops);
                                if (TransportTargetCrew != null)
                                    TransportCrewToShip(TransportTargetCrew);
                            }
                        }

                        if (Mobile)
                        {
                            if (Target != null)
                            {
                                if (TargetIsEnemy)
                                {
                                    if(shipData.AttackStyle != AttackStyle.none)
                                    EngageTarget();
                                }
                                else
                                    MoveTowardsGoal(Target.transform.position);
                            }
                            else if (ValidMoveOrder)
                            {
                                MoveTowardsGoal(goalPosition);
                            }
                            else
                            {
                                ApplyBrake();
                                transform.rotation = Quaternion.Lerp(transform.rotation, goalRotation, shipData.engineTurn * GetDeltaTime());
                                transform.GetChild(0).rotation = Quaternion.Slerp(transform.GetChild(0).rotation, transform.rotation, ResourceManager.instance.GetGameConstants().ShipTiltRate * GetDeltaTime());
                            }
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
                            WeaponCheckTimer = ResourceManager.instance.GetGameConstants().WeaponCheckTime;
                            if (!inCloakingState())
                            {
                                if (!HoldFire)
                                    CheckWeapons();
                                ApplyDamageOverlay(shipData.GetOuterHealthPercent());
                            }
                        }
                    }
                }
            }
        }
	}

	public void Initialize(ShipData data)
	{
		Initialize ();
        unitMeshObject = transform.GetChild(0).gameObject;
        shipData = data;
        hardPoints = data.designData.Hull.GetHardPoints();
        acceleration = shipData.engineThrust / shipData.mass;
		goalRotation = transform.rotation;
        currentMaterial = GetComponentInChildren<Renderer>().material;
        shipMaterial = currentMaterial;
        if (shipData.GetJammingCount() > 0 && shipData.GetJammingRange() > 0 && shipData.GetJammingDelay() > 0)
        {
            hasJamming = true;
            JammingTimer = shipData.GetJammingDelay();
        }
        hasPointDefense = shipData.hasPointDefense();
        unitWingsComponent = new UnitWingsParentComponent(this, data);
    }

	public void AttachRangeCircle()
	{
		if (RangeCircle != null)
			return;
		RangeCircle = ResourceManager.CreateRangeCircle ();
		RangeCircle.transform.position = transform.position;
        RangeCircle.transform.rotation = transform.rotation;
        RangeCircle.transform.parent = transform;
	}

	public void RemoveRangeCircle()
	{
		if (RangeCircle == null)
			return;
        Destroy(RangeCircle);
	}

    public ShipData GetShipData()
    {
        return shipData;
    }

    //Moves the ship towards the goal every tick
    protected override bool MoveTowardsGoal(Vector3 goal)
    {
        Vector3 Direction = (goal - transform.position).normalized;
        float Angle = Vector3.Angle(transform.forward, Direction);
        float goalDistance = Vector3.SqrMagnitude(goal - transform.position);

		//Check to see if ship is moving in wrong direction, and should apply brake
		float MovementAngle = Vector3.Angle(Direction, Velocity);
		if (MovementAngle > 60f) 
		{
			ApplyBrake ();
		}

        //Check to see if should move forward
        if (goalDistance > 1f)
        {
            RotateTowards(Direction, Angle > 15f);
            if (Angle < 15f)
            {
                MoveForwards();
            }
            return false;
        }
        else
        {
            ValidMoveOrder = false;
            return true;
        }
    }

    protected override void RotateTowards(Quaternion Rotation)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Rotation, GetTurnRate() * GetDeltaTime());

        unitMeshObject.transform.rotation = Quaternion.Slerp(transform.GetChild(0).rotation, transform.rotation, ResourceManager.instance.GetGameConstants().ShipTiltRate * GetDeltaTime());
    }

    void RotateTowards(Vector3 direction, bool tilt)
    {
        if (direction.sqrMagnitude > 0)
        {
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation =  Quaternion.Lerp(transform.rotation, rotation, shipData.engineTurn * GetDeltaTime());

            if(tilt)
            {
                Quaternion tiltRotation;
                Vector3 cross = Vector3.Cross(transform.forward, direction);
                if (cross.y < 0)
                {
                    tiltRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, shipData.designData.Hull.MaxTurnTilt);
                }
                else
                {
                    tiltRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, -shipData.designData.Hull.MaxTurnTilt);
                }
                unitMeshObject.transform.rotation = Quaternion.Slerp(unitMeshObject.transform.rotation, tiltRotation, ResourceManager.instance.GetGameConstants().ShipTiltRate * GetDeltaTime());
            }
        }
    }

    new void MoveForwards()
    {
		base.MoveForwards ();
        transform.GetChild(0).rotation = Quaternion.Slerp(transform.GetChild(0).rotation, transform.rotation, ResourceManager.instance.GetGameConstants().ShipTiltRate * GetDeltaTime());
    }

    void EngageTarget()
    {
        Vector3 Direction = (Target.transform.position - transform.position).normalized;
        float Angle = Vector3.Angle(transform.forward, Direction);
        float goalDistanceSqr = Vector3.SqrMagnitude(Target.transform.position - transform.position);

        if(goalDistanceSqr > shipData.GetAttackRangeSqr())
        {
			RotateTowards(Direction, Angle > 15f);
            if (Angle < 15f)
            {
                MoveForwards();
            }
        }
        else
        {
			if (shipData.AttackStyle == AttackStyle.holdPosition)
            {
				ApplyBrake ();
                Quaternion AttackRotation;

                if (shipData.AttackDirection == QuadrantTypes.Port)
                {
					AttackRotation = Quaternion.LookRotation (Direction, Vector3.up) * Quaternion.Euler (0, 90, 0);
				}
                else if (shipData.AttackDirection == QuadrantTypes.Starboard)
                {
					AttackRotation = Quaternion.LookRotation (Direction, Vector3.up) * Quaternion.Euler (0, 270, 0);
				}
                else
                {
                    //default to face target head on
                    AttackRotation = Quaternion.LookRotation(Direction, Vector3.up);                    
                    /*
                    RotateTowards (Direction, Angle > 15f);
					if (Angle < 15f)
                    {
						transform.rotation = Quaternion.Lerp (transform.rotation, goalRotation, shipData.engineTurn * GetDeltaTime());
						shipMeshObject.transform.rotation = Quaternion.Slerp (shipMeshObject.transform.rotation, transform.rotation, ResourceManager.instance.GetGameConstants().ShipTiltRate * GetDeltaTime());
					}*/
                }
                RotateTowards(AttackRotation);
            }
			else 
			{
                if (shipData.AttackDirection == QuadrantTypes.Port)
                {
                    Direction = Target.transform.position - transform.position + Vector3.Cross(Target.transform.position - transform.position, Vector3.up);
                    Angle = Vector3.Angle(transform.forward, Direction);
                }
                else if (shipData.AttackDirection == QuadrantTypes.Starboard)
                {
                    Direction = Target.transform.position - transform.position + Vector3.Cross(Target.transform.position - transform.position, Vector3.down);
                    Angle = Vector3.Angle(transform.forward, Direction);
                }
                if (Angle > 1f)
                    RotateTowards(Direction, Angle > 15f);
                MoveForwards();
			}
        }
    }

    public void SetNewGoalPosition(Vector3 point, Quaternion rotation)
    {
		SetNewGoalPosition (point);
        ResourceManager.instance.CreateClickPing(point);
        goalRotation = rotation;
		goalPosition = point;
    }

	public override void CancelOrders()
	{
		base.CancelOrders ();
		goalRotation = transform.rotation;
	}

    public new void SetPause(bool state)
    {
		base.SetPause (state);
        shipData.Pause(state);
    }

    /// <summary>
    /// Destroys ship object, and removes from ship manager and fleet
    /// </summary>
    public void DeleteSelf()
    {
        shipManager.DeleteShip(this);
    }

    public void Die()
    {
        Destroyed = true;
        shipManager.ShipKilled (this);
        MakeDissolve();
        ResourceManager.instance.CreateExplosion(transform.position, "effect_explosionship", "ShipExplosion", shipData.designData.Hull.ExplosionScale, 5f, true);     
        unitMeshObject.tag = "Exploder";
        Destroy(forceField.gameObject);
        unitMeshObject.transform.parent = null;
        SelfDestructor selfDestructor = unitMeshObject.AddComponent<SelfDestructor>();
        selfDestructor.lifeTimer = 2f;
        GameManager.instance.exploder.ExplodeObject(unitMeshObject);
        shipManager.RemoveFromSelection(this);
        gameObject.SetActive(false);
    }

    void CheckWeapons()
    {
        CacheTargets();

        if (!shipData.ForeSection.isDestroyed())
        {
            foreach (AttachedWeapon weapon in shipData.ForeSection.Weapons)
            {
                if (weapon.CanFire())
                {
                    CheckForTargets(weapon);
                }
            }
        }
        if (!shipData.AftSection.isDestroyed())
        {
            foreach (AttachedWeapon weapon in shipData.AftSection.Weapons)
            {
                if (weapon.CanFire())
                {
                    CheckForTargets(weapon);
                }
            }
        }
        if (!shipData.PortSection.isDestroyed())
        {
            foreach (AttachedWeapon weapon in shipData.PortSection.Weapons)
            {
                if (weapon.CanFire())
                {
                    CheckForTargets(weapon);
                }
            }
        }
        if (!shipData.StarboardSection.isDestroyed())
        {
            foreach (AttachedWeapon weapon in shipData.StarboardSection.Weapons)
            {
                if (weapon.CanFire())
                {
                    CheckForTargets(weapon);
                }
            }
        }
        foreach (AttachedWeapon weapon in shipData.CenterSection.Weapons)
        {
            if (weapon.CanFire())
            {
                CheckForTargets(weapon);
            }
        }
    }

    void CheckForTargets(AttachedWeapon weapon)
    {
        if (!weapon.baseWeapon.PointDefenseOnly)
        {
            //Check primary target validity
            if (Target != null && Target.CanBeTargeted() && isEnemy(Target) && inFiringArc(weapon, new TargetInfo(Target, MainTargetDirection, MainTargetRange)))
            {
                weapon.Fire(GetWeaponHardPoints(weapon), Target.gameObject, audioSource);
                return;
            }
            else //Check for targets of opportunity
            {
                foreach (TargetInfo potentialTarget in CachedTargets)
                {
                    if (inFiringArc(weapon, potentialTarget))
                    {
                        weapon.Fire(GetWeaponHardPoints(weapon), potentialTarget.Target.gameObject, audioSource);
                        return;
                    }
                }
                if (weapon.baseWeapon.PointDefense)
                {
                    foreach (PDTargetInfo potentialTarget in PDCachedTargets)
                    {
                        if (inFiringArc(weapon, potentialTarget))
                        {
                            weapon.Fire(GetWeaponHardPoints(weapon), potentialTarget.Target.gameObject, audioSource);
                            return;
                        }
                    }
                }
            }
        }
        else if (weapon.baseWeapon.PointDefense)
        {
            foreach (PDTargetInfo potentialTarget in PDCachedTargets)
            {
                if (inFiringArc(weapon, potentialTarget))
                {
                    weapon.Fire(GetWeaponHardPoints(weapon), potentialTarget.Target.gameObject, audioSource);
                    return;
                }
            }
        }
    }

    public bool inFiringArc(AttachedWeapon weapon, TargetInfo targetInfo)
    {
        //Check range in 2d
        if (weapon.baseWeapon.GetMaxRangeSqr() < targetInfo.RangeSqr)
            return false;
        Vector2 WeaponDirection;

        switch (weapon.GetFiringDirection())
        {
            case QuadrantTypes.Fore:
                {
                    WeaponDirection = new Vector2(transform.forward.x, transform.forward.z);
                    break;
                }
            case QuadrantTypes.Aft:
                {
                    WeaponDirection = new Vector2(-transform.forward.x, -transform.forward.z);
                    break;
                }
            case QuadrantTypes.Port:
                {
                    WeaponDirection = new Vector2(-transform.right.x, -transform.right.z);
                    break;
                }
            case QuadrantTypes.Starboard:
                {
                    WeaponDirection = new Vector2(transform.right.x, transform.right.z);
                    break;
                }
            default:
                {
                    WeaponDirection = new Vector2(transform.forward.x, transform.forward.z);
                    break;
                }
        }
        float targetAngle = Vector2.Angle(targetInfo.Direction, WeaponDirection);
        if (targetAngle > weapon.baseWeapon.GetHalfArc())
            return false;
        return true;
    }

    public bool inFiringArc(AttachedWeapon weapon, PDTargetInfo targetInfo)
    {
        //Check range in 2d
        if (weapon.baseWeapon.GetMaxRangeSqr() < targetInfo.RangeSqr)
            return false;
        Vector2 WeaponDirection;

        switch (weapon.GetFiringDirection())
        {
            case QuadrantTypes.Fore:
                {
                    WeaponDirection = new Vector2(transform.forward.x, transform.forward.z);
                    break;
                }
            case QuadrantTypes.Aft:
                {
                    WeaponDirection = new Vector2(-transform.forward.x, -transform.forward.z);
                    break;
                }
            case QuadrantTypes.Port:
                {
                    WeaponDirection = new Vector2(-transform.right.x, -transform.right.z);
                    break;
                }
            case QuadrantTypes.Starboard:
                {
                    WeaponDirection = new Vector2(transform.right.x, transform.right.z);
                    break;
                }
            default:
                {
                    WeaponDirection = new Vector2(transform.forward.x, transform.forward.z);
                    break;
                }
        }
        float targetAngle = Vector2.Angle(targetInfo.Direction, WeaponDirection);
        if (targetAngle > weapon.baseWeapon.GetHalfArc())
            return false;
        return true;
    }

    public Vector3 GetDamageHardPoint()
    {
        return hardPoints.GetRandomDamageOffset();
    }

    public Vector3[] GetWeaponHardPoints(AttachedWeapon weapon)
    {
        return hardPoints.GetRandomHardpoints(weapon.baseWeapon, weapon.GetFiringDirection());
    }

    public void TakeDamage(AttachedWeapon weapon, Vector3 hitPoint, Vector3 source, float damage, bool ignoreShields, bool ignoreArmor, bool ignoreArmorRating)
    {
        if(!ValidMoveOrder && Target == null)
        {
            SetNewTarget(weapon.GetParentUnit(), true);
        }

        Vector2 direction = new Vector2(source.x - transform.position.x, source.z - transform.position.z);

        if(inCloakingState())
        {
            ignoreShields = true;
        }

        QuadrantTypes quadHit = GetQuadrantDirection(direction);
        if(!ignoreShields)
            CheckShieldHit(hitPoint, quadHit);

        shipData.TakeDamage(weapon, hitPoint, damage, quadHit, ignoreShields, ignoreArmor, ignoreArmorRating);
    }

    public override void CheckShieldHit(Vector3 point, Vector2 origin)
    {
        if(inCloakingState())
        {
            return;
        }
        else if(shipData.HasCenterShields())
        {
            ShieldHit(point);
        }
        else
        {
            Vector2 direction = new Vector2(origin.x - transform.position.x, origin.y - transform.position.z);
            Vector2 forwardRotation = new Vector2(transform.forward.x, transform.forward.z);
            Vector2 portRotation = new Vector2(-transform.right.x, -transform.right.z);

            if (Vector2.Angle(forwardRotation, direction) <= 45f)
            {
                if(GetShipData().HasForeShields())
                {
                    ShieldHit(point);
                }
            }
            else if (Vector2.Angle(-forwardRotation, direction) <= 45f)
            {
                if (GetShipData().HasAftShields())
                {
                    ShieldHit(point);
                }
            }
            else if (Vector2.Angle(portRotation, direction) <= 45f)
            {
                if (GetShipData().HasPortShields())
                {
                    ShieldHit(point);
                }
            }
            else
            {
                if (GetShipData().HasStarboardShields())
                {
                    ShieldHit(point);
                }
            }
        }
    }

    void CheckShieldHit(Vector3 point, QuadrantTypes quad)
    {
        if (inCloakingState())
        {
            return;
        }
        else if (shipData.HasCenterShields())
        {
            ShieldHit(point);
        }
        else
        {
            if (quad == QuadrantTypes.Fore)
            {
                if (GetShipData().HasForeShields())
                {
                    ShieldHit(point);
                }
            }
            else if (quad == QuadrantTypes.Aft)
            {
                if (GetShipData().HasAftShields())
                {
                    ShieldHit(point);
                }
            }
            else if (quad == QuadrantTypes.Port)
            {
                if (GetShipData().HasPortShields())
                {
                    ShieldHit(point);
                }
            }
            else
            {
                if (GetShipData().HasStarboardShields())
                {
                    ShieldHit(point);
                }
            }
        }
    }

    public void MakeTransparent()
    {
        Material cloackedMaterial = ResourceManager.instance.GetCloackedShipMaterial();
        cloackedMaterial.SetTexture("_Normal", shipMaterial.GetTexture("_BumpMap"));
        GetComponentInChildren<Renderer>().material = cloackedMaterial;
        currentMaterial = cloackedMaterial;
    }

    public void MakeOpaque()
    {
        GetComponentInChildren<Renderer>().material = shipMaterial;
        currentMaterial = shipMaterial;
    }

    public void MakeDissolve()
    {
        Material dissolveMaterial = ResourceManager.instance.GetShipDissolveMaterial();
        dissolveMaterial.SetTexture("_MainTex", shipMaterial.GetTexture("_MainTex"));
        GetComponentInChildren<Renderer>().material = dissolveMaterial;
        currentMaterial = dissolveMaterial;
    }

    public void LaunchFighters()
    {
        unitWingsComponent.LaunchFighters();
    }

    public void LaunchHeavyFighters()
    {
        unitWingsComponent.LaunchHeavyFighters();
    }

    public void LaunchAssaultPods()
    {
        unitWingsComponent.LaunchAssaultPods();
    }

    public void RecallFighters()
    {
        unitWingsComponent.RecallFighters();
    }

    public void RecallHeavyFighters()
    {
        unitWingsComponent.RecallHeavyFighters();
    }

    public void RecallAssaultPods()
    {
        unitWingsComponent.RecallAssaultPods();
    }

    public void DockFighter(Fighter fighter)
    {
        unitWingsComponent.DockFighter(fighter);
    }

    public void RemoveFighterWing(FighterWing wing)
    {
        unitWingsComponent.RemoveFighterWing(wing);
    }

    public void UnParentFighterWings()
    {
        unitWingsComponent.UnParentFighterWings();
    }

	public void DestroyAllFighters()
	{
        unitWingsComponent.DestroyAllFighters();
    }

    public bool CanTransportTroops()
    {
        if (shipData.fleetData.GetTransport() == 0 || shipData.troops == 0)
            return false;
        return true;
    }

    public bool CanTransportCrew()
    {
        if (shipData.fleetData.GetTransport() == 0 || shipData.crew < (shipData.crewMax * ResourceManager.instance.GetGameConstants().MinCrewPercent) + 1)
            return false;
        return true;
    }

    public bool TransportingTroops()
    {
        return TransportTargetTroops != null;
    }

    public bool TransportingCrew()
    {
        return TransportTargetCrew != null;
    }

	public bool ValidTransporterTarget(ShipManager manager)
	{
		if (shipManager == manager)
			return true;
		if (manager.isEnemy(this) && shipData.HasShieldsDown())
			return true;
		return false;
	}

	public void SetTransportTroopTarget(Ship target)
	{
		TransportTargetTroops = target;
	}

    public void SetTransportCrewTarget(Ship target)
    {
        TransportTargetCrew = target;
    }

	public void CancelTransportingTroops()
	{
		TransportTargetTroops = null;
	}

	public void CancelTransportingCrew()
	{
		TransportTargetCrew = null;
	}

	public void TransportTroopsToShip(Ship targetShip)
	{
		if (shipData.troops > 0)
		{
			if (shipManager == targetShip.GetShipManager ()) 
			{
				if (targetShip.GetShipData().troops < targetShip.GetShipData().troopsMax) 
				{
					shipData.troops--;
					targetShip.GetShipData().troops++;
				}
				else 
				{
					CancelTransportingTroops();
					return;
				}
			}
			else
			{
				targetShip.AddBoardingForce(shipManager, 1, 0);
                shipData.troops--;
                ResourceManager.instance.CreatePopupMessage(targetShip.transform.position, "Troop Boarded", Color.green, 1.5f);
            }
		}
		else
			CancelTransportingTroops ();
	}

	public void TransportCrewToShip(Ship targetShip)
	{
		if (shipData.crew > shipData.crewMax * ResourceManager.instance.GetGameConstants().MinCrewPercent)
		{
			if (shipManager == targetShip.GetShipManager ()) 
			{
				if (targetShip.GetShipData ().crew < targetShip.GetShipData ().crewMax) 
				{
					shipData.crew--;
					targetShip.GetShipData ().crew++;
				}
				else 
				{
					CancelTransportingCrew ();
					return;
				}
			}
			else
			{
				targetShip.AddBoardingForce (shipManager, 0, 1);
                shipData.crew--;
                ResourceManager.instance.CreatePopupMessage(targetShip.transform.position, "Crew Boarded", Color.cyan, 1.5f);
            }
		}
		else
			CancelTransportingCrew ();
	}

	public void AddBoardingForce(ShipManager manager, int Troops, int Crew)
	{
        shipData.AddBoardingForce(manager, Troops, Crew);
	}

    public bool CanCloak()
    {
        return GetShipData().designData.CloakingPower > 0;
    }

    public bool IsCloaked()
    {
        return fullyCloaked;
    }

    public bool inCloakingState()
    {
        return fullyCloaked || cloakingIn || cloakingOut;
    }

    public void Cloak()
    {
        cloakingIn = true;
        cloakingOut = false;
        cloakTimer = ResourceManager.instance.GetGameConstants().CloakingTime;
        MakeTransparent();
        shipData.DestroyAllBeams();
        PlaySound("Cloaking");
    }

    public void DeCloak()
    {
        if (!cloakingOut)
        {
            cloakingOut = true;
            cloakTimer = ResourceManager.instance.GetGameConstants().CloakingTime;
        }
        cloakingIn = false;
        fullyCloaked = false;
        PlaySound("Cloaking");
    }

	protected override float GetMaxSpeed()
	{
		return shipData.engineThrust * shipData.GetcrewEfficiency();
	}

	protected override float GetTurnRate()
	{
		return shipData.engineTurn * shipData.GetcrewEfficiency();
	}

    void ApplyDamageOverlay(float percentage)
    {
        currentMaterial.SetFloat("_Damage", percentage);
    }

    public void SetSelfDestructTime(float time)
    {
        SelfDestructTimer = time;
        if (time > 0)
        {
            if (textMeshController != null)
                textMeshController.SetColor(Color.red);
            else
                AttachTextMeshController(SelfDestructTimer.ToString("0.#"), Color.red);
        }
    }

    public bool SelfDestructTimerRunning()
    {
        return SelfDestructTimer > 0;
    }

    public void SetRetreatTimer(float time)
    {
        RetreatTimer = time;
        if (time > 0)
        {
            if (textMeshController != null)
                textMeshController.SetColor(Color.white);
            else
                AttachTextMeshController(RetreatTimer.ToString("0.#"), Color.white);
        }
    }

    public bool RetreatTimerRunning()
    {
        return RetreatTimer > 0;
    }

    void Retreat()
    {
        shipData.Retreated = true;
        retreating = true;
        RetreatTimer = 7f;
        shipManager.DeselectShip(this);
        PlaySound("FTL"); 
    }

    void RetreatMove()
    {
        transform.position += transform.forward * GetDeltaTime() * 75f;
    }

    protected override float GetMaxWeaponRangeSqr()
    {
        return Mathf.Pow(GetShipData().designData.maxRange, 2f);
    }

    public void DestroyWeaponEffects()
    {
        shipData.DestroyWeaponEffects();
    }

    public bool HasFighters()
    {
        return shipData.HasFighters();
    }

    public bool HasLaunchedFighters()
    {
        return unitWingsComponent.hasLaunchedFighters();
    }

    public bool HasHeavyFighters()
    {
        return shipData.HasHeavyFighters();
    }

    public bool HasLaunchedHeavyFighters()
    {
        return unitWingsComponent.hasLaunchedHeavyFighters();
    }

    public bool HasAssaultPods()
    {
        return shipData.HasAssaultPods();
    }

    public bool HasLaunchedAssaultPods()
    {
        return unitWingsComponent.hasLaunchedAssaultPods();
    }

    public ShipHullData GetHull()
    {
        return shipData.designData.Hull;
    }

    public override bool HasEnoughPowerAndAmmo(float power, float ammo)
    {
        if (power > shipData.power)
            return false;
        return shipData.CanGetAmmo(ammo);
    }

    public override bool CanGetAmmo(float ammo)
    {
        return shipData.CanGetAmmo(ammo);
    }

    public override float GetDamageBonus()
    {
        return shipData.GetDamageBonus();
    }

    public override void ConsumePowerAndAmmo(float power, float ammo)
    {
        shipData.ConsumePowerAndAmmo(power, ammo);
    }

    public override bool ConsumePower(float power)
    {
        return shipData.ConsumePower(power);
    }

    public override void GetKill(SpaceUnit killedUnit)
    {
        if (killedUnit is Fighter)
        {
            shipData.AddExperience((killedUnit as Fighter).GetKillExperience());
        }
        else
        {
            if (killedUnit is Ship)
            {
                shipData.GetKill((killedUnit as Ship).GetShipData());
            }
            else if (killedUnit is Station)
            {
                shipData.GetKill((killedUnit as Station).stationData);
            }
        }
    }

    public override void RecordDamage(float amount)
    {
        shipData.damageDealt += amount;
    }

    public override float GetWeaponDelayModifier()
    {
        return 1f - shipData.GetcrewEfficiency();
    }
}
