using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_BagWindow : UI_WindowBase
{
    public Button closeButton;
    public Transform itemRoot;
    public GameObject emptySlotPrefab;
    private BagData bagData;

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
        this.bagData = bagData;
        for(int i = 0; i < BagData.itemCount; i++)
        {
            ItemDataBase itemData = bagData.items[i];
            if (itemData == null)//空格子
            {
                CreateEmptySlot(i);
            }
            else//有物品的格子
            {
                ItemConfigBase itemConfig = ResManager.Instance.GetItemConfig(itemData.id);
                CreateItemSlot(i,itemConfig,itemData);
            }
        }
    }

    private EmptySlot CreateEmptySlot(int index)
    {
        EmptySlot slot=GameObject.Instantiate(emptySlotPrefab,itemRoot).GetComponent<EmptySlot>();
        slot.Init(index);
        return slot;
    }

    private IItemSlotBase CreateItemSlot(int index,ItemConfigBase itemConfig,ItemDataBase itemData)
    {
        IItemSlotBase slot = GameObject.Instantiate(itemConfig.slotPrefab, itemRoot).GetComponent<IItemSlotBase>();
        slot.Init(index,itemConfig, itemData,OnItemSwap);
        return slot;
    }

    private void OnItemSwap(SlotBase slotA, SlotBase slotB)
    {
        int aIndex = slotA.index;
        int bIndex = slotB.index;
        slotA.transform.SetSiblingIndex(bIndex);
        slotA.SetIndex(bIndex);
        slotB.transform.SetSiblingIndex(aIndex);
        slotB.SetIndex(aIndex);
        bagData.Swap(aIndex, bIndex);
    }

}
