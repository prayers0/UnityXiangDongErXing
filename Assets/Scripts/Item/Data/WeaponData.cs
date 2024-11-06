using System;

[Serializable]
public class WeaponData : ItemDataBase
{
    public override ItemDataBase Copy()
    {
        return new WeaponData { id = id };
    }
}
