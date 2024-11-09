using UnityEngine;
using UnityEngine.UI;

public class UI_MenuSceneMainWindow : UI_WindowBase
{
    public Button newGameButton;
    public Button continueButton;
    public Button settingButton;
    public Button quitButton;

    public override void OnShow()
    {
        newGameButton.onClick.AddListener(NewGameButtonClick);
        continueButton.onClick.AddListener(ContinueButtonClick);
        if (!SaveManager.ExistGameData())
        {
            continueButton.gameObject.SetActive(false);
        }
        settingButton.onClick.AddListener(SettingButtonClick);
        quitButton.onClick.AddListener(quitButtonClick);
    }

    private void NewGameButtonClick()
    {
        GameManager.Instance.NewGame();
    }

    private void ContinueButtonClick()
    {
        GameManager.Instance.ContinueGame();
    }

    private void SettingButtonClick()
    {
        UIManager.Instance.ShowWindow<UI_SettingsWindow>();
    }

    private void quitButtonClick()
    {
        Application.Quit();
    }
}
