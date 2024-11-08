using UnityEngine;
using UnityEngine.UI;

public class UI_GameMainWindow : UI_WindowBase
{
    public Text coinText;
    public Image hpFillImage;
    public Animation coinAnimation;

    public void SetCoin(int count)
    {
        coinText.text = count.ToString();
    }

    public void SetHp(float fillAmount)
    {
        hpFillImage.fillAmount = fillAmount;
    }

    public void CoinFlash(bool isRed)
    {
        coinAnimation.CrossFade(isRed?"CoinFlashRed":"CoinFlashGreen", 0);
    }
}
