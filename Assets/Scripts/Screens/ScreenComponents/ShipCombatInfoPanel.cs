/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections.Generic;

public class ShipCombatInfoPanel
{
    #region Variables
    ShipManager shipManager;

    Rect baseRect;
    Rect DamageDisplayRect;
    Rect InfoDisplayRect;
    Rect OrderDisplayRect;
    Rect GroupShipWindowRect;
    Rect GroupShipViewRect;
    Vector2 GroupShipPosition = Vector2.zero;

    Rect NameRect;
    Rect DesignRect;
    Rect HullRect;
    Rect PowerIconRect;
    Rect AmmoIconRect;
    Rect CrewIconRect;
    Rect TroopIconRect;
    Rect LevelIconRect;

    Texture2D LevelIcon;
    Texture2D PowerIcon;
    Texture2D AmmoIcon;
    Texture2D CrewIcon;
    Texture2D TroopIcon;

    GUIStatBar PowerBar;
    GUIStatBar AmmoBar;
    GUIStatBar CrewBar;
    GUIStatBar TroopBar;
    GUIStatBar LevelBar;

    Rect DamageBonusRect;
    Rect DefenseBonusRect;
    Texture2D DamageIcon;
    Texture2D DefenseIcon;

    Texture2D CenterShields;
    Texture2D ForeShields;
    Texture2D AftShields;
    Texture2D PortShields;
    Texture2D StarboardShields;
    Texture2D CenterArmor;
    Texture2D ForeArmor;
    Texture2D AftArmor;
    Texture2D PortArmor;
    Texture2D StarboardArmor;
    Texture2D CenterHealth;
    Texture2D ForeHealth;
    Texture2D AftHealth;
    Texture2D PortHealth;
    Texture2D StarboardHealth;
    Texture2D Health;
    Texture2D Armor;
    Texture2D Shields;

    Texture2D DamageBarTexture;

    //Orders rects
    Rect ButtonTwoRect;
    Rect ButtonThreeRect;
    Rect ButtonFourRect;
    Rect ButtonFiveRect;
    Rect ButtonSixRect;
    Rect ButtonSevenRect;
    Rect ButtonEightRect;
    Rect ButtonNineRect;
    Rect ButtonTenRect;
    Rect ButtonElevenRect;
    Rect ButtonTwelveRect;
    Rect ButtonThirteenRect;
    Rect ButtonFourteenRect;
    Rect ButtonFithteenRect;
    Rect ButtonSixteenRect;

    //OrderButtons
    CancelOrdersButton cancelOrderButton;
    AttackDirectionButton attackDirectionButton;
    AttackStyleButton attackStyleButton;
    FireControlButton fireControlButton;
    FighterLaunchButton fighterLaunchButton;
    HeavyFighterLaunchButton heavyFighterLaunchButton;
    AssaultPodsLaunchButton assaultPodsLaunchButton;
    TransportTroopsButton transportTroopsButton;
    TransportCrewButton transportCrewButton;
    CloakingButton cloakingButton;
    RetreatButton retreatButton;
    SelfDestructButton selfDestructButton;

    Rect RangeAreaRect;
	Rect RangeRect;
    Rect RangeSliderRect;
    Rect[] DamageBars = new Rect[20];

    float OrderButtonSize;
    float shipButtonSize;

    Ship lastSelectedShip;

    //Tool tip
    GUIToolTip ToolTip;

    CombatScreens ParentScreen;
    #endregion

    public ShipCombatInfoPanel(CombatScreens parent, Rect rect, ShipManager manager, GUIToolTip toolTip)
    {
        ParentScreen = parent;
        SetBasicTextures();

        shipManager = manager;
        baseRect = rect;
        ToolTip = toolTip;

        Vector2 InnerRectSize = new Vector2(baseRect.width - baseRect.height * 0.08f, baseRect.height * 0.92f);

        DamageDisplayRect = new Rect(baseRect.x + baseRect.height * 0.04f, baseRect.y + baseRect.height * 0.04f, InnerRectSize.y, InnerRectSize.y);
        InfoDisplayRect = new Rect(DamageDisplayRect.xMax + baseRect.height * 0.04f, DamageDisplayRect.y, DamageDisplayRect.width * 1.35f, InnerRectSize.y);
        float InfoDisplayElementHeight = InfoDisplayRect.height / 9.25f;
        OrderDisplayRect = new Rect(InfoDisplayRect.xMax + baseRect.height * 0.04f, DamageDisplayRect.y, baseRect.xMax - (InfoDisplayRect.xMax + baseRect.height * 0.08f), InnerRectSize.y);
        OrderButtonSize = OrderDisplayRect.width / 8.25f;

        GroupShipWindowRect = new Rect(DamageDisplayRect.x, DamageDisplayRect.y, InfoDisplayRect.xMax - DamageDisplayRect.x, InnerRectSize.y);
        GroupShipViewRect = new Rect(0,0, GroupShipWindowRect.width * 0.95f, GroupShipWindowRect.height);
        shipButtonSize = GroupShipViewRect.width / 8f;

        NameRect = new Rect(InfoDisplayRect.x + InfoDisplayRect.width * 0.04f, InfoDisplayRect.y + InfoDisplayRect.width * 0.02f, InfoDisplayRect.width, InfoDisplayElementHeight);
        DesignRect = new Rect(NameRect.x, NameRect.yMax, InfoDisplayRect.width, InfoDisplayElementHeight);
        HullRect = new Rect(NameRect.x, DesignRect.yMax, InfoDisplayRect.width, InfoDisplayElementHeight);

        LevelIconRect = new Rect(InfoDisplayRect.x + InfoDisplayRect.width * 0.05f, HullRect.yMax, InfoDisplayElementHeight, InfoDisplayElementHeight);
        PowerIconRect = new Rect(InfoDisplayRect.x + InfoDisplayRect.width * 0.05f, LevelIconRect.yMax, InfoDisplayElementHeight, InfoDisplayElementHeight);
        AmmoIconRect = new Rect(PowerIconRect.x, PowerIconRect.yMax, InfoDisplayElementHeight, InfoDisplayElementHeight);
        CrewIconRect = new Rect(PowerIconRect.x, AmmoIconRect.yMax, InfoDisplayElementHeight, InfoDisplayElementHeight);
        TroopIconRect = new Rect(PowerIconRect.x, CrewIconRect.yMax, InfoDisplayElementHeight, InfoDisplayElementHeight);  

        float indent = InfoDisplayRect.width * 0.05f;
        float width = InfoDisplayRect.width - (InfoDisplayRect.width * 0.15f + InfoDisplayElementHeight);
        Rect LevelBarRect = new Rect(LevelIconRect.xMax + indent, LevelIconRect.y, width, InfoDisplayElementHeight);
        Rect PowerBarRect = new Rect(LevelBarRect.x, PowerIconRect.y, width, InfoDisplayElementHeight);
        Rect AmmoBarRect = new Rect(PowerBarRect.x, AmmoIconRect.y, width, InfoDisplayElementHeight);
        Rect CrewBarRect = new Rect(PowerBarRect.x, CrewIconRect.y, width, InfoDisplayElementHeight);
        Rect TroopBarRect = new Rect(PowerBarRect.x, TroopIconRect.y, width, InfoDisplayElementHeight);

        LevelBar = new GUIStatBar(LevelBarRect, "StatBarBackground", "StatBarExperience");
        PowerBar = new GUIStatBar(PowerBarRect, "StatBarBackground", "StatBarPower");
        AmmoBar = new GUIStatBar(AmmoBarRect, "StatBarBackground", "StatBarAmmo");
        CrewBar = new GUIStatBar(CrewBarRect, "StatBarBackground", "StatBarCrew");
        TroopBar = new GUIStatBar(TroopBarRect, "StatBarBackground", "StatBarTroop");

        DamageBonusRect = new Rect(LevelIconRect.x, TroopIconRect.yMax, GameManager.instance.StandardLabelSize.x * 0.6f, InfoDisplayElementHeight);
        DefenseBonusRect = new Rect(DamageBonusRect.xMax, DamageBonusRect.y, DamageBonusRect.width, InfoDisplayElementHeight);
        //DamageIcon = ResourceManager.instance.GetIconTexture("Icon_Damage");
        //DefenseIcon = ResourceManager.instance.GetIconTexture("Icon_Defense");

        float buttonIndent = (OrderDisplayRect.width - OrderButtonSize * 8) / 2f;

        Rect ButtonOneRect = new Rect(OrderDisplayRect.x + buttonIndent, OrderDisplayRect.y + buttonIndent, OrderButtonSize, OrderButtonSize);       
        ButtonTwoRect = new Rect(ButtonOneRect.xMax, ButtonOneRect.y, OrderButtonSize, OrderButtonSize);
        ButtonThreeRect = new Rect(ButtonTwoRect.xMax, ButtonOneRect.y, OrderButtonSize, OrderButtonSize);
        ButtonFourRect = new Rect(ButtonThreeRect.xMax, ButtonOneRect.y, OrderButtonSize, OrderButtonSize);
        ButtonFiveRect = new Rect(ButtonFourRect.xMax, ButtonOneRect.y, OrderButtonSize, OrderButtonSize);
        ButtonSixRect = new Rect(ButtonFiveRect.xMax, ButtonOneRect.y, OrderButtonSize, OrderButtonSize);
        ButtonSevenRect = new Rect(ButtonSixRect.xMax, ButtonOneRect.y, OrderButtonSize, OrderButtonSize);
        ButtonEightRect = new Rect(ButtonSevenRect.xMax, ButtonOneRect.y, OrderButtonSize, OrderButtonSize);
        ButtonNineRect = new Rect(ButtonOneRect.x, ButtonOneRect.yMax, OrderButtonSize, OrderButtonSize);
        ButtonTenRect = new Rect(ButtonNineRect.xMax, ButtonOneRect.yMax, OrderButtonSize, OrderButtonSize);
        ButtonElevenRect = new Rect(ButtonTenRect.xMax, ButtonOneRect.yMax, OrderButtonSize, OrderButtonSize);
        ButtonTwelveRect = new Rect(ButtonElevenRect.xMax, ButtonOneRect.yMax, OrderButtonSize, OrderButtonSize);
        ButtonThirteenRect = new Rect(ButtonTwelveRect.xMax, ButtonOneRect.yMax, OrderButtonSize, OrderButtonSize);
        ButtonFourteenRect = new Rect(ButtonThirteenRect.xMax, ButtonOneRect.yMax, OrderButtonSize, OrderButtonSize);
        ButtonFithteenRect = new Rect(ButtonFourteenRect.xMax, ButtonOneRect.yMax, OrderButtonSize, OrderButtonSize);
        ButtonSixteenRect = new Rect(ButtonFithteenRect.xMax, ButtonOneRect.yMax, OrderButtonSize, OrderButtonSize);

        //Order Buttons
        /*
        cancelOrderButton = new CancelOrdersButton(ButtonOneRect, ResourceManager.instance.GetIconTexture("Icon_Stop"), PlayMainButtonClick, ToolTip);
        attackDirectionButton = new AttackDirectionButton(ButtonTwoRect, ResourceManager.instance.GetIconTexture("Icon_ShipAttackRight"), ResourceManager.instance.GetIconTexture("Icon_ShipAttackLeft"), ResourceManager.instance.GetIconTexture("Icon_ShipAttackForward"), PlayMainButtonClick, ToolTip);
        attackStyleButton = new AttackStyleButton(ButtonThreeRect, ResourceManager.instance.GetIconTexture("Icon_StrafingAttack"), ResourceManager.instance.GetIconTexture("Icon_ShipHoldPositionAttack"), ResourceManager.instance.GetIconTexture("Icon_DontAttack"), PlayMainButtonClick, ToolTip);
        fireControlButton = new FireControlButton(ButtonFourRect, ResourceManager.instance.GetIconTexture("Icon_ShipHoldFire"), ResourceManager.instance.GetIconTexture("Icon_ShipFireAtWill"), PlayMainButtonClick, ToolTip);
        fighterLaunchButton = new FighterLaunchButton(ButtonFiveRect, ResourceManager.instance.GetIconTexture("Icon_LaunchFighters"), ResourceManager.instance.GetIconTexture("Icon_RecallFighters"), PlayMainButtonClick, ToolTip);
        heavyFighterLaunchButton = new HeavyFighterLaunchButton(ButtonSixRect, ResourceManager.instance.GetIconTexture("Icon_LaunchHeavyFighters"), ResourceManager.instance.GetIconTexture("Icon_RecallHeavyFighters"), PlayMainButtonClick, ToolTip);
        assaultPodsLaunchButton = new AssaultPodsLaunchButton(ButtonSevenRect, ResourceManager.instance.GetIconTexture("Icon_LaunchAssaultPods"), ResourceManager.instance.GetIconTexture("Icon_RecallAssaultPods"), PlayMainButtonClick, ToolTip);
        cloakingButton = new CloakingButton(ButtonEightRect, ResourceManager.instance.GetIconTexture("Icon_CloakCancel"), ResourceManager.instance.GetIconTexture("Icon_Cloak"), PlayMainButtonClick, ToolTip);
        transportTroopsButton = new TransportTroopsButton(ButtonNineRect, ResourceManager.instance.GetIconTexture("Icon_TransportTroopCancel"), ResourceManager.instance.GetIconTexture("Icon_TransportTroop"), PlayMainButtonClick, ToolTip);
        transportCrewButton = new TransportCrewButton(ButtonTenRect, ResourceManager.instance.GetIconTexture("Icon_TransportCrewCancel"), ResourceManager.instance.GetIconTexture("Icon_TransportCrew"), PlayMainButtonClick, ToolTip);
        retreatButton = new RetreatButton(ButtonFithteenRect, ResourceManager.instance.GetIconTexture("Icon_RetreatCancel"), ResourceManager.instance.GetIconTexture("Icon_Retreat"), PlayMainButtonClick, ToolTip);
        selfDestructButton = new SelfDestructButton(ButtonSixteenRect, ResourceManager.instance.GetIconTexture("Icon_SelfDestructCancel"), ResourceManager.instance.GetIconTexture("Icon_SelfDestruct"), PlayMainButtonClick, ToolTip);
        */

        cancelOrderButton.SetShipManager(shipManager);
        attackDirectionButton.SetShipManager(shipManager);
        attackStyleButton.SetShipManager(shipManager);
        fireControlButton.SetShipManager(shipManager);
        fighterLaunchButton.SetShipManager(shipManager);
        heavyFighterLaunchButton.SetShipManager(shipManager);
        assaultPodsLaunchButton.SetShipManager(shipManager);
        cloakingButton.SetShipManager(shipManager);
        transportTroopsButton.SetShipManager(shipManager);
        transportCrewButton.SetShipManager(shipManager);
        retreatButton.SetShipManager(shipManager);
        selfDestructButton.SetShipManager(shipManager);

        RangeAreaRect = new Rect(OrderDisplayRect.x, OrderDisplayRect.yMax - (OrderButtonSize + GameManager.instance.StandardLabelSize.y * 1.25f), OrderDisplayRect.width * 0.667f, OrderButtonSize + GameManager.instance.StandardLabelSize.y * 1.25f);
        RangeRect = new Rect(RangeAreaRect.x + RangeAreaRect.width * 0.04f, RangeAreaRect.y + GameManager.instance.StandardLabelSize.y * 0.1f, GameManager.instance.StandardLabelSize.x * 1.3f, GameManager.instance.StandardLabelSize.y * 1.25f);
        RangeSliderRect = new Rect(RangeRect.x, RangeRect.yMax + GameManager.instance.StandardLabelSize.y * 0.15f, RangeAreaRect.width * 0.92f, OrderButtonSize * 0.75f);
        float increment = RangeSliderRect.width / 20f;
        for (int i = 0; i < 20; i++)
        {
            DamageBars[i] = new Rect(RangeSliderRect.x + i * increment, RangeSliderRect.y, increment, RangeSliderRect.height);
        }
    }

    void SetBasicTextures()
    {
        /*
        CenterShields = ResourceManager.instance.GetUITexture("ShipStats_CenterShield");
        ForeShields = ResourceManager.instance.GetUITexture("ShipStats_ForeShield");
        AftShields = ResourceManager.instance.GetUITexture("ShipStats_AftShield");
        PortShields = ResourceManager.instance.GetUITexture("ShipStats_PortShield");
        StarboardShields = ResourceManager.instance.GetUITexture("ShipStats_StarboardShield");
        CenterArmor = ResourceManager.instance.GetUITexture("ShipStats_CenterArmor");
        ForeArmor = ResourceManager.instance.GetUITexture("ShipStats_ForeArmor");
        AftArmor = ResourceManager.instance.GetUITexture("ShipStats_AftArmor");
        PortArmor = ResourceManager.instance.GetUITexture("ShipStats_PortArmor");
        StarboardArmor = ResourceManager.instance.GetUITexture("ShipStats_StarboardArmor");
        CenterHealth = ResourceManager.instance.GetUITexture("ShipStats_CenterHealth");
        ForeHealth = ResourceManager.instance.GetUITexture("ShipStats_ForeHealth");
        AftHealth = ResourceManager.instance.GetUITexture("ShipStats_AftHealth");
        PortHealth = ResourceManager.instance.GetUITexture("ShipStats_PortHealth");
        StarboardHealth = ResourceManager.instance.GetUITexture("ShipStats_StarboardHealth");

        Health = ResourceManager.instance.GetUITexture("FighterStats_Health");
        Armor = ResourceManager.instance.GetUITexture("FighterStats_Armor");
        Shields = ResourceManager.instance.GetUITexture("FighterStats_Shield");

        LevelIcon = ResourceManager.instance.GetIconTexture("Icon_Level");
        PowerIcon = ResourceManager.instance.GetIconTexture("Icon_Power");
        AmmoIcon = ResourceManager.instance.GetIconTexture("Icon_Ammo");
        CrewIcon = ResourceManager.instance.GetIconTexture("Icon_Crew");
        TroopIcon = ResourceManager.instance.GetIconTexture("Icon_Troop");

        DamageBarTexture = ResourceManager.instance.GetUITexture("DamageBar");
        */
    }

    public void Draw(Vector2 mousePosition)
    {
        if (shipManager.GetUnOwnedShip() != null)
        {
            DrawShipInfo(shipManager.GetUnOwnedShip(), false, mousePosition);
        }
        else if (shipManager.GetUnOwnedStation() != null)
        {
            DrawStationInfo(shipManager.GetUnOwnedStation(), false, mousePosition);
        }
        else if (shipManager.GetUnOwnedFighterWing() != null)
        {
            DrawFighterInfo(shipManager.GetUnOwnedFighterWing(), false, mousePosition);
        }
        else if (shipManager.SelectedCount() > 1)
        {
            DrawGroupInfo(shipManager.GetSelectedShips(), shipManager.GetSelectedStations(), shipManager.GetSelectedFighters(), mousePosition);
        }
        else if (shipManager.GetSelectedShips().Count == 1)
        {
            DrawShipInfo(shipManager.GetSelectedShips()[0], true, mousePosition);
        }
        else if (shipManager.GetSelectedStations().Count == 1)
        {
            DrawStationInfo(shipManager.GetSelectedStations()[0], true, mousePosition);
        }
        else if (shipManager.GetSelectedFighters().Count == 1)
        {
            DrawFighterInfo(shipManager.GetSelectedFighters()[0], true, mousePosition);
        }
    }

    public void DrawShipInfo(Ship ship, bool control, Vector2 mousePosition)
    {
        if (ship == null)
            return;

        if(ship != lastSelectedShip)
        {
            lastSelectedShip = ship;
            BuildDamageBars(ship);
        }

        GUI.Box(baseRect, "");

        #region DamageOverlay
        //Regional damage display
        GUI.Box(DamageDisplayRect, "");
        if (!ship.inCloakingState())
        {
            if (ship.GetShipData().CenterSection.shieldHealthMax > 0)
            {
                GUI.color = new Color(1, 1, 1, ship.GetShipData().CenterSection.shieldHealth / ship.GetShipData().CenterSection.shieldHealthMax);
                GUI.DrawTexture(DamageDisplayRect, CenterShields);
                GUI.color = Color.white;
            }
            if (ship.GetShipData().ForeSection.shieldHealthMax > 0)
            {
                GUI.color = new Color(1, 1, 1, ship.GetShipData().ForeSection.shieldHealth / ship.GetShipData().ForeSection.shieldHealthMax);
                GUI.DrawTexture(DamageDisplayRect, ForeShields);
                GUI.color = Color.white;
            }
            if (ship.GetShipData().AftSection.shieldHealthMax > 0)
            {
                GUI.color = new Color(1, 1, 1, ship.GetShipData().AftSection.shieldHealth / ship.GetShipData().AftSection.shieldHealthMax);
                GUI.DrawTexture(DamageDisplayRect, AftShields);
                GUI.color = Color.white;
            }
            if (ship.GetShipData().PortSection.shieldHealthMax > 0)
            {
                GUI.color = new Color(1, 1, 1, ship.GetShipData().PortSection.shieldHealth / ship.GetShipData().PortSection.shieldHealthMax);
                GUI.DrawTexture(DamageDisplayRect, PortShields);
                GUI.color = Color.white;
            }
            if (ship.GetShipData().StarboardSection.shieldHealthMax > 0)
            {
                GUI.color = new Color(1, 1, 1, ship.GetShipData().StarboardSection.shieldHealth / ship.GetShipData().StarboardSection.shieldHealthMax);
                GUI.DrawTexture(DamageDisplayRect, StarboardShields);
                GUI.color = Color.white;
            }
        }
        if (ship.GetShipData().CenterSection.armorHealthMax > 0)
        {
            if (ship.GetShipData().CenterSection.armorHealth == 0)
            {
                GUI.color = Color.black;
            }
            else
            {
                float damageRatio = ship.GetShipData().CenterSection.armorHealth / ship.GetShipData().CenterSection.armorHealthMax;
                GUI.color = new Color(1, damageRatio, damageRatio);
            }
            GUI.DrawTexture(DamageDisplayRect, CenterArmor);
            GUI.color = Color.white;
        }
        if (ship.GetShipData().ForeSection.armorHealthMax > 0)
        {
            if (ship.GetShipData().ForeSection.armorHealth == 0)
            {
                GUI.color = Color.black;
            }
            else
            {
                float damageRatio = ship.GetShipData().ForeSection.armorHealth / ship.GetShipData().ForeSection.armorHealthMax;
                GUI.color = new Color(1, damageRatio, damageRatio);
            }
            GUI.DrawTexture(DamageDisplayRect, ForeArmor);
            GUI.color = Color.white;
        }
        if (ship.GetShipData().AftSection.armorHealthMax > 0)
        {
            if (ship.GetShipData().AftSection.armorHealth == 0)
            {
                GUI.color = Color.black;
            }
            else
            {
                float damageRatio = ship.GetShipData().AftSection.armorHealth / ship.GetShipData().AftSection.armorHealthMax;
                GUI.color = new Color(1, damageRatio, damageRatio);
            }
            GUI.DrawTexture(DamageDisplayRect, AftArmor);
            GUI.color = Color.white;
        }
        if (ship.GetShipData().PortSection.armorHealthMax > 0)
        {
            if (ship.GetShipData().PortSection.armorHealth == 0)
            {
                GUI.color = Color.black;
            }
            else
            {
                float damageRatio = ship.GetShipData().PortSection.armorHealth / ship.GetShipData().PortSection.armorHealthMax;
                GUI.color = new Color(1, damageRatio, damageRatio);
            }
            GUI.DrawTexture(DamageDisplayRect, PortArmor);
            GUI.color = Color.white;
        }
        if (ship.GetShipData().StarboardSection.armorHealthMax > 0)
        {
            if (ship.GetShipData().StarboardSection.armorHealth == 0)
            {
                GUI.color = Color.black;
            }
            else
            {
                float damageRatio = ship.GetShipData().StarboardSection.armorHealth / ship.GetShipData().StarboardSection.armorHealthMax;
                GUI.color = new Color(1, damageRatio, damageRatio);
            }
            GUI.DrawTexture(DamageDisplayRect, StarboardArmor);
            GUI.color = Color.white;
        }

        if (ship.GetShipData().CenterSection.health == 0)
        {
            GUI.color = Color.black;
        }
        else
        {
            float damageRatio = ship.GetShipData().CenterSection.health / ship.GetShipData().CenterSection.healthMax;
            GUI.color = new Color(1, damageRatio, damageRatio);
        }
        GUI.DrawTexture(DamageDisplayRect, CenterHealth);
        GUI.color = Color.white;

        if (ship.GetShipData().ForeSection.health == 0)
        {
            GUI.color = Color.black;
        }
        else
        {
            float damageRatio = ship.GetShipData().ForeSection.health / ship.GetShipData().ForeSection.healthMax;
            GUI.color = new Color(1, damageRatio, damageRatio);
        }
        GUI.DrawTexture(DamageDisplayRect, ForeHealth);
        GUI.color = Color.white;

        if (ship.GetShipData().AftSection.health == 0)
        {
            GUI.color = Color.black;
        }
        else
        {
            float damageRatio = ship.GetShipData().AftSection.health / ship.GetShipData().AftSection.healthMax;
            GUI.color = new Color(1, damageRatio, damageRatio);
        }
        GUI.DrawTexture(DamageDisplayRect, AftHealth);
        GUI.color = Color.white;

        if (ship.GetShipData().PortSection.health == 0)
        {
            GUI.color = Color.black;
        }
        else
        {
            float damageRatio = ship.GetShipData().PortSection.health / ship.GetShipData().PortSection.healthMax;
            GUI.color = new Color(1, damageRatio, damageRatio);
        }
        GUI.DrawTexture(DamageDisplayRect, PortHealth);
        GUI.color = Color.white;

        if (ship.GetShipData().StarboardSection.health == 0)
        {
            GUI.color = Color.black;
        }
        else
        {
            float damageRatio = ship.GetShipData().StarboardSection.health / ship.GetShipData().StarboardSection.healthMax;
            GUI.color = new Color(1, damageRatio, damageRatio);
        }
        GUI.DrawTexture(DamageDisplayRect, StarboardHealth);
        GUI.color = Color.white;
        #endregion

        #region Ship Info
        //Info
        GUI.Box(InfoDisplayRect, "");
        ship.GetShipData().DisplayName = GUI.TextField(NameRect, ship.GetShipData().DisplayName, GameManager.instance.standardLabelStyle);
        //GUI.Label(NameRect, ship.GetShipData().DisplayName, Style);
        string design = "Design: " + ship.GetShipData().designData.Design.Name;

        //GUI.Label(DesignRect, design, GameManager.instance.standardLabelStyle);

        if(GUI.Button(DesignRect, design, GameManager.instance.standardLabelStyle))
        {
            PlayMainButtonClick();
            ParentScreen.SetDesignWindowDesign(ship.GetShipData().designData.Design);
        }

        string hull = ship.GetShipData().designData.Design.Hull + " - " + ResourceManager.instance.GetShipHull(ship.GetShipData().designData.Design.Hull).Classification;
        GUI.Label(HullRect, hull, GameManager.instance.standardLabelStyle);

        GUI.DrawTexture(LevelIconRect, LevelIcon);
        LevelBar.Draw(ship.GetShipData().ExperienceRatio(), "Level: " + ship.GetShipData().Level.ToString());

        GUI.DrawTexture(PowerIconRect, PowerIcon);
        float ratio = ship.GetShipData().power / ship.GetShipData().powerMax;
        PowerBar.Draw(ratio, ship.GetShipData().power.ToString("0.#") + "/" + ship.GetShipData().powerMax.ToString("0.#"));

        GUI.DrawTexture(AmmoIconRect, AmmoIcon);
        ratio = ship.GetShipData().ammo / ship.GetShipData().ammoMax;
        AmmoBar.Draw(ratio, ship.GetShipData().ammo.ToString("0.#") + "/" + ship.GetShipData().ammoMax.ToString("0.#"));

        GUI.DrawTexture(CrewIconRect, CrewIcon);
        CrewBar.Draw(ship.GetShipData().GetCrewRatio(), ship.GetShipData().crew.ToString() + "/" + ship.GetShipData().crewMax.ToString());

        GUI.DrawTexture(TroopIconRect, TroopIcon);
        TroopBar.Draw(ship.GetShipData().GetTroopRatio(), ship.GetShipData().troops.ToString() + "/" + ship.GetShipData().troopsMax.ToString());

        //Damage Bonuses
        GameManager.instance.UIContent.image = DamageIcon;
        GameManager.instance.UIContent.text = (ship.GetShipData().GetDamageBonus() * 100f).ToString("0.#") + "%";
        GUI.Label(DamageBonusRect, GameManager.instance.UIContent, GameManager.instance.standardLabelStyle);
        GameManager.instance.UIContent.image = DefenseIcon;
        GameManager.instance.UIContent.text = (ship.GetShipData().GetDefenseBonus() * 100f).ToString("0.#") + "%";
        GUI.Label(DefenseBonusRect, GameManager.instance.UIContent, GameManager.instance.standardLabelStyle);
        GameManager.instance.UIContent.image = null;
        GameManager.instance.UIContent.text = "";
        #endregion

        #region ShipOrders
        //Orders
        GUI.Box(OrderDisplayRect, "");

        if (!control)
        {
            GUI.enabled = false;
        }
        /*******************************************************************************************************************/
        cancelOrderButton.Draw(ship);
        attackDirectionButton.Draw(ship);
        attackStyleButton.Draw(ship);
        fireControlButton.Draw(ship);
        fighterLaunchButton.Draw(ship);
        heavyFighterLaunchButton.Draw(ship);
        assaultPodsLaunchButton.Draw(ship);
        cloakingButton.Draw(ship);
        transportTroopsButton.Draw(ship);
        transportCrewButton.Draw(ship);
        retreatButton.Draw(ship);
        selfDestructButton.Draw(ship);      
        /*******************************************************************************************************************/

        GUI.Label(RangeRect, "Range: " + ship.GetShipData().GetAttackRangeDisplay().ToString("0"));

        GUI.Box(RangeSliderRect, "");

        //Draw Damage bars
        for (int i = 0; i < 20; i++)
        {
            GUI.DrawTexture(DamageBars[i], DamageBarTexture);
        }
        ship.GetShipData().SetAttackRange(GUI.HorizontalSlider(RangeSliderRect, ship.GetShipData().GetAttackRange(), 0f, ship.GetShipData().designData.maxRange));
        GUI.enabled = true;
        #endregion

        #region Tooltips
        //Tooltips
        if (RangeAreaRect.Contains(mousePosition))
        {
            ToolTip.SetText("attackRange", "attackRangeDesc");
            ship.AttachRangeCircle();
        }
        else
        {
            ship.RemoveRangeCircle();
            if (DamageDisplayRect.Contains(mousePosition))
            {
                ToolTip.SetText("damageOverlay", "damageOverlayDesc");
            }
            else if (InfoDisplayRect.Contains(mousePosition))
            {
                if(NameRect.Contains(mousePosition))
                {
                    ToolTip.SetText("shipName", "shipNameDesc");
                }
                else if(DesignRect.Contains(mousePosition))
                {
                    ToolTip.SetText("shipDesign", "shipDesignDesc");
                }
                else if(HullRect.Contains(mousePosition))
                {
                    ShipType classification = ship.GetShipData().designData.Hull.Classification;

                    if (classification == ShipType.Corvette)
                    {
                        ToolTip.SetText("corvetteClass", "corvetteClassDesc");
                    }
                    else if (classification == ShipType.Destroyer)
                    {
                        ToolTip.SetText("destroyerClass", "destroyerClassDesc");
                    }
                    else if (classification == ShipType.Frigate)
                    {
                        ToolTip.SetText("frigateClass", "frigateClassDesc");
                    }
                    else if (classification == ShipType.Cruiser)
                    {
                        ToolTip.SetText("cruiserClass", "cruiserClassDesc");
                    }
                    else if (classification == ShipType.Battlecruiser)
                    {
                        ToolTip.SetText("battlecruiserClass", "battlecruiserClassDesc");
                    }
                    else if (classification == ShipType.Battleship)
                    {
                        ToolTip.SetText("battleshipClass", "battleshipClassDesc");
                    }
                    else if (classification == ShipType.Dreadnought)
                    {
                        ToolTip.SetText("dreadnoughtClass", "dreadnoughtClassDesc");
                    }
                    else if (classification == ShipType.Freighter)
                    {
                        ToolTip.SetText("freighterClass", "freighterClassDesc");
                    }
                }
                else if (LevelIconRect.Contains(mousePosition) || LevelBar.Contains(mousePosition))
                {
                    ToolTip.SetText("level", "levelDesc");
                }
                else if (PowerIconRect.Contains(mousePosition) || PowerBar.Contains(mousePosition))
                {
                    ToolTip.SetText("power", "shipPower");
                }
                else if (AmmoIconRect.Contains(mousePosition) || AmmoBar.Contains(mousePosition))
                {
                    ToolTip.SetText("ammo", "shipAmmo");
                }
                else if (CrewIconRect.Contains(mousePosition) || CrewBar.Contains(mousePosition))
                {
                    ToolTip.SetText("crew", "crewDesc");
                }
                else if (TroopIconRect.Contains(mousePosition) || TroopBar.Contains(mousePosition))
                {
                    ToolTip.SetText("troops", "shipTroops");
                }
                else if (DamageBonusRect.Contains(mousePosition))
                {
                    ToolTip.SetText("damageBonus", "shipDamageBonus");
                }
                else if(DefenseBonusRect.Contains(mousePosition))
                {
                    ToolTip.SetText("defenseBonus", "shipDefenseBonus");
                }
            }
            else if (OrderDisplayRect.Contains(mousePosition))
            {
                if (cancelOrderButton.CheckToolTip(mousePosition))
                {
                }
                else if (attackDirectionButton.CheckToolTip(mousePosition, ship))
                {
                }
                else if (attackStyleButton.CheckToolTip(mousePosition, ship))
                {
                }
                else if (fireControlButton.CheckToolTip(mousePosition, ship))
                {
                }
                else if (fighterLaunchButton.CheckToolTip(mousePosition, ship))
                {
                }
                else if (heavyFighterLaunchButton.CheckToolTip(mousePosition, ship))
                {
                }
                else if (assaultPodsLaunchButton.CheckToolTip(mousePosition, ship))
                {
                }
                else if (cloakingButton.CheckToolTip(mousePosition, ship))
                {
                }
                else if (transportTroopsButton.CheckToolTip(mousePosition, ship))
                {
                }
                else if (transportCrewButton.CheckToolTip(mousePosition, ship))
                {
                }
                else if (retreatButton.CheckToolTip(mousePosition, ship))
                {
                }
                else if (selfDestructButton.CheckToolTip(mousePosition, ship))
                {
                }
            }
        }
        #endregion
    }

    public void DrawStationInfo(Station station, bool control, Vector2 mousePosition)
    {
        if (station == null)
            return;
        GUI.Box(baseRect, "");

        #region DamageOverlay
        //Regional damage display
        GUI.Box(DamageDisplayRect, "");

        if (station.stationData.shieldHealthMax > 0)
        {
            GUI.color = new Color(1, 1, 1, station.stationData.shieldHealth / station.stationData.shieldHealthMax);
            GUI.DrawTexture(DamageDisplayRect, Shields);
            GUI.color = Color.white;
        }

        if (station.stationData.armorHealthMax > 0)
        {
            float armor = station.stationData.armorHealth;
            if (armor == 0)
            {
                GUI.color = Color.black;
            }
            else
            {
                float damageRatio = armor / station.stationData.armorHealthMax;
                GUI.color = new Color(1, damageRatio, damageRatio);
            }
            GUI.DrawTexture(DamageDisplayRect, Armor);
            GUI.color = Color.white;
        }
        float health = station.stationData.health;
        if (health == 0)
        {
            GUI.color = Color.black;
        }
        else
        {
            float damageRatio = health / station.stationData.armorHealthMax;
            GUI.color = new Color(1, damageRatio, damageRatio);
        }
        GUI.DrawTexture(DamageDisplayRect, Health);
        GUI.color = Color.white;
        #endregion

        #region Station Info
        //Info
        GUI.Box(InfoDisplayRect, "");
        station.stationData.DisplayName = GUI.TextField(NameRect, station.stationData.DisplayName, GameManager.instance.standardLabelStyle);
        //GUI.Label(NameRect, ship.GetShipData().DisplayName, Style);
        string design = "Design: " + station.stationData.designData.Design.Name;
        GUI.Label(DesignRect, design, GameManager.instance.standardLabelStyle);
        string hull = station.stationData.designData.Design.Hull + " - " + station.stationData.designData.Design.GetHull().Classification;
        GUI.Label(HullRect, hull, GameManager.instance.standardLabelStyle);

        GUI.DrawTexture(PowerIconRect, PowerIcon);
        float ratio = station.stationData.power / station.stationData.powerMax;
        PowerBar.Draw(ratio, station.stationData.power.ToString("0.#") + "/" + station.stationData.powerMax.ToString("0.#"));

        GUI.DrawTexture(AmmoIconRect, AmmoIcon);
        ratio = station.stationData.ammo / station.stationData.ammoMax;
        AmmoBar.Draw(ratio, station.stationData.ammo.ToString("0.#") + "/" + station.stationData.ammoMax.ToString("0.#"));

        GUI.DrawTexture(CrewIconRect, CrewIcon);
        CrewBar.Draw(station.stationData.GetCrewRatio(), station.stationData.crew.ToString() + "/" + station.stationData.crewMax.ToString());

        GUI.DrawTexture(TroopIconRect, TroopIcon);
        TroopBar.Draw(station.stationData.GetTroopRatio(), station.stationData.troops.ToString() + "/" + station.stationData.troopsMax.ToString());

        //Damage Bonuses
        GameManager.instance.UIContent.image = DamageIcon;
        GameManager.instance.UIContent.text = (station.stationData.GetDamageBonus() * 100f).ToString("0.#") + "%";
        GUI.Label(DamageBonusRect, GameManager.instance.UIContent, GameManager.instance.standardLabelStyle);
        GameManager.instance.UIContent.image = DefenseIcon;
        GameManager.instance.UIContent.text = (station.stationData.GetDefenseBonus() * 100f).ToString("0.#") + "%";
        GUI.Label(DefenseBonusRect, GameManager.instance.UIContent, GameManager.instance.standardLabelStyle);
        GameManager.instance.UIContent.image = null;
        GameManager.instance.UIContent.text = "";
        
        #endregion
        
        #region Station Orders
        //Orders
        GUI.Box(OrderDisplayRect, "");

        if (!control)
        {
            GUI.enabled = false;
        }
        /*******************************************************************************************************************/
        cancelOrderButton.Draw(station);
        fireControlButton.Draw(station);
        fighterLaunchButton.Draw(station);
        heavyFighterLaunchButton.Draw(station);
        assaultPodsLaunchButton.Draw(station);
        transportTroopsButton.Draw(station);
        transportCrewButton.Draw(station);
        selfDestructButton.Draw(station);
        /*******************************************************************************************************************/
        GUI.enabled = true;
        #endregion

        #region Tooltips
        //Tooltips
        if (DamageDisplayRect.Contains(mousePosition))
        {
            ToolTip.SetText("damageOverlay", "damageOverlayDesc");
        }
        else if (InfoDisplayRect.Contains(mousePosition))
        {
            if (NameRect.Contains(mousePosition))
            {
                ToolTip.SetText("shipName", "shipNameDesc");
            }
            else if (DesignRect.Contains(mousePosition))
            {
                ToolTip.SetText("shipDesign", "shipDesignDesc");
            }
            else if (HullRect.Contains(mousePosition))
            {
                ToolTip.SetText("hullClass", "hullClassDesc");
            }
            else if (PowerIconRect.Contains(mousePosition) || PowerBar.Contains(mousePosition))
            {
                ToolTip.SetText("power", "shipPower");
            }
            else if (AmmoIconRect.Contains(mousePosition) || AmmoBar.Contains(mousePosition))
            {
                ToolTip.SetText("ammo", "shipAmmo");
            }
            else if (CrewIconRect.Contains(mousePosition) || CrewBar.Contains(mousePosition))
            {
                ToolTip.SetText("crew", "crewDesc");
            }
            else if (TroopIconRect.Contains(mousePosition) || TroopBar.Contains(mousePosition))
            {
                ToolTip.SetText("troops", "shipTroops");
            }
            else if (DamageBonusRect.Contains(mousePosition))
            {
                ToolTip.SetText("damageBonus", "shipDamageBonus");
            }
            else if (DefenseBonusRect.Contains(mousePosition))
            {
                ToolTip.SetText("defenseBonus", "shipDefenseBonus");
            }
        }
        else if (OrderDisplayRect.Contains(mousePosition))
        {
            if (cancelOrderButton.CheckToolTip(mousePosition))
            {
            }
            if (fireControlButton.CheckToolTip(mousePosition, station))
            {
            }
            else if (fighterLaunchButton.CheckToolTip(mousePosition, station))
            {
            }
            else if (heavyFighterLaunchButton.CheckToolTip(mousePosition, station))
            {
            }
            else if (assaultPodsLaunchButton.CheckToolTip(mousePosition, station))
            {
            }
            else if (transportTroopsButton.CheckToolTip(mousePosition, station))
            {
            }
            else if (transportCrewButton.CheckToolTip(mousePosition, station))
            {
            }
            else if (selfDestructButton.CheckToolTip(mousePosition, station))
            {
            }
        }
        #endregion
    }

    public void DrawFighterInfo(FighterWing fighterWing, bool control, Vector2 mousePosition)
    {
        GUI.Box(baseRect, "");

        GUI.Box(DamageDisplayRect, "");
        if (fighterWing.HasShields())
        {
            GUI.color = new Color(1, 1, 1, fighterWing.GetShieldRatio());
            GUI.DrawTexture(DamageDisplayRect, Shields);
            GUI.color = Color.white;
        }

        float damageRatio = fighterWing.GetArmorRatio();
        GUI.color = new Color(damageRatio, damageRatio, damageRatio);
        GUI.DrawTexture(DamageDisplayRect, Armor);
        GUI.color = Color.white; 

        damageRatio = fighterWing.GetHealthRatio();
        GUI.color = new Color(1, damageRatio, damageRatio);
        GUI.DrawTexture(DamageDisplayRect, Health);
        GUI.color = Color.white;

        GUI.Box(InfoDisplayRect, "");
        string name = "Name: " + fighterWing.GetFirstFighter().GetName();
        GUI.Label(NameRect, name, GameManager.instance.standardLabelStyle);
        string fighterCount = "Count: " + fighterWing.GetUnitCount().ToString() + "/" + fighterWing.GetUnitCapacity();
        GUI.Label(DesignRect, fighterCount, GameManager.instance.standardLabelStyle);

        GUI.Box(OrderDisplayRect, "");

        if (!control)
        {
            GUI.enabled = false;
        }
        cancelOrderButton.Draw(fighterWing);
        GUI.enabled = true;


        GUI.Box(RangeSliderRect, "Range Slider");

        if (OrderDisplayRect.Contains(mousePosition))
        {
            cancelOrderButton.CheckToolTip(mousePosition);
        }
    }

    public void DrawGroupInfo(List<Ship> ships, List<Station> stations, List<FighterWing> fighters, Vector2 mousePosition)
    {
        #region StateCheck
        bool hasFighters = false;
        bool hasHeavyFighters = false;
        bool hasAssaultPods = false;
        bool hasLaunchedFighters = false;
        bool hasLaunchedHeavyFighters = false;
        bool hasLaunchedAssaultPods = false;
        bool faceForward = false;
        bool faceRight = false;
        bool holdPosition = false;
        bool strafeRun = false;
        bool holdFire = false;
        bool isCloaked = false;
        bool canCloak = false;
        bool transportingCrew = false;
        bool canTransportCrew = false;
        bool transportingTroops = false;
        bool canTransportTroops = false;
        bool Retreating = false;
        bool selfDesctructing = false;

        foreach (Ship ship in ships)
        {
            if (!hasFighters && ship.HasFighters())
            {
                hasFighters = true;
            }
            else if (!hasLaunchedFighters && ship.HasLaunchedFighters())
            {
                hasLaunchedFighters = true;
            }
            if (!hasHeavyFighters && ship.HasHeavyFighters())
            {
                hasHeavyFighters = true;
            }
            else if (!hasLaunchedHeavyFighters && ship.HasLaunchedHeavyFighters())
            {
                hasLaunchedHeavyFighters = true;
            }
            if (!hasAssaultPods && ship.HasAssaultPods())
            {
                hasAssaultPods = true;
            }
            else if (!hasLaunchedAssaultPods && ship.HasLaunchedAssaultPods())
            {
                hasLaunchedAssaultPods = true;
            }
            if (!faceForward && ship.GetShipData().AttackDirection == QuadrantTypes.Fore)
            {
                faceForward = true;
            }
            else if(!faceRight && ship.GetShipData().AttackDirection == QuadrantTypes.Starboard)
            {
                faceRight = true;
            }
            if(!holdPosition && ship.GetShipData().AttackStyle == AttackStyle.holdPosition)
            {
                holdPosition = true;
            }
            else if(!strafeRun && ship.GetShipData().AttackStyle == AttackStyle.strafingRuns)
            {
                strafeRun = true;
            }
            if(!isCloaked && ship.inCloakingState())
            {
                isCloaked = true;
            }
            else if(!canCloak && ship.CanCloak())
            {
                canCloak = true;
            }
            if(!transportingCrew && ship.TransportingCrew())
            {
                transportingCrew = true;
            }
            else if(!canTransportCrew && ship.CanTransportCrew())
            {
                canTransportCrew = true;
            }
            if(!transportingTroops && ship.TransportingTroops())
            {
                transportingTroops = true;
            }
            else if(!canTransportTroops && ship.CanTransportTroops())
            {
                canTransportTroops = true;
            }
            if(!Retreating && ship.RetreatTimerRunning())
            {
                Retreating = true;
            }
            if(!selfDesctructing && ship.SelfDestructTimerRunning())
            {
                selfDesctructing = true;
            }
            if(!holdFire && ship.HoldFire)
            {
                holdFire = true;
            }
        }
        foreach(Station station in stations)
        {
            if (!hasFighters && station.HasFighters())
            {
                hasFighters = true;
            }
            else if (!hasLaunchedFighters && station.HasLaunchedFighters())
            {
                hasLaunchedFighters = true;
            }
            if (!hasHeavyFighters && station.HasHeavyFighters())
            {
                hasHeavyFighters = true;
            }
            else if (!hasLaunchedHeavyFighters && station.HasLaunchedHeavyFighters())
            {
                hasLaunchedHeavyFighters = true;
            }
            if (!hasAssaultPods && station.HasAssaultPods())
            {
                hasAssaultPods = true;
            }
            else if (!hasLaunchedAssaultPods && station.HasLaunchedAssaultPods())
            {
                hasLaunchedAssaultPods = true;
            }
            if (!transportingCrew && station.TransportingCrew())
            {
                transportingCrew = true;
            }
            else if (!canTransportCrew && station.CanTransportCrew())
            {
                canTransportCrew = true;
            }
            if (!transportingTroops && station.TransportingTroops())
            {
                transportingTroops = true;
            }
            else if (!canTransportTroops && station.CanTransportTroops())
            {
                canTransportTroops = true;
            }
            if (!selfDesctructing && station.SelfDestructTimerRunning())
            {
                selfDesctructing = true;
            }
            if (!holdFire && station.HoldFire)
            {
                holdFire = true;
            }
        }

        #endregion

        GUI.Box(baseRect, "");
        #region Groups Window
        //Auto size view rect
        GUI.Box(GroupShipWindowRect, "");
        GroupShipViewRect.height = Mathf.Max(((ships.Count + fighters.Count) / 8 + 1) * shipButtonSize, GroupShipWindowRect.height * 1.05f);
        GroupShipPosition = GUI.BeginScrollView(GroupShipWindowRect, GroupShipPosition, GroupShipViewRect);
        int buttonNum = 0;
        foreach (Ship ship in ships)
        {/*
            Rect rect = new Rect((buttonNum % 8) * shipButtonSize, (buttonNum / 8) * shipButtonSize, shipButtonSize, shipButtonSize);
            if (GUI.Button(rect, ship.GetShipData().designData.Hull.GetIcon()))
            {
                shipManager.DeselectShips();
                shipManager.AddToSelection(ship);
                return;
            }
            buttonNum++;*/
        }
        foreach (Station station in stations)
        {/*
            Rect rect = new Rect((buttonNum % 8) * shipButtonSize, (buttonNum / 8) * shipButtonSize, shipButtonSize, shipButtonSize);
            if (GUI.Button(rect, station.stationData.designData.Hull.GetIcon()))
            {
                shipManager.DeselectShips();
                shipManager.AddToSelection(station);
                return;
            }
            buttonNum++;*/
        }
        foreach (FighterWing fighterWing in fighters)
        {/*
            Rect rect = new Rect((buttonNum % 8) * shipButtonSize, (buttonNum / 8) * shipButtonSize, shipButtonSize, shipButtonSize);
            if (GUI.Button(rect, fighterWing.GetIcon()))
            {
                shipManager.DeselectShips();
                shipManager.AddToSelection(fighterWing);
                return;
            }
            buttonNum++;*/
        }
        GUI.EndScrollView();
        #endregion

        #region Orders
        //Orders
        GUI.Box(OrderDisplayRect, "");

        cancelOrderButton.Draw(ships, stations, fighters);
        attackDirectionButton.Draw(ships, faceForward, faceRight);
        attackStyleButton.Draw(ships, holdPosition, strafeRun);
        fireControlButton.Draw(ships, stations, holdFire);
        fighterLaunchButton.Draw(ships, stations, hasFighters, hasLaunchedFighters);
        heavyFighterLaunchButton.Draw(ships, stations, hasHeavyFighters, hasLaunchedHeavyFighters);
        assaultPodsLaunchButton.Draw(ships, stations, hasAssaultPods, hasLaunchedAssaultPods);
        cloakingButton.Draw(ships, isCloaked, canCloak);
        transportTroopsButton.Draw(ships, stations, transportingTroops, canTransportTroops);
        transportCrewButton.Draw(ships, stations, transportingCrew, canTransportCrew);
        retreatButton.Draw(ships, Retreating);
        selfDestructButton.Draw(ships, stations, selfDesctructing);
        #endregion

        GUI.enabled = true;

        #region Tooltips
        //Tooltips

        if (OrderDisplayRect.Contains(mousePosition))
        {
            if (cancelOrderButton.CheckToolTip(mousePosition))
            {
            }
            else if (attackDirectionButton.CheckToolTip(mousePosition, faceForward, faceRight))
            {
            }
            else if (attackStyleButton.CheckToolTip(mousePosition, holdPosition, strafeRun))
            {
            }
            else if (fireControlButton.CheckToolTip(mousePosition, holdFire))
            {
            }
            else if (fighterLaunchButton.CheckToolTip(mousePosition, hasFighters, hasLaunchedFighters))
            {
            }
            else if (heavyFighterLaunchButton.CheckToolTip(mousePosition, hasHeavyFighters, hasLaunchedHeavyFighters))
            {
            }
            else if (assaultPodsLaunchButton.CheckToolTip(mousePosition, hasAssaultPods, hasLaunchedAssaultPods))
            {
            }
            else if (cloakingButton.CheckToolTip(mousePosition, isCloaked, canCloak))
            {
            }
            else if (transportTroopsButton.CheckToolTip(mousePosition, canTransportTroops, transportingTroops))
            {
            }
            else if (transportCrewButton.CheckToolTip(mousePosition, canTransportCrew, transportingCrew))
            {
            }
            else if (retreatButton.CheckToolTip(mousePosition, Retreating))
            {
            }
            else if (selfDestructButton.CheckToolTip(mousePosition, selfDesctructing))
            {
            }
        }
        #endregion
    }

    public bool ContainsMouse(Vector2 mousePosition)
    {
        return baseRect.Contains(mousePosition);
    }

    void PlayMainButtonClick()
    {
        AudioManager.instance.PlayUIClip("MainButtonClick");
    }

    void BuildDamageBars(Ship ship)
    {
        //Scale Damage bars
        for (int i = 0; i < 20; i++)
        {
            DamageBars[i].height = RangeSliderRect.height * ship.GetShipData().designData.DamageGraph[i];
            DamageBars[i].y = RangeSliderRect.y + (RangeSliderRect.height - DamageBars[i].height) / 2f;
        }
    }

    public abstract class PanelButton
    {
        protected ShipManager shipManager;
        protected Rect baseRect;
        protected Texture2D[] textures;
        public delegate void PlayButtonClick();
        protected PlayButtonClick buttonClick;
        protected GUIToolTip ToolTip;
        protected bool GUIControl;

        public bool Contains(Vector2 point)
        {
            return baseRect.Contains(point);
        }

        public void SetShipManager(ShipManager shipmanager)
        {
            shipManager = shipmanager;
        }
    }

    public sealed class CancelOrdersButton : PanelButton
    {
        public CancelOrdersButton(Rect rect, Texture2D icon, PlayButtonClick playButtonClick, GUIToolTip toolTip)
        {
            baseRect = rect;
            textures = new Texture2D[1];
            textures[0] = icon;
            ToolTip = toolTip;
            buttonClick = playButtonClick;
        }

        public void Draw(Ship ship)
        {
            if(GUI.Button(baseRect, textures[0]))
            {
                ship.CancelOrders();
                buttonClick();
            }
        }

        public void Draw(Station station)
        {
            if (GUI.Button(baseRect, textures[0]))
            {
                station.CancelOrders();
                buttonClick();
            }
        }

        public void Draw(FighterWing fighterWing)
        {
            if (GUI.Button(baseRect, textures[0]))
            {
                fighterWing.CancelOrders();
                buttonClick();
            }
        }

        public void Draw(List<Ship> ships, List<Station> stations, List<FighterWing> fighters)
        {
            if (GUI.Button(baseRect, textures[0]))
            {
                foreach(Ship ship in ships)
                {
                    ship.CancelOrders();
                }
                foreach (Station station in stations)
                {
                    station.CancelOrders();
                }
                foreach (FighterWing fighterWing in fighters)
                {
                    fighterWing.CancelOrders();
                }
                buttonClick();
            }
        }

        public bool CheckToolTip(Vector2 mousePosition)
        {
            if(Contains(mousePosition))
            {
                ToolTip.SetText("cancelOrders", "cancelOrdersDesc");
                return true;
            }
            return false;
        }
    }

    public sealed class AttackDirectionButton : PanelButton
    {
        public AttackDirectionButton(Rect rect, Texture2D iconAttackRight, Texture2D iconAttackLeft, Texture2D iconAttackFront, PlayButtonClick playButtonClick, GUIToolTip toolTip)
        {
            baseRect = rect;
            textures = new Texture2D[3];
            textures[0] = iconAttackRight;
            textures[1] = iconAttackLeft;
            textures[2] = iconAttackFront;
            ToolTip = toolTip;
            buttonClick = playButtonClick;
        }

        public void Draw(Ship ship)
        {
            if (ship.GetShipData().AttackDirection == QuadrantTypes.Starboard)
            {
                if (GUI.Button(baseRect, textures[0]))
                {
                    ship.GetShipData().AttackDirection = QuadrantTypes.Port;
                    buttonClick();
                }
            }
            else if (ship.GetShipData().AttackDirection == QuadrantTypes.Port)
            {
                if (GUI.Button(baseRect, textures[1]))
                {
                    ship.GetShipData().AttackDirection = QuadrantTypes.Fore;
                    buttonClick();
                }
            }
            else if (ship.GetShipData().AttackDirection == QuadrantTypes.Fore)
            {
                if (GUI.Button(baseRect, textures[2]))
                {
                    ship.GetShipData().AttackDirection = QuadrantTypes.Starboard;
                    buttonClick();
                }
            }
        }

        public void Draw(List<Ship> ships, bool faceForward, bool faceRight)
        {
            if (faceForward)
            {
                if (GUI.Button(baseRect, textures[2]))
                {
                    foreach (Ship ship in ships)
                    {
                        ship.GetShipData().AttackDirection = QuadrantTypes.Starboard;
                    }
                    buttonClick();
                }
            }
            else if (faceRight)
            {
                if (GUI.Button(baseRect, textures[0]))
                {
                    foreach (Ship ship in ships)
                    {
                        ship.GetShipData().AttackDirection = QuadrantTypes.Port;
                    }
                    buttonClick();
                }
            }
            else
            {
                if (GUI.Button(baseRect, textures[1]))
                {
                    foreach (Ship ship in ships)
                    {
                        ship.GetShipData().AttackDirection = QuadrantTypes.Fore;
                    }
                    buttonClick();
                }
            }
        }

        public bool CheckToolTip(Vector2 mousePosition, Ship ship)
        {
            if (Contains(mousePosition))
            {
                if (ship.GetShipData().AttackDirection == QuadrantTypes.Starboard)
                {
                    ToolTip.SetText("attackRight", "attackRightDesc");
                }
                else if (ship.GetShipData().AttackDirection == QuadrantTypes.Port)
                {
                    ToolTip.SetText("attackLeft", "attackLeftDesc");
                }
                else
                {
                    ToolTip.SetText("attackFront", "attackFrontDesc");
                }
                return true;
            }
            return false;
        }

        public bool CheckToolTip(Vector2 mousePosition, bool faceForward, bool faceRight)
        {
            if (Contains(mousePosition))
            {
                if (faceForward)
                {
                    ToolTip.SetText("attackFront", "attackFrontDesc");
                }
                else if (faceRight)
                {
                    ToolTip.SetText("attackRight", "attackRightDesc");
                }
                else
                {
                    ToolTip.SetText("attackLeft", "attackLeftDesc");
                }
                return true;
            }
            return false;
        }
    }

    public sealed class AttackStyleButton : PanelButton
    {
        public AttackStyleButton(Rect rect, Texture2D iconStrafe, Texture2D iconHold, Texture2D iconDontAttack, PlayButtonClick playButtonClick, GUIToolTip toolTip)
        {
            baseRect = rect;
            textures = new Texture2D[3];
            textures[0] = iconStrafe;
            textures[1] = iconHold;
            textures[2] = iconDontAttack;
            ToolTip = toolTip;
            buttonClick = playButtonClick;
        }

        public void Draw(Ship ship)
        {
            if (ship.GetShipData().AttackStyle == AttackStyle.strafingRuns)
            {
                if (GUI.Button(baseRect, textures[0]))
                {
                    ship.GetShipData().AttackStyle = AttackStyle.none;
                    buttonClick();
                }
            }
            else if (ship.GetShipData().AttackStyle == AttackStyle.holdPosition)
            {
                if (GUI.Button(baseRect, textures[1]))
                {
                    ship.GetShipData().AttackStyle = AttackStyle.strafingRuns;
                    buttonClick();
                }
            }
            else
            {
                if (GUI.Button(baseRect, textures[2]))
                {
                    ship.GetShipData().AttackStyle = AttackStyle.holdPosition;
                    buttonClick();
                }
            }
        }

        public void Draw(List<Ship> ships, bool holdPosition, bool strafeRun)
        {
            if (holdPosition)
            {
                if (GUI.Button(baseRect, textures[1]))
                {
                    foreach (Ship ship in ships)
                    {
                        ship.GetShipData().AttackStyle = AttackStyle.strafingRuns;
                    }
                    buttonClick();
                }
            }
            else if (strafeRun)
            {
                if (GUI.Button(baseRect, textures[0]))
                {
                    foreach (Ship ship in ships)
                    {
                        ship.GetShipData().AttackStyle = AttackStyle.none;
                    }
                    buttonClick();
                }
            }
            else
            {
                if (GUI.Button(baseRect, textures[2]))
                {
                    foreach (Ship ship in ships)
                    {
                        ship.GetShipData().AttackStyle = AttackStyle.holdPosition;
                    }
                    buttonClick();
                }
            }
        }

        public bool CheckToolTip(Vector2 mousePosition, Ship ship)
        {
            if (Contains(mousePosition))
            {
                if (ship.GetShipData().AttackStyle == AttackStyle.strafingRuns)
                {
                    ToolTip.SetText("strafeAttack", "strafeAttackDesc");
                }
                else if (ship.GetShipData().AttackStyle == AttackStyle.holdPosition)
                {
                    ToolTip.SetText("holdPosition", "holdPositionDesc");
                }
                else
                {
                    ToolTip.SetText("dontAttack", "dontAttackDesc");
                }
                return true;
            }
            return false;
        }

        public bool CheckToolTip(Vector2 mousePosition, bool holdPosition, bool strafeRun)
        {
            if (Contains(mousePosition))
            {
                if (strafeRun)
                {
                    ToolTip.SetText("strafeAttack", "strafeAttackDesc");
                }
                else if (holdPosition)
                {
                    ToolTip.SetText("holdPosition", "holdPositionDesc");
                }
                else
                {
                    ToolTip.SetText("dontAttack", "dontAttackDesc");
                }
                return true;
            }
            return false;
        }
    }

    public sealed class FireControlButton : PanelButton
    {
        public FireControlButton(Rect rect, Texture2D iconHoldFire, Texture2D iconFireAtWill, PlayButtonClick playButtonClick, GUIToolTip toolTip)
        {
            baseRect = rect;
            textures = new Texture2D[2];
            textures[0] = iconHoldFire;
            textures[1] = iconFireAtWill;
            ToolTip = toolTip;
            buttonClick = playButtonClick;
        }

        public void Draw(Ship ship)
        {
            if (ship.HoldFire)
            {
                if (GUI.Button(baseRect, textures[0]))
                {
                    ship.HoldFire = false;
                    buttonClick();
                }
            }
            else
            {
                if (GUI.Button(baseRect, textures[1]))
                {
                    ship.HoldFire = true;
                    buttonClick();
                }
            }
        }

        public void Draw(Station station)
        {
            if (station.HoldFire)
            {
                if (GUI.Button(baseRect, textures[0]))
                {
                    station.HoldFire = false;
                    buttonClick();
                }
            }
            else
            {
                if (GUI.Button(baseRect, textures[1]))
                {
                    station.HoldFire = true;
                    buttonClick();
                }
            }
        }

        public void Draw(List<Ship> ships, List<Station> stations, bool holdFire)
        {
            if (holdFire)
            {
                if (GUI.Button(baseRect, textures[0]))
                {
                    foreach (Ship ship in ships)
                    {
                        ship.HoldFire = false;
                    }
                    foreach (Station station in stations)
                    {
                        station.HoldFire = false;
                    }
                    buttonClick();
                }
            }
            else
            {
                if (GUI.Button(baseRect, textures[1]))
                {
                    foreach (Ship ship in ships)
                    {
                        ship.HoldFire = true;
                    }
                    foreach (Station station in stations)
                    {
                        station.HoldFire = true;
                    }
                    buttonClick();
                }
            }
        }

        public bool CheckToolTip(Vector2 mousePosition, Ship ship)
        {
            if (Contains(mousePosition))
            {
                if (ship.HoldFire)
                {
                    ToolTip.SetText("holdFire", "holdFireDesc");
                }
                else
                {
                    ToolTip.SetText("fireAtWill", "fireAtWillDesc");
                }
                return true;
            }
            return false;
        }

        public bool CheckToolTip(Vector2 mousePosition, Station station)
        {
            if (Contains(mousePosition))
            {
                if (station.HoldFire)
                {
                    ToolTip.SetText("holdFire", "holdFireDesc");
                }
                else
                {
                    ToolTip.SetText("fireAtWill", "fireAtWillDesc");
                }
                return true;
            }
            return false;
        }

        public bool CheckToolTip(Vector2 mousePosition, bool holdFire)
        {
            if (Contains(mousePosition))
            {
                if (holdFire)
                {
                    ToolTip.SetText("holdFire", "holdFireDesc");
                }
                else
                {
                    ToolTip.SetText("fireAtWill", "fireAtWillDesc");
                }
                return true;
            }
            return false;
        }
    }

    public sealed class FighterLaunchButton : PanelButton
    {
        public FighterLaunchButton(Rect rect, Texture2D iconLaunchFighter, Texture2D iconRecallFighters, PlayButtonClick playButtonClick, GUIToolTip toolTip)
        {
            baseRect = rect;
            textures = new Texture2D[2];
            textures[0] = iconLaunchFighter;
            textures[1] = iconRecallFighters;
            ToolTip = toolTip;
            buttonClick = playButtonClick;
        }

        public void Draw(Ship ship)
        {
            if (ship.HasFighters())
            {
                if (GUI.Button(baseRect, textures[0]))
                {
                    ship.LaunchFighters();
                    buttonClick();
                }
            }
            else if (ship.HasLaunchedFighters())
            {
                if (GUI.Button(baseRect, textures[1]))
                {
                    ship.RecallFighters();
                    buttonClick();
                }
            }
            else
            {
                GUIControl = GUI.enabled;
                GUI.enabled = false;
                if (GUI.Button(baseRect, textures[0]))
                {

                }
                GUI.enabled = GUIControl;
            }
        }

        public void Draw(Station station)
        {
            if (station.HasFighters())
            {
                if (GUI.Button(baseRect, textures[0]))
                {
                    station.LaunchFighters();
                    buttonClick();
                }
            }
            else if (station.HasLaunchedFighters())
            {
                if (GUI.Button(baseRect, textures[1]))
                {
                    station.RecallFighters();
                    buttonClick();
                }
            }
            else
            {
                GUIControl = GUI.enabled;
                GUI.enabled = false;
                if (GUI.Button(baseRect, textures[0]))
                {

                }
                GUI.enabled = GUIControl;
            }
        }

        public void Draw(List<Ship> ships, List<Station> stations, bool hasFighters, bool hasLaunchedFighters)
        {
            if (hasFighters)
            {
                if (GUI.Button(baseRect, textures[0]))
                {
                    foreach (Ship ship in ships)
                    {
                        ship.LaunchFighters();
                    }
                    foreach (Station station in stations)
                    {
                        station.LaunchFighters();
                    }
                    buttonClick();
                }
            }
            else if (hasLaunchedFighters)
            {
                if (GUI.Button(baseRect, textures[1]))
                {
                    foreach (Ship ship in ships)
                    {
                        ship.RecallFighters();
                    }
                    foreach (Station station in stations)
                    {
                        station.RecallFighters();
                    }
                    buttonClick();
                }
            }
            else
            {
                GUIControl = GUI.enabled;
                GUI.enabled = false;
                if (GUI.Button(baseRect, textures[0]))
                {

                }
                GUI.enabled = GUIControl;
            }
        }

        public bool CheckToolTip(Vector2 mousePosition, Ship ship)
        {
            if (Contains(mousePosition))
            {
                if (ship.HasFighters())
                {
                    ToolTip.SetText("launchFighters", "launchFightersDesc");
                }
                else if (ship.HasLaunchedFighters())
                {
                    ToolTip.SetText("recallFighters", "recallFightersDesc");
                }
                else
                {
                    ToolTip.SetText("noFighters", "noFightersDesc");
                }
                return true;
            }
            return false;
        }

        public bool CheckToolTip(Vector2 mousePosition, Station station)
        {
            if (Contains(mousePosition))
            {
                if (station.HasFighters())
                {
                    ToolTip.SetText("launchFighters", "launchFightersDesc");
                }
                else if (station.HasLaunchedFighters())
                {
                    ToolTip.SetText("recallFighters", "recallFightersDesc");
                }
                else
                {
                    ToolTip.SetText("noFighters", "noFightersDesc");
                }
                return true;
            }
            return false;
        }

        public bool CheckToolTip(Vector2 mousePosition, bool hasFighters, bool hasLaunchedFighters)
        {
            if (Contains(mousePosition))
            {
                if (hasFighters)
                {
                    ToolTip.SetText("launchFighters", "launchFightersDesc");
                }
                else if (hasLaunchedFighters)
                {
                    ToolTip.SetText("recallFighters", "recallFightersDesc");
                }
                else
                {
                    ToolTip.SetText("noFighters", "noFightersDesc");
                }
                return true;
            }
            return false;
        }
    }

    public sealed class HeavyFighterLaunchButton : PanelButton
    {
        public HeavyFighterLaunchButton(Rect rect, Texture2D iconLaunchHeavyFighters, Texture2D iconRecallHeavyFighters, PlayButtonClick playButtonClick, GUIToolTip toolTip)
        {
            baseRect = rect;
            textures = new Texture2D[2];
            textures[0] = iconLaunchHeavyFighters;
            textures[1] = iconRecallHeavyFighters;
            ToolTip = toolTip;
            buttonClick = playButtonClick;
        }


        public void Draw(Ship ship)
        {
            if (ship.HasHeavyFighters())
            {
                if (GUI.Button(baseRect, textures[0]))
                {
                    ship.LaunchHeavyFighters();
                    buttonClick();
                }
            }
            else if (ship.HasLaunchedHeavyFighters())
            {
                if (GUI.Button(baseRect, textures[1]))
                {
                    ship.RecallHeavyFighters();
                    buttonClick();
                }
            }
            else
            {
                GUIControl = GUI.enabled;
                GUI.enabled = false;
                if (GUI.Button(baseRect, textures[0]))
                {

                }
                GUI.enabled = GUIControl;
            }
        }

        public void Draw(Station station)
        {
            if (station.HasHeavyFighters())
            {
                if (GUI.Button(baseRect, textures[0]))
                {
                    station.LaunchHeavyFighters();
                    buttonClick();
                }
            }
            else if (station.HasLaunchedHeavyFighters())
            {
                if (GUI.Button(baseRect, textures[1]))
                {
                    station.RecallHeavyFighters();
                    buttonClick();
                }
            }
            else
            {
                GUIControl = GUI.enabled;
                GUI.enabled = false;
                if (GUI.Button(baseRect, textures[0]))
                {

                }
                GUI.enabled = GUIControl;
            }
        }

        public void Draw(List<Ship> ships, List<Station> stations, bool hasHeavyFighters, bool hasLaunchedHeavyFighters)
        {
            if (hasHeavyFighters)
            {
                if (GUI.Button(baseRect, textures[0]))
                {
                    foreach (Ship ship in ships)
                    {
                        ship.LaunchHeavyFighters();
                    }
                    foreach (Station station in stations)
                    {
                        station.LaunchHeavyFighters();
                    }
                    buttonClick();
                }
            }
            else if (hasLaunchedHeavyFighters)
            {
                if (GUI.Button(baseRect, textures[1]))
                {
                    foreach (Ship ship in ships)
                    {
                        ship.RecallHeavyFighters();
                    }
                    foreach (Station station in stations)
                    {
                        station.RecallHeavyFighters();
                    }
                    buttonClick();
                }
            }
            else
            {
                GUIControl = GUI.enabled;
                GUI.enabled = false;
                if (GUI.Button(baseRect, textures[0]))
                {

                }
                GUI.enabled = GUIControl;
            }
        }

        public bool CheckToolTip(Vector2 mousePosition, Ship ship)
        {
            if (Contains(mousePosition))
            {
                if (ship.HasHeavyFighters())
                {
                    ToolTip.SetText("launchHeavyFighters", "launchHeavyFightersDesc");
                }
                else if (ship.HasLaunchedHeavyFighters())
                {
                    ToolTip.SetText("recallHeavyFighters", "recallHeavyFightersDesc");
                }
                else
                {
                    ToolTip.SetText("noHeavyFighters", "noHeavyFightersDesc");
                }
                return true;
            }
            return false;
        }

        public bool CheckToolTip(Vector2 mousePosition, Station station)
        {
            if (Contains(mousePosition))
            {
                if (station.HasHeavyFighters())
                {
                    ToolTip.SetText("launchHeavyFighters", "launchHeavyFightersDesc");
                }
                else if (station.HasLaunchedHeavyFighters())
                {
                    ToolTip.SetText("recallHeavyFighters", "recallHeavyFightersDesc");
                }
                else
                {
                    ToolTip.SetText("noHeavyFighters", "noHeavyFightersDesc");
                }
                return true;
            }
            return false;
        }

        public bool CheckToolTip(Vector2 mousePosition, bool hasHeavyFighters, bool hasLaunchedHeavyFighters)
        {
            if (Contains(mousePosition))
            {
                if (hasHeavyFighters)
                {
                    ToolTip.SetText("launchHeavyFighters", "launchHeavyFightersDesc");
                }
                else if (hasLaunchedHeavyFighters)
                {
                    ToolTip.SetText("recallHeavyFighters", "recallHeavyFightersDesc");
                }
                else
                {
                    ToolTip.SetText("noHeavyFighters", "noHeavyFightersDesc");
                }
                return true;
            }
            return false;
        }
    }

    public sealed class AssaultPodsLaunchButton : PanelButton
    {
        public AssaultPodsLaunchButton(Rect rect, Texture2D iconLaunchAssaultPods, Texture2D iconRecallAssaultPods, PlayButtonClick playButtonClick, GUIToolTip toolTip)
        {
            baseRect = rect;
            textures = new Texture2D[2];
            textures[0] = iconLaunchAssaultPods;
            textures[1] = iconRecallAssaultPods;
            ToolTip = toolTip;
            buttonClick = playButtonClick;
        }

        public void Draw(Ship ship)
        {
            if (ship.HasAssaultPods())
            {
                if (GUI.Button(baseRect, textures[0]))
                {
                    ship.LaunchAssaultPods();
                    buttonClick();
                }
            }
            else if (ship.HasLaunchedAssaultPods())
            {
                if (GUI.Button(baseRect, textures[1]))
                {
                    ship.RecallAssaultPods();
                    buttonClick();
                }
            }
            else
            {
                GUIControl = GUI.enabled;
                GUI.enabled = false;
                if (GUI.Button(baseRect, textures[0]))
                {

                }
                GUI.enabled = GUIControl;
            }
        }

        public void Draw(Station station)
        {
            if (station.HasAssaultPods())
            {
                if (GUI.Button(baseRect, textures[0]))
                {
                    station.LaunchAssaultPods();
                    buttonClick();
                }
            }
            else if (station.HasLaunchedAssaultPods())
            {
                if (GUI.Button(baseRect, textures[1]))
                {
                    station.RecallAssaultPods();
                    buttonClick();
                }
            }
            else
            {
                GUIControl = GUI.enabled;
                GUI.enabled = false;
                if (GUI.Button(baseRect, textures[0]))
                {

                }
                GUI.enabled = GUIControl;
            }
        }

        public void Draw(List<Ship> ships, List<Station> stations, bool hasAssaultPods, bool hasLaunchedAssaultPods)
        {
            if (hasAssaultPods)
            {
                if (GUI.Button(baseRect, textures[0]))
                {
                    foreach (Ship ship in ships)
                    {
                        ship.LaunchAssaultPods();
                    }
                    foreach (Station station in stations)
                    {
                        station.LaunchAssaultPods();
                    }
                    buttonClick();
                }
            }
            else if (hasLaunchedAssaultPods)
            {
                if (GUI.Button(baseRect, textures[1]))
                {
                    foreach (Ship ship in ships)
                    {
                        ship.RecallAssaultPods();
                    }
                    foreach (Station station in stations)
                    {
                        station.RecallAssaultPods();
                    }
                    buttonClick();
                }
            }
            else
            {
                GUIControl = GUI.enabled;
                GUI.enabled = false;
                if (GUI.Button(baseRect, textures[0]))
                {

                }
                GUI.enabled = GUIControl;
            }
        }

        public bool CheckToolTip(Vector2 mousePosition, Ship ship)
        {
            if (Contains(mousePosition))
            {
                if (ship.HasAssaultPods())
                {
                    ToolTip.SetText("launchAssaultPods", "launchAssaultPodsDesc");
                }
                else if (ship.HasLaunchedAssaultPods())
                {
                    ToolTip.SetText("recallAssaultPods", "recallAssaultPodsDesc");
                }
                else
                {
                    ToolTip.SetText("noAssaultPods", "noAssaultPodsDesc");
                }
                return true;
            }
            return false;
        }

        //Station tooltip check
        public bool CheckToolTip(Vector2 mousePosition, Station station)
        {
            if (Contains(mousePosition))
            {
                if (station.HasAssaultPods())
                {
                    ToolTip.SetText("launchAssaultPods", "launchAssaultPodsDesc");
                }
                else if (station.HasLaunchedAssaultPods())
                {
                    ToolTip.SetText("recallAssaultPods", "recallAssaultPodsDesc");
                }
                else
                {
                    ToolTip.SetText("noAssaultPods", "noAssaultPodsDesc");
                }
                return true;
            }
            return false;
        }

        //Group tooltip check
        public bool CheckToolTip(Vector2 mousePosition, bool hasAssaultPods, bool hasLaunchedAssaultPods)
        {
            if (Contains(mousePosition))
            {
                if (hasAssaultPods)
                {
                    ToolTip.SetText("launchAssaultPods", "launchAssaultPodsDesc");
                }
                else if (hasLaunchedAssaultPods)
                {
                    ToolTip.SetText("recallAssaultPods", "recallAssaultPodsDesc");
                }
                else
                {
                    ToolTip.SetText("noAssaultPods", "noAssaultPodsDesc");
                }
                return true;
            }
            return false;
        }
    }

    public sealed class TransportTroopsButton : PanelButton
    {
        public TransportTroopsButton(Rect rect, Texture2D iconCancelTransportTroops, Texture2D iconTransportTroops, PlayButtonClick playButtonClick, GUIToolTip toolTip)
        {
            baseRect = rect;
            textures = new Texture2D[2];
            textures[0] = iconCancelTransportTroops;
            textures[1] = iconTransportTroops;
            ToolTip = toolTip;
            buttonClick = playButtonClick;
        }

        public void Draw(Ship ship)
        {
            if (ship.CanTransportTroops())
            {
                ShipManager shipManager = ship.GetShipManager();
                if (shipManager.TransporterTargetingTroop || ship.TransportingTroops())
                {
                    if (shipManager.TransporterTargetingTroop)
                        GUI.color = Color.yellow;
                    if (GUI.Button(baseRect, textures[0]))
                    {
                        if (ship.TransportingTroops())
                            ship.CancelTransportingTroops();
                        shipManager.TransporterTargetingTroop = false;
                        buttonClick();
                    }
                }
                else
                {
                    if (GUI.Button(baseRect, textures[1]))
                    {
                        shipManager.ResetAbilities();
                        shipManager.TransporterTargetingTroop = true;
                        buttonClick();
                    }
                }
                GUI.color = Color.white;
            }
            else
            {
                GUIControl = GUI.enabled;
                GUI.enabled = false;
                if (GUI.Button(baseRect, textures[1]))
                {

                }
                GUI.enabled = GUIControl;
            }
        }

        public void Draw(Station station)
        {
            if (station.CanTransportTroops())
            {
                ShipManager shipManager = station.GetShipManager();
                if (shipManager.TransporterTargetingTroop || station.TransportingTroops())
                {
                    if (shipManager.TransporterTargetingTroop)
                        GUI.color = Color.yellow;
                    if (GUI.Button(baseRect, textures[0]))
                    {
                        if (station.TransportingTroops())
                            station.CancelTransportingTroops();
                        shipManager.TransporterTargetingTroop = false;
                        buttonClick();
                    }
                }
                else
                {
                    if (GUI.Button(baseRect, textures[1]))
                    {
                        shipManager.ResetAbilities();
                        shipManager.TransporterTargetingTroop = true;
                        buttonClick();
                    }
                }
                GUI.color = Color.white;
            }
            else
            {
                GUIControl = GUI.enabled;
                GUI.enabled = false;
                if (GUI.Button(baseRect, textures[1]))
                {

                }
                GUI.enabled = GUIControl;
            }
        }

        public void Draw(List<Ship> ships, List<Station> stations, bool transportingTroops, bool canTransportTroops)
        {
            if (transportingTroops)
            {
                if (GUI.Button(baseRect, textures[0]))
                {
                    foreach (Ship ship in ships)
                    {
                        ship.CancelTransportingTroops();
                    }
                    foreach (Station station in stations)
                    {
                        station.CancelTransportingTroops();
                    }
                    shipManager.TransporterTargetingTroop = false;
                    buttonClick();
                }
            }
            else if (canTransportTroops)
            {
                if (GUI.Button(baseRect, textures[1]))
                {
                    shipManager.ResetAbilities();
                    shipManager.TransporterTargetingTroop = true;
                    buttonClick();
                }
            }
            else
            {
                GUIControl = GUI.enabled;
                GUI.enabled = false;
                if (GUI.Button(baseRect, textures[1]))
                {

                }
                GUI.enabled = GUIControl;
            }
        }

        public bool CheckToolTip(Vector2 mousePosition, Ship ship)
        {
            if (Contains(mousePosition))
            {
                if (ship.CanTransportTroops())
                {
                    if (ship.TransportingTroops())
                    {
                        ToolTip.SetText("cancelTransportingTroops", "cancelTransportingTroopsDesc");
                    }
                    else
                    {
                        ToolTip.SetText("transportTroops", "transportTroopsDesc");
                    }
                }
                else
                {
                    ToolTip.SetText("cannotTransportTroops", "cannotTransportTroopsDesc");
                }
                return true;
            }
            return false;
        }

        public bool CheckToolTip(Vector2 mousePosition, Station station)
        {
            if (Contains(mousePosition))
            {
                if (station.CanTransportTroops())
                {
                    if (station.TransportingTroops())
                    {
                        ToolTip.SetText("cancelTransportingTroops", "cancelTransportingTroopsDesc");
                    }
                    else
                    {
                        ToolTip.SetText("transportTroops", "transportTroopsDesc");
                    }
                }
                else
                {
                    ToolTip.SetText("cannotTransportTroops", "cannotTransportTroopsDesc");
                }
                return true;
            }
            return false;
        }

        public bool CheckToolTip(Vector2 mousePosition, bool canTransportTroops, bool transportingTroops)
        {
            if (Contains(mousePosition))
            {
                if (canTransportTroops)
                {
                    if (transportingTroops)
                    {
                        ToolTip.SetText("cancelTransportingTroops", "cancelTransportingTroopsDesc");
                    }
                    else
                    {
                        ToolTip.SetText("transportTroops", "transportTroopsDesc");
                    }
                }
                else
                {
                    ToolTip.SetText("cannotTransportTroops", "cannotTransportTroopsDesc");
                }
                return true;
            }
            return false;
        }
    }

    public sealed class TransportCrewButton : PanelButton
    {
        public TransportCrewButton(Rect rect, Texture2D iconCancelTransportCrew, Texture2D iconTransportCrew, PlayButtonClick playButtonClick, GUIToolTip toolTip)
        {
            baseRect = rect;
            textures = new Texture2D[2];
            textures[0] = iconCancelTransportCrew;
            textures[1] = iconTransportCrew;
            ToolTip = toolTip;
            buttonClick = playButtonClick;
        }

        public void Draw(Ship ship)
        {
            if (ship.CanTransportCrew())
            {
                ShipManager shipManager = ship.GetShipManager();
                if (shipManager.TransporterTargetingCrew || ship.TransportingCrew())
                {
                    if (shipManager.TransporterTargetingCrew)
                        GUI.color = Color.yellow;
                    if (GUI.Button(baseRect, textures[0]))
                    {
                        if (ship.TransportingCrew())
                            ship.CancelTransportingCrew();
                        shipManager.TransporterTargetingCrew = false;
                        buttonClick();
                    }
                }
                else
                {
                    if (GUI.Button(baseRect, textures[1]))
                    {
                        shipManager.ResetAbilities();
                        shipManager.TransporterTargetingCrew = true;
                        buttonClick();
                    }
                }
                GUI.color = Color.white;
            }
            else
            {
                GUIControl = GUI.enabled;
                GUI.enabled = false;
                if (GUI.Button(baseRect, textures[1]))
                {

                }
                GUI.enabled = GUIControl;
            }
        }
        
        public void Draw(Station station)
        {
            if (station.CanTransportCrew())
            {
                ShipManager shipManager = station.GetShipManager();
                if (shipManager.TransporterTargetingCrew || station.TransportingCrew())
                {
                    if (shipManager.TransporterTargetingCrew)
                        GUI.color = Color.yellow;
                    if (GUI.Button(baseRect, textures[0]))
                    {
                        if (station.TransportingCrew())
                            station.CancelTransportingCrew();
                        shipManager.TransporterTargetingCrew = false;
                        buttonClick();
                    }
                }
                else
                {
                    if (GUI.Button(baseRect, textures[1]))
                    {
                        shipManager.ResetAbilities();
                        shipManager.TransporterTargetingCrew = true;
                        buttonClick();
                    }
                }
                GUI.color = Color.white;
            }
            else
            {
                GUIControl = GUI.enabled;
                GUI.enabled = false;
                if (GUI.Button(baseRect, textures[1]))
                {

                }
                GUI.enabled = GUIControl;
            }
        }

        public void Draw(List<Ship> ships, List<Station> stations, bool transportingCrew, bool canTransportCrew)
        {
            if (transportingCrew)
            {
                if (GUI.Button(baseRect, textures[0]))
                {
                    foreach (Ship ship in ships)
                    {
                        ship.CancelTransportingCrew();
                    }
                    foreach (Station station in stations)
                    {
                        station.CancelTransportingCrew();
                    }
                    shipManager.TransporterTargetingCrew = false;
                    buttonClick();
                }
            }
            else if (canTransportCrew)
            {
                if (GUI.Button(baseRect, textures[1]))
                {
                    shipManager.ResetAbilities();
                    shipManager.TransporterTargetingCrew = true;
                    buttonClick();
                }
            }
            else
            {
                GUIControl = GUI.enabled;
                GUI.enabled = false;
                if (GUI.Button(baseRect, textures[1]))
                {

                }
                GUI.enabled = GUIControl;
            }
        }

        public bool CheckToolTip(Vector2 mousePosition, Ship ship)
        {
            if (Contains(mousePosition))
            {
                if (ship.CanTransportCrew())
                {
                    if (ship.TransportingCrew())
                    {
                        ToolTip.SetText("cancelTransportingCrew", "cancelTransportingCrewDesc");
                    }
                    else
                    {
                        ToolTip.SetText("transportCrew", "transportCrewDesc");
                    }
                }
                else
                {
                    ToolTip.SetText("cannotTransportCrew", "cannotTransportCrewDesc");
                }
                return true;
            }
            return false;
        }

        public bool CheckToolTip(Vector2 mousePosition, Station station)
        {
            if (Contains(mousePosition))
            {
                if (station.CanTransportCrew())
                {
                    if (station.TransportingCrew())
                    {
                        ToolTip.SetText("cancelTransportingCrew", "cancelTransportingCrewDesc");
                    }
                    else
                    {
                        ToolTip.SetText("transportCrew", "transportCrewDesc");
                    }
                }
                else
                {
                    ToolTip.SetText("cannotTransportCrew", "cannotTransportCrewDesc");
                }
                return true;
            }
            return false;
        }

        public bool CheckToolTip(Vector2 mousePosition, bool canTransportCrew, bool transportingCrew)
        {
            if (Contains(mousePosition))
            {
                if (canTransportCrew)
                {
                    if (transportingCrew)
                    {
                        ToolTip.SetText("cancelTransportingCrew", "cancelTransportingCrewDesc");
                    }
                    else
                    {
                        ToolTip.SetText("transportCrew", "transportCrewDesc");
                    }
                }
                else
                {
                    ToolTip.SetText("cannotTransportCrew", "cannotTransportCrewDesc");
                }
                return true;
            }
            return false;
        }
    }

    public sealed class CloakingButton : PanelButton
    {
        public CloakingButton(Rect rect, Texture2D iconCancelCloak, Texture2D iconCloak, PlayButtonClick playButtonClick, GUIToolTip toolTip)
        {
            baseRect = rect;
            textures = new Texture2D[2];
            textures[0] = iconCancelCloak;
            textures[1] = iconCloak;
            ToolTip = toolTip;
            buttonClick = playButtonClick;
        }


        public void Draw(Ship ship)
        {
            if (ship.CanCloak())
            {
                if (ship.inCloakingState())
                {
                    if (GUI.Button(baseRect, textures[0]))
                    {
                        ship.DeCloak();
                        buttonClick();
                    }
                }
                else
                {
                    if (GUI.Button(baseRect, textures[1]))
                    {
                        ship.Cloak();
                        buttonClick();
                    }
                }
            }
            else
            {
                GUIControl = GUI.enabled;
                GUI.enabled = false;
                if (GUI.Button(baseRect, textures[1]))
                {

                }
                GUI.enabled = GUIControl;
            }
        }

        public void Draw(List<Ship> ships, bool isCloaked, bool canCloak)
        {
            if (isCloaked)
            {
                if (GUI.Button(baseRect, textures[0]))
                {
                    foreach (Ship ship in ships)
                    {
                        if (ship.inCloakingState())
                            ship.DeCloak();
                    }
                    buttonClick();
                }
            }
            else if (canCloak)
            {
                if (GUI.Button(baseRect, textures[1]))
                {
                    foreach (Ship ship in ships)
                    {
                        if (ship.CanCloak())
                            ship.Cloak();
                    }
                    buttonClick();
                }
            }
            else
            {
                GUIControl = GUI.enabled;
                GUI.enabled = false;
                if (GUI.Button(baseRect, textures[1]))
                {

                }
                GUI.enabled = GUIControl;
            }
        }

        public bool CheckToolTip(Vector2 mousePosition, Ship ship)
        {
            if (Contains(mousePosition))
            {
                if (ship.CanCloak())
                {
                    if (ship.inCloakingState())
                    {
                        ToolTip.SetText("cancelCloak", "cancelCloakDesc");
                    }
                    else
                    {
                        ToolTip.SetText("cloak", "cloakDesc");
                    }
                }
                else
                {
                    ToolTip.SetText("cannotCloak", "cannotCloakDesc");
                }
                return true;
            }
            return false;
        }

        public bool CheckToolTip(Vector2 mousePosition, bool isCloaked, bool canCloak)
        {
            if (Contains(mousePosition))
            {
                if (canCloak)
                {
                    if (isCloaked)
                    {
                        ToolTip.SetText("cancelCloak", "cancelCloakDesc");
                    }
                    else
                    {
                        ToolTip.SetText("cloak", "cloakDesc");
                    }
                }
                else
                {
                    ToolTip.SetText("cannotCloak", "cannotCloakDesc");
                }
                return true;
            }
            return false;
        }
    }

    public sealed class RetreatButton : PanelButton
    {
        public RetreatButton(Rect rect, Texture2D iconCancelRetreat, Texture2D iconRetreat, PlayButtonClick playButtonClick, GUIToolTip toolTip)
        {
            baseRect = rect;
            textures = new Texture2D[2];
            textures[0] = iconCancelRetreat;
            textures[1] = iconRetreat;
            ToolTip = toolTip;
            buttonClick = playButtonClick;
        }

        public void Draw(Ship ship)
        {
            if (ship.RetreatTimerRunning())
            {
                if (GUI.Button(baseRect, textures[0]))
                {
                    ship.SetRetreatTimer(0);
                    buttonClick();
                }
            }
            else
            {
                if (GUI.Button(baseRect, textures[1]))
                {
                    ship.SetRetreatTimer(ResourceManager.instance.GetGameConstants().RetreatTime);
                    ship.SetSelfDestructTime(0);
                    if (ship.inCloakingState())
                        ship.DeCloak();
                    buttonClick();
                }
            }
        }

        public void Draw(List<Ship> ships, bool Retreating)
        {
            if (Retreating)
            {
                if (GUI.Button(baseRect, textures[0]))
                {
                    foreach (Ship ship in ships)
                    {
                        ship.SetRetreatTimer(0);
                    }
                    buttonClick();
                }
            }
            else
            {
                if (GUI.Button(baseRect, textures[1]))
                {
                    foreach (Ship ship in ships)
                    {
                        ship.SetRetreatTimer(ResourceManager.instance.GetGameConstants().RetreatTime);
                        ship.SetSelfDestructTime(0);
                        if (ship.inCloakingState())
                            ship.DeCloak();
                    }
                    buttonClick();
                }
            }
        }

        public bool CheckToolTip(Vector2 mousePosition, Ship ship)
        {
            if (Contains(mousePosition))
            {
                if (ship.RetreatTimerRunning())
                {
                    ToolTip.SetText("cancelRetreat", "cancelRetreatDesc");
                }
                else
                {
                    ToolTip.SetText("retreat", "retreatDesc");
                }
                return true;
            }
            return false;
        }

        public bool CheckToolTip(Vector2 mousePosition, bool Retreating)
        {
            if (Contains(mousePosition))
            {
                if (Retreating)
                {
                    ToolTip.SetText("cancelRetreat", "cancelRetreatDesc");
                }
                else
                {
                    ToolTip.SetText("retreat", "retreatDesc");
                }
                return true;
            }
            return false;
        }
    }

    public sealed class SelfDestructButton : PanelButton
    {
        public SelfDestructButton(Rect rect, Texture2D iconCancelSelfDestruct, Texture2D iconSelfDestruct, PlayButtonClick playButtonClick, GUIToolTip toolTip)
        {
            baseRect = rect;
            textures = new Texture2D[2];
            textures[0] = iconCancelSelfDestruct;
            textures[1] = iconSelfDestruct;
            ToolTip = toolTip;
            buttonClick = playButtonClick;
        }

        public void Draw(Ship ship)
        {
            if (ship.SelfDestructTimerRunning())
            {
                if (GUI.Button(baseRect, textures[0]))
                {
                    ship.SetSelfDestructTime(0);
                    buttonClick();
                }
            }
            else
            {
                if (GUI.Button(baseRect, textures[1]))
                {
                    ship.SetSelfDestructTime(ResourceManager.instance.GetGameConstants().SelfDestructTime);
                    ship.SetRetreatTimer(0);
                    buttonClick();
                }
            }
        }

        public void Draw(Station station)
        {
            if (station.SelfDestructTimerRunning())
            {
                if (GUI.Button(baseRect, textures[0]))
                {
                    station.SetSelfDestructTime(0);
                    buttonClick();
                }
            }
            else
            {
                if (GUI.Button(baseRect, textures[1]))
                {
                    station.SetSelfDestructTime(ResourceManager.instance.GetGameConstants().SelfDestructTime);
                    buttonClick();
                }
            }
        }

        public void Draw(List<Ship> ships, List<Station> stations, bool selfDesctructing)
        {
            if (selfDesctructing)
            {
                if (GUI.Button(baseRect, textures[0]))
                {
                    foreach (Ship ship in ships)
                    {
                        ship.SetSelfDestructTime(0);
                    }
                    foreach (Station station in stations)
                    {
                        station.SetSelfDestructTime(0);
                    }
                    buttonClick();
                }
            }
            else
            {
                if (GUI.Button(baseRect, textures[1]))
                {
                    foreach (Ship ship in ships)
                    {
                        ship.SetSelfDestructTime(ResourceManager.instance.GetGameConstants().SelfDestructTime);
                        ship.SetRetreatTimer(0);
                    }
                    foreach (Station station in stations)
                    {
                        station.SetSelfDestructTime(ResourceManager.instance.GetGameConstants().SelfDestructTime);
                    }
                    buttonClick();
                }
            }
        }

        public bool CheckToolTip(Vector2 mousePosition, Ship ship)
        {
            if (Contains(mousePosition))
            {
                if (ship.SelfDestructTimerRunning())
                {
                    ToolTip.SetText("cancelSelfDestruct", "cancelSelfDestructDesc");
                }
                else
                {
                    ToolTip.SetText("selfDestruct", "selfDestructDesc");
                }
                return true;
            }
            return false;
        }

        public bool CheckToolTip(Vector2 mousePosition, Station station)
        {
            if (Contains(mousePosition))
            {
                if (station.SelfDestructTimerRunning())
                {
                    ToolTip.SetText("cancelSelfDestruct", "cancelSelfDestructDesc");
                }
                else
                {
                    ToolTip.SetText("selfDestruct", "selfDestructDesc");
                }
                return true;
            }
            return false;
        }

        public bool CheckToolTip(Vector2 mousePosition, bool selfDesctructing)
        {
            if (Contains(mousePosition))
            {
                if (selfDesctructing)
                {
                    ToolTip.SetText("cancelSelfDestruct", "cancelSelfDestructDesc");
                }
                else
                {
                    ToolTip.SetText("selfDestruct", "selfDestructDesc");
                }
                return true;
            }
            return false;
        }
    }
}
