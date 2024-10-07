using UnityEngine.UI;

public class WeaponSlot : ItemSlotBase<WeaponConfig,WeaponData>
{
    public Image usedStateImage;

    public override void Init(WeaponConfig itemConfig, WeaponData itemDate)
    {
        base.Init(itemConfig, itemDate);
        SetUseState(false);
    }

    public void SetUseState(bool used)
    {
        usedStateImage.gameObject.SetActive(used);
    }
}
