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

    public void Show(ItemConfigBase itemConfig)
    {
        iconImage.sprite=itemConfig.icon;
        nameText.text=itemConfig.itemName;
        if(itemConfig is WeaponConfig)
        {
            typeText.text = "ÎäÆ÷";
        }
        else if(itemConfig is ConsunableConfig)
        {
            typeText.text = "ÏûºÄÆ·";
        }
        priceText.text=itemConfig.price.ToString();
        descriptionText.text=itemConfig.description.ToString();
    }
}
