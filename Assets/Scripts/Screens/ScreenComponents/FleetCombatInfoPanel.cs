/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class FleetCombatInfoPanel
{
    FleetData fleetData;
    Rect mainRect;
    Rect AdmiralPortraitRect;
    Rect FleetNameRect;
    Rect AdmiralNameRect;
    Rect CommandRect;
    Rect DamageBonusRect;
    Rect DefenseBonusRect;
    Rect TransporterRect;

    GameManager gameController;

    public FleetCombatInfoPanel(Rect rect, FleetData fleet, GameManager GC)
    {
        mainRect = rect;
        fleetData = fleet;
        gameController = GC;

        AdmiralPortraitRect = new Rect(mainRect.x + mainRect.height * 0.02f, mainRect.y + mainRect.height * 0.02f, mainRect.width * 0.41f, mainRect.height * 0.36f);

        FleetNameRect = new Rect(AdmiralPortraitRect.xMax + mainRect.height * 0.02f, AdmiralPortraitRect.y + mainRect.height * 0.01f, gameController.StandardLabelSize.x, gameController.StandardLabelSize.y);
        AdmiralNameRect = new Rect(FleetNameRect.x, FleetNameRect.yMax + mainRect.height * 0.005f, gameController.StandardLabelSize.x, gameController.StandardLabelSize.y);
        CommandRect = new Rect(FleetNameRect.x, AdmiralNameRect.yMax + mainRect.height * 0.005f, gameController.StandardLabelSize.x, gameController.StandardLabelSize.y);
        DamageBonusRect = new Rect(FleetNameRect.x, CommandRect.yMax + mainRect.height * 0.005f, gameController.StandardLabelSize.x, gameController.StandardLabelSize.y);
        DefenseBonusRect = new Rect(FleetNameRect.x, DamageBonusRect.yMax + mainRect.height * 0.005f, gameController.StandardLabelSize.x, gameController.StandardLabelSize.y);
        TransporterRect = new Rect(FleetNameRect.x, DefenseBonusRect.yMax + mainRect.height * 0.005f, gameController.StandardLabelSize.x, gameController.StandardLabelSize.y);
    }

    public void Draw()
    {
        GUI.Box(mainRect, "");
        GUI.Box(AdmiralPortraitRect, "Admiral");

        GUI.Label(FleetNameRect, "1st Fleet", gameController.standardLabelStyle);

        GUI.Label(AdmiralNameRect, "No Admiral", gameController.standardLabelStyle);

        gameController.UIContent.text = fleetData.GetCommand().ToString();
        //gameController.UIContent.image = ResourceManager.instance.GetIconTexture("Icon_CommandPoint");
        GUI.Label(CommandRect, gameController.UIContent, gameController.standardLabelStyle);

        gameController.UIContent.text = fleetData.GetFleetDamageBonus().ToString();
        //gameController.UIContent.image = ResourceManager.instance.GetIconTexture("Icon_DamageFleet");
        GUI.Label(DamageBonusRect, gameController.UIContent, gameController.standardLabelStyle);

        gameController.UIContent.text = fleetData.GetFleetDefenseBonus().ToString();
        //gameController.UIContent.image = ResourceManager.instance.GetIconTexture("Icon_DefenseFleet");
        GUI.Label(DefenseBonusRect, gameController.UIContent, gameController.standardLabelStyle);

        gameController.UIContent.text = fleetData.GetTransport().ToString();
        //gameController.UIContent.image = ResourceManager.instance.GetIconTexture("Icon_Transporter");
        GUI.Label(TransporterRect, gameController.UIContent, gameController.standardLabelStyle);
    }

    public bool Contains(Vector2 point)
    {
        return mainRect.Contains(point);
    }
}
