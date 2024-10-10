using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_BagWindow : UI_WindowBase
{
    public Button closeButton;
    public Transform itemRoot;
    public GameObject emptySlotPrefab;

    public override void OnShow()
    {
        closeButton.onClick.AddListener(CloseButtonClick);
    }

    private void CloseButtonClick()
    {
        UIManager.Instance.CloseWindow<UI_BagWindow>();
    }

    public void Show(BagData bagData)
    {
        for(int i = 0; i < BagData.itemCount; i++)
        {
            ItemDataBase itemData = bagData.items[i];
            if (itemData == null)//空格子
            {
                CreateEmptySlot();
            }
            else//有物品的格子
            {
                ItemConfigBase itemConfig = ResManager.Instance.GetItemConfig(itemData.id);
                CreateItemSlot(itemConfig,itemData);
            }
        }
    }

    private EmptySlot CreateEmptySlot()
    {
        EmptySlot slot=GameObject.Instantiate(emptySlotPrefab,itemRoot).GetComponent<EmptySlot>();
        slot.Init();
        return slot;
    }

    private IItemSlotBase CreateItemSlot(ItemConfigBase itemConfig,ItemDataBase itemData)
    {
        IItemSlotBase slot = GameObject.Instantiate(itemConfig.slotPrefab, itemRoot).GetComponent<IItemSlotBase>();
        slot.Init(itemConfig, itemData);
        return slot;
    }
}
