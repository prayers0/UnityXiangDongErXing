using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class SlotBase : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public Image seletedImage;
    public int index;
    protected static SlotBase enteredSlot;

    public virtual void Init(int index)
    {
        SetIndex(index);
        OnPointerExit(null);
    }

    public void SetIndex(int index)
    {
        this.index = index;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        seletedImage.gameObject.SetActive(true);
        enteredSlot = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        seletedImage.gameObject.SetActive(false);
        enteredSlot = null;
    }
}

