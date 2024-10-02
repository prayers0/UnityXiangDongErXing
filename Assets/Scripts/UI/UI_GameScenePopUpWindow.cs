using UnityEngine;
using UnityEngine.UI;

public class UI_GameScenePopUpWindow : UI_WindowBase
{
    public Button saveGameButton;
    public Button continueButton;
    public Button settingButton;
    public Button backButton;
    public override void OnShow()
    {
        Time.timeScale = 0;
        saveGameButton.onClick.AddListener(SaveGameButtonClick);
        continueButton.onClick.AddListener(ContinueButtonClick);
        
        settingButton.onClick.AddListener(SettingButtonClick);
        backButton.onClick.AddListener(BackButtonClick);
    }

    public override void OnClose()
    {
        Time.timeScale = 1;
    }

    private void SaveGameButtonClick()
    {
        GameManager.Instance.SaveGameData();
    }

    private void ContinueButtonClick()
    {
        UIManager.Instance.CloseWindow<UI_GameScenePopUpWindow>();
    }

    private void SettingButtonClick()
    {

    }

    private void BackButtonClick()
    {
        SaveGameButtonClick();
        GameManager.Instance.LoadMenuScene();
    }
}
