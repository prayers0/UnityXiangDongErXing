using UnityEngine;
[CreateAssetMenu(menuName = "Config/Item/" + nameof(ConsunableConfig))]
public class ConsunableConfig : ItemConfigBase
{
    public float hpRegeneration;//HP»Ø¸´Á¿

    public override ItemDataBase GetDefaultData()
    {
        if (defaultData == null)
        {
            defaultData = new ConsumableData { id = this.name,count=1 };
        }
        return defaultData;
    }
}
