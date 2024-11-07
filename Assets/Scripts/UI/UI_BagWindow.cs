using System;
using UnityEngine;
using UnityEngine.EventSystems;
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
        slot.Init(this,index);
        slots[index]=slot;
        slot.transform.SetSiblingIndex(index);
        return slot;
    }

    private IItemSlot CreateItemSlot(int index,ItemConfigBase itemConfig,ItemDataBase itemData)
    {
        GameObject go = GameObject.Instantiate(itemConfig.slotPrefab, itemRoot);
        IItemSlot slot = go.GetComponent<IItemSlot>();
        slot.Init(this,index,itemConfig, itemData,OnSlotDrag,OnUseItem);
        slots[index] =(SlotBase) slot;
        go.transform.SetSiblingIndex(index);
        return slot;
    }

    private void OnUseItem(int index, ItemConfigBase itemConfig, ItemDataBase itemData, PointerEventData.InputButton inputButton)
    {
        if (PlayerController.Instance == null&&inputButton!=PointerEventData.InputButton.Right) return;
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

    //格子A来自内部单格子b不一定
    private void OnSlotDrag(SlotBase slotA, SlotBase slotB)
    {
        //背包内部的拖拽行为
        if (slotB.ownerWindow==this)
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
        //TODO:出售物品
        else if(slotB.ownerWindow is UI_ShopWindow)
        {
            GameSceneManager.Instance.SellItem(slotA.index);
        }
    }

    public void UpdateSlot(int index, ItemDataBase itemData)
    {
        //销毁旧的格子
        GameObject.Destroy(slots[index].gameObject);

        if (itemData == null)//空格子
        {
            CreateEmptySlot(index);
        }
        else//有物品的格子
        {
            ItemConfigBase itemConfig = ResManager.Instance.GetItemConfig(itemData.id);
            CreateItemSlot(index, itemConfig, itemData);
        }
    }
}
