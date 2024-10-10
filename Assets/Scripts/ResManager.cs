using System.Collections.Generic;
using UnityEngine;

public class ResManager : MonoBehaviour
{
    public static ResManager Instance { get; private set; }
    public List<ItemConfigBase> itemConfigList=new List<ItemConfigBase>();

    private Dictionary<string, ItemConfigBase> itemConfigDic=new Dictionary<string, ItemConfigBase>();

    private void Awake()
    {
        Instance = this;
        InItItemConfig();

    }

    private void InItItemConfig()
    {
        //����ӳ���ֵ�
        //itemConfigList = new List<ItemConfigBase>(itemConfigList.Count);
        for(int i = 0; i < itemConfigList.Count; i++)
        {
            
            ItemConfigBase itemConfig = itemConfigList[i];
            itemConfigDic.Add(itemConfig.name, itemConfig);
        }
    }

    public ItemConfigBase GetItemConfig(string name)
    {
        return itemConfigDic[name];
    }
}
