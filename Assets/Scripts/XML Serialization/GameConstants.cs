/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

[System.Serializable]
public class GameConstants
{
    #region Variables

    // Time
    public float dayTime = 1f;

    //Economy
    [SerializeField]
    public float BaseFoodValue { get; set; }
    public float BaseProductionValue { get; set; }
    public float BaseOreValue { get; set; }
    public float BaseAlloyValue { get; set; }
    public float BaseRareOreValue { get; set; }
    public float BaseAdvancedAlloyValue { get; set; }
    public float BaseExoticOreValue { get; set; }
    public float BaseSuperiorAlloyValue { get; set; }
    public float BaseCrystalValue { get; set; }
    public float BaseRareCrystalValue { get; set; }
    public float BaseExoticCrystalValue { get; set; }
    public float BaseExoticParticleValue { get; set; }

    //Highlight colors
    public ColorXML Highlight_Player { get; set; }
    public ColorXML Highlight_Enemy { get; set; }
    public ColorXML Highlight_Ally { get; set; }
    public ColorXML Highlight_Neutral { get; set; }

    //Ships
    public float ShipSTLSpeedMultiplier { get; set; }
    public float ShipSTLTurnMultiplier { get; set; }
    public float FighterTurnMultiplier { get; set; }
    public float ShipTiltRate { get; set; }
    public float ShipDrag { get; set; }
    public float ShipBrakeDrag { get; set; }
    public float FiringRangeFactor { get; set; }
    public bool AllowCombatRepair { get; set; }
    public float MinShieldAbsorb { get; set; }
    public float CloakingTime { get; set; }
    public float CloakingTransparency { get; set; }
    public float boardingIntervalMax { get; set; }
    public float boardingIntervalMin { get; set; }
    public float MinCrewPercent { get; set; }
    public float RetreatTime { get; set; }
    public float SelfDestructTime { get; set; }
    public float WeaponCheckTime { get; set; }
    public float BeamDamageIntervalTime { get; set; }
    public float ShipLevelDamageBonus { get; set; }
    public float ShipLevelDefenseBonus { get; set; }

    //Camera
    public float CameraSpeed { get; set; }
    public int CameraLimitUp { get; set; }
    public int CameraLimitDown { get; set; }
    public int CameraLimitLeft { get; set; }
    public int CameraLimitRight { get; set; }
    public float CameraZoomRate { get; set; }
    public int CameraLimitZoomMax { get; set; }
    public int CameraLimitZoomMin { get; set; }
    public int CameraBoarderArea { get; set; }
    public float CameraRotateRate { get; set; }

    #endregion

    public GameConstants()
    {
        BaseFoodValue = -1;
        BaseProductionValue = -1;
        BaseOreValue = -1;
        BaseAlloyValue = -1;
        BaseRareOreValue = -1;
        BaseAdvancedAlloyValue = -1;
        BaseExoticOreValue = -1;
        BaseSuperiorAlloyValue = -1;
        BaseCrystalValue = -1;
        BaseRareCrystalValue = -1;
        BaseExoticCrystalValue = -1;
        BaseExoticParticleValue = -1;

        CameraSpeed = 10;
        CameraLimitUp = 200;
        CameraLimitDown = -200;
        CameraLimitLeft = -200;
        CameraLimitRight = 200;
        CameraZoomRate = 10;
        CameraLimitZoomMax = 30;
        CameraLimitZoomMin = 1;
        CameraBoarderArea = 5;
        CameraRotateRate = 3;
    }

    /// <summary>
    /// Takes in a new GameConstants and determines what needs to be updated
    /// </summary>
    /// <param name="newConstants"></param>
    public void UpdateConstants(GameConstants newConstants)
    {

    }

    public float GetBaseResourceValue(float production, float alloy, float advAlloy, float supAlloy, float crystal, float rareCrystal, float exCrystal, float exParticle)
    {
        return GetBaseProductionValue(production) + GetBaseAlloyValue(alloy) + GetBaseAdvAlloyValue(advAlloy) + GetBaseSupAlloyValue(supAlloy) + GetBaseCrystalValue(crystal) + GetBaseRareCrystalValue(rareCrystal) + GetBaseExoticCrystalValue(exCrystal) + GetBaseExoticParticleValue(exParticle);
    }

    public float GetBaseProductionValue(float Production)
    {
        return Production * BaseProductionValue;
    }

    public float GetBaseAlloyValue(float Alloy)
    {
        return Alloy * BaseAlloyValue;
    }

    public float GetBaseAdvAlloyValue(float AdvAlloy)
    {
        return AdvAlloy * BaseAdvancedAlloyValue;
    }

    public float GetBaseSupAlloyValue(float SupAlloy)
    {
        return SupAlloy * BaseSuperiorAlloyValue;
    }

    public float GetBaseCrystalValue(float Crystal)
    {
        return Crystal * BaseCrystalValue;
    }

    public float GetBaseRareCrystalValue(float RareCrystal)
    {
        return RareCrystal * BaseRareCrystalValue;
    }

    public float GetBaseExoticCrystalValue(float ExoticCrystal)
    {
        return ExoticCrystal * BaseExoticCrystalValue;
    }

    public float GetBaseExoticParticleValue(float ExoticParticle)
    {
        return ExoticParticle * BaseExoticParticleValue;
    }
}
