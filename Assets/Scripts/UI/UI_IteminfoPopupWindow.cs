using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_IteminfoPopupWindow : UI_WindowBase
{
    public Image iconImage;
    public Text nameText;
    public Text typeText;
    public Text priceText;
    public Text descriptionText;
    private RectTransform rectTransform => (RectTransform)transform;

    public void Show(Vector3 uiWorldPosition,ItemConfigBase itemConfig)
    {
        //坐标计算
        transform.position = uiWorldPosition;
        Vector2 windowSize=rectTransform.sizeDelta;
        Vector2 canvasSize = new Vector2(1920, 1080);
        Vector2 widthRange = new Vector2(canvasSize.x / -2 + windowSize.x / 2, canvasSize.x / 2 - windowSize.x / 2);
        Vector2 heightRange = new Vector2(canvasSize.y / -2 + windowSize.y / 2, canvasSize.y / 2 - windowSize.y / 2);
        Vector2 uiPos = rectTransform.anchoredPosition;
        uiPos.x = Mathf.Clamp(uiPos.x, widthRange.x, widthRange.y);
        uiPos.y = Mathf.Clamp(uiPos.y, widthRange.x, heightRange.y);
        rectTransform.anchoredPosition = uiPos;

        //坐标信息的设置
        iconImage.sprite=itemConfig.icon;
        nameText.text=itemConfig.itemName;
        if(itemConfig is WeaponConfig)
        {
            typeText.text = "武器";
        }
        else if(itemConfig is ConsunableConfig)
        {
            typeText.text = "消耗品";
        }
        priceText.text=itemConfig.price.ToString();
        descriptionText.text=itemConfig.description.ToString();
    }
}
