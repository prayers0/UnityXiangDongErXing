using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ShopWindow : UI_WindowBase
{
    public const int itemCount= 5;
    public Button closeButton;
    public Transform itemRoot;
    public GameObject emptySlotPrefab;
    private SlotBase[] slots = new SlotBase[itemCount];

    public override void OnShow()
    {
        closeButton.onClick.AddListener(CloseButtonClick);
    }

    private void CloseButtonClick()
    {
        UIManager.Instance.CloseWindow<UI_ShopWindow>();
    }

    public void Show(List<ItemConfigBase> items)
    {
        for(int i = 0; i < itemCount; i++)
        {
            if (i >= items.Count)
            {
                CreateEmptySlot(i);
            }
            else
            {
                ItemConfigBase itemConifg = items[i];
                ItemDataBase itemData=itemConifg.GetDefaultData();
                CreateItemSlot(i, itemConifg, itemData);
            }
        }
    }

    private EmptySlot CreateEmptySlot(int index)
    {
        EmptySlot slot = GameObject.Instantiate(emptySlotPrefab, itemRoot).GetComponent<EmptySlot>();
        slot.Init(this,index);
        slots[index] = slot;
        slot.transform.SetSiblingIndex(index);
        return slot;
    }

    private IItemSlot CreateItemSlot(int index, ItemConfigBase itemConfig, ItemDataBase itemData)
    {
        GameObject go = GameObject.Instantiate(itemConfig.slotPrefab, itemRoot);
        IItemSlot slot = go.GetComponent<IItemSlot>();
        slot.Init(this,index, itemConfig, itemData, OnSlotDrag, null);
        slots[index] = (SlotBase)slot;
        go.transform.SetSiblingIndex(index);
        return slot;
    }

    private void OnSlotDrag(SlotBase slotA,SlotBase slotB)
    {
        //购物物品：格子A本身是物品&&格子B是背包格子&&格子B是空格子
        if (slotA is IItemSlot&&slotB.ownerWindow is UI_BagWindow)
        {
            IItemSlot itemSlot = (IItemSlot)slotA;

        }
    }
}
