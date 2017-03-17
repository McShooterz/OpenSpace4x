/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections.Generic;

public sealed class Station : SpaceUnit
{
    #region Variables
    public StationData stationData;
    HardPointsStored hardPoints;
    Material shipMaterial;

    //Orders
    public bool HoldFire = false;
    Ship TransportTargetTroops;
    Ship TransportTargetCrew;
    float transportTimer;

    //Fighters
    UnitWingsParentComponent unitWingsComponent;

    //Boarding
    List<BoardingForce> boardingForces = new List<BoardingForce>();

    //Dissolve effect
    float dissolveValue = 0;

    float SelfDestructTimer = 0;

    bool hasJamming;
    float JammingTimer;
    #endregion

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (Destroyed)
        {
            dissolveValue += Time.deltaTime;
            currentMaterial.SetFloat("_Dissolve", dissolveValue);
        }
        else
        {
            HighlightUpdate();
            ApplyDamageOverlay(stationData.GetHealthPercentOverlay());
            if (!isPaused)
            {
                //SelfDesctruct
                if (SelfDestructTimer > 0)
                {
                    SelfDestructTimer -= GetDeltaTime();
                    if (textMeshController != null)
                        textMeshController.SetText(SelfDestructTimer.ToString("0.#"));
                    if (SelfDestructTimer <= 0)
                    {
                        stationData.SelfDestruct();
                    }
                }
                else
                {
                    if (textMeshController != null)
                        Destroy(textMeshController.gameObject);
                }

                stationData.Update();
                if (!Disabled && stationData.crew > 0)
                {
                    //Jamming
                    if (hasJamming)
                    {
                        if (JammingTimer > 0)
                        {
                            JammingTimer -= GetDeltaTime();
                        }
                        else
                        {
                            JammingTimer = stationData.GetJammingDelay();
                            shipManager.JamEnemyProjectilesInRange(transform.position, stationData.GetJammingRangeSqr(), stationData.GetJammingCount());
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
                            transportTimer = 10f / (stationData.fleetData.GetTransport() + 1);
                            if (TransportTargetTroops != null)
                                TransportTroopsToShip(TransportTargetTroops);
                            if (TransportTargetCrew != null)
                                TransportCrewToShip(TransportTargetCrew);
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
                        WeaponCheckTimer = ResourceManager.gameConstants.WeaponCheckTime;
                        if (!HoldFire)
                        {
                            CheckWeapons();
                        }
                    }
                }
            }
        }
    }

    public void Initialize(StationData data)
    {
        Initialize();
        stationData = data;
        hardPoints = stationData.designData.Hull.GetHardPoints();
        unitMeshObject = transform.GetChild(0).gameObject;
        currentMaterial = GetComponentInChildren<Renderer>().material;
        shipMaterial = currentMaterial;
        if (stationData.GetJammingCount() > 0 && stationData.GetJammingRange() > 0 && stationData.GetJammingDelay() > 0)
        {
            hasJamming = true;
            JammingTimer = stationData.GetJammingDelay();
        }
        hasPointDefense = stationData.hasPointDefense();
        unitWingsComponent = new UnitWingsParentComponent(this, data);
    }

    public new void SetPause(bool state)
    {
        base.SetPause(state);
        stationData.Pause(state);
    }

    /// <summary>
    /// Destroys station object, and removes from ship manager and fleet
    /// </summary>
    public void DeleteSelf()
    {
        shipManager.DeleteStation(this);  
    }

    public void Die()
    {
        MakeDissolve();
        Destroyed = true;
        ResourceManager.CreateExplosion(transform.position, "effect_explosionship", "ShipExplosion", 2.5f, 5f, true);
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
        foreach (AttachedWeapon weapon in stationData.Weapons)
        {
            if (weapon.CanFire())
            {
                if (!weapon.baseWeapon.PointDefenseOnly)
                {
                    //Check primary target validity
                    if (Target != null && Target.CanBeTargeted() && isEnemy(Target) && MainTargetRange < weapon.baseWeapon.GetMaxRangeSqr())
                    {
                        Vector2 direction = new Vector2(Target.transform.position.x - transform.position.x, Target.transform.position.z - transform.position.z);
                        weapon.Fire(GetWeaponHardPoints(weapon, direction), Target.gameObject, audioSource);
                        return;
                    }
                    else //Check for targets of opportunity
                    {
                        foreach (TargetInfo potentialTarget in CachedTargets)
                        {
                            if (potentialTarget.RangeSqr < weapon.baseWeapon.GetMaxRangeSqr())
                            {
                                weapon.Fire(GetWeaponHardPoints(weapon, potentialTarget.Direction), potentialTarget.Target.gameObject, audioSource);
                                return;
                            }
                        }
                        if (weapon.baseWeapon.PointDefense)
                        {
                            foreach (PDTargetInfo potentialTarget in PDCachedTargets)
                            {
                                if (potentialTarget.RangeSqr < weapon.baseWeapon.GetMaxRangeSqr())
                                {
                                    weapon.Fire(GetWeaponHardPoints(weapon, potentialTarget.Direction), potentialTarget.Target.gameObject, audioSource);
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
                        if (potentialTarget.RangeSqr < weapon.baseWeapon.GetMaxRangeSqr())
                        {
                            weapon.Fire(GetWeaponHardPoints(weapon, potentialTarget.Direction), potentialTarget.Target.gameObject, audioSource);
                            return;
                        }
                    }
                }
            }
        }
    }

    public Vector3 GetDamageHardPoint()
    {
        return hardPoints.GetRandomDamageOffset();
    }

    public Vector3[] GetWeaponHardPoints(AttachedWeapon weapon, Vector2 Direct)
    {
        return hardPoints.GetRandomHardpoints(weapon.baseWeapon, GetQuadrantDirection(Direct));
    }

    public void TakeDamage(AttachedWeapon weapon, Vector3 hitPoint, Vector3 source, float damage, bool ignoreShields, bool ignoreArmor, bool ignoreArmorRating)
    {
        if (!ignoreShields)
            CheckShieldHit(hitPoint, Vector2.zero);

        stationData.TakeDamage(weapon, hitPoint, damage, ignoreShields, ignoreArmor, ignoreArmorRating);
    }

    public override void CheckShieldHit(Vector3 point, Vector2 origin)
    {
        if (stationData.shieldHealth > 0)
        {
            ShieldHit(point);
        }
    }

    public void MakeDissolve()
    {
        Material dissolveMaterial = ResourceManager.GetShipDissolveMaterial();
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
        if (stationData.fleetData.GetTransport() == 0 || stationData.troops == 0)
            return false;
        return true;
    }

    public bool CanTransportCrew()
    {
        if (stationData.fleetData.GetTransport() == 0 || stationData.crew < (stationData.crewMax * ResourceManager.gameConstants.MinCrewPercent) + 1)
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
        if (manager.isEnemy(this) && stationData.shieldHealth < 0)
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
        if (stationData.troops > 0)
        {
            if (shipManager == targetShip.GetShipManager())
            {
                if (targetShip.GetShipData().troops < targetShip.GetShipData().troopsMax)
                {
                    stationData.troops--;
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
                stationData.troops--;
                ResourceManager.CreatePopupMessage(targetShip.transform.position, "Troop Boarded", Color.green, 1.5f);
            }
        }
        else
            CancelTransportingTroops();
    }

    public void TransportCrewToShip(Ship targetShip)
    {
        if (stationData.crew > stationData.crewMax * ResourceManager.gameConstants.MinCrewPercent)
        {
            if (shipManager == targetShip.GetShipManager())
            {
                if (targetShip.GetShipData().crew < targetShip.GetShipData().crewMax)
                {
                    stationData.crew--;
                    targetShip.GetShipData().crew++;
                }
                else
                {
                    CancelTransportingCrew();
                    return;
                }
            }
            else
            {
                targetShip.AddBoardingForce(shipManager, 0, 1);
                stationData.crew--;
                ResourceManager.CreatePopupMessage(targetShip.transform.position, "Crew Boarded", Color.cyan, 1.5f);
            }
        }
        else
            CancelTransportingCrew();
    }

    public void AddBoardingForce(ShipManager manager, int Troops, int Crew)
    {
        stationData.AddBoardingForce(manager, Troops, Crew);
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

    protected override float GetMaxWeaponRangeSqr()
    {
        return Mathf.Pow(stationData.designData.maxRange, 2f);
    }

    public bool HasFighters()
    {
        return stationData.HasFighters();
    }

    public bool HasLaunchedFighters()
    {
        return unitWingsComponent.hasLaunchedFighters();
    }

    public bool HasHeavyFighters()
    {
        return stationData.HasHeavyFighters();
    }

    public bool HasLaunchedHeavyFighters()
    {
        return unitWingsComponent.hasLaunchedHeavyFighters();
    }

    public bool HasAssaultPods()
    {
        return stationData.HasAssaultPods();
    }

    public bool HasLaunchedAssaultPods()
    {
        return unitWingsComponent.hasLaunchedAssaultPods();
    }

    public StationHullData GetHull()
    {
        return stationData.designData.Hull;
    }

    public override bool HasEnoughPowerAndAmmo(float power, float ammo)
    {
        if (power > stationData.power)
            return false;
        return stationData.CanGetAmmo(ammo);
    }

    public override bool CanGetAmmo(float ammo)
    {
        return stationData.CanGetAmmo(ammo);
    }

    public override float GetDamageBonus()
    {
        return stationData.GetDamageBonus();
    }

    public override void ConsumePowerAndAmmo(float power, float ammo)
    {
        stationData.ConsumePowerAndAmmo(power, ammo);
    }

    public override bool ConsumePower(float power)
    {
        return stationData.ConsumePower(power);
    }

    public override void GetKill(SpaceUnit killedUnit)
    {
        if (killedUnit is Ship)
        {
            stationData.GetKill((killedUnit as Ship).GetShipData());
        }
        else if (killedUnit is Station)
        {
            stationData.GetKill((killedUnit as Station).stationData);
        }
    }

    public override void RecordDamage(float amount)
    {
        stationData.damageDealt += amount;
    }

    public override float GetWeaponDelayModifier()
    {
        return 1f - stationData.GetcrewEfficiency();
    }
}
