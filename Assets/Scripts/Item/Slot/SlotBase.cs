using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class SlotBase : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public Image seletedImage;

    public virtual void Init()
    {
        OnPointerExit(null);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        seletedImage.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        seletedImage.gameObject.SetActive(false);
    }
}

