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

public abstract class ShipyardBaseScreen : UnitDesignBaseScreen
{
    #region Variables
    protected ShipHullData selectedHullData;
    protected ShipDesign selectedDesign = null;

    //Ships
    protected ShipHullScrollList HullList;
    protected Rect ShipDesignScrollWindowRect;
    protected Rect ShipDesignScrollViewRect;
    protected Vector2 ShipDesignScrollPosition; 

    protected List<DesignListEntry> DesignList = new List<DesignListEntry>();
    protected ShipDesign DesignToDelete = null;

    protected List<Rect> ForeSlots = new List<Rect>();
    protected List<Rect> AftSlots = new List<Rect>();
    protected List<Rect> PortSlots = new List<Rect>();
    protected List<Rect> StarboardSlots = new List<Rect>();
    protected List<Rect> CenterSlots = new List<Rect>();
    protected Rect ForeSlotsAreaRect;
    protected Rect AftSlotsAreaRect;
    protected Rect PortSlotsAreaRect;
    protected Rect StarboardSlotsAreaRect;
    protected Rect CenterSlotsAreaRect;

    //Modules added to current ship
    protected List<SlotedModule> ForeSlotedModules = new List<SlotedModule>();
    protected List<SlotedModule> AftSlotedModules = new List<SlotedModule>();
    protected List<SlotedModule> PortSlotedModules = new List<SlotedModule>();
    protected List<SlotedModule> StarboardSlotedModules = new List<SlotedModule>();
    protected List<SlotedModule> CenterSlotedModules = new List<SlotedModule>();
    protected List<SlotedModule> ModulesToRemove = new List<SlotedModule>();

    //Design stats Header Rects  
    protected Rect DesignMovementHeaderRect;
    protected Rect DesignStealthHeaderRect;
    protected Rect DesignBombingHeaderRect;
    protected Rect DesignForeDefenseHeaderRect;
    protected Rect DesignAftDefenseHeaderRect;
    protected Rect DesignPortDefenseHeaderRect;
    protected Rect DesignStarboardDefenseHeaderRect;
    protected Rect DesignCenterDefenseHeaderRect;

    //Design stats  
    protected float DesignMass;
    protected float DesignFuel;  
    protected float DesignSupplies;
    protected float DesignEngineFTL;
    protected float DesignEngineThrust;
    protected float DesignEngineTurn;
    protected float DesignEngineBonus;  
    protected float DesignDamageBonusFleet;
    protected float DesignDefenseBonusFleet;    
    protected int DesignCommandPointReduction;  
    protected float DesignCloakingPowerPerMass;
    protected float DesignStealth;
    protected int DesignCommandPointBonusFleet;   
    protected float DesignColony;     
    protected float DesignConstruction;
    protected float DesignExperienceBonus;
    protected float DesignBombArmyDamage;
    protected float DesignBombStructureDamage;
    protected float DesignBombPopulationDamage;
    protected float DesignBombPollution;

    //Design stats quadrant specific
    protected float DesignHealthFore;
    protected float DesignArmorHealthFore;
    protected float DesignArmorRatingFore;
    protected float DesignShieldHealthFore;
    protected float DesignShieldRatingFore;
    protected float DesignShieldRechargeFore;
    protected float DesignShieldDelayFore;
    protected float DesignHealthAft;
    protected float DesignArmorHealthAft;
    protected float DesignArmorRatingAft;
    protected float DesignShieldHealthAft;
    protected float DesignShieldRatingAft;
    protected float DesignShieldRechargeAft;
    protected float DesignShieldDelayAft;
    protected float DesignHealthPort;
    protected float DesignArmorHealthPort;
    protected float DesignArmorRatingPort;
    protected float DesignShieldHealthPort;
    protected float DesignShieldRatingPort;
    protected float DesignShieldRechargePort;
    protected float DesignShieldDelayPort;
    protected float DesignHealthStarboard;
    protected float DesignArmorHealthStarboard;
    protected float DesignArmorRatingStarboard;
    protected float DesignShieldHealthStarboard;
    protected float DesignShieldRatingStarboard;
    protected float DesignShieldRechargeStarboard;
    protected float DesignShieldDelayStarboard;
    protected float DesignHealthCenter;
    protected float DesignArmorHealthCenter;
    protected float DesignArmorRatingCenter;
    protected float DesignShieldHealthCenter;
    protected float DesignShieldRatingCenter;
    protected float DesignShieldRechargeCenter;
    protected float DesignShieldDelayCenter;

    //Calculated design stats 
    protected float DesignFTLSpeed;
    protected float DesignSpeed;
    protected float DesignTurnSpeed;  
    protected float DesignCloakingPower;
    protected float DesignCloakingTime;
    protected float DesignEffectiveStealth;
    protected int DesignCommandPoints;

    //Behavoir
    protected bool BehaviorWindowOpen = false;
    protected Rect BehaviorButtonRect;
    protected Rect BehaviorWindowRect;
    protected Rect BehaviorTitleRect;
    protected Rect BehaviorCloseButtonRect;
    protected Rect BehaviorAILabelRect;
    protected Rect BehaviorTypeButtonRect;
    protected Rect MovementButtonRect;
    protected Rect FiringDirectionButtonRect;
    protected Rect RangeValueRect;
    protected DamageBarGraph damageBarGraph;
    protected ShipBehaviorType designBehaviorType;
    protected AttackStyle designBehaviorAttackStyle;
    protected QuadrantTypes designBehaviorAttackDirection;
    protected float designBehaviorAttackRange;
    #endregion

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.B) && !overwriteWarningPopupOpen)
        {
            BehaviorWindowOpen = !BehaviorWindowOpen;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (BehaviorWindowOpen)
            {
                BehaviorWindowOpen = false;
            }
            else if (overwriteWarningPopupOpen)
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
            foreach (DesignListEntry entry in DesignList)
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
                if (ResourceManager.instance.CheckForDesign(selectedHullData.Name, DesignName))
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

            if (BehaviorButtonRect.Contains(mousePosition))
            {
                ToolTip.SetText("designBehavior", "designBehaviorDesc");
            }

            if (GUI.Button(BehaviorButtonRect, "Behavior", GameManager.instance.standardButtonStyle))
            {
                BehaviorWindowOpen = true;
                DeselectModule();
                BuildDamageBarGraph();
                if (designBehaviorAttackRange == -1 || designBehaviorAttackRange > DesignMaxRange)
                {
                    designBehaviorAttackRange = DesignMaxRange;
                }
                PlayMainButtonClick();
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
                if (CheckHullModuleAllow(selectedHullData.ModuleLimitFore, selectedModuleSet.ModuleCategory))
                {
                    GUI.color = Color.white;
                }
                else
                {
                    GUI.color = Color.red;
                }
            }
            foreach (Rect slot in ForeSlots)
            {
                GUI.DrawTexture(slot, SlotTexture);
            }
            if (selectedModule != null)
            {
                if (CheckHullModuleAllow(selectedHullData.ModuleLimitAft, selectedModuleSet.ModuleCategory))
                {
                    GUI.color = Color.white;
                }
                else
                {
                    GUI.color = Color.red;
                }
            }
            foreach (Rect slot in AftSlots)
            {
                GUI.DrawTexture(slot, SlotTexture);
            }
            if (selectedModule != null)
            {
                if (CheckHullModuleAllow(selectedHullData.ModuleLimitPort, selectedModuleSet.ModuleCategory))
                {
                    GUI.color = Color.white;
                }
                else
                {
                    GUI.color = Color.red;
                }
            }
            foreach (Rect slot in PortSlots)
            {
                GUI.DrawTexture(slot, SlotTexture);
            }
            if (selectedModule != null)
            {
                if (CheckHullModuleAllow(selectedHullData.ModuleLimitStarboard, selectedModuleSet.ModuleCategory))
                {
                    GUI.color = Color.white;
                }
                else
                {
                    GUI.color = Color.red;
                }
            }
            foreach (Rect slot in StarboardSlots)
            {
                GUI.DrawTexture(slot, SlotTexture);
            }
            if (selectedModule != null)
            {
                if (CheckHullModuleAllow(selectedHullData.ModuleLimitCenter, selectedModuleSet.ModuleCategory))
                {
                    GUI.color = Color.white;
                }
                else
                {
                    GUI.color = Color.red;
                }
            }
            foreach (Rect slot in CenterSlots)
            {
                GUI.DrawTexture(slot, SlotTexture);
            }
            GUI.color = Color.white;

            //Draw placed modules on top of slots
            foreach (SlotedModule module in ForeSlotedModules)
            {
                module.Draw();
            }
            foreach (SlotedModule module in AftSlotedModules)
            {
                module.Draw();
            }
            foreach (SlotedModule module in PortSlotedModules)
            {
                module.Draw();
            }
            foreach (SlotedModule module in StarboardSlotedModules)
            {
                module.Draw();
            }
            foreach (SlotedModule module in CenterSlotedModules)
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

                    if (GUI.Button(ModuleRotationButtonRect, ResourceManager.instance.GetIconTexture("Icon_Rotate")))
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
            if (BehaviorWindowOpen)
            {
                DrawBehavoirWindow();
            }
            else if (overwriteWarningPopupOpen)
            {
                GUI.Box(overwriteWarningRect, "", GameManager.instance.standardBackGround);
                GUI.Box(overwriteWarningMessageRect, ResourceManager.instance.GetLocalization("overwriteWarning"));

                if (GUI.Button(overwriteAcceptButtonRect, ResourceManager.instance.GetLocalization("overwrite"), GameManager.instance.standardButtonStyle))
                {
                    SaveDesign();
                    overwriteWarningPopupOpen = false;
                    PlayMainButtonClick();
                }
                if (GUI.Button(overwriteCancelButtonRect, ResourceManager.instance.GetLocalization("cancel"), GameManager.instance.standardButtonStyle))
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

        BehaviorButtonRect = new Rect(RightSidePanelRect.x + GameManager.instance.StandardButtonSize.x * 0.15f, CloseButtonRect.y, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);

        Rect ShipHullScrollWindowRect = new Rect(RightSidePanelRect.x + RightSidePanelRect.width * 0.05f, Screen.height * 0.34f * 0.05f, RightSidePanelRect.width * 0.9f, Screen.height * 0.34f * 0.45f);
        Rect ShipHullScrollViewRect = new Rect(0, 0, ShipHullScrollWindowRect.width * 0.9f, ShipHullScrollWindowRect.height * 5f);

        HullList = new ShipHullScrollList(ShipHullScrollWindowRect, ChangeHull, CheckHullAllowed);

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
        Rect DesignCommandPointsRect = new Rect(XPos1, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignMassRect = new Rect(XPos2, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignSuppliesRect = new Rect(XPos3, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignCostRect = new Rect(XPos4, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        YPosition += HeightSpacing;
        Rect DesignCommandPointBonusFleetRect = new Rect(XPos1, YPosition, ModuleStatSize.x, ModuleStatSize.y);
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

        //Movement group rects
        YPosition += HeightSpacing;
        DesignMovementHeaderRect = new Rect(XPos2, YPosition, GameManager.instance.StandardLabelSize.x * 2, ModuleStatSize.y);
        YPosition += HeightSpacing;
        Rect DesignEngineThrustRect = new Rect(XPos1, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignEngineTurnRect = new Rect(XPos2, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignEngineFTLRect = new Rect(XPos3, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignFuelRect = new Rect(XPos4, YPosition, ModuleStatSize.x, ModuleStatSize.y);

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
        DesignForeDefenseHeaderRect = new Rect(XPos2, YPosition, GameManager.instance.StandardLabelSize.x * 2, ModuleStatSize.y);
        YPosition += HeightSpacing;
        Rect DesignHealthForeRect = new Rect(XPos1, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignArmorHealthForeRect = new Rect(XPos2, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignArmorRatingForeRect = new Rect(XPos3, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        YPosition += HeightSpacing;
        Rect DesignShieldHealthForeRect = new Rect(XPos1, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignShieldRatingForeRect = new Rect(XPos2, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignShieldRechargeForeRect = new Rect(XPos3, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignShieldDelayForeRect = new Rect(XPos4, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        YPosition += HeightSpacing;
        DesignAftDefenseHeaderRect = new Rect(XPos2, YPosition, GameManager.instance.StandardLabelSize.x * 2, ModuleStatSize.y);
        YPosition += HeightSpacing;
        Rect DesignHealthAftRect = new Rect(XPos1, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignArmorHealthAftRect = new Rect(XPos2, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignArmorRatingAftRect = new Rect(XPos3, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        YPosition += HeightSpacing;
        Rect DesignShieldHealthAftRect = new Rect(XPos1, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignShieldRatingAftRect = new Rect(XPos2, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignShieldRechargeAftRect = new Rect(XPos3, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignShieldDelayAftRect = new Rect(XPos4, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        YPosition += HeightSpacing;
        DesignPortDefenseHeaderRect = new Rect(XPos2, YPosition, GameManager.instance.StandardLabelSize.x * 2, ModuleStatSize.y);
        YPosition += HeightSpacing;
        Rect DesignHealthPortRect = new Rect(XPos1, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignArmorHealthPortRect = new Rect(XPos2, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignArmorRatingPortRect = new Rect(XPos3, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        YPosition += HeightSpacing;
        Rect DesignShieldHealthPortRect = new Rect(XPos1, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignShieldRatingPortRect = new Rect(XPos2, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignShieldRechargePortRect = new Rect(XPos3, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignShieldDelayPortRect = new Rect(XPos4, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        YPosition += HeightSpacing;
        DesignStarboardDefenseHeaderRect = new Rect(XPos2, YPosition, GameManager.instance.StandardLabelSize.x * 2, ModuleStatSize.y);
        YPosition += HeightSpacing;
        Rect DesignHealthStarboardRect = new Rect(XPos1, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignArmorHealthStarboardRect = new Rect(XPos2, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignArmorRatingStarboardRect = new Rect(XPos3, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        YPosition += HeightSpacing;
        Rect DesignShieldHealthStarboardRect = new Rect(XPos1, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignShieldRatingStarboardRect = new Rect(XPos2, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignShieldRechargeStarboardRect = new Rect(XPos3, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignShieldDelayStarboardRect = new Rect(XPos4, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        YPosition += HeightSpacing;
        DesignCenterDefenseHeaderRect = new Rect(XPos2, YPosition, GameManager.instance.StandardLabelSize.x * 2, ModuleStatSize.y);
        YPosition += HeightSpacing;
        Rect DesignHealthCenterRect = new Rect(XPos1, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignArmorHealthCenterRect = new Rect(XPos2, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignArmorRatingCenterRect = new Rect(XPos3, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        YPosition += HeightSpacing;
        Rect DesignShieldHealthCenterRect = new Rect(XPos1, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignShieldRatingCenterRect = new Rect(XPos2, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignShieldRechargeCenterRect = new Rect(XPos3, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignShieldDelayCenterRect = new Rect(XPos4, YPosition, ModuleStatSize.x, ModuleStatSize.y);

        //Combat bonus group
        YPosition += HeightSpacing;
        DesignCombatBonusHeaderRect = new Rect(XPos2, YPosition, GameManager.instance.StandardLabelSize.x * 2, ModuleStatSize.y);
        YPosition += HeightSpacing;
        Rect DesignDamageBonusRect = new Rect(XPos1, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignDamageBonusFleetRect = new Rect(XPos2, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignDefenseBonusRect = new Rect(XPos3, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignDefenseBonusFleetRect = new Rect(XPos4, YPosition, ModuleStatSize.x, ModuleStatSize.y);

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

        //Bombing group
        YPosition += HeightSpacing;
        DesignBombingHeaderRect = new Rect(XPos2, YPosition, GameManager.instance.StandardLabelSize.x * 2, ModuleStatSize.y);
        YPosition += HeightSpacing;
        Rect DesignBombArmyDamageRect = new Rect(XPos1, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignBombStructureDamageRect = new Rect(XPos2, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignBombPopulationDamageRect = new Rect(XPos3, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignBombPollutionRect = new Rect(XPos4, YPosition, ModuleStatSize.x, ModuleStatSize.y);

        //Stealth group
        YPosition += HeightSpacing;
        DesignStealthHeaderRect = new Rect(XPos2, YPosition, GameManager.instance.StandardLabelSize.x * 2, ModuleStatSize.y);
        YPosition += HeightSpacing;
        Rect DesignStealthRect = new Rect(XPos1, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignCloakingPowerRect = new Rect(XPos2, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignCloakingTimeRect = new Rect(XPos3, YPosition, ModuleStatSize.x, ModuleStatSize.y);

        //Engineering group
        YPosition += HeightSpacing;
        DesignEngineeringHeaderRect = new Rect(XPos2, YPosition, GameManager.instance.StandardLabelSize.x * 2, ModuleStatSize.y);
        YPosition += HeightSpacing;
        Rect DesignRepairRect = new Rect(XPos1, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignPowerEfficiencyRect = new Rect(XPos2, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignTransporterRect = new Rect(XPos3, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignEngineBonusRect = new Rect(XPos4, YPosition, ModuleStatSize.x, ModuleStatSize.y);

        //Misc group
        YPosition += HeightSpacing;
        DesignMiscHeaderRect = new Rect(XPos2, YPosition, GameManager.instance.StandardLabelSize.x * 2, ModuleStatSize.y);
        YPosition += HeightSpacing;
        Rect DesignResearchRect = new Rect(XPos1, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignMiningRect = new Rect(XPos2, YPosition, ModuleStatSize.x, ModuleStatSize.y);     
        Rect DesignBoardingDefenseRect = new Rect(XPos3, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignColonyRect = new Rect(XPos4, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        YPosition += HeightSpacing;
        Rect DesignDiplomacyRect = new Rect(XPos1, YPosition, ModuleStatSize.x, ModuleStatSize.y);
        Rect DesignConstructionRect = new Rect(XPos2, YPosition, ModuleStatSize.x, ModuleStatSize.y);      
        Rect DesignExperienceBonusRect = new Rect(XPos3, YPosition, ModuleStatSize.x, ModuleStatSize.y);

        YPosition += ModuleStatSize.y * 1.5f;
        DesignStatsViewRect.height = YPosition;

        

        //Design Stat Entries
        DesignStatList.Add(new DesignIconStatEntry(DesignCrewRect, "Icon_Crew", GetFormatedDesignRequiredCrew, "crew", "shipCrew", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignTroopsRect, "Icon_Troop", GetFormatedDesignTroops, "troops", "shipTroops", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignCostRect, "Icon_Money", GetFormattedDesignCost, "shipCost", "shipCostDesc", ToolTip));

        DesignStatList.Add(new DesignIconStatEntry(DesignCommandPointsRect, "Icon_CommandPoint", GetFormattedDesignCommandPoints, "commandPoints", "shipCommandPoints", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignCommandPointBonusFleetRect, "Icon_CommandPointFleet", GetFormattedDesignCommandPointBonusFleet, "fleetCommandPoints", "shipFleetCommandPoints", ToolTip));
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

        DesignStatList.Add(new DesignIconStatEntry(DesignEngineThrustRect, "Icon_EngineThrust", GetFormattedDesignSpeed, "speedSTL", "shipSpeedSTL", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignEngineTurnRect, "Icon_EngineTurn", GetFormattedDesignTurnSpeed, "speedRotation", "shipSpeedRotation", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignEngineFTLRect, "Icon_FTL", GetFormattedDesignFTLSpeed, "speedFTL", "shipSpeedFTL", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignFuelRect, "Icon_Fuel", GetFormattedDesignFuel, "fuel", "shipFuel", ToolTip));

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

        DesignStatList.Add(new DesignIconStatEntry(DesignHealthForeRect, "Icon_Health", GetFormattedDesignHealthFore, "health", "shipHealth", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignArmorHealthForeRect, "Icon_ArmorHealth", GetFormattedDesignArmorHealthFore, "armorHealth", "shipArmorHealth", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignArmorRatingForeRect, "Icon_ArmorRating", GetFormattedDesignArmorRatingFore, "armorRating", "shipArmorRating", ToolTip));

        DesignStatList.Add(new DesignIconStatEntry(DesignShieldHealthForeRect, "Icon_ShieldHealth", GetFormattedDesignShieldHealthFore, "shieldHealth", "shipShieldHealth", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignShieldRatingForeRect, "Icon_ShieldRating", GetFormattedDesignShieldRatingFore, "shieldRating", "shipShieldRating", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignShieldRechargeForeRect, "Icon_ShieldRecharge", GetFormattedDesignShieldRechargeFore, "shieldRecharge", "shipShieldRecharge", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignShieldDelayForeRect, "Icon_ShieldDelay", GetFormattedDesignShieldDelayFore, "shieldRechargeDelay", "shipShieldRechargeDelay", ToolTip));

        DesignStatList.Add(new DesignIconStatEntry(DesignHealthAftRect, "Icon_Health", GetFormattedDesignHealthAft, "health", "shipHealth", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignArmorHealthAftRect, "Icon_ArmorHealth", GetFormattedDesignArmorHealthAft, "armorHealth", "shipArmorHealth", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignArmorRatingAftRect, "Icon_ArmorRating", GetFormattedDesignArmorRatingAft, "armorRating", "shipArmorRating", ToolTip));

        DesignStatList.Add(new DesignIconStatEntry(DesignShieldHealthAftRect, "Icon_ShieldHealth", GetFormattedDesignShieldHealthAft, "shieldHealth", "shipShieldHealth", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignShieldRatingAftRect, "Icon_ShieldRating", GetFormattedDesignShieldRatingAft, "shieldRating", "shipShieldRating", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignShieldRechargeAftRect, "Icon_ShieldRecharge", GetFormattedDesignShieldRechargeAft, "shieldRecharge", "shipShieldRecharge", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignShieldDelayAftRect, "Icon_ShieldDelay", GetFormattedDesignShieldDelayAft, "shieldRechargeDelay", "shipShieldRechargeDelay", ToolTip));

        DesignStatList.Add(new DesignIconStatEntry(DesignHealthPortRect, "Icon_Health", GetFormattedDesignHealthPort, "health", "shipHealth", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignArmorHealthPortRect, "Icon_ArmorHealth", GetFormattedDesignArmorHealthPort, "armorHealth", "shipArmorHealth", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignArmorRatingPortRect, "Icon_ArmorRating", GetFormattedDesignArmorRatingPort, "armorRating", "shipArmorRating", ToolTip));

        DesignStatList.Add(new DesignIconStatEntry(DesignShieldHealthPortRect, "Icon_ShieldHealth", GetFormattedDesignShieldHealthPort, "shieldHealth", "shipShieldHealth", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignShieldRatingPortRect, "Icon_ShieldRating", GetFormattedDesignShieldRatingPort, "shieldRating", "shipShieldRating", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignShieldRechargePortRect, "Icon_ShieldRecharge", GetFormattedDesignShieldRechargePort, "shieldRecharge", "shipShieldRecharge", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignShieldDelayPortRect, "Icon_ShieldDelay", GetFormattedDesignShieldDelayPort, "shieldRechargeDelay", "shipShieldRechargeDelay", ToolTip));

        DesignStatList.Add(new DesignIconStatEntry(DesignHealthStarboardRect, "Icon_Health", GetFormattedDesignHealthStarboard, "health", "shipHealth", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignArmorHealthStarboardRect, "Icon_ArmorHealth", GetFormattedDesignArmorHealthStarboard, "armorHealth", "shipArmorHealth", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignArmorRatingStarboardRect, "Icon_ArmorRating", GetFormattedDesignArmorRatingStarboard, "armorRating", "shipArmorRating", ToolTip));

        DesignStatList.Add(new DesignIconStatEntry(DesignShieldHealthStarboardRect, "Icon_ShieldHealth", GetFormattedDesignShieldHealthStarboard, "shieldHealth", "shipShieldHealth", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignShieldRatingStarboardRect, "Icon_ShieldRating", GetFormattedDesignShieldRatingStarboard, "shieldRating", "shipShieldRating", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignShieldRechargeStarboardRect, "Icon_ShieldRecharge", GetFormattedDesignShieldRechargeStarboard, "shieldRecharge", "shipShieldRecharge", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignShieldDelayStarboardRect, "Icon_ShieldDelay", GetFormattedDesignShieldDelayStarboard, "shieldRechargeDelay", "shipShieldRechargeDelay", ToolTip));

        DesignStatList.Add(new DesignIconStatEntry(DesignHealthCenterRect, "Icon_Health", GetFormattedDesignHealthCenter, "health", "shipHealth", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignArmorHealthCenterRect, "Icon_ArmorHealth", GetFormattedDesignArmorHealthCenter, "armorHealth", "shipArmorHealth", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignArmorRatingCenterRect, "Icon_ArmorRating", GetFormattedDesignArmorRatingCenter, "armorRating", "shipArmorRating", ToolTip));

        DesignStatList.Add(new DesignIconStatEntry(DesignShieldHealthCenterRect, "Icon_ShieldHealth", GetFormattedDesignShieldHealthCenter, "shieldHealth", "shipShieldHealth", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignShieldRatingCenterRect, "Icon_ShieldRating", GetFormattedDesignShieldRatingCenter, "shieldRating", "shipShieldRating", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignShieldRechargeCenterRect, "Icon_ShieldRecharge", GetFormattedDesignShieldRechargeCenter, "shieldRecharge", "shipShieldRecharge", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignShieldDelayCenterRect, "Icon_ShieldDelay", GetFormattedDesignShieldDelayCenter, "shieldRechargeDelay", "shipShieldRechargeDelay", ToolTip));

        DesignStatList.Add(new DesignIconStatEntry(DesignFightersRect, "Icon_Fighter", GetFormattedDesignFighters, "figters", "shipFighters", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignHeavyFightersRect, "Icon_HeavyFighter", GetFormattedDesignHeavyFighters, "heavyFighters", "shipHeavyFighters", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignAssaultPodsRect, "Icon_AssaultPod", GetFormattedDesignAssaultPods, "assaultPods", "shipAssaultPods", ToolTip));

        DesignStatList.Add(new DesignIconStatEntry(DesignDamageBonusRect, "Icon_Damage", GetFormattedDesignDamageBonus, "damageBonus", "shipDamageBonus", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignDamageBonusFleetRect, "Icon_DamageFleet", GetFormattedDesignDamageBonusFleet, "fleetDamageBonus", "fleetDamageBonusDesc", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignDefenseBonusRect, "Icon_Defense", GetFormattedDesignDefenseBonus, "defenseBonus", "shipDefenseBonus", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignDefenseBonusFleetRect, "Icon_DefenseFleet", GetFormattedDesignDefenseBonusFleet, "fleetDefenseBonus", "fleetDefenseBonusDesc", ToolTip));

        DesignStatList.Add(new DesignIconStatEntry(DesignJammingCountRect, "Icon_Jamming", GetFormattedDesignJammingCount, "JammingCount", "shipJammingCount", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignJammingRangeRect, "Icon_JammingRange", GetFormattedDesignJammingRange, "JammingRange", "shipJammingRange", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignJammingDelayRect, "Icon_JammingDelay", GetFormattedDesignJammingDelay, "JammingDelay", "shipJammingDelay", ToolTip));

        DesignStatList.Add(new DesignIconStatEntry(DesignBombArmyDamageRect, "Icon_BombArmy", GetFormattedDesignBombArmyDamage, "BombingArmyDamage", "BombingArmyDamageDesc", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignBombStructureDamageRect, "Icon_BombStructure", GetFormattedDesignBombStructureDamage, "BombingStructureDamage", "BombingStructureDamageDesc", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignBombPopulationDamageRect, "Icon_BombPopulation", GetFormattedDesignBombPopulationDamage, "BombingPopulationDamage", "BombingPopulationDamageDesc", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignBombPollutionRect, "Icon_BombPolution", GetFormattedDesignBombPollution, "BombingPollution", "BombingPollutionDesc", ToolTip));

        DesignStatList.Add(new DesignIconStatEntry(DesignStealthRect, "Icon_Stealth", GetFormattedDesignEffectiveStealth, "stealth", "shipStealth", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignCloakingPowerRect, "Icon_CloakPower", GetFormattedDesignCloakingPower, "cloakingPower", "shipCloakingPower", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignCloakingTimeRect, "Icon_CloakTime", GetFormattedDesignCloakingTime, "cloakingTime", "shipCloakingTime", ToolTip));


        DesignStatList.Add(new DesignIconStatEntry(DesignMassRect, "Icon_Mass", GetFormattedDesignMass, "mass", "shipMass", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignSuppliesRect, "Icon_Supplies", GetFormattedDesignSupplies, "supplies", "shipSupplies", ToolTip));


        DesignStatList.Add(new DesignIconStatEntry(DesignResearchRect, "Icon_Research", GetFormattedDesignResearch, "research", "shipResearch", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignMedicalRect, "Icon_Medical", GetFormattedDesignMedical, "medical", "shipMedical", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignTransporterRect, "Icon_Transporter", GetFormattedDesignTransporter, "transporter", "shipTransporter", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignMiningRect, "Icon_Mining", GetFormattedDesignMining, "mining", "shipMining", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignRepairRect, "Icon_Repair", GetFormattedDesignRepair, "repairRate", "shipRepairRate", ToolTip));


        DesignStatList.Add(new DesignIconStatEntry(DesignBoardingDefenseRect, "Icon_BoardingDefense", GetFormattedDesignBoardingDefense, "boardingDefense", "shipBoardingDefense", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignColonyRect, "Icon_Colony", GetFormattedDesignColony, "colonySupplies", "shipColonySupplies", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignDiplomacyRect, "Icon_Diplomacy", GetFormattedDesignDiplomacy, "diplomacy", "shipDiplomacy", ToolTip));


        DesignStatList.Add(new DesignIconStatEntry(DesignConstructionRect, "Icon_Construction", GetFormattedDesignConstruction, "construction", "shipConstruction", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignExperienceRect, "Icon_Level", GetFormattedDesignExperience, "killExperience", "killExperienceDesc", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignExperienceBonusRect, "Icon_LevelBonus", GetFormattedDesignExperienceBonus, "experienceBonus", "experienceBonusDesc", ToolTip));

        DesignStatList.Add(new DesignIconStatEntry(DesignPowerEfficiencyRect, "Icon_PowerEfficiency", GetFormattedDesignPowerEfficiency, "powerEfficiency", "powerEfficiencyDesc", ToolTip));
        DesignStatList.Add(new DesignIconStatEntry(DesignEngineBonusRect, "Icon_EngineBonus", GetFormattedDesignEngineBonus, "engineBonus", "modEngineBonus", ToolTip));

        //Behavior Window
        BehaviorWindowRect = new Rect(Screen.width * 0.375f, Screen.height * 0.333f, Screen.width * 0.25f, Screen.height * 0.334f);
        float BehaviorButtonSize = BehaviorWindowRect.width / 7f;
        float BehaviorSpacing = BehaviorButtonSize * 0.15f;
        BehaviorTitleRect = new Rect(BehaviorWindowRect.x, BehaviorWindowRect.y, BehaviorWindowRect.width, BehaviorWindowRect.height * 0.1f);
        BehaviorCloseButtonRect = new Rect(BehaviorWindowRect.xMax - GameManager.instance.StandardButtonSize.x * 1.1f, BehaviorWindowRect.yMax - GameManager.instance.StandardButtonSize.y * 1.1f, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);

        BehaviorAILabelRect = new Rect(BehaviorWindowRect.x + GameManager.instance.StandardLabelSize.x * 0.1f, BehaviorTitleRect.yMax + GameManager.instance.StandardLabelSize.y * 0.1f, GameManager.instance.StandardLabelSize.x, GameManager.instance.StandardLabelSize.y);
        BehaviorTypeButtonRect = new Rect(BehaviorAILabelRect.x, BehaviorAILabelRect.yMax, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);

        MovementButtonRect = new Rect(BehaviorTypeButtonRect.xMax + BehaviorSpacing, BehaviorAILabelRect.y + BehaviorSpacing, BehaviorButtonSize, BehaviorButtonSize);
        FiringDirectionButtonRect = new Rect(MovementButtonRect.xMax + BehaviorSpacing, MovementButtonRect.y, BehaviorButtonSize, BehaviorButtonSize);
        RangeValueRect = new Rect(MovementButtonRect.x, MovementButtonRect.yMax + BehaviorSpacing * 2f, GameManager.instance.StandardLabelSize.x * 1.3f, GameManager.instance.StandardLabelSize.y * 1.25f);
        damageBarGraph = new DamageBarGraph(new Rect(RangeValueRect.x, RangeValueRect.yMax, BehaviorButtonSize * 3f, BehaviorButtonSize * 0.7f));

        overwriteWarningRect = new Rect(Screen.width * 0.35f, Screen.height * 0.333f, Screen.width * 0.3f, Screen.height * 0.125f);
        overwriteWarningMessageRect = new Rect(overwriteWarningRect.x, overwriteWarningRect.y, overwriteWarningRect.width, overwriteWarningRect.height * 0.4f);
        overwriteAcceptButtonRect = new Rect(overwriteWarningRect.x + overwriteWarningRect.width / 2f - GameManager.instance.StandardButtonSize.x * 1.5f, overwriteWarningMessageRect.yMax + overwriteWarningMessageRect.height * 0.225f, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);
        overwriteCancelButtonRect = new Rect(overwriteAcceptButtonRect.xMax + GameManager.instance.StandardButtonSize.x, overwriteAcceptButtonRect.y, GameManager.instance.StandardButtonSize.x, GameManager.instance.StandardButtonSize.y);

        BuildModuleSetLists();
        ChangeModuleSetList(ModuleListTypes.Weapon);

        HullList.CheckFirstHull(ChangeHull);
        ResetBehavior();
    }

    protected override void CheckPlacedModuleHover()
    {
        if (ForeSlotsAreaRect.Contains(mousePosition))
        {
            foreach (SlotedModule module in ForeSlotedModules)
            {
                if (module.baseRect.Contains(mousePosition))
                {
                    Weapon weapon = module.module.GetWeapon();
                    if (weapon != null)
                    {
                        GUI.DrawTexture(WeaponArcRect, WeaponArcCircle);
                        SetWeaponArcTexture((int)weapon.Arc);
                        if (weapon.AlwaysForward && module.Rotation != 0)
                        {
                            Matrix4x4 matrix = GUI.matrix;
                            GUIUtility.RotateAroundPivot(module.Rotation, WeaponArcRect.center);
                            GUI.DrawTexture(WeaponArcRect, WeaponArcTex);
                            GUI.matrix = matrix;
                        }
                        else
                        {
                            GUI.DrawTexture(WeaponArcRect, WeaponArcTex);
                        }
                    }
                    DrawHoveredModuleInfo(mousePosition, module.module);
                    return;
                }
            }
        }
        else if (AftSlotsAreaRect.Contains(mousePosition))
        {
            foreach (SlotedModule module in AftSlotedModules)
            {
                if (module.baseRect.Contains(mousePosition))
                {
                    Weapon weapon = module.module.GetWeapon();
                    if (weapon != null)
                    {
                        GUI.DrawTexture(WeaponArcRect, WeaponArcCircle);
                        SetWeaponArcTexture((int)weapon.Arc);
                        if (weapon.AlwaysForward)
                        {
                            if (module.Rotation != 0)
                            {
                                Matrix4x4 matrix = GUI.matrix;
                                GUIUtility.RotateAroundPivot(module.Rotation, WeaponArcRect.center);
                                GUI.DrawTexture(WeaponArcRect, WeaponArcTex);
                                GUI.matrix = matrix;
                            }
                            else
                            {
                                GUI.DrawTexture(WeaponArcRect, WeaponArcTex);
                            }
                        }
                        else
                        {
                            Matrix4x4 matrix = GUI.matrix;
                            GUIUtility.RotateAroundPivot(180, WeaponArcRect.center);
                            GUI.DrawTexture(WeaponArcRect, WeaponArcTex);
                            GUI.matrix = matrix;
                        }
                    }
                    DrawHoveredModuleInfo(mousePosition, module.module);
                    return;
                }
            }
        }
        else if (PortSlotsAreaRect.Contains(mousePosition))
        {
            foreach (SlotedModule module in PortSlotedModules)
            {
                if (module.baseRect.Contains(mousePosition))
                {
                    Weapon weapon = module.module.GetWeapon();
                    if (weapon != null)
                    {
                        GUI.DrawTexture(WeaponArcRect, WeaponArcCircle);
                        SetWeaponArcTexture((int)weapon.Arc);
                        if (weapon.AlwaysForward)
                        {
                            if (module.Rotation != 0)
                            {
                                Matrix4x4 matrix = GUI.matrix;
                                GUIUtility.RotateAroundPivot(module.Rotation, WeaponArcRect.center);
                                GUI.DrawTexture(WeaponArcRect, WeaponArcTex);
                                GUI.matrix = matrix;
                            }
                            else
                            {
                                GUI.DrawTexture(WeaponArcRect, WeaponArcTex);
                            }
                        }
                        else
                        {
                            Matrix4x4 matrix = GUI.matrix;
                            GUIUtility.RotateAroundPivot(270, WeaponArcRect.center);
                            GUI.DrawTexture(WeaponArcRect, WeaponArcTex);
                            GUI.matrix = matrix;
                        }
                    }
                    DrawHoveredModuleInfo(mousePosition, module.module);
                    return;
                }
            }
        }
        else if (StarboardSlotsAreaRect.Contains(mousePosition))
        {
            foreach (SlotedModule module in StarboardSlotedModules)
            {
                if (module.baseRect.Contains(mousePosition))
                {
                    Weapon weapon = module.module.GetWeapon();
                    if (weapon != null)
                    {
                        GUI.DrawTexture(WeaponArcRect, WeaponArcCircle);
                        SetWeaponArcTexture((int)weapon.Arc);
                        if (weapon.AlwaysForward)
                        {
                            if (module.Rotation != 0)
                            {
                                Matrix4x4 matrix = GUI.matrix;
                                GUIUtility.RotateAroundPivot(module.Rotation, WeaponArcRect.center);
                                GUI.DrawTexture(WeaponArcRect, WeaponArcTex);
                                GUI.matrix = matrix;
                            }
                            else
                            {
                                GUI.DrawTexture(WeaponArcRect, WeaponArcTex);
                            }
                        }
                        else
                        {
                            Matrix4x4 matrix = GUI.matrix;
                            GUIUtility.RotateAroundPivot(90, WeaponArcRect.center);
                            GUI.DrawTexture(WeaponArcRect, WeaponArcTex);
                            GUI.matrix = matrix;
                        }
                    }
                    DrawHoveredModuleInfo(mousePosition, module.module);
                    return;
                }
            }
        }
        else if (CenterSlotsAreaRect.Contains(mousePosition))
        {
            foreach (SlotedModule module in CenterSlotedModules)
            {
                if (module.baseRect.Contains(mousePosition))
                {
                    Weapon weapon = module.module.GetWeapon();
                    if (weapon != null)
                    {
                        GUI.DrawTexture(WeaponArcRect, WeaponArcCircle);
                        GUI.DrawTexture(WeaponArcRect, ResourceManager.instance.GetUITexture("WeaponArc360"));
                    }
                    DrawHoveredModuleInfo(mousePosition, module.module);
                    return;
                }
            }
        }
    }

    protected void DrawBehavoirWindow()
    {
        GUI.Box(BehaviorWindowRect, "", GameManager.instance.standardBackGround);
        GUI.Box(BehaviorTitleRect, "Ship Behavior");

        GUI.Label(BehaviorAILabelRect, "AI Profile");
        if (designBehaviorType == ShipBehaviorType.BestGuess)
        {
            if (BehaviorTypeButtonRect.Contains(mousePosition))
            {
                ToolTip.SetText("bestGuess", "bestGuessDesc");
            }
            if (GUI.Button(BehaviorTypeButtonRect, ResourceManager.instance.GetLocalization("bestGuess")))
            {
                designBehaviorType = ShipBehaviorType.Skirmisher;
                PlayMainButtonClick();
            }
        }
        else if (designBehaviorType == ShipBehaviorType.Skirmisher)
        {
            if (BehaviorTypeButtonRect.Contains(mousePosition))
            {
                ToolTip.SetText("skirmisher", "skirmisherDesc");
            }
            if (GUI.Button(BehaviorTypeButtonRect, ResourceManager.instance.GetLocalization("skirmisher")))
            {
                designBehaviorType = ShipBehaviorType.Brawler;
                PlayMainButtonClick();
            }
        }
        else if (designBehaviorType == ShipBehaviorType.Brawler)
        {
            if (BehaviorTypeButtonRect.Contains(mousePosition))
            {
                ToolTip.SetText("brawler", "brawlerDesc");
            }
            if (GUI.Button(BehaviorTypeButtonRect, ResourceManager.instance.GetLocalization("brawler")))
            {
                designBehaviorType = ShipBehaviorType.Artillery;
                PlayMainButtonClick();
            }
        }
        else if (designBehaviorType == ShipBehaviorType.Artillery)
        {
            if (BehaviorTypeButtonRect.Contains(mousePosition))
            {
                ToolTip.SetText("artillery", "artilleryDesc");
            }
            if (GUI.Button(BehaviorTypeButtonRect, ResourceManager.instance.GetLocalization("artillery")))
            {
                designBehaviorType = ShipBehaviorType.Support;
                PlayMainButtonClick();
            }
        }
        else
        {
            if (BehaviorTypeButtonRect.Contains(mousePosition))
            {
                ToolTip.SetText("support", "supportDesc");
            }
            if (GUI.Button(BehaviorTypeButtonRect, ResourceManager.instance.GetLocalization("support")))
            {
                designBehaviorType = ShipBehaviorType.BestGuess;
                PlayMainButtonClick();
            }
        }

        if (designBehaviorAttackStyle == AttackStyle.holdPosition)
        {
            if (MovementButtonRect.Contains(mousePosition))
            {
                ToolTip.SetText("holdPosition", "holdPositionDesc");
            }
            if (GUI.Button(MovementButtonRect, ResourceManager.instance.GetIconTexture("Icon_ShipHoldPositionAttack")))
            {
                designBehaviorAttackStyle = AttackStyle.strafingRuns;
                PlayMainButtonClick();
            }
        }
        else if (designBehaviorAttackStyle == AttackStyle.strafingRuns)
        {
            if (MovementButtonRect.Contains(mousePosition))
            {
                ToolTip.SetText("strafeAttack", "strafeAttackDesc");
            }
            if (GUI.Button(MovementButtonRect, ResourceManager.instance.GetIconTexture("Icon_StrafingAttack")))
            {
                designBehaviorAttackStyle = AttackStyle.none;
                PlayMainButtonClick();
            }
        }
        else
        {
            if (MovementButtonRect.Contains(mousePosition))
            {
                ToolTip.SetText("dontAttack", "dontAttackDesc");
            }
            if (GUI.Button(MovementButtonRect, ResourceManager.instance.GetIconTexture("Icon_DontAttack")))
            {
                designBehaviorAttackStyle = AttackStyle.holdPosition;
                PlayMainButtonClick();
            }
        }

        if (designBehaviorAttackDirection == QuadrantTypes.Fore)
        {
            if (FiringDirectionButtonRect.Contains(mousePosition))
            {
                ToolTip.SetText("attackFront", "attackFrontDesc");
            }
            if (GUI.Button(FiringDirectionButtonRect, ResourceManager.instance.GetIconTexture("Icon_ShipAttackForward")))
            {
                designBehaviorAttackDirection = QuadrantTypes.Port;
                PlayMainButtonClick();
            }
        }
        else if (designBehaviorAttackDirection == QuadrantTypes.Port)
        {
            if (FiringDirectionButtonRect.Contains(mousePosition))
            {
                ToolTip.SetText("attackLeft", "attackLeftDesc");
            }
            if (GUI.Button(FiringDirectionButtonRect, ResourceManager.instance.GetIconTexture("Icon_ShipAttackLeft")))
            {
                designBehaviorAttackDirection = QuadrantTypes.Starboard;
                PlayMainButtonClick();
            }
        }
        else
        {
            if (FiringDirectionButtonRect.Contains(mousePosition))
            {
                ToolTip.SetText("attackRight", "attackRightDesc");
            }
            if (GUI.Button(FiringDirectionButtonRect, ResourceManager.instance.GetIconTexture("Icon_ShipAttackRight")))
            {
                designBehaviorAttackDirection = QuadrantTypes.Fore;
                PlayMainButtonClick();
            }
        }

        if (damageBarGraph.Contains(mousePosition))
        {
            ToolTip.SetText("attackRange", "attackRangeDesc");
        }

        GUI.Label(RangeValueRect, "Range: " + (designBehaviorAttackRange / ResourceManager.instance.GetGameConstants().FiringRangeFactor).ToString("0"));
        designBehaviorAttackRange = damageBarGraph.Draw();

        if (GUI.Button(BehaviorCloseButtonRect, "Close", GameManager.instance.standardButtonStyle))
        {
            BehaviorWindowOpen = false;
            PlayMainButtonClick();
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
        if (DesignSpeed > 0 && DesignTurnSpeed > 0)
        {
            GUI.Label(DesignMovementHeaderRect, "Movement");
        }
        else
        {
            Color guiColor = GUI.color;
            GUI.color = Color.red;
            GUI.Label(DesignMovementHeaderRect, "Movement");
            GUI.color = guiColor;
        }
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
        GUI.Label(DesignForeDefenseHeaderRect, "Fore Defense");
        GUI.Label(DesignAftDefenseHeaderRect, "Aft Defense");
        GUI.Label(DesignPortDefenseHeaderRect, "Port Defense");
        GUI.Label(DesignStarboardDefenseHeaderRect, "Starboard Defense");
        GUI.Label(DesignCenterDefenseHeaderRect, "Center Defense");
        GUI.Label(DesignHangarHeaderRect, "Hangar");
        GUI.Label(DesignCombatBonusHeaderRect, "Combat Bonuses");
        GUI.Label(DesignJammingHeaderRect, "Jamming Defense");
        GUI.Label(DesignBombingHeaderRect, "Planetary Bombing");
        GUI.Label(DesignStealthHeaderRect, "Stealth");
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
                DrawShipClassificationTooltip(selectedHullData.Classification);
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

    protected void ChangeHull(ShipHullData hullData)
    {
        ClearFormatedSlots();
        ClearSlottedModules();
        selectedHullData = hullData;
        FormatSlotLayout(ResourceManager.instance.GetShipSlotLayout(hullData.Name));
        SetShipModel(hullData);
        BuildDesignList(selectedHullData);
        ClearDesignModuleStats();
        ShipDesignScrollPosition = Vector2.zero;

        FormattedHullName = selectedHullData.Name + " - " + selectedHullData.Classification.ToString();
        ResetBehavior();
    }

    protected void LoadDesign(ShipDesign design)
    {
        selectedDesign = design;
        DesignName = selectedDesign.Name;

        designBehaviorType = design.AI_Profile;
        designBehaviorAttackDirection = design.attackDirection;
        designBehaviorAttackStyle = design.attackStyle;
        designBehaviorAttackRange = design.attackRange;

        ClearSlottedModules();

        foreach (DesignModule designMod in design.ForeModules)
        {
            if(designMod.Position < ForeSlots.Count)
            {
                SlotedModule slotedModule = SlotedModule.CreateSlotedModule(designMod, ForeSlots[designMod.Position]);
                if(slotedModule != null)
                {
                    ForeSlotedModules.Add(slotedModule);
                }
            }
        }
        foreach (DesignModule designMod in design.AftModules)
        {
            if (designMod.Position < AftSlots.Count)
            {
                SlotedModule slotedModule = SlotedModule.CreateSlotedModule(designMod, AftSlots[designMod.Position]);
                if (slotedModule != null)
                {
                    AftSlotedModules.Add(slotedModule);
                }
            }
        }
        foreach (DesignModule designMod in design.PortModules)
        {
            if (designMod.Position < PortSlots.Count)
            {
                SlotedModule slotedModule = SlotedModule.CreateSlotedModule(designMod, PortSlots[designMod.Position]);
                if (slotedModule != null)
                {
                    PortSlotedModules.Add(slotedModule);
                }
            }
        }
        foreach (DesignModule designMod in design.StarboardModules)
        {
            if (designMod.Position < StarboardSlots.Count)
            {
                SlotedModule slotedModule = SlotedModule.CreateSlotedModule(designMod, StarboardSlots[designMod.Position]);
                if (slotedModule != null)
                {
                    StarboardSlotedModules.Add(slotedModule);
                }
            }
        }
        foreach (DesignModule designMod in design.CenterModules)
        {
            if (designMod.Position < CenterSlots.Count)
            {
                SlotedModule slotedModule = SlotedModule.CreateSlotedModule(designMod, CenterSlots[designMod.Position]);
                if (slotedModule != null)
                {
                    CenterSlotedModules.Add(slotedModule);
                }
            }
        }
        RecalculateDesignStats();
    }

    protected void FormatSlotLayout(ShipSlotLayout sourceLayout)
    {
        moduleScale = 32f;

        ClearFormatedSlots();
        ClearSlottedModules();

        Vector2 ForePosition, AftPosition, PortPosition, StarboardPosition, CenterPosition;
        Vector2 ForeSize, AftSize, PortSize, StarboardSize, CenterSize;
        float MinX, MaxX, MinY, MaxY;
        Vector2 ForeMin, AftMin, PortMin, StarboardMin, CenterMin;

        //Get size of center quadrant
        if (sourceLayout.CenterSlots.Count > 0)
        {
            Rect CenterOuterRect = sourceLayout.GetCenterOuterRect();
            CenterSize = new Vector2(CenterOuterRect.width / 16, CenterOuterRect.height / 16);
            CenterMin = CenterOuterRect.position;
        }
        else
        {
            CenterSize = Vector2.zero;
            CenterMin = Vector2.zero;
        }

        //Get size of port quadrant
        if (sourceLayout.PortSlots.Count > 0)
        {
            Rect PortOuterRect = sourceLayout.GetPortOuterRect();

            PortSize = new Vector2(PortOuterRect.width / 16, PortOuterRect.height / 16);
            PortMin = PortOuterRect.position;
        }
        else
        {
            PortSize = Vector2.zero;
            PortMin = Vector2.zero;
        }

        //Get size of starboard quadrant
        if (sourceLayout.StarboardSlots.Count > 0)
        {
            Rect StarboardOuterRect = sourceLayout.GetStarboardOuterRect();

            StarboardSize = new Vector2(StarboardOuterRect.width / 16, StarboardOuterRect.height / 16);
            StarboardMin = StarboardOuterRect.position;
        }
        else
        {
            StarboardSize = Vector2.zero;
            StarboardMin = Vector2.zero;
        }

        //Get size of fore quadrant
        if (sourceLayout.ForeSlots.Count > 0)
        {
            Rect ForeOuterRect = sourceLayout.GetForeOuterRect();

            ForeSize = new Vector2(ForeOuterRect.width / 16, ForeOuterRect.height / 16);
            ForeMin = ForeOuterRect.position;
        }
        else
        {
            ForeSize = Vector2.zero;
            ForeMin = Vector2.zero;
        }

        //Get size of aft quadrant
        if (sourceLayout.AftSlots.Count > 0)
        {
            Rect AftOuterRect = sourceLayout.GetAftOuterRect();

            AftSize = new Vector2(AftOuterRect.width / 16, AftOuterRect.height / 16);
            AftMin = AftOuterRect.position;
        }
        else
        {
            AftSize = Vector2.zero;
            AftMin = Vector2.zero;
        }

        //Determine scale of modules
        float MaxSlotsVertical = 1 + ForeSize.y + AftSize.y + Mathf.Max(CenterSize.y, PortSize.y, StarboardSize.y);
        float MaxSlotsHorizontal = Mathf.Max(ForeSize.x, AftSize.x, 1 + CenterSize.x + PortSize.x + StarboardSize.x);
        if (MaxSlotsVertical * moduleScale > Screen.height * 0.9f)
        {
            moduleScale = Screen.height * 0.9f / MaxSlotsVertical;
        }
        if (MaxSlotsHorizontal * moduleScale > Screen.width * 0.45f)
        {
            moduleScale = Screen.width * 0.45f / MaxSlotsHorizontal;
        }

        //Apply scale
        ForeSize *= moduleScale;
        AftSize *= moduleScale;
        PortSize *= moduleScale;
        StarboardSize *= moduleScale;
        CenterSize *= moduleScale;
        MaxSlotsVertical *= moduleScale;
        MaxSlotsHorizontal *= moduleScale;
        if (selectedModule != null)
        {
            SelectedModuleRect = new Rect(0, 0, selectedModule.SizeX * moduleScale, selectedModule.SizeY * moduleScale);
        }

        //Determine the center of the layout
        //float centerY = (Screen.height - MaxSlotsVertical) / 2 + ForeSize.y + (MaxSlotsVertical - ForeSize.y - AftSize.y) / 2;
        //float centerX = Screen.width * 0.25f + (Screen.width - MaxSlotsHorizontal) / 2 + PortSize.x + moduleScale;
        float centerY = (Screen.height - MaxSlotsVertical) / 2 + ForeSize.y + moduleScale / 2 + (Mathf.Max(CenterSize.y, PortSize.y, StarboardSize.y) - CenterSize.y) / 2;
        float centerX = Screen.width * 0.25f + (Screen.width * 0.5f - MaxSlotsHorizontal) / 2 + PortSize.x + moduleScale / 2;

        CenterPosition = new Vector2(centerX, centerY);
        StarboardPosition = new Vector2(CenterPosition.x + CenterSize.x + moduleScale / 2, CenterPosition.y + CenterSize.y / 2 - StarboardSize.y / 2);
        PortPosition = new Vector2(CenterPosition.x - PortSize.x - moduleScale / 2, CenterPosition.y + CenterSize.y / 2 - PortSize.y / 2);
        ForePosition = new Vector2(CenterPosition.x + CenterSize.x / 2 - ForeSize.x / 2, Mathf.Min(CenterPosition.y, StarboardPosition.y, PortPosition.y) - moduleScale / 2 - ForeSize.y);
        AftPosition = new Vector2(CenterPosition.x + CenterSize.x / 2 - AftSize.x / 2, Mathf.Max(CenterPosition.y + CenterSize.y, StarboardPosition.y + StarboardSize.y, PortPosition.y + PortSize.y) + moduleScale / 2);

        MinX = Screen.width;
        MaxX = 0;
        MinY = Screen.height;
        MaxY = 0;

        float CenterMinX = Screen.width;
        float CenterMaxX = 0;
        float CenterMinY = Screen.height;
        float CenterMaxY = 0;

        //Build new slots
        foreach (Rect slot in sourceLayout.CenterSlots)
        {
            Rect newSlot = new Rect((slot.x - CenterMin.x) / 16f * moduleScale + CenterPosition.x, (slot.y - CenterMin.y) / 16f * moduleScale + CenterPosition.y, moduleScale, moduleScale);
            if (newSlot.x < MinX)
                MinX = newSlot.x;
            if (newSlot.xMax > MaxX)
                MaxX = newSlot.xMax;
            if (newSlot.y < MinY)
                MinY = newSlot.y;
            if (newSlot.yMax > MaxY)
                MaxY = newSlot.yMax;

            if (newSlot.x < CenterMinX)
                CenterMinX = newSlot.x;
            if (newSlot.xMax > CenterMaxX)
                CenterMaxX = newSlot.xMax;
            if (newSlot.y < CenterMinY)
                CenterMinY = newSlot.y;
            if (newSlot.yMax > CenterMaxY)
                CenterMaxY = newSlot.yMax;
            CenterSlots.Add(newSlot);
        }
        CenterSlotsAreaRect = new Rect(CenterMinX, CenterMinY, CenterMaxX - CenterMinX, CenterMaxY - CenterMinY);
        float StarboardMinX = Screen.width;
        float StarboardMaxX = 0;
        float StarboardMinY = Screen.height;
        float StarboardMaxY = 0;

        foreach (Rect slot in sourceLayout.StarboardSlots)
        {
            Rect newSlot = new Rect((slot.x - StarboardMin.x) / 16f * moduleScale + StarboardPosition.x, (slot.y - StarboardMin.y) / 16f * moduleScale + StarboardPosition.y, moduleScale, moduleScale);
            if (newSlot.x < MinX)
                MinX = newSlot.x;
            if (newSlot.xMax > MaxX)
                MaxX = newSlot.xMax;
            if (newSlot.y < MinY)
                MinY = newSlot.y;
            if (newSlot.yMax > MaxY)
                MaxY = newSlot.yMax;

            if (newSlot.x < StarboardMinX)
                StarboardMinX = newSlot.x;
            if (newSlot.xMax > StarboardMaxX)
                StarboardMaxX = newSlot.xMax;
            if (newSlot.y < StarboardMinY)
                StarboardMinY = newSlot.y;
            if (newSlot.yMax > StarboardMaxY)
                StarboardMaxY = newSlot.yMax;
            StarboardSlots.Add(newSlot);
        }
        StarboardSlotsAreaRect = new Rect(StarboardMinX, StarboardMinY, StarboardMaxX - StarboardMinX, StarboardMaxY - StarboardMinY);
        float PortMinX = Screen.width;
        float PortMaxX = 0;
        float PortMinY = Screen.height;
        float PortMaxY = 0;

        foreach (Rect slot in sourceLayout.PortSlots)
        {
            Rect newSlot = new Rect((slot.x - PortMin.x) / 16f * moduleScale + PortPosition.x, (slot.y - PortMin.y) / 16f * moduleScale + PortPosition.y, moduleScale, moduleScale);
            if (newSlot.x < MinX)
                MinX = newSlot.x;
            if (newSlot.xMax > MaxX)
                MaxX = newSlot.xMax;
            if (newSlot.y < MinY)
                MinY = newSlot.y;
            if (newSlot.yMax > MaxY)
                MaxY = newSlot.yMax;

            if (newSlot.x < PortMinX)
                PortMinX = newSlot.x;
            if (newSlot.xMax > PortMaxX)
                PortMaxX = newSlot.xMax;
            if (newSlot.y < PortMinY)
                PortMinY = newSlot.y;
            if (newSlot.yMax > PortMaxY)
                PortMaxY = newSlot.yMax;
            PortSlots.Add(newSlot);
        }
        PortSlotsAreaRect = new Rect(PortMinX, PortMinY, PortMaxX - PortMinX, PortMaxY - PortMinY);
        float ForeMinX = Screen.width;
        float ForeMaxX = 0;
        float ForeMinY = Screen.height;
        float ForeMaxY = 0;

        foreach (Rect slot in sourceLayout.ForeSlots)
        {
            Rect newSlot = new Rect((slot.x - ForeMin.x) / 16f * moduleScale + ForePosition.x, (slot.y - ForeMin.y) / 16f * moduleScale + ForePosition.y, moduleScale, moduleScale);
            if (newSlot.x < MinX)
                MinX = newSlot.x;
            if (newSlot.xMax > MaxX)
                MaxX = newSlot.xMax;
            if (newSlot.y < MinY)
                MinY = newSlot.y;
            if (newSlot.yMax > MaxY)
                MaxY = newSlot.yMax;

            if (newSlot.x < ForeMinX)
                ForeMinX = newSlot.x;
            if (newSlot.xMax > ForeMaxX)
                ForeMaxX = newSlot.xMax;
            if (newSlot.y < ForeMinY)
                ForeMinY = newSlot.y;
            if (newSlot.yMax > ForeMaxY)
                ForeMaxY = newSlot.yMax;
            ForeSlots.Add(newSlot);
        }
        ForeSlotsAreaRect = new Rect(ForeMinX, ForeMinY, ForeMaxX - ForeMinX, ForeMaxY - ForeMinY);
        float AftMinX = Screen.width;
        float AftMaxX = 0;
        float AftMinY = Screen.height;
        float AftMaxY = 0;

        foreach (Rect slot in sourceLayout.AftSlots)
        {
            Rect newSlot = new Rect((slot.x - AftMin.x) / 16f * moduleScale + AftPosition.x, (slot.y - AftMin.y) / 16f * moduleScale + AftPosition.y, moduleScale, moduleScale);
            if (newSlot.x < MinX)
                MinX = newSlot.x;
            if (newSlot.xMax > MaxX)
                MaxX = newSlot.xMax;
            if (newSlot.y < MinY)
                MinY = newSlot.y;
            if (newSlot.yMax > MaxY)
                MaxY = newSlot.yMax;

            if (newSlot.x < AftMinX)
                AftMinX = newSlot.x;
            if (newSlot.xMax > AftMaxX)
                AftMaxX = newSlot.xMax;
            if (newSlot.y < AftMinY)
                AftMinY = newSlot.y;
            if (newSlot.yMax > AftMaxY)
                AftMaxY = newSlot.yMax;
            AftSlots.Add(newSlot);
        }
        AftSlotsAreaRect = new Rect(AftMinX, AftMinY, AftMaxX - AftMinX, AftMaxY - AftMinY);
        SlotsAreaRect = new Rect(MinX, MinY, MaxX - MinX, MaxY - MinY);
    }

    protected void SetShipModel(ShipHullData hullData)
    {
        Vector3 Position = Camera.main.transform.position + Camera.main.transform.forward * 5f;

        if (unitModel != null)
        {
            Object.Destroy(unitModel);
        }
        unitModel = ResourceManager.instance.CreateShip(hullData, Position, Quaternion.identity);
    }

    protected override void ClearFormatedSlots()
    {
        ForeSlots.Clear();
        AftSlots.Clear();
        PortSlots.Clear();
        StarboardSlots.Clear();
        CenterSlots.Clear();
    }

    protected override void ClearSlottedModules()
    {
        ForeSlotedModules.Clear();
        AftSlotedModules.Clear();
        PortSlotedModules.Clear();
        StarboardSlotedModules.Clear();
        CenterSlotedModules.Clear();
    }

    protected void CheckModulePlacement(Vector2 checkPosition)
    {
        if(ForeSlotsAreaRect.Contains(checkPosition))
        {
            CheckModulePlacementForQuadrant(checkPosition, ForeSlots, ForeSlotedModules, QuadrantTypes.Fore);
        }
        else if (AftSlotsAreaRect.Contains(checkPosition))
        {
            CheckModulePlacementForQuadrant(checkPosition, AftSlots, AftSlotedModules, QuadrantTypes.Aft);
        }
        else if (PortSlotsAreaRect.Contains(checkPosition))
        {
            CheckModulePlacementForQuadrant(checkPosition, PortSlots, PortSlotedModules, QuadrantTypes.Port);
        }
        else if (StarboardSlotsAreaRect.Contains(checkPosition))
        {
            CheckModulePlacementForQuadrant(checkPosition, StarboardSlots, StarboardSlotedModules, QuadrantTypes.Starboard);
        }
        else if (CenterSlotsAreaRect.Contains(checkPosition))
        {
            CheckModulePlacementForQuadrant(checkPosition, CenterSlots, CenterSlotedModules, QuadrantTypes.Center);
        }
    }

    protected bool CheckModulePlacementForQuadrant(Vector2 checkPosition, List<Rect> Slots, List<SlotedModule> slotedModules, QuadrantTypes quadrant)
    {
        bool Valid = false;
        bool SlotMissing = false;
        int SizeX;
        int SizeY;
        List<Rect> HoveredRects = new List<Rect>();

        for (int index = 0; index < Slots.Count; index++)
        {
            if (Slots[index].Contains(checkPosition))
            {
                //Check if the quadrant allows the module return true so it will stop checking for placement
                switch (quadrant)
                {
                    case QuadrantTypes.Fore:
                        {
                            if (!CheckHullModuleAllow(selectedHullData.ModuleLimitFore, selectedModuleSet.ModuleCategory))
                            {
                                return true;
                            }
                            break;
                        }
                    case QuadrantTypes.Aft:
                        {
                            if (!CheckHullModuleAllow(selectedHullData.ModuleLimitAft, selectedModuleSet.ModuleCategory))
                            {
                                return true;
                            }
                            break;
                        }
                    case QuadrantTypes.Port:
                        {
                            if (!CheckHullModuleAllow(selectedHullData.ModuleLimitPort, selectedModuleSet.ModuleCategory))
                            {
                                return true;
                            }
                            break;
                        }
                    case QuadrantTypes.Starboard:
                        {
                            if (!CheckHullModuleAllow(selectedHullData.ModuleLimitStarboard, selectedModuleSet.ModuleCategory))
                            {
                                return true;
                            }
                            break;
                        }
                    case QuadrantTypes.Center:
                        {
                            if (!CheckHullModuleAllow(selectedHullData.ModuleLimitCenter, selectedModuleSet.ModuleCategory))
                            {
                                return true;
                            }
                            break;
                        }
                }
                HoveredRects.Add(Slots[index]);
                //Check if module already in slot
                foreach (SlotedModule slotmod in slotedModules)
                {
                    if (slotmod.module == selectedModule && slotmod.baseRect.x == Slots[index].x && slotmod.baseRect.y == Slots[index].y)
                    {
                        return false;
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
                    return false;
                }

                //Check for overlapping               
                ModulesToRemove.Clear();
                foreach (SlotedModule slotmod in slotedModules)
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
                            slotedModules.Remove(slotmod);
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
                    slotedModules.Add(module);
                    AddDesignModuleStats(module, quadrant);
                }
                return true;
            }
        }
        return false;
    }

    protected override void CheckModuleRemoval()
    {
        //Selected Module       
        ModulesToRemove.Clear();
        //Fore modules
        if (ForeSlotsAreaRect.Contains(mousePosition))
        {
            foreach (SlotedModule slotmod in ForeSlotedModules)
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
                    ForeSlotedModules.Remove(slotmod);
                }
                RecalculateDesignStats();
            }
        }
        else if (AftSlotsAreaRect.Contains(mousePosition))
        {
            foreach (SlotedModule slotmod in AftSlotedModules)
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
                    AftSlotedModules.Remove(slotmod);
                }
                RecalculateDesignStats();
            }
        }
        else if (PortSlotsAreaRect.Contains(mousePosition))
        {
            foreach (SlotedModule slotmod in PortSlotedModules)
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
                    PortSlotedModules.Remove(slotmod);
                }
                RecalculateDesignStats();
            }
        }
        else if (StarboardSlotsAreaRect.Contains(mousePosition))
        {
            foreach (SlotedModule slotmod in StarboardSlotedModules)
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
                    StarboardSlotedModules.Remove(slotmod);
                }
                RecalculateDesignStats();
            }
        }
        else if (CenterSlotsAreaRect.Contains(mousePosition))
        {
            foreach (SlotedModule slotmod in CenterSlotedModules)
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
                    CenterSlotedModules.Remove(slotmod);
                }
                RecalculateDesignStats();
            }
        }
        ModulesToRemove.Clear();
    }

    protected override void CheckPlacedModuleSelected()
    {
        if (ForeSlotsAreaRect.Contains(mousePosition))
        {
            foreach (SlotedModule slotmod in ForeSlotedModules)
            {
                if (slotmod.baseRect.Contains(mousePosition))
                {
                    ChangeModule(slotmod.module);
                    ScrollToModuleSet(selectedModuleSet);
                    return;
                }
            }
        }
        else if (AftSlotsAreaRect.Contains(mousePosition))
        {
            foreach (SlotedModule slotmod in AftSlotedModules)
            {
                if (slotmod.baseRect.Contains(mousePosition))
                {
                    ChangeModule(slotmod.module);
                    ScrollToModuleSet(selectedModuleSet);
                    return;
                }
            }
        }
        else if (PortSlotsAreaRect.Contains(mousePosition))
        {
            foreach (SlotedModule slotmod in PortSlotedModules)
            {
                if (slotmod.baseRect.Contains(mousePosition))
                {
                    ChangeModule(slotmod.module);
                    ScrollToModuleSet(selectedModuleSet);
                    return;
                }
            }
        }
        else if (StarboardSlotsAreaRect.Contains(mousePosition))
        {
            foreach (SlotedModule slotmod in StarboardSlotedModules)
            {
                if (slotmod.baseRect.Contains(mousePosition))
                {
                    ChangeModule(slotmod.module);
                    ScrollToModuleSet(selectedModuleSet);
                    return;
                }
            }
        }
        else if (CenterSlotsAreaRect.Contains(mousePosition))
        {
            foreach (SlotedModule slotmod in CenterSlotedModules)
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
        DesignMass = 0;
        DesignPower = 0;
        DesignPowerGenerated = 0;
        DesignFuel = selectedHullData.BaseFuel;
        DesignAmmo = 0;
        DesignPowerStorage = 0;
        DesignCrew = selectedHullData.BaseCrew;
        DesignRequiredCrew = 0;
        DesignSupplies = selectedHullData.BaseSupply;
        DesignEngineFTL = 0;
        DesignEngineThrust = 0;
        DesignEngineTurn = 0;
        DesignSensor = selectedHullData.BaseSensor;
        DesignLongRangeSensor = selectedHullData.BaseLongRangeSensor;
        DesignAdvancedSensor = selectedHullData.BaseAdvancedSensor;
        DesignResearch = selectedHullData.BaseResearch;
        DesignMining = 0;
        DesignRepair = selectedHullData.BaseRepair;
        DesignExperience = selectedHullData.BaseCommandPoints;
        DesignEngineBonus = 0;
        DesignDamageBonus = 0;
        DesignDefenseBonus = 0;
        DesignDamageBonusFleet = 0;
        DesignDefenseBonusFleet = 0;
        DesignPowerEfficiency = 0;
        DesignAmmoGenerated = 0;
        DesignTroops = 0;
        DesignCommandPointReduction = 0;
        DesignFighters = 0;
        DesignHeavyFighters = 0;
        DesignAssaultPods = 0;
        DesignFirePowerFighter = 0;
        DesignMedical = 0;
        DesignCloakingPowerPerMass = 0;
        DesignStealth = 0;
        DesignCommandPointBonusFleet = 0;
        DesignBoardingDefense = 0;
        DesignTransporter = 0;
        DesignColony = 0;
        DesignDiplomacy = 0;
        DesignFirePowerAmmo = 0;
        DesignFirePowerPower = 0;
        DesignFirePowerCombination = 0;
        DesignConstruction = 0;
        DesignDefenseRating = 0;
        DesignMaxRange = 0;
        DesignExperienceBonus = 0;
        DesignBombArmyDamage = 0;
        DesignBombStructureDamage = 0;
        DesignBombPopulationDamage = 0;
        DesignBombPollution = 0;

        DesignHealthFore = selectedHullData.BaseHealthPerSlot * ForeSlots.Count;
        DesignArmorHealthFore = selectedHullData.BaseArmorPerSlot * ForeSlots.Count;
        DesignArmorRatingFore = 0;
        DesignShieldHealthFore = 0;
        DesignShieldRatingFore = 0;
        DesignShieldRechargeFore = 0;
        DesignShieldDelayFore = 0;

        DesignHealthAft = selectedHullData.BaseHealthPerSlot * AftSlots.Count;
        DesignArmorHealthAft = selectedHullData.BaseArmorPerSlot * AftSlots.Count;
        DesignArmorRatingAft = 0;
        DesignShieldHealthAft = 0;
        DesignShieldRatingAft = 0;
        DesignShieldRechargeAft = 0;
        DesignShieldDelayAft = 0;

        DesignHealthPort = selectedHullData.BaseHealthPerSlot * PortSlots.Count;
        DesignArmorHealthPort = selectedHullData.BaseArmorPerSlot * PortSlots.Count;
        DesignArmorRatingPort = 0;
        DesignShieldHealthPort = 0;
        DesignShieldRatingPort = 0;
        DesignShieldRechargePort = 0;
        DesignShieldDelayPort = 0;

        DesignHealthStarboard = selectedHullData.BaseHealthPerSlot * StarboardSlots.Count;
        DesignArmorHealthStarboard = selectedHullData.BaseArmorPerSlot * StarboardSlots.Count;
        DesignArmorRatingStarboard = 0;
        DesignShieldHealthStarboard = 0;
        DesignShieldRatingStarboard = 0;
        DesignShieldRechargeStarboard = 0;
        DesignShieldDelayStarboard = 0;

        DesignHealthCenter = selectedHullData.BaseHealthPerSlot * CenterSlots.Count;
        DesignArmorHealthCenter = selectedHullData.BaseArmorPerSlot * CenterSlots.Count;
        DesignArmorRatingCenter = 0;
        DesignShieldHealthCenter = 0;
        DesignShieldRatingCenter = 0;
        DesignShieldRechargeCenter = 0;
        DesignShieldDelayCenter = 0;

        DesignJammingCount = 0;
        DesignJammingRange = 0;
        DesignJammingDelay = 0;

        DesignCommandPoints = selectedHullData.BaseCommandPoints;
        DesignFTLSpeed = selectedHullData.BaseFTL;
        DesignSpeed = 0;
        DesignTurnSpeed = 0;
        DesignPowerSum = 0;
        DesignLongRangeSensorSum = DesignSensor + DesignLongRangeSensor;
        DesignWeaponAmmoConsumption = 0;
        DesignWeaponPowerConsumption = 0;
        DesignWeaponAmmoTime = 0;
        DesignWeaponPowerTime = 0;
        DesignCloakingTime = 0;
        DesignCloakingPower = 0;
        DesignEffectiveStealth = 0;
        DesignFirepower = 0;
    }

    protected override void RecalculateDesignStats()
    {
        ClearDesignModuleStats();

        foreach (SlotedModule module in ForeSlotedModules)
        {
            AddDesignModuleStats(module, QuadrantTypes.Fore);
        }
        foreach (SlotedModule module in AftSlotedModules)
        {
            AddDesignModuleStats(module, QuadrantTypes.Aft);
        }
        foreach (SlotedModule module in PortSlotedModules)
        {
            AddDesignModuleStats(module, QuadrantTypes.Port);
        }
        foreach (SlotedModule module in StarboardSlotedModules)
        {
            AddDesignModuleStats(module, QuadrantTypes.Starboard);
        }
        foreach (SlotedModule module in CenterSlotedModules)
        {
            AddDesignModuleStats(module, QuadrantTypes.Center);
        }
    }

    protected void AddDesignModuleStats(SlotedModule slottedModule, QuadrantTypes quadrant)
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
        DesignMass += module.Mass;
        DesignPower += module.Power;
        DesignPowerGenerated += module.PowerGenerated;
        DesignFuel += module.Fuel;
        DesignAmmo += module.Ammo;
        DesignPowerStorage += module.PowerStorage;
        DesignCrew += module.Crew;
        DesignRequiredCrew += module.RequiredCrew;
        DesignSupplies += module.Supplies;
        DesignEngineFTL += module.EngineFTL;
        DesignEngineThrust += module.EngineThrust;
        DesignEngineTurn += module.EngineTurn;
        DesignSensor += module.Sensor;
        DesignLongRangeSensor += module.LongRangeSensor;
        DesignAdvancedSensor += module.AdvancedSensor;
        DesignResearch += module.Research;
        DesignMining += module.Mining;
        DesignRepair += module.Repair;
        DesignEngineBonus = Mathf.Max(DesignEngineBonus, module.EngineBonus);
        DesignDamageBonus = Mathf.Max(DesignDamageBonus, module.DamageBonus);
        DesignDefenseBonus = Mathf.Max(DesignDefenseBonus, module.DefenseBonus);
        DesignDamageBonusFleet = Mathf.Max(DesignDamageBonusFleet, module.DamageBonusFleet);
        DesignDefenseBonusFleet = Mathf.Max(DesignDefenseBonusFleet, module.DefenseBonusFleet);
        DesignPowerEfficiency = Mathf.Max(DesignPowerEfficiency, module.PowerEfficiency);
        DesignAmmoGenerated += module.AmmoGenerated;
        DesignTroops += module.Troops;
        DesignCommandPointReduction = Mathf.Max(module.CommandPointReduction, DesignCommandPointReduction);
        DesignCommandPoints = selectedHullData.BaseCommandPoints - DesignCommandPointReduction;
        if (DesignCommandPoints < 1)
        {
            DesignCommandPoints = 1;
        }
        DesignFighters += module.GetFighterCount();
        DesignHeavyFighters += module.GetHeavyFighterCount();
        DesignAssaultPods += module.GetAssaultPodCount();
        DesignFirePowerFighter += module.GetFighterFirePower();

        DesignMedical += module.Medical;

        if (module.CloakingPowerPerMass > 0)
        {
            if (DesignCloakingPowerPerMass == 0)
            {
                DesignCloakingPowerPerMass = module.CloakingPowerPerMass;
            }
            else
            {
                DesignCloakingPowerPerMass = Mathf.Min(module.CloakingPowerPerMass, DesignCloakingPowerPerMass);
            }
        }
        if (DesignCloakingPowerPerMass > 0)
        {
            DesignCloakingPower = DesignCloakingPowerPerMass * DesignMass;
            DesignCloakingTime = DesignPowerStorage / DesignCloakingPower;
        }

        DesignStealth += module.Stealth;
        DesignCommandPointBonusFleet = Mathf.Max(DesignCommandPointBonusFleet, module.CommandPointBonusFleet);
        DesignBoardingDefense += module.BoardingDefense;
        DesignTransporter += module.Transporter;
        DesignColony += module.Colonies;
        DesignDiplomacy += module.Diplomacy;
        DesignConstruction += module.Construction;
        DesignExperienceBonus = Mathf.Max(DesignExperienceBonus, module.ExperienceBonus);
        DesignBombArmyDamage += module.BombArmyDamage;
        DesignBombStructureDamage += module.BombStructureDamage;
        DesignBombPopulationDamage += module.BombPopulationDamage;
        DesignBombPollution += module.BombPollution;
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
        if (DesignMass > 0)
        {
            DesignFTLSpeed = selectedHullData.BaseFTL + DesignEngineFTL / DesignMass;
            DesignSpeed = DesignEngineThrust / DesignMass;
            DesignTurnSpeed = DesignEngineTurn / DesignMass;
        }
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

        if (DesignStealth > 0)
        {
            DesignEffectiveStealth = DesignStealth / DesignMass;
        }

        switch (quadrant)
        {
            case QuadrantTypes.Fore:
                {
                    DesignHealthFore += module.Health;
                    DesignArmorHealthFore += module.ArmorHealth;
                    DesignArmorRatingFore += module.ArmorRating;
                    DesignShieldHealthFore += module.ShieldHealth;
                    DesignShieldRatingFore += module.ShieldRating;
                    DesignShieldRechargeFore += module.ShieldRechargeRate;
                    DesignShieldDelayFore = Mathf.Max(module.ShieldRechargeDelay, DesignShieldDelayFore);
                    break;
                }
            case QuadrantTypes.Aft:
                {
                    DesignHealthAft += module.Health;
                    DesignArmorHealthAft += module.ArmorHealth;
                    DesignArmorRatingAft += module.ArmorRating;
                    DesignShieldHealthAft += module.ShieldHealth;
                    DesignShieldRatingAft += module.ShieldRating;
                    DesignShieldRechargeAft += module.ShieldRechargeRate;
                    DesignShieldDelayAft = Mathf.Max(module.ShieldRechargeDelay, DesignShieldDelayAft);
                    break;
                }
            case QuadrantTypes.Port:
                {
                    DesignHealthPort += module.Health;
                    DesignArmorHealthPort += module.ArmorHealth;
                    DesignArmorRatingPort += module.ArmorRating;
                    DesignShieldHealthPort += module.ShieldHealth;
                    DesignShieldRatingPort += module.ShieldRating;
                    DesignShieldRechargePort += module.ShieldRechargeRate;
                    DesignShieldDelayPort = Mathf.Max(module.ShieldRechargeDelay, DesignShieldDelayPort);
                    break;
                }
            case QuadrantTypes.Starboard:
                {
                    DesignHealthStarboard += module.Health;
                    DesignArmorHealthStarboard += module.ArmorHealth;
                    DesignArmorRatingStarboard += module.ArmorRating;
                    DesignShieldHealthStarboard += module.ShieldHealth;
                    DesignShieldRatingStarboard += module.ShieldRating;
                    DesignShieldRechargeStarboard += module.ShieldRechargeRate;
                    DesignShieldDelayStarboard = Mathf.Max(module.ShieldRechargeDelay, DesignShieldDelayStarboard);
                    break;
                }
            case QuadrantTypes.Center:
                {
                    DesignHealthCenter += module.Health;
                    DesignArmorHealthCenter += module.ArmorHealth;
                    DesignArmorRatingCenter += module.ArmorRating;
                    DesignShieldHealthCenter += module.ShieldHealth;
                    DesignShieldRatingCenter += module.ShieldRating;
                    DesignShieldRechargeCenter += module.ShieldRechargeRate;
                    DesignShieldDelayCenter = Mathf.Max(module.ShieldRechargeDelay, DesignShieldDelayCenter);
                    break;
                }
        }
    }

    protected override bool CheckValidDesign()
    {
        if (DesignName == "" || DesignSpeed <= 0 || DesignTurnSpeed <= 0 || DesignPowerSum <= 0 || DesignHealthAft <= 0 ||
            DesignHealthCenter <= 0 || DesignHealthFore <= 0 || DesignHealthPort <= 0 || DesignHealthStarboard <= 0
            || DesignPowerSum <= 0 || (DesignAmmo == 0 && DesignWeaponAmmoConsumption > 0))
        {
            return false;
        }

        return true;
    }

    protected override void SaveDesign()
    {
        ShipDesign design = new ShipDesign();

        design.Name = DesignName;
        design.Hull = selectedHullData.Name;

        design.AI_Profile = designBehaviorType;
        design.attackDirection = designBehaviorAttackDirection;
        design.attackStyle = designBehaviorAttackStyle;
        if (designBehaviorAttackRange != -1)
        {
            design.attackRange = designBehaviorAttackRange;
        }
        else
        {
            design.attackRange = DesignMaxRange;
        }

        foreach (SlotedModule slotmod in ForeSlotedModules)
        {
            DesignModule module = new DesignModule(ResourceManager.instance.GetModuleName(slotmod.module), slotmod.Rotation, slotmod.Position);
            design.ForeModules.Add(module);
        }
        foreach (SlotedModule slotmod in AftSlotedModules)
        {
            DesignModule module = new DesignModule(ResourceManager.instance.GetModuleName(slotmod.module), slotmod.Rotation, slotmod.Position);
            design.AftModules.Add(module);
        }
        foreach (SlotedModule slotmod in PortSlotedModules)
        {
            DesignModule module = new DesignModule(ResourceManager.instance.GetModuleName(slotmod.module), slotmod.Rotation, slotmod.Position);
            design.PortModules.Add(module);
        }
        foreach (SlotedModule slotmod in StarboardSlotedModules)
        {
            DesignModule module = new DesignModule(ResourceManager.instance.GetModuleName(slotmod.module), slotmod.Rotation, slotmod.Position);
            design.StarboardModules.Add(module);
        }
        foreach (SlotedModule slotmod in CenterSlotedModules)
        {
            DesignModule module = new DesignModule(ResourceManager.instance.GetModuleName(slotmod.module), slotmod.Rotation, slotmod.Position);
            design.CenterModules.Add(module);
        }


        ResourceManager.instance.SaveShipDesign(design);

        BuildDesignList(selectedHullData);
        selectedDesign = design;
        DesignName = selectedDesign.Name;
    }

    protected void DeleteDesign(ShipDesign design)
    {
        DesignToDelete = null;
        if (selectedDesign == design)
        {
            selectedDesign = null;
        }
        design.Deleted = true;
        ResourceManager.instance.SaveShipDesign(design);
        BuildDesignList(selectedHullData);
    }

    protected void SetDesignToDelete(ShipDesign design)
    {
        DesignToDelete = design;
    }

    protected override void ModuleSwap(ModuleSet newModuleSet)
    {
        SwapModulesInList(newModuleSet, ForeSlotedModules);
        SwapModulesInList(newModuleSet, AftSlotedModules);
        SwapModulesInList(newModuleSet, PortSlotedModules);
        SwapModulesInList(newModuleSet, StarboardSlotedModules);
        SwapModulesInList(newModuleSet, CenterSlotedModules);

        RecalculateDesignStats();
    }

    protected void ResetBehavior()
    {
        designBehaviorType = ShipBehaviorType.BestGuess;
        designBehaviorAttackStyle = AttackStyle.holdPosition;
        designBehaviorAttackDirection = QuadrantTypes.Fore;
        designBehaviorAttackRange = -1;
    }

    protected void BuildDamageBarGraph()
    {
        List<Weapon> allWeapons = new List<Weapon>();

        foreach (SlotedModule module in ForeSlotedModules)
        {
            Weapon weapon = module.module.GetWeapon();
            if (weapon != null)
            {
                allWeapons.Add(weapon);
            }
        }
        foreach (SlotedModule module in AftSlotedModules)
        {
            Weapon weapon = module.module.GetWeapon();
            if (weapon != null)
            {
                allWeapons.Add(weapon);
            }
        }
        foreach (SlotedModule module in PortSlotedModules)
        {
            Weapon weapon = module.module.GetWeapon();
            if (weapon != null)
            {
                allWeapons.Add(weapon);
            }
        }
        foreach (SlotedModule module in StarboardSlotedModules)
        {
            Weapon weapon = module.module.GetWeapon();
            if (weapon != null)
            {
                allWeapons.Add(weapon);
            }
        }
        foreach (SlotedModule module in CenterSlotedModules)
        {
            Weapon weapon = module.module.GetWeapon();
            if (weapon != null)
            {
                allWeapons.Add(weapon);
            }
        }

        damageBarGraph.SetDamageBars(designBehaviorAttackRange ,allWeapons);
    }

    protected override bool PopupOpen()
    {
        return overwriteWarningPopupOpen || BehaviorWindowOpen;
    }

    protected void BuildDesignList(ShipHullData data)
    {

    }

    protected override bool CheckDesignAllowed(ShipDesign design)
    {
        return true;
    }

    protected override bool CheckHullAllowed(ShipHullData hull)
    {
        return true;
    }

    protected override bool CheckModuleSetAllowed(ModuleSet moduleSet)
    {
        if (moduleSet.SetRestriction == ModuleSetRestriction.StationsOnly)
            return false;
        return true;
    }

    protected override float GetModifiedDamage(Weapon weapon)
    {
        float moduleBonus = Mathf.Max(DesignDamageBonus, DesignDamageBonusFleet);

        return moduleBonus;
    }

    protected void DrawShipClassificationTooltip(ShipType classification)
    {
        switch (classification)
        {
            case ShipType.Corvette:
                {
                    ToolTip.SetText("corvetteClass", "corvetteClassDesc");
                    break;
                }
            case ShipType.Destroyer:
                {
                    ToolTip.SetText("destroyerClass", "destroyerClassDesc");
                    break;
                }
            case ShipType.Frigate:
                {
                    ToolTip.SetText("frigateClass", "frigateClassDesc");
                    break;
                }
            case ShipType.Cruiser:
                {
                    ToolTip.SetText("cruiserClass", "cruiserClassDesc");
                    break;
                }
            case ShipType.Battlecruiser:
                {
                    ToolTip.SetText("battlecruiserClass", "battlecruiserClassDesc");
                    break;
                }
            case ShipType.Battleship:
                {
                    ToolTip.SetText("battleshipClass", "battleshipClassDesc");
                    break;
                }
            case ShipType.Dreadnought:
                {
                    ToolTip.SetText("dreadnoughtClass", "dreadnoughtClassDesc");
                    break;
                }
            case ShipType.Freighter:
                {
                    ToolTip.SetText("freighterClass", "freighterClassDesc");
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    public string GetFormattedDesignMass()
    {
        return GetStandardDesignStatFormat(DesignMass);
    }

    public string GetFormattedDesignFuel()
    {
        return GetStandardDesignStatFormat(DesignFuel);
    }

    public string GetFormattedDesignSupplies()
    {
        return GetStandardDesignStatFormat(DesignSupplies);
    }

    public string GetFormattedDesignCommandPoints()
    {
        return GetStandardDesignStatFormat(DesignCommandPoints);
    }

    public string GetFormattedDesignCommandPointBonusFleet()
    {
        return GetStandardDesignStatFormat(DesignCommandPointBonusFleet);
    }

    public string GetFormattedDesignColony()
    {
        return GetStandardDesignStatFormat(DesignColony);
    }

    public string GetFormattedDesignConstruction()
    {
        return GetStandardDesignStatFormat(DesignConstruction);
    }

    public string GetFormattedDesignExperienceBonus()
    {
        return GetPercentageDesignStatFormat(DesignExperienceBonus);
    }

    public string GetFormattedDesignSpeed()
    {
        return GetOnlyPositiveDesignStatFormat(DesignSpeed);
    }

    public string GetFormattedDesignTurnSpeed()
    {
        return GetOnlyPositiveDesignStatFormat(DesignTurnSpeed);
    }

    public string GetFormattedDesignFTLSpeed()
    {
        return GetOnlyPositiveDesignStatFormat(DesignFTLSpeed);
    }

    public string GetFormattedDesignHealthFore()
    {
        return GetStandardDesignStatFormat(DesignHealthFore);
    }

    public string GetFormattedDesignArmorHealthFore()
    {
        return GetStandardDesignStatFormat(DesignArmorHealthFore);
    }

    public string GetFormattedDesignArmorRatingFore()
    {
        return GetStandardDesignStatFormat(DesignArmorRatingFore);
    }

    public string GetFormattedDesignShieldHealthFore()
    {
        return GetStandardDesignStatFormat(DesignShieldHealthFore);
    }

    public string GetFormattedDesignShieldRatingFore()
    {
        return GetStandardDesignStatFormat(DesignShieldRatingFore);
    }

    public string GetFormattedDesignShieldRechargeFore()
    {
        return GetStandardDesignStatFormat(DesignShieldRechargeFore);
    }

    public string GetFormattedDesignShieldDelayFore()
    {
        return GetStandardDesignStatFormat(DesignShieldDelayFore);
    }

    public string GetFormattedDesignHealthAft()
    {
        return GetStandardDesignStatFormat(DesignHealthAft);
    }

    public string GetFormattedDesignArmorHealthAft()
    {
        return GetStandardDesignStatFormat(DesignArmorHealthAft);
    }

    public string GetFormattedDesignArmorRatingAft()
    {
        return GetStandardDesignStatFormat(DesignArmorRatingAft);
    }

    public string GetFormattedDesignShieldHealthAft()
    {
        return GetStandardDesignStatFormat(DesignShieldHealthAft);
    }

    public string GetFormattedDesignShieldRatingAft()
    {
        return GetStandardDesignStatFormat(DesignShieldRatingAft);
    }

    public string GetFormattedDesignShieldRechargeAft()
    {
        return GetStandardDesignStatFormat(DesignShieldRechargeAft);
    }

    public string GetFormattedDesignShieldDelayAft()
    {
        return GetStandardDesignStatFormat(DesignShieldDelayAft);
    }

    public string GetFormattedDesignHealthPort()
    {
        return GetStandardDesignStatFormat(DesignHealthPort);
    }

    public string GetFormattedDesignArmorHealthPort()
    {
        return GetStandardDesignStatFormat(DesignArmorHealthPort);
    }

    public string GetFormattedDesignArmorRatingPort()
    {
        return GetStandardDesignStatFormat(DesignArmorRatingPort);
    }

    public string GetFormattedDesignShieldHealthPort()
    {
        return GetStandardDesignStatFormat(DesignShieldHealthPort);
    }

    public string GetFormattedDesignShieldRatingPort()
    {
        return GetStandardDesignStatFormat(DesignShieldRatingPort);
    }

    public string GetFormattedDesignShieldRechargePort()
    {
        return GetStandardDesignStatFormat(DesignShieldRechargePort);
    }

    public string GetFormattedDesignShieldDelayPort()
    {
        return GetStandardDesignStatFormat(DesignShieldDelayPort);
    }

    public string GetFormattedDesignHealthStarboard()
    {
        return GetStandardDesignStatFormat(DesignHealthStarboard);
    }

    public string GetFormattedDesignArmorHealthStarboard()
    {
        return GetStandardDesignStatFormat(DesignArmorHealthStarboard);
    }

    public string GetFormattedDesignArmorRatingStarboard()
    {
        return GetStandardDesignStatFormat(DesignArmorRatingStarboard);
    }

    public string GetFormattedDesignShieldHealthStarboard()
    {
        return GetStandardDesignStatFormat(DesignShieldHealthStarboard);
    }

    public string GetFormattedDesignShieldRatingStarboard()
    {
        return GetStandardDesignStatFormat(DesignShieldRatingStarboard);
    }

    public string GetFormattedDesignShieldRechargeStarboard()
    {
        return GetStandardDesignStatFormat(DesignShieldRechargeStarboard);
    }

    public string GetFormattedDesignShieldDelayStarboard()
    {
        return GetStandardDesignStatFormat(DesignShieldDelayStarboard);
    }

    public string GetFormattedDesignHealthCenter()
    {
        return GetStandardDesignStatFormat(DesignHealthCenter);
    }

    public string GetFormattedDesignArmorHealthCenter()
    {
        return GetStandardDesignStatFormat(DesignArmorHealthCenter);
    }

    public string GetFormattedDesignArmorRatingCenter()
    {
        return GetStandardDesignStatFormat(DesignArmorRatingCenter);
    }

    public string GetFormattedDesignShieldHealthCenter()
    {
        return GetStandardDesignStatFormat(DesignShieldHealthCenter);
    }

    public string GetFormattedDesignShieldRatingCenter()
    {
        return GetStandardDesignStatFormat(DesignShieldRatingCenter);
    }

    public string GetFormattedDesignShieldRechargeCenter()
    {
        return GetStandardDesignStatFormat(DesignShieldRechargeCenter);
    }

    public string GetFormattedDesignShieldDelayCenter()
    {
        return GetStandardDesignStatFormat(DesignShieldDelayCenter);
    }

    public string GetFormattedDesignDamageBonusFleet()
    {
        return GetPercentageDesignStatFormat(DesignDamageBonusFleet);
    }

    public string GetFormattedDesignDefenseBonusFleet()
    {
        return GetPercentageDesignStatFormat(DesignDefenseBonusFleet);
    }

    public string GetFormattedDesignBombArmyDamage()
    {
        return GetStandardDesignStatFormat(DesignBombArmyDamage);
    }

    public string GetFormattedDesignBombStructureDamage()
    {
        return GetStandardDesignStatFormat(DesignBombStructureDamage);
    }

    public string GetFormattedDesignBombPopulationDamage()
    {
        return GetStandardDesignStatFormat(DesignBombPopulationDamage);
    }

    public string GetFormattedDesignBombPollution()
    {
        return GetStandardDesignStatFormat(DesignBombPollution);
    }

    public string GetFormattedDesignEffectiveStealth()
    {
        return GetStandardDesignStatFormat(DesignEffectiveStealth);
    }

    public string GetFormattedDesignCloakingPower()
    {
        return GetStandardDesignStatFormat(DesignCloakingPower);
    }

    public string GetFormattedDesignCloakingTime()
    {
        return GetStandardDesignStatFormat(DesignCloakingTime);
    }

    public string GetFormattedDesignEngineBonus()
    {
        return GetPercentageDesignStatFormat(DesignEngineBonus);
    }
}
