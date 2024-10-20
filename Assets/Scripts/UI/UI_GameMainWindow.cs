using UnityEngine.UI;

public class UI_GameMainWindow : UI_WindowBase
{
    public Text coinText;
    public Image hpFillImage;

    public void SetCoin(int count)
    {
        coinText.text = count.ToString();
    }

    public void SetHp(float fillAmount)
    {
        hpFillImage.fillAmount = fillAmount;
    }
}
