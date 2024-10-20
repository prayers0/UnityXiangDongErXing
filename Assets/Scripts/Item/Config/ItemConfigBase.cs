using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

//��Ʒ���õĻ���
public abstract class ItemConfigBase : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    [Multiline]
    public string description;
    public GameObject slotPrefab;
    [NonSerialized]protected ItemDataBase defaultData;//����UNity���л�
    public abstract ItemDataBase GetDefaultData();
}
