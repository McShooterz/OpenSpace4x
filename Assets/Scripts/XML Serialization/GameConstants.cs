/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class GameConstants
{
    //Game Settings
    public float ScreenTransitionRate = 3;

    //
    public int BaseFleetCommandPointLimit = -1;

    //Economy
    public float BaseFoodValue = -1;
    public float BaseProductionValue = -1;
    public float BaseOreValue = -1;
    public float BaseAlloyValue = -1;
    public float BaseRareOreValue = -1;
    public float BaseAdvancedAlloyValue = -1;
    public float BaseExoticOreValue = -1;
    public float BaseSuperiorAlloyValue = -1;
    public float BaseCrystalValue = -1;
    public float BaseRareCrystalValue = -1;
    public float BaseExoticCrystalValue = -1;
    public float BaseExoticParticleValue = -1;

    //Highlight colors
    public ColorXML Highlight_Player;
    public ColorXML Highlight_Enemy;
    public ColorXML Highlight_Ally;
	public ColorXML Highlight_Neutral;

    //Ships
    public float ShipSTLSpeedMultiplier;
    public float ShipSTLTurnMultiplier;
	public float FighterTurnMultiplier;
    public float ShipTiltRate;
    public float ShipDrag;
	public float ShipBrakeDrag;
    public float FiringRangeFactor;
    public bool AllowCombatRepair;
    public float MinShieldAbsorb;
    public float CloakingTime;
    public float CloakingTransparency;
	public float boardingIntervalMax;
    public float boardingIntervalMin;
    public float MinCrewPercent;
    public float RetreatTime;
    public float SelfDestructTime;
    public float WeaponCheckTime;
    public float BeamDamageIntervalTime;
    public float ShipLevelDamageBonus;
    public float ShipLevelDefenseBonus;

    //Camera
    public float CameraSpeed = 10;
    public int CameraLimitUp = 200;
    public int CameraLimitDown = -200;
    public int CameraLimitLeft = -200;
    public int CameraLimitRight = 200;
    public float CameraZoomRate = 10;
    public int CameraLimitZoomMax = 30;
    public int CameraLimitZoomMin = 1;
    public int CameraBoarderArea = 5;
    public float CameraRotateRate = 3;

    public GameConstants() { }

    public void UpdateConstants(GameConstants newConstants)
    {
        if(newConstants.BaseFleetCommandPointLimit != -1)
        {
            BaseFleetCommandPointLimit = newConstants.BaseFleetCommandPointLimit;
        }
        if (newConstants.BaseFoodValue != -1)
        {
            BaseFoodValue = newConstants.BaseFoodValue;
        }
        if (newConstants.BaseProductionValue != -1)
        {
            BaseProductionValue = newConstants.BaseProductionValue;
        }
        if (newConstants.BaseOreValue != -1)
        {
            BaseOreValue = newConstants.BaseOreValue;
        }
        if (newConstants.BaseAlloyValue != -1)
        {
            this.BaseAlloyValue = newConstants.BaseAlloyValue;
        }
        if (newConstants.BaseRareOreValue != -1)
        {
            this.BaseRareOreValue = newConstants.BaseRareOreValue;
        }
        if (newConstants.BaseAdvancedAlloyValue != -1)
        {
            this.BaseAdvancedAlloyValue = newConstants.BaseAdvancedAlloyValue;
        }
        if (newConstants.BaseExoticOreValue != -1)
        {
            this.BaseExoticOreValue = newConstants.BaseExoticOreValue;
        }
        if (newConstants.BaseSuperiorAlloyValue != -1)
        {
            this.BaseSuperiorAlloyValue = newConstants.BaseSuperiorAlloyValue;
        }
        if (newConstants.BaseCrystalValue != -1)
        {
            this.BaseCrystalValue = newConstants.BaseCrystalValue;
        }
        if (newConstants.BaseRareCrystalValue != -1)
        {
            this.BaseRareCrystalValue = newConstants.BaseRareCrystalValue;
        }
        if (newConstants.BaseExoticCrystalValue != -1)
        {
            this.BaseExoticCrystalValue = newConstants.BaseExoticCrystalValue;
        }
        if (newConstants.BaseExoticParticleValue != -1)
        {
            this.BaseExoticParticleValue = newConstants.BaseExoticParticleValue;
        }
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

    public float GetBaseExoticCrystalValue(float ExoticCrytsl)
    {
        return ExoticCrytsl * BaseExoticCrystalValue;
    }

    public float GetBaseExoticParticleValue(float ExoticParticle)
    {
        return ExoticParticle * BaseExoticParticleValue;
    }
}
