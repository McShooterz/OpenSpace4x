/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class OptionsScreen : ScreenParent
{
    //Main Screen Sections
    Rect SidePanelRect;
    Rect MainPanelRect;

    //Side panel buttons
    Rect BackButtonRect;
    Rect GameButtonRect;
    Rect VideoButtonRect;
    Rect AudioButtonRect;

    // Main panel shared
    Rect PanelTitle;

    // Gameplay options
    Rect CombatDamageShowToggleRect;
    Rect CombatDamageShowRect;

    // Video options


    // Audio options
    Rect MasterVolumeLabelRect;
    Rect MasterVolumeSliderRect;
    Rect MusicVolumeLabelRect;
    Rect MusicVolumeSliderRect;
    Rect UIVolumeLabelRect;
    Rect UIVolumeSliderRect;
    Rect EffectsVolumeLabelRect;
    Rect EffectsVolumeSliderRect;

    GUIStyle ScrollStyle;

    OptionMode mode = OptionMode.gameplay;

    ScreenParent LastScreen;

    public OptionsScreen(ScreenParent currentScreen)
    {
        LastScreen = currentScreen;
        SidePanelRect = new Rect(Screen.width * 0.04f, Screen.height * 0.06f, Screen.width * 0.12f, Screen.height * 0.88f);
        MainPanelRect = new Rect(SidePanelRect.xMax + Screen.width * 0.04f, SidePanelRect.y, Screen.width * 0.76f, Screen.height * 0.88f);

        //Side panel buttons
        Vector2 SidePanelButtonSize = new Vector2(SidePanelRect.width * 0.88f, SidePanelRect.height * 0.07f);
        BackButtonRect = new Rect(SidePanelRect.x + SidePanelRect.width * 0.06f, SidePanelRect.yMax - SidePanelButtonSize.y * 1.5f, SidePanelButtonSize.x, SidePanelButtonSize.y);
        GameButtonRect = new Rect(BackButtonRect.x, SidePanelRect.y + SidePanelButtonSize.y * 0.5f, SidePanelButtonSize.x, SidePanelButtonSize.y);
        VideoButtonRect = new Rect(BackButtonRect.x, GameButtonRect.yMax + SidePanelButtonSize.y * 0.5f, SidePanelButtonSize.x, SidePanelButtonSize.y);
        AudioButtonRect = new Rect(BackButtonRect.x, VideoButtonRect.yMax + SidePanelButtonSize.y * 0.5f, SidePanelButtonSize.x, SidePanelButtonSize.y);

        // Main panel shared
        Vector2 TitleSize = new Vector2(MainPanelRect.width * 0.16f, MainPanelRect.height * 0.08f);
        PanelTitle = new Rect(MainPanelRect.x + (MainPanelRect.width - TitleSize.x) / 2f, MainPanelRect.y + TitleSize.y * 0.334f, TitleSize.x, TitleSize.y);
        float Indent = MainPanelRect.x + MainPanelRect.width * 0.025f;

        // Gameplay options
        CombatDamageShowToggleRect = new Rect(Indent, PanelTitle.yMax + PanelTitle.height * 0.25f, GameManager.instance.StandardLabelSize.y, GameManager.instance.StandardLabelSize.y);
        CombatDamageShowRect = new Rect(Indent + GameManager.instance.StandardLabelSize.y + MainPanelRect.width * 0.03f, CombatDamageShowToggleRect.y, GameManager.instance.StandardLabelSize.x * 1.25f, GameManager.instance.StandardLabelSize.y);

        // Video options


        // Audio options
        MasterVolumeLabelRect = new Rect(Indent, PanelTitle.yMax + PanelTitle.height * 0.25f, GameManager.instance.StandardLabelSize.x, GameManager.instance.StandardLabelSize.y);
        MasterVolumeSliderRect = new Rect(MasterVolumeLabelRect.xMax + MainPanelRect.width * 0.03f, MasterVolumeLabelRect.y, GameManager.instance.StandardLabelSize.x * 2f, GameManager.instance.StandardLabelSize.y);
        MusicVolumeLabelRect = new Rect(Indent, MasterVolumeLabelRect.yMax + PanelTitle.height * 0.25f, GameManager.instance.StandardLabelSize.x, GameManager.instance.StandardLabelSize.y);
        MusicVolumeSliderRect = new Rect(MusicVolumeLabelRect.xMax + MainPanelRect.width * 0.03f, MusicVolumeLabelRect.y, GameManager.instance.StandardLabelSize.x * 2f, GameManager.instance.StandardLabelSize.y);
        UIVolumeLabelRect = new Rect(Indent, MusicVolumeLabelRect.yMax + PanelTitle.height * 0.25f, GameManager.instance.StandardLabelSize.x, GameManager.instance.StandardLabelSize.y);
        UIVolumeSliderRect = new Rect(UIVolumeLabelRect.xMax + MainPanelRect.width * 0.03f, UIVolumeLabelRect.y, GameManager.instance.StandardLabelSize.x * 2f, GameManager.instance.StandardLabelSize.y);
        EffectsVolumeLabelRect = new Rect(Indent, UIVolumeLabelRect.yMax + PanelTitle.height * 0.25f, GameManager.instance.StandardLabelSize.x, GameManager.instance.StandardLabelSize.y);
        EffectsVolumeSliderRect = new Rect(EffectsVolumeLabelRect.xMax + MainPanelRect.width * 0.03f, EffectsVolumeLabelRect.y, GameManager.instance.StandardLabelSize.x * 2f, GameManager.instance.StandardLabelSize.y);
    }

    public override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseScreen();
        }
    }

    public override void Draw()
    {
        if (ScrollStyle == null)
            ScrollStyle = new GUIStyle(GUI.skin.button);

        GUI.Box(SidePanelRect, "", GameManager.instance.standardBackGround);
        GUI.Box(MainPanelRect, "", GameManager.instance.standardBackGround);

        if(GUI.Button(BackButtonRect, "Back"))
        {
            PlayMainButtonClick();
            CloseScreen();
        }

        if (GUI.Button(GameButtonRect, "Game"))
        {
            PlayMainButtonClick();
            mode = OptionMode.gameplay;
        }

        if (GUI.Button(VideoButtonRect, "Video"))
        {
            PlayMainButtonClick();
            mode = OptionMode.video;
        }

        if (GUI.Button(AudioButtonRect, "Audio"))
        {
            PlayMainButtonClick();
            mode = OptionMode.audio;
        }

        if (mode == OptionMode.gameplay)
        {
            DrawGameplayOptions();
        }
        else if(mode == OptionMode.video)
        {
            DrawVideoOptions();
        }
        else
        {
            DrawAudioOptions();
        }
    }

    void DrawGameplayOptions()
    {
        GUI.Box(PanelTitle, "Game", GameManager.instance.ModuleTitleStyle);

        if (GameManager.instance.GetShowCombatDamage())
        {
            if(GUI.Button(CombatDamageShowToggleRect, "X"))
            {
                GameManager.instance.SetShowCombatDamage(false);
            }
        }
        else
        {
            if (GUI.Button(CombatDamageShowToggleRect, ""))
            {
                GameManager.instance.SetShowCombatDamage(true);
            }
        }
        GUI.Label(CombatDamageShowRect, "Show Combat Damage");
    }

    void DrawVideoOptions()
    {
        GUI.Box(PanelTitle, "Video", GameManager.instance.ModuleTitleStyle);
    }

    void DrawAudioOptions()
    {
        GUI.Box(PanelTitle, "Audio", GameManager.instance.ModuleTitleStyle);

        GUI.Label(MasterVolumeLabelRect, "Master Volume");
        AudioManager.instance.MasterVolume = GUI.HorizontalSlider(MasterVolumeSliderRect, AudioManager.instance.MasterVolume, 0f, 1f, ScrollStyle, ScrollStyle);

        GUI.Label(MusicVolumeLabelRect, "Music Volume");
        AudioManager.instance.MusicVolume = GUI.HorizontalSlider(MusicVolumeSliderRect, AudioManager.instance.MusicVolume, 0f, 1f, ScrollStyle, ScrollStyle);

        GUI.Label(UIVolumeLabelRect, "UI Volume");
        AudioManager.instance.UIVolume = GUI.HorizontalSlider(UIVolumeSliderRect, AudioManager.instance.UIVolume, 0f, 1f, ScrollStyle, ScrollStyle);

        GUI.Label(EffectsVolumeLabelRect, "Effects Volume");
        AudioManager.instance.EffectsVolume = GUI.HorizontalSlider(EffectsVolumeSliderRect, AudioManager.instance.EffectsVolume, 0f, 1f, ScrollStyle, ScrollStyle);


        // Update for slider changes
        AudioManager.instance.UpdateMusicVolume();
        AudioManager.instance.UpdateUIVolume();
    }

    protected override void CloseScreen()
    {
        GameManager.instance.ChangeScreen(LastScreen);
    }

    enum OptionMode
    {
        gameplay,
        video,
        audio
    }
}
