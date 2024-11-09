using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_SettingsWindow : UI_WindowBase
{
    public Button closeButton;
    public Slider audioSlider;
    public Dropdown resolutionDropDown;
    public Toggle fullScreenToggle;
    private GameSettings gameSettings;

    public override void OnShow()
    {
        closeButton.onClick.AddListener(OnCloseButtonClick);
        gameSettings = GameManager.Instance.gameSettings;
        audioSlider.value = gameSettings.volume;
        audioSlider.onValueChanged.AddListener(OnAudioSliderValueChanged);
        List<string> resolutionTypeString = GameSettings.ResolutionTypeString;
        List<Dropdown.OptionData> optionDatas = new List<Dropdown.OptionData>();
        for(int i = 0; i < resolutionTypeString.Count; i++)
        {
            optionDatas.Add(new Dropdown.OptionData(resolutionTypeString[i]));
        }
        resolutionDropDown.options=optionDatas;
        resolutionDropDown.value = (int)gameSettings.resolutionType;
        resolutionDropDown.onValueChanged.AddListener(OnResolutionDropdownValueChanged);

        fullScreenToggle.isOn = gameSettings.fullScreen;
        fullScreenToggle.onValueChanged.AddListener(OnFullScreenToggleValueChanged);
    }

    public override void OnClose()
    {
        GameManager.Instance.SaveGameSettings();
    }

    private void OnCloseButtonClick()
    {
        UIManager.Instance.CloseWindow<UI_SettingsWindow>();
    }

    private void OnAudioSliderValueChanged(float value)
    {
        gameSettings.volume = value;
        GameManager.Instance.ApplyGameSettings();
    }

    private void OnResolutionDropdownValueChanged(int value)
    {
        gameSettings.resolutionType = (ResolutionType)value;
        GameManager.Instance.ApplyGameSettings();
    }

    private void OnFullScreenToggleValueChanged(bool value)
    {
        gameSettings.fullScreen=value;
        GameManager.Instance.ApplyGameSettings();
    }


}
