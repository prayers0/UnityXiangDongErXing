using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Config/Item/"+nameof(WeaponConfig))]
public class WeaponConfig : ItemConfigBase
{
    public float attackValue;

    public override ItemDataBase GetDefaultData()
    {
        if(defaultData==null)
        {
            defaultData = new WeaponData { id = this.name };
        }
        return defaultData;
    }
}
