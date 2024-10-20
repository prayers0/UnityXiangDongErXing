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
        slot.Init(index);
        slots[index] = slot;
        slot.transform.SetSiblingIndex(index);
        return slot;
    }

    private IItemSlotBase CreateItemSlot(int index, ItemConfigBase itemConfig, ItemDataBase itemData)
    {
        GameObject go = GameObject.Instantiate(itemConfig.slotPrefab, itemRoot);
        IItemSlotBase slot = go.GetComponent<IItemSlotBase>();
        slot.Init(index, itemConfig, itemData, null, OnUseItem);
        slots[index] = (SlotBase)slot;
        go.transform.SetSiblingIndex(index);
        return slot;
    }

    private void OnUseItem(int index, ItemConfigBase itemConfig, ItemDataBase itemData, PointerEventData.InputButton inputButton)
    {
        if (PlayerController.Instance == null&&inputButton!=PointerEventData.InputButton.Left) return;
    }
}
