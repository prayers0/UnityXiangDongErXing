using UnityEngine.UI;


public interface IItemSlotBase
{
    public void Init(ItemConfigBase itemConfig, ItemDataBase itemData);
}

public abstract class ItemSlotBase<C,D> : SlotBase,IItemSlotBase where C:ItemConfigBase where D:ItemDataBase
{
    public Image iconImage;
    protected C itemConfig;
    protected D itemData;

    public virtual void Init(C itemConfig,D itemDate)
    {
        this.itemConfig = itemConfig;
        this.itemData = itemDate;
        Init();
    }

    public void Init(ItemConfigBase itemConfig, ItemDataBase itemData)
    {
        Init((C)itemConfig, (D)itemData);
    }
}
