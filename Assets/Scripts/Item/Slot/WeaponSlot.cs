using UnityEngine.UI;

public class WeaponSlot : ItemSlotBase<WeaponConfig,WeaponData>
{
    public Image usedStateImage;

    public override void Init(int index, WeaponConfig itemConfig, WeaponData itemDate)
    {
        base.Init(index, itemConfig, itemDate);
        SetUseState(false);
    }

    public void SetUseState(bool used)
    {
        usedStateImage.gameObject.SetActive(used);
    }
}
