using System;

[Serializable]
public abstract class ItemDataBase 
{
    public string id;//对应的是配置的名字
    public abstract ItemDataBase Copy();
}
