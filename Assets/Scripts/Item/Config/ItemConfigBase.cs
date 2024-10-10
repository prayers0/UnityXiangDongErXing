using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

//��Ʒ���õĻ���
public class ItemConfigBase : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    [Multiline]
    public string description;
    public GameObject slotPrefab;
}
