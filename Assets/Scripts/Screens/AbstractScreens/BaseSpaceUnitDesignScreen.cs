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

public abstract class BaseSpaceUnitDesignScreen : BaseScreen
{
    #region Variables

    [SerializeField]
    protected ModulePanel ModulePanel;

    protected Vector2 mirrorMousePosition;

    protected Rect SelectedModuleRect;
    protected Rect SlotsAreaRect;

    protected GameObject unitModel;
    protected Texture2D selectedModuleTexture;
    protected Texture2D SlotTexture = ResourceManager.GetUITexture("ShipSlot");
    protected Texture2D WeaponArcCircle = ResourceManager.GetUITexture("WeaponArcCircle");
    protected Texture2D WeaponArcTex;

    protected string DesignName = "";



    //Design stats
    protected float DesignProductionCost;
    protected float DesignAlloyCost;
    protected float DesignAdvancedAlloyCost;
    protected float DesignSuperiorAlloyCost;
    protected float DesignCrystalCost;
    protected float DesignRareCrystalCost;
    protected float DesignExoticCrystalCost;
    protected float DesignExoticParticleCost;
    protected float DesignPower;
    protected float DesignPowerGenerated;
    protected float DesignAmmo;
    protected float DesignPowerStorage;
    protected float DesignCrew;
    protected float DesignRequiredCrew;
    protected float DesignSensor;
    protected float DesignLongRangeSensor;
    protected float DesignAdvancedSensor;
    protected float DesignResearch;
    protected float DesignMining;
    protected float DesignRepair;
    protected float DesignDamageBonus;
    protected float DesignDefenseBonus;
    protected float DesignPowerEfficiency;
    protected float DesignAmmoGenerated;
    protected int DesignTroops;
    protected int DesignFighters;
    protected int DesignHeavyFighters;
    protected int DesignAssaultPods;
    protected float DesignMedical;
    protected float DesignBoardingDefense;
    protected float DesignTransporter;
    protected float DesignFirePowerAmmo;
    protected float DesignFirePowerPower;
    protected float DesignFirePowerCombination;
    protected float DesignFirePowerFighter;
    protected float DesignMaxRange;
    protected float DesignCost;
    protected float DesignExperience;
    protected float DesignDiplomacy;

    //Jamming Stats
    protected float DesignJammingCount;
    protected float DesignJammingRange;
    protected float DesignJammingDelay;

    //Calculated design stats
    protected float DesignPowerSum;
    protected float DesignLongRangeSensorSum;
    protected float DesignWeaponAmmoConsumption;
    protected float DesignWeaponPowerConsumption;
    protected float DesignWeaponAmmoTime;
    protected float DesignWeaponPowerTime;
    protected float DesignFirepower;
    protected float DesignDefenseRating;

    #endregion

    // Use this for initialization
    protected override void Start ()
    {
        

    }
	
	// Update is called once per frame
	protected override void Update ()
    {
        SetMousePosition();

        if(ModulePanel.GetUseMirrorMode())
        {
            mirrorMousePosition = ModulePanel.GetMirrorModePosition(mousePosition);
        }
    }

    protected virtual Module GetSelectedModule()
    {
        return ModulePanel.GetSelectedModule();
    }

    protected ModuleSet GetSelectedModuleSet()
    {
        return ModulePanel.GetSelectedModuleSet();
    }

    protected virtual void BuildModuleSetLists()
    {

    }

    protected bool CheckHullModuleAllow(ModuleLimitType limit, ModuleCategory category)
    {
        if (category == ModuleCategory.Systems)
        {
            return true;
        }
        if (category == ModuleCategory.Engines)
        {
            if (limit == ModuleLimitType.NoEngines || limit == ModuleLimitType.NoWeaponsOrEngines)
            {
                return false;
            }
        }
        if (category == ModuleCategory.Weapons)
        {
            if (limit == ModuleLimitType.NoWeapons || limit == ModuleLimitType.NoWeaponsOrEngines)
            {
                return false;
            }
        }
        return true;
    }

    protected virtual void CheckPlacedModuleHover()
    {

    }

    protected void SetWeaponArcTexture(int angle)
    {
        switch (angle)
        {
            case 15:
                {
                    WeaponArcTex = ResourceManager.GetUITexture("WeaponArc15");
                    break;
                }
            case 30:
                {
                    WeaponArcTex = ResourceManager.GetUITexture("WeaponArc30");
                    break;
                }
            case 60:
                {
                    WeaponArcTex = ResourceManager.GetUITexture("WeaponArc60");
                    break;
                }
            case 90:
                {
                    WeaponArcTex = ResourceManager.GetUITexture("WeaponArc90");
                    break;
                }
            case 120:
                {
                    WeaponArcTex = ResourceManager.GetUITexture("WeaponArc120");
                    break;
                }
            case 150:
                {
                    WeaponArcTex = ResourceManager.GetUITexture("WeaponArc150");
                    break;
                }
            case 180:
                {
                    WeaponArcTex = ResourceManager.GetUITexture("WeaponArc180");
                    break;
                }
            case 210:
                {
                    WeaponArcTex = ResourceManager.GetUITexture("WeaponArc210");
                    break;
                }
            case 240:
                {
                    WeaponArcTex = ResourceManager.GetUITexture("WeaponArc240");
                    break;
                }
            case 270:
                {
                    WeaponArcTex = ResourceManager.GetUITexture("WeaponArc270");
                    break;
                }
            case 300:
                {
                    WeaponArcTex = ResourceManager.GetUITexture("WeaponArc300");
                    break;
                }
            case 330:
                {
                    WeaponArcTex = ResourceManager.GetUITexture("WeaponArc330");
                    break;
                }
            case 360:
                {
                    WeaponArcTex = ResourceManager.GetUITexture("WeaponArc360");
                    break;
                }
        }
    }

    protected void DeselectModule()
    {
        ModulePanel.DeselectModule();
        //ModuleStatsList.Clear();
    }

    //This currently just sums, but needs more complex calculations to account for enough ammo and energy
    protected void CalculateFirePower()
    {
        DesignFirepower = (DesignFirePowerAmmo + DesignFirePowerPower + DesignFirePowerCombination + DesignFirePowerFighter) / 10f;
    }

    protected void SwapModulesInList(ModuleSet newModuleSet, List<SlotedModule> sectionModules)
    {
        foreach (SlotedModule slotedModule in sectionModules)
        {
            if (slotedModule.module.GetParentSet().SwapType == newModuleSet.SwapType)
            {
                foreach (Module newModule in newModuleSet.GetModules())
                {
                    if (slotedModule.module.SizeX == newModule.SizeX && slotedModule.module.SizeY == newModule.SizeY)
                    {
                        slotedModule.SwapModule(newModule);
                    }
                }
            }
        }
    }

    protected bool ModuleSetCanSwap(ModuleSet modSet)
    {
        if (modSet.SwapType != ModuleSetSwapType.None)
        {
            return true;
        }
        return false;
    }

    protected float GetTotalValue(float production, float alloy, float advancedAlloy, float superiorAlloy, float crystal, float rareCrystal, float exoticCrystal, float exoticParticle)
    {
        return ResourceManager.gameConstants.GetBaseResourceValue(production, alloy, advancedAlloy, superiorAlloy, crystal, rareCrystal, exoticCrystal, exoticParticle);
    }

    /*protected void DrawHoveredModuleInfo(Vector2 position, Module module)
    {
        if (LastHoveredModule == null || LastHoveredModule != module)
        {
            LastHoveredModule = module;
            LastHoveredModuleStats = BuildModuleStats(module, Vector2.zero);
        }

        float xPos = position.x + Screen.width * 0.01f;
        float width = Screen.width * 0.18f;
        float Indent = xPos + width * 0.05f;
        float InsideWidth = width * 0.9f;
        float height;
        string text;
        Weapon weapon = module.GetWeapon();

        Rect ModuleSetNameRect = new Rect(Indent, position.y, InsideWidth, Screen.height * 0.03f);

        text = ResourceManager.GetLocalization(module.GetParentSet().Description);
        GameManager.instance.UIContent.text = text;
        height = GameManager.instance.ModuleDescStyle.CalcHeight(GameManager.instance.UIContent, width * 0.9f);

        Rect ModuleSetDescRect = new Rect(Indent, ModuleSetNameRect.yMax, InsideWidth, height);

        height = ModuleSetNameRect.height + ModuleSetDescRect.height + ModuleStatSize.y * (LastHoveredModuleStats.Count / 4 + 1);
        if (weapon != null)
        {
            height += ModuleWeaponGraphRect.height + ModuleWeaponRangeRect.height + ModuleWeaponGraphTitleRect.height * 1.25f;
        }

        Rect ModuleInfoRect = new Rect(xPos, position.y, width, height);

        GUI.Box(ModuleInfoRect, "", GameManager.instance.standardBackGround);
        GUI.Label(ModuleSetNameRect, ResourceManager.GetLocalization(module.GetParentSet().Name), GameManager.instance.ModuleTitleStyle);
        GUI.Label(ModuleSetDescRect, ResourceManager.GetLocalization(module.GetParentSet().Description), GameManager.instance.ModuleDescStyle);

        for (int i = 0; i < LastHoveredModuleStats.Count; i++)
        {
            Vector2 statPosition = new Vector2(Indent + ModuleStatSize.x * (i % 4), ModuleSetDescRect.yMax + ModuleStatSize.y * (i / 4));
            LastHoveredModuleStats[i].DrawOffset(statPosition);
        }

        if (weapon != null)
        {
            float DamageBonus = GetModifiedDamage(weapon);
            float maxDamage = Mathf.Max(weapon.GetMaxDamage() + weapon.GetMaxDamage() * DamageBonus, 1f);
            float maxRange = Mathf.Max(weapon.GetMaxRangeDisplay(), 1f);
            float halfIndent = width * 0.025f;

            Vector2 graphPosition = new Vector2(xPos + ModuleWeaponDamageRect.width + halfIndent, ModuleSetDescRect.yMax + ModuleStatSize.y * (LastHoveredModuleStats.Count / 4 + 1));

            GUI.Label(new Rect(graphPosition, ModuleWeaponGraphTitleRect.size), "Weapon Damage Over Range", GameManager.instance.ModuleTitleStyle);

            graphPosition = new Vector2(xPos + halfIndent, graphPosition.y + ModuleWeaponGraphTitleRect.height);

            GUI.Label(new Rect(graphPosition, ModuleWeaponDamageRect.size), "Damage: " + maxDamage.ToString("0"), GameManager.instance.standardLabelStyle);

            graphPosition = new Vector2(xPos + ModuleWeaponDamageRect.width + halfIndent, graphPosition.y);
            Rect DamageGraphRect = new Rect(graphPosition.x + ModuleWeaponGraphRect.width * 0.05f, graphPosition.y + ModuleWeaponGraphRect.height * 0.05f, ModuleWeaponGraphRect.width * 0.9f, ModuleWeaponGraphRect.height * 0.9f);

            GUI.DrawTexture(new Rect(graphPosition, ModuleWeaponGraphRect.size), ResourceManager.GetUITexture("WeaponDamageGraph"));

            graphPosition = new Vector2(xPos + ModuleWeaponDamageRect.width + ModuleWeaponGraphRect.width - ModuleWeaponRangeRect.width + halfIndent, graphPosition.y + ModuleWeaponGraphRect.height);

            GUI.Label(new Rect(graphPosition, ModuleWeaponRangeRect.size), "Range: " + maxRange.ToString("0"), GameManager.instance.standardLabelStyle);


            List<Vector2> Points = new List<Vector2>();
            foreach (Weapon.DamageNode node in weapon.DamageGraph)
            {
                Points.Add(new Vector2(DamageGraphRect.x + DamageGraphRect.width * (node.GetDisplayRange() / maxRange), DamageGraphRect.yMax - DamageGraphRect.height * ((node.Damage + node.Damage * DamageBonus) / maxDamage)));
            }
            for (int i = 0; i < Points.Count - 1; i++)
            {
                new GUILine(Points[i], Points[i + 1], Color.red).Draw();
            }
        }
    }*/

    protected virtual bool CheckDesignAllowed(ShipDesign design)
    {
        return true;
    }

    protected virtual bool CheckDesignAllowed(StationDesign design)
    {
        return true;
    }

    protected virtual bool CheckHullAllowed(ShipHullData hull)
    {
        return true;
    }

    protected virtual bool CheckHullAllowed(StationHullData hull)
    {
        return true;
    }

    protected virtual bool CheckModuleSetAllowed(ModuleSet moduleSet)
    {
        return true;
    }

    protected virtual float GetModifiedDamage(Weapon weapon)
    {
        float moduleBonus = Mathf.Max(DesignDamageBonus);

        return moduleBonus;
    }

    protected void CalculateDesignWeaponPowerAmmoTime()
    {
        if (DesignWeaponAmmoConsumption > 0)
        {
            if (DesignAmmoGenerated >= DesignWeaponAmmoConsumption)
            {
                DesignWeaponAmmoTime = 9999;
            }
            else
            {
                DesignWeaponAmmoTime = DesignAmmo / (DesignWeaponAmmoConsumption - DesignAmmoGenerated);
            }
        }
        if (DesignWeaponPowerConsumption > 0)
        {
            if (DesignPowerSum >= DesignWeaponPowerConsumption)
            {
                DesignWeaponPowerTime = 9999;
            }
            else
            {
                DesignWeaponPowerTime = DesignPowerStorage / (DesignWeaponPowerConsumption - DesignPowerSum);
            }
        }
    }

    public string GetFormatedDesignRequiredCrew()
    {
        if (DesignRequiredCrew > DesignCrew)
        {
            return "<color=yellow>" + GetStandardDesignStatFormat(DesignRequiredCrew) + "/" + DesignCrew.ToString() + "</color>";
        }
        else
        {
            return GetStandardDesignStatFormat(DesignRequiredCrew) + "/" + DesignCrew.ToString();
        }
    }

    public string GetFormatedDesignTroops()
    {
        return GetStandardDesignStatFormat(DesignTroops);
    }

    public string GetFormattedDesignCost()
    {
        return GetStandardDesignStatFormat(DesignCost);
    }

    public string GetFormatedDesignProductionCost()
    {
        return GetStandardDesignStatFormat(DesignProductionCost);
    }

    public string GetFormattedDesignAlloyCost()
    {
        return GetStandardDesignStatFormat(DesignAlloyCost);
    }

    public string GetFormattedDesignAdvancedAlloyCost()
    {
        return GetStandardDesignStatFormat(DesignAdvancedAlloyCost);
    }

    public string GetFormattedDesignSuperiorAlloyCost()
    {
        return GetStandardDesignStatFormat(DesignSuperiorAlloyCost);
    }

    public string GetFormattedDesignCrystalCost()
    {
        return GetStandardDesignStatFormat(DesignCrystalCost);
    }

    public string GetFormattedDesignRareCrystalCost()
    {
        return GetStandardDesignStatFormat(DesignRareCrystalCost);
    }

    public string GetFormattedDesignExoticCrystalCost()
    {
        return GetStandardDesignStatFormat(DesignExoticCrystalCost);
    }

    public string GetFormattedDesignExoticParticleCost()
    {
        return GetStandardDesignStatFormat(DesignExoticParticleCost);
    }

    public string GetFormattedDesignPowerSum()
    {
        return GetOnlyPositiveDesignStatFormat(DesignPowerSum);
    }

    public string GetFormattedDesignAmmo()
    {
        return GetStandardDesignStatFormat(DesignAmmo);
    }

    public string GetFormattedDesignPowerStorage()
    {
        return GetStandardDesignStatFormat(DesignPowerStorage);
    }

    public string GetFormattedDesignSensor()
    {
        return GetStandardDesignStatFormat(DesignSensor);
    }

    public string GetFormattedDesignLongRangeSensorSum()
    {
        return GetStandardDesignStatFormat(DesignLongRangeSensorSum);
    }

    public string GetFormattedDesignAdvancedSensor()
    {
        return GetStandardDesignStatFormat(DesignAdvancedSensor);
    }

    public string GetFormattedDesignResearch()
    {
        return GetStandardDesignStatFormat(DesignResearch);
    }

    public string GetFormattedDesignMining()
    {
        return GetStandardDesignStatFormat(DesignMining);
    }

    public string GetFormattedDesignRepair()
    {
        return GetStandardDesignStatFormat(DesignRepair);
    }

    public string GetFormattedDesignAmmoGenerated()
    {
        return GetStandardDesignStatFormat(DesignAmmoGenerated);
    }

    public string GetFormattedDesignFighters()
    {
        return GetStandardDesignStatFormat(DesignFighters);
    }

    public string GetFormattedDesignHeavyFighters()
    {
        return GetStandardDesignStatFormat(DesignHeavyFighters);
    }

    public string GetFormattedDesignAssaultPods()
    {
        return GetStandardDesignStatFormat(DesignAssaultPods);
    }

    public string GetFormattedDesignMedical()
    {
        return GetStandardDesignStatFormat(DesignMedical);
    }

    public string GetFormattedDesignBoardingDefense()
    {
        return GetStandardDesignStatFormat(DesignBoardingDefense);
    }

    public string GetFormattedDesignTransporter()
    {
        return GetStandardDesignStatFormat(DesignTransporter);
    }

    public string GetFormattedDesignFirepower()
    {
        return GetStandardDesignStatFormat(DesignFirepower);
    }

    public string GetFormattedDesignWeaponAmmoConsumption()
    {
        return GetStandardDesignStatFormat(DesignWeaponAmmoConsumption);
    }

    public string GetFormattedDesignWeaponPowerConsumption()
    {
        return GetStandardDesignStatFormat(DesignWeaponPowerConsumption);
    }

    public string GetFormattedDesignDefenseRating()
    {
        return GetStandardDesignStatFormat(DesignDefenseRating);
    }

    public string GetFormattedDesignExperience()
    {
        return GetStandardDesignStatFormat(DesignExperience);
    }

    public string GetFormattedDesignWeaponPowerTime()
    {
        if (DesignWeaponPowerTime > 9998)
        {
            return "Infinite";
        }
        else
        {
            return GetStandardDesignStatFormat(DesignWeaponPowerTime);
        }
    }

    public string GetFormattedDesignWeaponAmmoTime()
    {
        if (DesignWeaponAmmoTime > 9998)
        {
            return "Infinite";
        }
        else
        {
            return GetStandardDesignStatFormat(DesignWeaponAmmoTime);
        }
    }

    public string GetFormattedDesignJammingCount()
    {
        return GetStandardDesignStatFormat(DesignJammingCount);
    }

    public string GetFormattedDesignJammingRange()
    {
        return GetStandardDesignStatFormat(DesignJammingRange);
    }

    public string GetFormattedDesignJammingDelay()
    {
        return GetStandardDesignStatFormat(DesignJammingDelay);
    }

    public string GetFormattedDesignDamageBonus()
    {
        return GetPercentageDesignStatFormat(DesignDamageBonus);
    }

    public string GetFormattedDesignDefenseBonus()
    {
        return GetPercentageDesignStatFormat(DesignDefenseBonus);
    }

    public string GetFormattedDesignPowerEfficiency()
    {
        return GetPercentageDesignStatFormat(DesignPowerEfficiency);
    }

    public string GetFormattedDesignDiplomacy()
    {
        return GetStandardDesignStatFormat(DesignDiplomacy);
    }

    protected string GetStandardDesignStatFormat(float value)
    {
        return value.ToString("0.#");
    }

    protected string GetPercentageDesignStatFormat(float value)
    {
        return "%" + GetStandardDesignStatFormat(value * 100);
    }

    protected string GetOnlyPositiveDesignStatFormat(float value)
    {
        if (value > 0)
        {
            return GetStandardDesignStatFormat(value);
        }
        else
        {
            return "<color=red>" + GetStandardDesignStatFormat(value) + "</color>";
        }
    }
}
