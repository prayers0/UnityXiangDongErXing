using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_BagWindow : UI_WindowBase
{
    public Button closeButton;
    public Transform itemRoot;
    public GameObject emptySlotPrefab;
    private BagData bagData;
    private SlotBase[] slots = new SlotBase[BagData.itemCount];

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
        WeaponSlot weaponSlot = (WeaponSlot)slots[bagData.usedWeaponIndex];
        weaponSlot.SetUseState(true);
    }

    private EmptySlot CreateEmptySlot(int index)
    {
        EmptySlot slot=GameObject.Instantiate(emptySlotPrefab,itemRoot).GetComponent<EmptySlot>();
        slot.Init(index);
        slots[index]=slot;
        slot.transform.SetSiblingIndex(index);
        return slot;
    }

    private IItemSlotBase CreateItemSlot(int index,ItemConfigBase itemConfig,ItemDataBase itemData)
    {
        GameObject go = GameObject.Instantiate(itemConfig.slotPrefab, itemRoot);
        IItemSlotBase slot = go.GetComponent<IItemSlotBase>();
        slot.Init(index,itemConfig, itemData,OnItemSwap,OnUseItem);
        slots[index] =(SlotBase) slot;
        go.transform.SetSiblingIndex(index);
        return slot;
    }

    private void OnUseItem(int index, ItemConfigBase itemConfig, ItemDataBase itemData)
    {
        if (PlayerController.Instance == null) return;
        PlayerController.Instance.OnUssItem(itemConfig, itemData);

        if(itemConfig is WeaponConfig)//切换武器
        {
            if (bagData.usedWeaponIndex != -1)
            {
                ((WeaponSlot)slots[bagData.usedWeaponIndex]).SetUseState(false);
            }
            bagData.usedWeaponIndex = index;
            ((WeaponSlot)slots[bagData.usedWeaponIndex]).SetUseState(true);
        }
        else if(itemConfig is ConsunableConfig)//使用消耗品
        {
            ConsumableData consumableData = (ConsumableData)itemData;
            consumableData.count -= 1;
            if (consumableData.count == 0)
            {
                bagData.items[index] = null;
                GameObject.Destroy(slots[index].gameObject);
                CreateEmptySlot(index);
            }
            else
            {
                ((ConsumableSlot)slots[index]).SetCount(consumableData.count);
            }
        }
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

        slots[aIndex] = slotB;
        slots[bIndex] = slotA;
    }

}
