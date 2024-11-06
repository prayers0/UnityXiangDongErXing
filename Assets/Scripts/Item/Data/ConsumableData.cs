using System;

[Serializable]
public class ConsumableData : ItemDataBase
{
    public int count;
    public override ItemDataBase Copy()
    {
        return new ConsumableData { id=id,count=count};
    }
}
