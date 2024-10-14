using System.Collections.Generic;
using UnityEngine;

public class ResManager : GlobaManagerBase<ResManager>
{
    public List<ItemConfigBase> itemConfigList=new List<ItemConfigBase>();

    private Dictionary<string, ItemConfigBase> itemConfigDic=new Dictionary<string, ItemConfigBase>();

 
    public override void Init()
    {
        base.Init();
        InItItemConfig();
    }

    private void InItItemConfig()
    {
        //½¨Á¢Ó³Éä×Öµä
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
