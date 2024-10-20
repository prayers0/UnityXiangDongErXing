using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using System;

public interface IItemSlotBase
{
    public void Init(int index, ItemConfigBase itemConfig, ItemDataBase itemData,
        Action<SlotBase, SlotBase> onSwapAction, Action<int, ItemConfigBase, ItemDataBase,
            PointerEventData.InputButton> onRightButtonUseAction);
}

public abstract class ItemSlotBase<C,D> : SlotBase,IItemSlotBase,IBeginDragHandler,
    IDragHandler,IEndDragHandler,IPointerClickHandler
    where C:ItemConfigBase where D:ItemDataBase
{
    public Image iconImage;
    protected C itemConfig;
    protected D itemData;
    protected Action<SlotBase, SlotBase> onSwapAction;
    protected Action<int, ItemConfigBase, ItemDataBase, PointerEventData.InputButton> onUseAction;

    public virtual void Init(int index,C itemConfig,D itemDate)
    {
        this.itemConfig = itemConfig;
        this.itemData = itemDate;
        Init(index);
        iconImage.sprite = itemConfig.icon;
    }

    public void Init(int index,ItemConfigBase itemConfig, ItemDataBase itemData,
        Action<SlotBase,SlotBase> onSwapAction, Action<int, ItemConfigBase, ItemDataBase
            , PointerEventData.InputButton> onRightButtonUseAction)
    {
        this.onUseAction = onRightButtonUseAction;
        this.onSwapAction=onSwapAction;
        Init(index,(C)itemConfig, (D)itemData);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //将icon置到拖拽层
        iconImage.transform.SetParent(UIManager.Instance.dragLayer);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //icon跟随鼠标移动
        iconImage.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //icon归位
        iconImage.transform.SetParent(transform);
        iconImage.transform.SetAsFirstSibling();
        iconImage.transform.localPosition = Vector3.zero;

        //当前有进入其他格子
        if (enteredSlot != null && enteredSlot != this)
        {
            onSwapAction?.Invoke(this, enteredSlot);

        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (onUseAction!=null&&eventData.button == PointerEventData.InputButton.Right)
        {
            onUseAction.Invoke(index, itemConfig, itemData,eventData.button);
        }
    }
}
