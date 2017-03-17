/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class ScenarioListEntry
{
    Rect mainRect;
    Rect TitleRect;
    Rect DifficultlyRect;
    Rect TimeRect;
    Rect CommandRect;
    Rect MoneyRect;
    Rect DescriptionRect;

    string Difficulty;
    string Time;
    string Command;
    string Money;

    public Scenario scenario;
    public delegate void ButtonPress(Scenario scenario);
    protected ButtonPress buttonCallBack;
    GameManager gameController;

    public ScenarioListEntry(Rect rect, Scenario scene, ButtonPress callBack, GameManager controller)
    {
        mainRect = rect;

        TitleRect = new Rect(rect.x + rect.width * 0.05f, rect.y + rect.height * 0.03f, rect.width * 0.9f, rect.height * 0.15f);
        DifficultlyRect = new Rect(TitleRect.x, TitleRect.yMax, TitleRect.width, rect.height * 0.1f);
        TimeRect = new Rect(TitleRect.x, DifficultlyRect.yMax, TitleRect.width, rect.height * 0.1f);
        CommandRect = new Rect(TitleRect.x, TimeRect.yMax, TitleRect.width, rect.height * 0.1f);
        MoneyRect = new Rect(TitleRect.x, CommandRect.yMax, TitleRect.width, rect.height * 0.1f);
        DescriptionRect = new Rect(TitleRect.x, MoneyRect.yMax, TitleRect.width, rect.height * 0.35f);

        scenario = scene;
        buttonCallBack = callBack;

        gameController = controller;

        Difficulty = "Difficulty: " + GetDifficultyString(scenario.Difficulty);
        Time = "Time: " + ((int)scenario.TimeLimit / 60).ToString("0") + ":" + (scenario.TimeLimit % 60).ToString("00");
        Command = "Command: " + scenario.CommandLimit.ToString();
        Money = "Money: " + scenario.MoneyLimit.ToString();
    }

    string GetDifficultyString(ScenarioDifficulty difficulty)
    {
        switch(difficulty)
        {
            case ScenarioDifficulty.veryEasy:
                {
                    return "Very Easy";
                }
            case ScenarioDifficulty.easy:
                {
                    return "Easy";
                }
            case ScenarioDifficulty.normal:
                {
                    return "Normal";
                }
            case ScenarioDifficulty.hard:
                {
                    return "Hard";
                }
            case ScenarioDifficulty.veryHard:
                {
                    return "Very Hard";
                }
            default:
                {
                    return "";
                }
        }
    }

    public void Draw(Scenario selected)
    {
        if (selected != null && selected.Name == scenario.Name)
        {
            if (GUI.Button(mainRect, "", gameController.SquareButtonGreenStyle))
            {
                buttonCallBack(scenario);
                AudioManager.instance.PlayUIClip("SelectButtonClick");
            }
        }
        else
        {
            if (GUI.Button(mainRect, ""))
            {
                buttonCallBack(scenario);
                AudioManager.instance.PlayUIClip("SelectButtonClick");
            }
        }

        //Display info
        GUI.Label(TitleRect, scenario.Name, gameController.ModuleTitleStyle);
        GUI.Label(DifficultlyRect, Difficulty);
        GUI.Label(TimeRect, Time, gameController.standardLabelStyle);
        GUI.Label(CommandRect, Command, gameController.standardLabelStyle);
        GUI.Label(MoneyRect, Money, gameController.standardLabelStyle);
        GUI.Box(DescriptionRect, scenario.Description, gameController.standardLabelStyle);
    }
}
