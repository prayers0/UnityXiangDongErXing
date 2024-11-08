using UnityEngine.UI;

public class UI_GameOverWindow : UI_WindowBase
{
    public Button homeButton;
    public override void OnShow()
    {
        homeButton.onClick.AddListener(OnHomeButtonClick);
    }

    private void OnHomeButtonClick()
    {
        GameManager.Instance.LoadMenuScene();
    }
}
