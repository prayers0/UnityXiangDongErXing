using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

//物品配置的基类
public abstract class ItemConfigBase : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public int price;
    [Multiline]
    public string description;
    public GameObject slotPrefab;
    [NonSerialized]protected ItemDataBase defaultData;//避免UNity序列化
    public abstract ItemDataBase GetDefaultData();
}
