/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModulePanel : MonoBehaviour
{
    #region Variables

    [SerializeField]
    ModuleSetScrollList moduleSetScrollList;

    [SerializeField]
    ModuleButtonList moduleButtonList;

    [SerializeField]
    StatEntryList ModuleStatEntries;

    [SerializeField]
    WeaponDamageGraph weaponDamageGraph;

    List<ModuleSet> WeaponModuleSets = new List<ModuleSet>();
    List<ModuleSet> DefenseModuleSets = new List<ModuleSet>();
    List<ModuleSet> SystemModuleSets = new List<ModuleSet>();

    ModuleSet SelectedModuleSet;

    Module SelectedModule;

    ModuleCategory Category = ModuleCategory.Weapons;

    bool useSymmetricCursor = false;

    Rect SelectedModuleCursorRect = new Rect();
    Texture2D SelectedModuleTexture;

    float ModuleScale = 32;
    int ModuleRotation = 0;

    #endregion

    // Use this for initialization
    void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetMouseButtonDown(1))
        {
            DeselectModule();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            RotateModule();
        }

        if(Input.GetKeyDown(KeyCode.M))
        {
            ToggleMirrorMode();
        }

        if(Input.GetKeyDown(KeyCode.S))
        {

        }
    }

    void OnGUI()
    {
        //DrawSelectedModuleTexture();
    }

    public void SetWeaponModuleSetList(List<ModuleSet> moduleSetList)
    {
        WeaponModuleSets = moduleSetList;
    }

    public void SetDefenseModuleSetList(List<ModuleSet> moduleSetList)
    {
        DefenseModuleSets = moduleSetList;
    }

    public void SetSystemModuleSetList(List<ModuleSet> moduleSetList)
    {
        SystemModuleSets = moduleSetList;
    }

    public void BuildModuleSetButtons(List<ModuleSet> moduleSetList)
    {
        moduleSetScrollList.BuildModuleSetsButtons(moduleSetList, ChangeModuleSet);
    }

#region Button Clicks

    public void ClickWeaponsCategory()
    {
        if(Category != ModuleCategory.Weapons)
        {
            Category = ModuleCategory.Weapons;
            BuildModuleSetButtons(WeaponModuleSets);
        }
    }

    public void ClickDefenseCategory()
    {
        if (Category != ModuleCategory.Defenses)
        {
            Category = ModuleCategory.Defenses;
            BuildModuleSetButtons(DefenseModuleSets);
        }
    }

    public void ClickSystemsCategory()
    {
        if (Category != ModuleCategory.Systems)
        {
            Category = ModuleCategory.Systems;
            BuildModuleSetButtons(SystemModuleSets);
        }
    }

    public void ClickMirrorButton()
    {
        ToggleMirrorMode();
    }

    public void ClickRotateButton()
    {
        RotateModule();
    }

    public void ClickSwapButton()
    {

    }

#endregion

    void ClearModuleSetButtons()
    {
        moduleSetScrollList.Clear();
    }

    public Module GetSelectedModule()
    {
        return SelectedModule;
    }

    public ModuleSet GetSelectedModuleSet()
    {
        return SelectedModuleSet;
    }

    public void DeselectModule()
    {
        SelectedModule = null;

        moduleButtonList.DeselectModuleButton();

        ModuleStatEntries.Clear();
    }

    public void ChangeModuleSet(ModuleSet moduleSet)
    {
        SelectedModuleSet = moduleSet;
        moduleButtonList.CreateModuleButtons(moduleSet.GetModules(), ChangeModule);
    }

    public void ChangeModule(Module module)
    {
        SelectedModule = module;

        ModuleRotation = 0;

        SelectedModuleTexture = SelectedModule.GetTexture();

        ApplyModuleScale();

        AddModuleStatEntries(SelectedModule);
    }

    void AddModuleStatEntries(Module module)
    {
        ModuleStatEntries.Clear();

        float cost = module.GetCost();
        if (cost != 0)
        {
            AddModuleStatEntry("Icon_Money", cost.ToString("0.##"), "moduleCost", "moduleCostDesc");
        }
        if (module.ProductionCost != 0)
        {
            AddModuleStatEntry("Icon_Production", module.ProductionCost.ToString("0.##"), "productionCost", "productionCostDesc");
        }
        if (module.AlloyCost != 0)
        {
            AddModuleStatEntry("Icon_Alloy", module.AlloyCost.ToString("0.##"), "alloyCost", "alloyCostDesc");
        }
        if (module.AdvancedAlloyCost != 0)
        {
            AddModuleStatEntry("Icon_AlloyAdvanced", module.AdvancedAlloyCost.ToString("0.##"), "advancedAlloyCost", "advancedAlloyCostDesc");
        }
        if (module.SuperiorAlloyCost != 0)
        {
            AddModuleStatEntry("Icon_AlloySuperior", module.SuperiorAlloyCost.ToString("0.##"), "superiorAllorCost", "superiorAllorCostDesc");
        }
        if (module.CrystalCost != 0)
        {
            AddModuleStatEntry("Icon_Crystal", module.CrystalCost.ToString("0.##"), "crystalCost", "crystalCostDesc");
        }
        if (module.RareCrystalCost != 0)
        {
            AddModuleStatEntry("Icon_CrystalRare", module.RareCrystalCost.ToString("0.##"), "rareCrystalCost", "rareCrystalCostDesc");
        }
        if (module.ExoticCrystalCost != 0)
        {
            AddModuleStatEntry("Icon_CrystalExotic", module.ExoticCrystalCost.ToString("0.##"), "exoticCrystalCost", "exoticCrystalCostDesc");
        }
        if (module.ExoticParticleCost != 0)
        {
            AddModuleStatEntry("Icon_ParticleExotic", module.ExoticParticleCost.ToString("0.##"), "exoticParticleCost", "exoticParticleCostDesc");
        }
        if (module.Health != 0)
        {
            AddModuleStatEntry("Icon_Health", module.Health.ToString("0.##"), "health", "modHealth");
        }
        if (module.Mass != 0)
        {
            AddModuleStatEntry("Icon_Mass", module.Mass.ToString("0.##"), "mass", "modMass");
        }
        if (module.Power != 0)
        {
            AddModuleStatEntry("Icon_Power", module.Power.ToString("0.##"), "power", "modPower");
        }
        if (module.PowerGenerated != 0)
        {
            AddModuleStatEntry("Icon_PowerGenerated", module.PowerGenerated.ToString("0.##"), "powerGenerated", "powerGeneratedDesc");
        }
        if (module.PowerEfficiency != 0)
        {
            AddModuleStatEntry("Icon_PowerEfficiency", "%" + (module.PowerEfficiency * 100).ToString("0.##"), "powerEfficiency", "powerEfficiencyDesc");
        }
        if (module.Fuel != 0)
        {
            AddModuleStatEntry("Icon_Fuel", module.Fuel.ToString("0.##"), "fuel", "modFuel");
        }
        if (module.ArmorHealth != 0)
        {
            AddModuleStatEntry("Icon_ArmorHealth", module.ArmorHealth.ToString("0.##"), "armorHealth", "modArmorHealth");
        }
        if (module.ArmorRating != 0)
        {
            AddModuleStatEntry("Icon_ArmorRating", module.ArmorRating.ToString("0.##"), "armorRating", "modArmorRating");
        }
        if (module.ShieldHealth != 0)
        {
            AddModuleStatEntry("Icon_ShieldHealth", module.ShieldHealth.ToString("0.##"), "shieldHealth", "modShieldHealth");
        }
        if (module.ShieldRating != 0)
        {
            AddModuleStatEntry("Icon_ShieldRating", module.ShieldRating.ToString("0.##"), "shieldRating", "modShieldRating");
        }
        if (module.ShieldRechargeRate != 0)
        {
            AddModuleStatEntry("Icon_ShieldRecharge", module.ShieldRechargeRate.ToString("0.##"), "shieldRecharge", "modShieldRecharge");
        }
        if (module.ShieldRechargeDelay != 0)
        {
            AddModuleStatEntry("Icon_ShieldDelay", module.ShieldRechargeDelay.ToString("0.##"), "shieldRechargeDelay", "modShieldRechargeDelay");
        }
        if (module.Ammo != 0)
        {
            AddModuleStatEntry("Icon_Ammo", module.Ammo.ToString("0.##"), "ammo", "modAmmo");
        }
        if (module.PowerStorage != 0)
        {
            AddModuleStatEntry("Icon_PowerStorage", module.PowerStorage.ToString("0.##"), "powerStorage", "modPowerStorage");
        }
        if (module.Crew != 0)
        {
            AddModuleStatEntry("Icon_Crew", module.Crew.ToString("0.##"), "crew", "modCrew");
        }
        if (module.RequiredCrew != 0)
        {
            AddModuleStatEntry("Icon_RequiredCrew", module.RequiredCrew.ToString("0.##"), "requiredCrew", "modRequiredCrew");
        }
        if (module.Supplies != 0)
        {
            AddModuleStatEntry("Icon_Supplies", module.Supplies.ToString("0.##"), "supplies", "modSupplies");
        }
        if (module.EngineThrust != 0)
        {
            AddModuleStatEntry("Icon_EngineThrust", module.EngineThrust.ToString("0.##"), "engineTrust", "modEngineTrust");
        }
        if (module.EngineTurn != 0)
        {
            AddModuleStatEntry("Icon_EngineTurn", module.EngineTurn.ToString("0.##"), "engineTurn", "modEngineTurn");
        }
        if (module.EngineFTL != 0)
        {
            AddModuleStatEntry("Icon_FTL", module.EngineFTL.ToString("0.##"), "engineFTL", "modEngineFTL");
        }
        if (module.EngineBonus != 0)
        {
            AddModuleStatEntry("Icon_EngineBonus", (module.EngineBonus * 100).ToString("0.##") + "%", "engineBonus", "modEngineBonus");
        }
        if (module.Sensor != 0)
        {
            AddModuleStatEntry("Icon_Sensor", module.Sensor.ToString("0.##"), "rangeSensor", "modRangeSensor");
        }
        if (module.LongRangeSensor != 0)
        {
            AddModuleStatEntry("Icon_SensorLongRange", module.LongRangeSensor.ToString("0.##"), "rangeSensorLong", "modRangeSensorLong");
        }
        if (module.AdvancedSensor != 0)
        {
            AddModuleStatEntry("Icon_SensorAdvanced", module.AdvancedSensor.ToString("0.##"), "rangeSensorAdvanced", "modRangeSensorAdvanced");
        }
        if (module.DamageBonus != 0)
        {
            AddModuleStatEntry("Icon_Damage", "%" + (module.DamageBonus * 100f).ToString("0.##"), "damageBonus", "modDamageBonus");
        }
        if (module.DamageBonusFleet != 0)
        {
            AddModuleStatEntry("Icon_DamageFleet", "%" + (module.DamageBonusFleet * 100f).ToString("0.##"), "fleetDamageBonus", "fleetDamageBonusDesc");
        }
        if (module.DefenseBonus != 0)
        {
            AddModuleStatEntry("Icon_Defense", "%" + (module.DefenseBonus * 100f).ToString("0.##"), "defenseBonus", "modDefenseBonus");
        }
        if (module.DefenseBonusFleet != 0)
        {
            AddModuleStatEntry("Icon_DefenseFleet", "%" + (module.DefenseBonusFleet * 100).ToString("0.##"), "fleetDefenseBonus", "fleetDefenseBonusDesc");
        }
        if (module.Repair != 0)
        {
            AddModuleStatEntry("Icon_Repair", module.Repair.ToString("0.##"), "repairRate", "modRepairRate");
        }
        if (module.Research != 0)
        {
            AddModuleStatEntry("Icon_Research", module.Research.ToString("0.##"), "research", "modResearch");
        }
        if (module.Mining != 0)
        {
            AddModuleStatEntry("Icon_Mining", module.Mining.ToString("0.##"), "mining", "modMining");
        }
        if (module.Construction != 0)
        {
            AddModuleStatEntry("Icon_Construction", module.Construction.ToString("0.##"), "construction", "modConstruction");
        }
        if (module.AmmoGenerated != 0)
        {
            AddModuleStatEntry("Icon_AmmoGenerated", module.AmmoGenerated.ToString("0.##"), "ammoGenerated", "modAmmoGenerated");
        }
        if (module.Medical != 0)
        {
            AddModuleStatEntry("Icon_Medical", module.Medical.ToString("0.##"), "medical", "modMedical");
        }
        if (module.Transporter != 0)
        {
            AddModuleStatEntry("Icon_Transporter", module.Transporter.ToString("0.##"), "transporter", "modTransporter");
        }
        if (module.Troops != 0)
        {
            AddModuleStatEntry("Icon_Troop", module.Troops.ToString("0.##"), "troops", "modTroops");
        }
        if (module.BoardingDefense != 0)
        {
            AddModuleStatEntry("Icon_BoardingDefense", module.BoardingDefense.ToString("0.##"), "boardingDefense", "modBoardingDefense");
        }
        if (module.Colonies != 0)
        {
            AddModuleStatEntry("Icon_Colony", module.Colonies.ToString("0.##"), "colonySupplies", "modColonySupplies");
        }
        if (module.CloakingPowerPerMass != 0)
        {
            AddModuleStatEntry("Icon_CloakPower", module.CloakingPowerPerMass.ToString("0.##"), "cloakingPowerPerMass", "modCloakingPower");
        }
        if (module.Stealth != 0)
        {
            AddModuleStatEntry("Icon_Stealth", module.Stealth.ToString("0.##"), "stealth", "modStealth");
        }
        if (module.Diplomacy != 0)
        {
            AddModuleStatEntry("Icon_Diplomacy", module.Diplomacy.ToString("0.##"), "diplomacy", "modDiplomacy");
        }
        if (module.CommandPointReduction != 0)
        {
            AddModuleStatEntry("Icon_CommandPoint", module.CommandPointReduction.ToString("0.##"), "commandPointReduction", "modCommandPointRecution");
        }
        if (module.CommandPointBonusFleet != 0)
        {
            AddModuleStatEntry("Icon_CommandPointFleet", module.CommandPointBonusFleet.ToString("0.##"), "fleetCommandPoints", "modFleetCommandPoints");
        }
        if (module.ExperienceBonus != 0)
        {
            AddModuleStatEntry("Icon_LevelBonus", "%" + (module.ExperienceBonus * 100f).ToString("0.##"), "experienceBonus", "experienceBonusDesc");
        }
        if (module.JammingCount > 0)
        {
            AddModuleStatEntry("Icon_Jamming", module.JammingCount.ToString("0.##"), "JammingCount", "modJammingCount");
        }
        if (module.JammingRange > 0)
        {
            AddModuleStatEntry("Icon_JammingRange", module.GetJammingRangeDisplay().ToString("0.##"), "JammingRange", "modJammingRange");
        }
        if (module.JammingDelay > 0)
        {
            AddModuleStatEntry("Icon_JammingDelay", module.JammingDelay.ToString("0.##"), "JammingDelay", "modJammingDelay");
        }
        if (module.BombArmyDamage > 0)
        {
            AddModuleStatEntry("Icon_BombArmy", module.BombArmyDamage.ToString("0.##"), "BombingArmyDamage", "BombingArmyDamageDesc");
        }
        if (module.BombStructureDamage > 0)
        {
            AddModuleStatEntry("Icon_BombStructure", module.BombStructureDamage.ToString("0.##"), "BombingStructureDamage", "BombingStructureDamageDesc");
        }
        if (module.BombPopulationDamage > 0)
        {
            AddModuleStatEntry("Icon_BombPopulation", module.BombPopulationDamage.ToString("0.##"), "BombingPopulationDamage", "BombingPopulationDamageDesc");
        }
        if (module.BombPollution > 0)
        {
            AddModuleStatEntry("Icon_BombPolution", module.BombPollution.ToString("0.##"), "BombingPollution", "BombingPollutionDesc");
        }
        float defenseRating = module.GetDefenseRating();
        if (defenseRating > 0)
        {
            AddModuleStatEntry("Icon_DefenseRating", defenseRating.ToString("0.##"), "defenseRating", "moduleDefenseRating");
        }
        if (module.Fighters.Count > 0)
        {
            FighterDefinition firstFighter = module.GetFirstFighter();
            if (firstFighter != null)
            {
                AddModuleStatEntry("Icon_Fighter", firstFighter.MaxSquadronSize.ToString("0.##"), "figters", "modFighters");
                AddModuleStatEntry("Icon_FighterHealth", firstFighter.Health.ToString("0.##"), "modFighterHealth", "modFighterHealthDesc");
                if (firstFighter.ArmorHealth > 0)
                {
                    AddModuleStatEntry("Icon_FighterArmorHealth", firstFighter.Health.ToString("0.##"), "modFighterArmorHealth", "modFighterArmorHealthDesc");
                }
                if (firstFighter.ArmorRating > 0)
                {
                    AddModuleStatEntry("Icon_FighterArmor", firstFighter.ArmorRating.ToString("0.##"), "modFighterArmor", "modFighterArmorDesc");
                }
                if (firstFighter.ShieldHealth > 0)
                {
                    AddModuleStatEntry("Icon_FighterShieldsHealth", firstFighter.ShieldHealth.ToString("0.##"), "modFighterShieldHealth", "modFighterShieldHealthDesc");
                }
                if (firstFighter.ShieldRating > 0)
                {
                    AddModuleStatEntry("Icon_FighterShields", firstFighter.ShieldRating.ToString("0.##"), "modFighterShield", "modFighterShieldDesc");
                }
                if (firstFighter.ShieldRecharge > 0)
                {
                    AddModuleStatEntry("Icon_FighterShieldsRecharge", firstFighter.ShieldRecharge.ToString("0.##"), "modFighterShieldRecharge", "modFighterShieldRechargeDesc");
                }
                if (firstFighter.ShieldDelay > 0)
                {
                    AddModuleStatEntry("Icon_FighterShieldsDelay", firstFighter.ShieldDelay.ToString("0.##"), "modFighterShieldDelay", "modFighterShieldDelayDesc");
                }
                float firePower = firstFighter.GetFirePower();
                if (firePower > 0)
                {
                    AddModuleStatEntry("Icon_FighterFirePower", firePower.ToString("0.##"), "modFighterFirepower", "modFighterFirepowerDesc");
                }
                if (firstFighter.Crew > 0)
                {
                    AddModuleStatEntry("Icon_FighterCrew", firstFighter.Crew.ToString("0.##"), "fighterCrew", "fighterCrewDesc");
                }
            }
        }
        if (module.HeavyFighters.Count > 0)
        {
            FighterDefinition firstHeavyFighter = module.GetFirstHeavyFighter();
            if (firstHeavyFighter != null)
            {
                AddModuleStatEntry("Icon_HeavyFighter", firstHeavyFighter.MaxSquadronSize.ToString("0.##"), "heavyFighters", "modHeavyFighters");
                AddModuleStatEntry("Icon_HeavyFighterHealth", firstHeavyFighter.Health.ToString("0.##"), "modHeavyFighterHealth", "modHeavyFighterHealthDesc");

                if (firstHeavyFighter.ArmorHealth > 0)
                {
                    AddModuleStatEntry("Icon_HeavyFighterArmorHealth", firstHeavyFighter.ArmorHealth.ToString("0.##"), "modHeavyFighterArmorHealth", "modHeavyFighterArmorHealthDesc");
                }
                if (firstHeavyFighter.ArmorRating > 0)
                {
                    AddModuleStatEntry("Icon_HeavyFighterArmor", firstHeavyFighter.ArmorRating.ToString("0.##"), "modHeavyFighterArmor", "modHeavyFighterArmorDesc");
                }
                if (firstHeavyFighter.ShieldHealth > 0)
                {
                    AddModuleStatEntry("Icon_HeavyFighterShieldsHealth", firstHeavyFighter.ShieldHealth.ToString("0.##"), "modHeavyFighterShieldHealth", "modHeavyFighterShieldHealthDesc");
                }
                if (firstHeavyFighter.ShieldRating > 0)
                {
                    AddModuleStatEntry("Icon_HeavyFighterShields", firstHeavyFighter.ShieldRating.ToString("0.##"), "modHeavyFighterShield", "modHeavyFighterShieldDesc");
                }
                if (firstHeavyFighter.ShieldRecharge > 0)
                {
                    AddModuleStatEntry("Icon_HeavyFighterShieldsRecharge", firstHeavyFighter.ShieldRecharge.ToString("0.##"), "modHeavyFighterShieldRecharge", "modHeavyFighterShieldRechargeDesc");
                }
                if (firstHeavyFighter.ShieldDelay > 0)
                {
                    AddModuleStatEntry("Icon_HeavyFighterShieldsDelay", firstHeavyFighter.ShieldDelay.ToString("0.##"), "modHeavyFighterShieldDelay", "modHeavyFighterShieldDelayDesc");
                }
                float firePower = firstHeavyFighter.GetFirePower();
                if (firePower > 0)
                {
                    AddModuleStatEntry("Icon_HeavyFighterFirePower", firePower.ToString("0.##"), "modHeavyFighterFirepower", "modHeavyFighterFirepowerDesc");
                }
                if (firstHeavyFighter.Crew > 0)
                {
                    AddModuleStatEntry("Icon_HeavyFighterCrew", firstHeavyFighter.Crew.ToString("0.##"), "HeavyFighterCrew", "HeavyFighterCrewDesc");
                }
            }
        }
        if (module.AssaultPods.Count > 0)
        {
            FighterDefinition firstAssaultPod = module.GetFirstAssaultPod();
            if (firstAssaultPod != null)
            {
                AddModuleStatEntry("Icon_AssaultPod", firstAssaultPod.MaxSquadronSize.ToString("0.##"), "assaultPods", "modHeavyFighters");
                AddModuleStatEntry("Icon_AssaultPodHealth", firstAssaultPod.Health.ToString("0.##"), "AssaultPodHealth", "AssaultPodHealthDesc");
                if (firstAssaultPod.Troops > 0)
                {
                    AddModuleStatEntry("Icon_AssaultPodTroops", firstAssaultPod.Troops.ToString("0.##"), "AssaultPodTroops", "AssaultPodTroopsDesc");
                }
                if (firstAssaultPod.Crew > 0)
                {
                    AddModuleStatEntry("Icon_AssaultPodCrew", firstAssaultPod.Crew.ToString("0.##"), "AssaultPodCrew", "AssaultPodCrewDesc");
                }
            }
        }

        //Weapon stats
        Weapon weapon = module.GetWeapon();
        if (weapon != null)
        {
            if (weapon.SalvoSize != 0)
            {
                AddModuleStatEntry("Icon_WeaponVolley", (weapon.SalvoSize * weapon.Projectiles).ToString("0.##"), "salvoSize", "salvoSizeDesc");
            }
            if (weapon.Arc != 0)
            {
                AddModuleStatEntry("Icon_WeaponArc", weapon.Arc.ToString("0.##"), "weaponArc", "weaponArcDesc");
            }
            if (weapon.AmmoCost != 0)
            {
                AddModuleStatEntry("Icon_WeaponAmmo", weapon.AmmoCost.ToString("0.##"), "ammoCost", "ammoCostDesc");
            }
            if (weapon.PowerCost != 0)
            {
                AddModuleStatEntry("Icon_WeaponPower", weapon.PowerCost.ToString("0.##"), "powerCost", "powerCostDesc");
            }
            if (weapon.isBeam && weapon.BeamPowerCost != 0)
            {
                AddModuleStatEntry("Icon_BeamPower", weapon.BeamPowerCost.ToString("0.##"), "beamPower", "beamPowerDesc");
            }
            if (weapon.isBeam && weapon.BeamDuration != 0)
            {
                AddModuleStatEntry("Icon_BeamDuration", weapon.BeamDuration.ToString("0.##"), "beamDuration", "beamDurationDesc");
            }
            if (weapon.Delay != 0)
            {
                AddModuleStatEntry("Icon_WeaponDelay", weapon.Delay.ToString("0.##"), "fireDelay", "fireDelayDesc");
            }
            if (weapon.Spread != 0)
            {
                AddModuleStatEntry("Icon_WeaponSpread", weapon.Spread.ToString("0.##"), "weaponSpread", "weaponSpreadDesc");
            }
            if (weapon.HealthDamageModifier != 1f)
            {
                AddModuleStatEntry("Icon_WeaponDamageHealth", "%" + (weapon.HealthDamageModifier * 100f).ToString("0.##"), "HealthDamageModifier", "HealthDamageModifierDesc");
            }
            if (weapon.ShieldDamageModifier != 1f)
            {
                AddModuleStatEntry("Icon_WeaponDamageShield", "%" + (weapon.ShieldDamageModifier * 100f).ToString("0.##"), "ShieldDamageModifier", "ShieldDamageModifierDesc");
            }
            if (weapon.ArmorDamageModifier != 1f)
            {
                AddModuleStatEntry("Icon_WeaponDamageArmor", "%" + (weapon.ArmorDamageModifier * 100f).ToString("0.##"), "ArmorDamageModifier", "ArmorDamageModifierDesc");
            }
            if (weapon.FighterDamageModifier != 1f)
            {
                AddModuleStatEntry("Icon_WeaponDamageFighter", "%" + (weapon.FighterDamageModifier * 100f).ToString("0.##"), "FighterDamageModifier", "FighterDamageModifierDesc");
            }
            if (weapon.ProjectileDamageModifier != 1f)
            {
                AddModuleStatEntry("Icon_WeaponDamagePointDefense", "%" + (weapon.ProjectileDamageModifier * 100f).ToString("0.##"), "pointDefenseDamageModifier", "pointDefenseDamageModifierDesc");
            }
            if (weapon.DamageAllQuads)
            {
                AddModuleStatEntry("Icon_WeaponDamageAllSides", "True", "DamageAllSides", "DamageAllSidesDesc");
            }
            if (weapon.PowerDamageModifier != 0f)
            {
                AddModuleStatEntry("Icon_WeaponDamagePower", "%" + (weapon.PowerDamageModifier * 100f).ToString("0.##"), "powerDamage", "powerDamageDesc");
            }
            if (weapon.IgnoreShieldChance > 0)
            {
                AddModuleStatEntry("Icon_WeaponShieldIgnore", "%" + (weapon.IgnoreShieldChance * 100f).ToString("0.##"), "ShieldIgnoreChance", "ShieldIgnoreChanceDesc");
            }
            if (weapon.IgnoreArmorChance > 0)
            {
                AddModuleStatEntry("Icon_WeaponArmorIgnore", "%" + (weapon.IgnoreArmorChance * 100f).ToString("0.##"), "ArmorIgnoreChance", "ArmorIgnoreChanceDesc");
            }
            if (weapon.IgnoreArmorRatingChance > 0)
            {
                AddModuleStatEntry("Icon_WeaponArmorRatingIgnore", "%" + (weapon.IgnoreArmorRatingChance * 100f).ToString("0.##"), "ArmorRatingIgnoreChance", "ArmorRatingIgnoreChanceDesc");
            }
            if (weapon.PointDefense)
            {
                if (weapon.PointDefenseOnly)
                {
                    AddModuleStatEntry("Icon_PointDefenseOnly", "True", "PointDefenseOnly", "PointDefenseOnlyDesc");
                }
                else
                {
                    AddModuleStatEntry("Icon_PointDefense", "True", "PointDefense", "PointDefenseDesc");
                }
            }
            if (weapon.ProjectileHealth > 0)
            {
                AddModuleStatEntry("Icon_ProjectileHealth", weapon.ProjectileHealth.ToString("0.##"), "ProjectileHealth", "ProjectileHealthDesc");
            }
            float firePowerRating = weapon.GetAverageDPS() / 10f;
            if (firePowerRating > 0)
            {
                AddModuleStatEntry("Icon_FirePower", firePowerRating.ToString("0.##"), "firepower", "weaponFirepower");
            }

            weaponDamageGraph.gameObject.SetActive(true);
            weaponDamageGraph.SetWeapon(weapon);
        }
        else
        {
            weaponDamageGraph.gameObject.SetActive(false);
        }
    }

    void AddModuleStatEntry(string IconName, string text, string ToolTipTitle, string ToolTipDescription)
    {
        ModuleStatEntries.AddEntry(ResourceManager.GetIconTexture(IconName), text);
    }

    public void SetModuleScale(float scale)
    {
        ModuleScale = scale;

        ApplyModuleScale();
    }

    void ApplyModuleScale()
    {
        if (SelectedModule != null)
        {
            SelectedModuleCursorRect.size = new Vector2(SelectedModule.SizeX * ModuleScale, SelectedModule.SizeY * ModuleScale);
        }
    }

    public float GetModuleScale()
    {
        return ModuleScale;
    }

    public int GetModuleRotation()
    {
        return ModuleRotation;
    }

    public void ResetModuleRotation()
    {
        ModuleRotation = 0;
    }

    public void RotateModule()
    {
        ModuleRotation += 90;

        if(ModuleRotation == 360)
        {
            ModuleRotation = 0;
        }
    }

    void ToggleMirrorMode()
    {
        useSymmetricCursor = !useSymmetricCursor;
    }

    public void DrawSelectedModuleTexture()
    {
        if (SelectedModule != null)
        {
            Vector2 mousePosition = Input.mousePosition;
            mousePosition.y = Screen.height - mousePosition.y;

            SelectedModuleCursorRect.position = mousePosition;

            Matrix4x4 matrixBackup = GUI.matrix;
            GUIUtility.RotateAroundPivot(ModuleRotation, SelectedModuleCursorRect.center);
            if (SelectedModuleTexture != null)
                GUI.DrawTexture(SelectedModuleCursorRect, SelectedModuleTexture);
            if (useSymmetricCursor)
            {
                GUI.matrix = matrixBackup;
                mousePosition.x = Screen.width - mousePosition.x;
                if (ModuleRotation == 0 || ModuleRotation == 180)
                {
                    mousePosition.x -= SelectedModule.SizeX * ModuleScale - ModuleScale;
                    SelectedModuleCursorRect.x = mousePosition.x - ModuleScale;
                }
                else
                {
                    mousePosition.x -= SelectedModule.SizeY * ModuleScale - ModuleScale;
                    SelectedModuleCursorRect.x = mousePosition.x;
                }
                GUIUtility.RotateAroundPivot(ModuleRotation, SelectedModuleCursorRect.center);
                if (SelectedModuleTexture != null)
                    GUI.DrawTexture(SelectedModuleCursorRect, SelectedModuleTexture);
            }
            GUI.matrix = matrixBackup;
        }
    }

    public enum ModuleCategory
    {
        Weapons,
        Defenses,
        Systems
    }
}
