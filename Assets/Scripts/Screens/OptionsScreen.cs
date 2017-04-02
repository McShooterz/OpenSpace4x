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

    [SerializeField]
    Button GameplayButton;
    [SerializeField]
    Button VideoButton;
    [SerializeField]
    Button AudioButton;

    //Gameplay Options
    [Header("Gameplay")]
    [SerializeField]
    Text GameplayTitle;
    [SerializeField]
    Toggle ShowCombatDamageToggle;

    //Video Options
    [Header("Video")]
    [SerializeField]
    Text VideoTitle;

    //Audio Options
    [Header("Audio")]
    [SerializeField]
    Text AudioTitle;

    [SerializeField]
    Text VoumeLabel;
    [SerializeField]
    Text MasterVoumeLabel;
    [SerializeField]
    Text MusicVolumeLabel;
    [SerializeField]
    Text UIVolumeLabel;
    [SerializeField]
    Text EffectsVolumeLabel;

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

        GameplayButton.GetComponentInChildren<Text>().text = "Gameplay";
        VideoButton.GetComponentInChildren<Text>().text = "Video";
        AudioButton.GetComponentInChildren<Text>().text = "Audio";

        GameplayTitle.text = "Gameplay";

        ShowCombatDamageToggle.GetComponentInChildren<Text>().text = "Show Combat Damage";
        ShowCombatDamageToggle.isOn = GameManager.instance.GetShowCombatDamage();

        VideoTitle.text = "Video";

        AudioTitle.text = "Audio";

        VoumeLabel.text = "Volume";
        MasterVoumeLabel.text = "Master";
        MusicVolumeLabel.text = "Music";
        UIVolumeLabel.text = "UI";
        EffectsVolumeLabel.text = "Effects";

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
