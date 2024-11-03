using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsumableSlot : ItemSlotBase<ConsunableConfig, ConsumableData>
{
    public Text countText;
    public override void Init(UI_WindowBase ownerWindow, int index, ConsunableConfig itemConfig, ConsumableData itemDate)
    {
        base.Init(ownerWindow, index, itemConfig, itemDate);
        SetCount(itemData.count);
    }

    public void SetCount(int count)
    {
        countText.text=count.ToString();
    }
}
