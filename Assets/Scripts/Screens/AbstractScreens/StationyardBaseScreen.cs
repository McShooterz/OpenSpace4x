/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public abstract class StationyardBaseScreen : UnitDesignBaseScreen
{
    #region Variables
    protected StationHullData selectedHullData = null;
    protected StationDesign selectedDesign = null;

    //Ships
    protected StationHullScrollList HullList;
    protected Rect ShipDesignScrollWindowRect;
    protected Rect ShipDesignScrollViewRect;
    protected Vector2 ShipDesignScrollPosition;

    protected List<StationDesignListEntry> DesignList = new List<StationDesignListEntry>();
    protected StationDesign DesignToDelete = null;

    protected List<Rect> Slots = new List<Rect>();

    //Modules added to current ship
    protected List<SlotedModule> SlotedModules = new List<SlotedModule>();
    protected List<SlotedModule> ModulesToRemove = new List<SlotedModule>();

    //Quadrant based stats Rect
    protected Rect DesignDefenseHeaderRect;

    //Design stats quadrant specific
    protected float DesignHealth;
    protected float DesignArmorHealth;
    protected float DesignArmorRating;
    protected float DesignShieldHealth;
    protected float DesignShieldRating;
    protected float DesignShieldRecharge;
    protected float DesignShieldDelay;

    #endregion

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (overwriteWarningPopupOpen)
            {
                overwriteWarningPopupOpen = false;
            }
            if (quitResumeSubScreen.isOpen())
                quitResumeSubScreen.SetOpen(false);
            else
                quitResumeSubScreen.SetOpen(true);
        }
    }

    public override void Draw()
    {
        if (quitResumeSubScreen.isOpen())
        {
            quitResumeSubScreen.Draw();
        }
        else
        {
            base.Draw();

            HullList.Draw();

            ShipDesignScrollPosition = GUI.BeginScrollView(ShipDesignScrollWindowRect, ShipDesignScrollPosition, ShipDesignScrollViewRect);
            foreach (StationDesignListEntry entry in DesignList)
            {
                entry.Draw(selectedDesign);
            }
            GUI.EndScrollView();

            if (DesignToDelete != null)
                DeleteDesign(DesignToDelete);

            if (!CheckValidDesign())
            {
                GUI.enabled = false;
            }
            if (GUI.Button(DesignSaveButtonRect, "Save", GameManager.instance.standardButtonStyle))
            {
                if (ResourceManager.CheckForStationDesign(selectedHullData.Name, DesignName))
                {
                    overwriteWarningPopupOpen = true;
                    DeselectModule();
                }
                else
                {
                    SaveDesign();
                }
                PlayMainButtonClick();
            }
            GUI.enabled = true;

            if (GUI.Button(CloseButtonRect, "Close", GameManager.instance.standardButtonStyle))
            {
                PlayMainButtonClick();
                CloseScreen();
            }

            if (MirrirModeButtonRect.Contains(mousePosition))
            {
                ToolTip.SetText("mirrormodule", "mirrormoduleDesc");
            }

            if (useSymmetricCursor)
            {
                if (GUI.Button(MirrirModeButtonRect, "Mirror: On", GameManager.instance.standardButtonStyle))
                {
                    useSymmetricCursor = false;
                    PlayMainButtonClick();
                }
            }
            else
            {
                if (GUI.Button(MirrirModeButtonRect, "Mirror: Off", GameManager.instance.standardButtonStyle))
                {
                    useSymmetricCursor = true;
                    PlayMainButtonClick();
                }
            }

            //Draw slot layout
            if (selectedModule != null)
            {
                if (CheckHullModuleAllow(selectedHullData.ModuleLimitations, selectedModuleSet.ModuleCategory))
                {
                    GUI.color = Color.white;
                }
                else
                {
                    GUI.color = Color.red;
                }
            }
            foreach (Rect slot in Slots)
            {
                GUI.DrawTexture(slot, SlotTexture);
            }
            GUI.color = Color.white;

            //Draw placed modules on top of slots
            foreach (SlotedModule module in SlotedModules)
            {
                module.Draw();
            }

            //Draw selected module texture
            if (selectedModule != null)
            {
                if (ModuleCanRotate(selectedModule))
                {
                    if (ModuleRotationButtonRect.Contains(mousePosition))
                    {
                        ToolTip.SetText("rotateModule", "rotateModuleDesc");
                    }

                    if (GUI.Button(ModuleRotationButtonRect, ResourceManager.GetIconTexture("Icon_Rotate")))
                    {
                        RotateModule();
                        PlayMainButtonClick();
                    }
                }

                //Don't draw or check for placement if mouse over side UI menus, this draws currently selected module on mouse cursor
                if (!LeftSidePanelRect.Contains(mousePosition) && !RightSidePanelRect.Contains(mousePosition))
                {
                    if (ModuleRotation == 0 || ModuleRotation == 180)
                    {
                        SelectedModuleRect.position = mousePosition;
                    }
                    else
                    {
                        float offset = (Mathf.Max(SelectedModuleRect.height, SelectedModuleRect.width) - Mathf.Min(SelectedModuleRect.height, SelectedModuleRect.width)) / 2;
                        if (SelectedModuleRect.height > SelectedModuleRect.width)
                        {
                            SelectedModuleRect.position = new Vector2(mousePosition.x + offset, mousePosition.y - offset);
                        }
                        else
                        {
                            SelectedModuleRect.position = new Vector2(mousePosition.x - offset, mousePosition.y + offset);
                        }
                    }

                    DrawSelectedModuleTexture();

                    //Check for attempt to place module
                    if (SlotsAreaRect.Contains(mousePosition))
                    {
                        CheckModulePlacement(mousePosition);
                        if (useSymmetricCursor)
                        {
                            CheckModulePlacement(SymmetricMouse);
                        }
                    }
                }
            }
            else
            {
                //Check to see if clicked on placed module and grab it
                if (Input.GetMouseButtonUp(0) && SlotsAreaRect.Contains(mousePosition) && !PopupOpen())
                {
                    CheckPlacedModuleSelected();
                }
            }

            GUI.enabled = true;
            if (overwriteWarningPopupOpen)
            {
                GUI.Box(overwriteWarningRect, "", GameManager.instance.standardBackGround);
                GUI.Box(overwriteWarningMessageRect, ResourceManager.GetLocalization("overwriteWarning"));

                if (GUI.Button(overwriteAcceptButtonRect, ResourceManager.GetLocalization("overwrite"), GameManager.instance.standardButtonStyle))
                {
                    SaveDesign();
                    overwriteWarningPopupOpen = false;
                    PlayMainButtonClick();
                }
                if (GUI.Button(overwriteCancelButtonRect, ResourceManager.GetLocalization("cancel"), GameManager.instance.standardButtonStyle))
                {
                    overwriteWarningPopupOpen = false;
                    PlayMainButtonClick();
                }
            }
            if (SlotsAreaRect.Contains(mousePosition) && !PopupOpen())
            {
                //Show weapon arcs if hovering over weapon
                CheckPlacedModuleHover();
            }

            ToolTip.Draw();
        }
    }

    protected override void Initialize()
    {
        base.Initialize();

        quitResumeSubScreen = new QuitResumeSubScreen(this, CloseScreen);

        Rect ShipHullScrollWindowRect = new Rect(RightSidePanelRect.x + RightSidePanelRect.width * 0.05f, Screen.height * 0.34f * 0.05f, RightSidePanelRect.width * 0.9f, Screen.height * 0.34f * 0.45f);
        Rect ShipHullScrollViewRect = new Rect(0, 0, ShipHullScrollWindowRect.width * 0.9f, ShipHullScrollWindowRect.height * 5f);
        HullList = new StationHullScrollList(ShipHullScrollWindowRect, ChangeHull, CheckHullAllowed);

        ShipDesignScrollWindowRect = new Rect(ShipHullScrollWindowRect.x, ShipHullScrollWindowRect.y + ShipHullScrollWindowRect.height + GameManager.instance.QuarterButtonSpacing, RightSidePanelRect.width * 0.9f, Screen.height * 0.34f * 0.45f);
        ShipDesignScrollViewRect = new Rect(0, 0, ShipHullScrollViewRect.width, ShipDesignScrollWindowRect.height * 5f);
        ShipDesignScrollPosition = Vector2.zero;
        DesignListEntrySize = new Vector2(ShipDesignScrollViewRect.width, ShipDesignScrollWindowRect.height * 0.2f);

        //Design stats setup
        float YPosition, XPos1, XPos2, XPos3, XPos4;
        YPosition = ModuleStatSize.y * 0.334f;
        float StatIndent = DesignStatsWindowRect.width * 0.025f;
        float HeightSpacing = ModuleStatSize.y * 1.2f;
        XPos1 = DesignStatsWindowRect.width * 0.15f;
        XPos2 = XPos1 + ModuleStatSize.x + StatIndent;
        XPos3 = XPos2 + ModuleStatSize.x + StatIndent;
        XPos4 = XPos3 + ModuleStatSize.x + StatIndent;

        //Hull info
        HullNameRect = new Rect(XPos2, YPosition, GameManager.instance.StandardLabelSize.x * 3f, ModuleStatSize.y);
        YPosition += HeightSpacing;
        Rect DesignCostRect = new Rect(XPos1, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignFirepowerRect = new Rect(XPos2, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignDefenseRatingRect = new Rect(XPos3, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignExperienceRect = new Rect(XPos4, YPosition, ModuleStatSize.x, ModuleStatSize.y);

        //Cost group
        YPosition += HeightSpacing;
        DesignResourceCostHeaderRect = new Rect(XPos2, YPosition, GameManager.instance.StandardLabelSize.x * 2, ModuleStatSize.y);
        YPosition += HeightSpacing;
        Rect DesignProductionCostRect = new Rect(XPos1, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignAlloyCostRect = new Rect(XPos2, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignAdvancedAlloyCostRect = new Rect(XPos3, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignSuperiorAlloyCostRect = new Rect(XPos4, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        YPosition += HeightSpacing;
        Rect DesignCrystalCostRect = new Rect(XPos1, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignRareCrystalCostRect = new Rect(XPos2, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignExoticCrystalCostRect = new Rect(XPos3, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignExoticParticleCostRect = new Rect(XPos4, YPosition, ModuleStatSize.x, ModuleStatSize.y);


        //Personnel group rects
        YPosition += HeightSpacing;
        DesignPersonnelHeaderRect = new Rect(XPos2, YPosition, GameManager.instance.StandardLabelSize.x * 2, ModuleStatSize.y);
        YPosition += HeightSpacing;
        Rect DesignCrewRect = new Rect(XPos1, YPosition, ModuleStatSize.x * 1.9f, ModuleStatSize.y);
        Rect DesignTroopsRect = new Rect(XPos3, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignMedicalRect = new Rect(XPos4, YPosition, ModuleStatSize.x, ModuleStatSize.y);

        //Power group rects
        YPosition += HeightSpacing;
        DesignPowerHeaderRect = new Rect(XPos2, YPosition, GameManager.instance.StandardLabelSize.x * 2, ModuleStatSize.y);
        YPosition += HeightSpacing;
        Rect DesignPowerRect = new Rect(XPos1, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignPowerStorageRect = new Rect(XPos2, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignPowerConsumeRect = new Rect(XPos3, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignPowerTimeRect = new Rect(XPos4, YPosition, ModuleStatSize.x, ModuleStatSize.y);

        //Ammo Group rects
        YPosition += HeightSpacing;
        DesignAmmoHeaderRect = new Rect(XPos2, YPosition, GameManager.instance.StandardLabelSize.x * 2, ModuleStatSize.y);
        YPosition += HeightSpacing;
        Rect DesignAmmoRect = new Rect(XPos1, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignAmmoGeneratedRect = new Rect(XPos2, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignAmmoConsumeRect = new Rect(XPos3, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignAmmoTimeRect = new Rect(XPos4, YPosition, ModuleStatSize.x, ModuleStatSize.y);

        //Sensor group rects
        YPosition += HeightSpacing;
        DesignSensorsHeaderRect = new Rect(XPos2, YPosition, GameManager.instance.StandardLabelSize.x * 2, ModuleStatSize.y);
        YPosition += HeightSpacing;
        Rect DesignSensorRect = new Rect(XPos1, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignLongRangeSensorRect = new Rect(XPos2, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignAdvancedSensorRect = new Rect(XPos3, YPosition, ModuleStatSize.x, ModuleStatSize.y);

        //Defense stat rects
        YPosition += HeightSpacing;
        DesignDefenseHeaderRect = new Rect(XPos2, YPosition, GameManager.instance.StandardLabelSize.x * 2, ModuleStatSize.y);
        YPosition += HeightSpacing;
        Rect DesignHealthRect = new Rect(XPos1, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignArmorHealthRect = new Rect(XPos2, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignArmorRatingRect = new Rect(XPos3, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        YPosition += HeightSpacing;
        Rect DesignShieldHealthRect = new Rect(XPos1, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignShieldRatingRect = new Rect(XPos2, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignShieldRechargeRect = new Rect(XPos3, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignShieldDelayRect = new Rect(XPos4, YPosition, ModuleStatSize.x, ModuleStatSize.y);

        //Combat bonus group
        YPosition += HeightSpacing;
        DesignCombatBonusHeaderRect = new Rect(XPos2, YPosition, GameManager.instance.StandardLabelSize.x * 2, ModuleStatSize.y);
        YPosition += HeightSpacing;
        Rect DesignDamageBonusRect = new Rect(XPos1, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignDefenseBonusRect = new Rect(XPos2, YPosition, ModuleStatSize.x, ModuleStatSize.y);

        //Hangar Group
        YPosition += HeightSpacing;
        DesignHangarHeaderRect = new Rect(XPos2, YPosition, GameManager.instance.StandardLabelSize.x * 2, ModuleStatSize.y);
        YPosition += HeightSpacing;
        Rect DesignFightersRect = new Rect(XPos1, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignHeavyFightersRect = new Rect(XPos2, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignAssaultPodsRect = new Rect(XPos3, YPosition, ModuleStatSize.x, ModuleStatSize.y);

        //Jamming group
        YPosition += HeightSpacing;
        DesignJammingHeaderRect = new Rect(XPos2, YPosition, GameManager.instance.StandardLabelSize.x * 2, ModuleStatSize.y);
        YPosition += HeightSpacing;
        Rect DesignJammingCountRect = new Rect(XPos1, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignJammingRangeRect = new Rect(XPos2, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignJammingDelayRect = new Rect(XPos3, YPosition, ModuleStatSize.x, ModuleStatSize.y);

        //Engineering group
        YPosition += HeightSpacing;
        DesignEngineeringHeaderRect = new Rect(XPos2, YPosition, GameManager.instance.StandardLabelSize.x * 2, ModuleStatSize.y);
        YPosition += HeightSpacing;
        Rect DesignRepairRect = new Rect(XPos1, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignPowerEfficiencyRect = new Rect(XPos2, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignTransporterRect = new Rect(XPos3, YPosition, ModuleStatSize.x, ModuleStatSize.y);

        //Misc group
        YPosition += HeightSpacing;
        DesignMiscHeaderRect = new Rect(XPos2, YPosition, GameManager.instance.StandardLabelSize.x * 2, ModuleStatSize.y);
        YPosition += HeightSpacing;
        Rect DesignResearchRect = new Rect(XPos1, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignMiningRect = new Rect(XPos2, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignBoardingDefenseRect = new Rect(XPos3, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignDiplomacyRect = new Rect(XPos4, YPosition, ModuleStatSize.x, ModuleStatSize.y);

        YPosition += ModuleStatSize.y * 1.5f;
        DesignStatsViewRect.height = YPosition;


        //Design Stat Entries
        DesignStatList.Add(new DesignIconStatEntry(DesignCrewRect, "Icon_Crew", GetFormatedDesignRequiredCrew, "crew", "shipCrew", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignTroopsRect, "Icon_Troop", GetFormatedDesignTroops, "troops", "shipTroops", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignCostRect, "Icon_Money", GetFormattedDesignCost, "shipCost", "shipCostDesc", ToolTip));

        DesignStatList.Add(new DesignIconStatEntry(DesignFirepowerRect, "Icon_FirePower", GetFormattedDesignFirepower, "firepower", "shipFirepower", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignDefenseRatingRect, "Icon_DefenseRating", GetFormattedDesignDefenseRating, "defenseRating", "shipDefenseRating", ToolTip));

        DesignStatList.Add(new DesignIconStatEntry(DesignProductionCostRect, "Icon_Production", GetFormatedDesignProductionCost, "productionCost", "productionCostDesc", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignAlloyCostRect, "Icon_Alloy", GetFormattedDesignAlloyCost, "alloyCost", "alloyCostDesc", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignAdvancedAlloyCostRect, "Icon_AlloyAdvanced", GetFormattedDesignAdvancedAlloyCost, "advancedAlloyCost", "advancedAlloyCostDesc", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignSuperiorAlloyCostRect, "Icon_AlloySuperior", GetFormattedDesignSuperiorAlloyCost, "superiorAllorCost", "superiorAllorCostDesc", ToolTip));

        DesignStatList.Add(new DesignIconStatEntry(DesignCrystalCostRect, "Icon_Crystal", GetFormattedDesignCrystalCost, "crystalCost", "crystalCostDesc", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignRareCrystalCostRect, "Icon_CrystalRare", GetFormattedDesignRareCrystalCost, "rareCrystalCost", "rareCrystalCostDesc", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignExoticCrystalCostRect, "Icon_CrystalExotic", GetFormattedDesignExoticCrystalCost, "exoticCrystalCost", "exoticCrystalCostDesc", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignExoticParticleCostRect, "Icon_ParticleExotic", GetFormattedDesignExoticParticleCost, "exoticParticleCost", "exoticParticleCostDesc", ToolTip));

        DesignStatList.Add(new DesignIconStatEntry(DesignPowerRect, "Icon_Power", GetFormattedDesignPowerSum, "powerGenerated", "powerGeneratedDesc", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignPowerStorageRect, "Icon_PowerStorage", GetFormattedDesignPowerStorage, "powerStorage", "shipPowerStorage", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignPowerConsumeRect, "Icon_WeaponPower", GetFormattedDesignWeaponPowerConsumption, "powerConsumption", "shipPowerConsumption", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignPowerTimeRect, "Icon_PowerTime", GetFormattedDesignWeaponPowerTime, "powerTime", "shipPowerTime", ToolTip));

        DesignStatList.Add(new DesignIconStatEntry(DesignAmmoRect, "Icon_Ammo", GetFormattedDesignAmmo, "ammo", "shipAmmo", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignAmmoGeneratedRect, "Icon_AmmoGenerated", GetFormattedDesignAmmoGenerated, "ammoGenerated", "shipAmmoGenerated", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignAmmoConsumeRect, "Icon_WeaponAmmo", GetFormattedDesignWeaponAmmoConsumption, "ammoConsumption", "shipAmmoConsumption", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignAmmoTimeRect, "Icon_AmmoTime", GetFormattedDesignWeaponAmmoTime, "ammoTime", "shipAmmoTime", ToolTip));

        DesignStatList.Add(new DesignIconStatEntry(DesignSensorRect, "Icon_Sensor", GetFormattedDesignSensor, "rangeSensor", "shipRangeSensor", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignLongRangeSensorRect, "Icon_SensorLongRange", GetFormattedDesignLongRangeSensorSum, "rangeSensorLong", "shipRangeSensorLong", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignAdvancedSensorRect, "Icon_SensorAdvanced", GetFormattedDesignAdvancedSensor, "rangeSensorAdvanced", "shiprangeSensorAdvanced", ToolTip));

        DesignStatList.Add(new DesignIconStatEntry(DesignHealthRect, "Icon_Health", GetFormattedDesignHealth, "health", "shipHealth", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignArmorHealthRect, "Icon_ArmorHealth", GetFormattedDesignArmorHealth, "armorHealth", "shipArmorHealth", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignArmorRatingRect, "Icon_ArmorRating", GetFormattedDesignArmorRating, "armorRating", "shipArmorRating", ToolTip));

        DesignStatList.Add(new DesignIconStatEntry(DesignShieldHealthRect, "Icon_ShieldHealth", GetFormattedDesignShieldHealth, "shieldHealth", "shipShieldHealth", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignShieldRatingRect, "Icon_ShieldRating", GetFormattedDesignShieldRating, "shieldRating", "shipShieldRating", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignShieldRechargeRect, "Icon_ShieldRecharge", GetFormattedDesignShieldRecharge, "shieldRecharge", "shipShieldRecharge", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignShieldDelayRect, "Icon_ShieldDelay", GetFormattedDesignShieldDelay, "shieldRechargeDelay", "shipShieldRechargeDelay", ToolTip));

        DesignStatList.Add(new DesignIconStatEntry(DesignFightersRect, "Icon_Fighter", GetFormattedDesignFighters, "figters", "shipFighters", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignHeavyFightersRect, "Icon_HeavyFighter", GetFormattedDesignHeavyFighters, "heavyFighters", "shipHeavyFighters", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignAssaultPodsRect, "Icon_AssaultPod", GetFormattedDesignAssaultPods, "assaultPods", "shipAssaultPods", ToolTip));

        DesignStatList.Add(new DesignIconStatEntry(DesignDamageBonusRect, "Icon_Damage", GetFormattedDesignDamageBonus, "damageBonus", "shipDamageBonus", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignDefenseBonusRect, "Icon_Defense", GetFormattedDesignDefenseBonus, "defenseBonus", "shipDefenseBonus", ToolTip));

        DesignStatList.Add(new DesignIconStatEntry(DesignJammingCountRect, "Icon_Jamming", GetFormattedDesignJammingCount, "JammingCount", "shipJammingCount", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignJammingRangeRect, "Icon_JammingRange", GetFormattedDesignJammingRange, "JammingRange", "shipJammingRange", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignJammingDelayRect, "Icon_JammingDelay", GetFormattedDesignJammingDelay, "JammingDelay", "shipJammingDelay", ToolTip));

        DesignStatList.Add(new DesignIconStatEntry(DesignResearchRect, "Icon_Research", GetFormattedDesignResearch, "research", "shipResearch", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignMedicalRect, "Icon_Medical", GetFormattedDesignMedical, "medical", "shipMedical", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignTransporterRect, "Icon_Transporter", GetFormattedDesignTransporter, "transporter", "shipTransporter", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignMiningRect, "Icon_Mining", GetFormattedDesignMining, "mining", "shipMining", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignRepairRect, "Icon_Repair", GetFormattedDesignRepair, "repairRate", "shipRepairRate", ToolTip));


        DesignStatList.Add(new DesignIconStatEntry(DesignBoardingDefenseRect, "Icon_BoardingDefense", GetFormattedDesignBoardingDefense, "boardingDefense", "shipBoardingDefense", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignDiplomacyRect, "Icon_Diplomacy", GetFormattedDesignDiplomacy, "diplomacy", "shipDiplomacy", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignExperienceRect, "Icon_Level", GetFormattedDesignExperience, "killExperience", "killExperienceDesc", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignPowerEfficiencyRect, "Icon_PowerEfficiency", GetFormattedDesignPowerEfficiency, "powerEfficiency", "powerEfficiencyDesc", ToolTip));

        overwriteWarningRect = new Rect(Screen.width * 0.35f, Screen.height * 0.333f, Screen.width * 0.3f, Screen.height * 0.125f);
        overwriteWarningMessageRect = new Rect(overwriteWarningRect.x, overwriteWarningRect.y, overwriteWarningRect.width, overwriteWarningRect.height * 0.4f);
        overwriteAcceptButtonRect = new Rect(overwriteWarningRect.x + overwriteWarningRect.width / 2f - GameManager.instance.StandardButtonSize.x * 1.5f, overwriteWarningMessageRect.yMax + overwriteWarningMessageRect.height * 0.225f, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);
        overwriteCancelButtonRect = new Rect(overwriteAcceptButtonRect.xMax + GameManager.instance.StandardButtonSize.x, overwriteAcceptButtonRect.y, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);

        BuildModuleSetLists();
        ChangeModuleSetList(ModuleListTypes.Weapon);

        HullList.CheckFirstHull(ChangeHull);
    }

    protected override void CheckPlacedModuleHover()
    {
        if (SlotsAreaRect.Contains(mousePosition))
        {
            foreach (SlotedModule module in SlotedModules)
            {
                if (module.baseRect.Contains(mousePosition))
                {
                    if (module.module.GetWeaponExists())
                    {
                        GUI.DrawTexture(WeaponArcRect, WeaponArcCircle);
                        GUI.DrawTexture(WeaponArcRect, ResourceManager.GetUITexture("WeaponArc360"));
                    }
                    DrawHoveredModuleInfo(mousePosition, module.module);
                    return;
                }
            }
        }
    }

    protected override void DrawDesignStats()
    {
        DesignStatsPosition = GUI.BeginScrollView(DesignStatsWindowRect, DesignStatsPosition, DesignStatsViewRect);

        //Hull info
        GUI.Label(HullNameRect, FormattedHullName, GameManager.instance.ModuleTitleStyle);

        //Group Headers
        GUI.Label(DesignResourceCostHeaderRect, "Construction Cost");
        GUI.Label(DesignPersonnelHeaderRect, "Personnel");
        if (DesignPowerSum > 0)
        {
            GUI.Label(DesignPowerHeaderRect, "Power");
        }
        else
        {
            Color guiColor = GUI.color;
            GUI.color = Color.red;
            GUI.Label(DesignPowerHeaderRect, "Power");
            GUI.color = guiColor;
        }
        GUI.Label(DesignAmmoHeaderRect, "Ammo");
        GUI.Label(DesignSensorsHeaderRect, "Sensors");
        GUI.Label(DesignDefenseHeaderRect, "Defense");
        GUI.Label(DesignHangarHeaderRect, "Hangar");
        GUI.Label(DesignCombatBonusHeaderRect, "Combat Bonuses");
        GUI.Label(DesignJammingHeaderRect, "Jamming Defense");
        GUI.Label(DesignEngineeringHeaderRect, "Engineering");
        GUI.Label(DesignMiscHeaderRect, "Miscellaneous");

        foreach (DesignIconStatEntry entry in DesignStatList)
        {
            entry.Draw();
        }

        GUI.EndScrollView();

        if (DesignStatsWindowRect.Contains(mousePosition))
        {
            Vector2 ScrollMousePosition = mousePosition - DesignStatsWindowRect.position + DesignStatsPosition;
            if (HullNameRect.Contains(ScrollMousePosition))
            {
                
            }
            else
            {
                foreach (DesignIconStatEntry entry in DesignStatList)
                {
                    if (entry.CheckToolTip(ScrollMousePosition))
                    {
                        break;
                    }
                }
            }
        }
    }

    protected void ChangeHull(StationHullData hullData)
    {
        ClearFormatedSlots();
        ClearSlottedModules();
        selectedHullData = hullData;
        FormatSlotLayout(ResourceManager.GetStationSlotLayout(hullData.Name));
        SetStationModel(hullData);
        BuildDesignList(selectedHullData);
        ClearDesignModuleStats();
        ShipDesignScrollPosition = Vector2.zero;

        FormattedHullName = selectedHullData.Name + " - " + selectedHullData.Classification.ToString();
    }

    protected void LoadDesign(StationDesign design)
    {
        selectedDesign = design;
        DesignName = selectedDesign.Name;

        ClearSlottedModules();

        foreach (DesignModule designMod in design.Modules)
        {
            if (designMod.Position < Slots.Count)
            {
                SlotedModule slotedModule = SlotedModule.CreateSlotedModule(designMod, Slots[designMod.Position]);
                if (slotedModule != null)
                {
                    SlotedModules.Add(slotedModule);
                }
            }
        }
        RecalculateDesignStats();
    }

    protected void FormatSlotLayout(StationSlotLayout sourceLayout)
    {
        moduleScale = 32f;

        ClearFormatedSlots();
        ClearSlottedModules();

        Vector2 Position;
        Vector2 Size;
        float MinX, MaxX, MinY, MaxY;
        Vector2 SizeMin;

        //Get size of center quadrant
        if (sourceLayout.SlotList.Count > 0)
        {
            Rect temp = sourceLayout.SlotList[0];
            MinX = temp.xMin;
            MaxX = temp.xMax;
            MinY = temp.yMin;
            MaxY = temp.yMax;
            foreach (Rect slot in sourceLayout.SlotList)
            {
                if (slot.xMin < MinX)
                {
                    MinX = slot.xMin;
                }
                else if (slot.xMax > MaxX)
                {
                    MaxX = slot.xMax;
                }

                if (slot.yMin < MinY)
                {
                    MinY = slot.yMin;
                }
                else if (slot.yMax > MaxY)
                {
                    MaxY = slot.yMax;
                }
            }
            Size = new Vector2((MaxX - MinX) / 16, (MaxY - MinY) / 16);
            SizeMin = new Vector2(MinX, MinY);
        }
        else
        {
            Size = Vector2.zero;
            SizeMin = Vector2.zero;
        } 

        //Determine scale of modules
        float MaxSlotsVertical = Size.y;
        float MaxSlotsHorizontal = Size.x;
        if (MaxSlotsVertical * moduleScale > Screen.height * 0.9f)
        {
            moduleScale = Screen.height * 0.9f / MaxSlotsVertical;
        }
        if (MaxSlotsHorizontal * moduleScale > Screen.width * 0.45f)
        {
            moduleScale = Screen.width * 0.45f / MaxSlotsHorizontal;
        }

        //Apply scale
        Size *= moduleScale;
        MaxSlotsVertical *= moduleScale;
        MaxSlotsHorizontal *= moduleScale;
        if (selectedModule != null)
        {
            SelectedModuleRect = new Rect(0, 0, selectedModule.SizeX * moduleScale, selectedModule.SizeY * moduleScale);
        }

        //Determine the center of the layout
        float centerY = (Screen.height - MaxSlotsVertical) / 2;
        float centerX = Screen.width * 0.25f + (Screen.width * 0.5f - MaxSlotsHorizontal) / 2;

        Position = new Vector2(centerX, centerY);

        MinX = Screen.width;
        MaxX = 0;
        MinY = Screen.height;
        MaxY = 0;

        //Build new slots
        foreach (Rect slot in sourceLayout.SlotList)
        {
            Rect newSlot = new Rect((slot.x - SizeMin.x) / 16f * moduleScale + Position.x, (slot.y - SizeMin.y) / 16f * moduleScale + Position.y, moduleScale, moduleScale);
            if (newSlot.x < MinX)
                MinX = newSlot.x;
            if (newSlot.xMax > MaxX)
                MaxX = newSlot.xMax;
            if (newSlot.y < MinY)
                MinY = newSlot.y;
            if (newSlot.yMax > MaxY)
                MaxY = newSlot.yMax;

            Slots.Add(newSlot);
        }
        SlotsAreaRect = new Rect(MinX, MinY, MaxX - MinX, MaxY - MinY);
    }

    protected void SetStationModel(StationHullData hullData)
    {
        Vector3 Position = Camera.main.transform.position + Camera.main.transform.forward * 5f;

        if (unitModel != null)
        {
            Object.Destroy(unitModel);
        }
        unitModel = ResourceManager.CreateStation(hullData, Position, Quaternion.identity);
    }

    protected override void ClearFormatedSlots()
    {
        Slots.Clear();
    }

    protected override void ClearSlottedModules()
    {
        SlotedModules.Clear();
    }

    protected void CheckModulePlacement(Vector2 checkPosition)
    {
        bool Valid = false;
        bool SlotMissing = false;
        int SizeX;
        int SizeY;
        List<Rect> HoveredRects = new List<Rect>();

        if (CheckHullModuleAllow(selectedHullData.ModuleLimitations, selectedModuleSet.ModuleCategory))
        {
            for (int index = 0; index < Slots.Count; index++)
            {
                if (Slots[index].Contains(checkPosition))
                {
                    HoveredRects.Add(Slots[index]);
                    //Check if module already in slot
                    foreach (SlotedModule slotmod in SlotedModules)
                    {
                        if (slotmod.module == selectedModule && slotmod.baseRect.x == Slots[index].x && slotmod.baseRect.y == Slots[index].y)
                        {
                            return;
                        }
                    }
                    //Check to see if module can fit               
                    if (ModuleRotation == 0 || ModuleRotation == 180)
                    {
                        SizeX = selectedModule.SizeX;
                        SizeY = selectedModule.SizeY;
                    }
                    else
                    {
                        SizeX = selectedModule.SizeY;
                        SizeY = selectedModule.SizeX;
                    }

                    for (int x = 0; x < SizeX; x++)
                    {
                        for (int y = 0; y < SizeY; y++)
                        {
                            if (x == 0 && y == 0)
                            {
                                continue;
                            }
                            else
                            {
                                Valid = false;
                                Vector2 TestPoint = new Vector2(Slots[index].center.x + x * moduleScale, Slots[index].center.y + y * moduleScale);
                                foreach (Rect checkSlot in Slots)
                                {
                                    if (checkSlot.Contains(TestPoint))
                                    {
                                        Valid = true;
                                        HoveredRects.Add(checkSlot);
                                    }
                                }
                                if (!Valid)
                                {
                                    SlotMissing = true;
                                }
                            }
                        }
                    }

                    //If doesn't fit draw in red and return
                    if (SlotMissing)
                    {
                        GUI.color = Color.red;
                        foreach (Rect slot in HoveredRects)
                        {
                            GUI.DrawTexture(slot, SlotTexture);
                        }
                        GUI.color = Color.white;
                        return;
                    }

                    //Check for overlapping               
                    ModulesToRemove.Clear();
                    foreach (SlotedModule slotmod in SlotedModules)
                    {
                        foreach (Rect slot in HoveredRects)
                        {
                            if (slotmod.baseRect.Contains(slot.center))
                            {
                                if (!ModulesToRemove.Contains(slotmod))
                                    ModulesToRemove.Add(slotmod);
                                continue;
                            }
                        }
                    }

                    if (ModulesToRemove.Count > 0)
                    {
                        GUI.color = Color.yellow;
                        foreach (Rect slot in HoveredRects)
                        {
                            GUI.DrawTexture(slot, SlotTexture);
                        }
                        GUI.color = Color.white;
                    }
                    else
                    {
                        GUI.color = Color.green;
                        foreach (Rect slot in HoveredRects)
                        {
                            GUI.DrawTexture(slot, SlotTexture);
                        }
                        GUI.color = Color.white;
                    }

                    //Click to remove overlaps and place
                    if (Input.GetMouseButton(0))
                    {
                        if (ModulesToRemove.Count > 0)
                        {
                            //Remove any existing modules
                            foreach (SlotedModule slotmod in ModulesToRemove)
                            {
                                SlotedModules.Remove(slotmod);
                            }
                            RecalculateDesignStats();
                            ModulesToRemove.Clear();
                        }
                        //Place Module
                        Rect slotedRect = new Rect(Slots[index].x, Slots[index].y, moduleScale * SizeX, moduleScale * SizeY);
                        Rect imageRect = new Rect(Slots[index].x, Slots[index].y, moduleScale * selectedModule.SizeX, moduleScale * selectedModule.SizeY);
                        if (ModuleRotation == 90 || ModuleRotation == 270)
                        {
                            float offset = (Mathf.Max(SelectedModuleRect.height, SelectedModuleRect.width) - Mathf.Min(SelectedModuleRect.height, SelectedModuleRect.width)) / 2;
                            if (SelectedModuleRect.height > SelectedModuleRect.width)
                            {
                                imageRect.x += offset;
                                imageRect.y -= offset;
                            }
                            else
                            {
                                imageRect.x -= offset;
                                imageRect.y += offset;
                            }
                        }
                        SlotedModule module = new SlotedModule(slotedRect, imageRect, selectedModule, ModuleRotation, index);
                        SlotedModules.Add(module);
                        AddDesignModuleStats(module);
                    }
                    return;
                }
            }
        }
    }

    protected override void CheckModuleRemoval()
    {
        //Selected Module       
        ModulesToRemove.Clear();
        //Modules
        foreach (SlotedModule slotmod in SlotedModules)
        {
            if (slotmod.baseRect.Contains(mousePosition))
            {
                ModulesToRemove.Add(slotmod);
                break;
            }
        }
        if (ModulesToRemove.Count > 0)
        {
            foreach (SlotedModule slotmod in ModulesToRemove)
            {
                SlotedModules.Remove(slotmod);
            }
            RecalculateDesignStats();
        }
        ModulesToRemove.Clear();
    }

    protected override void CheckPlacedModuleSelected()
    {
        if (SlotsAreaRect.Contains(mousePosition))
        {
            foreach (SlotedModule slotmod in SlotedModules)
            {
                if (slotmod.baseRect.Contains(mousePosition))
                {
                    ChangeModule(slotmod.module);
                    ScrollToModuleSet(selectedModuleSet);
                    return;
                }
            }
        } 
    }

    protected override void ClearDesignModuleStats()
    {
        DesignCost = 0;
        DesignProductionCost = 0;
        DesignAlloyCost = 0;
        DesignAdvancedAlloyCost = 0;
        DesignSuperiorAlloyCost = 0;
        DesignCrystalCost = 0;
        DesignRareCrystalCost = 0;
        DesignExoticCrystalCost = 0;
        DesignExoticParticleCost = 0;
        DesignPower = 0;
        DesignPowerGenerated = 0;
        DesignAmmo = 0;
        DesignPowerStorage = 0;
        DesignCrew = selectedHullData.BaseCrew;
        DesignRequiredCrew = 0;
        DesignSensor = selectedHullData.BaseSensor;
        DesignLongRangeSensor = selectedHullData.BaseLongRangeSensor;
        DesignAdvancedSensor = selectedHullData.BaseAdvancedSensor;
        DesignResearch = selectedHullData.BaseResearch;
        DesignMining = 0;
        DesignRepair = selectedHullData.BaseRepair;
        DesignExperience = selectedHullData.ExperienceKillValue;
        DesignDamageBonus = 0;
        DesignDefenseBonus = 0;
        DesignPowerEfficiency = 0;
        DesignAmmoGenerated = 0;
        DesignTroops = 0;
        DesignFighters = 0;
        DesignHeavyFighters = 0;
        DesignAssaultPods = 0;
        DesignFirePowerFighter = 0;
        DesignMedical = 0;
        DesignBoardingDefense = 0;
        DesignTransporter = 0;
        DesignFirePowerAmmo = 0;
        DesignFirePowerPower = 0;
        DesignFirePowerCombination = 0;
        DesignDefenseRating = 0;
        DesignMaxRange = 0;

        DesignHealth = selectedHullData.BaseHealthPerSlot * Slots.Count;
        DesignArmorHealth = selectedHullData.BaseArmorPerSlot * Slots.Count;
        DesignArmorRating = 0;
        DesignShieldHealth = 0;
        DesignShieldRating = 0;
        DesignShieldRecharge = 0;
        DesignShieldDelay = 0;

        DesignJammingCount = 0;
        DesignJammingRange = 0;
        DesignJammingDelay = 0;

        DesignPowerSum = 0;
        DesignLongRangeSensorSum = DesignSensor + DesignLongRangeSensor;
        DesignWeaponAmmoConsumption = 0;
        DesignWeaponPowerConsumption = 0;
        DesignWeaponAmmoTime = 0;
        DesignWeaponPowerTime = 0;
        DesignFirepower = 0;
    }

    protected override void RecalculateDesignStats()
    {
        ClearDesignModuleStats();

        foreach (SlotedModule module in SlotedModules)
        {
            AddDesignModuleStats(module);
        }
    }

    protected void AddDesignModuleStats(SlotedModule slottedModule)
    {
        Module module = slottedModule.module;

        DesignProductionCost += module.ProductionCost;
        DesignAlloyCost += module.AlloyCost;
        DesignAdvancedAlloyCost += module.AdvancedAlloyCost;
        DesignSuperiorAlloyCost += module.SuperiorAlloyCost;
        DesignCrystalCost += module.CrystalCost;
        DesignRareCrystalCost += module.RareCrystalCost;
        DesignExoticCrystalCost += module.ExoticCrystalCost;
        DesignExoticParticleCost += module.ExoticParticleCost;
        DesignCost = GetTotalValue(DesignProductionCost, DesignAlloyCost, DesignAdvancedAlloyCost, DesignSuperiorAlloyCost, DesignCrystalCost, DesignRareCrystalCost, DesignExoticCrystalCost, DesignExoticParticleCost);
        DesignPower += module.Power;
        DesignPowerGenerated += module.PowerGenerated;
        DesignAmmo += module.Ammo;
        DesignPowerStorage += module.PowerStorage;
        DesignCrew += module.Crew;
        DesignRequiredCrew += module.RequiredCrew;
        DesignSensor += module.Sensor;
        DesignLongRangeSensor += module.LongRangeSensor;
        DesignAdvancedSensor += module.AdvancedSensor;
        DesignResearch += module.Research;
        DesignMining += module.Mining;
        DesignRepair += module.Repair;
        DesignDamageBonus = Mathf.Max(DesignDamageBonus, module.DamageBonus);
        DesignDefenseBonus = Mathf.Max(DesignDefenseBonus, module.DefenseBonus);
        DesignPowerEfficiency = Mathf.Max(DesignPowerEfficiency, module.PowerEfficiency);
        DesignAmmoGenerated += module.AmmoGenerated;
        DesignTroops += module.Troops;
        DesignFighters += module.GetFighterCount();
        DesignHeavyFighters += module.GetHeavyFighterCount();
        DesignAssaultPods += module.GetAssaultPodCount();
        DesignFirePowerFighter += module.GetFighterFirePower();

        DesignMedical += module.Medical;
        DesignBoardingDefense += module.BoardingDefense;
        DesignTransporter += module.Transporter;
        DesignDefenseRating += module.GetDefenseRating();

        DesignJammingCount += module.JammingCount;
        if (module.JammingRange > 0)
        {
            if (DesignJammingRange == 0)
                DesignJammingRange = module.GetJammingRangeDisplay();
            else
                DesignJammingRange = Mathf.Min(DesignJammingRange, module.GetJammingRangeDisplay());
        }
        if (module.JammingDelay > 0)
        {
            if (DesignJammingDelay == 0)
                DesignJammingDelay = module.JammingDelay;
            else
                DesignJammingDelay = Mathf.Max(DesignJammingDelay, module.JammingDelay);
        }

        DesignPowerSum = DesignPowerGenerated - (DesignPower - (DesignPower * DesignPowerEfficiency));
        DesignLongRangeSensorSum = DesignSensor + DesignLongRangeSensor;
        Weapon weapon = module.GetWeapon();
        if (weapon != null && weapon.Delay > 0)
        {
            if (weapon.GetMaxRange() > DesignMaxRange)
            {
                DesignMaxRange = weapon.GetMaxRange();
            }
            DesignWeaponAmmoConsumption += weapon.SalvoSize * weapon.AmmoCost / weapon.Delay;
            if (weapon.isBeam)
            {
                DesignWeaponPowerConsumption += weapon.BeamPowerCost * weapon.BeamDuration / weapon.Delay;
            }
            else
            {
                DesignWeaponPowerConsumption += weapon.SalvoSize * weapon.PowerCost / weapon.Delay;
            }

            if (weapon.AmmoCost > 0 && weapon.PowerCost > 0 || weapon.BeamPowerCost > 0)
            {
                DesignFirePowerCombination += weapon.GetAverageDPS();
            }
            else if (weapon.AmmoCost > 0)
            {
                DesignFirePowerAmmo += weapon.GetAverageDPS();
            }
            else if (weapon.PowerCost > 0 || weapon.BeamPowerCost > 0)
            {
                DesignFirePowerPower += weapon.GetAverageDPS();
            }
        }

        CalculateFirePower();
        CalculateDesignWeaponPowerAmmoTime();

        DesignHealth += module.Health;
        DesignArmorHealth += module.ArmorHealth;
        DesignArmorRating += module.ArmorRating;
        DesignShieldHealth += module.ShieldHealth;
        DesignShieldRating += module.ShieldRating;
        DesignShieldRecharge += module.ShieldRechargeRate;
        DesignShieldDelay = Mathf.Max(module.ShieldRechargeDelay, DesignShieldDelay);
                    
    }

    protected override bool CheckValidDesign()
    {
        if (DesignName == "" || DesignPowerSum <= 0 || DesignHealth <= 0 || DesignPowerSum <= 0 || (DesignAmmo == 0 && DesignWeaponAmmoConsumption > 0))
        {
            return false;
        }
        return true;
    }

    protected override void SaveDesign()
    {
        StationDesign design = new StationDesign();

        design.Name = DesignName;
        design.Hull = selectedHullData.Name;

        foreach (SlotedModule slotmod in SlotedModules)
        {
            DesignModule module = new DesignModule(ResourceManager.GetModuleName(slotmod.module), slotmod.Rotation, slotmod.Position);
            design.Modules.Add(module);
        }

        ResourceManager.SaveStationDesign(design);

        BuildDesignList(selectedHullData);
        selectedDesign = design;
        DesignName = selectedDesign.Name;
    }

    protected void DeleteDesign(StationDesign design)
    {
        DesignToDelete = null;
        if (selectedDesign == design)
        {
            selectedDesign = null;
        }
        design.Deleted = true;
        ResourceManager.SaveStationDesign(design);
        BuildDesignList(selectedHullData);
    }

    protected void SetDesignToDelete(StationDesign design)
    {
        DesignToDelete = design;
    }

    protected override void ModuleSwap(ModuleSet newModuleSet)
    {
        SwapModulesInList(newModuleSet, SlotedModules);
        RecalculateDesignStats();
    }

    protected void BuildDesignList(StationHullData data)
    {
        DesignList.Clear();
        selectedDesign = null;
        DesignName = "";

        if (ResourceManager.stationDesigns.ContainsKey(selectedHullData.Name))
        {
            if (ResourceManager.stationDesigns[selectedHullData.Name].Count > 0)
            {
                foreach (KeyValuePair<string, StationDesign> keyVal in ResourceManager.stationDesigns[selectedHullData.Name])
                {
                    if (!keyVal.Value.Deleted && CheckDesignAllowed(keyVal.Value))
                    {
                        Rect rect = new Rect(0, DesignListEntrySize.y * DesignList.Count, DesignListEntrySize.x, DesignListEntrySize.y);
                        StationDesignListEntry DLE = new StationDesignListEntry(rect, keyVal.Value, LoadDesign, SetDesignToDelete, GameManager.instance);
                        DesignList.Add(DLE);
                    }
                }
            }
        }
        ShipDesignScrollViewRect.height = Mathf.Max(ShipDesignScrollWindowRect.height * 1.04f, DesignListEntrySize.y * (DesignList.Count + 1));
    }

    protected override bool CheckModuleSetAllowed(ModuleSet moduleSet)
    {
        if (moduleSet.SetRestriction == ModuleSetRestriction.ShipsOnly)
            return false;
        return true;
    }

    public string GetFormattedDesignHealth()
    {
        return GetStandardDesignStatFormat(DesignHealth);
    }

    public string GetFormattedDesignArmorHealth()
    {
        return GetStandardDesignStatFormat(DesignArmorHealth);
    }

    public string GetFormattedDesignArmorRating()
    {
        return GetStandardDesignStatFormat(DesignArmorRating);
    }

    public string GetFormattedDesignShieldHealth()
    {
        return GetStandardDesignStatFormat(DesignShieldHealth);
    }

    public string GetFormattedDesignShieldRating()
    {
        return GetStandardDesignStatFormat(DesignShieldRating);
    }

    public string GetFormattedDesignShieldRecharge()
    {
        return GetStandardDesignStatFormat(DesignShieldRecharge);
    }

    public string GetFormattedDesignShieldDelay()
    {
        return GetStandardDesignStatFormat(DesignShieldDelay);
    }
}
