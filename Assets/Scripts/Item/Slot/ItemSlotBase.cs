using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using System;

public interface IItemSlot
{
    public void Init(UI_WindowBase ownerWindow, int index, ItemConfigBase itemConfig, ItemDataBase itemData,
        Action<SlotBase, SlotBase> onDragAction, Action<int, ItemConfigBase, ItemDataBase,
            PointerEventData.InputButton> onRightButtonUseAction);
}

public abstract class ItemSlotBase<C,D> : SlotBase,IItemSlot,IBeginDragHandler,
    IDragHandler,IEndDragHandler,IPointerClickHandler
    where C:ItemConfigBase where D:ItemDataBase
{
    public Image iconImage;
    protected C itemConfig;
    protected D itemData;
    protected Action<SlotBase, SlotBase> onDragAction;
    protected Action<int, ItemConfigBase, ItemDataBase, PointerEventData.InputButton> onUseAction;

    public virtual void Init(UI_WindowBase ownerWindow,int index,C itemConfig,D itemDate)
    {
        this.itemConfig = itemConfig;
        this.itemData = itemDate;
        Init(ownerWindow,index);
        iconImage.sprite = itemConfig.icon;
    }

    public void Init(UI_WindowBase ownerWindow, int index,ItemConfigBase itemConfig, ItemDataBase itemData,
        Action<SlotBase,SlotBase> onDragAction, Action<int, ItemConfigBase, ItemDataBase
            , PointerEventData.InputButton> onRightButtonUseAction)
    {
        this.onUseAction = onRightButtonUseAction;
        this.onDragAction = onDragAction;
        Init(ownerWindow,index,(C)itemConfig, (D)itemData);
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
            onDragAction?.Invoke(this, enteredSlot);

        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (onUseAction!=null&&eventData.button == PointerEventData.InputButton.Right)
        {
            onUseAction.Invoke(index, itemConfig, itemData,eventData.button);
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        RectTransform rectTransform = (RectTransform)transform;
        UIManager.Instance.ShowWindow<UI_IteminfoPopupWindow>().Show(transform.position+
            new Vector3(0,rectTransform.sizeDelta.y),itemConfig);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        UIManager.Instance.CloseWindow<UI_IteminfoPopupWindow>();
    }

    //如果鼠标正在格子中并没有离开，但是格子销毁了并不会调用onpointerExit
    private void OnDestroy()
    {
        if (enteredSlot == this)
        {
            UIManager.Instance.CloseWindow<UI_IteminfoPopupWindow>();
        }
    }
}
