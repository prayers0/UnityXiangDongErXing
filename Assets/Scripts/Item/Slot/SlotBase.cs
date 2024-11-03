using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class SlotBase : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public Image seletedImage;
    public int index;
    protected static SlotBase enteredSlot;
    public UI_WindowBase ownerWindow { get; private set; }//我所才的窗口

    public virtual void Init(UI_WindowBase ownerWindow,int index)
    {
        this.ownerWindow = ownerWindow;
        SetIndex(index);
        OnPointerExit(null);
    }

    public void SetIndex(int index)
    {
        this.index = index;
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        seletedImage.gameObject.SetActive(true);
        enteredSlot = this;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        seletedImage.gameObject.SetActive(false);
        enteredSlot = null;
    }
}

