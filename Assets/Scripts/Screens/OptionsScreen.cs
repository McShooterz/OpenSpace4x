/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsScreen : BaseScreen
{
    #region Variables

    [SerializeField]
    GameObject GameplayGroup;
    [SerializeField]
    GameObject VideoGroup;
    [SerializeField]
    GameObject AudioGroup;


    //Gameplay Options
    [SerializeField]
    Toggle ShowCombatDamageToggle;

    //Audio Options
    [SerializeField]
    Slider MasterVolumeSlider;
    [SerializeField]
    Slider MusicVolumeSlider;
    [SerializeField]
    Slider UIVolumeSlider;
    [SerializeField]
    Slider EffectsVolumeSlider;

    #endregion

    // Use this for initialization
    void Start()
    {
        GameplayGroup.SetActive(true);
        VideoGroup.SetActive(false);
        AudioGroup.SetActive(false);

        ShowCombatDamageToggle.isOn = GameManager.instance.GetShowCombatDamage();

        MasterVolumeSlider.value = AudioManager.instance.MasterVolume;
        MusicVolumeSlider.value = AudioManager.instance.MusicVolume;
        UIVolumeSlider.value = AudioManager.instance.UIVolume;
        EffectsVolumeSlider.value = AudioManager.instance.EffectsVolume;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChangeLastScreen();
        }
        AudioManager.instance.UpdateMusicVolume();
        AudioManager.instance.UpdateUIVolume();
    }

    public void ClickGameplayButton()
    {
        GameplayGroup.SetActive(true);
        VideoGroup.SetActive(false);
        AudioGroup.SetActive(false);
    }

    public void ClickVideoButton()
    {
        GameplayGroup.SetActive(false);
        VideoGroup.SetActive(true);
        AudioGroup.SetActive(false);
    }

    public void ClickAudioButton()
    {
        GameplayGroup.SetActive(false);
        VideoGroup.SetActive(false);
        AudioGroup.SetActive(true);
    }

    public void UpdateShowCombatDamage(bool state)
    {
        GameManager.instance.SetShowCombatDamage(state);
    }

    public void UpdateMasterVolume(float value)
    {
        AudioManager.instance.MasterVolume = value;
    }

    public void UpdateMusicVolume(float value)
    {
        AudioManager.instance.MusicVolume = value;
    }

    public void UpdateUIVolume(float value)
    {
        AudioManager.instance.UIVolume = value;
    }

    public void UpdateEffectsVolume(float value)
    {
        AudioManager.instance.EffectsVolume = value;
    }
}
