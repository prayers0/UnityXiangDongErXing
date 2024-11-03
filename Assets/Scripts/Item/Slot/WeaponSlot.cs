using UnityEngine.UI;

public class WeaponSlot : ItemSlotBase<WeaponConfig,WeaponData>
{
    public Image usedStateImage;

    public override void Init(UI_WindowBase ownerWindow, int index, WeaponConfig itemConfig, WeaponData itemDate)
    {
        base.Init(ownerWindow, index, itemConfig, itemDate);
        SetUseState(false);
    }

    public void SetUseState(bool used)
    {
        usedStateImage.gameObject.SetActive(used);
    }
}
