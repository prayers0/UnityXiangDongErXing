using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

//物品配置的基类
public class ItemConfigBase : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    [Multiline]
    public string description;
    public GameObject slotPrefab;
}
